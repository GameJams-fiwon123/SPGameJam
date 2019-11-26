using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[] elementsPrefabs;
	public GameObject[] stardustPrefabs;

	private int countObjects = 0;

	private int countPlanets = 0;
	private bool flagSun = false;

	public GameObject starExplosion;
	public GameObject cometaExplosion;

	public Transform spawner;

	public GameObject panel;

	public void StartGame() {
		StartCoroutine(StartSpawn());
	}

	IEnumerator StartSpawn() {
		while (true) {

			if (countObjects < 10) {
				float x = Random.Range(0f, Screen.width);
				float y = Random.Range(0f, Screen.height);

				Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, y));
				newPosition.z = 0f;

				if (!flagSun) {
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Elemento Entra");
					Instantiate(elementsPrefabs[0], newPosition, Quaternion.identity, spawner);
				} else {
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Poeira Entra");
					Instantiate(stardustPrefabs[0], newPosition, Quaternion.identity, spawner);
				}

				countObjects++;
			}

			yield return new WaitForSeconds(2);
		}
	}

	public void SpawnObject(MatcherObject.type type, int level, Vector3 newPosition) {
		countObjects--;
		switch (type) {
			case MatcherObject.type.ELEMENT:
				if (level < 5) {
					Instantiate(elementsPrefabs[level], newPosition, Quaternion.identity);
				} else if (!flagSun) {
					Instantiate(elementsPrefabs[level], Vector3.zero, Quaternion.identity);
					countObjects--;
					foreach (Transform child in spawner) {
						Destroy(child.gameObject);
					}
					flagSun = true;
				} else {
					Instantiate(starExplosion, newPosition, Quaternion.identity);
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cometa");
					countObjects--;
				}
				break;
			case MatcherObject.type.STARDUST:
				if (level < 5) {
					Instantiate(stardustPrefabs[level], newPosition, Quaternion.identity);
				} else if (countPlanets < 3) {
					panel.SetActive(true);
					panel.transform.GetChild(countPlanets).gameObject.SetActive(true);
					countPlanets++;
					FindObjectOfType<BackgroundManager>().Next();
					countObjects--;
				} else {
					Instantiate(cometaExplosion, newPosition, Quaternion.identity);
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Explosão Planetária");
					countObjects--;
				}
				break;
		}
	}
}
