using FMODUnity;
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


	public int countSky = 0;
	public int countWater = 0;
	public int countMountain = 0;
	public int countTerrain = 0;
	public int countLife = 0;

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

	public void NextSky(GameObject areaObject) {
		countSky++;

		if (countSky == 3) {
			musicEarth.SetParameter("Céu", 1);
			Destroy(areaObject);
		}
	}

	public void NextWater(GameObject areaObject) {
		countWater++;

		if (countWater == 3) {
			musicEarth.SetParameter("Mar", 1);
			Destroy(areaObject);
		}
	}

	public void NextMountain(GameObject areaObject) {
		countMountain++;

		if (countMountain == 3) {
			musicEarth.SetParameter("Montanha", 1);
			Destroy(areaObject);
		}
	}

	public void NextTerrain(GameObject areaObject) {
		countTerrain++;

		if (countTerrain == 6) {
			musicEarth.SetParameter("Terra", 1);
			Destroy(areaObject);
		}
	}

	public void NextLife() {
		countLife++;

		if (countLife == 15) {
			FindObjectOfType<DialogueManager>().ShowCongratulations();
		}
	}

	private void OnEnable() {
		if (countSky == 3) {
			musicEarth.SetParameter("Céu", 1);
		}
		if (countWater == 3) {
			musicEarth.SetParameter("Mar", 1);
		}
		if (countMountain == 3) {
			musicEarth.SetParameter("Montanha", 1);
		}
		if (countTerrain == 3) {
			musicEarth.SetParameter("Terra", 1);
		}

		StartSpawn();
	}

	private void OnDisable() {
		StopSpawn();
	}
}
