using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer am;
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropDowm;

    private void Start() {
        resolutions = Screen.resolutions;
        resolutionDropDowm.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (!options.Contains(option)) {
                options.Add(option);
            }
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDowm.AddOptions(options);
        resolutionDropDowm.value = currentResolutionIndex;
        resolutionDropDowm.RefreshShownValue();
    }

    public void SetVolume(float volume) {
        am.SetFloat("volume", volume);
    }

    public void SetQuality(int quality) {
        QualitySettings.SetQualityLevel(quality +1);
    }

    public void SetFullScreen(bool isFS) {
        Screen.fullScreen = isFS;
    }

    public void SetResolution(int index) {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
