using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSensitivityManager : MonoBehaviour
{
    public static float MouseSensitivity = 1.0f;
    private const float MinSensitivity = 0.1f;

    public void AdjustMouseSensitivity(float sliderValue)
    {
        MouseSensitivity = Mathf.Max(sliderValue, MinSensitivity);
        Debug.Log("Mouse Sensitivity adjusted to: " + MouseSensitivity);

    }
}
