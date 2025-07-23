using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed = 2.5f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;

    [Header("Jump")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 1.5f;
    float verticalVelocity;

    [Header("References")]
    [SerializeField] Transform cameraTransform;

    CharacterController characterController;
    AnimationsController animationsController;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationsController = GetComponent<AnimationsController>();
    }

    void Update()
    {
        MoveCharacter();
        ApplyGravityAndJump();
    }

    void MoveCharacter()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float mouseDeltaX = Input.GetAxis("Mouse X");

        Vector3 input = new Vector3(inputX, 0f, inputZ).normalized;

        if (input.magnitude >= 0.1f)
        {
            // Rotation speed depends on mouse movement
            float baseRotationSpeed = 2f;
            float boostedRotationSpeed = 6f;
            float currentRotationSpeed = Mathf.Abs(mouseDeltaX) > 0.01f ? boostedRotationSpeed : baseRotationSpeed;

            // Check if the movement is only sideways (A/D)
            bool sideOnly = Mathf.Abs(inputX) > 0.1f && Mathf.Abs(inputZ) < 0.1f;
            float finalYaw;

            if (sideOnly)
            {
                float yawOffset = inputX > 0f ? 90f : -90f;
                finalYaw = cameraTransform.eulerAngles.y + yawOffset;
            }
            else
            {
                finalYaw = cameraTransform.eulerAngles.y;
            }

            Quaternion targetRotation = Quaternion.Euler(0f, finalYaw, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * currentRotationSpeed);

            // Direction of movement relative to the camera
            Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
            Vector3 moveDir = (camForward * inputZ + camRight * inputX).normalized;

            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            characterController.Move(moveDir * speed * Time.deltaTime);

            // Animations
            if (inputZ < -0.1f)
                animationsController.ChangeAnimation(animationsController.walk_backward);
            else if (Input.GetKey(KeyCode.LeftShift))
                animationsController.ChangeAnimation(animationsController.run);
            else
                animationsController.ChangeAnimation(animationsController.walk_forward);
        }
        else
        {
            animationsController.ChangeAnimation(animationsController.idle);
        }
    }

    void ApplyGravityAndJump()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -2f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animationsController.ChangeAnimation(animationsController.jump);
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}
