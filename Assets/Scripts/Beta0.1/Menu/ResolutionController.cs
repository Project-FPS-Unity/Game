using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResolutionController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filterdResolutions;

    private float currentRefleshRate;
    private int currentResolutionIndex = 0;

    private void Start()
    {
        resolutions = Screen.resolutions;
        filterdResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefleshRate = Screen.currentResolution.refreshRate;
        // Debug.Log("RefreshRate : " + currentRefleshRate);

        for (int i = 0; i < resolutions.Length; i++)
        {
            // Debug.Log("Resolution : " + resolutions[i]);
            if (resolutions[i].refreshRate == currentRefleshRate)
            {
                filterdResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filterdResolutions.Count; i++)
        {
            string resolutionOption = filterdResolutions[i].width + "x" 
                + filterdResolutions[i].height + " " + filterdResolutions[i].refreshRate + "Hz";
            options.Add(resolutionOption);
            if (filterdResolutions[i].width == Screen.width
                && filterdResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filterdResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
