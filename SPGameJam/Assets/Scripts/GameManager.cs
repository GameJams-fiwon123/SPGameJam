using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[] elementsPrefabs;
	public GameObject[] stardustPrefabs;
	public GameObject[] biologicalPrefabs;

	[Tooltip("Sky, Water, Mountain, Terrain")]
	public GameObject[] earthAreaPrefabs;

	private int countObjects = 0;

	public int countPlanets = 0;
	public int countInOrbit = 0;

	//private int countSky = 0;
	//private int countWater = 0;
	//private int countMountain = 0;
	//private int countTerrain = 0;
	//private int countLife = 0;

	private bool flagSun = false;

	public GameObject starExplosion;
	public GameObject cometaExplosion;

	public Transform spawner;

	public GameObject panel;

	public GameObject sunExplosion;

	public GameObject orbits;

	//public Transform universe;

	public BackgroundManager backgrounManager;

	public EarthPlanet earthPlanet;

	public Universe universe;


	private bool showVisits;

	private void Start() {
		universe.gameObject.SetActive(true);
	}

	public void StartGame() {
		StartAllAvaliableSpawn();
		FindObjectOfType<DialogueManager>().ShowStar();
	}

	private void Update() {
		countObjects = spawner.transform.childCount;
	}

	private Vector3 GetPositionSpawn() {
		float y = Random.Range(10f, Screen.height - 10);
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
				obj.name = "Element1";

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


	public void ActivatePlanet(int index, GameObject obj) {

		panel.transform.GetChild(index).gameObject.SetActive(true);
		panel.transform.GetChild(index).GetComponent<WarpIcon>().objWarp = obj;

		if (!showVisits) {
			showVisits = true;
			FindObjectOfType<DialogueManager>().ShowVisits();
		}

		universe.NextNivel();
		backgrounManager.Next();

		countInOrbit++;

		if (countInOrbit == 3) {
			StartAllAvaliableSpawn();
			FindObjectOfType<DialogueManager>().ShowElementsPlanets();

			foreach (Transform objTransform in spawner) {
				Destroy(objTransform.gameObject);
			}
		}
	}

	public void SpawnInArea(Area.type type, Vector3 newPosition, GameObject areaObject) {
		GameObject obj = null;
		Vector3 refPos = areaObject.transform.position;

		switch (type) {
			case Area.type.SKY:
				earthPlanet.NextSky(areaObject);


				obj = Instantiate(earthAreaPrefabs[0], newPosition, Quaternion.identity, earthPlanet.transform);
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Pássaro - Céu");
				break;
			case Area.type.WATER:

				earthPlanet.NextWater(areaObject);

				obj = Instantiate(earthAreaPrefabs[1], newPosition, Quaternion.identity, earthPlanet.transform);
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Água - Peixe");
				break;
			case Area.type.MOUNTAIN:

				earthPlanet.NextMountain(areaObject);

				obj = Instantiate(earthAreaPrefabs[2], newPosition, Quaternion.identity, earthPlanet.transform);
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Roedor - Montanha");
				break;
			case Area.type.TERRAIN:

				earthPlanet.NextTerrain(areaObject);

				if (earthPlanet.countTerrain > 3) {
					obj = Instantiate(earthAreaPrefabs[3], newPosition, Quaternion.identity, earthPlanet.transform);
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Réptil - Terra");
				} else {
					earthPlanet.backgrounManager.Next();
				}

				break;
		}

		if (obj)
			obj.GetComponent<Animal>().centerPosition = refPos;

		earthPlanet.NextLife();
	}

	public void SpawnObject(MatcherObject.type type, int level, Vector3 newPosition) {

		GameObject obj = null;

		switch (type) {
			case MatcherObject.type.ELEMENT:
				if (level < 5) {
					obj = Instantiate(elementsPrefabs[level], newPosition, Quaternion.identity, spawner);
				} else if (!flagSun) {
					Instantiate(sunExplosion, newPosition, Quaternion.identity, universe.transform);

					foreach (Transform objTransform in spawner) {
						Destroy(objTransform.gameObject);
					}

					flagSun = true;
					FindObjectOfType<DialogueManager>().ShowPlanets();
					StartAllAvaliableSpawn();
					universe.NextNivel();
					backgrounManager.Next();
					return;
				} else {
					Instantiate(starExplosion, newPosition, Quaternion.identity, universe.transform);
				}
				break;
			case MatcherObject.type.STARDUST:
				if (level < 5 && countPlanets < 3) {
					obj = Instantiate(stardustPrefabs[level], newPosition, Quaternion.identity, spawner);
				} else if (countPlanets < 3) {
					panel.SetActive(true);
					Instantiate(stardustPrefabs[level], newPosition, Quaternion.identity, universe.transform);
					FindObjectOfType<DialogueManager>().ShowOrbits();
					orbits.SetActive(true);
					countPlanets++;
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Explosão Planetária");

					return;
				} else {
					Instantiate(cometaExplosion, newPosition, Quaternion.identity, universe.transform);
					FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cometa");
					FindObjectOfType<DialogueManager>().ShowComet();
				}
				break;
			case MatcherObject.type.BIOLOGICAL:
				if (level < 5) {
					obj = Instantiate(biologicalPrefabs[level], newPosition, Quaternion.identity, earthPlanet.spawner);
					obj.GetComponent<MatcherObject>().speed = 1f;

					if (level == 4) {
						FindObjectOfType<DialogueManager>().ShowSpecie();
					}
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
				obj.transform.position = new Vector3(50f, 0f, 0f);
				obj.GetComponent<MatcherObject>().speed = 1f;
				obj.transform.parent = earthPlanet.spawner;
				break;
			case 2:
				break;
		}
	}
}
