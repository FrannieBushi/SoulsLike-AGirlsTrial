using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    void Awake()
    {
        DestroyDontDestroyOnLoadObjects();    
    }
    void Start()
    {
        
        //SceneManager.LoadScene("MainMenu");   
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

        SceneManager.LoadScene("MainMenu");
    }
}
