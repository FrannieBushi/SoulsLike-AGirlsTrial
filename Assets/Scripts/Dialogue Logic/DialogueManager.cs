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

    void Awake()
    {
        lines = new Queue<DialogueLine>();
        dialoguePanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialogue(List<DialogueLine> dialogue)
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        lines.Clear();

        foreach (var line in dialogue)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.P))
        {
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
