using UnityEngine;

public class BonfireInteraction : MonoBehaviour
{
    public GameObject interactIcon;
    public GameObject upgradeMenu;
    public PauseMenuController pauseMenuController;
    private bool isPlayerNear = false;
    private PlayerStats playerStats;
    private bool enteredToMenu = false;

    void Start()
    {
        interactIcon.SetActive(false);

        
        if (upgradeMenu != null)
        {
            upgradeMenu.SetActive(false);
        }
            
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            playerStats.RestoreAll();
            SaveSystem.SavePlayer(playerStats, transform.position);
            Debug.Log("Interacción con la hoguera");

            if (EnemyRespawnManager.instance != null)
            {
                EnemyRespawnManager.instance.RespawnEnemies();
            }

            if (upgradeMenu != null)
            {
                enteredToMenu = true;
                pauseMenuController.enabled = false;
                upgradeMenu.SetActive(true);  
                Time.timeScale = 0f;
            }
        }

        if(enteredToMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            enteredToMenu = false;
            pauseMenuController.enabled = true;
            upgradeMenu.SetActive(false);
            Time.timeScale = 1f;
        }
           
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactIcon.SetActive(true);
            isPlayerNear = true;

            playerStats = other.GetComponent<PlayerStats>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactIcon.SetActive(false);
            isPlayerNear = false;
            playerStats = null;
        }
    }
}
