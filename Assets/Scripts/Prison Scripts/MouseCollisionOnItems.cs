using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Diagnostics.Contracts;

public class MouseCollisionOnItems : MonoBehaviour //this started as an item script and is now how i do collision for the whole game :skull:
{
    public List<string> disabledTags = new List<string>();
    private List<Collider2D> hitColliders = new List<Collider2D>();
    private List<GameObject> collidedObjects = new List<GameObject>();
    private List<string> priorityOrder = new List<string>();

    public bool isTouchingDeskSlot;
    public GameObject touchedDeskSlot;
    public bool isTouchingInvSlot;
    public GameObject touchedInvSlot;
    public bool isTouchingWall;
    public GameObject touchedWall;
    public bool isTouchingItem;
    public GameObject touchedItem;
    public bool isTouchingDesk;
    public GameObject touchedDesk;
    public bool isTouchingNPC;
    public GameObject touchedNPC;
    public bool isTouchingBars;
    public GameObject touchedBars;
    public bool isTouchingFence;
    public GameObject touchedFence;
    public bool isTouchingElectricFence;
    public GameObject touchedElectricFence;
    public bool isTouchingFloor;
    public GameObject touchedFloor;
    public bool isTouchingButton;
    public GameObject touchedButton;
    public bool isTouchingIDPanel;
    public GameObject touchedIDPanel;
    public bool isTouchingIDSlot;
    public GameObject touchedIDSlot;
    public bool isTouchingVentCover;
    public GameObject touchedVentCover;
    public bool isTouchingOpenVent;
    public GameObject touchedOpenVent;
    public bool isTouchingSlats;
    public GameObject touchedSlats;
    public bool isTouchingRoofLadder;
    public GameObject touchedRoofLadder;
    public bool isTouchingGroundLadder;
    public GameObject touchedGroundLadder;
    public bool isTouchingVentLadder;
    public GameObject touchedVentLadder;
    public bool isTouchingRoofLedge;
    public GameObject touchedRoofLedge;
    public bool isTouchingHoleDown;
    public GameObject touchedHoleDown;
    public bool isTouchingDirt;
    public GameObject touchedDirt;
    public bool isTouchingEmptyDirt;
    public GameObject touchedEmptyDirt;
    public bool isTouchingHoleUp;
    public GameObject touchedHoleUp;
    public bool isTouchingExtra;
    public GameObject touchedExtra;
    public bool isTouchingRock;
    public GameObject touchedRock;
    public bool isTouchingDeskPanel;
    public GameObject touchedDeskPanel;
    public bool isTouchingEquipment;
    public GameObject touchedEquipment;
    public bool isTouchingReader;
    public GameObject touchedReader;
    public bool isTouchingSittable;
    public GameObject touchedSittable;
    public bool isTouchingNPCInvSlot;
    public GameObject touchedNPCInvSlot;
    public bool isTouchingNPCInvPanel;
    public GameObject touchedNPCInvPanel;
    public bool isTouchingFoodTable;
    public GameObject touchedFoodTable;
    public bool isTouchingGiveSlot;
    public GameObject touchedGiveSlot;
    public bool isTouchingShopSlot;
    public GameObject touchedShopSlot;
    void Update()
    {
        ClearCollisions();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        hitColliders = System.Linq.Enumerable.ToList(Physics2D.OverlapPointAll(mousePosition));
        foreach (var obj in hitColliders)
        {
            collidedObjects.Add(obj.gameObject);
        }

        priorityOrder = new List<string>
        {
            "DeskSlot",
            "InvSlot",
            "IDSlot",
            "NPCInvSlot",
            "GiveSlot",
            "ShopSlot", //REMEMBER WHEN ADDING NEW SLOTS TO ALSO ADD THEM TO THE PAUSECONTROLLER
            "Button",
            "DeskPanel",
            "IDPanel",
            "NPCInvPanel",
            "Extra",
            "NPC",
            "Desk",
            "Ladder(Roof)",
            "Ladder(Vent)",
            "Ladder(Ground)",
            "Item",
            "HoleDown",
            "HoleUp",
            "VentCover",
            "OpenVent",
            "Equipment", //workout stuff
            "Reader",
            "Sittable", //beds, chairs, anything that you can sit on
            "FoodTable",
            "Digable",
            "Dirt",
            "Rock",
            "EmptyDirt",
            "Wall",
            "Bars",
            "Fence",
            "ElectricFence",
            "RoofLedge",
            "Slats"
        };

        GameObject highestPriorityObject = null;
        int highestPriorityIndex = int.MaxValue;

        foreach (GameObject obj in collidedObjects)
        {
            GameObject touchedObject = obj;

            if (disabledTags.Contains(touchedObject.tag))
            {
                continue;
            }

            int priorityIndex = priorityOrder.IndexOf(touchedObject.tag);
            if (priorityIndex != -1 && priorityIndex < highestPriorityIndex)
            {
                highestPriorityIndex = priorityIndex;
                highestPriorityObject = touchedObject;
            }
        }

        if (highestPriorityObject != null)
        {
            switch (priorityOrder[highestPriorityIndex])
            {
                case "DeskSlot":
                    isTouchingDeskSlot = true;
                    touchedDeskSlot = highestPriorityObject;
                    break;
                case "InvSlot":
                    isTouchingInvSlot = true;
                    touchedInvSlot = highestPriorityObject;
                    break;
                case "IDSlot":
                    isTouchingIDSlot = true;
                    touchedIDSlot = highestPriorityObject;
                    break;
                case "NPCInvSlot":
                    isTouchingNPCInvSlot = true;
                    touchedNPCInvSlot = highestPriorityObject;
                    break;
                case "GiveSlot":
                    isTouchingGiveSlot = true;
                    touchedGiveSlot = highestPriorityObject;
                    break;
                case "ShopSlot":
                    isTouchingShopSlot = true;
                    touchedShopSlot = highestPriorityObject;
                    break;
                case "DeskPanel":
                    isTouchingDeskPanel = true;
                    touchedDeskPanel = highestPriorityObject;
                    break;
                case "IDPanel":
                    isTouchingIDPanel = true;
                    touchedIDPanel = highestPriorityObject;
                    break;
                case "NPCInvPanel":
                    isTouchingNPCInvPanel = true;
                    touchedNPCInvPanel = highestPriorityObject;
                    break;
                case "Extra":
                    isTouchingExtra = true;
                    touchedExtra = highestPriorityObject;
                    break;
                case "Button":
                    isTouchingButton = true;
                    touchedButton = highestPriorityObject;
                    break;
                case "NPC":
                    isTouchingNPC = true;
                    touchedNPC = highestPriorityObject;
                    break;
                case "Desk":
                    isTouchingDesk = true;
                    touchedDesk = highestPriorityObject;
                    break;
                case "Ladder(Roof)":
                    isTouchingRoofLadder = true;
                    touchedRoofLadder = highestPriorityObject;
                    break;
                case "Ladder(Vent)":
                    isTouchingVentLadder = true;
                    touchedVentLadder = highestPriorityObject;
                    break;
                case "Ladder(Ground)":
                    isTouchingGroundLadder = true;
                    touchedGroundLadder = highestPriorityObject;
                    break;
                case "Item":
                    isTouchingItem = true;
                    touchedItem = highestPriorityObject;
                    break;
                case "HoleDown":
                    isTouchingHoleDown = true;
                    touchedHoleDown = highestPriorityObject;
                    break;
                case "HoleUp":
                    isTouchingHoleUp = true;
                    touchedHoleUp = highestPriorityObject;
                    break;
                case "VentCover":
                    isTouchingVentCover = true;
                    touchedVentCover = highestPriorityObject;
                    break;
                case "OpenVent":
                    isTouchingOpenVent = true;
                    touchedOpenVent = highestPriorityObject;
                    break;
                case "Equipment":
                    isTouchingEquipment = true;
                    touchedEquipment = highestPriorityObject;
                    break;
                case "Reader":
                    isTouchingReader = true;
                    touchedReader = highestPriorityObject;
                    break;
                case "Sittable":
                    isTouchingSittable = true;
                    touchedSittable = highestPriorityObject;
                    break;
                case "FoodTable":
                    isTouchingFoodTable = true;
                    touchedFoodTable = highestPriorityObject;
                    break;
                case "Digable":
                    isTouchingFloor = true;
                    touchedFloor = highestPriorityObject;
                    break;
                case "Dirt":
                    isTouchingDirt = true;
                    touchedDirt = highestPriorityObject;
                    break;
                case "Rock":
                    isTouchingRock = true;
                    touchedRock = highestPriorityObject;
                    break;
                case "EmptyDirt":
                    isTouchingEmptyDirt = true;
                    touchedEmptyDirt = highestPriorityObject;
                    break;
                case "Wall":
                    isTouchingWall = true;
                    touchedWall = highestPriorityObject;
                    break;
                case "Bars":
                    isTouchingBars = true;
                    touchedBars = highestPriorityObject;
                    break;
                case "Fence":
                    isTouchingFence = true;
                    touchedFence = highestPriorityObject;
                    break;
                case "ElectricFence":
                    isTouchingElectricFence = true;
                    touchedElectricFence = highestPriorityObject;
                    break;
                case "RoofLedge":
                    isTouchingRoofLedge = true;
                    touchedRoofLedge = highestPriorityObject;
                    break;
                case "Slats":
                    isTouchingSlats = true;
                    touchedSlats = highestPriorityObject;
                    break;
            }
        }
    }

