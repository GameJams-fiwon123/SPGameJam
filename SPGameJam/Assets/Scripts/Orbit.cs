using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
	[Range(0,2)]
	public int index;
	public float RotateSpeed = 1f;
	public float Radius = 1.2f;

	public SpriteRenderer sprRenderer;

	public Sprite sprite;

}
