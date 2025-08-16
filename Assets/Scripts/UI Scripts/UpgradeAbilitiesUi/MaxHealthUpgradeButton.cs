using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MaxHealthUpgradeButton : MonoBehaviour
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
        
        int currentLevel = playerStats.maxHealthLevel;

        if (currentLevel >= upgradeManager.upgradesData.maxHealth.Count)
        {
            buttonText.text = "Health : MAX";
            buttonText.color = Color.gray;
            buttonTextCost.text = "";
            
            upgradeButton.interactable = true; // sigue navegable
            upgradeButton.onClick.RemoveAllListeners(); // no hace nada si se pulsa
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