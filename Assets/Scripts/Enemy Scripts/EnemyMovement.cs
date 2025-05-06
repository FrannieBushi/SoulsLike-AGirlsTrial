
using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    float speed;
    Rigidbody2D rb;
    Animator anim;
    public bool isStatic;
    public bool isWalker;
    public bool isPatroling;
    public bool wait;
    bool isWaiting;
    public float timeToWait;

    public bool walksRight;

    public Transform wallCheck, pitCheck, groundCheck;
    public bool wallDetected, pitDetected, isGrounded;
    public float detectionRadius;
    public LayerMask whatIsGround;

    private bool canFlip = true;
    public float flipCooldown = 0.2f; 

    public Transform pointA, pointB;
    bool goToA, goToB;

    void Start()
    {
        goToA = true;
        speed = GetComponent<Enemy>().speed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        pitDetected = !Physics2D.OverlapCircle(pitCheck.position, detectionRadius, whatIsGround);
        wallDetected = Physics2D.OverlapCircle(wallCheck.position, detectionRadius, whatIsGround);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, detectionRadius, whatIsGround);

        if ((pitDetected || wallDetected) && isGrounded && canFlip)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {

        if (isStatic)
        {
            anim.SetBool("idle", true);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (isWalker)
        {
            anim.SetBool("idle", false);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
               
            if (walksRight)
            {
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            }
        }
        if (isPatroling)
        {
            anim.SetBool("idle", false);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (goToA)
            {
                if(!isWaiting)
                {
                    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
                }
                
                if (Vector2.Distance(transform.position, pointA.position) < 1f)
                {

                    if(wait)
                    {
                        StartCoroutine(Waiting());
                    }

                    goToA = false;
                    goToB = true;
                    
                }
            }
            else if (goToB)
            {
                if(!isWaiting)
                {
                    rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
                }
                
                if (Vector2.Distance(transform.position, pointB.position) < 0.5f)
                {

                    if(wait)
                    {
                        StartCoroutine(Waiting());
                    }

                    goToA = true;
                    goToB = false;
                    
                }
            }
        }
    }

    public void Flip()
    {
        walksRight = !walksRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        StartCoroutine(FlipCooldownCoroutine());
    }

    private IEnumerator FlipCooldownCoroutine()
    {
        canFlip = false;
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;
    }

    private IEnumerator Waiting()
    {
        anim.SetBool("idle", true);
        isWaiting = true;
        yield return new WaitForSeconds(timeToWait);
        Flip();
        isWaiting = false;
    }
}
