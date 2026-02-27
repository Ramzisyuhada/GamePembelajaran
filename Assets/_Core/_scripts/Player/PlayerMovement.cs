using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;



[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private float _Speed ;
    private float _RotationSmoothTime = 0.1f;
    private float _RotationSmoothSpeed;
    [SerializeField] private float _WalkSpeed = 400f;

    [SerializeField]
    private Transform _GroundDetector;

    [SerializeField]
    private LayerMask _GroundLayer;

    [SerializeField]
    private float _JumpForce;

    [SerializeField]
    private float _DetectorRadius;

    [SerializeField]
    private float _SprintSpeed;

    [SerializeField] 
    private float _WalkSprintTransition; 
    private bool _IsGround;
    [SerializeField] InputSystem _InputManager;
    
    private Rigidbody rb;
    private Camera camera;
    private Animator _anim;
    
    private void Awake()
    {
        Subscribe();

        camera = Camera.main;
        _anim = GetComponent<Animator>();   
        rb = GetComponent<Rigidbody>(); 
        _Speed = _WalkSpeed;
    }
    private void OnDestroy()
    {
       Unsubscribe();

    }

    private void OnDisable()
    {
    }

    private void PovMouseOn(bool Click)
    {

    }
    private void Subscribe()
    {
        _InputManager.OnMoveInput += MoveOn;
        _InputManager.OnPovInput += PovOn;
        _InputManager.OnJumpInput += JumpOn;
        _InputManager.OnSprintInput += SprintOn;
    }
    private void Unsubscribe()
    {
        _InputManager.OnMoveInput -= MoveOn;
        _InputManager.OnPovInput -= PovOn;
        _InputManager.OnJumpInput -= JumpOn;
        _InputManager.OnSprintInput -= SprintOn;
    }
    public void MoveOn(Vector2 input)
    {
        Vector3 movementDirection = Vector3.zero;

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        _anim.SetFloat("Velocity", horizontalVelocity.magnitude);

        if (input.magnitude > 0.1f)
        {
            // Hitung sudut rotasi berdasarkan kamera
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg
                                + camera.transform.eulerAngles.y;

            float smoothAngle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref _RotationSmoothSpeed,
                _RotationSmoothTime
            );

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            // Hitung arah gerak
            movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Set velocity horizontal saja
            rb.linearVelocity = new Vector3(
                movementDirection.x * _Speed,
                rb.linearVelocity.y, // jangan ganggu gravitasi
                movementDirection.z * _Speed
            );
        }
        else
        {
            // Stop perlahan (lebih natural)
            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f
            );
        }
    }
    private void CheckGrounded()
    {
        _IsGround = Physics.CheckSphere(_GroundDetector.position, _DetectorRadius, _GroundLayer);
        _anim.SetBool("IsGround", _IsGround);

    }
    public void PovOn(Vector2 input) 
    {

    }
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    private void FixedUpdate()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    public void JumpOn()
    {
        if (_IsGround)
        {
            rb.linearVelocity = new Vector3(
     rb.linearVelocity.x,
     0f,
     rb.linearVelocity.z
 );

            rb.AddForce(Vector3.up * _JumpForce, ForceMode.Impulse);
            _anim.SetTrigger("IsJump");

        }
    }

    public void SprintOn(bool IsSprint)
    {
        if (IsSprint)
        {
            if (_Speed < _SprintSpeed)
            {
                _Speed = _Speed + _WalkSprintTransition * Time.deltaTime;
            }
        }
        else
        {
            // Turun menuju walk speed
            if (_Speed > _WalkSpeed)
            {
                _Speed = _Speed - _WalkSprintTransition * Time.deltaTime;
            }
        }
    }

    private void Update()
    {
        CheckGrounded();
    }
}
    
