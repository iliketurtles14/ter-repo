using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public MouseCollisionOnButtons mcs;
    public GameObject black;
    public Sprite checkedBoxSprite;
    public Sprite uncheckedBoxSprite;
    public Transform gameplayMenu;
    public Transform audioMenu;
    public Transform visualMenu;
    public Transform gameplayButton;
    public Transform audioButton;
    public Transform visualButton;
    public Sprite gameplayButtonNormal;
    public Sprite audioButtonNormal;
    public Sprite visualButtonNormal;
    public Sprite gameplayButtonPressed;
    public Sprite audioButtonPressed;
    public Sprite visualButtonPressed;
    private Dictionary<string, string> childNameToFileNameDict = new Dictionary<string, string>
    {
        { "NormalizeCheckBox", "NormalizePlayerMovement" },
        { "DarkModeCheckBox", "DarkMode" },
        { "PhysicsCheckBox", "PhysicsMode" },
        { "NewVisitorsCheckBox", "EnableNewVisitors" },
        { "DebugCheckBox", "DebugMode" },
        { "MusicSlider", "Music" },
        { "SoundsSlider", "Sounds" },
        { "ScreenPicker", "Screen" },
        { "BackgroundPicker", "TitleScreenBackground" }
    };

    //gameplay
    public bool normalizePlayerMovement;
    public bool darkMode;
    public bool physicsMode;
    public bool enableNewVisitors;
    public bool debugMode;
    public bool useOriginalDeskRandomization;

    //visual
    public int screen;
    public int titleScreenBackground;

    //audio
    public int music;
    public int sounds;

    private IniFile iniFile;
    private Dictionary<string, bool> boolDict;
    private Dictionary<string, int> intDict;
    private void Start()
    {
        Close();
    }
    private void SetSettings()
    {
        iniFile = new IniFile(System.IO.Path.Combine(UnityEngine.Application.streamingAssetsPath, "UserData.ini"));

        normalizePlayerMovement = iniFile.Read("NormalizePlayerMovement", "Settings") == "True";
        darkMode = iniFile.Read("DarkMode", "Settings") == "True";
        physicsMode = iniFile.Read("PhysicsMode", "Settings") == "True";
        enableNewVisitors = iniFile.Read("EnableNewVisitors", "Settings") == "True";
        debugMode = iniFile.Read("DebugMode", "Settings") == "True";
        screen = Convert.ToInt32(iniFile.Read("Screen", "Settings"));
        titleScreenBackground = Convert.ToInt32(iniFile.Read("TitleScreenBackground", "Settings"));
        music = Convert.ToInt32(iniFile.Read("Music", "Settings"));
        sounds = Convert.ToInt32(iniFile.Read("Sounds", "Settings"));

        UpdateDicts();

        List<Transform> menus = new List<Transform>
        {
            gameplayMenu, visualMenu, audioMenu
        };

        foreach(Transform menu in menus)
        {
            foreach(Transform child in menu.Find("Viewport").Find("Content"))
            {
                if (boolDict.ContainsKey(child.name)) //bools
                {
                    if (boolDict[child.name])
                    {
                        child.GetComponent<Image>().sprite = checkedBoxSprite;
                    }
                    else
                    {
                        child.GetComponent<Image>().sprite = uncheckedBoxSprite;
                    }
                    continue;
                }
                
                if(child.GetComponent<Picker>() != null)
                {
                    child.GetComponent<Picker>().currentIndex = intDict[child.name];
                    continue;
                }

                if(child.GetComponent<Slider>() != null)
                {
                    child.GetComponent<Slider>().value = intDict[child.name];
                    continue;
                }
            }
        }
    }
    public void Save()
    {
        List<Transform> menus = new List<Transform>
        {
            gameplayMenu, visualMenu, audioMenu
        };
        foreach (Transform menu in menus)
        {
            foreach(Transform child in menu.Find("Viewport").Find("Content"))
            {
                if (boolDict.ContainsKey(child.name))
                {
                    if (child.GetComponent<Image>().sprite == checkedBoxSprite)
                    {
                        boolDict[child.name] = true;
                        iniFile.Write(childNameToFileNameDict[child.name], "True", "Settings");
                    }
                    else
                    {
                        boolDict[child.name] = false;
                        iniFile.Write(childNameToFileNameDict[child.name], "False", "Settings");
                    }
                    continue;
                }

                if(child.GetComponent<Picker>() != null)
                {
                    intDict[child.name] = child.GetComponent<Picker>().currentIndex;
                    iniFile.Write(childNameToFileNameDict[child.name], intDict[child.name].ToString(), "Settings");
                    continue;
                }

                if(child.GetComponent<Slider>() != null)
                {
                    intDict[child.name] = Mathf.FloorToInt(child.GetComponent<Slider>().value);
                    iniFile.Write(childNameToFileNameDict[child.name], intDict[child.name].ToString(), "Settings");
                    continue;
                }
            }
        }
    }

    public void Open(string menu) //gameplay, visual, audio
    {
        SetSettings();

        transform.Find("OptionsText").GetComponent<TextMeshProUGUI>().text = menu.ToUpper();
        Transform menuToOpen;
        gameplayButton.GetComponent<Image>().sprite = gameplayButtonNormal;
        audioButton.GetComponent<Image>().sprite = audioButtonNormal;
        visualButton.GetComponent<Image>().sprite = visualButtonNormal;
        switch (menu)
        {
            case "visual":
                menuToOpen = visualMenu;
                visualButton.GetComponent<Image>().sprite = visualButtonPressed;
                break;
            case "audio":
                menuToOpen = audioMenu;
                audioButton.GetComponent<Image>().sprite = audioButtonPressed;
                break;
            default:
                menuToOpen = gameplayMenu;
                gameplayButton.GetComponent<Image>().sprite = gameplayButtonPressed;
                break;
        }

        foreach(Transform child in transform)
        {
            if(child.name != "GameplayMenu" && child.name != "AudioMenu" && child.name != "VisualMenu")
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        menuToOpen.gameObject.SetActive(true);
        black.GetComponent<Image>().enabled = true;
        GetComponent<Image>().enabled = true;
    }
    public void Close()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        black.GetComponent<Image>().enabled = false;
        GetComponent<Image>().enabled = false;
    }
    private void UpdateDicts()
    {
        boolDict = new Dictionary<string, bool>
        {
            { "NormalizeCheckBox", normalizePlayerMovement },
            { "DarkModeCheckBox", darkMode },
            { "PhysicsCheckBox", physicsMode },
            { "NewVisitorsCheckBox", enableNewVisitors },
            { "DebugCheckBox", debugMode },
        };
        intDict = new Dictionary<string, int>
        {
            { "MusicSlider", music },
            { "SoundsSlider", sounds },
            { "ScreenPicker", screen },
            { "BackgroundPicker", titleScreenBackground }
        };
    }
}
