using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSet : MonoBehaviour
{
    public Transform savePanel;
    private Dictionary<int, Sprite> headDict;
    private Dictionary<int, Vector2> sizeDict = new Dictionary<int, Vector2>
    {
        { 4, new Vector2(10, 8) }, { 0, new Vector2(10, 9) }, { 1, new Vector2(10, 9) },
        { 2, new Vector2(10, 8) }, { 3, new Vector2(10, 9) }, { 5, new Vector2(10, 10) },
        { 6, new Vector2(12, 9) }, { 7, new Vector2(10, 9) }, { 8, new Vector2(10, 8) },
        { 30, new Vector2(10, 9) }, { 19, new Vector2(10, 9) }, { 9, new Vector2(14, 11) }
    };
    public void SetHeadDict()
    {
        DataSender ds = DataSender.instance;
        headDict = new Dictionary<int, Sprite>
        {
            { 4, ds.UIImages[298] }, { 0, ds.UIImages[299] }, { 1, ds.UIImages[300] },
            { 2, ds.UIImages[301] }, { 3, ds.UIImages[302] }, { 5, ds.UIImages[303] },
            { 6, ds.UIImages[304] }, { 7, ds.UIImages[305] }, { 8, ds.UIImages[306] },
            { 30, ds.UIImages[525] }, { 19, ds.UIImages[533] }, { 9, ds.UIImages[537] }
        };
    }
    public void SetSaves()
    {
        string saveFolderPath = Path.Combine(Application.streamingAssetsPath, "Saves");
        bool save0 = File.Exists(Path.Combine(saveFolderPath, "Save0.ini"));
        bool save1 = File.Exists(Path.Combine(saveFolderPath, "Save1.ini"));
        bool save2 = File.Exists(Path.Combine(saveFolderPath, "Save2.ini"));

        List<bool> saveBools = new List<bool>
        {
            save0, save1, save2
        };

        for(int i = 0; i < 3; i++)
        {
            if (!saveBools[i])
            {
                DeactivateSaveSlot(i);
                continue;
            }

            ActivateSaveSlot(i);
        }
    }
    public void ActivateSaveSlot(int slot)
    {
        string[] saveFile = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Saves", "Save" + slot.ToString() + ".ini"));
        string playerName = GetINIVar("Player", "Name", saveFile);
        string prisonName = GetINIVar("Map", "Name", saveFile);
        string day = GetINIVar("Map", "Time", saveFile).Split(",")[2];

        string saveText = playerName + " - Day " + day + "\n" + prisonName;
        Transform button = savePanel.Find("Save" + slot.ToString() + "Button");
        button.Find("NewGameText").gameObject.SetActive(false);
        button.Find("LoadGameText").GetComponent<TextMeshProUGUI>().text = saveText;
        button.Find("LoadGameText").gameObject.SetActive(true);
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<EventTrigger>().enabled = true;

        int charNum = Convert.ToInt32(GetINIVar("Player", "Character", saveFile));
        if (!headDict.ContainsKey(charNum))
        {
            charNum = 0;
        }
        Vector2 size = sizeDict[charNum];
        Sprite spr = headDict[charNum];

        button.Find("Head").GetComponent<Image>().sprite = spr;
        button.Find("Head").GetComponent<RectTransform>().sizeDelta = size;
        button.Find("Head").gameObject.SetActive(true);

        button.Find("YesButton").gameObject.SetActive(false);
        button.Find("NoButton").gameObject.SetActive(false);
        button.Find("DeleteButton").gameObject.SetActive(true);
        button.Find("SureText").gameObject.SetActive(false);
    }
    public void DeactivateSaveSlot(int slot)
    {
        Transform button = savePanel.Find("Save" + slot.ToString() + "Button");
        button.Find("NewGameText").gameObject.SetActive(true);
        button.Find("LoadGameText").gameObject.SetActive(false);
        button.Find("Head").gameObject.SetActive(false);
        button.Find("YesButton").gameObject.SetActive(false);
        button.Find("NoButton").gameObject.SetActive(false);
        button.Find("DeleteButton").gameObject.SetActive(false);
        button.Find("SureText").gameObject.SetActive(false);
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<EventTrigger>().enabled = true;
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Contains("[") && file[j].Contains("]") && j != i)
                    {
                        line = null;
                        break;
                    }
                    if (file[j].Split('=')[0] == varName)
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
}
