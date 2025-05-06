
using UnityEngine;
public class CameraController : MonoBehaviour
{

    public Transform player;
    public Transform room;

    public static CameraController instance;

    [Range(-10,10)]
    public float minModX, maxModX, minModY, maxModY;
    public float dampSpeed;
    
    void Start()
    {
        
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    void Update()
    {
        var minPosY = room.GetComponent<BoxCollider2D>().bounds.min.y + minModY;
        var maxPosY = room.GetComponent<BoxCollider2D>().bounds.max.y + maxModY;
        var minPosX = room.GetComponent<BoxCollider2D>().bounds.min.x + minModX;
        var maxPosX = room.GetComponent<BoxCollider2D>().bounds.max.x + maxModX;

        Vector3 clampedPos = new Vector3(
            Mathf.Clamp(player.position.x, minPosX,maxPosX),
            Mathf.Clamp(player.position.y, minPosY,maxPosY),
            Mathf.Clamp(player.position.z, transform.position.z, transform.position.z)
        );

        Vector3 smoothPos = Vector3.Lerp(transform.position, clampedPos, dampSpeed * Time.deltaTime);

        transform.position = smoothPos;
    }
}
