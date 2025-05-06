
using UnityEngine;
public class UpgradeMenuController : MonoBehaviour
{
    public GameObject menuController;
    public PauseMenuController pauseMenuController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        if (menuController != null)
        {
            Time.timeScale = 1f;  
            pauseMenuController.enabled = true;
            gameObject.SetActive(false);
            
        }
    }
}
