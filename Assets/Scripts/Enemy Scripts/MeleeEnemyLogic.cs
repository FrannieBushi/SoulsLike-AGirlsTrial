using UnityEngine;

public class MeleeEnemyLogic : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f;
    public float attackCooldown = 2f;
    private float cooldownTimer = 0f;
    private bool isAttacking = false;
    private bool isFacingRight = true;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private Enemy enemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }

        if (enemy.healthPoints > 0 && player != null && !isAttacking)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if ((player.position.x < transform.position.x && isFacingRight) ||
                (player.position.x > transform.position.x && !isFacingRight))
            {
                Flip();
            }

            if (distance <= attackRange && cooldownTimer <= 0f)
            {
                StartAttack();
            }
            else if (distance <= detectionRange)
            {
                FollowPlayer();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("Move", false);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("Move", false);
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        anim.SetBool("Move", true);
    }

    void StartAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("Move", false);
        anim.SetTrigger("Attack");
        cooldownTimer = attackCooldown;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        sprite.flipX = !sprite.flipX;
    }
}