using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform room;

    public static CameraController instance;

    [Range(-10, 10)]
    public float minModX, maxModX, minModY, maxModY;
    public float dampSpeed;

    void Awake()
    {
        bool destruir = instance != null && instance != this;

        if (!destruir)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SearchReferences();
        }

        if (destruir)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedSearch());
    }

    System.Collections.IEnumerator DelayedSearch()
    {
        yield return null;
        SearchReferences();

        while (UiManager.instance == null)
            yield return null;

        //UiManager.instance.CameraAsign();
    }

    private void SearchReferences()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        GameObject roomObj = GameObject.FindGameObjectWithTag("Room");
        if (roomObj != null)
        {
            room = roomObj.transform;
        }
    }

    void LateUpdate()
    {
        if (player == null || room == null)
        {
            SearchReferences();
            if (player == null || room == null) return;
        }

        var bounds = room.GetComponent<BoxCollider2D>().bounds;

        float minPosY = bounds.min.y + minModY;
        float maxPosY = bounds.max.y + maxModY;
        float minPosX = bounds.min.x + minModX;
        float maxPosX = bounds.max.x + maxModX;

        Vector3 clampedPos = new Vector3(
            Mathf.Clamp(player.position.x, minPosX, maxPosX),
            Mathf.Clamp(player.position.y, minPosY, maxPosY),
            transform.position.z
        );

        Vector3 smoothPos = Vector3.Lerp(transform.position, clampedPos, dampSpeed * Time.deltaTime);
        transform.position = smoothPos;
    }
}