using System.Collections;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{
    private Transform cam;
    private Vector3 previousCameraPosition;
    public float parallaxMultiplierX = 0.5f;
    public float parallaxMultiplierY = 0.5f;

    private void Start()
    {
        StartCoroutine(WaitForCamera());
    }

    private void LateUpdate()
    {
        if (cam == null) return;

        Vector3 deltaMovement = cam.position - previousCameraPosition;

        transform.position += new Vector3(
            deltaMovement.x * parallaxMultiplierX,
            deltaMovement.y * parallaxMultiplierY,
            0f
        );

        previousCameraPosition = cam.position;
    }

    private System.Collections.IEnumerator WaitForCamera()
    {
        while (Camera.main == null)
        {
            yield return null;
        }

        cam = Camera.main.transform;
        previousCameraPosition = cam.position;
    }

    public void ResetParallaxOrigin()
    {
        if (Camera.main == null) return;

        cam = Camera.main.transform;
        previousCameraPosition = cam.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float spriteWidth = sr != null ? sr.bounds.size.x : 0f;
        float spriteHeight = sr != null ? sr.bounds.size.y : 0f;

        float newX = cam.position.x - (spriteWidth * 0.5f * (1 - parallaxMultiplierX));
        float newY = cam.position.y - (spriteHeight * 0.5f * (1 - parallaxMultiplierY));

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}