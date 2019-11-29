using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlanet : MonoBehaviour
{
	public Transform sky;

	public GameObject lightningPrefab;
	public Transform spawner;

	private int countObjects = 0;

	private void Update() {
		countObjects = spawner.transform.childCount;
	}

	public void StartSpawn() {
		StartCoroutine(StartSpawnLightning());
	}

	public void StopSpawn() {
		StopAllCoroutines();
	}

	IEnumerator StartSpawnLightning() {
		while (true) {
			if (countObjects <= 20) {
				int index = Random.Range(0, sky.childCount);
				Vector3 newPosition = sky.transform.GetChild(index).position;

				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Raio");
				yield return new WaitForSeconds(1);
				GameObject obj = Instantiate(lightningPrefab, newPosition, Quaternion.identity, spawner);
				obj.GetComponent<MatcherObject>().dir = Vector3.down;
				obj.GetComponent<MatcherObject>().speed = 0.05f;
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Queda");
			}

			yield return new WaitForSeconds(2);
		}
	}

	private void OnEnable() {
		StartSpawn();
	}

	private void OnDisable() {
		StopSpawn();
	}
}
