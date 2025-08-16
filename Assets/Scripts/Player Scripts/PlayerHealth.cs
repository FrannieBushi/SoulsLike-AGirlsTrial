using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    private Image healthImage; 
    bool isInmune;
    public float InmunityTime;
    Blink material;
    SpriteRenderer sprite;
    public float knockBackForceX;
    public float knockBackForceY;
    Rigidbody2D rb;
    PotionManagerController potionManager;
    PlayerAnimationManager animationManager;
    CombatController combatController;
    public AudioSource audioSource;
    public AudioClip sound;
    PlayerStats playerStats;

    public void Init(Image healthBarImage)
    {
        healthImage = healthBarImage;
        if (healthImage != null)
        {
            healthImage.fillAmount = playerStats.health / playerStats.maxHealth;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        material = GetComponent<Blink>();
        playerStats = GetComponent<PlayerStats>();
        playerStats.health = playerStats.maxHealth;
        potionManager = GetComponent<PotionManagerController>();  
        potionManager.heal.AddListener(HealedByPotion);
        animationManager = GetComponent<PlayerAnimationManager>();
        combatController = GetComponent<CombatController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (playerStats.health > playerStats.maxHealth)
        {
            playerStats.health = playerStats.maxHealth;
        }

        if (healthImage != null)
        {
            healthImage.fillAmount = playerStats.health / playerStats.maxHealth;
        }
    }

    private void HealedByPotion(int amountRestauration)
    {
        playerStats.health += amountRestauration;
    }

    public void RestoreHealth(float healthRestored)
    {
        if (playerStats.health + healthRestored > playerStats.maxHealth)
        {
            playerStats.health = playerStats.maxHealth;
        }
        else
        {
            playerStats.health += healthRestored;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isInmune)
        {
            playerStats.health -= collision.GetComponent<Enemy>().damageToGive;
            StartCoroutine(Inmunity());

            if (collision.transform.position.x > transform.position.x)
            {
                rb.AddForce(new Vector2(-knockBackForceX, knockBackForceY), ForceMode2D.Force);
            }
            else
            {
                rb.AddForce(new Vector2(knockBackForceX, knockBackForceY), ForceMode2D.Force);
            }

            if (playerStats.health <= 0)
            {
                SceneManager.LoadScene("DeathScene");
            }
        }
    }

    public void SoundOfDamage()
    {
        audioSource.PlayOneShot(sound);    
    }

    IEnumerator Inmunity()
    {
        isInmune = true;
        combatController.CancelAttack();
        if (!animationManager.isJumping && !animationManager.isFalling)
        {
            animationManager.isHurted = true;
        }
        sprite.material = material.blink;
        yield return new WaitForSeconds(InmunityTime);
        sprite.material = material.original;
        isInmune = false;
    }
}