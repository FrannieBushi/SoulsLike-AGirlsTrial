using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class SpecialAttackUpgradeBtn : MonoBehaviour
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
        int currentLevel = playerStats.specialAttackDamageLevel;

        if (currentLevel >= upgradeManager.upgradesData.specialAttackDamage.Count)
        {
            buttonText.text = "Special Attack : MAX";
            buttonText.color = Color.gray;
            buttonTextCost.text = "";

            upgradeButton.interactable = true; // sigue navegable
            upgradeButton.onClick.RemoveAllListeners(); // no hace nada si se pulsa
            return;
        }

        var nextUpgrade = upgradeManager.upgradesData.specialAttackDamage[currentLevel];

        buttonText.text = $"Upgrade special attack damage to {nextUpgrade.value}";
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
        int currentLevel = playerStats.specialAttackDamageLevel;

        if (currentLevel >= upgradeManager.upgradesData.specialAttackDamage.Count)
            return;

        var nextUpgrade = upgradeManager.upgradesData.specialAttackDamage[currentLevel];

        if (playerStats.soulsAmount >= nextUpgrade.cost)
        {
            playerStats.soulsAmount -= nextUpgrade.cost;
            playerStats.specialAttackDamageLevel++;
            playerStats.specialAttackDamage = (int)nextUpgrade.value;

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
