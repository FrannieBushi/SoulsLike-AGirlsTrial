using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    Enemy enemy;
    //public bool isDamaged;

    public bool hasDeathAnimation;
    public GameObject deathEffect;
    SpriteRenderer sprite;
    Blink material;
    Rigidbody2D rb;
    Animator anim;
    public CapsuleCollider2D bodyCollider;
    public BoxCollider2D damageCollider;
    string lastAttackID = "no attack";
    public AudioSource audioSource;
    public AudioClip sound;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        material = GetComponent<Blink>();
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon")  /* && !isDamaged*/)
        {
            WeaponHitbox weapon = collision.GetComponent<WeaponHitbox>();
            if (!weapon.attackID.Equals(lastAttackID))
            {
                lastAttackID = weapon.attackID;

                float damage = weapon.GetDamage();
                Debug.Log("Daño realizado: " + damage);
                enemy.healthPoints -= damage;

                if (collision.transform.position.x < transform.position.x)
                {
                    rb.AddForce(new Vector2(enemy.knockBackForceX, enemy.knockBackForceY), ForceMode2D.Force);
                }
                else
                {
                    rb.AddForce(new Vector2(-enemy.knockBackForceX, enemy.knockBackForceY), ForceMode2D.Force);
                }

                StartCoroutine(Damager());

                if (enemy.healthPoints <= 0 && !hasDeathAnimation && !enemy.isBoss)
                {
                    audioSource.PlayOneShot(sound); 
                    enemy.DropSoul();
                    Instantiate(deathEffect, transform.position, Quaternion.identity);
                    gameObject.SetActive(false);
                }

                if (enemy.healthPoints <= 0 && hasDeathAnimation)
                {
                    audioSource.PlayOneShot(sound); 
                    gameObject.tag = "EnemyDead";
                    anim.SetBool("death", true);
                }
            }
        }
    }

    public void DisableColliders()
    {
        if(damageCollider != null) damageCollider.enabled = false;
    }

    public void desactivateEnemy()
    {
        enemy.DropSoul();
        gameObject.SetActive(false);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
        enemy.DropSoul();
    }
    
    //Metodo para evitar que se dañe a un enemigo varias veces en la misma animacion de ataque:
    IEnumerator Damager()
    {
        //isDamaged = true;
        sprite.material = material.blink;
        yield return new WaitForSeconds(0.2f);
        //isDamaged = false;
        sprite.material = material.original;
    }
}
