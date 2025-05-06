using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SoulsManager : MonoBehaviour
{

    PlayerStats playerStats;
    public Text soulsText;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();    
    }

    void Update()
    {
        UpdateSouls();    
    }

    public void AddSouls(int amount)
    {
        playerStats.soulsAmount += amount;
    }

    public void UpdateSouls()
    {
        soulsText.text = "X" + playerStats.soulsAmount;
    }
}
