using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileReflection : MonoBehaviour {
	public float speed = 10;
	
	Vector3 dir;
	Vector3 pos;

	public LayerMask collisionMask;
	//private ParticulasPool poolParts;
	private AudioSource _audio;

	// Use this for initialization
	void Start () {
		//poolParts = GetComponent<ParticulasPool>();
		_audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Move();
	}

	public void Move() {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);

		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + .1f, collisionMask)) { 
			Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
			float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0, rot, 0);
			//poolParts.GetParticula(transform);
			_audio.Play ();
		}
	}

	
}