    public void DisableTag(string tag)
    {
        foreach(string aTag in disabledTags)
        {
            if(aTag == tag)
            {
                return;
            }
        }
        disabledTags.Add(tag);
    }

    public void EnableTag(string tag)
    {
        disabledTags.Remove(tag);
    }

    public void DisableAllTags()
    {
        foreach(string tag in priorityOrder)
        {
            disabledTags.Add(tag);
        }
    }

    public void EnableAllTags()
    {
        disabledTags.Clear();
    }

    private void ClearCollisions()
    {
        hitColliders.Clear();
        collidedObjects.Clear();

        isTouchingDeskSlot = false;
        touchedDeskSlot = null;
        isTouchingInvSlot = false;
        touchedInvSlot = null;
        isTouchingWall = false;
        touchedWall = null;
        isTouchingItem = false;
        touchedItem = null;
        isTouchingDesk = false;
        touchedDesk = null;
        isTouchingNPC = false;
        touchedNPC = null;
        isTouchingBars = false;
        touchedBars = null;
        isTouchingFence = false;
        touchedFence = null;
        isTouchingElectricFence = false;
        touchedElectricFence = null;
        isTouchingFloor = false;
        touchedFloor = null;
        isTouchingButton = false;
        touchedButton = null;
        isTouchingIDPanel = false;
        touchedIDPanel = null;
        isTouchingIDSlot = false;
        touchedIDSlot = null;
        isTouchingVentCover = false;
        touchedVentCover = null;
        isTouchingOpenVent = false;
        touchedOpenVent = null;
        isTouchingSlats = false;
        touchedSlats = null;
        isTouchingRoofLadder = false;
        touchedRoofLadder = null;
        isTouchingGroundLadder = false;
        touchedGroundLadder = null;
        isTouchingVentLadder = false;
        touchedVentLadder = null;
        isTouchingRoofLedge = false;
        touchedRoofLedge = null;
        isTouchingHoleDown = false;
        touchedHoleDown = null;
        isTouchingDirt = false;
        touchedDirt = null;
        isTouchingEmptyDirt = false;
        touchedEmptyDirt = null;
        isTouchingHoleUp = false;
        touchedHoleUp = null;
        isTouchingExtra = false;
        touchedExtra = null;
        isTouchingRock = false;
        touchedRock = null;
        isTouchingDeskPanel = false;
        touchedDeskPanel = null;
        isTouchingEquipment = false;
        touchedEquipment = null;
        isTouchingReader = false;
        touchedReader = null;
        isTouchingSittable = false;
        touchedSittable = null;
        isTouchingNPCInvSlot = false;
        touchedNPCInvSlot = null;
        isTouchingNPCInvPanel = false;
        touchedNPCInvPanel = null;
        isTouchingFoodTable = false;
        touchedFoodTable = null;
        isTouchingGiveSlot = false;
        touchedGiveSlot = null;
        isTouchingShopSlot = false;
        touchedShopSlot = null;
    }
}
