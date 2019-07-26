using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DualCameraSprings : MonoBehaviour {
	
	public bool TrackTwoPlayers = true;
	public Transform char1, char2;
	public float smoothSpeed = 2f;
	public float smoothTime = .25f;
	public Vector3 offset;
	public float zoomFactor = 1.0f;
	public float maxDistanceZoom = 7;
	public float closeZoomDistance = 3f;
	public float closeZoom = 0.3f;
	public float midZoomDistance = 10f;
	public LayerMask layerMask;
	public float springMinDistance = 2f;
	public float wallHitUp = 2.5f;
	public float maxCameraWallDistance = 5f;

	private Vector3 _target;
	private Vector3 _targetPosition;
	private bool _focusing = true;
	private float _slowTime = 1f;

	public static Vector3 MiddlePointBetweenChars = Vector3.zero;

	private Vector3 _refPositionVelocity;

	public Vector3 lookAtOffset;
	public float lookAtSmoothFactor = 15f;

	private Camera _cam;

	private float _distanceBetweenChars;

	private bool _insideCam = true;
	private bool _farAway = false;
	private float _actualSmoothTime;
	private bool _isChar1Inside = true;
	private bool _isChar2Inside = true;
	private Renderer _char1Renderer;
	private Renderer _char2Renderer;

	private Vector3 hit;
	private Vector3 lastHit;

	void Awake()
	{
		_cam = GetComponent<Camera>();
		InitCamera();
		_char1Renderer = char1.gameObject.GetComponentInChildren<Renderer>();
		_char2Renderer = char2.gameObject.GetComponentInChildren<Renderer>();
	}

	void Start()
	{
		_actualSmoothTime = smoothTime;
	}

	void FixedUpdate()
	{
		if (TrackTwoPlayers) {
			FocusPlayers();
			CompensateForWalls(ref _target, MiddlePointBetweenChars);	// _target - offset
			SmoothPosition();
		}
		else {
			
			SmoothPositionSingle();
		}
	}

	void FocusPlayers() {
		// Punto medio entre los personajes
		MiddlePointBetweenChars = (char1.position + char2.position) * 0.5f;

		// Distancia entre los personajes
		_distanceBetweenChars = (char1.position - char2.position).magnitude;

		_insideCam = CheckPlayersInsideCam();
		zoomFactor = FixZoom();

		float distanceZoom = _distanceBetweenChars*zoomFactor;

		// Impedimos que la cámara se aleje demasiado
		if (distanceZoom < maxDistanceZoom) {
			_farAway = false;
			// Mover cámara a una distancia
			_target = MiddlePointBetweenChars - transform.forward * distanceZoom + offset;

			if (!_focusing) StartCoroutine(Refocusing(2));
			_focusing = true;
		}	
		else {
			_focusing = false;
			_actualSmoothTime = smoothTime * 0.3f * _distanceBetweenChars;
			_slowTime = 0.25f;
			_farAway = true;
			_target = MiddlePointBetweenChars - transform.forward * maxDistanceZoom + offset;
		}
	}

	bool CheckCloserInsideCam() {
		float distC1, distC2;

		distC1 = Vector3.Distance(char1.position, transform.position);
		distC2 = Vector3.Distance(char2.position, transform.position);

		//Vector3 charPosition = (distC1 < distC2 ? char1.position : char2.position);
		//Vector3 charViewport = _cam.WorldToViewportPoint(charPosition);

		bool isCloserInside = (distC1 < distC2 ? _isChar1Inside : _isChar2Inside);

		return (isCloserInside);
	}

	bool CheckPlayersInsideCam() {
		Vector3 char1Viewport = _cam.WorldToViewportPoint(char1.position);
		Vector3 char2Viewport = _cam.WorldToViewportPoint(char2.position);

		_isChar1Inside = CheckPlayerInsideCam(char1Viewport);
		
		_isChar2Inside= CheckPlayerInsideCam(char2Viewport);

		return (_isChar1Inside && _isChar2Inside);
	}

	bool CheckPlayerInsideCam(Vector3 charViewport) {
		float offset = (_insideCam ? 0.05f : 0.15f);
		bool isInside = (charViewport.x - offset > 0 && charViewport.x + offset < 1 
								&& charViewport.y - offset > 0 && charViewport.y + offset < 1);
		return isInside;
	}

	float FixZoom() {
		float zoom;

		if (_insideCam) {
			if (_distanceBetweenChars < closeZoomDistance) 
				zoom = closeZoom;
			else
				zoom = .4f;
		}
		else 
			zoom = .55f;
		
		
		return zoom;
	}

	private void CompensateForWallsOld(ref Vector3 from, Vector3 to, bool checkBoth = true) {
		Debug.DrawLine(from, to, Color.magenta);

		RaycastHit wallHit = new RaycastHit();
		if (Physics.Linecast(to, from, out wallHit, layerMask)) {
			Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
			//Debug.DrawLine(to, _cam.transform.position, Color.yellow);

			//float distance = Vector3.Distance(from, wallHit.point);
			float distance = Vector3.Distance(to, wallHit.point);

			if (distance < springMinDistance || !CheckIfCharsAreVisible(checkBoth, _cam.transform.position)) {
				from = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z);
				lastHit = from;
				Debug.Log("[CAMERA] DENTRO IF");
			}
			else {
				float percentaje = 1 / distance;
				Debug.Log("[CAMERA] FUERA IF - " + wallHitUp*percentaje);
				if (lastHit != Vector3.zero) 
					from = lastHit;
				else
					from = new Vector3(wallHit.point.x, wallHit.point.y + wallHitUp*percentaje, wallHit.point.z);
			}

			hit = from;
		}
		else {
			lastHit = Vector3.zero;
			hit = from;
		}
	}

	private void CompensateForWalls(ref Vector3 from, Vector3 to, bool checkBoth = true) {
		Debug.DrawLine(from, to, Color.magenta);

		RaycastHit wallHit = new RaycastHit();
		if (Physics.Linecast(to, from, out wallHit, layerMask)) {
			Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
			//Debug.DrawLine(to, _cam.transform.position, Color.yellow);

			float distance = Vector3.Distance(to, wallHit.point);

			// Si distancia mínima, la cámara comienza a subir
			if (distance < springMinDistance || !CheckIfCharsAreVisible(checkBoth, _cam.transform.position)) {
				float percentaje = 1 / distance;
				float upDistance = Mathf.Clamp(wallHitUp*percentaje, 0, maxCameraWallDistance);
				from = new Vector3(wallHit.point.x, wallHit.point.y + upDistance, wallHit.point.z);
				lastHit = from;
				//Debug.Log(upDistance + ":" + wallHit.point.x + ":" + wallHit.point.z);
			}
			else {
				if (lastHit != Vector3.zero) 
					from = lastHit;
				else
					from = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z);
				//Debug.Log("wat " + from.y + ":" + from.x + ":" + from.z);
			}

			hit = from;
			_actualSmoothTime = smoothTime * 0.8f * Vector3.Distance(from, _cam.transform.position);
		}
		else {
			lastHit = Vector3.zero;
			hit = from;
		}
	}

	void SmoothPosition() {
		SmoothMovement();
		if (_focusing) SmoothRotation();
	}

	void SmoothPositionSingle() {
		SmoothMovementSingle();
	}

	void SmoothMovementSingle() {
		Vector3 targetPosition = char1.position + offset;
		
		CompensateForWalls(ref targetPosition, char1.position, false);

		transform.position = Vector3.SmoothDamp(
			transform.position,
			targetPosition,
			ref _refPositionVelocity,
			_actualSmoothTime
		) ;
		
	}

	public void InitCamera() {
		FocusPlayers();
		InitPosition();
		InitRotation();
	}

	IEnumerator Refocusing(float time) {
		yield return new WaitForSeconds(time);
		_actualSmoothTime = smoothTime;
		_slowTime = 1f;
	}

	private void SmoothMovement() {
		Vector3 targetPosition = _target;

		//CompensateForWalls(transform.position + offset, ref targetPosition);

		if (CheckCloserInsideCam() || !_farAway) {
			//Debug.Log(_actualSmoothTime);
			transform.position = Vector3.SmoothDamp(
				transform.position,
				targetPosition,
				ref _refPositionVelocity,
				_actualSmoothTime
			) ;
		}
	}

	private void InitPosition() {
		Vector3 targetPosition = _target + offset;

		transform.position = targetPosition;
	}

	private void InitRotation() {
		Quaternion targetRot = Quaternion.LookRotation(MiddlePointBetweenChars + lookAtOffset-transform.position);

		transform.rotation = targetRot;
	}

	private void SmoothRotation() {
		
		//if (CheckPlayersInsideCam()) {
			
			Quaternion targetRot = Quaternion.LookRotation(MiddlePointBetweenChars + lookAtOffset-transform.position);

			transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _slowTime * lookAtSmoothFactor * Time.deltaTime);
		//}
		
	}

	private bool CheckIfCharsAreVisible(bool both, Vector3 cameraPos) {
		bool visible;

		Debug.Log("[CAMERA] COMPROBANDO VISIBILIDAD");

		if (both) {
			visible = CheckIfFlameVisible(cameraPos) && CheckIfWaxIsVisible(cameraPos);
		}
		else {
			visible = CheckIfWaxIsVisible(cameraPos);
		}
		if (!visible)
			Debug.Log("[CAMERA] CHARS HAN DEJADO DE SER VISIBLES !!");
		else
			Debug.Log("[CAMERA] CHARS SON VISIBLES");
		return visible;
	}

	private bool CheckIfWaxIsVisible(Vector3 cameraPos) {
		return CheckIfVisible(char1.transform.position, cameraPos);
	}

	private bool CheckIfFlameVisible(Vector3 cameraPos) {
		return CheckIfVisible(char2.transform.position, cameraPos);
	}

	private bool CheckIfVisible(Vector3 charPosition, Vector3 cameraPos) {
		bool visible = true;
		RaycastHit hit = new RaycastHit();

		Debug.DrawLine(charPosition, cameraPos, Color.yellow);
		if (Physics.Linecast(cameraPos, charPosition, out hit, layerMask)) {
			Debug.DrawLine(hit.point, cameraPos, Color.green);
			visible = false;
		}
		return visible;
	}

	private bool CheckIfVisibleOld(Renderer renderer) {
		return IsVisibleFrom(renderer, _cam);
	}

	private bool IsVisibleFrom(Renderer renderer, Camera camera) {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(MiddlePointBetweenChars, .5f);

		if (hit != null) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(hit, .5f);
		}

		if (_target != null) {
			//Gizmos.color = Color.blue;
			Gizmos.DrawSphere(_target, .25f);
			//Gizmos.color = Color.green;
			Gizmos.DrawSphere(transform.position, .35f);
		}
	}

}
