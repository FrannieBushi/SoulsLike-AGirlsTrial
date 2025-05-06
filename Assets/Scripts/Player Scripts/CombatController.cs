using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Animator anim;
    public int combo;
    public bool attacking;

    public PlayerAnimationManager animationManager;
    public WeaponHitbox weaponHitbox;

    public PlayerStats playerStats;
    public PlayerMana playerMana;
    public AudioSource audioSource;
    public AudioClip[] sound;

    public AudioClip jumpAttackSound;
    public AudioClip specialAttackSound;

    public int attackActivationID = 0;
    public bool canAirAttack;

    [SerializeField] private PlayerInputHandler inputHandler;

    void Start()
    {
        anim = GetComponent<Animator>();
        animationManager = GetComponent<PlayerAnimationManager>();
        playerStats = GetComponent<PlayerStats>();
        playerMana = GetComponent<PlayerMana>();
        audioSource = GetComponent<AudioSource>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        Combos();

        // Por si se queda pillada la animación
        if (attacking && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !anim.IsInTransition(0))
        {
            attacking = false;
            combo = 0;
        }
    }

    public void Combos()
    {
        // Ataque básico (combo o aéreo)
        if (inputHandler != null && inputHandler.attackPressed && !attacking && !animationManager.isHurted)
        {
            inputHandler.ResetInputs();

            if ((animationManager.isJumping || animationManager.isFalling) && canAirAttack)
            {
                GenerateAttackId();
                canAirAttack = false;
                anim.SetTrigger("AirAttack");
            }
            else if (!animationManager.isJumping && !animationManager.isFalling)
            {
                attacking = true;
                anim.SetTrigger("" + combo);
                GenerateAttackId();
            }
        }

        // Ataque especial
        if (inputHandler != null && inputHandler.specialAttackPressed && !attacking && !animationManager.isJumping && !animationManager.isFalling && !animationManager.isHurted)
        {
            inputHandler.ResetInputs();

            if (playerStats.mana >= 75)
            {
                GenerateAttackId();
                animationManager.specialAttack = true;
                playerMana.useMana(75);
            }
        }
    }

    public void StartCombo()
    {
        attacking = false;

        if (combo < 3)
        {
            combo++;
        }
    }

    public void FinishAnimation()
    {
        attacking = false;
        combo = 0;
    }

    public void FinishSpecialAttack()
    {
        animationManager.specialAttack = false;
    }

    public int GetCurrentAttackDamage()
    {
        if (animationManager.specialAttack)
            return playerStats.specialAttackDamage;

        if (animationManager.AirAttack)
            return playerStats.airAttackDamage;

        return playerStats.comboDamages[combo];
    }

    public void GenerateAttackId()
    {
        attackActivationID++;
        if (attackActivationID >= 1000000)
            attackActivationID = 0;

        weaponHitbox.SetAttackID();
        weaponHitbox.gameObject.SetActive(true);
    }

    public void CancelAttack()
    {
        attacking = false;
        combo = 0;
        anim.ResetTrigger("1");
        anim.ResetTrigger("2");
        anim.ResetTrigger("AirAttack");
        animationManager.specialAttack = false;
    }

    public void SoundOfComboOn()
    {
        audioSource.PlayOneShot(sound[combo]);
    }

    public void SoundOfSpecialAttack()
    {
        audioSource.PlayOneShot(specialAttackSound);
    }

    public void SoundOfJumpAttack()
    {
        audioSource.PlayOneShot(jumpAttackSound);
    }
}
