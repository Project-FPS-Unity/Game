using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public static float volume = 0f;
    public AudioMixer audioMixer;
    public static float xlookSpeed = 200f;
    public static float ylookSpeed = 200f;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetMouseXSensitivity(float xSen)
    {
        xlookSpeed = xSen * 10;
    }

    public void SetMouseYSensitivity(float ySen)
    {
        ylookSpeed = ySen * 10;
    }
}
