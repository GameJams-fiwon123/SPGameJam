using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
	public Animator anim;

	public void Show() {
		anim.SetBool("IsShow", true);
	}
}
