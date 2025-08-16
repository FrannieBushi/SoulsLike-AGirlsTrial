using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    public float maxMana;
    public float mana;

    public PlayerStats playerStats;

    private Image manaImage; 

    public void Init(Image manaBarImage)
    {
        manaImage = manaBarImage;
        if (manaImage != null)
        {
            manaImage.fillAmount = playerStats.mana / playerStats.maxMana;
        }
    }

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerStats.mana = playerStats.maxMana;
    }

    void Update()
    {
        if (manaImage != null)
        {
            manaImage.fillAmount = playerStats.mana / playerStats.maxMana;
        }
    }

    public void useMana(int amountMana)
    {
        playerStats.mana -= amountMana;
    }

    public void restoreMana(float amountMana)
    {
        playerStats.mana += amountMana;
        if (playerStats.mana > playerStats.maxMana)
        {
            playerStats.mana = playerStats.maxMana;
        }
    }
}
