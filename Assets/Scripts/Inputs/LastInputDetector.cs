using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class LastInputDetector : MonoBehaviour
{
    public static LastInputDetector instance;

    public enum InputDeviceType { KeyboardMouse, Xbox, PlayStation, Unknown }

    public InputDeviceType LastDeviceUsed { get; private set; } = InputDeviceType.KeyboardMouse;

    void Awake()
    {
        bool destroy = instance != null && instance != this;

        if (!destroy)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InputSystem.onAnyButtonPress.Call(OnAnyInput);
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnAnyInput(InputControl control)
    {
        var device = control.device;

        if (device is Keyboard || device is Mouse)
        {
            LastDeviceUsed = InputDeviceType.KeyboardMouse;
        }
        else if (device.name.ToLower().Contains("dualshock") || device.name.ToLower().Contains("sony"))
        {
            LastDeviceUsed = InputDeviceType.PlayStation;
        }
        else if (device.name.ToLower().Contains("xbox"))
        {
            LastDeviceUsed = InputDeviceType.Xbox;
        }    
    }
}
