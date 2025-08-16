using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string dialogueFileName;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueLoader dialogueLoader;
    public GameObject interactionUI; 

    private bool playerInRange = false;
    private PlayerInputHandler inputHandler;

    public Sprite keyboardSprite;
    public Sprite playstationSprite;
    public Sprite xboxSprite;
    public LastInputDetector lastInputDetector; 
    private SpriteRenderer iconRenderer;

    void Start()
    {
        iconRenderer = interactionUI.GetComponent<SpriteRenderer>();
        interactionUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && inputHandler != null && inputHandler.interactionPressed)
        {
            inputHandler.ResetInputs();
            StartDialogue(); 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            inputHandler = other.GetComponent<PlayerInputHandler>();

            interactionUI.SetActive(true);
            UpdateInteractionIcon();

        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            inputHandler = null;
            
            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    void StartDialogue()
    { 
        string lang = PlayerPrefs.GetString("language", "es");
        string localizedFileName = $"{dialogueFileName}_{lang}";

        var dialogue = dialogueLoader.LoadDialogueFromFile("Dialogues/" + localizedFileName);
        dialogueManager.StartDialogue(dialogue);
    }

    void UpdateInteractionIcon()
    {
        switch (lastInputDetector.LastDeviceUsed)
        {
            case LastInputDetector.InputDeviceType.KeyboardMouse:
                iconRenderer.sprite = keyboardSprite;
                break;
            case LastInputDetector.InputDeviceType.PlayStation:
                iconRenderer.sprite = playstationSprite;
                break;
            case LastInputDetector.InputDeviceType.Xbox:
                iconRenderer.sprite = xboxSprite;
                break;
            default:
                iconRenderer.sprite = keyboardSprite; 
                break;
        }
    }
}
