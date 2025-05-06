using UnityEngine;

public class PlayerStats : MonoBehaviour
{

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

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMana = GetComponent<PlayerMana>();
        potionManager= GetComponent<PotionManagerController>();

        maxHealthLevel = 0;
        maxManaLevel = 0; 
        maxPotionsLevel = 0;
        amountRestaurationLevel = 0;
        comboDamageLevel = 0;
        airAttackLevel = 0;
        specialAttackDamageLevel = 0;      
    }

    public void RestoreAll()
    {
        playerHealth.RestoreHealth(maxHealth);
        playerMana.restoreMana(maxMana);
        potionManager.RestorePotions(maxPotions);
    }

}
