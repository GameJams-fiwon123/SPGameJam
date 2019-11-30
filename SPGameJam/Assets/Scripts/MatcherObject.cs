using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatcherObject : MonoBehaviour {
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

	private void Update() {
		if (dir != Vector3.zero) {
			Move();
			HoldObject();
		}

		currentTime += Time.deltaTime;
		if (currentTime >= waitTime) {
			currentTime = waitTime;
		}

		if (isEntering && id != type.BIOLOGICAL) {
			GetComponent<Animator>().Play("IdleEarth");
		}
	}

	private void Move() {
		transform.position += dir * speed * Time.deltaTime;
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
		if (collision.tag == "Wall") {
			Debug.Log(gameObject.name);
			if ((level > 0 || isEntering) && currentTime >= waitTime) {
				float x = Random.Range(dir.x - 0.1f, dir.x + 0.1f);
				float y = Random.Range(dir.y - 0.1f, dir.y - 0.1f);
				dir = new Vector3(x, y);
				dir = -dir;
				dir = dir.normalized;
				currentTime = 0f;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Interact") {
			MatcherObject other = collision.GetComponent<MatcherObject>();

			if (other.id == id && other.level == level && isHolding && !isEntering) {
				FindObjectOfType<GameManager>().SpawnObject(id, level + 1, transform.position, isEntering);
				Instantiate(combineExplosion, transform.position, Quaternion.identity);
				switch (id) {
					case type.ELEMENT:
						FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Combinação Elementos");
						break;
					case type.STARDUST:
						FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Combinação Poeiras");
						break;
				}

				Destroy(collision.gameObject);
				Destroy(gameObject);

			} else if ((id == type.ELEMENT && other.id == type.BIOLOGICAL) || (other.id == type.ELEMENT && id == type.BIOLOGICAL) && isHolding && level == other.level) {
				FindObjectOfType<GameManager>().SpawnObject(other.id, level + 1, transform.position, isEntering);
				Instantiate(combineExplosion, transform.position, Quaternion.identity, transform.parent);
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Meleca (Mistura)");

				Destroy(collision.gameObject);
				Destroy(gameObject);
			} else if (other.id == id && other.level == level && isHolding && isEntering) {
				FindObjectOfType<GameManager>().SpawnObject(id, level + 1, transform.position, isEntering);
				Instantiate(combineExplosion, transform.position, Quaternion.identity);

				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Meleca (Mistura)");

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

	public void Entering() {
		isEntering = true;
		FindObjectOfType<GameManager>().SendPlanet(gameObject, orbitIndex);
	}

}
