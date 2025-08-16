using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats instance;

    public int maxHealthLevel;
    public float maxHealth;
    public float health;

    public int maxManaLevel;
    public float maxMana;
    public float mana;

    public int maxPotionsLevel;
    public int maxPotions;
    public int potions;

    public int amountRestaurationLevel;
    public int amountRestauration;

    public bool doubleJumpUnlock;

    public int soulsAmount;

    public int comboDamageLevel;
    public int[] comboDamages = new int[3] { 10, 20, 35 };
    public int airAttackLevel;
    public int airAttackDamage = 15;
    public int specialAttackDamageLevel;
    public int specialAttackDamage = 75;

    public bool dodgeUnlocked;

    PlayerHealth playerHealth;
    PlayerMana playerMana;
    PotionManagerController potionManager;
    public PlayerInput playerInput;
    public PlayerController playerController;

    void Awake()
    {
        bool destroy = instance != null && instance != this;

        if (!destroy)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if (destroy)
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMana = GetComponent<PlayerMana>();
        potionManager = GetComponent<PotionManagerController>();
        playerInput = GetComponent<PlayerInput>();

        if (PlayerPrefs.GetInt("loadedFromSave", 0) == 0)
        {
            maxHealthLevel = 0;
            maxManaLevel = 0;
            maxPotionsLevel = 0;
            amountRestaurationLevel = 0;
            comboDamageLevel = 0;
            airAttackLevel = 0;
            specialAttackDamageLevel = 0;
        }

        playerController = GetComponent<PlayerController>();      
    }

    public void RestoreAll()
    {
        playerHealth.RestoreHealth(maxHealth);
        playerMana.restoreMana(maxMana);
        potionManager.RestorePotions(maxPotions);
    }

    public void PausePlayer()
    {
        StartCoroutine(ResumeAfterDelay());
    }

    private IEnumerator ResumeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        GetComponent<JumpController>().enabled = true;
        GetComponent<DodgeController>().enabled = true;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<PotionManagerController>().enabled = true;
        GetComponent<CombatController>().enabled = true;

        playerInput.SwitchCurrentActionMap("Player");    
        GetComponent<PlayerInputHandler>().ResetInputs();       
    }
}
