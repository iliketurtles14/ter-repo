using NUnit.Framework;
using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PauseController : MonoBehaviour
{
    private GameObject player;
    private MouseCollisionOnItems mcs;
    private Transform aStar;
    private GameObject timeObject;
    private Transform ic;
    private Sittables sittablesScript;

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

        //mouse collision stuff
        foreach(string tag in mcs.disabledTags)
        {
            currentDisabledTags.Add(tag);
        }

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
        
        //npc movement
        foreach(GameObject npc in aStar)
        {
            npc.GetComponent<AILerp>().speed = 0;
            npc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            npc.GetComponent<NPCAnimation>().enabled = false;
            npc.GetComponent<NavMeshAgent>().speed = 0;
        }

        //time freeze
        timeObject.GetComponent<Routine>().enabled = false;

        //disable id button
        ic.Find("PlayerIDButton").GetComponent<BoxCollider2D>().enabled = false;
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

        //npc movement
        if (aStar == null)
        {
            aStar = RootObjectCache.GetRoot("A*").transform;
        }
        foreach(GameObject npc in aStar)
        {
            npc.GetComponent<AILerp>().speed = 8;
            npc.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            npc.GetComponent<NPCAnimation>().enabled = true;
            npc.GetComponent<NavMeshAgent>().speed = 8;
        }

        //time unfreeze
        if(timeObject == null)
        {
            timeObject = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").gameObject;
        }
        timeObject.GetComponent<Routine>().enabled = true;

        //enable id button
        if(ic == null)
        {
            ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        }
        ic.Find("PlayerIDButton").GetComponent<BoxCollider2D>().enabled = true;
    }
}
