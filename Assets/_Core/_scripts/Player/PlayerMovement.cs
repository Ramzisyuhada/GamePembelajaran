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
        Vector3 MovementDirection = Vector3.zero;


        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Debug.Log( "Nilai Kecepatan Drai Input Player : "+ velocity.magnitude * input.magnitude);
        _anim.SetFloat("Velocity", velocity.magnitude * input.magnitude);
        if (input.magnitude > 0.1f)
        {

            float RotasiAngle = Mathf.Atan2(input.x,input.y) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            float SmoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, RotasiAngle, ref _RotationSmoothSpeed, _RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, SmoothAngle, 0f);
            MovementDirection = Quaternion.Euler(0f, RotasiAngle, 0f) * Vector3.forward;
            rb.AddForce(MovementDirection * Time.deltaTime * _Speed);
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

    public void JumpOn()
    {
        if (_IsGround)
        {
            rb.AddForce(Vector3.up * _JumpForce * Time.deltaTime);
            _anim.SetTrigger("IsJump");

        }
    }

    public void SprintOn(bool IsSprint)
    {
        if (IsSprint)
        {
            // Naik menuju sprint speed
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
    
