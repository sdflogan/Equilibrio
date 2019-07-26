using UnityEngine;
using System.Collections;

public class SimpleCharacterController3D : MonoBehaviour
{
    [Header("Movement")]
    public string horizontalAxis;
    public string verticalAxis;
    public float moveSpeed;

    [Header("Jump")]
    public bool canJump;
    public string jumpAxis;
    public float jumpForce;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private bool _grounded;
    private bool _jumpKeyPressed;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw(horizontalAxis);
        float v = Input.GetAxisRaw(verticalAxis);

        CheckMove(h, v);
        if(canJump)
            CheckJump();

        if(groundCheck != null)
            _grounded = Physics.OverlapSphere(groundCheck.position, 0.01f, whatIsGround).Length > 0;
    }

    private void CheckMove(float h, float v)
    {
        if (h != 0 || v != 0)
        {
            Rotating(h, v);
            transform.Translate(new Vector3(0, 0, Time.deltaTime * moveSpeed));
        }
    }

    private void Rotating(float h, float v)
    {
        Vector3 targetDirection = new Vector3(h, 0f, v);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        _rb.MoveRotation(targetRotation);
    }

    private void CheckJump()
    {
        if (_grounded && jumpAxis != "" && Input.GetAxisRaw(jumpAxis) > 0 && _jumpKeyPressed == false)
        {
            _jumpKeyPressed = true;
            _grounded = false;
            _rb.AddForce(Vector3.up * jumpForce);
        }

        if (Input.GetAxisRaw(jumpAxis) == 0 && _jumpKeyPressed == true)
        {
            _jumpKeyPressed = false;
        }
    }
}
