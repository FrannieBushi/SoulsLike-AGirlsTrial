using UnityEngine;
using System.Collections;

public class DodgeController : MonoBehaviour
{
    public float dodgeForce = 5f;            
    public float dodgeCooldown = 2f;         
    public float dodgeDuration = 0.2f;       
    public KeyCode dodgeKey = KeyCode.LeftShift; 
    private Rigidbody2D rb;
    private Animator anim;
    private bool isDodging = false;
    public bool canDodge;
    PlayerStats playerStats;

    public AudioSource audioSource;
    public AudioClip sound;

    [SerializeField] private PlayerInputHandler inputHandler;
    private bool inputBuffered = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
        inputHandler = GetComponent<PlayerInputHandler>();   
    }

    void Update()
    {
        if (inputHandler != null && inputHandler.dodgePressed)
        {
            inputHandler.ResetInputs();

            bool grounded = GetComponent<JumpController>().IsGrounded();

            if (playerStats.dodgeUnlocked && grounded && canDodge)
            {
                StartCoroutine(Dodge());
            }
        }

        if (inputBuffered && playerStats.dodgeUnlocked && GetComponent<JumpController>().IsGrounded() && canDodge)
        {
            inputBuffered = false; 
            StartCoroutine(Dodge());
        }   
    }

    private IEnumerator Dodge()
    {
        canDodge = false;
        isDodging = true;
        GetComponent<PlayerController>().isDodging = true;
        anim.SetTrigger("dodge");
        audioSource.PlayOneShot(sound);
        float dodgeDirection = transform.localScale.x > 0 ? 1f : -1f;
        rb.linearVelocity = new Vector2(dodgeDirection * dodgeForce, rb.linearVelocity.y);

        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false;
        GetComponent<PlayerController>().isDodging = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

    public bool IsDodging()
    {
        return isDodging;
    }
}
