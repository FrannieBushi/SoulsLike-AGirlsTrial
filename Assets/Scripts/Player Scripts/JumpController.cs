using UnityEngine;

public class JumpController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] public float jumpForce;
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float coyoteConfing = 0.1f;
    [SerializeField] float fallCancelDistance = 0.5f;

    [SerializeField] float fallMultiplier = 2f;
    [SerializeField] float lowJumpMultiplier = 1.1f;

    float coyoteTime;    
    bool jumpPressed = false;
    int jumpCount;
    int maxJumpCount = 2;
    
    PlayerAnimationManager animationManager;
    Rigidbody2D rb;

    PlayerStats playerStats;

    public AudioSource audioSource;
    public AudioClip sound;

    [SerializeField] PlayerInputHandler inputHandler;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        animationManager = GetComponent<PlayerAnimationManager>();
        audioSource = GetComponent<AudioSource>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        if (IsGrounded())
        {
            coyoteTime = Time.time + coyoteConfing;
            jumpCount = 0;

            animationManager.isJumping = false;
            animationManager.isFalling = false;

            GetComponent<CombatController>().canAirAttack = true;
        }

        if (animationManager.isFalling)
        {
            RaycastHit2D closeHit = Physics2D.Raycast(transform.position, Vector2.down, fallCancelDistance, groundLayer);
            if (closeHit.collider != null)
            {
                Debug.Log("idle");
                animationManager.isFalling = false;
            }
        }

        if (rb.linearVelocity.y < -0.1f && !IsGrounded())
        {
            animationManager.isFalling = true;
            //animationManager.isJumping = false;
        }

        if (inputHandler.jumpPressed && !animationManager.isDrinking)
        {
            jumpPressed = true;
            inputHandler.ResetInputs();
        }
    }

    private void FixedUpdate()
    {
        if (jumpPressed)
        {
            if (Time.time <= coyoteTime && jumpCount == 0)
            {
                DoJump();
            }
            else if (jumpCount > 0 && jumpCount < maxJumpCount && playerStats.doubleJumpUnlock)
            {
                DoJump();
            }

            jumpPressed = false;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !inputHandler.jumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpCount++;

        animationManager.isJumping = true;
        animationManager.isFalling = false;
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        return hit.collider != null;
    }

    public void JumpSoundOn()
    {
        if(!IsGrounded())
        {
            audioSource.PlayOneShot(sound); 
        }
               
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * rayDistance);
    }
}