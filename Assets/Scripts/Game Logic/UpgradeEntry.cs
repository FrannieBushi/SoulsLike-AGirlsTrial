
using System.Collections.Generic;

[System.Serializable]
public class UpgradeEntry 
{
    public int level;
    public float value;
    public int cost;  
}

[System.Serializable]
public class ComboUpgradeEntry
{
    public int level;
    public float[] values;
    public int cost;
}

[System.Serializable]
public class UpgradesData
{
    public List<UpgradeEntry> maxHealth;
    public List<UpgradeEntry> maxMana;
    public List<UpgradeEntry> amountRestauration;
    public List<ComboUpgradeEntry> comboDamages;
    public List<UpgradeEntry> airAttackDamage;
    public List<UpgradeEntry> specialAttackDamage;
    
}

[System.Serializable]
public class PlayerData
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
    public bool dodgeUnlocked;

    public int soulsAmount;

    public int[] comboDamages;
    public int comboDamageLevel;
    public int airAttackLevel;
    public int specialAttackDamageLevel;

    public string sceneName;
    public float bonfireX;
    public float bonfireY;
    public float bonfireZ;
}

[System.Serializable]
public class UpgradeWrapper
{
    public UpgradesData upgrades;
}
