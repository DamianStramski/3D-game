using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 180f;
    public float jumpHeight = 2f;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;

    private bool doubleJumpUnlocked = false;
    private bool canDoubleJump = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;

        // Odczyt z GameManagera
        if (GameManager.Instance != null)
        {
            EnableDoubleJump(GameManager.Instance.hasDoubleJump);
        }
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 moveDir = transform.forward * verticalInput;
        float magnitude = Mathf.Clamp01(Mathf.Abs(verticalInput)) * speed;
        moveDir.Normalize();

        Vector3 velocity = moveDir * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

        if (characterController.isGrounded)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            canDoubleJump = true;

            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
                if (animator != null) animator.SetTrigger("Jump");
            }
        }
        else
        {
            characterController.stepOffset = 0;
            ySpeed += Physics.gravity.y * Time.deltaTime;

            if (doubleJumpUnlocked && canDoubleJump && Input.GetButtonDown("Jump"))
            {
                ySpeed = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
                canDoubleJump = false;
                if (animator != null) animator.SetTrigger("DoubleJump");
            }
        }

        if (animator != null)
        {
            animator.SetBool("IsMoving", Mathf.Abs(verticalInput) > 0.1f);
            animator.SetBool("IsGrounded", characterController.isGrounded);
        }
    }

    public void EnableDoubleJump(bool enabled)
    {
        doubleJumpUnlocked = enabled;

    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.HandleHitFromPlayer(hit);
        }
    }

}