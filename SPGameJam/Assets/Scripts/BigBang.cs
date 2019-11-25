﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBang : MonoBehaviour
{
	public Animator anim;

	private void OnMouseDown() {
		anim.Play("Explosion");
	}

	public void ExplosionEffect() {
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Explosão Estrelar");
	}

	public void NextAnimation() {
		FindObjectOfType<BackgroundManager>().Next();
	}

	public void FinishAnimation() {
		FindObjectOfType<GameManager>().StartGame();
		Destroy(gameObject);
	}
}
