using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
	public Camera cam;
	public bool inWarp = false;

   public void StartWarp(bool isEnter, Vector3 pos, GameObject objDest, GameObject activePanel, GameObject deactivePanel, GameObject localObject) {
		StopAllCoroutines();
		inWarp = true;
		deactivePanel.SetActive(false);
		if (isEnter) {
			StartCoroutine(Warp(isEnter, -0.1f, pos, 0.1f, objDest, activePanel, deactivePanel, localObject));
		} else {
			StartCoroutine(Warp(isEnter, 0.1f, pos, 5f, objDest, activePanel, deactivePanel, localObject ));
		}

	}

	IEnumerator Warp(bool isEnter, float speed,Vector3 pos, float destSize, GameObject objDest, GameObject activePanel, GameObject deactivePanel, GameObject localObject) {
		float distance = cam.orthographicSize - destSize;
		Vector3 positionToVerify = new Vector3(pos.x, pos.y, -10);

		while (Mathf.Abs(distance) > 0.1f) {
			Vector3 dir = positionToVerify - cam.transform.position;
			cam.transform.position += dir * 0.08f;
			cam.orthographicSize += speed;
			distance = cam.orthographicSize - destSize;
			yield return new WaitForSeconds(0.01f);

		}
		objDest.SetActive(true);
		cam.transform.position = new Vector3(0f, 0f, -10);
		inWarp = false;

		activePanel.SetActive(true);
		localObject.SetActive(false);

		if (isEnter) {
			FindObjectOfType<GameManager>().StopAllSpawn();
		} else {
			FindObjectOfType<GameManager>().StartAllAvaliableSpawn();
		}
	}
}
