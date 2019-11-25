using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[] elementsPrefabs;

	public Animator sunAnim;

	private int countObjects = 0;

	private bool flagSun = false;

	public GameObject starExplosion;
	public GameObject cometaExplosion;

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

				Instantiate(elementsPrefabs[0], newPosition, Quaternion.identity);
				countObjects++;
			}

			yield return new WaitForSeconds(2);
		}
	}

	public void SpawnObject(MatcherObject.type type, int level, Vector3 newPosition) {
		countObjects -= 2;
		switch (type) { 
			case MatcherObject.type.ELEMENT:
				if (level < 5) {
					Instantiate(elementsPrefabs[level], newPosition, Quaternion.identity);
				} else if (!flagSun){
					Instantiate(elementsPrefabs[level], Vector3.zero, Quaternion.identity);
					flagSun = true;
				} else {
					Instantiate(starExplosion, newPosition, Quaternion.identity);
				}
				break;
		}
	}
}
