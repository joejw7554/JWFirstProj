using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputComponent : MonoBehaviour
{
    [Header("MovementSettings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private InputAction moveAction;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // InputAction 생성 및 바인딩 설정
        moveAction = new InputAction("Move", InputActionType.Value);
        
        // WASD 키 바인딩 (2D Vector Composite)
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // InputAction 활성화
        moveAction.Enable();

    }
    
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight= cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight* moveInput.x).normalized;


        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void OnDestroy()
    {
        moveAction?.Dispose();
    }
}
