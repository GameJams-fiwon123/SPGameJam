using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatcherObject : MonoBehaviour
{
	[Range(0, 4)]
	public int level = 0;

	public enum type { ELEMENT, STARDUST };
	public type id;

	public GameObject combineExplosion;

	bool isHolding = false;

	private void Update() {
		if (Input.GetMouseButton(0) && isHolding) {
			Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			newPosition.z = 0;
			transform.position = newPosition;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Interact") {
			MatcherObject other = collision.GetComponent<MatcherObject>();

			if (other.id == id && other.level == level && isHolding) {
				FindObjectOfType<GameManager>().SpawnObject(id, level+1, transform.position);
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
			}
		}

	}

	private void OnMouseDown() {
		isHolding = true;
	}

	private void OnMouseUp() {
		isHolding = false;
	}
}
