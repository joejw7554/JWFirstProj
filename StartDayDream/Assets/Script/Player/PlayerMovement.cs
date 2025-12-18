using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;

    [Header("MovementSettings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    private Animator animator;
    private PlayerInputComponent input;
    private Rigidbody rb;

    private int speedHash;

    private void InitializeComponents()
    {
        this.GetRequiredComponent(out animator);
        this.GetRequiredComponent(out input);
        this.GetRequiredComponent(out rb);

        speedHash = Animator.StringToHash("speed");
    }

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
       
    }

    private void Update()
    {
        BaseMovement();
    }

    private void BaseMovement()
    {
        Vector2 moveInput = input.MoveInput;
        bool isRunning = input.IsRunning;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

        animator.SetFloat(speedHash, moveDirection.magnitude * currentSpeed);

        if (moveDirection.magnitude >= 0.1f)
        {
            transform.position += moveDirection * currentSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
