using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class Player : MonoBehaviour {
   
	/*
     
    [Header("Jump Config")]
	[Range(1, 10)]
	public float jumpBase = 2.5f;
	[Range(1, 10)]
	public float jumpHealth = 2.5f;
	public LayerMask layers;
	public float jumpDelay;

	protected bool grounded;

	[Header("Partner config")]
	public Player partner;

	[Header("Movement config")]
	public float speedByHealthFactor = 1.75f;
	[Range(0, 1)]
	public float minSpeedByHealth = 0;

	private float _speedDampTime = .1f;

	private bool _forceLookAt = false;
	
	private Transform _forceLookAtRot;

    public float fadeTime = 0.5f;
    public float scaleValue = 2f;
    private Vector3 _defaultScaleUI;

    // Use this for initialization

    virtual protected void Start () {
		_gc = GetComponentInChildren<GroundChecker>();
		_gc.SetLayers(layers);
		_gc.SetJumpDelay(jumpDelay);
		
		_pi = GetComponent<PlayerInteractuator>();

		_interacting = false;
		_givingHands = false;
		playerActions = new PlayerActions(control);
		clase = CharClass.undefined;
		_gc.SetPlayerActions(playerActions);
        SetupUI();
        //LoadData();
    }

	virtual protected void LoadData() {

	}
	
	// Update is called once per frame
	void Update () {
		if (_health.IsAlive() && !_loading)
			Interact();
	}

	void FixedUpdate() {
		if (_health.IsAlive() && !_loading && !_recovering) {
			Move();
			Jump();	
		}
		else if (_health.IsAlive() && _recovering) {
			ForceLookMate();
		}

		UpdateAnim();
	}

	void Move() {
		float h = playerActions.Move.X;//Input.GetAxis(horizontalAxis);
		float v = playerActions.Move.Y;//Input.GetAxis(verticalAxis);

		CheckSpeed(h, v);
		CheckMovement(h, v);
		CheckRotation(h, v);
	}

	public void Jump() {
		_rb.velocity = Vector3.up * (jumpBase);
	}

	// //////////////////////////////////////////////////////////
	// 														   //
	// 					MÉTODOS PRIVADOS                       //
	//                                                         //
	// //////////////////////////////////////////////////////////

	private void CheckMovement(float h, float v) {
		if (canMove) {
			// Movimiento X-Z del input
			Vector3 movement = new Vector3(h, 0f, v);

			// Obtenemos el desplazamiento del Input
			float percentajeSpeed = movement.magnitude;
			if (percentajeSpeed > 1) percentajeSpeed = 1;

			// Obtenemos el porcentaje correspondiente a la salud restante
			float speedByHealth = (_health.GetPercentajeHealth() < minSpeedByHealth ? minSpeedByHealth : _health.GetPercentajeHealth()) * speedByHealthFactor;
			if (speedByHealth > 1) speedByHealth = 1;

			// Normalizamos y lo hacemos proporcional a la velocidad por segundo
			if (_defaultSpeed)
				movement = movement.normalized * maxMoveSpeed * Time.deltaTime * speedByHealth;
			else
				movement = movement.normalized * _changedSpeed * Time.deltaTime;
			
			// PARA PC
			//if (playerActions.Walk.IsPressed) movement *= 0.5f;

			// Rotamos el vector para que se ajuste a la rotación de la cámara
			movement = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * movement;

			
			// Desplazamos el personaje si no se sale del viewport de la cámara

			if (CheckInsideCamera(transform.position + (movement * percentajeSpeed))) 
				_rb.MovePosition(transform.position + (movement * percentajeSpeed));
			else if (CheckIfGoingInsideCamera(transform.position + (movement * percentajeSpeed)))
				_rb.MovePosition(transform.position + (movement * percentajeSpeed));
		}
	}

	private void CheckRotation(float h, float v) {
		// Rotación X-Z del input
		Vector3 rotation = new Vector3(h, 0f, v);

		// Rotamos el vector para que se ajuste a la rotación de la cámara
		rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * rotation;

		// Obtenemos la rotación final
		Quaternion quatR = Quaternion.LookRotation(rotation);
		
		// Interpolación para que la rotación se realice de forma suave
		if (rotation != Vector3.zero && !_forceLookAt) {
			_rb.MoveRotation(Quaternion.Lerp(_rb.rotation, quatR, Time.deltaTime * rotateSpeed));
			//_rb.MoveRotation(quatR);
		}
		else if (_forceLookAt) {
			Vector3 fixedLookAt = new Vector3(_forceLookAtRot.position.x,
											transform.position.y,
											_forceLookAtRot.position.z);

			rotation = fixedLookAt - transform.position;
			quatR = Quaternion.LookRotation(rotation);
			_rb.MoveRotation(Quaternion.Lerp(_rb.rotation, quatR, Time.deltaTime * rotateSpeed));
			Debug.Log("ROTACION MODIFICADA");
		}
	}

	private void ForceLookMate() {
		Vector3 matePos = new Vector3(partner.transform.position.x, transform.position.y, partner.transform.position.z);
		Vector3 rotation = matePos - transform.position;
		Quaternion quatR = Quaternion.LookRotation(rotation);
		_rb.MoveRotation(Quaternion.Lerp(_rb.rotation, quatR, Time.deltaTime * rotateSpeed));
		//_forceLookAt = true;
		//canMove = false;
		//_forceLookAtRot = partner.transform;
		//_forceLookAtRot.position = new Vector3(partner.transform.position.x, transform.position.y, partner.transform.position.z);
	}

	private bool CheckInsideCamera(Vector3 dest) {
		Vector3 viewPos = _camera.WorldToViewportPoint(dest);
		bool canMove = true;
		
		if (viewPos.x - .025f < 0 || viewPos.x + .025f > 1 ||
				viewPos.y - .025f < 0 || viewPos.y + .025f > 1)
			canMove = false;

		//Debug.Log("Viewpos: [" + viewPos.x + ", " + viewPos.y +"]");

		return canMove;
	}

	private bool CheckIfGoingInsideCamera(Vector3 dest) {
		Vector3 viewDest = _camera.WorldToViewportPoint(dest);
		Vector3 viewActual = _camera.WorldToViewportPoint(transform.position);

		bool canMove = false;

		bool nearX = ((viewActual.x < 0.1f && viewDest.x > viewActual.x) ||
						(viewActual.x > 0.9f && viewDest.x < viewActual.x) ||
						(viewActual.x >= 0.1f && viewActual.x <= 0.9f));
		bool nearY = ((viewActual.y < 0.1f && viewDest.y > viewActual.y) ||
						(viewActual.y > 0.9f && viewDest.y < viewActual.y) ||
						(viewActual.y >= 0.1f && viewActual.y <= 0.9f));
		
		if (nearX && nearY) canMove = true;

		//Debug.Log("viewActual: [" + viewActual.x + ", " + viewActual.y 
		//	+ "] -- viewDest: [" + viewDest.x + ", " + viewDest.y + "]");
	
		return canMove;
	}

    */
}
