using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiSoundSliderChange : MonoBehaviour
{
    public AudioClip valueChangeSound;
    private AudioSource audioSource;
    public Slider slider;
    private float lastValue;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastValue = slider.value;
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        if (Mathf.Abs(value - lastValue) >= 0.01f && valueChangeSound != null)
        {
            audioSource.PlayOneShot(valueChangeSound);
            lastValue = value;
        }
    }
}
