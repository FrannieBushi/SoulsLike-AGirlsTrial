using UnityEngine;
using System.Collections;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    void Awake()
    {
        bool destroy = instance != null && instance != this;

        if (!destroy)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(AssignCameraWhenReady());
    }

    private IEnumerator AssignCameraWhenReady()
    {
        while (Camera.main == null || !Camera.main.enabled || !Camera.main.gameObject.activeInHierarchy)
        {
            yield return null;
        }

        AssignCameraToCanvases();
    }

    private void AssignCameraToCanvases()
    {
        Camera cam = Camera.main;
        Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
            {
                canvas.worldCamera = cam;
            }
        }
    }
}