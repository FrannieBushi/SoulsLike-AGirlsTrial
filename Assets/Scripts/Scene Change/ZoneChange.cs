using UnityEngine;
using UnityEngine.SceneManagement;
public class ZoneChange : MonoBehaviour
{
    public string sceneName;
    public bool enteredFromLeft= true;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("enteredFromLeft", enteredFromLeft? 1 : 0);
            SceneManager.LoadScene(sceneName);
        }
    }
}
