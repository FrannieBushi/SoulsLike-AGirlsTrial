using UnityEngine;

public class BossUI : MonoBehaviour
{
    public GameObject bossPanel;
    public GameObject walls;

    public AudioSource musicSource;
    public AudioClip normalMusic;
    public AudioClip bossMusic;

    public static BossUI instance;

    void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
        }*/

        instance = this;

    }

    void Start()
    {
        bossPanel.SetActive(false);
        walls.SetActive(false);
    }

    public void BossActivator()
    {
        bossPanel.SetActive(true);
        walls.SetActive(true);

        if (musicSource != null && bossMusic != null)
        {
            musicSource.clip = bossMusic;
            musicSource.Play();
        }
    }

    public void BossDeactivator()
    {
        bossPanel.SetActive(false);
        walls.SetActive(false);

        if (musicSource != null && normalMusic != null)
        {
            musicSource.clip = normalMusic;
            musicSource.Play();
        }
    }
}