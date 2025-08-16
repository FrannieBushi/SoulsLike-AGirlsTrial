using UnityEngine;

public class FlameScript : MonoBehaviour
{
    float moveSpeed;
    Rigidbody2D rb;
    Vector2 moveDirection;
    PlayerStats target;

    void Start()
    {
        moveSpeed = GetComponent<Enemy>().speed;
        rb = GetComponent<Rigidbody2D>();
        target = PlayerStats.instance;

        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y);
        
    }

}
