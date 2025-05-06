using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboDamageUpgradeBtn : MonoBehaviour
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
            return;

        int currentLevel = playerStats.comboDamageLevel;

        if (currentLevel >= upgradeManager.upgradesData.comboDamages.Count)
        {
            buttonText.text = "Combo Damage: MAX";
            upgradeButton.interactable = false;
            return;
        }

        var nextUpgrade = upgradeManager.upgradesData.comboDamages[currentLevel];
        buttonText.text = $"Upgrade Combo damage to : {nextUpgrade.values[0]} / {nextUpgrade.values[1]} / {nextUpgrade.values[2]}";
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
        int currentLevel = playerStats.comboDamageLevel;

        if (currentLevel >= upgradeManager.upgradesData.comboDamages.Count)
            return;

        var nextUpgrade = upgradeManager.upgradesData.comboDamages[currentLevel];

        if (playerStats.soulsAmount >= nextUpgrade.cost)
        {
            playerStats.soulsAmount -= nextUpgrade.cost;
            playerStats.comboDamageLevel++;

            for (int i = 0; i < playerStats.comboDamages.Length; i++)
            {
                playerStats.comboDamages[i] = (int)nextUpgrade.values[i];
            }

            UpdateButton();
        }
    }
}
