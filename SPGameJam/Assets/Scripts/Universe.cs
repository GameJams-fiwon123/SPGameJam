using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
	public StudioEventEmitter musicUniverse;
	private int nivel = 0;

	private void OnEnable() {
		ChangeNivel(nivel);
	}

	public void NextNivel() {
		nivel += 1;
		ChangeNivel(nivel);
	}

	private void ChangeNivel(int nivel) {
		musicUniverse.SetParameter("Níveis", nivel);
	}
}
