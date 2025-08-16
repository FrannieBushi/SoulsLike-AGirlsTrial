using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GhostBossBehavior : MonoBehaviour
{
    public Transform[] transforms;
    public GameObject flame;

    public float timeToShoot, countDown;
    public float timeToTp, countDownTp;

    public Enemy enemy;
    public EnemyHealth enemyHealth;
    public Image healthBar;

    private bool isFacingRight = true;

    public bool finalBoss; 

    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyHealth = GetComponent<EnemyHealth>();

        var InitialPosition = Random.Range(0, transforms.Length);
        transform.position = transforms[InitialPosition].position;
        countDown = timeToShoot;
        countDownTp = timeToTp;
    }

    void Update()
    {
        countDown -= Time.deltaTime;
        countDownTp -= Time.deltaTime;
        CountDowns();
        DamageBoss();
        BossScale(); 
    }

    public void CountDowns()
    {
        if (countDown <= 0f)
        {
            ShootPlayer();
            countDown = timeToShoot;
        }

        if (countDownTp <= 0f)
        {
            countDownTp = timeToTp;
            Teleport();
        }
    }

    public void ShootPlayer()
    {
        GameObject spell = Instantiate(flame, transform.position, Quaternion.identity);
    }

    public void Teleport()
    {
        var InitialPosition = Random.Range(0, transforms.Length);
        transform.position = transforms[InitialPosition].position;
        countDownTp = timeToTp;
    }

    public void DamageBoss()
    {
        healthBar.fillAmount = enemy.healthPoints / enemy.maxHealthPoints;

        if (enemy.healthPoints <= 0 && !finalBoss)
        {
            PlayerStats.instance.doubleJumpUnlock = true;
            BossUI.instance.BossDeactivator();
            enemyHealth.DestroyGameObject();
        }

        else if (enemy.healthPoints <= 0 && finalBoss)
        {
            SceneManager.LoadScene("Credits");
        }
    }

    public void BossScale()
    {
        if (transform.position.x > PlayerStats.instance.transform.position.x && isFacingRight)
        {
            Flip();
        }
        else if (transform.position.x < PlayerStats.instance.transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
