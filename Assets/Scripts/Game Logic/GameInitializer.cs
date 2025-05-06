using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public PlayerStats playerStats;
    public Transform playerTransform;

    void Start()
    {
        if (GameManager.instance != null && GameManager.instance.shouldLoadFromSave)
        {
            SaveSystem.LoadPlayer(playerStats, playerTransform);
        }
        else
        {
            Debug.Log("Nueva partida: no se carga ningún dato.");
        }    
    }

}
