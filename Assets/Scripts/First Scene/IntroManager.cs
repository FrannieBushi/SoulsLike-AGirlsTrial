using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class DialogueData
{
    public string[] lines;
}

public class IntroManager : MonoBehaviour
{
    public Button btnEspañol;
    public Button btnEnglish;
    public GameObject languagePanel;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;

    private string[] lines;
    private int index;
    private bool isTyping = false;

    private Button[] languageButtons;
    private int selectedButtonIndex = 0;

    public AudioSource audioSource;
    public AudioClip typeSound;
    public AudioClip selectionSound;

    void Start()
    {
        dialogueText.text = "";
        languagePanel.SetActive(true);

        languageButtons = new Button[] { btnEspañol, btnEnglish };
        selectedButtonIndex = 0;
        EventSystem.current.SetSelectedGameObject(languageButtons[selectedButtonIndex].gameObject);

        audioSource = GetComponent<AudioSource>();

    }

    public void SelectLanguage(string langCode)
    {
        PlayerPrefs.SetString("language", langCode);
        Debug.Log("Botón pulsado. Idioma: " + langCode);
        languagePanel.SetActive(false);
        LoadDialogue(langCode);

        if (lines != null && lines.Length > 0)
        {
            Debug.Log("Diálogos cargados correctamente. Iniciando TypeLine.");
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.LogError("Los diálogos no se cargaron correctamente.");
        }
    }

    void LoadDialogue(string lang)
    {
        string path = Path.Combine(Application.streamingAssetsPath, $"dialogues_{lang}.json");
        string json = File.ReadAllText(path);

        DialogueData data = JsonUtility.FromJson<DialogueData>(json);

        lines = data.lines;
        index = 0;
    }

    void Update()
    {
        if (languagePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedButtonIndex = (selectedButtonIndex - 1 + languageButtons.Length) % languageButtons.Length;
                EventSystem.current.SetSelectedGameObject(languageButtons[selectedButtonIndex].gameObject);
                audioSource.PlayOneShot(selectionSound);
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                selectedButtonIndex = (selectedButtonIndex + 1) % languageButtons.Length;
                EventSystem.current.SetSelectedGameObject(languageButtons[selectedButtonIndex].gameObject);
                audioSource.PlayOneShot(selectionSound);
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                languageButtons[selectedButtonIndex].onClick.Invoke();
                audioSource.PlayOneShot(selectionSound);
            }

            return;

        }

        if (lines == null || lines.Length == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;

            if (c != ' ' && typeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typeSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCoroutine(FadeTransition());
        }
    }

    IEnumerator FadeTransition()
    {
        yield return ScreenFader.instance.FadeOut();

        GameObject bootstrap = new GameObject("GameBootstrapper");
        bootstrap.AddComponent<GameBootstrapper>();
        //PlayerPrefs.SetInt("loadedFromSave", 0);
        PlayerPrefs.DeleteKey("loadedFromSave");
        SceneManager.LoadScene("Graveyard01"); 
    }
}
