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
		FindObjectOfType<WarpManager>().StartWarp(true, objWarp.transform.position, objDest, activePanel, deactivePanel, LocalObject);
	}

	public void ZoomOut() {
		FindObjectOfType<WarpManager>().StartWarp(false, Vector3.zero, objDest, activePanel, deactivePanel, LocalObject);
	}
}
