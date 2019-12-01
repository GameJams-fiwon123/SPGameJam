﻿using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPlanet : MonoBehaviour
{
	public Transform sky;

	public GameObject lightningPrefab;
	public Transform spawner;

	public BackgroundManager backgrounManager;

	public StudioEventEmitter musicEarth;

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
				if (spawner.Find("Element1")) {
					int index = Random.Range(0, sky.childCount);
					Vector3 newPosition = sky.transform.GetChild(index).position;

					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Raio");
					GameObject obj = Instantiate(lightningPrefab, newPosition, Quaternion.identity, spawner);
					obj.GetComponent<MatcherObject>().dir = Vector3.down;
					//obj.GetComponent<MatcherObject>().speed = 0.05f;
				}
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
