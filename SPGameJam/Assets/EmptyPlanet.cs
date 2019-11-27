using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPlanet : MonoBehaviour
{
	public float RotateSpeed = 1f;
	public float Radius = 1.2f;
	public SpriteRenderer sprRenderer;

	private Vector2 _centre;
	private float _angle;

	public bool isHolding = false;

	bool isOrbit = false;

	private void Start() {
		_centre = Vector3.zero;
	}

	// Update is called once per frame
	void Update() {
		if (!isOrbit) {
			HoldObject();
		} else {
			_angle += RotateSpeed * Time.deltaTime;

			var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
			transform.position = _centre + offset;
		}
	}

	private void HoldObject() {
		if (Input.GetMouseButton(0) && isHolding) {
			float x = Mathf.Clamp(Input.mousePosition.x, 10f, Screen.width - 10f);
			float y = Mathf.Clamp(Input.mousePosition.y, 10f, Screen.height - 10f);
			Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(x, y));
			newPosition.z = 0;
			transform.position = newPosition;
		}
	}

	public void StartOrbit() {
		isOrbit = true;
	}

	private void OnMouseDown() {
		isHolding = true;
	}

	private void OnMouseUp() {
		isHolding = false;
	}
}
