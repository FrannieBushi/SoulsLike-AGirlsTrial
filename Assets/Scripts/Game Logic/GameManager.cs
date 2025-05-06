using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool shouldLoadFromSave = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void StartNewGame()
    {
        shouldLoadFromSave = false;
    }

    public void ContinueGame()
    {
        shouldLoadFromSave = true;
    }
       
}
