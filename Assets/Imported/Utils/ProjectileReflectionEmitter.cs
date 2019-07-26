using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileReflectionEmitter : MonoBehaviour {
	
	public int maxReflectionCount = 5, maxLaserPoints = 8;
	public float maxStepDistance = 100;

	private LineRenderer line;
	private int _laserPos = 0;
	private bool _stop = false;
	private bool _col = false;

	public LayerMask collisionMask;

	//private LaserGlowPool glowPool;
	public GameObject glowPrefab;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		line.positionCount = maxLaserPoints+1;
		//glowPool = GetComponent<LaserGlowPool>();
		//glowPool.Init(line.positionCount, glowPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		DrawLaser();
	}

	void DrawLaser() {
		DrawPredictedLaserPatternD(this.transform.position, this.transform.forward, maxLaserPoints, maxStepDistance);
	}

	void DrawPredictedLaserPattern(Vector3 position, Vector3 direction, int lasersRemainings) {
		if (lasersRemainings == 0) {
			_laserPos = 0;
			return;
		}
		
		Vector3 startingPosition = position;

		if (_laserPos == 0 && lasersRemainings != 0) {
			line.SetPosition(_laserPos, startingPosition);
			_laserPos++;
		}

		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, maxStepDistance, collisionMask)) {
			direction = Vector3.Reflect(direction, hit.normal);
			position = hit.point;
		}
		else 
			position += direction * maxStepDistance;

		line.SetPosition(_laserPos, position);
		_laserPos++;
		DrawPredictedLaserPattern(position, direction, lasersRemainings - 1);
	}

	void DrawPredictedLaserPatternD(Vector3 position, Vector3 direction, int lasersRemainings, float dRemaining) {
		if (lasersRemainings == 0 || _stop) {
			if (_stop) 
				line.positionCount = _laserPos;

			_laserPos = 0;
			_stop = false;
			_laserPos = 0;
			//glowPool.CloseGlows();
			return;
		}
		
		Vector3 startingPosition = position;
		Vector3 targetPosition = startingPosition;

		if (_laserPos == 0 && lasersRemainings != 0) {
			line.SetPosition(_laserPos, startingPosition);
			_laserPos++;
		}

		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, dRemaining, collisionMask)) {
			direction = Vector3.Reflect(direction, hit.normal);
			targetPosition = hit.point;
			_col = true;
		}
		else {
			targetPosition += direction * dRemaining;
			_col = false;
		}
		
		float distancia = Vector3.Distance(startingPosition, targetPosition);
		if (distancia <= dRemaining) {
			position = targetPosition;
			dRemaining -= distancia;
			//if (_col) glowPool.GetGlow(targetPosition);
		}
		else {
			position += direction * dRemaining;
			_stop = true;
		}
		
		if (_laserPos >= line.positionCount)
			line.positionCount++;

		line.SetPosition(_laserPos, position);
		_laserPos++;
		
		DrawPredictedLaserPatternD(position, direction, lasersRemainings - 1, dRemaining);
	}
	/*
	void OnDrawGizmos()
	{
		Handles.color = Color.red;
		Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, 
			this.transform.rotation, 0.5f, EventType.Repaint);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.transform.position, 0.25f);

		DrawPredictedReflectionPattern(this.transform.position + this.transform.forward * 0.75f, 
			this.transform.forward, maxReflectionCount);
	}

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0) 
			return;
		
		Vector3 startingPosition = position;

		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, maxStepDistance)) {
			direction = Vector3.Reflect(direction, hit.normal);
			position = hit.point;
		}
		else 
			position += direction * maxStepDistance;

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(startingPosition, position);

		DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }
    */
}
