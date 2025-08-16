using UnityEngine;
using System.Collections;

public class PlayerInitialPosition : MonoBehaviour
{
    public Transform leftEntrance;
    public Transform rightEntrance;
    public GameObject player;
    
    void Awake()
    {
        if (!PlayerPrefs.HasKey("enteredFromLeft"))
        {
            
            return;
        }

        bool loadedFromSave = PlayerPrefs.GetInt("loadedFromSave", 0) == 1;

        if (!loadedFromSave)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                bool enteredFromLeft = PlayerPrefs.GetInt("enteredFromLeft", 1) == 1;

                player.transform.position = enteredFromLeft
                    ? leftEntrance.position
                    : rightEntrance.position;

                PlayerPrefs.DeleteKey("enteredFromLeft");

                HorizontalParallax[] parallaxLayers = FindObjectsByType<HorizontalParallax>(FindObjectsSortMode.None);
                foreach (var layer in parallaxLayers)
                {
                    layer.ResetParallaxOrigin();
                }

                StartCoroutine(ResetParallaxWhenCameraIsReady());
            }
            else
            {
                Debug.LogWarning("Player no encontrado ");
            }
        }

        PlayerPrefs.DeleteKey("loadedFromSave");
    }

    private IEnumerator ResetParallaxWhenCameraIsReady()
    {

        while (Camera.main == null)
        {
            yield return null;
        }


        yield return null;

        ParallaxMovement[] parallaxMovements = FindObjectsByType<ParallaxMovement>(FindObjectsSortMode.None);
        foreach (var pm in parallaxMovements)
        {
            pm.ResetParallaxOrigin();
        }

        HorizontalParallax[] parallaxLayers = FindObjectsByType<HorizontalParallax>(FindObjectsSortMode.None);
        foreach (var layer in parallaxLayers)
        {
            layer.ResetParallaxOrigin();
        }
    }
}
