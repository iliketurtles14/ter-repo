using System;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class JobBoxes : MonoBehaviour
{
    /*
     * BlueBox - 116
     * CleanLaundry - 29, 39
     * ClothesBox - 152, 148, 178, 135
     * FurnitureBox - 149
     * PlatesBox - 182
     * RedBox - 117
     */
    private InventorySelection selectionScript;
    private MouseCollisionOnItems mcs;
    private Inventory inventoryScript;
    private Schedule scheduleScript;
    private Transform player;
    private Jobs jobsScript;
    private Dictionary<string, string> jobContainerDict = new Dictionary<string, string>() //goes from job box to what job its associated with
    {
        { "BlueBox", "Deliveries" }, { "RedBox", "Deliveries" }, { "CleanLaundry", "Laundry" },
        { "FurnitureBox", "Woodshop" }, { "PlatesBox", "Metalshop" }
    };
    private void Start()
    {
        selectionScript = GetComponent<InventorySelection>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        inventoryScript = GetComponent<Inventory>();
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        player = RootObjectCache.GetRoot("Player").transform;
        jobsScript = GetComponent<Jobs>();
    }
    private void Update()
    {
        if(mcs.isTouchingJobBox && selectionScript.aSlotSelected && Input.GetMouseButtonDown(0))
        {
            PutInBox(mcs.touchedJobBox, selectionScript.selectedSlotNum);
        }
    }
    private void PutInBox(GameObject box, int invIndex)
    {
        int id = inventoryScript.inventory[invIndex].itemData.id;
        int denom = 1;
        
        switch (box.name)
        {
            case "BlueBox":
                if(id == 116)
                {
                    inventoryScript.inventory[invIndex].itemData = null;
                    denom = 19;
                }
                break;
            case "RedBox":
                if(id == 117)
                {
                    inventoryScript.inventory[invIndex].itemData = null;
                    denom = 19;
                }
                break;
            case "CleanLaundry":
                if(id == 29 || id == 39)
                {
                    inventoryScript.inventory[invIndex].itemData = null;
                    denom = 10;
                }
                break;
            case "ClothesBox":
                if(id == 152 || id == 148 || id == 178 || id == 135)
                {
                    inventoryScript.inventory[invIndex].itemData = null;
                    denom = 10;
                }
                break;
            case "FurnitureBox":
                if(id == 149)
                {
                    inventoryScript.inventory[invIndex].itemData = null;
                    denom = 10;
                }
                break;
            case "PlatesBox":
                if(id == 182)
                {
                    inventoryScript.inventory[invIndex].itemData = null;
                    denom = 19;
                }
                break;
        }

        if(scheduleScript.periodCode == "W" && !String.IsNullOrEmpty(player.GetComponent<PlayerCollectionData>().playerData.job))
        {
            if (jobContainerDict[box.name] == player.GetComponent<PlayerCollectionData>().playerData.job)
            {
                jobsScript.AddToQuota(1, denom);
            }
        }
    }
}
