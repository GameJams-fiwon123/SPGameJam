using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	public Animator dialogueVisits;
	public Animator dialogueComet;
	public Animator dialogueElements;
	public Animator dialogueStar;
	public Animator dialogueColdPlanet;
	public Animator dialogueOrbits;
	public Animator dialogueEarthPlanet;
	public Animator dialoguePlanets;
	public Animator dialogueHotPlanet;
	public Animator dialogueLife;
	public Animator dialogueContratulations;


	public void ShowStar() {
		dialogueStar.Play("Show");
	}

	public void ShowPlanets() {
		dialoguePlanets.Play("Show");
	}

	public void ShowColdPlanet() {
		dialogueColdPlanet.Play("Show");
	}

	public void ShowHotPlanet() {
		dialogueHotPlanet.Play("Show");
	}

	public void ShowEarthPlanet() {
		dialogueEarthPlanet.Play("Show");
	}

	public void ShowOrbits() {
		dialogueOrbits.Play("Show");
	}

	public void ShowVisits() {
		dialogueVisits.Play("Show");
	}

	public void ShowElementsPlanets() {
		Debug.Log("Entrou");
		dialogueElements.Play("Show");
	}

	public void ShowComet() {
		dialogueComet.Play("Show");
	}

	public void ShowLife() {
		dialogueLife.Play("Show");
	}

	public void ShowCongratulations() {
		dialogueContratulations.Play("Show");
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Parabéns!");
	}
}
