using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AmountRestaurationUpgradeBtn : MonoBehaviour
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
        int currentLevel = playerStats.amountRestaurationLevel;

        if (currentLevel >= upgradeManager.upgradesData.amountRestauration.Count)
        {
            buttonText.text = "Amount restoration : MAX";
            buttonText.color = Color.gray;
            buttonTextCost.text = "";

            upgradeButton.interactable = true; // sigue navegable
            upgradeButton.onClick.RemoveAllListeners(); // no hace nada si se pulsa
            return;
        }

        var nextUpgrade = upgradeManager.upgradesData.amountRestauration[currentLevel];

        buttonText.text = $"Upgrade amount restoration to {nextUpgrade.value}";
        buttonTextCost.text = $"X {nextUpgrade.cost}";

        if (playerStats.soulsAmount >= nextUpgrade.cost)
        {
            buttonText.color = Color.black;
            buttonTextCost.color = Color.black;
            upgradeButton.interactable = true;

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(ApplyUpgrade);
        }
        else
        {
            buttonText.color = Color.red;
            buttonTextCost.color = Color.red;

            upgradeButton.interactable = true; 
            upgradeButton.onClick.RemoveAllListeners(); 
        }
    }

    void ApplyUpgrade()
    {
        int currentLevel = playerStats.amountRestaurationLevel;

        if (currentLevel >= upgradeManager.upgradesData.amountRestauration.Count)
            return;

        var nextUpgrade = upgradeManager.upgradesData.amountRestauration[currentLevel];

        if (playerStats.soulsAmount >= nextUpgrade.cost)
        {
            playerStats.soulsAmount -= nextUpgrade.cost;
            playerStats.amountRestaurationLevel++;
            playerStats.amountRestauration = (int)nextUpgrade.value;

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
