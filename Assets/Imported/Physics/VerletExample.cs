using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VerletExample : MonoBehaviour
{

	public bool isPlayerController = false;
	public float acceleration = 1;
	public float frictionFactor = 1;
	public float radius = 8.5f;
	public Vector3 center = Vector3.zero;
	public Transform target;
	public Vector3 gravity;
	public float targetRadius=1;

	public List<KeyMapping> keyMappings;

	Vector3 previousPosition;
	Vector3 currentAcceleration;

	void Start ()
	{
		keyMappings = new List<KeyMapping>();
		if (isPlayerController)
		{
			keyMappings.Add(new KeyMapping() { key = KeyCode.UpArrow, action = () => Accelerate(Vector3.up), actionType = KeyActionType.OnKey });
			keyMappings.Add(new KeyMapping() { key = KeyCode.DownArrow, action = () => Accelerate(Vector3.down), actionType = KeyActionType.OnKey });
			keyMappings.Add(new KeyMapping() { key = KeyCode.LeftArrow, action = () => Accelerate(Vector3.left), actionType = KeyActionType.OnKey });
			keyMappings.Add(new KeyMapping() { key = KeyCode.RightArrow, action = () => Accelerate(Vector3.right), actionType = KeyActionType.OnKey });
		}
		previousPosition = transform.position;
	}

	private void Accelerate(Vector3 direction)
	{
		currentAcceleration += direction * acceleration;
	}

	void Update ()
	{
		if (Application.isPlaying)
		{
			ResetForces();
			ProcessInputForces();
			ProcessGravityForces();
			ProcessFrictionForces();
			ApplicateConstraints();
			ApplicateVerletIntegration();
		}
	}

	private void ProcessGravityForces()
	{
		currentAcceleration += gravity;
	}

	private void ApplicateConstraints()
	{
		if (radius != 0)
		{
			var delta = transform.position - center;
			bool isInsideCircle = delta.magnitude <= radius;
			if (!isInsideCircle)
			{
				var perfectPosition = center + delta.normalized * radius;
				var error = perfectPosition - transform.position;
				transform.position = transform.position + error;
			}
		}

		if (target != null)
		{
			var delta = transform.position - target.transform.position;
			bool isInsideCircle = delta.magnitude <= targetRadius;
			if (!isInsideCircle)
			{
				var perfectPosition = target.transform.position + delta.normalized * targetRadius;
				var error = perfectPosition - transform.position;
				transform.position = transform.position + error * 0.5f;
				target.transform.position = target.transform.position - error * 0.5f;
			}
		}
	}

	private void ProcessFrictionForces()
	{
		//currentAcceleration -= (transform.position - previousPosition) * frictionFactor;
	}

	private void ApplicateVerletIntegration()
	{
		var newPosition = transform.position + (transform.position - previousPosition) + currentAcceleration * Time.deltaTime * Time.deltaTime;
		previousPosition = transform.position;
		transform.position = newPosition;
	}

	private void ProcessInputForces()
	{
		foreach (var keyMapping in keyMappings)
		{
			if (Input.GetKey(keyMapping.key)) { keyMapping.action.Invoke(); }
		}
	}

	private void ResetForces()
	{
		currentAcceleration = Vector3.zero;
	}

	private void OnDrawGizmos()
	{
		if (target != null)
		{
			Gizmos.DrawLine(transform.position, target.transform.position);
		}
		Gizmos.DrawWireSphere(center, radius);

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position + (transform.position - previousPosition));
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + currentAcceleration);
	}

}
