using UnityEngine;

public class PlayerDetect : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            var enemy = transform.GetComponentInParent<EnemyProjectile>();

            if(!enemy.hasShootAnimation && enemy.watcher)
            {
                enemy.Shoot();
            }
            else if(enemy.hasShootAnimation && enemy.watcher)
            {
                enemy.GetComponent<Animator>().SetBool("attackshoot", true);
            }
        }
    }
}
