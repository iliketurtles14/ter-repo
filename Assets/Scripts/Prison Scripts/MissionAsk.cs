using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionAsk : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Transform mc;
    public List<Mission> savedMissions = new List<Mission>();
    public GameObject currentNPC;
    private PauseController pc;
    private ItemDataCreator creator;
    private Transform aStar;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();
        aStar = RootObjectCache.GetRoot("A*").transform;

        CloseMenu();
    }
    private void Update()
    {
        if(mcs.isTouchingNPC && mcs.touchedNPC.name.Contains("Inmate") && !String.IsNullOrEmpty(mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.mission.type) && Input.GetMouseButtonDown(0))
        {
            AskFavor(mcs.touchedNPC);
        }
    }
    private void AskFavor(GameObject npc)
    {
        currentNPC = npc;
        LoadMessage(npc);
        OpenMenu();
    }
    private void LoadMessage(GameObject npc)
    {
        Mission currentMission = npc.GetComponent<NPCCollectionData>().npcData.mission;
        string msg = currentMission.message;
        int payout = currentMission.pay;
        msg += "\n\nPayout: $" + payout.ToString();

        transform.Find("MessageText").GetComponent<TextMeshProUGUI>().text = msg;
    }
    private void OpenMenu()
    {
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
            if(child.GetComponent<EventTrigger>() != null)
            {
                child.GetComponent<EventTrigger>().enabled = true;
            }
        }
        transform.Find("Header").gameObject.SetActive(true);
        transform.Find("MessageText").gameObject.SetActive(true);
        transform.Find("YesButton").Find("YesText").gameObject.SetActive(true);
        transform.Find("NoButton").Find("NoText").gameObject.SetActive(true);
        transform.Find("MaybeButton").Find("MaybeText").gameObject.SetActive(true);
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        mc.Find("Black").GetComponent<Image>().enabled = true;

        pc.Pause(true);
    }
    public void CloseMenu()
    {
        currentNPC = null;

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
        transform.Find("Header").gameObject.SetActive(false);
        transform.Find("MessageText").gameObject.SetActive(false);
        transform.Find("YesButton").Find("YesText").gameObject.SetActive(false);
        transform.Find("NoButton").Find("NoText").gameObject.SetActive(false);
        transform.Find("MaybeButton").Find("MaybeText").gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        mc.Find("Black").GetComponent<Image>().enabled = false;

        pc.Unpause();
    }
    public void SaveMission(GameObject npc)
    {
        savedMissions.Add(npc.GetComponent<NPCCollectionData>().npcData.mission);
        LoadMission(npc.GetComponent<NPCCollectionData>().npcData.mission);
        CloseMenu();
        npc.GetComponent<NPCCollectionData>().npcData.opinion += 1;
        npc.GetComponent<NPCCollectionData>().npcData.mission = new Mission("", -1, "", "", "", "", -1);
    }
    public void CancelMission(GameObject npc)
    {
        CloseMenu();
        npc.GetComponent<NPCCollectionData>().npcData.mission = new Mission("", -1, "", "", "", "", -1);
    }
    public void MaybeMission()
    {
        CloseMenu();
    }
    public void LoadMission(Mission mission)
    {
        string type = mission.type;

        switch (type)
        {
            case "stealInmateBody":
            case "stealGuardBody":
                ItemData data = creator.CreateItemData(mission.item);
                data.inmateGiveName = mission.giver;
                data.forFavor = true;
                GameObject targetNPC = null;

                foreach(Transform npc in aStar)
                {
                    if(npc.GetComponent<NPCCollectionData>().npcData.displayName == mission.target)
                    {
                        targetNPC = npc.gameObject;
                        break;
                    }
                }

                int i = 0;
                bool inInv = false;
                foreach(NPCInvItem item in targetNPC.GetComponent<NPCCollectionData>().npcData.inventory)
                {
                    if (i == 6)
                    {
                        break;
                    }
                    if (item.itemData == null)
                    {
                        targetNPC.GetComponent<NPCCollectionData>().npcData.inventory[i].itemData = data;
                        inInv = true;
                        break;
                    }
                    i++;
                }
                if (!inInv)
                {
                    targetNPC.GetComponent<NPCCollectionData>().npcData.inventory[5].itemData = data;
                }
                break;
            case "stealDesk":
                data = creator.CreateItemData(mission.item);
                data.inmateGiveName = mission.giver;
                data.forFavor = true;
                targetNPC = null;

                foreach(Transform npc in aStar)
                {
                    if(npc.GetComponent<NPCCollectionData>().npcData.displayName == mission.target)
                    {
                        targetNPC = npc.gameObject;
                        break;
                    }
                }

                GameObject targetDesk = null;

                try
                {
                    targetDesk = targetNPC.GetComponent<NPCCollectionData>().npcData.desk;
                }
                catch //if the inmate doesnt have a desk
                {
                    CancelMission(targetNPC);
                    savedMissions.Remove(mission);
                    return;
                }

                i = 0;
                inInv = false;
                foreach(DeskItem item in targetDesk.GetComponent<DeskData>().deskInv)
                {
                    if(i == 20)
                    {
                        break;
                    }
                    if(item.itemData == null)
                    {
                        targetDesk.GetComponent<DeskData>().deskInv[i].itemData = data;
                        inInv = true;
                        break;
                    }
                    i++;
                }
                if (!inInv)
                {
                    targetDesk.GetComponent<DeskData>().deskInv[19].itemData = data;
                }
                break;
        }
    }
}
