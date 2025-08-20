using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
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
    private GameObject lastTouchedButton;
    public OnMainButtonPress titlePanelScript;
    public Canvas MainMenuCanvas;
    private List<string> mainPrisonPaths = new List<string>();
    public int currentPrisonInmateNum;
    public int currentPrisonGuardNum;
    public bool currentPrisonHasPOW;
    private List<Sprite> mainPrisonIcons = new List<Sprite>();
    private List<int> mainPrisonInmateNums = new List<int>();
    private List<int> mainPrisonGuardNums = new List<int>();
    private List<bool> mainPrisonHasPOWBools = new List<bool>();
    private List<string> mainPrisonNames = new List<string>();
    private Sprite noIconSprite;
    public int amountOfMainPrisons;

    public void Start()
    {
        noIconSprite = Resources.Load<Sprite>("Main Menu Resources/UI Stuff");
    }
    public void OnEnable()
    {
        foreach (string path in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Prisons", "MainPrisons")))
        {
            if(Path.GetExtension(path) == ".zmap")
            {
                mainPrisonPaths.Add(path);
            }
        }

        for(int i = 0; i < mainPrisonPaths.Count; i++)
        {
            string extractPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "MainPrisons") + Path.DirectorySeparatorChar;
            ZipFile.ExtractToDirectory(mainPrisonPaths[i], extractPath);
            string[] data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));
            File.Delete(Path.Combine(extractPath, "Data.ini"));
            if(File.Exists(Path.Combine(extractPath, "Icon.png")))
            {
                mainPrisonIcons.Add(ConvertPNGToSprite(Path.Combine(extractPath, "Icon.png")));
            }
            else
            {
                mainPrisonIcons.Add(noIconSprite);
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
            if(GetINIVar("Properties", "POWOutfits", data) == "True")
            {
                mainPrisonHasPOWBools.Add(true);
            }
            else
            {
                mainPrisonHasPOWBools.Add(false);
            }
            mainPrisonInmateNums.Add(Convert.ToInt32(GetINIVar("Properties", "Inmates", data)));
            mainPrisonGuardNums.Add(Convert.ToInt32(GetINIVar("Properties", "Guards", data)));
            mainPrisonNames.Add(GetINIVar("Properties", "MapName", data).ToUpper());
        }
        amountOfMainPrisons = mainPrisonPaths.Count;
    }
    public void Update()
    {

        if(whichPrison == 0)
        {
            LeftArrow.GetComponent<Image>().enabled = false;
            LeftArrow.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            LeftArrow.GetComponent<Image>().enabled = true;
            LeftArrow.GetComponent<BoxCollider2D>().enabled = true;
        }

        if (whichPrison == amountOfMainPrisons)
        {
            RightArrow.GetComponent<Image>().enabled = false;
            RightArrow.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            RightArrow.GetComponent<Image>().enabled = true;
            RightArrow.GetComponent<BoxCollider2D>().enabled = true;
        }

        prisonText.text = mainPrisonNames[whichPrison];
        CurrentPrisonObject.GetComponent<Image>().sprite = mainPrisonIcons[whichPrison];
        currentPrisonGuardNum = mainPrisonGuardNums[whichPrison];
        currentPrisonInmateNum = mainPrisonInmateNums[whichPrison];
        currentPrisonHasPOW = mainPrisonHasPOWBools[whichPrison];
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
