using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

public class Tooltips : MonoBehaviour
{
    private Transform PlayerTransform;
    private GameObject InventoryCanvas;
    private GameObject menuCanvas;
    private GameObject aStar;
    private Transform TooltipPanelTransform;
    private GameObject TooltipMid;
    private GameObject TooltipSide;
    private GameObject TooltipTextBox;
    private string toPrint;
    private string printDurability;
    private int textWidth;
    public bool showingTooltip;
    public string tooltipType; //invItem, groundItem, deskItem, wall, 
    private List<InventoryItem> inventoryList;
    private List<DeskItem> deskInvList;
    private List<IDItem> idInvList;
    private List<NPCInvItem> npcInvList;
    private Inventory inventoryScript;
    private DeskInv deskInvScript;
    private MouseCollisionOnItems mcs;
    private InventorySelection selectionScript;
    private ItemBehaviours itemBehavioursScript;
    private NPCIDInv npcIDInvScript;
    private GameObject touchedInvSlot;
    private bool isTouchingInvSlot;
    private int invSlotNumber; //starts at 0
    private int deskSlotNumber;
    private int idSlotNumber;
    private int npcInvSlotNumber;
    private int npcShopSlotNumber;
    private bool isTouchingItem;
    private GameObject touchedItem;
    private float wallDistance;
    private float fenceDistance;
    private float barsDistance;
    private float ventCoverDistance;
    private float slatsDistance;
    private float floorDistance;
    private float emptyDirtDistance;
    private float dirtDistance;
    private float rockDistance;
    private int printedInvSlotNumber;
    private int printedDeskSlotNumber;
    private int printedIDSlotNumber;
    private int printedNPCInvSlotNumber;
    private int printedShopSlotNumber;

