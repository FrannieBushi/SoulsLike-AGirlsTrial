
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
public class UpgradeMenuController : MonoBehaviour
{
    public static UpgradeMenuController instance;

    public GameObject firstSelected;
    public PlayerInput playerInput;
    private InputAction cancelAction;
    public GameObject player;

    void Awake()
    {
        bool destroy = instance != null && instance != this;

        if (!destroy)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
         if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj;
            }
            else
            {
                Debug.LogWarning("no player");
                return;
            }
        }

        if (playerInput == null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                Debug.LogWarning("no player input");
                return;
            }
        }

        playerInput.SwitchCurrentActionMap("UI");

        cancelAction = playerInput.actions["Cancel"];
        cancelAction.performed += OnCancel;
        cancelAction.Enable();

        player.GetComponent<JumpController>().enabled = false;
        player.GetComponent<DodgeController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PotionManagerController>().enabled = false;
        player.GetComponent<CombatController>().enabled = false;
        
        Time.timeScale = 0f;

    }

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void OnDisable()
    {
        playerInput.SwitchCurrentActionMap("Player");

        cancelAction.performed -= OnCancel;
        cancelAction.Disable();
        
        player.GetComponent<PlayerStats>().PausePlayer();
        
        Time.timeScale = 1f;
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        CloseMenu();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false); 
    }

}
