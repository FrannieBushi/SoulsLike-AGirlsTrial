using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{
    Transform cam;           
    Vector3 camStartPos;     
    Vector3 startPos;        
    public float parallaxFactor = 0.5f; 

    void Start()
    {
        cam = Camera.main.transform;       
        camStartPos = cam.position;       
        startPos = transform.position;     
    }

    void LateUpdate()
    {
        // Calcula cuánto se ha movido la cámara desde su posición inicial
        Vector3 deltaMovement = cam.position - camStartPos;

        // Aplica el factor de parallax
        float newX = startPos.x + deltaMovement.x * parallaxFactor;
        float newY = startPos.y + deltaMovement.y * parallaxFactor;

        // Redondea a 2 decimales para evitar jitter en cámara ortográfica
        newX = Mathf.Round(newX * 100f) / 100f;
        newY = Mathf.Round(newY * 100f) / 100f;

        // Asigna la nueva posición
        transform.position = new Vector3(newX, newY, startPos.z);
    }
}
