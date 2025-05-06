using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaxHealthUpgradeButton : MonoBehaviour
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
            Debug.LogWarning("Faltan referencias en MaxHealthUpgradeButton");
            return;
        }

        int currentLevel = playerStats.maxHealthLevel;

        if (currentLevel >= upgradeManager.upgradesData.maxHealth.Count)
        {
            buttonText.text = "Health : MAX";
            upgradeButton.interactable = false;
            return;
        }

        var nextUpgrade = upgradeManager.upgradesData.maxHealth[currentLevel];

        buttonText.text = $"Upgrade Max Health to {nextUpgrade.value}";
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
        int currentLevel = playerStats.maxHealthLevel;
        
        if (currentLevel >= upgradeManager.upgradesData.maxHealth.Count)
            return;

        var nextUpgrade = upgradeManager.upgradesData.maxHealth[currentLevel];

        if (playerStats.soulsAmount >= nextUpgrade.cost)
        {
            playerStats.soulsAmount -= nextUpgrade.cost;
            playerStats.maxHealthLevel++;
            playerStats.maxHealth = nextUpgrade.value;

            UpdateButton();
        }
    }
}