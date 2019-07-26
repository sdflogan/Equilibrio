using UnityEngine;
using System.Collections;

public class PlayerController3D : MonoBehaviour
{
    [Header("Movement")]
    public string horizontalAxis;
    public string verticalAxis;
    public float moveSpeed;
    public LayerMask floorMask;
    private Rigidbody _rb;
    private Transform _cameraTransform;

    private Animator _anim;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
        _anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw(horizontalAxis);
        float v = Input.GetAxisRaw(verticalAxis);

        CheckMovement(h, v);

        CheckRotation();

        _anim.SetFloat("Speed", Mathf.Abs(h) + Mathf.Abs(v));
    }

    private void CheckMovement(float h, float v)
    {
        // Obtenemos un vector de movimiento basándonos en el input
        Vector3 movement = new Vector3(h, 0f, v);

        // Normalizamos el vector y lo hacemos proporcional a la velocidad por segundo
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        // Rota el vector de movimiento teniendo en cuenta la rotación de la cámara Quaternion * Vector3
        movement = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0) * movement;

        // Desplaza al personaje moviendo su rigidbody
        _rb.MovePosition(transform.position + movement);
    }

    private void CheckRotation()
    {
        // Crea un rayo que va desde el cursor en la pantalla en la dirección que tenga la cámara
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Variable para guardar información de la colisión del rayo (la llamamos floorHit porque sólo chocará con el layer suelo)
        RaycastHit floorHit;

        // Comprueba si el rayo choca con algo (cuyo layer esté en floorMask) y lo almacena en floorHit
        if (Physics.Raycast(camRay, out floorHit, 100f, floorMask))
        {
            // Crea un vector dirección que va del personaje al punto en el que ha colisionado el rayo
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Nos aseguramos de que el vector se mantenga en el plano correspondiente a la posición y del personaje
            playerToMouse.y = transform.position.y;

            // Creamos un Quaternion basándonos en ese vector dirección
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Aplicamos el Quaternion a la rotación del personaje
            _rb.MoveRotation(newRotation);
        }
    }
}
