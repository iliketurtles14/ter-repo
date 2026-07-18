using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedDummySheetRemove : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Transform player;
    private Inventory invScript;
    private List<GameObject> slots = new List<GameObject>();
    private ItemDataCreator creator;
    private Transform badObjects;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        invScript = GetComponent<Inventory>();
        creator = GetComponent<ItemDataCreator>();
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;
        foreach(Transform slot in RootObjectCache.GetRoot("InventoryCanvas").transform.Find("GUIPanel"))
        {
            slots.Add(slot.gameObject);
        }
    }
    private void Update()
    {
        if((mcs.isTouchingSheet || mcs.isTouchingDummy) && Input.GetMouseButtonDown(1))
        {
            if (mcs.isTouchingSheet)
            {
                if(Vector2.Distance(mcs.touchedSheet.transform.position, player.position) <= 2.4f)
                {
                    TakeOff(mcs.touchedSheet);
                }
            }
            else if (mcs.isTouchingDummy)
            {
                if(Vector2.Distance(mcs.touchedDummy.transform.position, player.position) <= 2.4f)
                {
                    TakeOff(mcs.touchedDummy);
                }
            }
        }
    }
    private void TakeOff(GameObject obj)
    {
        bool invIsFull = true;
        for(int i = 0; i < 6; i++)
        {
            if (invScript.inventory[i].itemData == null)
            {
                invIsFull = false;
                break;
            }
        }
        if (invIsFull)
        {
            return;
        }

        int id = 0;
        if (obj.name.Contains("Dummy"))
        {
            id = 75;
        }
        else if(obj.name == "Sheet")
        {
            id = 76;
        }
        ItemData data = creator.CreateItemData(id);
        for(int i = 0; i < 6; i++)
        {
            if (invScript.inventory[i].itemData == null)
            {
                invScript.inventory[i].itemData = data;
                slots[i].GetComponent<Image>().sprite = data.sprite;
                break;
            }
        }

        if(obj.name == "Sheet")
        {
            foreach(Transform bo in badObjects)
            {
                if(bo.GetComponent<BadObjectData>().attachedObject == obj && bo.name == "sheets")
                {
                    Destroy(bo.gameObject);
                    break;
                }
            }
        }
        PSoundController.PlaySound("pickup");
        Destroy(obj);
    }
}
