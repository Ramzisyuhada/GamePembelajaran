using System;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _Speed = 400f;
    private float _RotationSmoothTime = 0.1f;
    private float _RotationSmoothSpeed;

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
    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }
    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
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

        if (input.magnitude > 0.1f)
        {
            float RotasiAngle = Mathf.Atan2(input.x,input.y) * Mathf.Rad2Deg;
            float SmoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, RotasiAngle, ref _RotationSmoothSpeed, _RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, SmoothAngle, 0f);
            MovementDirection = Quaternion.Euler(0f, RotasiAngle, 0f) * Vector3.forward;
            rb.AddForce(MovementDirection * Time.deltaTime * _Speed);
        }
        //rb.AddForce();
    }
    private void CheckGrounded()
    {
        _IsGround = Physics.CheckSphere(_GroundDetector.position, _DetectorRadius, _GroundLayer);
    }
    public void PovOn(Vector2 input) 
    {

    }

    public void JumpOn()
    {
        if (_IsGround) rb.AddForce(Vector3.up * _JumpForce * Time.deltaTime);
    }

    public void SprintOn(bool IsSprint)
    {
        if(IsSprint)
        {
            if (_Speed < _SprintSpeed)
            {
                _Speed = _Speed + _WalkSprintTransition * Time.deltaTime;
            }
        }
        else
        {
            if (_Speed > _SprintSpeed)
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
    