    private TileData printedTileData;
    private int printedTileDurability;
    private int printedItemDurability;
    private string changeType;
    private GameObject currentTouchedItem;
    private bool isScrewingVent;
    private bool isScrewingSlats;
    private GameObject deskMenu;
    private Sprite clearSprite;
    private List<IDItem> currentIDList;
    private NPCInv npcInvScript;
    private Giving givingScript;
    private ShopMenu shopMenuScript;
    public void Start()
    {
        PlayerTransform = RootObjectCache.GetRoot("Player").transform;
        InventoryCanvas = RootObjectCache.GetRoot("InventoryCanvas");
        menuCanvas = RootObjectCache.GetRoot("MenuCanvas");
        aStar = RootObjectCache.GetRoot("A*");
        TooltipMid = Resources.Load<GameObject>("PrisonResources/UI Stuff/TooltipMid");
        TooltipSide = Resources.Load<GameObject>("PrisonResources/UI Stuff/TooltipSide");
        TooltipTextBox = Resources.Load<GameObject>("PrisonResources/UI Stuff/TooltipText");
        inventoryScript = GetComponent<Inventory>();
        mcs = InventoryCanvas.transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        selectionScript = GetComponent<InventorySelection>();
        itemBehavioursScript = GetComponent<ItemBehaviours>();
        deskMenu = menuCanvas.transform.Find("DeskMenuPanel").gameObject;
        deskInvScript = deskMenu.GetComponent<DeskInv>();
        clearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        npcIDInvScript = menuCanvas.transform.Find("NPCMenuPanel").GetComponent<NPCIDInv>();
        npcInvScript = menuCanvas.transform.Find("NPCInvMenu").GetComponent<NPCInv>();
        givingScript = menuCanvas.transform.Find("NPCGiveMenuPanel").GetComponent<Giving>();
        shopMenuScript = menuCanvas.transform.Find("NPCShopMenuPanel").GetComponent<ShopMenu>();
    }
    public void Update()
    {
        //update these vars
        inventoryList = inventoryScript.inventory;
        idInvList = menuCanvas.transform.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv;
        touchedInvSlot = mcs.touchedInvSlot;
        isTouchingInvSlot = mcs.isTouchingInvSlot;
        isTouchingItem = mcs.isTouchingItem;
        touchedItem = mcs.touchedItem;
        deskInvList = deskInvScript.deskInv;

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
        if (mcs.isTouchingInvSlot && showingTooltip && inventoryList[invSlotNumber].itemData.currentDurability != printedItemDurability)
        {
            changeType = "invItemDurability";
            ChangeTooltipText(changeType);
            return;
        }
        //for desk items
        if (mcs.isTouchingDeskSlot)
        {
            for (int i = 1; i <= 20; i++)
            {
                if (mcs.touchedDeskSlot.name == "Slot" + i)
                {
                    deskSlotNumber = i - 1;
                    break;
                }
            }
        }
        if(mcs.isTouchingDeskSlot && mcs.touchedDeskSlot.GetComponent<Image>().sprite != clearSprite && !showingTooltip)
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
        else if(showingTooltip && tooltipType == "deskItem" && !mcs.isTouchingDeskSlot)
        {
            DestroyTooltip();
            return;
        }
        else if ((showingTooltip && tooltipType == "deskItem" && mcs.isTouchingDeskSlot && deskInvList[deskSlotNumber].itemData == null) ||
            (showingTooltip && tooltipType == "deskItem" && mcs.isTouchingDeskSlot && deskInvList[deskSlotNumber].itemData.displayName != toPrint) ||
            (showingTooltip && tooltipType == "deskItem" && mcs.isTouchingDeskSlot && deskInvList[deskSlotNumber].itemData.currentDurability != deskInvList[printedDeskSlotNumber].itemData.currentDurability))
        {
            DestroyTooltip();
            return;
        }
        else if(mcs.isTouchingDeskSlot && showingTooltip && deskInvList[deskSlotNumber].itemData == null)
        {
            DestroyTooltip();
            return;
        }
        //for ID items
        if (mcs.isTouchingIDSlot)
        {
            if (mcs.touchedIDSlot.transform.parent.name == "NPCMenuPanel")
            {
                IDItem outfitItem = new IDItem();
                outfitItem.itemData = npcIDInvScript.currentNPC.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData;
                IDItem weaponItem = new IDItem();
                weaponItem.itemData = npcIDInvScript.currentNPC.GetComponent<NPCCollectionData>().npcData.inventory[6].itemData;
                currentIDList = new List<IDItem>()
                {
                    outfitItem, weaponItem
                };
            }
            else
            {
                currentIDList = idInvList;
            }
            if (mcs.touchedIDSlot.name == "Outfit")
            {
                idSlotNumber = 0;
            }
            else if (mcs.touchedIDSlot.name == "Weapon")
            {
                idSlotNumber = 1;
            }
            else
            {
                idSlotNumber = -1;
            }
        }
        if (mcs.isTouchingIDSlot && currentIDList[idSlotNumber].itemData != null && !showingTooltip)
        {
            if (currentIDList[idSlotNumber].itemData.durability != -1)
            {
                printedIDSlotNumber = idSlotNumber;
                toPrint = currentIDList[idSlotNumber].itemData.displayName;
                printedItemDurability = currentIDList[idSlotNumber].itemData.currentDurability;
                printDurability = " (" + currentIDList[idSlotNumber].itemData.currentDurability + "%)";
            }
            else if (currentIDList[idSlotNumber].itemData.durability == -1)
            {
                printedIDSlotNumber = idSlotNumber;
                toPrint = currentIDList[idSlotNumber].itemData.displayName;
                printDurability = "";
            }
            tooltipType = "idItem";
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if (showingTooltip && tooltipType == "idItem" && !mcs.isTouchingIDSlot)
        {
            DestroyTooltip();
            return;
        }
        else if ((showingTooltip && tooltipType == "idItem" && mcs.isTouchingIDSlot && currentIDList[idSlotNumber].itemData == null) ||
            (showingTooltip && tooltipType == "idItem" && mcs.isTouchingIDSlot && currentIDList[idSlotNumber].itemData.displayName != toPrint) ||
            (showingTooltip && tooltipType == "idItem" && mcs.isTouchingIDSlot && currentIDList[idSlotNumber].itemData.currentDurability != currentIDList[printedIDSlotNumber].itemData.currentDurability))
        {
            DestroyTooltip();
            return;
        }
        else if (mcs.isTouchingIDSlot && showingTooltip && currentIDList[idSlotNumber].itemData == null)
        {
            DestroyTooltip();
            return;
        }

        //for npcInv items
        if (mcs.isTouchingNPCInvSlot)
        {
            npcInvList = npcInvScript.npc.GetComponent<NPCCollectionData>().npcData.inventory;
            for(int i = 1; i <= 6; i++)
            {
                if(mcs.touchedNPCInvSlot.name == "Slot" + i)
                {
                    npcInvSlotNumber = i - 1;
                    break;
                }
            }
            if(mcs.touchedNPCInvSlot.name == "Outfit")
            {
                npcInvSlotNumber = 7;
            }
            else if(mcs.touchedNPCInvSlot.name == "Weapon")
            {
                npcInvSlotNumber = 6;
            }
        }
        if(mcs.isTouchingNPCInvSlot && mcs.touchedNPCInvSlot.GetComponent<Image>().sprite != clearSprite && !showingTooltip)
        {
            if (npcInvList[npcInvSlotNumber].itemData.durability != -1)
            {
                printedNPCInvSlotNumber = npcInvSlotNumber;
                toPrint = npcInvList[npcInvSlotNumber].itemData.displayName;
                printedItemDurability = npcInvList[npcInvSlotNumber].itemData.currentDurability;
                printDurability = " (" + npcInvList[npcInvSlotNumber].itemData.currentDurability + "%)";
            }
            else if (npcInvList[npcInvSlotNumber].itemData.durability == -1)
            {
                printedNPCInvSlotNumber = npcInvSlotNumber;
                toPrint = npcInvList[npcInvSlotNumber].itemData.displayName;
                printDurability = "";
            }
            tooltipType = "npcItem";
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "npcItem" && !mcs.isTouchingNPCInvSlot)
        {
            DestroyTooltip();
            return;
        }
        else if ((showingTooltip && tooltipType == "npcItem" && mcs.isTouchingNPCInvSlot && npcInvList[npcInvSlotNumber].itemData == null) ||
            (showingTooltip && tooltipType == "npcItem" && mcs.isTouchingNPCInvSlot && npcInvList[npcInvSlotNumber].itemData.displayName != toPrint) ||
            (showingTooltip && tooltipType == "npcItem" && mcs.isTouchingNPCInvSlot && npcInvList[npcInvSlotNumber].itemData.currentDurability != npcInvList[printedNPCInvSlotNumber].itemData.currentDurability))
        {
            DestroyTooltip();
            return;
        }
        else if(mcs.isTouchingNPCInvSlot && showingTooltip && npcInvList[npcInvSlotNumber].itemData == null)
        {
            DestroyTooltip();
            return;
        }

        //for shop items
        if (mcs.isTouchingShopSlot)
        {
            for (int i = 0; i < 4; i++)
            {
                if (mcs.touchedShopSlot.transform.parent.name == "Slot" + i)
                {
                    npcShopSlotNumber = i;
                    break;
                }
            }
        }
        if(mcs.isTouchingShopSlot && mcs.touchedShopSlot.GetComponent<Image>().sprite != clearSprite && !showingTooltip)
        {
            if (shopMenuScript.shopItems[npcShopSlotNumber].itemData.durability != -1)
            {
                printedShopSlotNumber = npcShopSlotNumber;
                toPrint = shopMenuScript.shopItems[npcShopSlotNumber].itemData.displayName;
                printedItemDurability = shopMenuScript.shopItems[npcShopSlotNumber].itemData.currentDurability;
                printDurability = " (" + shopMenuScript.shopItems[npcShopSlotNumber].itemData.currentDurability + "%)";
            }
            else if (shopMenuScript.shopItems[npcShopSlotNumber].itemData.durability == -1)
            {
                printedShopSlotNumber = npcShopSlotNumber;
                toPrint = shopMenuScript.shopItems[npcShopSlotNumber].itemData.displayName;
                printDurability = "";
            }
            tooltipType = "shopItem";
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "shopItem" && !mcs.isTouchingShopSlot)
        {
            DestroyTooltip();
            return;
        }
        else if ((showingTooltip && tooltipType == "shopItem" && mcs.isTouchingShopSlot && shopMenuScript.shopItems[npcShopSlotNumber].itemData == null) ||
            (showingTooltip && tooltipType == "shopItem" && mcs.isTouchingShopSlot && shopMenuScript.shopItems[npcShopSlotNumber].itemData.displayName != toPrint) ||
            (showingTooltip && tooltipType == "shopItem" && mcs.isTouchingShopSlot && shopMenuScript.shopItems[npcShopSlotNumber].itemData.currentDurability != shopMenuScript.shopItems[printedShopSlotNumber].itemData.currentDurability))
        {
            DestroyTooltip();
            return;
        }
        else if(mcs.isTouchingShopSlot && showingTooltip && shopMenuScript.shopItems[npcShopSlotNumber].itemData == null)
        {
            DestroyTooltip();
            return;
        }

        //for give items
        if (mcs.isTouchingGiveSlot && mcs.touchedGiveSlot.GetComponent<Image>().sprite != clearSprite && !showingTooltip)
        {
            if (givingScript.item.itemData.durability != -1)
            {
                toPrint = givingScript.item.itemData.displayName;
                printedItemDurability = givingScript.item.itemData.currentDurability;
                printDurability = " (" + givingScript.item.itemData.currentDurability + "%)";
            }
            else if (givingScript.item.itemData.durability == -1)
            {
                toPrint = givingScript.item.itemData.displayName;
                printDurability = "";
            }
            tooltipType = "giveItem";
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if (showingTooltip && tooltipType == "giveItem" && !mcs.isTouchingGiveSlot)
        {
            DestroyTooltip();
            return;
        }
        else if ((showingTooltip && tooltipType == "giveItem" && mcs.isTouchingGiveSlot && givingScript.item.itemData == null) ||
            (showingTooltip && tooltipType == "giveItem" && mcs.isTouchingGiveSlot && givingScript.item.itemData.displayName != toPrint)) //DONT USE THIS AS A TEMPLATE. USE NCPINVSLOTS
        {
            DestroyTooltip();
            return;
        }

            //for ground items
            if (isTouchingItem && !showingTooltip)
        {
            currentTouchedItem = mcs.touchedItem;
            toPrint = touchedItem.GetComponent<ItemCollectionData>().itemData.displayName;
            tooltipType = "groundItem";
            DrawTooltip(GetWidth(toPrint), toPrint);
            return;
        }
        else if (showingTooltip && tooltipType == "groundItem" && !isTouchingItem)
        {
            DestroyTooltip();
            return;
        }
        
        if(isTouchingItem && showingTooltip && mcs.touchedItem != currentTouchedItem)
        {
            DestroyTooltip();
            return;
        }
        ///TILES
        //floors
        if (mcs.isTouchingFloor)
        {
            floorDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedFloor.transform.position);
        }
        if (!showingTooltip && mcs.isTouchingFloor && floorDistance <= 2.4f && itemBehavioursScript.selectedDiggingItem)
        {
            toPrint = "Dig";
            printedTileDurability = mcs.touchedFloor.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mcs.touchedFloor.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "floor";
            printedTileData = mcs.touchedFloor.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if (showingTooltip && tooltipType == "floor" &&
            (!mcs.isTouchingFloor || !itemBehavioursScript.selectedDiggingItem))
        {
            DestroyTooltip();
            return;
        }
        else if (showingTooltip && tooltipType == "floor" && mcs.isTouchingFloor && itemBehavioursScript.selectedDiggingItem && mcs.touchedFloor.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingFloor && showingTooltip && mcs.touchedFloor.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            DestroyTooltip();
            return;
        }

        //empty dirt tiles
        if (mcs.isTouchingEmptyDirt)
        {
            emptyDirtDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedEmptyDirt.transform.position);
        }
        if (!showingTooltip && mcs.isTouchingEmptyDirt && emptyDirtDistance <= 2.4f && itemBehavioursScript.selectedDiggingItem)
        {
            toPrint = "Dig";
            printedTileDurability = mcs.touchedEmptyDirt.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mcs.touchedEmptyDirt.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "emptyDirt";
            printedTileData = mcs.touchedEmptyDirt.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if (showingTooltip && tooltipType == "emptyDirt" &&
            (!mcs.isTouchingEmptyDirt || !itemBehavioursScript.selectedDiggingItem))
        {
            DestroyTooltip();
            return;
        }
        else if (showingTooltip && tooltipType == "emptyDirt" && mcs.isTouchingEmptyDirt && itemBehavioursScript.selectedDiggingItem && mcs.touchedEmptyDirt.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingEmptyDirt && tooltipType == "emptyDirt" && showingTooltip && mcs.touchedEmptyDirt.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingEmptyDirt && showingTooltip && emptyDirtDistance > 2.4f && itemBehavioursScript.selectedDiggingItem)
        {
            DestroyTooltip();
            return;
        }

        //dirt tiles
        if (mcs.isTouchingDirt)
        {
            dirtDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedDirt.transform.position);
        }
        if (!showingTooltip && mcs.isTouchingDirt && dirtDistance <= 2.4f && itemBehavioursScript.selectedDiggingItem)
        {
            toPrint = "Dig";
            printedTileDurability = mcs.touchedDirt.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mcs.touchedDirt.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "dirt";
            printedTileData = mcs.touchedDirt.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if (showingTooltip && tooltipType == "dirt" &&
            (!mcs.isTouchingDirt || !itemBehavioursScript.selectedDiggingItem))
        {
            DestroyTooltip();
            return;
        }
        else if (showingTooltip && tooltipType == "dirt" && mcs.isTouchingDirt && itemBehavioursScript.selectedDiggingItem && mcs.touchedDirt.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingDirt && tooltipType == "dirt" && showingTooltip && mcs.touchedDirt.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingDirt && showingTooltip && dirtDistance > 2.4f && itemBehavioursScript.selectedDiggingItem)
        {
            DestroyTooltip();
            return;
        }

        //walls
        if (mcs.isTouchingWall)
        {
            wallDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedWall.transform.position);
        }
        if (!showingTooltip && mcs.isTouchingWall && wallDistance <= 2.4f && itemBehavioursScript.selectedChippingItem)
        {
            toPrint = "Chip Wall";
            printedTileDurability = mcs.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mcs.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "wall";
            printedTileData = mcs.touchedWall.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "wall" && 
            (!mcs.isTouchingWall || !itemBehavioursScript.selectedChippingItem))
        {
            DestroyTooltip();
            return;
        }
        else if(showingTooltip && tooltipType == "wall" &&  mcs.isTouchingWall && itemBehavioursScript.selectedChippingItem && mcs.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingWall && tooltipType == "wall" && showingTooltip && mcs.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }
        if(mcs.isTouchingWall && showingTooltip && wallDistance > 2.4f && itemBehavioursScript.selectedChippingItem)
        {
            DestroyTooltip();
            return;
        }
        //fences
        if (mcs.isTouchingFence)
        {
            fenceDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedFence.transform.position);
        }
        if(!showingTooltip && mcs.isTouchingFence && fenceDistance <= 2.4f && itemBehavioursScript.selectedCuttingItem)
        {
            toPrint = "Cut Fence";
            printedTileDurability = mcs.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mcs.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "fence";
            printedTileData = mcs.touchedFence.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "fence" && 
            (!mcs.isTouchingFence || !itemBehavioursScript.selectedCuttingItem))
        {
            DestroyTooltip();
            return;
        }
        else if (showingTooltip && tooltipType == "fence" && mcs.isTouchingFence && itemBehavioursScript.selectedCuttingItem && mcs.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingFence && tooltipType == "fence" && showingTooltip && mcs.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }
        if (mcs.isTouchingFence && showingTooltip && fenceDistance > 2.4f && itemBehavioursScript.selectedCuttingItem)
        {
            DestroyTooltip();
            return;
        }

        //bars
        if (mcs.isTouchingBars)
        {
            barsDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedBars.transform.position);
        }
        if(!showingTooltip && mcs.isTouchingBars && barsDistance <= 2.4f && itemBehavioursScript.selectedCuttingItem)
        {
            toPrint = "Cut Bars";
            printedTileDurability = mcs.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mcs.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "bars";
            printedTileData = mcs.touchedBars.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "bars" &&
            (!mcs.isTouchingBars || !itemBehavioursScript.selectedCuttingItem))
        {
            DestroyTooltip();
            return;
        }
        else if (showingTooltip && tooltipType == "bars" && mcs.isTouchingBars && itemBehavioursScript.selectedCuttingItem && mcs.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingBars && tooltipType == "bars" && showingTooltip && mcs.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }
        if (mcs.isTouchingBars && showingTooltip && barsDistance > 2.4f && itemBehavioursScript.selectedCuttingItem)
        {
            DestroyTooltip();
            return;
        }

