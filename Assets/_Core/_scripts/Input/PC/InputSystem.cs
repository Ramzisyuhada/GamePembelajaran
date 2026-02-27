using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{

    public InputActionAsset inputActions;
    /// <summary>
    /// Input Variable
    /// </summary>
    /// 
    private InputAction MoveAction;
    private InputAction JumpAction;
    private InputAction PovAction;
    private InputAction SprintAction;
    private InputAction MouseClickAction;


    private InputActionMap playerMap;


    public Action<Vector2> OnMoveInput;
    public Action OnJumpInput;
    public Action<Vector2> OnPovInput;
    public Action<bool> OnSprintInput;
    public Action<bool> Pause;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        playerMap = inputActions.FindActionMap("Player");
        MoveAction = playerMap.FindAction("Move");
        JumpAction = playerMap.FindAction("Jump");
        PovAction = playerMap.FindAction("Look");
        SprintAction = playerMap.FindAction("Sprint");
        MouseClickAction = playerMap.FindAction("Fire");

    }
  
    private void CheckMoveInout()
    {
        
        OnMoveInput(MoveAction.ReadValue<Vector2>());
    }

    private void CheckJumpInout() 
    {

        bool jump = JumpAction.triggered;

        if (jump)
        {
            if (OnJumpInput != null) OnJumpInput();
        }

    }

    private void CheckPovInput()
    {
        OnPovInput(PovAction.ReadValue<Vector2>());    
    }

    private void CheckSprintInput()
    {

        if (OnSprintInput != null)
            OnSprintInput(SprintAction.IsPressed());

    }
    void Update()
    {
 CheckPovInput();
        CheckJumpInout();
        CheckSprintInput(); 
    }

    private void FixedUpdate()
    {
        CheckMoveInout();

    }
}
