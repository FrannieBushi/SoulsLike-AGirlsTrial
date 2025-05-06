using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AirAttackUpgradeBtn : MonoBehaviour
{
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI buttonTextCost;

    public PlayerStats playerStats;
    public UpgradeManager upgradeManager;

    void Start()
    {
        upgradeButton = GetComponent<Button>();
        UpdateButton();
        upgradeButton.onClick.AddListener(ApplyUpgrade);
    }

    void Update()
    {
        UpdateButton();
    }

    void UpdateButton()
    {
        if (playerStats == null || upgradeManager == null || upgradeManager.upgradesData == null)
        {
            Debug.LogWarning("Faltan referencias en air");
            return;
        }

        int currentLevel = playerStats.airAttackLevel;

        if (currentLevel >= upgradeManager.upgradesData.airAttackDamage.Count)
        {
            buttonText.text = "Air Attack : MAX";
            upgradeButton.interactable = false;
            return;
        }

        var nextUpgrade = upgradeManager.upgradesData.airAttackDamage[currentLevel];

        buttonText.text = $"Upgrade the damage of air attacks  {nextUpgrade.value}";
        buttonTextCost.text = $"X {nextUpgrade.cost}";

        if (playerStats.soulsAmount >= nextUpgrade.cost)
        {
            buttonText.color = Color.black;
            buttonTextCost.color = Color.black;
            upgradeButton.interactable = true;
        }
        else
        {
            buttonText.color = Color.red;
            buttonTextCost.color = Color.red;
            upgradeButton.interactable = false;
        }
    }

    void ApplyUpgrade()
    {
        int currentLevel = playerStats.airAttackLevel;
        
        if (currentLevel >= upgradeManager.upgradesData.airAttackDamage.Count)
            return;

        var nextUpgrade = upgradeManager.upgradesData.airAttackDamage[currentLevel];

        if (playerStats.soulsAmount >= nextUpgrade.cost)
        {
            playerStats.soulsAmount -= nextUpgrade.cost;
            playerStats.airAttackLevel++;
            playerStats.airAttackDamage = (int)nextUpgrade.value;

            UpdateButton();
        }
    }
}
