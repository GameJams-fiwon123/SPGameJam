using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[] ObjectsPrefabs;
	public Animator sunAnim;

	public void StartGame() {
		StartCoroutine(StartSpawn());
	}

	IEnumerator StartSpawn() {
		while (true) {
			yield return new WaitForSeconds(1000);

			//Instantiate(ObjectsPrefabs[0], ,,)
		}
	}
}
