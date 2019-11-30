using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	public Vector3 dir = Vector3.zero;
	public float speed = 1f;

	private float currentTime = 0f;
	private float waitTime = 0.1f;

	public bool isEntering = true;

	private void Start() {
		ChangeDirection();
	}

	private void Update() {
		if (dir != Vector3.zero) {
			Move();
		}

		currentTime += Time.deltaTime;
		if (currentTime >= waitTime) {
			currentTime = waitTime;
		}

	}

	private void Move() {
		transform.position += dir * speed * Time.deltaTime;
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.tag == "Wall") {
			if (currentTime >= waitTime) {
				float x = Random.Range(dir.x - 0.1f, dir.x + 0.1f);
				float y = Random.Range(dir.y - 0.1f, dir.y - 0.1f);
				dir = new Vector3(x, y);
				dir = -dir;
				dir = dir.normalized;
				currentTime = 0f;
			}
		}
	}

	public void ChangeDirection() {
		float x = Random.Range(-1f, 1f);
		float y = Random.Range(-1f, 1f);

		dir.Set(x, y, 0f);
		dir = dir.normalized;
	}
}
