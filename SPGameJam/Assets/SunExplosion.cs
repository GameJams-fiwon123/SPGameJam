using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunExplosion : MonoBehaviour
{

	public GameObject sunPrefab;
	public Animator anim;

	// Start is called before the first frame update
	void Start() {
		StartCoroutine(Move());
	}

	IEnumerator Move() {
		float dist = Vector3.Distance(transform.position, Vector3.zero);
		while (dist > 0.1f) {
			Vector3 dir = Vector3.zero - transform.position;
			transform.position += dir * 2 * Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
			dist = Vector3.Distance(transform.position, Vector3.zero);
			Debug.Log(dist);

		}
		anim.Play("Explosion");
	 Instantiate(sunPrefab, Vector3.zero, Quaternion.identity);

	}

	public void FinishAnimation() {
		Destroy(gameObject);
	}
}
