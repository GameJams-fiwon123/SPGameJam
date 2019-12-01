using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatcherObject : MonoBehaviour
{
	[Range(0, 4)]
	public int level = 0;

	public enum type { ELEMENT, STARDUST, BIOLOGICAL };
	public type id;

	public GameObject combineExplosion;

	public bool isHolding = false;

	public Vector3 dir = Vector3.zero;
	public float speed = 1f;

	private float currentTime = 0f;
	private float waitTime = 0.1f;

	public int orbitIndex;

	public bool isEntering;

	public GameObject planet = null;

	private void Update() {
		if (dir != Vector3.zero) {
			Move();
			HoldObject();
		}

		currentTime += Time.deltaTime;
		if (currentTime >= waitTime) {
			currentTime = waitTime;
		}

	}

	private void Move() {
		if (!planet) {
			transform.position += dir * speed * Time.deltaTime;
		} else {
			dir = planet.transform.position - transform.position;
			transform.position += dir * speed * Time.deltaTime;
		}
	}

	private void HoldObject() {
		if (Input.GetMouseButton(0) && isHolding) {
			float x = Mathf.Clamp(Input.mousePosition.x, 10f, Screen.width - 10f);
			float y = Mathf.Clamp(Input.mousePosition.y, 10f, Screen.height - 10f);
			Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, y));
			newPosition.z = 0;
			transform.position = newPosition;
		}
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("Wall")) {
			if ((level > 0 || isEntering) && currentTime >= waitTime) {
				float x = Random.Range(dir.x - 0.1f, dir.x + 0.1f);
				float y = Random.Range(dir.y - 0.1f, dir.y - 0.1f);
				dir = new Vector3(x, y);
				dir = -dir;
				dir = dir.normalized;
				currentTime = 0f;
			} 
		} else if (collision.CompareTag( "Area")) {
			if (!Input.GetMouseButton(0) && isHolding) {
				if (id == type.BIOLOGICAL && level == 4) {
					FindObjectOfType<GameManager>().SpawnInArea(collision.GetComponent<Area>().id, collision.transform.position, collision.gameObject);
					Destroy(gameObject);
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Interact")) {
			MatcherObject other = collision.GetComponent<MatcherObject>();


			if (((id == type.ELEMENT && other.id == type.BIOLOGICAL) || (other.id == type.ELEMENT && id == type.BIOLOGICAL)) && isHolding && level == 0 && other.level == 0) {
				FindObjectOfType<GameManager>().SpawnObject(type.BIOLOGICAL, level + 1, transform.position);
				Instantiate(combineExplosion, transform.position, Quaternion.identity, transform.parent);
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Meleca (Mistura)");

				FindObjectOfType<DialogueManager>().ShowLife();

				Destroy(collision.gameObject);
				Destroy(gameObject);
			} else if (other.id == id && other.level == level && isHolding && !isEntering) {

				if (id == type.BIOLOGICAL && level == 0) {
					return;
				} else if (id == type.BIOLOGICAL && level == 4) {
					return;
				} else if (id == type.ELEMENT && FindObjectOfType<GameManager>().countPlanets == 3) {
					return;
				} else if (id == type.STARDUST && FindObjectOfType<GameManager>().countPlanets == 3) {
					FindObjectOfType<GameManager>().SpawnObject(id, level + 1, transform.position);
				} else {
					FindObjectOfType<GameManager>().SpawnObject(id, level + 1, transform.position);
					Instantiate(combineExplosion, transform.position, Quaternion.identity);
				}

				switch (id) {
					case type.ELEMENT:
						FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Combinação Elementos");
						break;
					case type.STARDUST:
						FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Combinação Poeiras");
						break;
					case type.BIOLOGICAL:
						FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Meleca (Mistura)");
						break;
				}

				Destroy(collision.gameObject);
				Destroy(gameObject);

			}

		}

	}

	public void ChangeDirection() {
		float x = Random.Range(-1f, 1f);
		float y = Random.Range(-1f, 1f);

		dir.Set(x, y, 0f);
		dir = dir.normalized;
	}

	private void OnMouseDown() {
		isHolding = true;
	}

	private void OnMouseUp() {
		ChangeDirection();

		isHolding = false;
	}

	private void OnBecameInvisible() {
		if (level == 0 && !isEntering)
			Destroy(gameObject);
	}

	private void OnBecameVisible() {
		gameObject.layer = 10;
		planet = null;
	}

	public void StartEntering() {
		gameObject.layer = 13;
	}

	public void Entering() {
		isEntering = true;
		FindObjectOfType<GameManager>().SendPlanet(gameObject, orbitIndex);
	}

}
