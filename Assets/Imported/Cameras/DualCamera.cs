using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DualCamera : MonoBehaviour {
	
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

	private Vector3 hit;

    public bool isTrueInstance = false;

    private bool _shakeLoop = false;

    public static DualCamera s_instance;

    public static DualCamera Instance
    {

        get
        {
            if (s_instance == null)
            {
                DualCamera tmp = GameObject.FindObjectOfType<DualCamera>();
                if (tmp.isTrueInstance) s_instance = tmp;
            }
            return s_instance;
        }
    }

    void Awake()
	{
        s_instance = this;
		_cam = GetComponent<Camera>();
		InitCamera();
	}

	void Start()
	{
		_actualSmoothTime = smoothTime;
	}

	void FixedUpdate()
    {
        if (TrackTwoPlayers)
        {
            FocusPlayers();
            CompensateForWalls(ref _target, _target - offset);
            SmoothPosition();
        }
        else
        {
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

	private void CompensateForWalls(ref Vector3 from, Vector3 to) {
		/*Debug.DrawLine(from, to, Color.magenta);

		RaycastHit wallHit = new RaycastHit();
		if (Physics.Linecast(to, from, out wallHit, layerMask)) {
			Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
			from = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z);
			hit = from;
		}*/
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
		
		CompensateForWalls(ref targetPosition, char1.position);

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

    public void Shake(float time, float strength)
    {
        //transform.DOShakePosition(time, strength).Play();
    }

    public void ShakeLoop(float strength, bool enable)
    {
        if (enable && !_shakeLoop)
        {
            _shakeLoop = true;
            //transform.DOShakePosition(100f, strength).Play();
        }
        else
        {
            //transform.dosha
            //DOTween.Kill(transform);
            _shakeLoop = false;
        }
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
		
		if (CheckPlayersInsideCam()) {
			Quaternion targetRot = Quaternion.LookRotation(MiddlePointBetweenChars + lookAtOffset-transform.position);

			transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, _slowTime * lookAtSmoothFactor * Time.deltaTime);
		}
		
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
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(_target, .25f);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(transform.position, .35f);
		}
	}

}
