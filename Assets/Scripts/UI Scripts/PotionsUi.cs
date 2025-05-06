using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PotionsUi : MonoBehaviour
{

    public List<Image> potionsList;
    public GameObject potionPrefab;
    public PotionManagerController potionManager;
    public int actualIndex;
    public Sprite fullPotion;
    public Sprite emptyPotion;

    void Awake()
    {
        potionManager.potionsChange.AddListener(ChangePotions);   
    }

    private void ChangePotions(int potions)
    {
        if(!potionsList.Any())
        {
            CreatePotions(potions);    
        }
        else
        {
            ChangeAmountPotions(potions);
        }
    }

    private void CreatePotions(int maxPotions)
    {
        for (int i = 0; i < maxPotions; i++)
        {
            GameObject potion = Instantiate(potionPrefab, transform);

            potionsList.Add(potion.GetComponent<Image>());   
        }

        actualIndex = maxPotions - 1;    
    }

    private void ChangeAmountPotions(int actualPotions)
    {

        if(actualPotions <= actualIndex)
        {
            RemovePotion(actualPotions);
        }
        else
        {
            AddPotions(actualPotions);
        }
        
    }

    private void RemovePotion(int actualPotions)
    {
        for (int i = actualIndex; i >= actualPotions; i--)
        {
            actualIndex = i;
            potionsList[actualIndex].sprite = emptyPotion;
        }
        
    }

    private void AddPotions(int actualPotions)
    {
        for (int i = actualIndex; i < actualPotions; i++)
        {
            actualIndex = i;
            potionsList[actualIndex].sprite = fullPotion;
        }
        
    }

}
