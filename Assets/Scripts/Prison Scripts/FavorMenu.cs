using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FavorMenu : MonoBehaviour
{
    private Map currentMap;
    private string[] items;
    private PlayerIDInv playerIDInvScript;
    private PauseController pc;
    private MissionAsk missionAskScript;
    private MouseCollisionOnItems mcs;
    private Transform mc;
    private bool favorMenuIsOpen;
    private void Start()
    {
        playerIDInvScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        missionAskScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("MissionPanel").GetComponent<MissionAsk>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        CloseMenu(false);
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
        items = currentMap.items;
    }
    private void Update()
    {
        if (!mcs.isTouchingIDPanel && !mcs.isTouchingButton && !mcs.isTouchingInvSlot && !mcs.isTouchingExtra && !mcs.isTouchingIDSlot && Input.GetMouseButtonDown(0) && favorMenuIsOpen)
        {
            CloseMenu(false);
        }
    }
    public void SetFavors(List<Mission> missions)
    {
        foreach(Transform child in transform.Find("MissionScrollView").Find("Viewport").Find("Content"))
        {
            if(child.name != "PlaceholderMission")
            {
                Destroy(child.gameObject);
            }
        }
        
        foreach(Mission mission in missions)
        {
            GameObject missionObj = Instantiate(transform.Find("MissionScrollView").Find("Viewport").Find("Content").Find("PlaceholderMission").gameObject);
            missionObj.SetActive(true);
            missionObj.name = "Mission";
            missionObj.transform.Find("MessageText").GetComponent<TextMeshProUGUI>().text = GetMissionText(mission);
            missionObj.transform.parent = transform.Find("MissionScrollView").Find("Viewport").Find("Content");
            missionObj.transform.localScale = Vector3.one;
        }
    }
    private string GetMissionText(Mission mission)
    {
        string msg = "";
        string str = "";
        switch (mission.type)
        {
            case "stealInmateBody":
            case "stealDesk":
            case "stealGuardBody":
                str = mission.item.ToString();
                if(str.Length == 1)
                {
                    str = "00" + str;
                }
                else if(str.Length == 2)
                {
                    str = "0" + str;
                }
                msg = "Retrieve " + mission.giver + "'s " + GetINIVar(str, "Name", items) + " from " + mission.target + ".";
                break;
            case "inmateBeat":
            case "guardBeat":
                msg = "Beat up " + mission.target + " for " + mission.giver + ".";
                break;
            case "give":
                str = mission.item.ToString();
                if (str.Length == 1)
                {
                    str = "00" + str;
                }
                else if (str.Length == 2)
                {
                    str = "0" + str;
                }
                msg = "Find a " + GetINIVar(str, "Name", items) + " for " + mission.giver + ".";
                break;
            case "distract":
                msg = "Distraction for " + mission.giver + " at " + mission.period + ".";
                break;
        }
        return msg.Replace("\n", "").Replace("\r", "");
    }
    public void CloseMenu(bool goToPlayerID)
    {
        if (goToPlayerID)
        {
            StartCoroutine(playerIDInvScript.OpenMenu(true));
        }
        else
        {
            mc.Find("Black").GetComponent<Image>().enabled = false;
            pc.Unpause();
        }
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = false;
            }
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (child.GetComponent<EventTrigger>() != null)
            {
                child.GetComponent<EventTrigger>().enabled = false;
            }
        }
        transform.Find("TitleText").gameObject.SetActive(false);
        transform.Find("MissionScrollView").gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        favorMenuIsOpen = false;
    }
    public void OpenMenu()
    {
        favorMenuIsOpen = true;
        SetFavors(missionAskScript.savedMissions);
        
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = true;
            }
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = true;
            }
            if (child.GetComponent<EventTrigger>() != null)
            {
                child.GetComponent<EventTrigger>().enabled = true;
            }
        }
        transform.Find("TitleText").gameObject.SetActive(true);
        transform.Find("MissionScrollView").gameObject.SetActive(true);
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        mc.Find("Black").GetComponent<Image>().enabled = true;
        //you dont have to pause here since the game will already be paused
        playerIDInvScript.CloseMenu();
        mc.Find("Black").GetComponent<Image>().enabled = true;
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
