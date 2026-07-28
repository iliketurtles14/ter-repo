using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private IniFile iniFile;
    private Pause pauseScript;
    private void Start()
    {
        iniFile = new IniFile(System.IO.Path.Combine(UnityEngine.Application.streamingAssetsPath, "UserData.ini"));
        pauseScript = transform.parent.Find("PauseMenuPanel").GetComponent<Pause>();
        Back(true);
    }
    public void Open()
    {
        transform.Find("MusicSlider").GetComponent<Slider>().value = Convert.ToInt32(iniFile.Read("Music", "Settings"));
        transform.Find("SoundsSlider").GetComponent<Slider>().value = Convert.ToInt32(iniFile.Read("Sounds", "Settings"));
        transform.Find("ScreenPicker").GetComponent<Picker>().currentIndex = Convert.ToInt32(iniFile.Read("Screen", "Settings"));
    
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void Back(bool atStart)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        if (!atStart)
        {
            pauseScript.OpenPauseMenu();
        }
    }
    public void Set()
    {
        int music;
        int sounds;
        int screen;

        music = Convert.ToInt32(transform.Find("MusicSlider").GetComponent<Slider>().value);
        sounds = Convert.ToInt32(transform.Find("SoundsSlider").GetComponent<Slider>().value);
        screen = transform.Find("ScreenPicker").GetComponent<Picker>().currentIndex;

        iniFile.Write("Music", music.ToString(), "Settings");
        iniFile.Write("Sounds", sounds.ToString(), "Settings");
        iniFile.Write("Screen", screen.ToString(), "Settings");

        int w = Display.main.systemWidth;
        int h = Display.main.systemHeight;
        switch (iniFile.Read("Screen", "Settings"))
        {
            case "0":
                Screen.SetResolution(w, h, FullScreenMode.Windowed);
                break;
            case "1":
                Screen.SetResolution(w, h, FullScreenMode.FullScreenWindow);
                break;
            case "2":
                Screen.SetResolution(w, h, FullScreenMode.ExclusiveFullScreen);
                break;
        }
    }
}
