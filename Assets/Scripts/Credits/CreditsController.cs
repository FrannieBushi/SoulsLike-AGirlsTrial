using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CreditsController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "MainMenu";

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Ui.Submit.performed += OnSkipCredits;
        inputActions.Ui.Cancel.performed += OnSkipCredits; 
    }

    void OnDisable()
    {
        inputActions.Ui.Submit.performed -= OnSkipCredits;
        inputActions.Ui.Cancel.performed -= OnSkipCredits;
        inputActions.Disable();
    }

    void Start()
    {
        DestroyDontDestroyOnLoadObjects();
    }

    private void OnSkipCredits(InputAction.CallbackContext ctx)
    {
        EndCredits(); 
    }

    public void EndCredits()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void DestroyDontDestroyOnLoadObjects()
    {
        GameObject temp = new GameObject("Temp_DontDestroyProbe");
        DontDestroyOnLoad(temp);
        Scene dontDestroyScene = temp.scene;
        Destroy(temp);

        foreach (GameObject go in dontDestroyScene.GetRootGameObjects())
        {
            Destroy(go);
        }
    }
}