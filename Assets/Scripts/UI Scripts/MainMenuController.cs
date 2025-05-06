using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour
{
    public Button newGameButton;
    public Button continueButton;
    public Button optionsButton;
    public Button exitButton;

    public string firstSceneName = "CinematicIntro";

    private List<Button> menuButtons;
    private int currentIndex = 0;
    private string savePath;

    AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        savePath = Application.persistentDataPath + "/save.json";

        if (!File.Exists(savePath))
        {
            continueButton.interactable = false;
        }

        menuButtons = new List<Button> { newGameButton, continueButton, optionsButton, exitButton };

        SelectButton(currentIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveSelection(-1);
            audioSource.PlayOneShot(audioClip);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveSelection(1);
            audioSource.PlayOneShot(audioClip);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PressSelectedButton();
            audioSource.PlayOneShot(audioClip);
        }
    }

    void MoveSelection(int direction)
    {
        int previousIndex = currentIndex;

        do
        {
            currentIndex = (currentIndex + direction + menuButtons.Count) % menuButtons.Count;
        }
        while (!menuButtons[currentIndex].interactable && currentIndex != previousIndex);

        SelectButton(currentIndex);
    }

    void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(menuButtons[index].gameObject);
    }

    void PressSelectedButton()
    {
        menuButtons[currentIndex].onClick.Invoke();
    }

    public void OnNewGame()
    {
        Debug.Log("Intentando cargar escena: " + firstSceneName);
        
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager no encontrado.");
        }

        else
        {
            GameManager.instance.StartNewGame();
        }

        SceneManager.LoadScene(firstSceneName);
    }

    public void OnContinue()
    {
        if (File.Exists(savePath))
        {
            GameManager.instance.ContinueGame();
            string sceneToLoad = SaveSystem.GetSavedSceneName();

            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("No se encontró el nombre de la escena en el guardado.");
            }
        }
    }

    public void OnOptions()
    {
        Debug.Log("Opciones aún no implementadas.");
    }

    public void OnExit()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }
       
}
