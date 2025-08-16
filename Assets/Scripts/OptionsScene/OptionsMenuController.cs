using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Audio;
using TMPro;

public class OptionsMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Button windowedButton;
    public Button backButton;

    private TextMeshProUGUI windowedButtonText;
    private bool isWindowed;

    private List<Selectable> menuItems;
    private int currentIndex = 0;
    private float inputCooldown = 0.2f;
    private float lastInputTime;

    private PlayerInputActions inputActions;

    [SerializeField] private AudioMixer audioMixer;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Ui.Submit.performed += OnSubmit;
    }

    void OnDisable()
    {
        inputActions.Ui.Submit.performed -= OnSubmit;
        inputActions.Disable();
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f); 
        volumeSlider.value = savedVolume;
        ChangeVolume(savedVolume);

        isWindowed = PlayerPrefs.GetInt("windowed", 1) == 1;
        Screen.fullScreen = !isWindowed;

        windowedButtonText = windowedButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateWindowedButtonText();

        menuItems = new List<Selectable> { volumeSlider, windowedButton, backButton };
        SelectItem(currentIndex);
    }

    void Update()
    {
        Vector2 nav = inputActions.Ui.Navigate.ReadValue<Vector2>();

        if (Time.time - lastInputTime > inputCooldown)
        {
            if (nav.y > 0.5f)
            {
                MoveSelection(-1);
                lastInputTime = Time.time;
            }
            else if (nav.y < -0.5f)
            {
                MoveSelection(1);
                lastInputTime = Time.time;
            }
        }
    }

    void MoveSelection(int direction)
    {
        int previousIndex = currentIndex;

        do
        {
            currentIndex = (currentIndex + direction + menuItems.Count) % menuItems.Count;
        }
        while (!menuItems[currentIndex].IsInteractable() && currentIndex != previousIndex);

        SelectItem(currentIndex);
    }

    void SelectItem(int index)
    {
        EventSystem.current.SetSelectedGameObject(menuItems[index].gameObject);
    }

    void OnSubmit(InputAction.CallbackContext context)
    {
        var selected = menuItems[currentIndex];

        if (selected is Button button)
        {
            button.onClick.Invoke();
        }
    }

    public void ChangeVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("Volume", dB);

        PlayerPrefs.SetFloat("volume", value); 
        PlayerPrefs.Save(); 
    }

    public void ToggleWindowedMode()
    {
        isWindowed = !isWindowed;
        Screen.fullScreen = !isWindowed;
        PlayerPrefs.SetInt("windowed", isWindowed ? 1 : 0);
        UpdateWindowedButtonText();
    }

    private void UpdateWindowedButtonText()
    {
        if (windowedButtonText != null)
        {
            windowedButtonText.text = "Windowed: " + (isWindowed ? "Yes" : "No");
            windowedButtonText.color = isWindowed ? Color.green : Color.red;
        }
    }

    public void OnBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}