using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController instance;

    public GameObject pauseMenuUI;
    private PlayerInput playerInput;
    private bool isPaused = false;
    public GameObject firstSelected;
    public Slider volumeSlider;
    public static bool GameIsPaused { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    void Awake()
    {
        bool destroy = instance != null && instance != this;

        if (!destroy)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            playerInput = GetComponent<PlayerInput>();
        }

        if (destroy)
        {
            Destroy(gameObject);
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
        if (context.action.name == "Pause" && context.performed)
        {
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
        isPaused = false;
        GameIsPaused = false;

        GetComponent<PlayerInputHandler>()?.ResetInputs();

        StartCoroutine(ResumeAfterDelay());

        Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        GameIsPaused = true;

        GetComponent<JumpController>().enabled = false;
        GetComponent<DodgeController>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PotionManagerController>().enabled = false;
        GetComponent<CombatController>().enabled = false;

        playerInput.SwitchCurrentActionMap("UI");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void Close()
    {
        Application.Quit();

        //SceneManager.LoadScene("Transition"); --> Error por dontdestroyonload;
    }

    private IEnumerator ResumeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        GetComponent<JumpController>().enabled = true;
        GetComponent<DodgeController>().enabled = true;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<PotionManagerController>().enabled = true;
        GetComponent<CombatController>().enabled = true;

        playerInput.SwitchCurrentActionMap("Player");

    }
    
    public void ChangeVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("Volume", dB);

        PlayerPrefs.SetFloat("volume", value); 
        PlayerPrefs.Save(); 
    }
}