        //rocks
        if (mcs.isTouchingRock)
        {
            rockDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedRock.transform.position);
        }
        if(!showingTooltip && mcs.isTouchingRock && rockDistance <=2.4f && itemBehavioursScript.selectedChippingItem)
        {
            toPrint = "Chip Rock";
            printedTileDurability = mcs.touchedRock.GetComponent<TileCollectionData>().tileData.currentDurability;
            printDurability = " (" + mcs.touchedRock.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
            tooltipType = "rock";
            printedTileData = mcs.touchedRock.GetComponent<TileCollectionData>().tileData;
            DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
            return;
        }
        else if(showingTooltip && tooltipType == "rock" &&
            (!mcs.isTouchingRock || !itemBehavioursScript.selectedChippingItem))
        {
            DestroyTooltip();
            return;
        }
        else if(showingTooltip && tooltipType == "rock" && mcs.isTouchingRock && itemBehavioursScript.selectedChippingItem && mcs.touchedRock.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if(mcs.isTouchingRock && tooltipType == "rock" && showingTooltip && mcs.touchedRock.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }
        if (mcs.isTouchingRock && showingTooltip && rockDistance > 2.4f && itemBehavioursScript.selectedChippingItem)
        {
            DestroyTooltip();
            return;
        }
        //vent covers (ONLY BIG BECAUSE UNSCREWING AND CUTTING CAPABILITIES)
        if (mcs.isTouchingVentCover && mcs.touchedVentCover != null)
        {
            ventCoverDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedVentCover.transform.position);
        }
        if (itemBehavioursScript.selectedVentBreakingItem || itemBehavioursScript.selectedCuttingItem)
        {
            if (!showingTooltip && mcs.isTouchingVentCover && ventCoverDistance <= 2.4f)
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
                printedTileDurability = mcs.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability;
                printDurability = " (" + mcs.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
                tooltipType = "ventCover";
                printedTileData = mcs.touchedVentCover.GetComponent<TileCollectionData>().tileData;
                DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
                return;
            }
        }
        if (isScrewingVent)
        {
            if (showingTooltip && tooltipType == "ventCover" &&
                (!mcs.isTouchingVentCover || !itemBehavioursScript.selectedVentBreakingItem))
            {
                DestroyTooltip();
                return;
            }
        }
        else if (!isScrewingVent)
        {
            if (showingTooltip && tooltipType == "ventCover" &&
                (!mcs.isTouchingVentCover || !itemBehavioursScript.selectedCuttingItem))
            {
                DestroyTooltip();
                return;
            }
        }
        else if (showingTooltip && tooltipType == "ventCover" && mcs.isTouchingVentCover && (itemBehavioursScript.selectedVentBreakingItem || itemBehavioursScript.selectedCuttingItem) && mcs.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingVentCover && tooltipType == "ventCover" && showingTooltip && mcs.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }
        if (mcs.isTouchingVentCover && showingTooltip && ventCoverDistance > 2.4f && (itemBehavioursScript.selectedCuttingItem || itemBehavioursScript.selectedVentBreakingItem))
        {
            DestroyTooltip();
            return;
        }

        //slats
        if (mcs.isTouchingSlats)
        {
            slatsDistance = Vector2.Distance(PlayerTransform.position, mcs.touchedSlats.transform.position);
        }
        if (itemBehavioursScript.selectedVentBreakingItem || itemBehavioursScript.selectedCuttingItem)
        {
            if (!showingTooltip && mcs.isTouchingSlats && slatsDistance <= 2.4f)
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
                printedTileDurability = mcs.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability;
                printDurability = " (" + mcs.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability + "%)";
                tooltipType = "slats";
                printedTileData = mcs.touchedSlats.GetComponent<TileCollectionData>().tileData;
                DrawTooltip(GetWidth(toPrint + printDurability), toPrint + printDurability);
                return;
            }
        }
        if (isScrewingSlats)
        {
            if (showingTooltip && tooltipType == "slats" &&
                (!mcs.isTouchingSlats || !itemBehavioursScript.selectedVentBreakingItem))
            {
                DestroyTooltip();
                return;
            }
        }
        else if (!isScrewingSlats)
        {
            if (showingTooltip && tooltipType == "slats" &&
                (!mcs.isTouchingSlats || !itemBehavioursScript.selectedCuttingItem))
            {
                DestroyTooltip();
                return;
            }
        }
        else if (showingTooltip && tooltipType == "slats" && mcs.isTouchingSlats && (itemBehavioursScript.selectedVentBreakingItem || itemBehavioursScript.selectedCuttingItem) && mcs.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileData.currentDurability)
        {
            DestroyTooltip();
            return;
        }
        if (mcs.isTouchingSlats && tooltipType == "slats" && showingTooltip && mcs.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability != printedTileDurability)
        {
            changeType = "tileDurability";
            ChangeTooltipText(changeType);
            return;
        }
        if (mcs.isTouchingSlats && showingTooltip && slatsDistance > 2.4f && (itemBehavioursScript.selectedCuttingItem || itemBehavioursScript.selectedVentBreakingItem))
        {
            DestroyTooltip();
            return;
        }

        ///OBJECTS
        //desks
        if (mcs.isTouchingDesk && !showingTooltip)
        {
            if (mcs.touchedDesk.name == "PlayerDesk")
            {
                toPrint = "Your Desk";
            }
            else if (mcs.touchedDesk.name.StartsWith("NPCDesk"))
            {
                int num = mcs.touchedDesk.GetComponent<DeskData>().inmateCorrelationNumber;

                if (num == -1)
                {
                    toPrint = "Desk";
                }
                else
                {
                    foreach (Transform child in aStar.transform)
                    {
                        if (child.name.StartsWith("Inmate"))
                        {
                            if (child.GetComponent<NPCCollectionData>().npcData.order == num)
                            {
                                toPrint = child.GetComponent<NPCCollectionData>().npcData.displayName.Replace("\r\n", "").Replace("\n", "").Replace("\r", "") + "'s Desk";
                                break;
                            }
                        }
                    }
                }
            }
            else if(mcs.touchedDesk.name == "DevDesk")
            {
                toPrint = "Dev Desk";
            }
            tooltipType = "desk";
            DrawTooltip(GetWidth(toPrint), toPrint);
            return;
        }
        else if (showingTooltip && tooltipType == "desk" &&
            !mcs.isTouchingDesk)
        {
            DestroyTooltip();
            return;
        }
        else if (showingTooltip && tooltipType == "desk")
        {
            string str = null;

            if(mcs.touchedDesk.name == "PlayerDesk")
            {
                str = "Your Desk";
            }
            else if (mcs.touchedDesk.name.StartsWith("NPCDesk"))
            {
                int num = mcs.touchedDesk.GetComponent<DeskData>().inmateCorrelationNumber;

                if (num == -1)
                {
                    str = "Desk";
                }
                else
                {
                    foreach (Transform child in aStar.transform)
                    {
                        if (child.name.StartsWith("Inmate"))
                        {
                            if(child.GetComponent<NPCCollectionData>().npcData.order == num)
                            {
                                str = child.GetComponent<NPCCollectionData>().npcData.displayName.Replace("\r\n", "").Replace("\n", "").Replace("\r", "") + "'s Desk";
                                break;
                            }
                        }
                    }
                }
            }
            else if(mcs.touchedDesk.name == "DevDesk")
            {
                str = "Dev Desk";
            }

            if (str != toPrint)
            {
                DestroyTooltip();
                return;
            }
        }

        //equipment
        if(mcs.isTouchingEquipment && !showingTooltip)
        {
            if (mcs.touchedEquipment.name.StartsWith("Treadmill"))
            {
                toPrint = "Train (treadmill)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("Benchpress"))
            {
                toPrint = "Train (weights)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("PullupBar"))
            {
                toPrint = "Train (chin ups)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("PushupMat"))
            {
                toPrint = "Train (press ups)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("RunningMat"))
            {
                toPrint = "Train (jogging)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("JumpropeMat"))
            {
                toPrint = "Train (skipping)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("PunchingMat"))
            {
                toPrint = "Train (punch bag)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("SpeedBag"))
            {
                toPrint = "Train (speed bag)";
            }
            tooltipType = "equipment";
            DrawTooltip(GetWidth(toPrint), toPrint);
            return;
        }
        else if(showingTooltip && tooltipType == "equipment" && !mcs.isTouchingEquipment)
        {
            DestroyTooltip();
            return;
        }
        else if(showingTooltip && tooltipType == "equipment")
        {
            string str = null;

            if (mcs.touchedEquipment.name.StartsWith("Treadmill"))
            {
                str = "Train (treadmill)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("Benchpress"))
            {
                str = "Train (weights)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("PullupBar"))
            {
                str = "Train (chin ups)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("PushupMat"))
            {
                str = "Train (press ups)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("RunningMat"))
            {
                str = "Train (jogging)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("JumpropeMat"))
            {
                str = "Train (skipping)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("PunchingMat"))
            {
                str = "Train (punch bag)";
            }
            else if (mcs.touchedEquipment.name.StartsWith("SpeedBag"))
            {
                str = "Train (speed bag)";
            }

            if(str != toPrint)
            {
                DestroyTooltip();
                return;
            }
        }

        //readers
        if(mcs.isTouchingReader && !showingTooltip)
        {
            if (mcs.touchedReader.name.StartsWith("ComputerTable"))
            {
                toPrint = "Internet";
            }
            tooltipType = "reader";
            DrawTooltip(GetWidth(toPrint), toPrint);
            return;
        }
        else if(showingTooltip && tooltipType == "reader" && !mcs.isTouchingReader)
        {
            DestroyTooltip();
            return;
        }
        else if(showingTooltip && tooltipType == "reader")
        {
            string str = null;

            if (mcs.touchedReader.name.StartsWith("ComputerTable"))
            {
                str = "Internet";
            }

            if(str != toPrint)
            {
                DestroyTooltip();
                return;
            }
        }

        //sittables
        if(mcs.isTouchingSittable && !showingTooltip)
        {
            if (mcs.touchedSittable.name.StartsWith("PlayerBed"))
            {
                toPrint = "Your Bed";
            }
            else if (mcs.touchedSittable.name.StartsWith("MedicBed"))
            {
                toPrint = "Infirmary Bed";
            }
            else if (mcs.touchedSittable.name.StartsWith("Lounger"))
            {
                toPrint = "Sun Lounger";
            }
            else if (mcs.touchedSittable.name.StartsWith("Seat"))
            {
                toPrint = "Sit Down";
            }
            tooltipType = "sittable";
            DrawTooltip(GetWidth(toPrint), toPrint);
            return;
        }
        if(showingTooltip && tooltipType == "sittable" && !mcs.isTouchingSittable)
        {
            DestroyTooltip();
            return;
        }
        if(showingTooltip && tooltipType == "sittable")
        {
            string str = null;

            if (mcs.touchedSittable.name.StartsWith("PlayerBed"))
            {
                str = "Your Bed";
            }
            else if (mcs.touchedSittable.name.StartsWith("MedicBed"))
            {
                str = "Infirmary Bed";
            }
            else if (mcs.touchedSittable.name.StartsWith("Lounger"))
            {
                str = "Sun Lounger";
            }
            else if (mcs.touchedSittable.name.StartsWith("Seat"))
            {
                str = "Sit Down";
            }

            if(str != toPrint)
            {
                DestroyTooltip();
                return;
            }
        }

        ///NPCS
        //inmates/guards
        if (mcs.isTouchingNPC && !showingTooltip)
        {
            toPrint = mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.displayName;
            tooltipType = "inmates/guards";
            DrawTooltip(GetWidth(toPrint), toPrint);
            return;
        }
        else if(showingTooltip && tooltipType == "inmates/guards" && !mcs.isTouchingNPC)
        {
            DestroyTooltip();
            return;
        }
        else if(showingTooltip && tooltipType == "inmates/guards")
        {
            string str = mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.displayName;

            if(str != toPrint)
            {
                DestroyTooltip();
                return;
            }
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

        TooltipPanelTransform.position = InventoryCanvas.transform.Find("MouseOverlay").position;

        //make the textbox
        TooltipTextBox.GetComponent<RectTransform>().sizeDelta = new Vector2(width * 5, 80);
        TooltipTextBox.GetComponent<TextMeshProUGUI>().text = text;
        GameObject textBox = Instantiate(TooltipTextBox, InventoryCanvas.transform);

        textBox.transform.position = InventoryCanvas.transform.Find("MouseOverlay").position + new Vector3(1.46f, -.9f); //this is just an offset

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
                    case "wall": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedWall.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "fence": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedFence.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "bars": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedBars.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "ventCover": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedVentCover.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "slats": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedSlats.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "floor": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedFloor.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "emptyDirt": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedDirt.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "dirt": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedDirt.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                    case "rock": InventoryCanvas.transform.Find("TooltipText(Clone)").GetComponent<TextMeshProUGUI>().text = toPrint + " (" + mcs.touchedRock.GetComponent<TileCollectionData>().tileData.currentDurability + "%)"; break;
                }
                break;


        }
    }

}
