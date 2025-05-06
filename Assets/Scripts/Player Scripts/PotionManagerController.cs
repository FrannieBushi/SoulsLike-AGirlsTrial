using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PotionManagerController : MonoBehaviour
{
    public int maxPotions;
    public int potions;
    [SerializeField] private int amountRestauration = 30;

    PlayerAnimationManager animationManager;
    PlayerController playerController;
    PlayerHealth playerHealth;
    public AudioSource audioSource;
    public AudioClip sound;

    PlayerStats playerStats;

    public UnityEvent<int> potionsChange;
    public UnityEvent<int> heal;

    [SerializeField] private PlayerInputHandler inputHandler;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerStats.potions= playerStats.maxPotions;

        animationManager = GetComponent<PlayerAnimationManager>();
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();

        potionsChange.Invoke(playerStats.potions);
        inputHandler = GetComponent<PlayerInputHandler>();

    }

    void Update()
    {

        if(playerStats.potions > playerStats.maxPotions)
        {
            playerStats.potions = playerStats.maxPotions;
        }

        if (inputHandler.usePotionPressed)
        {
            inputHandler.ResetInputs();
            UsePotion();
        }
    }

    void UsePotion()
    {
        if ((playerStats.potions > 0) && animationManager.isJumping == false && animationManager.isFalling == false)
        {
            animationManager.isDrinking = true;
            StartCoroutine(DrinkPotionRoutine());
            potionsChange.Invoke(playerStats.potions);
        }    
    }

    public void RestorePotions(int amountPotions)
    {
        if(amountPotions + playerStats.potions >= playerStats.maxPotions)
        {
            playerStats.potions = playerStats.maxPotions;
        }

        else
        {
            playerStats.potions += amountPotions;
        }

        potionsChange.Invoke(playerStats.potions);
    }

    public void SoundOfDrinkingOn()
    {
        audioSource.PlayOneShot(sound);    
    }

    IEnumerator DrinkPotionRoutine()
    {
        playerController.enabled = false;
        playerStats.potions--;
        heal.Invoke(amountRestauration);
        yield return new WaitForSeconds(0.50f);
        playerController.enabled = true;
        animationManager.isDrinking = false;     
    }  
}
