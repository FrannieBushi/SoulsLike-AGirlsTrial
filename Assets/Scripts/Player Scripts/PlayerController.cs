using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed, jumpHeight;
    float velX, velY;
    Rigidbody2D rb;

    public Transform groundcheck;
    public bool isGrounded;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    PlayerAnimationManager animManager;
    public AudioSource audioSource;
    public AudioClip sound;

    public bool isDodging = false;

    [SerializeField] PlayerInputHandler inputHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animManager = GetComponent<PlayerAnimationManager>();
        audioSource = GetComponent<AudioSource>();   
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        FlipCharacter();
    }

    void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        if (isDodging) return;

        velX = inputHandler.moveInput;
        velY = rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(velX * speed, velY);

        animManager.isWalking = Mathf.Abs(velX) > 0.01f;

        /*if (isDodging) return;
        
        velX = Input.GetAxisRaw("Horizontal");
        velY = rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(velX * speed, velY);

        if(rb.linearVelocity.x != 0)
        {
            animManager.isWalking = true;
        }
        else
        {
            animManager.isWalking = false;
        }*/
    }

    public void FlipCharacter()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
    }

    public void StepSoundOn()
    {
        audioSource.PlayOneShot(sound);        
    }
    
}