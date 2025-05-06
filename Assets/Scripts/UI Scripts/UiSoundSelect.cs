using UnityEngine;
using UnityEngine.EventSystems;

public class UiSoundSelect : MonoBehaviour
{
    public AudioClip selectionSound;
    public AudioSource audioSource;

    public void OnSelect(BaseEventData eventData)
    {
        if (selectionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(selectionSound);
        }
    }
}
