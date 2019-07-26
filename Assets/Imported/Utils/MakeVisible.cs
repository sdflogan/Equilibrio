using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVisible : MonoBehaviour {
	public GameObject puerta;
	private BoxCollider _collider;
	private Renderer _renderer;
	private bool _visible = false;

	// Use this for initialization
	void Start () {
		_collider = puerta.GetComponent<BoxCollider>();
		_renderer = puerta.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	bool CheckIfVisible() {
		return IsVisibleFrom(_renderer, Camera.main);
	}

	bool IsVisibleFrom(Renderer renderer, Camera camera) {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}

	void MakeVisibleObject() {
		_collider.enabled = true;
		_renderer.enabled = true;
		_visible = true;
	}

	private void Buu() {
		Destroy(this);
	}

	private void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			bool isVisible = CheckIfVisible();

			if (!_visible && !isVisible)
				MakeVisibleObject();
			else if (_visible && isVisible)
				Buu();
		}	
	}

	/*
	////////////////////////////////////////////////////////////////////////////////////////////
	void opcionAObsoleta() {
		Vector3 screenPoint = playerCamera.WorldToViewportPoint(targetPoint.position);
 		bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

		if (!onScreen)
			Debug.Log("Fuera de EN PANTALLA");
		else
			Debug.Log("EN PANTALLA");
	}
	*/
}
