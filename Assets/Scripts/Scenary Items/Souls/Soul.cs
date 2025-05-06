using UnityEngine;

public class Soul : MonoBehaviour
{
    public int amount = 0;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<SoulsManager>().AddSouls(amount);
            Destroy(gameObject);
        }
    }
}
