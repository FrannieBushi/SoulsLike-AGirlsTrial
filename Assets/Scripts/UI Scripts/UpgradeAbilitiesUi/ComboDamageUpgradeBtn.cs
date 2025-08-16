using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ComboDamageUpgradeBtn : MonoBehaviour
{
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI buttonTextCost;

    public PlayerStats playerStats;
    public UpgradeManager upgradeManager;

    void Start()
    {
        StartCoroutine(WaitForDependenciesAndInitialize());
        upgradeButton = GetComponent<Button>();
        //UpdateButton();
        //upgradeButton.onClick.AddListener(ApplyUpgrade);
    }

    void Update()
    {
        if (upgradeManager != null &&
        upgradeManager.upgradesData != null &&
        playerStats != null)
        {
            UpdateButton();
        }
    }

    void UpdateButton()
    {

        int currentLevel = playerStats.comboDamageLevel;

        if (currentLevel >= upgradeManager.upgradesData.comboDamages.Count)
        {
            buttonText.text = "Combo damage : MAX";
            buttonText.color = Color.gray;
            buttonTextCost.text = "";

            upgradeButton.interactable = true;
            upgradeButton.onClick.RemoveAllListeners();
            return;
        }

        var nextUpgrade = upgradeManager.upgradesData.comboDamages[currentLevel];

        buttonText.text = $"Upgrade combo damages to [{string.Join(", ", nextUpgrade.values)}]";
        buttonTextCost.text = $"X {nextUpgrade.cost}";

        if (playerStats.soulsAmount >= nextUpgrade.cost)
        {
            buttonText.color = Color.black;
            buttonTextCost.color = Color.black;
            upgradeButton.interactable = true;

            // Evita añadir listeners duplicados
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(ApplyUpgrade);
        }
        else
        {
            buttonText.color = Color.red;
            buttonTextCost.color = Color.red;

            upgradeButton.interactable = true; // sigue navegable
            upgradeButton.onClick.RemoveAllListeners(); // no hace nada si se pulsa
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
    
    private IEnumerator WaitForDependenciesAndInitialize()
    {
        while (playerStats == null)
        {
            playerStats = PlayerStats.instance;
            yield return null;
        }


        while (upgradeManager == null || upgradeManager.upgradesData == null)
        {
            upgradeManager = FindAnyObjectByType<UpgradeManager>();
            yield return null;
        }


        upgradeButton = GetComponent<Button>();
        UpdateButton();
        upgradeButton.onClick.AddListener(ApplyUpgrade);
    }

}
