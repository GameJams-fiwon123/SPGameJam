using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
	public float RotateSpeed = 1f;
	public float Radius = 1.2f;
	public SpriteRenderer sprRenderer;

	private Vector2 _centre;
	private float _angle;

	public bool isHolding = false;

	public bool isOrbit = false;
	private int orbitIndex;

	private void Start() {
		_centre = Vector3.zero;
	}

	// Update is called once per frame
	void Update() {
		if (!isOrbit) {
			HoldObject();
		} else if (!FindObjectOfType<WarpManager>().inWarp) {
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

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("Orbit")) {
			if (!Input.GetMouseButton(0) && isHolding) {
				StartOrbit();
				RotateSpeed = collision.GetComponent<Orbit>().RotateSpeed;
				Radius = collision.GetComponent<Orbit>().Radius;
				sprRenderer.sprite = collision.GetComponent<Orbit>().sprite;
				collision.gameObject.layer = 13;  // Nothing
				collision.GetComponent<Orbit>().sprRenderer.color = new Color(1f, 1f, 1f, 0.5f);
				orbitIndex = collision.GetComponent<Orbit>().index;
				FindObjectOfType<GameManager>().ActivatePlanet(orbitIndex, gameObject);
			}
		} else if (collision.CompareTag("Interact") && isOrbit && orbitIndex == 1) { //Earth Only
			if (collision.GetComponent<MatcherObject>().id == MatcherObject.type.ELEMENT &&
				collision.GetComponent<MatcherObject>().level == 0 && 
				collision.GetComponent<MatcherObject>().isHolding) {
				collision.transform.position = gameObject.transform.position;
				collision.GetComponent<MatcherObject>().speed = 0f;
				collision.GetComponent<MatcherObject>().isHolding = false;
				collision.GetComponent<MatcherObject>().orbitIndex = orbitIndex;
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Queda");
				collision.gameObject.GetComponent<Animator>().SetBool("IsEntering", true);
			}
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
