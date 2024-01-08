using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Setting : MonoBehaviour
{
    public float volume;
    public AudioMixer audioMixer;
    public static float xlookSpeed = 200f;
    public static float ylookSpeed = 200f;
    private float cameraDirection = 1;
    public void SetVolumn(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetMouseXSensitivity(float xSen)
    {
        xlookSpeed = xSen * 10 * cameraDirection;
    }

    public void SetMouseYSensitivity(float ySen)
    {
        ylookSpeed = ySen * 10 * cameraDirection;
    }

    public void ToggleInvertCamera(bool isInvert)
    {
        if (isInvert == true)
        {
            cameraDirection = -1;
        }
        else
        {
            cameraDirection = 1;
        }
    }
}
