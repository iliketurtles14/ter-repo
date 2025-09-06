using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySelection : MonoBehaviour
{
    private Image slot1SelectionImage;
    private Image slot2SelectionImage;
    private Image slot3SelectionImage;
    private Image slot4SelectionImage;
    private Image slot5SelectionImage;
    private Image slot6SelectionImage;
    public bool slot1Selected;
    public bool slot2Selected;
    public bool slot3Selected;
    public bool slot4Selected;
    public bool slot5Selected;
    public bool slot6Selected;
    public bool aSlotSelected;
    private GameObject InventoryCanvas;
    private MouseCollisionOnItems mouseCollisionScript;
    private bool isTouchingSlotWithItem;
    private bool isTouchingSlot;
    private GameObject touchedSlot;
    private string slotName;
    private bool hasItem;
    private Inventory inventoryScript;
    private List<InventoryItem> inventoryList;

    public void Start()
    {
        InventoryCanvas = RootObjectCache.GetRoot("InventoryCanvas");
        mouseCollisionScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        inventoryScript = GetComponent<Inventory>();

        //define the Images
        Transform slot1 = InventoryCanvas.transform.Find("SelectionPanel/SelectOutline1");
        Transform slot2 = InventoryCanvas.transform.Find("SelectionPanel/SelectOutline2");
        Transform slot3 = InventoryCanvas.transform.Find("SelectionPanel/SelectOutline3");
        Transform slot4 = InventoryCanvas.transform.Find("SelectionPanel/SelectOutline4");
        Transform slot5 = InventoryCanvas.transform.Find("SelectionPanel/SelectOutline5");
        Transform slot6 = InventoryCanvas.transform.Find("SelectionPanel/SelectOutline6");
        slot1SelectionImage = slot1.GetComponent<Image>();
        slot2SelectionImage = slot2.GetComponent<Image>();
        slot3SelectionImage = slot3.GetComponent<Image>();
        slot4SelectionImage = slot4.GetComponent<Image>();
        slot5SelectionImage = slot5.GetComponent<Image>();
        slot6SelectionImage = slot6.GetComponent<Image>();
        //disable the images
        slot1SelectionImage.enabled = false;
        slot2SelectionImage.enabled = false;
        slot3SelectionImage.enabled = false;
        slot4SelectionImage.enabled = false;
        slot5SelectionImage.enabled = false;
        slot6SelectionImage.enabled = false;

    }
    public void Update()
    {
        //check
        isTouchingSlot = mouseCollisionScript.isTouchingInvSlot;
        inventoryList = inventoryScript.inventory;
        if (isTouchingSlot)
        {
            touchedSlot = mouseCollisionScript.touchedInvSlot;
            slotName = touchedSlot.name;
        }
        else { slotName = null; }
        
        switch (slotName)
        {
            case "Slot1":
                if (inventoryList[0].itemData != null)
                {
                    isTouchingSlotWithItem = true;
                }
                else { isTouchingSlotWithItem = false; }
                break;
            case "Slot2":
                if (inventoryList[1].itemData != null)
                {
                    isTouchingSlotWithItem = true;
                }
                else { isTouchingSlotWithItem = false; }
                break;
            case "Slot3":
                if (inventoryList[2].itemData != null)
                {
                    isTouchingSlotWithItem = true;
                }
                else { isTouchingSlotWithItem = false; }
                break;
            case "Slot4":
                if (inventoryList[3].itemData != null)
                {
                    isTouchingSlotWithItem = true;
                }
                else { isTouchingSlotWithItem = false; }
                break;
            case "Slot5":
                if (inventoryList[4].itemData != null)
                {
                    isTouchingSlotWithItem = true;
                }
                else { isTouchingSlotWithItem = false; }
                break;
            case "Slot6":
                if (inventoryList[5].itemData != null)
                {
                    isTouchingSlotWithItem = true;
                }
                else { isTouchingSlotWithItem = false; }
                break;
            default: isTouchingSlotWithItem = false; break;
        }

        //check if any other slot is already selected
        if(slot1Selected || slot2Selected || slot3Selected || slot4Selected || slot5Selected || slot6Selected)
        {
            aSlotSelected = true;
        }
        else { aSlotSelected = false; }

        //when clicking an item in the slot
        if (Input.GetMouseButtonDown(0) && isTouchingSlotWithItem)
        {
            ClearAllSelections();

            switch (slotName)
            {
                case "Slot1": slot1SelectionImage.enabled = true; slot1Selected = true; break;
                case "Slot2": slot2SelectionImage.enabled = true; slot2Selected = true; break;
                case "Slot3": slot3SelectionImage.enabled = true; slot3Selected = true; break;
                case "Slot4": slot4SelectionImage.enabled = true; slot4Selected = true; break;
                case "Slot5": slot5SelectionImage.enabled = true; slot5Selected = true; break;
                case "Slot6": slot6SelectionImage.enabled = true; slot6Selected = true; break;
            }
        }

        //check if the selected item is dropped

        if ((slot1Selected && inventoryList[0].itemData == null) ||
            (slot2Selected && inventoryList[1].itemData == null) ||
            (slot3Selected && inventoryList[2].itemData == null) ||
            (slot4Selected && inventoryList[3].itemData == null) ||
            (slot5Selected && inventoryList[4].itemData == null) ||
            (slot6Selected && inventoryList[5].itemData == null))
        {
            ClearAllSelections();
        }

        //if you click on screen not on another slot
        if (!isTouchingSlot && Input.GetMouseButtonDown(0))
        {
            ClearAllSelections();
        }
        
        //if you click on an empty slot
        if((isTouchingSlot && !isTouchingSlotWithItem && Input.GetMouseButtonDown(0) && slot1Selected && slotName != "Slot1") ||
            (isTouchingSlot && !isTouchingSlotWithItem && Input.GetMouseButtonDown(0) && slot2Selected && slotName != "Slot2") ||
            (isTouchingSlot && !isTouchingSlotWithItem && Input.GetMouseButtonDown(0) && slot3Selected && slotName != "Slot3") ||
            (isTouchingSlot && !isTouchingSlotWithItem && Input.GetMouseButtonDown(0) && slot4Selected && slotName != "Slot4") ||
            (isTouchingSlot && !isTouchingSlotWithItem && Input.GetMouseButtonDown(0) && slot5Selected && slotName != "Slot5") ||
            (isTouchingSlot && !isTouchingSlotWithItem && Input.GetMouseButtonDown(0) && slot6Selected && slotName != "Slot6"))
        {
            ClearAllSelections();
        }

        //keybind version (1-6)
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList[0].itemData != null)
        {
            ClearAllSelections();
            slot1Selected = true;
            slot1SelectionImage.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList[1].itemData != null)
        {
            ClearAllSelections();
            slot2Selected = true;
            slot2SelectionImage.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList[2].itemData != null)
        {
            ClearAllSelections();
            slot3Selected = true;
            slot3SelectionImage.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList[3].itemData != null)
        {
            ClearAllSelections();
            slot4Selected = true;
            slot4SelectionImage.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && inventoryList[4].itemData != null)
        {
            ClearAllSelections();
            slot5Selected = true;
            slot5SelectionImage.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && inventoryList[5].itemData != null)
        {
            ClearAllSelections();
            slot6Selected = true;
            slot6SelectionImage.enabled = true;
        }
    }
    public void ClearAllSelections()
    {
        slot1SelectionImage.enabled = false;
        slot2SelectionImage.enabled = false;
        slot3SelectionImage.enabled = false;
        slot4SelectionImage.enabled = false;
        slot5SelectionImage.enabled = false;
        slot6SelectionImage.enabled = false;
        slot1Selected = false;
        slot2Selected = false;
        slot3Selected = false;
        slot4Selected = false;
        slot5Selected = false;
        slot6Selected = false;
        aSlotSelected = false;
    }
}
