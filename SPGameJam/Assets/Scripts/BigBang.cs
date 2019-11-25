using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBang : MonoBehaviour
{
	public Animator anim;

	private void OnMouseDown() {
		anim.Play("Explosion");
	}

	public void FinishAnimation() {
		FindObjectOfType<BackgroundManager>().Next();
		FindObjectOfType<GameManager>().StartGame();
		Destroy(gameObject);
	}
}
