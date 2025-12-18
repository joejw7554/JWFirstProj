using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputComponent : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction runAction;
    private InputAction attackAction;

    public event Action OnAttackPressed;

    public Vector2 MoveInput { get; private set; }
    public bool IsRunning { get; private set;  }

    void Awake()
    {
        InputSetup(); //Å°¼³Á¤
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void InputSetup()
    {
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");


        runAction = new InputAction("Run", InputActionType.Button);
        runAction.AddBinding("<keyboard>/leftShift");


        attackAction= new InputAction("Attack", InputActionType.Button);
        attackAction.AddBinding("<mouse>/leftButton");


        runAction.Enable();
        moveAction.Enable();
        attackAction.Enable();
    }

    void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        IsRunning = runAction.IsPressed();
        
        if(attackAction.WasPressedThisFrame())
        {
            OnAttackPressed?.Invoke();
        }
        
    }

    void OnDestroy()
    {
        runAction?.Dispose();
        moveAction?.Dispose();
        attackAction?.Dispose();


    }
}
