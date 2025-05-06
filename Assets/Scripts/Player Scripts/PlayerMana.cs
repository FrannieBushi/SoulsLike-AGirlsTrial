
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public float maxMana;
    public float mana;

    public PlayerStats playerStats;

    public Image manaImage; 
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerStats.mana = playerStats.maxMana;   
    }

    void Update()
    {
        manaImage.fillAmount = playerStats.mana/playerStats.maxMana;   
    }

    public void useMana(int amountMana)
    {
        playerStats.mana -= amountMana;
    }

    public void restoreMana(float amountMana)
    {
        playerStats.mana += amountMana;
        if(playerStats.mana > playerStats.maxMana)
        {
            playerStats.mana = playerStats.maxMana;
        }
    }
}
