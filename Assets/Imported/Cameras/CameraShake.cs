using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	// Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    public Camera camera;
    
    // How long the object should shake for.
    private float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    private float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private bool startShake = false;

    public bool shakeOnEnable = false;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void Start()
    {
        //Shake(20,0.2f);

    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (startShake)
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                camTransform.localPosition = originalPos;
                startShake = false;
            }
        }
        if (shakeOnEnable && camera != null && camera.enabled) {
            shakeOnEnable = false;
            Shake(5, 0.4f);
            StartCoroutine(NextShake());
        }
    }

    public void Shake(float t)
    {
        startShake = true;
        shakeDuration = t;
        originalPos = camTransform.localPosition;
    }

    public void Shake(float t, float c)
    {
        if (!startShake) {
            startShake = true;
            shakeAmount = c;
            shakeDuration = t;
            originalPos = camTransform.localPosition;
        }
    }

    public void IncrementShake(float t, float c)
    {
        startShake = true;
        shakeAmount = c;
        shakeDuration = t;
        //originalPos = camTransform.localPosition;
    }

    public void StopShake() {
        shakeDuration = 0f;
        camTransform.localPosition = originalPos;
        startShake = false;
    }

    IEnumerator NextShake() {
        yield return new WaitForSeconds(5f);
        shakeOnEnable = true;
    }
}
