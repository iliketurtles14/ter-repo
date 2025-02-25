using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Image = UnityEngine.UI.Image;

public class ItemBehaviours : MonoBehaviour
{
    public InventorySelection selectionScript;
    public GameObject perksTiles;
    private ItemData selectedItemData;
    private ItemData usedItemData;
    public Canvas InventoryCanvas;
    private int slotNumber;
    private int usedSlotNumber;
    private List<InventoryItem> inventoryList;
    public Inventory inventoryScript;
    public MouseCollisionOnItems mouseCollisionScript;
    public GameObject barLine;
    public GameObject actionBarPanel;
    public bool barIsMoving;
    public Transform PlayerTransform;
    public Transform oldPlayerTransform;
    private bool cancelBar;
    public TextMeshProUGUI ActionTextBox;
    private int whatSlot;
    public Sprite clearSprite;
    private string whatAction;
    //general

    //breaking
    public bool selectedChippingItem;
    public bool selectedCuttingItem;
    public bool selectedDiggingItem;
    public bool selectedVentBreakingItem;

    public GameObject touchedTileObject;
    public GameObject emptyTile;
    public GameObject emptyVentCover;
    public void Start()
    {
        InventoryCanvas.transform.Find("ActionBar").GetComponent<Image>().enabled = false;
        ActionTextBox.text = "";
    }
    public void Update()
    {
        //define vars
        inventoryList = inventoryScript.inventory;

        if (selectionScript.aSlotSelected)
        {
            if (selectionScript.slot1Selected) { slotNumber = 0; }
            else if (selectionScript.slot2Selected) { slotNumber = 1; }
            else if (selectionScript.slot3Selected) { slotNumber = 2; }
            else if (selectionScript.slot4Selected) { slotNumber = 3; }
            else if (selectionScript.slot5Selected) { slotNumber = 4; }
            else if (selectionScript.slot6Selected) { slotNumber = 5; }

            selectedItemData = inventoryList[slotNumber].itemData;
        }
        else { selectedItemData = null; }


        ///BREAKING TILES
        //vars

        if (selectionScript.aSlotSelected)
        {
            if (selectedItemData.chippingPower != -1) { selectedChippingItem = true; }
            else { selectedChippingItem = false; }
            if (selectedItemData.cuttingPower != -1) { selectedCuttingItem = true; }
            else { selectedCuttingItem = false; }
            if (selectedItemData.diggingPower != -1) { selectedDiggingItem = true; }
            else { selectedDiggingItem = false; }
            if (selectedItemData.ventBreakingPower != -1) { selectedVentBreakingItem = true; }
            else { selectedVentBreakingItem = false; }
        }
        else if (!selectionScript.aSlotSelected) 
        {
            Deselect();
        }


        //chipping
        if (mouseCollisionScript.isTouchingWall && Input.GetMouseButtonDown(0) && selectedChippingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedWall.transform.position);
            if (distance <= 2.4f)
            {
                whatAction = "chipping";
                touchedTileObject = mouseCollisionScript.touchedWall.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Chipping");
                Deselect();
            }
        }
        else if(Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }
        //cutting fences
        if(mouseCollisionScript.isTouchingFence && Input.GetMouseButtonDown(0) && selectedCuttingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedFence.transform.position);
            if(distance <= 2.4f)
            {
                whatAction = "cutting fence";
                touchedTileObject = mouseCollisionScript.touchedFence.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Cutting");
                Deselect();
            }
        }else if(Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }
        //cutting bars
        if(mouseCollisionScript.isTouchingBars && Input.GetMouseButtonDown(0) && selectedCuttingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedBars.transform.position);
            if(distance <= 2.4f)
            {
                whatAction = "cutting bars";
                touchedTileObject = mouseCollisionScript.touchedBars.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Cutting");
                Deselect();
            }
        }else if(Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }
        //unscrewing vents
        if(mouseCollisionScript.isTouchingVentCover && Input.GetMouseButtonDown(0) && selectedVentBreakingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mouseCollisionScript.touchedVentCover.transform.position);
            if(distance <= 2.4f)
            {
                whatAction = "unscrewing vent";
                touchedTileObject = mouseCollisionScript.touchedVentCover.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Unscrewing");
                Deselect();
            }
        }
        
        if(barIsMoving && oldPlayerTransform.position != PlayerTransform.position)
        {
            StopCoroutine(DrawActionBar());
            DestroyActionBar();
        }
    }
    public void Deselect()
    {
        selectedChippingItem = false;
        selectedCuttingItem = false;
        selectedDiggingItem = false;
        selectedVentBreakingItem = false;
    }
    public void RemoveItemDurability(int currentDurability, int durability)
    {
        usedItemData.currentDurability = currentDurability - durability;
        if(usedItemData.currentDurability <= 0)
        {
            BreakItem();
        }
    }
    public void RemoveTileDurability(GameObject touchedTile, int currentDurability, int itemStrength)
    {
        TileData touchedTileData = touchedTile.GetComponent<TileCollectionData>().tileData;
        touchedTileData.currentDurability = currentDurability - itemStrength;
        if(touchedTileData.currentDurability <= 0)
        {
            BreakTile();
        }
    }
    public IEnumerator DrawActionBar()
    {
        cancelBar = false;
        barIsMoving = true;
        oldPlayerTransform.position = PlayerTransform.position;
        usedItemData = selectedItemData;
        usedSlotNumber = slotNumber;
        InventoryCanvas.transform.Find("ActionBar").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(.045f);
        if (cancelBar) { yield break; }
        for(int i = 1; i <= 49; i++)
        {
            if (cancelBar) { yield break; }
            Instantiate(barLine, actionBarPanel.transform);
            yield return new WaitForSeconds(.045f);
        }
        DestroyActionBar();
        RemoveItemDurability(usedItemData.currentDurability, usedItemData.durability);

        switch (whatAction)
        {
            case "chipping": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.chippingPower); break;
            case "cutting fence": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.cuttingPower); break;
            case "cutting bars": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.cuttingPower/2); break;
            case "unscrewing vent": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.ventBreakingPower); break;
        }

    }
    public void CreateActionText(string text)
    {
        ActionTextBox.text = text;
    }
    public void DestroyActionBar()
    {
        barIsMoving = false;
        cancelBar = true;
        foreach (Transform child in actionBarPanel.GetComponent<Transform>())
        {
            Destroy(child.gameObject);
        }
        InventoryCanvas.transform.Find("ActionBar").GetComponent<Image>().enabled = false;
        ActionTextBox.text = "";

    }
    public void BreakItem()
    {

        inventoryList[usedSlotNumber].itemData = null;
        switch (usedSlotNumber)
        {
            case 0: InventoryCanvas.transform.Find("GUIPanel").Find("Slot1").GetComponent<Image>().sprite = clearSprite; break;
            case 1: InventoryCanvas.transform.Find("GUIPanel").Find("Slot2").GetComponent<Image>().sprite = clearSprite; break;
            case 2: InventoryCanvas.transform.Find("GUIPanel").Find("Slot3").GetComponent<Image>().sprite = clearSprite; break;
            case 3: InventoryCanvas.transform.Find("GUIPanel").Find("Slot4").GetComponent<Image>().sprite = clearSprite; break;
            case 4: InventoryCanvas.transform.Find("GUIPanel").Find("Slot5").GetComponent<Image>().sprite = clearSprite; break;
            case 5: InventoryCanvas.transform.Find("GUIPanel").Find("Slot6").GetComponent<Image>().sprite = clearSprite; break;
        }
    }
    public void BreakTile()
    {
        if(whatAction == "unscrewing vent")
        {
            Vector3 ventPosition = new Vector3(touchedTileObject.transform.position.x, touchedTileObject.transform.position.y);
            Quaternion ventRotation = Quaternion.identity;
            Destroy(touchedTileObject);
            Instantiate(emptyVentCover, ventPosition, ventRotation, perksTiles.transform.Find("VentObjects"));

            //set transparency of vents
            SpriteRenderer[] ventSpriteRenderers = perksTiles.transform.Find("Vents").GetComponentsInChildren<SpriteRenderer>();
            SpriteRenderer[] ventObjectSpriteRenderers = perksTiles.transform.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in ventSpriteRenderers)
            {
                Color color = sr.color;
                color.a = .75f;
                sr.color = color;
            }
            foreach (SpriteRenderer sr in ventObjectSpriteRenderers)
            {
                Color color = sr.color;
                color.a = .75f;
                sr.color = color;
            }
        }
        else
        {
            Vector3 tilePosition = new Vector3(touchedTileObject.transform.position.x, touchedTileObject.transform.position.y);
            Quaternion rotation = Quaternion.identity;
            Destroy(touchedTileObject);
            Instantiate(emptyTile, tilePosition, rotation, perksTiles.transform.Find("Ground"));
        }
    }
}
