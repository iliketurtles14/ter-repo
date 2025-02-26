using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System;
using System.Drawing;

public class Tooltips : MonoBehaviour
{
    public Transform PlayerTransform;
    public Canvas InventoryCanvas;
    private Transform TooltipPanelTransform;
    public GameObject TooltipMid;
    public GameObject TooltipSide;
    public GameObject TooltipTextBox;
    private string toPrint;
    private string printDurability;
    private int textWidth;
    public bool showingTooltip;
    public string tooltipType; //invItem, groundItem, deskItem, wall, 
    private List<InventoryItem> inventoryList;
    private List<DeskItem> deskInvList;
    public Inventory inventoryScript;
    public DeskInv deskInvScript;
    public MouseCollisionOnItems mouseCollisionScript;
    public InventorySelection selectionScript;
    public ItemBehaviours itemBehavioursScript;
    private GameObject touchedInvSlot;
    private bool isTouchingInvSlot;
    private int invSlotNumber; //starts at 0
    private int deskSlotNumber;
    private bool isTouchingItem;
    private GameObject touchedItem;
    private float wallDistance;
    private float fenceDistance;
    private float barsDistance;
    private float ventCoverDistance;
    private float slatsDistance;
    private int printedInvSlotNumber;
    private int printedDeskSlotNumber;
    private TileData printedTileData;
    private int printedTileDurability;
    private int printedItemDurability;
    private string changeType;
    private GameObject currentTouchedItem;
    private bool isScrewingVent;
    private bool isScrewingSlats;
    public void Start()
    {
    }
    public void Update()
    {
        //update these vars
        inventoryList = inventoryScript.inventory;
        deskInvList = deskInvScript.deskInv;
        touchedInvSlot = mouseCollisionScript.touchedInvSlot;
        isTouchingInvSlot = mouseCollisionScript.isTouchingInvSlot;
        isTouchingItem = mouseCollisionScript.isTouchingItem;
        touchedItem = mouseCollisionScript.touchedItem;


        ///ITEMS
        //for inventory items
        if (isTouchingInvSlot)
        {
            switch (touchedInvSlot.name)
            {
                case "Slot1": invSlotNumber = 0; break;
                case "Slot2": invSlotNumber = 1; break;
                case "Slot3": invSlotNumber = 2; break;
                case "Slot4": invSlotNumber = 3; break;
                case "Slot5": invSlotNumber = 4; break;
                case "Slot6": invSlotNumber = 5; break;
                default: break;
            }
        } else if (!isTouchingInvSlot) { invSlotNumber = -1; }

        if (isTouchingInvSlot && inventoryList[invSlotNumber].itemData != null && !showingTooltip)
        {
            if (inventoryList[invSlotNumber].itemData.durability != -1)
            {
                printedInvSlotNumber = invSlotNumber;
                toPrint = inventoryList[invSlotNumber].itemData.displayName;
                printedItemDurability = inventoryList[invSlotNumber].itemData.currentDurability;
                printDurability = " (" + inventoryList[invSlotNumber].itemData.currentDurability + "%)";
            }
            else if (inventoryList[invSlotNumber].itemData.durability == -1)
            {
                printedInvSlotNumber = invSlotNumber;
                toPrint = inventoryList[invSlotNumber].itemData.displayName;
                printDurability = "";
            }
            tooltipType = "invItem";
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if ((showingTooltip && tooltipType == "invItem" && !isTouchingInvSlot))
        {
            DestroyTooltip();
            return;
        } 
        else if ((showingTooltip && tooltipType == "invItem" && isTouchingInvSlot && inventoryList[invSlotNumber].itemData == null) ||
            (showingTooltip && tooltipType == "invItem" && isTouchingInvSlot && inventoryList[invSlotNumber].itemData.displayName != toPrint) ||
            (showingTooltip && tooltipType == "invItem" && isTouchingInvSlot && inventoryList[invSlotNumber].itemData.currentDurability != inventoryList[printedInvSlotNumber].itemData.currentDurability))
        {
            DestroyTooltip();
            return;
        }
        if (mouseCollisionScript.isTouchingInvSlot && showingTooltip && inventoryList[invSlotNumber].itemData.currentDurability != printedItemDurability)
        {
            changeType = "invItemDurability";
            ChangeTooltipText(changeType);
            return;
        }
        //for desk items
        if (mouseCollisionScript.isTouchingDeskSlot)
        {
            for (int i = 1; i <= 20; i++)
            {
                if (mouseCollisionScript.touchedDeskSlot.name == "Slot" + i)
                {
                    deskSlotNumber = i - 1;
                    break;
                }
            }
        }
        if(mouseCollisionScript.isTouchingDeskSlot && deskInvList[deskSlotNumber].itemData != null && !showingTooltip)
        {
            if (deskInvList[deskSlotNumber].itemData.durability != -1)
            {
                printedDeskSlotNumber = deskSlotNumber;
                toPrint = deskInvList[deskSlotNumber].itemData.displayName;
                printedItemDurability = deskInvList[deskSlotNumber].itemData.currentDurability;
                printDurability = " (" + deskInvList[deskSlotNumber].itemData.currentDurability + "%)";
            }
            else if (deskInvList[deskSlotNumber].itemData.durability == -1)
            {
                printedDeskSlotNumber = deskSlotNumber;
                toPrint = deskInvList[deskSlotNumber].itemData.displayName;
                printDurability = "";
            }
            tooltipType = "deskItem";
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "deskItem" && !mouseCollisionScript.isTouchingDeskSlot)
        {
            DestroyTooltip();
            return;
        }
        else if ((showingTooltip && tooltipType == "deskItem" && mouseCollisionScript.isTouchingDeskSlot && deskInvList[deskSlotNumber].itemData == null) ||
            (showingTooltip && tooltipType == "deskItem" && mouseCollisionScript.isTouchingDeskSlot && deskInvList[deskSlotNumber].itemData.displayName != toPrint) ||
            (showingTooltip && tooltipType == "deskItem" && mouseCollisionScript.isTouchingDeskSlot && deskInvList[deskSlotNumber].itemData.currentDurability != deskInvList[printedDeskSlotNumber].itemData.currentDurability))
        {
            DestroyTooltip();
            return;
        }
        else if(mouseCollisionScript.isTouchingDeskSlot && showingTooltip && deskInvList[deskSlotNumber].itemData == null)
        {
            DestroyTooltip();
            return;
        }

            //for ground items
        if (isTouchingItem && !showingTooltip)
        {
            currentTouchedItem = mouseCollisionScript.touchedItem;
            toPrint = touchedItem.GetComponent<ItemCollectionData>().itemData.displayName;
            tooltipType = "groundItem";
            DrawTooltip(GetWidth(toPrint), toPrint);
            return;
        }else if(showingTooltip && tooltipType == "groundItem" && !isTouchingItem)
        {
            DestroyTooltip();
            return;
        }
        
        if(isTouchingItem && showingTooltip && mouseCollisionScript.touchedItem != currentTouchedItem)
        {
            DestroyTooltip();
            return;
        }
        ///TILES
        //walls
        if (mouseCollisionScript.isTouchingWall)
        {
            wallDistance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedWall.transform.position);
        }
        if (!showingTooltip && mouseCollisionScript.isTouchingWall && wallDistance <= 2.4f && itemBehavioursScript.selectedChippingItem)
        {
            toPrint = "Chip Wall";
            printedTileDurability = mouseCollisionScript.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mouseCollisionScript.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "wall";
            printedTileData = mouseCollisionScript.touchedWall.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }else if((showingTooltip && tooltipType == "wall") && 
            (!mouseCollisionScript.isTouchingWall || !itemBehavioursScript.selectedChippingItem))
        {
            DestroyTooltip();
            return;
        }
        else if(showingTooltip && tooltipType == "wall" &&  mouseCollisionScript.isTouchingWall && itemBehavioursScript.selectedChippingItem && mouseCollisionScript.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if(mouseCollisionScript.isTouchingWall && showingTooltip && mouseCollisionScript.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }
        //fences
        if (mouseCollisionScript.isTouchingFence)
        {
            fenceDistance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedFence.transform.position);
        }
        if(!showingTooltip && mouseCollisionScript.isTouchingFence && fenceDistance <= 2.4f && itemBehavioursScript.selectedCuttingItem)
        {
            toPrint = "Cut Fence";
            printedTileDurability = mouseCollisionScript.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mouseCollisionScript.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "fence";
            printedTileData = mouseCollisionScript.touchedFence.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "fence" && 
            (!mouseCollisionScript.isTouchingFence || !itemBehavioursScript.selectedCuttingItem))
        {
            DestroyTooltip();
            return;
        }
        else if (showingTooltip && tooltipType == "fence" && mouseCollisionScript.isTouchingFence && itemBehavioursScript.selectedCuttingItem && mouseCollisionScript.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mouseCollisionScript.isTouchingFence && showingTooltip && mouseCollisionScript.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }

        //bars
        if (mouseCollisionScript.isTouchingBars)
        {
            barsDistance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedBars.transform.position);
        }
        if(!showingTooltip && mouseCollisionScript.isTouchingBars && barsDistance <= 2.4f && itemBehavioursScript.selectedCuttingItem)
        {
            toPrint = "Cut Bars";
            printedTileDurability = mouseCollisionScript.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mouseCollisionScript.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "bars";
            printedTileData = mouseCollisionScript.touchedBars.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "bars" &&
            (!mouseCollisionScript.isTouchingBars || !itemBehavioursScript.selectedCuttingItem))
        {
            DestroyTooltip();
            return;
        }
        else if (showingTooltip && tooltipType == "bars" && mouseCollisionScript.isTouchingBars && itemBehavioursScript.selectedCuttingItem && mouseCollisionScript.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mouseCollisionScript.isTouchingBars && showingTooltip && mouseCollisionScript.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }
        //vent covers (ONLY BIG BECAUSE UNSCREWING AND CUTTING CAPABILITIES)
        if (mouseCollisionScript.isTouchingVentCover)
        {
            ventCoverDistance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedVentCover.transform.position);
        }
        if (itemBehavioursScript.selectedVentBreakingItem || itemBehavioursScript.selectedCuttingItem)
        {
            if (!showingTooltip && mouseCollisionScript.isTouchingVentCover && ventCoverDistance <= 2.4f)
            {
                if (itemBehavioursScript.selectedVentBreakingItem)
                {
                    toPrint = "Unscrew Vent";
                    isScrewingVent = true;
                }
                else
                {
                    toPrint = "Cut Vent";
                    isScrewingVent = false;
                }
                printedTileDurability = mouseCollisionScript.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability;
                printDurability = " (" + mouseCollisionScript.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
                tooltipType = "ventCover";
                printedTileData = mouseCollisionScript.touchedVentCover.GetComponent<TileCollectionData>().tileData;
                DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
                return;
            }
        }
        if (isScrewingVent)
        {
            if (showingTooltip && tooltipType == "ventCover" &&
                (!mouseCollisionScript.isTouchingVentCover || !itemBehavioursScript.selectedVentBreakingItem))
            {
                DestroyTooltip();
                return;
            }
        }
        else if (!isScrewingVent)
        {
            if (showingTooltip && tooltipType == "ventCover" &&
                (!mouseCollisionScript.isTouchingVentCover || !itemBehavioursScript.selectedCuttingItem))
            {
                DestroyTooltip();
                return;
            }
        }
        else if (showingTooltip && tooltipType == "ventCover" && mouseCollisionScript.isTouchingVentCover && (itemBehavioursScript.selectedVentBreakingItem || itemBehavioursScript.selectedCuttingItem) && mouseCollisionScript.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mouseCollisionScript.isTouchingVentCover && showingTooltip && mouseCollisionScript.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }

        //slats
        if (mouseCollisionScript.isTouchingSlats)
        {
            slatsDistance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedSlats.transform.position);
        }
        if (itemBehavioursScript.selectedVentBreakingItem || itemBehavioursScript.selectedCuttingItem)
        {
            if (!showingTooltip && mouseCollisionScript.isTouchingSlats && slatsDistance <= 2.4f)
            {
                if (itemBehavioursScript.selectedVentBreakingItem)
                {
                    toPrint = "Unscrew Slats";
                    isScrewingSlats = true;
                }
                else
                {
                    toPrint = "Cut Slats";
                    isScrewingSlats = false;
                }
                printedTileDurability = mouseCollisionScript.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability;
                printDurability = " (" + mouseCollisionScript.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
                tooltipType = "slats";
                printedTileData = mouseCollisionScript.touchedSlats.GetComponent<TileCollectionData>().tileData;
                DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
                return;
            }
        }
        if (isScrewingSlats)
        {
            if (showingTooltip && tooltipType == "slats" &&
                (!mouseCollisionScript.isTouchingSlats || !itemBehavioursScript.selectedVentBreakingItem))
            {
                DestroyTooltip();
                return;
            }
        }
        else if (!isScrewingSlats)
        {
            if (showingTooltip && tooltipType == "slats" &&
                (!mouseCollisionScript.isTouchingSlats || !itemBehavioursScript.selectedCuttingItem))
            {
                DestroyTooltip();
                return;
            }
        }
        else if (showingTooltip && tooltipType == "slats" && mouseCollisionScript.isTouchingSlats && (itemBehavioursScript.selectedVentBreakingItem || itemBehavioursScript.selectedCuttingItem) && mouseCollisionScript.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mouseCollisionScript.isTouchingSlats && showingTooltip && mouseCollisionScript.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }


    }

    public void DrawTooltip(int width, string text)
    {
        TooltipPanelTransform = InventoryCanvas.transform.Find("TooltipPanel");
        //draw the tooltip
        Instantiate(TooltipSide, TooltipPanelTransform);
        for(int i = 1; i <= width - 2; i++)
        {
            Instantiate(TooltipMid, TooltipPanelTransform);
        }
        Instantiate(TooltipSide, TooltipPanelTransform);

        //make the textbox
        TooltipTextBox.GetComponent<RectTransform>().sizeDelta = new Vector2(width * 5, 80);
        TooltipTextBox.GetComponent<TextMeshProUGUI>().text = text;
        Instantiate(TooltipTextBox, InventoryCanvas.transform);

        showingTooltip = true;
    }
    public void DestroyTooltip()
    {
        foreach(Transform child in TooltipPanelTransform)
        {
            Destroy(child.gameObject);
        }
        Destroy(InventoryCanvas.transform.Find("TooltipText(Clone)").gameObject);
        showingTooltip = false;
        tooltipType = null;
    }

    public int GetWidth(string text)
    {
        textWidth = 0;
        int characterAmount;
        characterAmount = text.Length;
        List<char> charList = new List<char>(text);

        Dictionary<char, int> charWidths = new Dictionary<char, int>
        {
            {'A', 5}, {'B', 5}, {'C', 5}, {'D', 5}, {'E', 5}, {'F', 5}, {'G', 5}, {'H', 5}, {'I', 5}, {'J', 5}, {'K', 5}, {'L', 5}, {'M', 7}, {'N', 5}, {'O', 5}, {'P', 5}, {'Q', 5}, {'R', 5}, {'S', 5}, {'T', 5}, {'U', 5}, {'V', 5}, {'W', 7}, {'X', 5}, {'Y', 5}, {'Z', 5},
            {'a', 5}, {'b', 5}, {'c', 5}, {'d', 5}, {'e', 5}, {'f', 4}, {'g', 5}, {'h', 5}, {'i', 1}, {'j', 3}, {'k', 5}, {'l', 2}, {'m', 5}, {'n', 5}, {'o', 5}, {'p', 5}, {'q', 5}, {'r', 5}, {'s', 5}, {'t', 3}, {'u', 5}, {'v', 5}, {'w', 5}, {'x', 5}, {'y', 5}, {'z', 5},
            {'0', 5}, {'1', 3}, {'2', 5}, {'3', 5}, {'4', 5}, {'5', 5}, {'6', 5}, {'7', 5}, {'8', 5}, {'9', 5},
            {',', 2}, {':', 1}, {';', 2}, {'\'', 1}, {'"', 3}, {'!', 1}, {'?', 5}, {'(', 3}, {')', 3}, {'+', 5}, {'-', 5}, {'_', 7}, {'*', 5}, {'/', 3}, {'=', 5}, {'@', 7}, {'#', 5}, {'$', 5}, {'%', 7}, {'^', 5}, {'&', 5}, {'`', 2}, {'~', 5},
            {'[', 3}, {']', 3}, {'{', 4}, {'}', 4}, {'\\', 3}, {'|', 1}, {'<', 5}, {'>', 5}, {' ', 1}
        };

        foreach (char c in charList)
        {
            if(charWidths.TryGetValue(c, out int width))
            {
                textWidth += width;
            }
        }

        textWidth += (text.Length - 1);
        textWidth += 6;

        return textWidth;
    }
    public void ChangeTooltipText(string changeType)
    {
        switch (changeType)
        {
            case "invItemDurability":
                if (inventoryList[invSlotNumber].itemData.durability != -1)
                    {
                        InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + inventoryList[invSlotNumber].itemData.currentDurability + "%)";
                    }
                break;
            case "tileDurability":
                switch (tooltipType)
                {
                    case "wall": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mouseCollisionScript.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "fence": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mouseCollisionScript.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "bars": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mouseCollisionScript.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "ventCover": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mouseCollisionScript.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "slats": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mouseCollisionScript.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                }
                break;


        }
    }

}
