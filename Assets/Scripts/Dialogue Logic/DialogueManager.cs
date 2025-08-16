using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text speakerText;     
    public TMP_Text dialogueText;

    private Queue<DialogueLine> lines;
    private bool isDialogueActive = false;

    private bool isTyping = false;
    private string currentLine = "";

    [Header("Typing Effect")]
    public float typingSpeed = 0.02f; 

    [Header("Audio")]
    public AudioClip typeSound;       
    private AudioSource audioSource; 

    public PlayerInputHandler inputHandler; 
    public GameObject player;

    int x = 1, y = 2, z = 3;

    void Awake()
    {
        lines = new Queue<DialogueLine>();
        //dialoguePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialogue(List<DialogueLine> dialogue)
    {
        if (!dialoguePanel || !dialogueText || !speakerText)
        {
            TryAssignDialogueUI();
        }

        // Buscar al jugador si no está asignado
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        // Buscar el inputHandler si no está asignado
        if (inputHandler == null && player != null)
        {
            inputHandler = player.GetComponent<PlayerInputHandler>();
        }

        if (!dialoguePanel || !dialogueText || !speakerText || player == null || inputHandler == null)
        {
            Debug.LogError("❌ DialogueManager no está correctamente configurado o no se encontró el jugador.");
            return;
        }

        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        lines.Clear();

        foreach (var line in dialogue)
        {
            lines.Enqueue(line);
        }

        SetPlayerControls(false);
        DisplayNextLine();
    }

    void Update()
    {
        if (isDialogueActive && inputHandler != null && inputHandler.submitPressed)
        {
            inputHandler.ResetInputs();
            DisplayNextLine();
        }
    }

    public void DisplayNextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentLine;
            isTyping = false;
            return;
        }

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = lines.Dequeue();
        speakerText.text = line.speaker;
        currentLine = line.text;

        if (line.speaker == "Vin")
        {
            Color customBlue;
            if (ColorUtility.TryParseHtmlString("#9EDFFF", out customBlue))
            {
                dialogueText.color = customBlue;
            }
        }
            
        else
            dialogueText.color = Color.white;

        StartCoroutine(TypeLine(currentLine));
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        SetPlayerControls(true);
    }

    void SetPlayerControls(bool state)
    {
        player.GetComponent<JumpController>().enabled = state;
        player.GetComponent<DodgeController>().enabled = state;
        player.GetComponent<PlayerController>().enabled = state;
        player.GetComponent<PotionManagerController>().enabled = state;
        player.GetComponent<CombatController>().enabled = state;

        if(state)
        {
            player.GetComponent<PlayerInputHandler>().ResetInputs();
        }
    }

    private void TryAssignDialogueUI()
    {
        GameObject uiRoot = GameObject.Find("Uis"); 
        if (uiRoot == null)
        {
            Debug.LogWarning("⚠ No se encontró 'Uis' en la escena.");
            return;
        }

        Transform dialogueUI = uiRoot.transform.Find("DialogueUI");
        if (dialogueUI == null)
        {
            Debug.LogWarning("⚠ No se encontró 'DialogueUI' dentro de 'Uis'.");
            return;
        }

        dialoguePanel = dialogueUI.gameObject;
        dialogueText = dialogueUI.Find("Panel/Dialogue")?.GetComponent<TMP_Text>();
        speakerText = dialogueUI.Find("Panel (1)/Name")?.GetComponent<TMP_Text>();

        if (dialoguePanel && dialogueText && speakerText)
        {
            Debug.Log("✅ DialogueManager configurado correctamente.");
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;

            if (typeSound != null && audioSource != null && c != ' ')
            {
                audioSource.PlayOneShot(typeSound);
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
