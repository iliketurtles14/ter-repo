using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

public class BedRemoveCovers : MonoBehaviour //also puts them on
{
    private MouseCollisionOnItems mcs;
    private Transform player;
    private Inventory invScript;
    private ItemDataCreator creator;
    private List<GameObject> slots = new List<GameObject>();
    private InventorySelection invSelectionScript;
    private Sprite clear;
    private List<string> bedNames = new List<string>
    {
        "BedHorizontal", "BedVertical", "PlayerBedHorizontal", "PlayerBedVertical"
    };
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        invScript = GetComponent<Inventory>();
        creator = GetComponent<ItemDataCreator>();
        invSelectionScript = GetComponent<InventorySelection>();
        clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        foreach(Transform slot in RootObjectCache.GetRoot("InventoryCanvas").transform.Find("GUIPanel"))
        {
            slots.Add(slot.gameObject);
        }
    }
    private void Update()
    {
        if((mcs.isTouchingSittable || mcs.isTouchingInmateBed) && Input.GetMouseButtonDown(1))
        {
            if (mcs.isTouchingInmateBed && bedNames.Contains(mcs.touchedInmateBed.name))
            {
                if (Vector2.Distance(player.position, mcs.touchedInmateBed.transform.position) <= 2.4f)
                {
                    TakeOff(mcs.touchedInmateBed);
                }
            }
            else if(mcs.isTouchingSittable && bedNames.Contains(mcs.touchedSittable.name))
            {
                if (Vector2.Distance(player.position, mcs.touchedSittable.transform.position) <= 2.4f)
                {
                    TakeOff(mcs.touchedSittable);
                }
            }
        }
        else if((mcs.isTouchingSittable || mcs.isTouchingInmateBed) && Input.GetMouseButtonDown(0) && invSelectionScript.aSlotSelected)
        {
            if (mcs.isTouchingInmateBed && bedNames.Contains(mcs.touchedInmateBed.name))
            {
                if (Vector2.Distance(player.position, mcs.touchedInmateBed.transform.position) <= 2.4f)
                {
                    PutOn(invSelectionScript.selectedSlotNum, mcs.touchedInmateBed);
                }
            }
            else if (mcs.isTouchingSittable && bedNames.Contains(mcs.touchedSittable.name))
            {
                if (Vector2.Distance(player.position, mcs.touchedSittable.transform.position) <= 2.4f)
                {
                    PutOn(invSelectionScript.selectedSlotNum, mcs.touchedSittable);
                }
            }

        }
    }
    private void PutOn(int slot, GameObject bed)
    {
        int id = invScript.inventory[slot].itemData.id;
        DataSender ds = DataSender.instance;
        SpriteRenderer sr = bed.GetComponent<SpriteRenderer>();
        if (bed.name.Contains("BedVertical"))
        {
            if (id == 76 && sr.sprite == ds.PrisonObjectImages[261])
            {
                sr.sprite = ds.PrisonObjectImages[264];
                invScript.inventory[slot].itemData = null;
                slots[slot].GetComponent<Image>().sprite = clear;
            }
            else if (id == 65 && sr.sprite == ds.PrisonObjectImages[262])
            {
                sr.sprite = ds.PrisonObjectImages[261];
                invScript.inventory[slot].itemData = null;
                slots[slot].GetComponent<Image>().sprite = clear;
            }
        }
        else if (bed.name.Contains("BedHorizontal"))
        {
            if (id == 76 && sr.sprite == ds.PrisonObjectImages[266])
            {
                sr.sprite = ds.PrisonObjectImages[265];
                invScript.inventory[slot].itemData = null;
                slots[slot].GetComponent<Image>().sprite = clear;
            }
            else if (id == 65 && sr.sprite == ds.PrisonObjectImages[267])
            {
                sr.sprite = ds.PrisonObjectImages[266];
                invScript.inventory[slot].itemData = null;
                slots[slot].GetComponent<Image>().sprite = clear;
            }
        }
    }
    private void TakeOff(GameObject bed)
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

        DataSender ds = DataSender.instance;
        SpriteRenderer sr = bed.GetComponent<SpriteRenderer>();
        if (bed.name.Contains("BedVertical"))
        {
            if(sr.sprite == ds.PrisonObjectImages[264]) //full
            {
                AddItem(76);
                sr.sprite = ds.PrisonObjectImages[261];
            }
            else if(sr.sprite == ds.PrisonObjectImages[261]) //pillow only
            {
                AddItem(65);
                sr.sprite = ds.PrisonObjectImages[262];
            }
        }
        else if (bed.name.Contains("BedHorizontal"))
        {
            if (sr.sprite == ds.PrisonObjectImages[265]) //full
            {
                AddItem(76);
                sr.sprite = ds.PrisonObjectImages[266];
            }
            else if (sr.sprite == ds.PrisonObjectImages[266]) //pillow only
            {
                AddItem(65);
                sr.sprite = ds.PrisonObjectImages[267];
            }
        }
    }
    private void AddItem(int id)
    {
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
    }
}
