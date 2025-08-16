using System.Collections;
using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public bool isFirstBoss;

    public GameObject bossGO;

    private void Start()
    {
        bossGO.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFirstBoss)
        {
            if (collision.CompareTag("Player") && PlayerStats.instance.doubleJumpUnlock == false)
            {
                BossUI.instance.BossActivator();
                StartCoroutine(WaitForBoss());
                Destroy(gameObject);
            }
        }

        else
        {
            if (collision.CompareTag("Player"))
            {
                BossUI.instance.BossActivator();
                StartCoroutine(WaitForBoss());
                Destroy(gameObject);
            }
        }
    }

    IEnumerator WaitForBoss()
    {
        bossGO.SetActive(true);
        yield return new WaitForSeconds(3f);
    }
}
