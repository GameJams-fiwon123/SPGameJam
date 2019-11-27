using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
	public float RotateSpeed = 1f;
	public float Radius = 1.2f;

	public Sprite sprite;

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.tag == "EmptyPlanet") {
			if (Input.GetMouseButtonUp(0)) {
				collision.GetComponent<EmptyPlanet>().StartOrbit();
				collision.GetComponent<EmptyPlanet>().RotateSpeed = RotateSpeed;
				collision.GetComponent<EmptyPlanet>().Radius = Radius;
				collision.GetComponent<EmptyPlanet>().sprRenderer.sprite = sprite;
			}
		}
	}

}
