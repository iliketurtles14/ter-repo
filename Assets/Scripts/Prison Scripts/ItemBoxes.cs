using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxes : MonoBehaviour
{
    MouseCollisionOnItems mcs;
    ItemDataCreator creator;
    Inventory inventoryScript;
    Transform aStar;
    Transform ic;
    List<GameObject> invSlots = new List<GameObject>();
    List<string> inmateNames = new List<string>();
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        creator = GetComponent<ItemDataCreator>();
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        aStar = RootObjectCache.GetRoot("A*").transform;
        foreach(Transform child in ic.Find("GUIPanel"))
        {
            invSlots.Add(child.gameObject);
        }
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach(Transform npc in aStar)
        {
            if (npc.name.StartsWith("Inmate"))
            {
                inmateNames.Add(npc.GetComponent<NPCCollectionData>().npcData.displayName);
            }
        }
    }
    private void Update()
    {
        if(mcs.isTouchingItemBox && Input.GetMouseButtonDown(0))
        {
            switch (mcs.touchedItemBox.name)
            {
                case "BookBox":
                    AddItem(77, true);
                    break;
                case "DeliveryTruckDown":
                case "DeliveryTruckUp":
                case "DeliveryTruckLeft":
                case "DeliveryTruckRight":
                    int rand = UnityEngine.Random.Range(0, 2);
                    if(rand == 0)
                    {
                        AddItem(116, false);
                    }
                    else
                    {
                        AddItem(117, false);
                    }
                    break;
                case "DirtyLaundry":
                    rand = UnityEngine.Random.Range(0, 2);
                    if(rand == 0)
                    {
                        AddItem(37, false);
                    }
                    else
                    {
                        AddItem(38, false);
                    }
                    break;
                case "Freezer":
                    AddItem(147, false);
                    break;
                case "MailBox":
                    AddItem(107, true);
                    break;
                case "MetalBox":
                    AddItem(130, false);
                    break;
                case "TailorBox":
                    AddItem(94, false);
                    break;
                case "TimberBox":
                    AddItem(139, false);
                    break;
            }
        }
    }
    private void AddItem(int id, bool named)
    {
        bool invIsFull = true;
        for(int i = 0; i < 6; i++)
        {
            try
            {
                if (inventoryScript.inventory[i].itemData.sprite == null)
                {
                    invIsFull = false;
                    break;
                }
            }
            catch
            {
                invIsFull = false;
                break;
            }
        }
        
        ItemData data = creator.CreateItemData(id);
        if(inmateNames.Count > 0 && named)
        {
            int rand = UnityEngine.Random.Range(0, inmateNames.Count);
            string randName = inmateNames[rand];
            data.inmateGiveName = randName;
        }
        if (!invIsFull)
        {
            for(int i = 0; i < 6; i++)
            {
                try
                {
                    if (inventoryScript.inventory[i].itemData == null)
                    {
                        inventoryScript.inventory[i].itemData = data;
                        invSlots[i].GetComponent<Image>().sprite = data.sprite;
                        break;
                    }
                }
                catch
                {
                    inventoryScript.inventory[i].itemData = data;
                    invSlots[i].GetComponent<Image>().sprite = data.sprite;
                    break;
                }
            }
        }
    }
}
