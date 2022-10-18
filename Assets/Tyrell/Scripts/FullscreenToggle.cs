using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    public Toggle _toggle;

    private void OnEnable()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.isOn = Screen.fullScreen;
    }
    public void Change(bool on)
    {
        Screen.fullScreenMode = on ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }
}
