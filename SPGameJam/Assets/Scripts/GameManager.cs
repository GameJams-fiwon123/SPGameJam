using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[] elementsPrefabs;
	public GameObject[] stardustPrefabs;
	public GameObject[] biologicalPrefabs;

	public GameObject[] earthAreaPrefabs;

	private int countObjects = 0;

	private int countPlanets = 0;
	private int countAnimals = 0;
	private bool flagSun = false;

	public GameObject starExplosion;
	public GameObject cometaExplosion;

	public Transform spawner;

	public GameObject panel;

	public GameObject sunExplosion;

	public GameObject orbits;

	public Transform universe;

	public BackgroundManager backgrounManager;

	public EarthPlanet earthPlanet;

	public StudioEventEmitter musicUniverse;


	private bool showVisits;

	public void StartGame() {
		StartAllAvaliableSpawn();
		FindObjectOfType<DialogueManager>().ShowStar();
	}

	private void Update() {
		countObjects = spawner.transform.childCount;
	}

	private Vector3 GetPositionSpawn() {
		float y = Random.Range(10f, Screen.height-10);
		float x = 0;

		int value = Random.Range(0, 2);

		switch (value) {
			case 0:
				x = 0f;
				break;
			case 1:
				x = Screen.width;
				break;
		}

		Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, y));
		newPosition.z = 0f;

		return newPosition;
	}

	IEnumerator StartSpawnElement() {
		while (true) {

			if (countObjects <= 20) {

				Vector3 newPosition = GetPositionSpawn();



				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Elemento Entra");
				GameObject obj = Instantiate(elementsPrefabs[0], newPosition, Quaternion.identity, spawner);


					if (newPosition.x < 0f) {
						obj.GetComponent<MatcherObject>().dir = Vector3.right;
					} else {
						obj.GetComponent<MatcherObject>().dir = Vector3.left;
					}
			}

			yield return new WaitForSeconds(2);
		}
	}

	IEnumerator StartSpawnStardust() {
		while (true) {
			if (countObjects <= 20) {

				Vector3 newPosition = GetPositionSpawn();

				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Poeira Entra");
				GameObject obj = Instantiate(stardustPrefabs[0], newPosition, Quaternion.identity, spawner);

				if (newPosition.x < 0f) {
					obj.GetComponent<MatcherObject>().dir = Vector3.right;
				} else {
					obj.GetComponent<MatcherObject>().dir = Vector3.left;
				}
			}

			yield return new WaitForSeconds(2);
		}
	}

	public void StartAllAvaliableSpawn() {
		StopAllSpawn();
		if (flagSun && countPlanets == 3) {
			StartCoroutine(StartSpawnElement());
			StartCoroutine(StartSpawnStardust());
		} else if (flagSun) {
			StartCoroutine(StartSpawnStardust());
		} else {
			StartCoroutine(StartSpawnElement());
		}
	}

	public void StopAllSpawn() {
		StopAllCoroutines();
	} 

	IEnumerator ShowDialoguePlanet(int index, GameObject obj) {
		panel.transform.GetChild(index).gameObject.SetActive(true);
		panel.transform.GetChild(index).GetComponent<WarpIcon>().objWarp = obj;

		if (!showVisits) {
			showVisits = true;
			FindObjectOfType<DialogueManager>().ShowVisits();
			yield return new WaitForSeconds(5);
		}

		switch (index) {
			case 0:
				FindObjectOfType<DialogueManager>().ShowHotPlanet();
				break;
			case 1:
				FindObjectOfType<DialogueManager>().ShowEarthPlanet();
				break;
			case 2:
				FindObjectOfType<DialogueManager>().ShowColdPlanet();
				break;
		}
	}

	public void ActivatePlanet(int index, GameObject obj) {
		StartCoroutine(ShowDialoguePlanet(index, obj));
	}

	public void SpawnInArea(Area.type type, Vector3 newPosition) {
		switch (type) {
			case Area.type.SKY:
				countAnimals++;
				Instantiate(earthAreaPrefabs[0], newPosition, Quaternion.identity, earthPlanet.transform);
				break;
			case Area.type.WATER:
				countAnimals++;
				Instantiate(earthAreaPrefabs[1], newPosition, Quaternion.identity, earthPlanet.transform);
				break;
			case Area.type.MOUNTAIN:
				countAnimals++;
				Instantiate(earthAreaPrefabs[2], newPosition, Quaternion.identity, earthPlanet.transform);
				break;
			case Area.type.TERRAIN:
				countAnimals++;
				Instantiate(earthAreaPrefabs[3], newPosition, Quaternion.identity, earthPlanet.transform);
				break;
		}
	}

	public void SpawnObject(MatcherObject.type type, int level, Vector3 newPosition, bool isEntering) {

		GameObject obj = null;

		switch (type) {
			case MatcherObject.type.ELEMENT:
				if (level < 5) {
					obj = Instantiate(elementsPrefabs[level], newPosition, Quaternion.identity, spawner);
				} else if (!flagSun) {
					Instantiate(sunExplosion, newPosition, Quaternion.identity, universe);

					foreach (Transform objTransform in spawner) {
						Destroy(objTransform.gameObject);
					}

					flagSun = true;
					FindObjectOfType<DialogueManager>().ShowPlanets();
					StartAllAvaliableSpawn();
					musicUniverse.SetParameter("Níveis", 1);
					backgrounManager.Next();
					return;
				} else {
					Instantiate(starExplosion, newPosition, Quaternion.identity, universe);
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cometa");
					FindObjectOfType<DialogueManager>().ShowComet();
				}
				break;
			case MatcherObject.type.STARDUST:
				if (level < 5) {
					obj = Instantiate(stardustPrefabs[level], newPosition, Quaternion.identity, spawner);
				} else if (countPlanets < 3) {
					panel.SetActive(true);
					Instantiate(stardustPrefabs[level], newPosition, Quaternion.identity, universe);
					FindObjectOfType<DialogueManager>().ShowOrbits();
					orbits.SetActive(true);
					countPlanets++;
					musicUniverse.SetParameter("Níveis", 1+ countPlanets);

					if (countPlanets == 3) {
						StartAllAvaliableSpawn();
						FindObjectOfType<DialogueManager>().ShowElementsPlanets();
					}

					backgrounManager.Next();
					return;
				} else {
					Instantiate(cometaExplosion, newPosition, Quaternion.identity, universe);
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Explosão Planetária");
				}
				break;
			case MatcherObject.type.BIOLOGICAL:
				if (level < 5) {
					obj = Instantiate(biologicalPrefabs[level], newPosition, Quaternion.identity, earthPlanet.spawner);
					obj.GetComponent<MatcherObject>().speed = 0.01f;
					obj.GetComponent<Animator>().Play("IdleEarth");
					obj.GetComponent<MatcherObject>().isEntering = true;
				} 
				break;
		}

		if (obj)
			obj.GetComponent<MatcherObject>().ChangeDirection();
	}

	public void SendPlanet(GameObject obj, int indexPlanet) {
		switch (indexPlanet) {
			case 0:
				break;
			case 1:
				obj.transform.position = Vector3.zero;
				obj.GetComponent<MatcherObject>().speed = 0.01f;
				obj.transform.parent = earthPlanet.spawner;
				break;
			case 2:
				break;
		}
	}
}
