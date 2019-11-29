using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBang : MonoBehaviour
{
	public Animator anim;
	public BackgroundManager backgroundManager;

	private void OnMouseDown() {
		anim.Play("Explosion");
	}

	public void ExplosionEffect() {
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Explosão Estrelar");
	}

	public void NextAnimation() {
		backgroundManager.Next();
	}

	public void FinishAnimation() {
		FindObjectOfType<GameManager>().StartGame();
		Destroy(gameObject);
	}
}
