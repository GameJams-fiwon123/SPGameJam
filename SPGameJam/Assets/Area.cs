using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
	public enum type { SKY, WATER, MOUNTAIN, TERRAIN };
	public type id;

	private bool isHolding;

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.tag == "Interact") {
			if (!Input.GetMouseButton(0) && isHolding) {
				if (collision.GetComponent<MatcherObject>().id == MatcherObject.type.BIOLOGICAL &&
					collision.GetComponent<MatcherObject>().level == 4) {
					FindObjectOfType<GameManager>().SpawnInArea(id, gameObject.transform.position);
					gameObject.SetActive(false);
				}

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