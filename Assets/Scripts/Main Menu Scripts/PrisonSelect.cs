using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrisonSelect : MonoBehaviour
{
    public MouseCollisionOnButtons mouseCollisionScript;
    public List<Sprite> prisonSprites = new List<Sprite>();
    public TextMeshProUGUI prisonText;
    public TextMeshProUGUI difficultyText;
    public GameObject CurrentPrisonObject;
    public GameObject CurrentDifficultyObject;
    public GameObject LeftArrow;
    public GameObject RightArrow;
    public int whichPrison;
    public GameObject TitlePanel;
    public GameObject PrisonSelectPanel;
    public Sprite ButtonNormalSprite;
    public Sprite ButtonPressedSprite;
    public OnMainButtonPress titlePanelScript;
    public Canvas MainMenuCanvas;
    public List<string> mainPrisonPaths = new List<string>();
    public List<string> bonusPrisonPaths = new List<string>();
    public List<string> customPrisonPaths = new List<string>();
    public int currentPrisonInmateNum;
    public int currentPrisonGuardNum;
    public bool currentPrisonHasPOW;
    public string currentPrisonPath;
    private List<Sprite> mainPrisonIcons = new List<Sprite>();
    private List<int> mainPrisonInmateNums = new List<int>();
    private List<int> mainPrisonGuardNums = new List<int>();
    private List<bool> mainPrisonHasPOWBools = new List<bool>();
    public List<string> mainPrisonNames = new List<string>();
    private List<Sprite> bonusPrisonIcons = new List<Sprite>();
    private List<int> bonusPrisonInmateNums = new List<int>();
    private List<int> bonusPrisonGuardNums = new List<int>();
    private List<bool> bonusPrisonHasPOWBools = new List<bool>();
    public List<string> bonusPrisonNames = new List<string>();
    private List<Sprite> customPrisonIcons = new List<Sprite>();
    private List<int> customPrisonInmateNums = new List<int>();
    private List<int> customPrisonGuardNums = new List<int>();
    private List<bool> customPrisonHasPOWBools = new List<bool>();
    public List<string> customPrisonNames = new List<string>();
    public int amountOfMainPrisons;
    public int amountOfBonusPrisons;
    public int amountOfCustomPrisons;
    public int whichTab; //0 = main, 1 = bonus, 2 = custom
    public bool doneWithInitialLoad = false;
    public Sprite mainPrisonsButtonNormal;
    public Sprite mainPrisonsButtonPressed;
    public Sprite bonusPrisonsButtonNormal;
    public Sprite bonusPrisonsButtonPressed;
    public Sprite customPrisonsButtonNormal;
    public Sprite customPrisonsButtonPressed;
    private List<Sprite> currentPrisonIcons = new List<Sprite>();
    private List<int> currentPrisonInmateNums = new List<int>();
    private List<int> currentPrisonGuardNums = new List<int>();
    private List<bool> currentPrisonHasPOWBools = new List<bool>();
    public List<string> currentPrisonNames = new List<string>();
    public List<string> currentPrisonPaths = new List<string>();
    public int amountOfCurrentPrisons;
    public Sprite clearSprite;
    public Sprite continueButtonMad;
    public Sprite continueButtonGood;


    public void Update()
    {
        switch (whichTab)
        {
            case 0:
                amountOfCurrentPrisons = amountOfMainPrisons;
                transform.Find("ReloadPrisonsButton").gameObject.SetActive(false);
                break;
            case 1:
                amountOfCurrentPrisons = amountOfBonusPrisons;
                transform.Find("ReloadPrisonsButton").gameObject.SetActive(false);
                break;
            case 2:
                amountOfCurrentPrisons = amountOfCustomPrisons;
                transform.Find("ReloadPrisonsButton").gameObject.SetActive(true);
                break;
        }


        if(whichPrison == 0)
        {
            LeftArrow.GetComponent<Image>().enabled = false;
            LeftArrow.GetComponent<Button>().enabled = false;
        }
        else
        {
            LeftArrow.GetComponent<Image>().enabled = true;
            LeftArrow.GetComponent<Button>().enabled = true;
        }

        if (whichPrison + 1 == amountOfCurrentPrisons)
        {
            RightArrow.GetComponent<Image>().enabled = false;
            RightArrow.GetComponent<Button>().enabled = false;
        }
        else
        {
            RightArrow.GetComponent<Image>().enabled = true;
            RightArrow.GetComponent<Button>().enabled = true;
        }

        switch (whichTab)
        {
            case 0:
                transform.Find("MainPrisonsButton").GetComponent<Image>().sprite = mainPrisonsButtonPressed;
                transform.Find("BonusPrisonsButton").GetComponent<Image>().sprite = bonusPrisonsButtonNormal;
                transform.Find("CustomPrisonsButton").GetComponent<Image>().sprite = customPrisonsButtonNormal;

                currentPrisonIcons = mainPrisonIcons;
                currentPrisonNames = mainPrisonNames;
                currentPrisonInmateNums = mainPrisonInmateNums;
                currentPrisonGuardNums = mainPrisonGuardNums;
                currentPrisonHasPOWBools = mainPrisonHasPOWBools;
                currentPrisonPaths = mainPrisonPaths;
                break;
            case 1:
                transform.Find("MainPrisonsButton").GetComponent<Image>().sprite = mainPrisonsButtonNormal;
                transform.Find("BonusPrisonsButton").GetComponent<Image>().sprite = bonusPrisonsButtonPressed;
                transform.Find("CustomPrisonsButton").GetComponent<Image>().sprite = customPrisonsButtonNormal;

                currentPrisonIcons = bonusPrisonIcons;
                currentPrisonNames = bonusPrisonNames;
                currentPrisonInmateNums = bonusPrisonInmateNums;
                currentPrisonGuardNums = bonusPrisonGuardNums;
                currentPrisonHasPOWBools = bonusPrisonHasPOWBools;
                currentPrisonPaths = bonusPrisonPaths;
                break;
            case 2:
                transform.Find("MainPrisonsButton").GetComponent<Image>().sprite = mainPrisonsButtonNormal;
                transform.Find("BonusPrisonsButton").GetComponent<Image>().sprite = bonusPrisonsButtonNormal;
                transform.Find("CustomPrisonsButton").GetComponent<Image>().sprite = customPrisonsButtonPressed;

                currentPrisonIcons = customPrisonIcons;
                currentPrisonNames = customPrisonNames;
                currentPrisonInmateNums = customPrisonInmateNums;
                currentPrisonGuardNums = customPrisonGuardNums;
                currentPrisonHasPOWBools = customPrisonHasPOWBools;
                currentPrisonPaths = customPrisonPaths;
                break;
        }

        if(amountOfCurrentPrisons == 0)
        {
            prisonText.text = "";
            CurrentPrisonObject.GetComponent<Image>().sprite = clearSprite;
            currentPrisonGuardNum = 0;
            currentPrisonInmateNum = 0;
            currentPrisonHasPOW = false;
            currentPrisonPath = null;

            transform.Find("ContinueButton").GetComponent<Image>().sprite = continueButtonMad;
            transform.Find("ContinueButton").GetComponent<Button>().enabled = false;
            transform.Find("ContinueButton").GetComponent<EventTrigger>().enabled = false;

            LeftArrow.SetActive(false);
            RightArrow.SetActive(false);
        }
        else
        {
            prisonText.text = currentPrisonNames[whichPrison];
            CurrentPrisonObject.GetComponent<Image>().sprite = currentPrisonIcons[whichPrison];
            currentPrisonGuardNum = currentPrisonGuardNums[whichPrison];
            currentPrisonInmateNum = currentPrisonInmateNums[whichPrison];
            currentPrisonHasPOW = currentPrisonHasPOWBools[whichPrison];
            currentPrisonPath = currentPrisonPaths[whichPrison];

            transform.Find("ContinueButton").GetComponent<Image>().sprite = ButtonNormalSprite;
            transform.Find("ContinueButton").GetComponent<Button>().enabled = true;
            transform.Find("ContinueButton").GetComponent<EventTrigger>().enabled = true;

            LeftArrow.SetActive(true);
            RightArrow.SetActive(true);
        }
    }
    public void ReloadPrisons(bool onlyCustom)
    {
        ResetLists(onlyCustom);

        if (!onlyCustom)
        {
            foreach (string path in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Prisons", "MainPrisons")))
            {
                if (Path.GetExtension(path) == ".zmap")
                {
                    mainPrisonPaths.Add(path);
                }
            }
            foreach (string path in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Prisons", "BonusPrisons")))
            {
                if (Path.GetExtension(path) == ".zmap")
                {
                    bonusPrisonPaths.Add(path);
                }
            }
        }
        foreach (string path in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Prisons", "CustomPrisons")))
        {
            if (Path.GetExtension(path) == ".zmap")
            {
                customPrisonPaths.Add(path);
            }
        }

        if (!onlyCustom)
        {
            for (int i = 0; i < mainPrisonPaths.Count; i++)
            {
                string extractPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "MainPrisons") + Path.DirectorySeparatorChar;
                ZipFile.ExtractToDirectory(mainPrisonPaths[i], extractPath);
                string[] data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));
                File.Delete(Path.Combine(extractPath, "Data.ini"));
                if (File.Exists(Path.Combine(extractPath, "Icon.png")))
                {
                    mainPrisonIcons.Add(ConvertPNGToSprite(Path.Combine(extractPath, "Icon.png")));
                    File.Delete(Path.Combine(extractPath, "Icon.png"));
                }
                else
                {
                    mainPrisonIcons.Add(Resources.Load<Sprite>("Main Menu Resources/UI Stuff/noicon"));
                }
                if (File.Exists(Path.Combine(extractPath, "Tiles.png")))
                {
                    File.Delete(Path.Combine(extractPath, "Tiles.png"));
                }
                if (File.Exists(Path.Combine(extractPath, "Ground.png")))
                {
                    File.Delete(Path.Combine(extractPath, "Ground.png"));
                }
                if (File.Exists(Path.Combine(extractPath, "Music.mp3")))
                {
                    File.Delete(Path.Combine(extractPath, "Music.mp3"));
                }
                if (GetINIVar("Properties", "POWOutfits", data) == "True")
                {
                    mainPrisonHasPOWBools.Add(true);
                }
                else
                {
                    mainPrisonHasPOWBools.Add(false);
                }

                for (int j = 0; j < data.Length; j++)
                {
                    data[j] = data[j].Replace("\n", "");
                    data[j] = data[j].Replace("\r", "");
                    data[j] = data[j].Replace("\u200B", "");
                }
                mainPrisonInmateNums.Add(Convert.ToInt32(GetINIVar("Properties", "Inmates", data)));
                mainPrisonGuardNums.Add(Convert.ToInt32(GetINIVar("Properties", "Guards", data)));
                mainPrisonNames.Add(GetINIVar("Properties", "MapName", data).ToUpper());
            }
            for (int i = 0; i < bonusPrisonPaths.Count; i++)
            {
                string extractPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "BonusPrisons") + Path.DirectorySeparatorChar;
                ZipFile.ExtractToDirectory(bonusPrisonPaths[i], extractPath);
                string[] data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));
                File.Delete(Path.Combine(extractPath, "Data.ini"));
                if (File.Exists(Path.Combine(extractPath, "Icon.png")))
                {
                    bonusPrisonIcons.Add(ConvertPNGToSprite(Path.Combine(extractPath, "Icon.png")));
                    File.Delete(Path.Combine(extractPath, "Icon.png"));
                }
                else
                {
                    bonusPrisonIcons.Add(Resources.Load<Sprite>("Main Menu Resources/UI Stuff/noicon"));
                }
                if (File.Exists(Path.Combine(extractPath, "Tiles.png")))
                {
                    File.Delete(Path.Combine(extractPath, "Tiles.png"));
                }
                if (File.Exists(Path.Combine(extractPath, "Ground.png")))
                {
                    File.Delete(Path.Combine(extractPath, "Ground.png"));
                }
                if (File.Exists(Path.Combine(extractPath, "Music.mp3")))
                {
                    File.Delete(Path.Combine(extractPath, "Music.mp3"));
                }
                if (GetINIVar("Properties", "POWOutfits", data) == "True")
                {
                    bonusPrisonHasPOWBools.Add(true);
                }
                else
                {
                    bonusPrisonHasPOWBools.Add(false);
                }

                for (int j = 0; j < data.Length; j++)
                {
                    data[j] = data[j].Replace("\n", "");
                    data[j] = data[j].Replace("\r", "");
                    data[j] = data[j].Replace("\u200B", "");
                }
                bonusPrisonInmateNums.Add(Convert.ToInt32(GetINIVar("Properties", "Inmates", data)));
                bonusPrisonGuardNums.Add(Convert.ToInt32(GetINIVar("Properties", "Guards", data)));
                bonusPrisonNames.Add(GetINIVar("Properties", "MapName", data).ToUpper());
            }

        }
        for (int i = 0; i < customPrisonPaths.Count; i++)
        {
            string extractPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "CustomPrisons") + Path.DirectorySeparatorChar;
            ZipFile.ExtractToDirectory(customPrisonPaths[i], extractPath);
            string[] data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));
            File.Delete(Path.Combine(extractPath, "Data.ini"));
            if (File.Exists(Path.Combine(extractPath, "Icon.png")))
            {
                customPrisonIcons.Add(ConvertPNGToSprite(Path.Combine(extractPath, "Icon.png")));
                File.Delete(Path.Combine(extractPath, "Icon.png"));
            }
            else
            {
                customPrisonIcons.Add(Resources.Load<Sprite>("Main Menu Resources/UI Stuff/noicon"));
            }
            if (File.Exists(Path.Combine(extractPath, "Tiles.png")))
            {
                File.Delete(Path.Combine(extractPath, "Tiles.png"));
            }
            if (File.Exists(Path.Combine(extractPath, "Ground.png")))
            {
                File.Delete(Path.Combine(extractPath, "Ground.png"));
            }
            if (File.Exists(Path.Combine(extractPath, "Music.mp3")))
            {
                File.Delete(Path.Combine(extractPath, "Music.mp3"));
            }
            if (GetINIVar("Properties", "POWOutfits", data) == "True")
            {
                customPrisonHasPOWBools.Add(true);
            }
            else
            {
                customPrisonHasPOWBools.Add(false);
            }

            for (int j = 0; j < data.Length; j++)
            {
                data[j] = data[j].Replace("\n", "");
                data[j] = data[j].Replace("\r", "");
                data[j] = data[j].Replace("\u200B", "");
            }
            customPrisonInmateNums.Add(Convert.ToInt32(GetINIVar("Properties", "Inmates", data)));
            customPrisonGuardNums.Add(Convert.ToInt32(GetINIVar("Properties", "Guards", data)));
            customPrisonNames.Add(GetINIVar("Properties", "MapName", data).ToUpper());
        }

        if (!onlyCustom)
        {
            amountOfMainPrisons = mainPrisonPaths.Count;
            amountOfBonusPrisons = bonusPrisonPaths.Count;
        }
        amountOfCustomPrisons = customPrisonPaths.Count;

        if (!doneWithInitialLoad)
        {
            doneWithInitialLoad = true;
        }
    }

    private void ResetLists(bool onlyCustom)
    {
        if (!onlyCustom)
        {
            mainPrisonPaths.Clear();
            mainPrisonIcons.Clear();
            mainPrisonInmateNums.Clear();
            mainPrisonGuardNums.Clear();
            mainPrisonHasPOWBools.Clear();
            mainPrisonNames.Clear();
            amountOfMainPrisons = 0;
            bonusPrisonPaths.Clear();
            bonusPrisonIcons.Clear();
            bonusPrisonInmateNums.Clear();
            bonusPrisonGuardNums.Clear();
            bonusPrisonHasPOWBools.Clear();
            bonusPrisonNames.Clear();
            amountOfBonusPrisons = 0;
        }
        customPrisonPaths.Clear();
        customPrisonIcons.Clear();
        customPrisonInmateNums.Clear();
        customPrisonGuardNums.Clear();
        customPrisonHasPOWBools.Clear();
        customPrisonNames.Clear();
        amountOfCustomPrisons = 0;
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length - i; j++)
                {
                    if (file[j].Contains(varName))
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }

        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');

        return parts[1];
    }
    private Sprite ConvertPNGToSprite(string path)
    {
        byte[] pngBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(pngBytes);

        texture.filterMode = FilterMode.Point;

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100.0f);
    }
}
