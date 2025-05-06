using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float healthPoints;
    public float maxHealthPoints;
    public float speed;
    public float knockBackForceX;
    public float knockBackForceY;
    public float damageToGive;
    public int amountSoulsGived = 0;
    public GameObject soulPrefab;
    public bool isBoss;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    void Start()
    {
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        healthPoints = maxHealthPoints;

        EnemyRespawnManager.instance.RegisterEnemy(this);


    }

    public void DropSoul()
    {
        Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
        GameObject soul = Instantiate(soulPrefab, transform.position + offset, Quaternion.identity);
        soul.GetComponent<Soul>().amount = amountSoulsGived;

    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        healthPoints = maxHealthPoints;

        GetComponent<SpriteRenderer>().material = GetComponent<Blink>().original;
        if(GetComponent<EnemyHealth>().hasDeathAnimation == true)
        {
            GetComponent<Animator>().SetBool("death", false);
        }
        
        gameObject.tag = "Enemy";

    }
}
