using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpIcon : MonoBehaviour
{
	public GameObject LocalObject;

	public GameObject objWarp;
	public GameObject objDest;

	public GameObject activePanel;
	public GameObject deactivePanel;

	public void ZoomIn() {
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Zoom In");
		FindObjectOfType<WarpManager>().StartWarp(true, objWarp.transform.position, objDest, activePanel, deactivePanel, LocalObject);
	}

	public void ZoomOut() {
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Zoom Out");
		FindObjectOfType<WarpManager>().StartWarp(false, Vector3.zero, objDest, activePanel, deactivePanel, LocalObject);
	}
}
