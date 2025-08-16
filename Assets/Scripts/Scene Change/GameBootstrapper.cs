using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameBootstrapper : MonoBehaviour
{
    private static GameBootstrapper instance;
    public string playerPrefabPath = "Player/Player";
    public string cameraPrefabPath = "PersitentCamera/Main Camera";
    public string uiPrefabPath = "Uis/Uis";

    public Vector3 newGameStartPosition = new Vector3(-39.9f, 2.33f, 0f);
    public string newGameScene = "Graveyard01";

    private bool loadedFromSave;
    private Camera cachedCamera;
    private GameObject uiInstance;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        loadedFromSave = PlayerPrefs.GetInt("loadedFromSave", 0) == 1;
        StartCoroutine(SetupGame());
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator SetupGame()
    {
        GameObject playerPrefab = Resources.Load<GameObject>(playerPrefabPath);
        GameObject playerInstance = null;

        if (playerPrefab != null)
        {
            Vector3 playerPos = loadedFromSave
                ? SaveSystem.GetSavedPlayerPosition()
                : newGameStartPosition;

            playerInstance = Instantiate(playerPrefab, playerPos, Quaternion.identity);
            DontDestroyOnLoad(playerInstance);
        }

        if (loadedFromSave && playerInstance != null)
        {
            PlayerStats stats = playerInstance.GetComponent<PlayerStats>();
            if (stats != null)
            {
                SaveSystem.LoadPlayer(stats, playerInstance.transform);
            }
        }

        yield return null;

        GameObject camPrefab = Resources.Load<GameObject>(cameraPrefabPath);
        if (camPrefab != null)
        {
            Vector3 camPos = playerInstance != null
                ? new Vector3(playerInstance.transform.position.x, playerInstance.transform.position.y, -10f)
                : new Vector3(0f, 0f, -10f);

            GameObject camInstance = Instantiate(camPrefab, camPos, Quaternion.identity);

            camInstance.tag = "MainCamera";
            camInstance.SetActive(true);
            DontDestroyOnLoad(camInstance);

            cachedCamera = camInstance.GetComponent<Camera>();

        }

        yield return null;

        SceneManager.sceneLoaded += OnSceneLoaded;

        string sceneToLoad = loadedFromSave
            ? SaveSystem.GetSavedSceneName()
            : newGameScene;

        SceneManager.LoadScene(sceneToLoad);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject uiPrefab = Resources.Load<GameObject>(uiPrefabPath);

        if (uiPrefab != null)
        {
            uiInstance = Instantiate(uiPrefab);
            uiInstance.name = "Uis";
            DontDestroyOnLoad(uiInstance);
        }

        PlayerHealth playerHealth = PlayerStats.instance?.GetComponent<PlayerHealth>();

        Transform healthBarTransform = uiInstance?.transform
            .Find("PlayerUI/HealthBar/HealthBarBackground/HealthBar");

        Image healthImage = healthBarTransform?.GetComponent<Image>();

        if (playerHealth != null && healthImage != null)
        {
            playerHealth.Init(healthImage);
        }

        PlayerMana playerMana = PlayerStats.instance?.GetComponent<PlayerMana>();

        Transform manaBarTransform = uiInstance?.transform
            .Find("PlayerUI/ManaBar/ManaBarBackground/ManaBar");

        Image manaImage = manaBarTransform?.GetComponent<Image>();

        if (playerMana != null && manaImage != null)
        {
            playerMana.Init(manaImage);
        }

        SoulsManager soulsManager = PlayerStats.instance?.GetComponent<SoulsManager>();

        Transform soulsTextTransform = uiInstance?.transform
            .Find("PlayerUI/SoulsLabel/AmountSouls");

        Text soulsText = soulsTextTransform?.GetComponent<Text>();

        if (soulsManager != null && soulsText != null)
        {
            soulsManager.Init(soulsText);
        }

        if (cachedCamera != null && uiInstance != null)
        {

            AssignCameraToCanvases(cachedCamera);
            StartCoroutine(AssignCameraWithRetry());
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PauseMenuController pauseController = player.GetComponent<PauseMenuController>();

            if (pauseController != null)
            {
                Transform pauseMenuTransform = uiInstance.transform.Find("PauseMenu");

                if (pauseMenuTransform == null) return;

                pauseController.pauseMenuUI = pauseMenuTransform.gameObject;

                bool wasActive = pauseMenuTransform.gameObject.activeSelf;

                if (!wasActive) pauseMenuTransform.gameObject.SetActive(true);
                Button[] buttons = pauseMenuTransform.GetComponentsInChildren<Button>(true);

                foreach (Button button in buttons)
                {
                    switch (button.name)
                    {
                        case "ResumeBtn":
                            button.onClick.AddListener(pauseController.Resume);
                            break;
                        case "QuitBtn":
                            button.onClick.AddListener(pauseController.Close);
                            break;
                    }
                }

                Slider volumeSlider = pauseMenuTransform.GetComponentInChildren<Slider>(true);
                if (volumeSlider != null)
                {
                    pauseController.volumeSlider = volumeSlider;
                    volumeSlider.onValueChanged.AddListener(pauseController.ChangeVolume);
                    volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
                }

                if (!wasActive) pauseMenuTransform.gameObject.SetActive(false);
            }

            if (player != null)
            {
                StartCoroutine(DelayedParallaxReset());
            }

            var horizontalParallaxes = Object.FindObjectsByType<HorizontalParallax>(FindObjectsSortMode.None);

            foreach (var parallax in horizontalParallaxes)
            {
                parallax.ResetParallaxOrigin();
            }
        }
    }

    private void AssignCameraToCanvases(Camera camera)
    {
        Canvas[] canvases = uiInstance.GetComponentsInChildren<Canvas>(true);

        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
            }

            canvas.worldCamera = camera;
        }

    }

    private IEnumerator AssignCameraWithRetry()
    {
        int attempts = 0;
        const int maxAttempts = 10;

        while (attempts < maxAttempts)
        {
            if (cachedCamera != null && uiInstance != null)
            {
                Canvas[] canvases = uiInstance.GetComponentsInChildren<Canvas>(true);
                foreach (Canvas canvas in canvases)
                {
                    if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
                    {
                        canvas.worldCamera = cachedCamera;
                    }
                }

                yield break;
            }

            attempts++;

            yield return null;
        }
    }
    
    private IEnumerator DelayedParallaxReset()
    {
        yield return null; 
        yield return null;

        var parallaxLayers = Object.FindObjectsByType<ParallaxMovement>(FindObjectsSortMode.None);
        foreach (var layer in parallaxLayers)
        {
            layer.ResetParallaxOrigin();
        }
    }
}