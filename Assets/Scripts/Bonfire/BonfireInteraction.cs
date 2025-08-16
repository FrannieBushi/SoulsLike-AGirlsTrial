using UnityEngine;
using System.Linq;

public class BonfireInteraction : MonoBehaviour
{
    public GameObject interactIcon;
    public GameObject upgradeMenu;

    private bool isPlayerNear = false;
    private PlayerStats playerStats;
    private bool enteredToMenu = false;

    public Sprite keyboardSprite;
    public Sprite playstationSprite;
    public Sprite xboxSprite;

    private PlayerInputHandler inputHandler;

    void Start()
    {
        interactIcon.SetActive(false);

        if (UpgradeMenuController.instance != null)
        {
            upgradeMenu = UpgradeMenuController.instance.gameObject;
            upgradeMenu.SetActive(false);
        }

        playerStats = PlayerStats.instance;
        inputHandler = PlayerStats.instance?.GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        if (upgradeMenu == null)
            TryFindUpgradeMenu();

        UpdateIcon();

        bool canInteract = !PauseMenuController.GameIsPaused && inputHandler != null;

        if (canInteract && isPlayerNear && inputHandler.interactionPressed)
        {
            inputHandler.ResetInputs();

            playerStats.RestoreAll();
            SaveSystem.SavePlayer(playerStats, transform.position);
            Debug.Log("Interacción con la hoguera");

            var respawnManager = Object.FindFirstObjectByType<EnemyRespawnManager>();
            if (respawnManager != null)
            {
                respawnManager.RespawnEnemies();
            }

            if (upgradeMenu != null)
            {
                enteredToMenu = true;
                PauseMenuController.instance.enabled = false;
                upgradeMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        if (canInteract && enteredToMenu && inputHandler.cancelPressed)
        {
            enteredToMenu = false;
            PauseMenuController.instance.enabled = true;
            upgradeMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    void UpdateIcon()
    {
        if (isPlayerNear && LastInputDetector.instance != null)
        {
            switch (LastInputDetector.instance.LastDeviceUsed)
            {
                case LastInputDetector.InputDeviceType.KeyboardMouse:
                    interactIcon.GetComponent<SpriteRenderer>().sprite = keyboardSprite;
                    break;
                case LastInputDetector.InputDeviceType.PlayStation:
                    interactIcon.GetComponent<SpriteRenderer>().sprite = playstationSprite;
                    break;
                case LastInputDetector.InputDeviceType.Xbox:
                    interactIcon.GetComponent<SpriteRenderer>().sprite = xboxSprite;
                    break;
            }
        }
    }

    void TryFindUpgradeMenu()
    {
        UpgradeMenuController found = Resources.FindObjectsOfTypeAll<UpgradeMenuController>()
        .FirstOrDefault(x => x.gameObject.hideFlags == HideFlags.None);

        if (found != null)
        {
            upgradeMenu = found.gameObject;
            upgradeMenu.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactIcon.SetActive(true);
            isPlayerNear = true;

            playerStats = PlayerStats.instance;
            inputHandler = PlayerStats.instance?.GetComponent<PlayerInputHandler>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactIcon.SetActive(false);
            isPlayerNear = false;
            playerStats = null;
            inputHandler = null;
        }
    }
}