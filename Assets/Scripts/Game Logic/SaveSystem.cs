using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/save.json";
    public static void SavePlayer(PlayerStats stats, Vector3 bonfirePosition)
    {
        PlayerData data = new PlayerData
        {
            maxHealthLevel = stats.maxHealthLevel,
            maxHealth = stats.maxHealth,
            health = stats.health,
            maxManaLevel = stats.maxManaLevel,
            maxMana = stats.maxMana,
            mana = stats.mana,
            maxPotionsLevel = stats.maxPotionsLevel,
            maxPotions = stats.maxPotions,
            potions = stats.potions,
            amountRestaurationLevel = stats.amountRestaurationLevel,
            amountRestauration = stats.amountRestauration,
            doubleJumpUnlock = stats.doubleJumpUnlock,
            dodgeUnlocked = stats.dodgeUnlocked,
            soulsAmount = stats.soulsAmount,
            comboDamages = stats.comboDamages,
            comboDamageLevel = stats.comboDamageLevel,
            airAttackLevel = stats.airAttackLevel,
            specialAttackDamageLevel = stats.specialAttackDamageLevel,

            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            bonfireX = bonfirePosition.x,
            bonfireY = bonfirePosition.y,
            bonfireZ = bonfirePosition.z
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Datos guardados en: " + savePath);
    }

    public static void LoadPlayer(PlayerStats stats, Transform playerTransform)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No se encontró archivo de guardado.");
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        stats.maxHealthLevel = data.maxHealthLevel;
        stats.maxHealth = data.maxHealth;
        stats.health = data.health;

        stats.maxManaLevel = data.maxManaLevel;
        stats.maxMana = data.maxMana;
        stats.mana = data.mana;

        stats.maxPotionsLevel = data.maxPotionsLevel;
        stats.maxPotions = data.maxPotions;
        stats.potions = data.potions;

        stats.amountRestaurationLevel = data.amountRestaurationLevel;
        stats.amountRestauration = data.amountRestauration;

        stats.doubleJumpUnlock = data.doubleJumpUnlock;
        stats.dodgeUnlocked = data.dodgeUnlocked;

        stats.soulsAmount = data.soulsAmount;

        stats.comboDamages = data.comboDamages;
        stats.comboDamageLevel = data.comboDamageLevel;
        stats.airAttackLevel = data.airAttackLevel;
        stats.specialAttackDamageLevel = data.specialAttackDamageLevel;

        playerTransform.position = new Vector3(data.bonfireX, data.bonfireY, data.bonfireZ);

        Debug.Log("Datos cargados desde JSON.");
    }

    public static string GetSavedSceneName()
    {
        if (!File.Exists(savePath))
            return null;

        string json = File.ReadAllText(savePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        return data.sceneName;
    } 

    public static Vector3 GetSavedPlayerPosition()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No se encontró el archivo de guardado.");
            return Vector3.zero;
        }

        string json = File.ReadAllText(savePath);
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        return new Vector3(data.bonfireX, data.bonfireY, data.bonfireZ);
    }   
}

