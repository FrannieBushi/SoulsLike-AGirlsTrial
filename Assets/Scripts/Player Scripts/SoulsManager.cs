using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SoulsManager : MonoBehaviour
{
    PlayerStats playerStats;
    private Text soulsText; 

    public void Init(Text soulsLabel)
    {
        soulsText = soulsLabel;
        UpdateSouls(); 
    }

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (soulsText != null)
        {
            UpdateSouls();
        }
    }

    public void AddSouls(int amount)
    {
        playerStats.soulsAmount += amount;
    }

    public void UpdateSouls()
    {
        if (soulsText != null)
        {
            soulsText.text = "X" + playerStats.soulsAmount.ToString(CultureInfo.InvariantCulture);
        }
    }
}