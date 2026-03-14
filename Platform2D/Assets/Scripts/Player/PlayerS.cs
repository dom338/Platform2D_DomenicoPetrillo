using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerS : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioSource PlayerAudio;



    private PlayerControls controls;

    public float MouvmentSpeed = 5.0f;
    private float moveInput;
    private bool facingRight = true;
    public float maxLife = 100f;
    public float currentLife = 0;
    public LifeBarS LifeBar;



    [SerializeField] public float jumpForce = 14f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;

    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private float coyoteTimer;
    private float jumpBufferTimer;

    [SerializeField] private float jumpCutMultiplier = 0.5f;

    private bool jumpHeld;

    [SerializeField] private Animator animator;
    private const string flashRedAmin = "FlashRed";

    [SerializeField] private CameraTargetFollow cameraTarget;

    public AudioClip jumpSound;

    [SerializeField] private Transform firePoint;
    private Vector3 firePointLocalPos;


    private void Awake()
    {
        controls = new PlayerControls();
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (cameraTarget == null)
        {
            cameraTarget = FindFirstObjectByType<CameraTargetFollow>();
        }
        if (PlayerAudio == null)
        {
            PlayerAudio = GetComponent<AudioSource>();
        }
        if (firePoint != null)
        {
            firePointLocalPos = firePoint.localPosition;
        }

    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Jump.performed += OnJump;
        controls.Player.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        controls.Player.Jump.performed -= OnJump;
        controls.Player.Jump.canceled -= OnJumpCanceled;
        controls.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLife = maxLife;

        LifeBar.SetMaxHealth(maxLife);


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();
        moveInput = input.x;

        HandleFlip();

        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("YVelocity", rb.linearVelocityY);

        isGrounded = Physics2D.OverlapCircle(
        groundCheck.position,
        groundCheckRadius,
        groundLayer
        );

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }


        //this code is only for testing
        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(20);

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Heal(20);
        }

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * MouvmentSpeed, rb.linearVelocityY);

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            jumpBufferTimer = 0;
            coyoteTimer = 0;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpHeld = true;
        jumpBufferTimer = jumpBufferTime;
        if (jumpSound != null)
        {
            PlayerAudio.PlayOneShot(jumpSound);
        }

    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        jumpHeld = false;

        if (rb.linearVelocityY > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * jumpCutMultiplier);
        }
    }



    private void HandleFlip()
    {
        if (moveInput > 0 && !facingRight)
        {
            facingRight = true;
            spriteRenderer.flipX = false;

            if (cameraTarget != null)
            {
                cameraTarget.SetFacingDirection(true);
            }

            if (firePoint != null)
            {
                firePoint.localPosition = new Vector3(
                    Mathf.Abs(firePointLocalPos.x),
                    firePointLocalPos.y,
                    firePointLocalPos.z
                );
            }
        }
        else if (moveInput < 0 && facingRight)
        {
            facingRight = false;
            spriteRenderer.flipX = true;

            if (cameraTarget != null)
            {
                cameraTarget.SetFacingDirection(false);
            }

            if (firePoint != null)
            {
                firePoint.localPosition = new Vector3(
                    -Mathf.Abs(firePointLocalPos.x),
                    firePointLocalPos.y,
                    firePointLocalPos.z
                );
            }
        }
    }
    public bool IsFacingRight()
    {
        return facingRight;
    }

    public void TakeDamage(int Damage)
    {
        currentLife -= Damage;
        animator.SetTrigger(flashRedAmin);
        LifeBar.SetHealth(currentLife);
    }

    public void Heal(int Healing)
    {
        currentLife += Healing;
        if (currentLife > maxLife)
        {
            currentLife = maxLife;
        }
        LifeBar.SetHealth(currentLife);
    }
}