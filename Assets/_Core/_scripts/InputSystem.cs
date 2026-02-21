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


    private InputActionMap playerMap;


    public Action<Vector2> OnMoveInput;
    public Action OnJumpInput;
    public Action<Vector2> OnPovInput;
    public Action<bool> OnSprintInput;

    
    private void OnEnable()
    {
        playerMap.Enable();
    }

    private void OnDisable()
    {

        playerMap.Disable();

    }
    private void Awake()
    {
        playerMap = inputActions.FindActionMap("Player");
        MoveAction = playerMap.FindAction("Move");
        JumpAction = playerMap.FindAction("Jump");
        PovAction = playerMap.FindAction("Look");
        SprintAction = playerMap.FindAction("Sprint");

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
        OnMoveInput(PovAction.ReadValue<Vector2>());    
    }

    private void CheckSprintInput()
    {
        OnSprintInput(SprintAction.triggered);
    }
    void Update()
    {
        CheckMoveInout();
        CheckJumpInout();   
        CheckPovInput();
        CheckSprintInput(); 
    }
}
