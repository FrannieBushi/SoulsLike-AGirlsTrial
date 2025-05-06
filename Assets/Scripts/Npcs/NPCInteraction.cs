using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private string dialogueFileName = "prueba";
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueLoader dialogueLoader;
    public GameObject interactionUI; 
    private bool playerInRange = false;

    void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            StartDialogue(); 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionUI != null)
                interactionUI.SetActive(true);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    void StartDialogue()
    {
        var dialogue = dialogueLoader.LoadDialogueFromFile(dialogueFileName);
        dialogueManager.StartDialogue(dialogue);
    }
}
