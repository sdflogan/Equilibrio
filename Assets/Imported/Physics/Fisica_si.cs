using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PhysicsExamples : MonoBehaviour
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

	Vector3 currentSpeed;
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
			ApplicateEulerIntegration();
		}
		ApplicateConstraints();
	}

	private void ProcessGravityForces()
	{
		currentAcceleration += gravity;
	}

	private void ApplicateConstraints()
	{
		if (transform.position.x > 8)
		{
			transform.position = new Vector3(8, transform.position.y, transform.position.z);
			currentSpeed = new Vector3(0, currentSpeed.y, currentSpeed.z);
		}
		if (transform.position.x < -8)
		{
			transform.position = new Vector3(-8, transform.position.y, transform.position.z);
			currentSpeed = new Vector3(0, currentSpeed.y, currentSpeed.z);
		}

		if (radius != 0)
		{
			var delta = transform.position - center;
			bool isInsideCircle = delta.magnitude <= radius;
			if (!isInsideCircle)
			{
				transform.position = center + delta.normalized * radius;
				var normal = -delta.normalized;
				var normalProjection = Vector3.Project(currentSpeed, normal);
				currentSpeed -= normalProjection;
			}
		}

		if (target != null)
		{
			var delta = transform.position - target.transform.position;
			bool isInsideCircle = delta.magnitude <= targetRadius;
			if (!isInsideCircle)
			{
				transform.position = target.transform.position + delta.normalized * targetRadius;
				var normal = -delta.normalized;
				var normalProjection = Vector3.Project(currentSpeed, normal);
				currentSpeed -= normalProjection;
			}
		}
	}

	private void ProcessFrictionForces()
	{
		currentAcceleration -= currentSpeed * frictionFactor;
	}

	private void ApplicateEulerIntegration()
	{
		currentSpeed += currentAcceleration * Time.deltaTime;
		transform.position += currentSpeed * Time.deltaTime;
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
		Gizmos.DrawLine(transform.position, transform.position + currentSpeed);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + currentAcceleration);
	}

}

public class KeyMapping
{
	public KeyCode key;
	public Action action;
	public KeyActionType actionType;

	public bool GetKeyState()
	{
		switch (actionType)
		{
			case KeyActionType.OnKeyDown: return Input.GetKeyDown(key);
			case KeyActionType.OnKey: return Input.GetKey(key);
			case KeyActionType.OnKeyUp: return Input.GetKeyUp(key);
		}
		return false;
	}
}

public enum KeyActionType { Undefined, OnKeyDown, OnKey, OnKeyUp}
