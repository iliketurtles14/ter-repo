using NUnit.Framework;
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
    private void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        CloseAllNotes();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateWardenNote("day1", "This is the first day!!!", "Warden diddy heh");
        }
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
    }
}
