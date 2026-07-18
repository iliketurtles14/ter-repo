using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNote : MonoBehaviour
{
    private Transform mc;
    private PauseController pc;
    private Transform currentPanel;
    public bool inNote;
    private string playerName;
    private Map currentMap;
    private List<string> wardenNames = new List<string>
    {
        "Dean Hall", "Monkey Alan", "Alberto Valero", "Craig Monkford", "Patrick Garratt", "Chinsworth", "Stuart Foot", "Jim Sterling", "Geoff Lamp", "Paul Soares Jr.", "Davis", "Isaac", "Sparrow"
    };
    private void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        CloseAllNotes();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        playerName = RootObjectCache.GetRoot("Player").GetComponent<PlayerCollectionData>().playerData.displayName.Replace("\n", "");
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    CreateWardenNote("day1", "This is the first day!!!", "Warden diddy heh");
        //}
    }
    private void CloseAllNotes()
    {
        List<Transform> panels = new List<Transform>()
        {
            mc.Find("NoteDay1MenuPanel"),
            mc.Find("NoteLoseJobMenuPanel"),
            mc.Find("NoteGetJobMenuPanel"),
            mc.Find("NoteSolitaryMenuPanel")
        };
        foreach(Transform panel in panels)
        {
            currentPanel = panel;
            CloseWardenNote();
        }
        currentPanel = null;
    }
    public void CloseWardenNote()
    {
        Transform panel = currentPanel;
        foreach (Transform child in panel)
        {
            child.gameObject.SetActive(false);
        }
        panel.GetComponent<BoxCollider2D>().enabled = false;
        panel.GetComponent<Image>().enabled = false;
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
        inNote = false;
    }
    public void CreateWardenNote(string type, string msg, string warden) //day1, loseJob, getJob, solitary
    {
        Transform panel = null;
        switch (type)
        {
            case "day1":
                panel = mc.Find("NoteDay1MenuPanel");
                break;
            case "loseJob":
                panel = mc.Find("NoteLoseJobMenuPanel");
                break;
            case "getJob":
                panel = mc.Find("NoteGetJobMenuPanel");
                break;
            case "solitary":
                panel = mc.Find("NoteSolitaryMenuPanel");
                break;
            default:
                panel = mc.Find("NoteDay1MenuPanel");
                break;
        }
        currentPanel = panel;
        if (msg.Contains("$name"))
        {
            msg = msg.Replace("$name", playerName);
        }
        if (msg.Contains("$warden"))
        {
            msg = msg.Replace("$warden", wardenNames[UnityEngine.Random.Range(0, wardenNames.Count)]);
        }
        if (warden.Contains("$warden"))
        {
            warden = warden.Replace("$warden", wardenNames[UnityEngine.Random.Range(0, wardenNames.Count)]);
        }
        panel.Find("NoteText").GetComponent<TextMeshProUGUI>().text = msg;
        panel.Find("WardenText").GetComponent<TextMeshProUGUI>().text = warden;

        foreach(Transform child in panel)
        {
            child.gameObject.SetActive(true);
        }
        panel.GetComponent<BoxCollider2D>().enabled = true;
        panel.GetComponent<Image>().enabled = true;
        mc.Find("Black").GetComponent<Image>().enabled = true;
        pc.Pause(true);
        inNote = true;
        PSoundController.PlaySound("open");
    }
    public string GetNoteText(string messageType, int intellect)//pretty much js for solitary and job baord stuff
    {
        string message = GetINIVar("Notes", messageType, currentMap.speech);
        if (message.Contains("$intellect"))
        {
            message = message.Replace("$intellect", intellect.ToString());
        }
        if (message.Contains("#"))
        {
            message = message.Replace("#", "\n");
        }

        return message;
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
