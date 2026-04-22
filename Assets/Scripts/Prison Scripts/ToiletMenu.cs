using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToiletMenu : MonoBehaviour
{
    private Transform player;
    private Transform tiles;
    private MouseCollisionOnItems mcs;
    private bool inMenu;
    private bool invIsFull;
    private bool toiletIsFull;
    private GameObject currentToilet;
    private Inventory inventoryScript;
    private Sprite clear;
    private List<GameObject> invSlots = new List<GameObject>();
    private List<GameObject> toiletSlots = new List<GameObject>();
    private Transform ic;
    private InventorySelection selectionScript;
    private Transform mc;
    private PauseController pc;
    private void Start()
    {
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        player = RootObjectCache.GetRoot("Player").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        mcs = ic.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        selectionScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<InventorySelection>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();

        foreach(Transform slot in ic.Find("GUIPanel"))
        {
            invSlots.Add(slot.gameObject);
        }
        foreach(Transform slot in transform.Find("Items"))
        {
            toiletSlots.Add(slot.gameObject);
        }

        CloseMenu();
    }
    private void Update()
    {
        if (!inMenu)
        {
            foreach (Transform toilet in tiles.Find("GroundObjects"))
            {
                if (toilet.name.StartsWith("Toilet") && mcs.isTouchingToilet && mcs.touchedToilet.transform.position == toilet.position && Input.GetMouseButtonDown(0))
                {
                    float distance = Vector2.Distance(player.position, toilet.position);
                    if (distance <= 2.4f)
                    {
                        if(selectionScript.aSlotSelected && inventoryScript.inventory[selectionScript.selectedSlotNum].itemData.id == 122)
                        {
                            UnclogToilet(toilet.gameObject);
                            return;
                        }
                        
                        currentToilet = toilet.gameObject;
                        OpenMenu();
                    }
                    break;
                }
            }
        }
        else //in menu
        {
            if(Input.GetMouseButtonDown(0) && mcs.isTouchingToiletSlot) //toilet to inv
            {
                int index = Convert.ToInt32(mcs.touchedToiletSlot.name.Replace("Item", ""));
                ItemData data = currentToilet.GetComponent<ToiletInv>().toiletInv[index];

                if(data == null)
                {
                    return;
                }

                int emptyInvIndex = 0;
                for(int i = 0; i < 6; i++)
                {
                    if (inventoryScript.inventory[i].itemData != null)
                    {
                        invIsFull = true;
                    }
                    else if (inventoryScript.inventory[i].itemData == null)
                    {
                        invIsFull = false;
                        emptyInvIndex = i;
                        break;
                    }
                }

                if (invIsFull)
                {
                    return;
                }

                mcs.touchedToiletSlot.GetComponent<Image>().sprite = clear;
                currentToilet.GetComponent<ToiletInv>().toiletInv[index] = null;
                inventoryScript.inventory[emptyInvIndex].itemData = data;
                invSlots[emptyInvIndex].GetComponent<Image>().sprite = data.sprite;
            }
            else if(Input.GetMouseButtonDown(0) && mcs.isTouchingInvSlot) //inv to toilet
            {
                int index = Convert.ToInt32(mcs.touchedInvSlot.name.Replace("Slot", "")) - 1;
                ItemData data = inventoryScript.inventory[index].itemData;

                if(data == null)
                {
                    return;
                }

                int emptyToiletIndex = 0;
                for(int i = 0; i < 3; i++)
                {
                    if (currentToilet.GetComponent<ToiletInv>().toiletInv[i] != null)
                    {
                        toiletIsFull = true;
                    }
                    else
                    {
                        toiletIsFull = false;
                        emptyToiletIndex = i;
                        break;
                    }
                }

                if (toiletIsFull)
                {
                    return;
                }

                mcs.touchedInvSlot.GetComponent<Image>().sprite = clear;
                inventoryScript.inventory[index].itemData = null;
                currentToilet.GetComponent<ToiletInv>().toiletInv[emptyToiletIndex] = data;
                toiletSlots[emptyToiletIndex].GetComponent<Image>().sprite = data.sprite;
            }
            else if(!mcs.isTouchingInvSlot && !mcs.isTouchingIDPanel && !mcs.isTouchingToiletSlot &&!mcs.isTouchingExtra && !mcs.isTouchingToilet && Input.GetMouseButtonDown(0))
            {
                bool shouldClose = true;
                if(mcs.isTouchingButton && mcs.touchedButton.name != "FlushButton")
                {
                    shouldClose = true;
                }
                else if(mcs.isTouchingButton && mcs.touchedButton.name == "FlushButton")
                {
                    shouldClose = false;
                }
                if (!shouldClose)
                {
                    return;
                }

                CloseMenu();
            }
        }
    }
    private void OpenMenu()
    {
        Debug.Log("Opening menu");
        mc.Find("Black").GetComponent<Image>().enabled = true;
        SetItems();
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        inMenu = true;
        pc.Pause(false);
    }
    private void CloseMenu()
    {
        Debug.Log("Closing Menu");
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        inMenu = false;
    }
    private void SetItems()
    {
        for(int i = 0; i < 3; i++)
        {
            if (currentToilet.GetComponent<ToiletInv>().toiletInv[i] != null)
            {
                toiletSlots[i].GetComponent<Image>().sprite = currentToilet.GetComponent<ToiletInv>().toiletInv[i].sprite;
            }
            else
            {
                toiletSlots[i].GetComponent<Image>().sprite = clear;
            }
        }
    }
    public void Flush()
    {
        if(currentToilet.GetComponent<ToiletInv>().flushTimer > 0 || 
            currentToilet.GetComponent<ToiletInv>().isClogged)
        {
            return;
        }
        
        int rand = UnityEngine.Random.Range(0, 10);
        if(rand == 0)
        {
            ClogToilet(currentToilet);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                currentToilet.GetComponent<ToiletInv>().toiletInv[i] = null;
                toiletSlots[i].GetComponent<Image>().sprite = clear;
            }
            StartCoroutine(FlushWait(currentToilet));
        }
    }
    private void ClogToilet(GameObject toilet)
    {
        toilet.GetComponent<ToiletInv>().isClogged = true;
        Debug.Log("Toilet got clogged");
        //do water animation stuff
        for(int i = 0; i < 4; i++)
        {
            Vector2 pos = new Vector2(toilet.transform.position.x, toilet.transform.position.y);
            switch (i)
            {
                case 0:
                    pos += new Vector2(.4f, .4f);
                    break;
                case 1:
                    pos += new Vector2(.4f, -.4f);
                    break;
                case 2:
                    pos += new Vector2(-.4f, .4f);
                    break;
                case 3:
                    pos += new Vector2(-.4f, -.4f);
                    break;
            }
            GameObject toiletWater = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/ToiletWater"));
            toiletWater.name = "ToiletWater";
            toiletWater.transform.parent = tiles.Find("GroundObjects");
            toiletWater.transform.position = pos;
            toiletWater.transform.position += new Vector3(0, 0, -1);
            toiletWater.GetComponent<ToiletWater>().waterLevel = 4;
        }
    }
    private void UnclogToilet(GameObject toilet)
    {
        if (!toilet.GetComponent<ToiletInv>().isClogged)
        {
            return;
        }

        //make toilet normal and destroy all water tiles DONT BREAK THE PLUNGER
    }
    private IEnumerator FlushWait(GameObject toilet)
    {
        toilet.GetComponent<ToiletInv>().flushTimer = 50;
        
        while (true)
        {
            yield return new WaitForSeconds(1);
            toilet.GetComponent<ToiletInv>().flushTimer--;

            if (toilet.GetComponent<ToiletInv>().flushTimer == 0)
            {
                break;
            }
        }
    }
}
