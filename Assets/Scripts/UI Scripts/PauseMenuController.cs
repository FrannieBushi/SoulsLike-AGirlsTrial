using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private PlayerInput playerInput;
    private bool isPaused = false;
    public GameObject firstSelected;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("No se encontró PlayerInput en el GameObject.");
        }
    }

    void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.onActionTriggered += OnActionTriggered;
        }
    }

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        // Detecta la acción "Pause" en cualquier mapa activo
        if (context.action.name == "Pause" && context.performed)
        {
            Debug.Log("Se pulsó Pause");
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        playerInput.SwitchCurrentActionMap("Player");
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        playerInput.SwitchCurrentActionMap("UI");

        EventSystem.current.SetSelectedGameObject(null); 
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void Close()
    {
        Application.Quit();
    }
}