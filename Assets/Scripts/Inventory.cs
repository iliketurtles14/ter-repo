using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public Canvas InventoryCanvas;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public GameObject Player;
    public Sprite ClearSprite;
    public GameObject MouseOverlayObject;
    public GameObject perksTiles;
    public DeskStand deskStandScript;
    public ItemBehaviours itemBehavioursScript;
    public PlayerFloorCollision playerCollisionScript;
    private int slotIndex;
    private MouseCollisionOnItems mouseCollisionScript;
    public Transform PlayerTransform;
    private string slotName;
    public int droppedDurability = 100;
    public bool isDropped = false;
    public void Start()
    {
        mouseCollisionScript = MouseOverlayObject.GetComponent<MouseCollisionOnItems>();

    }
    public void Update()
    {
        Transform slot1 = InventoryCanvas.transform.Find("GUIPanel/Slot1");
        PolygonCollider2D slot1Collider = slot1.GetComponent<PolygonCollider2D>();
        Transform slot2 = InventoryCanvas.transform.Find("GUIPanel/Slot2");
        PolygonCollider2D slot2Collider = slot2.GetComponent<PolygonCollider2D>();
        Transform slot3 = InventoryCanvas.transform.Find("GUIPanel/Slot3");
        PolygonCollider2D slot3Collider = slot3.GetComponent<PolygonCollider2D>();
        Transform slot4 = InventoryCanvas.transform.Find("GUIPanel/Slot4");
        PolygonCollider2D slot4Collider = slot4.GetComponent<PolygonCollider2D>();
        Transform slot5 = InventoryCanvas.transform.Find("GUIPanel/Slot5");
        PolygonCollider2D slot5Collider = slot5.GetComponent<PolygonCollider2D>();
        Transform slot6 = InventoryCanvas.transform.Find("GUIPanel/Slot6");
        PolygonCollider2D slot6Collider = slot6.GetComponent<PolygonCollider2D>();
        Image slot1Image = slot1.GetComponent<Image>();
        Image slot2Image = slot2.GetComponent<Image>();
        Image slot3Image = slot3.GetComponent<Image>();
        Image slot4Image = slot4.GetComponent<Image>();
        Image slot5Image = slot5.GetComponent<Image>();
        Image slot6Image = slot6.GetComponent<Image>();
        PolygonCollider2D mouseCollider = MouseOverlayObject.GetComponent<PolygonCollider2D>();
        GameObject itemObject = mouseCollisionScript.touchedItem;

        if(inventory[0].itemData == null || 
            inventory[1].itemData == null || 
            inventory[2].itemData == null || 
            inventory[3].itemData == null || 
            inventory[4].itemData == null || 
            inventory[5].itemData == null)
        {
            if (mouseCollisionScript.touchedItem != null)
            {
                Transform ItemTransform = mouseCollisionScript.touchedItem.transform;
                if (Input.GetMouseButtonDown(1) && mouseCollisionScript.isTouchingItem == true && Vector2.Distance(PlayerTransform.position, ItemTransform.position) <= 2.4f && mouseCollisionScript.touchedItem.layer == Player.layer)
                {
                    ItemCollectionData itemCollectionData = itemObject.GetComponent<ItemCollectionData>();
                    Add(itemCollectionData.itemData);
                    Destroy(itemObject);
                }
            }
        }

        foreach(Transform child in InventoryCanvas.transform.Find("GUIPanel"))
        {
            if(child.GetComponent<Image>().sprite = ClearSprite)
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                child.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if (Input.GetMouseButtonDown(1) && mouseCollisionScript.isTouchingInvSlot == true)
        {
            if(mouseCollisionScript.touchedInvSlot.name == "Slot1") 
            {
                slotIndex = 0;
                slotName = "Slot1";
            }else if(mouseCollisionScript.touchedInvSlot.name == "Slot2")
            {
                slotIndex = 1;
                slotName = "Slot2";
            }
            else if(mouseCollisionScript.touchedInvSlot.name == "Slot3")
            {
                slotIndex = 2;
                slotName = "Slot3";
            }
            else if(mouseCollisionScript.touchedInvSlot.name == "Slot4")
            {
                slotIndex = 3;
                slotName = "Slot4";
            }
            else if(mouseCollisionScript.touchedInvSlot.name == "Slot5")
            {
                slotIndex = 4;
                slotName = "Slot5";
            }
            else if(mouseCollisionScript.touchedInvSlot.name == "Slot6")
            {
                slotIndex = 5;
                slotName = "Slot6";
            }
            else
            {
                return;
            }
            DropItem(slotIndex, slotName);
        }
    }
    public void Add(ItemData itemData)
    {
        isDropped = false;
        InventoryItem newItem = new InventoryItem();
        Transform slot1 = InventoryCanvas.transform.Find("GUIPanel/Slot1");
        Image slot1Image = slot1.GetComponent<Image>();
        Transform slot2 = InventoryCanvas.transform.Find("GUIPanel/Slot2");
        Image slot2Image = slot2.GetComponent<Image>();
        Transform slot3 = InventoryCanvas.transform.Find("GUIPanel/Slot3");
        Image slot3Image = slot3.GetComponent<Image>();
        Transform slot4 = InventoryCanvas.transform.Find("GUIPanel/Slot4");
        Image slot4Image = slot4.GetComponent<Image>();
        Transform slot5 = InventoryCanvas.transform.Find("GUIPanel/Slot5");
        Image slot5Image = slot5.GetComponent<Image>();
        Transform slot6 = InventoryCanvas.transform.Find("GUIPanel/Slot6");
        Image slot6Image = slot6.GetComponent<Image>();

        if (slot1Image.sprite == ClearSprite)
        {
            slot1Image.sprite = itemData.icon;
            inventory[0].itemData = itemData;
            return;
        } else if(slot2Image.sprite == ClearSprite)
        {
            slot2Image.sprite = itemData.icon;
            inventory[1].itemData = itemData;
            return;
        } else if(slot3Image.sprite == ClearSprite)
        {
            slot3Image.sprite = itemData.icon;
            inventory[2].itemData = itemData;
            return;
        } else if(slot4Image.sprite == ClearSprite)
        {
            slot4Image.sprite = itemData.icon;
            inventory[3].itemData = itemData;
            return;
        } else if(slot5Image.sprite == ClearSprite)
        {
            slot5Image.sprite = itemData.icon;
            inventory[4].itemData = itemData;
            return;
        } else if( slot6Image.sprite == ClearSprite)
        {
            slot6Image.sprite = itemData.icon;
            inventory[5].itemData = itemData;
            return;
        } else
        {
            return;
        }
    }
    public void DropItem(int slotIndex, string slotName)
    {
        if (!deskStandScript.hasClimbed && !deskStandScript.isClimbing && !itemBehavioursScript.isRoping) //checks if standing on a desk
        {
            foreach(GameObject item in GameObject.FindGameObjectsWithTag("Item"))//checks if item is in the way
            {
                if (item.transform.position == playerCollisionScript.playerFloor.transform.position)
                {
                    return;
                }
            }

            //ground dropping
            if (Player.layer == 3)
            {
                Vector3 playerPosition = Player.transform.position;
                GameObject droppedItem = Instantiate(inventory[slotIndex].itemData.prefab, playerCollisionScript.playerFloor.transform.position, Quaternion.identity, perksTiles.transform.Find("GroundObjects"));
                droppedItem.GetComponent<ItemCollectionData>().itemData = inventory[slotIndex].itemData;
                droppedItem.GetComponent<SpriteRenderer>().sprite = droppedItem.GetComponent<ItemCollectionData>().itemData.icon;
                droppedItem.GetComponent<SpriteRenderer>().sortingOrder = 3;
                droppedItem.layer = 3;
            }
            else if (Player.layer == 12) //vent dropping
            {
                Vector3 playerPosition = Player.transform.position;
                GameObject droppedItem = Instantiate(inventory[slotIndex].itemData.prefab, playerCollisionScript.playerFloor.transform.position, Quaternion.identity, perksTiles.transform.Find("VentObjects"));
                droppedItem.GetComponent<ItemCollectionData>().itemData = inventory[slotIndex].itemData;
                droppedItem.GetComponent<SpriteRenderer>().sprite = droppedItem.GetComponent<ItemCollectionData>().itemData.icon;
                droppedItem.GetComponent<SpriteRenderer>().sortingOrder = 10;
                droppedItem.layer = 12;
            }
            else if (Player.layer == 13) // roof dropping
            {
                Vector3 playerPosition = Player.transform.position;
                GameObject droppedItem = Instantiate(inventory[slotIndex].itemData.prefab, playerCollisionScript.playerFloor.transform.position, Quaternion.identity, perksTiles.transform.Find("RoofObjects"));
                droppedItem.GetComponent<ItemCollectionData>().itemData = inventory[slotIndex].itemData;
                droppedItem.GetComponent<SpriteRenderer>().sprite = droppedItem.GetComponent<ItemCollectionData>().itemData.icon;
                droppedItem.GetComponent<SpriteRenderer>().sortingOrder = 14;
                droppedItem.layer = 13;
            }
            else if (Player.layer == 11) //underground deropping
            {
                Vector3 playerPosition = Player.transform.position;
                GameObject droppedItem = Instantiate(inventory[slotIndex].itemData.prefab, playerCollisionScript.playerFloor.transform.position, Quaternion.identity, perksTiles.transform.Find("UndergroundObjects"));
                droppedItem.GetComponent<ItemCollectionData>().itemData = inventory[slotIndex].itemData;
                droppedItem.GetComponent<SpriteRenderer>().sprite = droppedItem.GetComponent<ItemCollectionData>().itemData.icon;
                droppedItem.GetComponent<SpriteRenderer>().sortingOrder = 11;
                droppedItem.layer = 11;
            }
            else { return; }
            droppedDurability = inventory[slotIndex].itemData.currentDurability;
            isDropped = true;
            inventory[slotIndex].itemData = null;

            Image slotImage = InventoryCanvas.transform.Find("GUIPanel/" + slotName).GetComponent<Image>();
            slotImage.sprite = ClearSprite;
        }
    }
}
