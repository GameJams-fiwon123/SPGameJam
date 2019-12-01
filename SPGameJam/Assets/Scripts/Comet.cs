using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
	private Vector3 dir = new Vector3(-1.5f, -1f);
	private float speed = 3f;

	private void Start() {
		StartCoroutine(StartMove());
	}

	IEnumerator StartMove() {
		while (true) {
			transform.position += dir * speed * Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
	}

	private void OnBecameInvisible() {
		Destroy(gameObject);
	}

}
