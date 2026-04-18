using NUnit.Framework;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{
    private GameObject player;
    private MouseCollisionOnItems mcs;
    private Transform aStar;
    private GameObject timeObject;
    private Transform ic;
    private Sittables sittablesScript;
    private bool npcsAreStopped;
    private bool noAnimOnNPCs;
    private bool isPaused = false;

    private List<string> currentDisabledTags = new List<string>();

    private void Start()
    {
        player = RootObjectCache.GetRoot("Player");
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        timeObject = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").gameObject;
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        sittablesScript = GetComponent<Sittables>();

    }
    public void Pause(bool disableInv)
    {
        //player movement
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerCtrl>().enabled = false;
        player.GetComponent<PlayerAnimation>().enabled = false;

        // Only capture state if not already paused
        if (!isPaused)
        {
            //mouse collision stuff - save disabled tags
            foreach(string tag in mcs.disabledTags)
            {
                if (!currentDisabledTags.Contains(tag))
                {
                    currentDisabledTags.Add(tag);
                }
            }
            
            //npc movement - capture state
            bool anyNPCMoving = false;
            bool anyNPCAnim = false;
            foreach(Transform npc in aStar)
            {
                if (npc.GetComponent<AILerp>().canMove)
                {
                    anyNPCMoving = true;
                }
                if ((npc.CompareTag("NPC") && npc.GetComponent<NPCAnimation>().enabled) ||
                    npc.GetComponent<ExtraNPCAnimation>().enabled)
                {
                    anyNPCAnim = true;
                }
            }

            npcsAreStopped = !anyNPCMoving;
            noAnimOnNPCs = !anyNPCAnim;
        }

        // Always set these when pausing
        mcs.DisableAllTags();
        mcs.EnableTag("DeskSlot");
        if (!disableInv)
        {
            mcs.EnableTag("InvSlot");
        }
        mcs.EnableTag("IDSlot");
        mcs.EnableTag("DeskPanel");
        mcs.EnableTag("IDPanel");
        mcs.EnableTag("Button");
        mcs.EnableTag("Extra");
        mcs.EnableTag("NPCInvSlot");
        mcs.EnableTag("NPCInvPanel");
        mcs.EnableTag("GiveSlot");
        mcs.EnableTag("ShopSlot");
        mcs.EnableTag("CraftSlot");
        
        // Always freeze NPCs
        foreach(Transform npc in aStar)
        {
            npc.GetComponent<AILerp>().canMove = false;
            if (npc.CompareTag("NPC"))
            {
                npc.GetComponent<NPCAnimation>().enabled = false;
                npc.GetComponent<NavMeshAgent>().speed = 0;
            }
            else
            {
                npc.GetComponent<ExtraNPCAnimation>().enabled = false;
            }
        }

        //time freeze
        timeObject.GetComponent<Routine>().enabled = false;

        //disable id button and craft button
        ic.Find("PlayerIDButton").GetComponent<EventTrigger>().enabled = false;
        ic.Find("CraftButton").GetComponent<EventTrigger>().enabled = false;

        isPaused = true;
    }

    public void Unpause()
    {
        //player movement
        if(player == null)
        {
            player = RootObjectCache.GetRoot("Player");
        }
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<PlayerCtrl>().enabled = true;
        int actionNum = player.GetComponent<BodyController>().currentActionNum;
        if((actionNum == 2 || actionNum == 15 || actionNum == 11 || actionNum == 4 || actionNum == 1) &&
            !(sittablesScript.onSittable && sittablesScript.onBed))
        {
            player.GetComponent<PlayerAnimation>().enabled = true;
        }
        
        //mouse collision stuff
        if(mcs == null)
        {
            mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        }
        mcs.EnableAllTags();
        foreach(string tag in currentDisabledTags)
        {
            mcs.DisableTag(tag);
        }
        currentDisabledTags.Clear();

        //npc movement
        if (aStar == null)
        {
            aStar = RootObjectCache.GetRoot("A*").transform;
        }
        foreach(Transform npc in aStar)
        {
            if (!npcsAreStopped)
            {
                npc.GetComponent<AILerp>().canMove = true;
            }
            if (!noAnimOnNPCs)
            {
                if (npc.CompareTag("ExtraNPC"))
                {
                    npc.GetComponent<ExtraNPCAnimation>().enabled = true;
                }
                else
                {
                    npc.GetComponent<NPCAnimation>().enabled = true;
                }
            }
            if (!npc.CompareTag("ExtraNPC"))
            {
                npc.GetComponent<NavMeshAgent>().speed = 8;
            }
        }

        //time unfreeze
        if(timeObject == null)
        {
            timeObject = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").gameObject;
        }
        timeObject.GetComponent<Routine>().enabled = true;

        //enable id button and craft button
        if(ic == null)
        {
            ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        }
        ic.Find("PlayerIDButton").GetComponent<EventTrigger>().enabled = true;
        ic.Find("CraftButton").GetComponent<EventTrigger>().enabled = true;

        isPaused = false;
    }
}
