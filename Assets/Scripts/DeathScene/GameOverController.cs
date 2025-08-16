using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverController : MonoBehaviour
{
    [Header("TextMeshPro UI Elements")]
    public TMP_Text youDiedText;
    public TMP_Text pressSubmitText;

    [Header("Timing Settings")]
    public float delayBeforeYouDied = 0.5f;
    public float delayBeforePress = 2.5f;

    [Header("Scene Settings")]
    public string sceneToLoad = "MainMenu";

    private bool canContinue = false;
    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Ui.Submit.performed -= OnSubmitPerformed;
            inputActions.Disable();
        }
    }

    void Start()
    {
        youDiedText.alpha = 0f;
        pressSubmitText.gameObject.SetActive(false);

        StartCoroutine(ShowTextsSequence());
    }

    IEnumerator ShowTextsSequence()
    {
        yield return new WaitForSeconds(delayBeforeYouDied);
        yield return FadeInText(youDiedText);

        yield return new WaitForSeconds(delayBeforePress);
        pressSubmitText.gameObject.SetActive(true);
        canContinue = true;

        inputActions.Ui.Submit.performed += OnSubmitPerformed;
    }

    IEnumerator FadeInText(TMP_Text text)
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
    }

    private void OnSubmitPerformed(InputAction.CallbackContext ctx)
    {
        if (canContinue)
        {
            DestroyDontDestroyOnLoadObjects();
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        GameObject temp = new GameObject("TempObjectToGetScene");
        DontDestroyOnLoad(temp);
        Scene dontDestroyScene = temp.scene;
        Destroy(temp);

        GameObject[] rootObjects = dontDestroyScene.GetRootGameObjects();

        foreach (GameObject go in rootObjects)
        {
            Destroy(go);
        }
    }
}