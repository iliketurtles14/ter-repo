using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public bool menuIsOpen;
    public List<NPCInvItem> shopItems = new List<NPCInvItem>();
    private Sprite clearSprite;
    private List<GameObject> shopSlots = new List<GameObject>();
    private List<int> prices = new List<int>() { -1, -1, -1, -1 };
    private bool invIsFull;
    private Inventory inventoryScript;
    private MouseCollisionOnItems mcs;
    private int shopSlotNum;
    private Transform player;
    private int playerMoney;
    private List<GameObject> invSlots = new List<GameObject>();
    private Transform ic;
    private Transform mc;
    private NPCIDInv npcIDInvScript;
    private Giving givingScript;
    public GameObject currentNPC;
    private PauseController pc;
    private Shops shopsScript;
    private List<GameObject> costObjects = new List<GameObject>();
    private void Start()
    {
        clearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        npcIDInvScript = mc.Find("NPCMenuPanel").GetComponent<NPCIDInv>();
        givingScript = mc.Find("NPCGiveMenuPanel").GetComponent<Giving>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        shopsScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Shops>();

        foreach(Transform slot in transform.Find("SlotGrid"))
        {
            shopSlots.Add(slot.Find("Item").gameObject);
        }
        foreach(Transform slot in ic.Find("GUIPanel"))
        {
            invSlots.Add(slot.gameObject);
        }
        foreach(Transform child in transform.Find("CostGrid"))
        {
            costObjects.Add(child.gameObject);
        }

        StartCoroutine(CloseMenu(false, false));
    }
    private void Update()
    {
        playerMoney = player.GetComponent<PlayerCollectionData>().playerData.money;
        
        if (menuIsOpen)
        {
            //load items
            for(int i = 0; i < 4; i++)
            {
                Sprite itemSprite;
                try
                {
                    itemSprite = shopItems[i].itemData.sprite;
                }
                catch
                {
                    itemSprite = clearSprite;
                }
                if(itemSprite == null)
                {
                    itemSprite = clearSprite;
                }

                shopSlots[i].GetComponent<Image>().sprite = itemSprite;
            }

            //buying
            for (int i = 0; i < 6; i++)
            {
                if (inventoryScript.inventory[i].itemData != null)
                {
                    invIsFull = true;
                }
                else
                {
                    invIsFull = false;
                    break;
                }
            }

            if (mcs.isTouchingShopSlot)
            {
                shopSlotNum = Convert.ToInt32(mcs.touchedShopSlot.transform.parent.name.Replace("Slot", ""));
            }

            if(mcs.isTouchingShopSlot && mcs.touchedShopSlot.name.StartsWith("Item") &&
                mcs.touchedShopSlot.GetComponent<Image>().sprite != clearSprite && 
                Input.GetMouseButtonDown(0) && !invIsFull && playerMoney >= prices[shopSlotNum])
            {
                foreach(InventoryItem slot in inventoryScript.inventory)
                {
                    if(slot.itemData == null)
                    {
                        slot.itemData = shopItems[shopSlotNum].itemData;
                        break;
                    }
                }
                foreach(GameObject slot in invSlots)
                {
                    if (slot.GetComponent<Image>().sprite == clearSprite)
                    {
                        slot.GetComponent<Image>().sprite = shopItems[shopSlotNum].itemData.sprite;
                        break;
                    }
                }

                shopItems[shopSlotNum].itemData = null;
                shopSlots[shopSlotNum].GetComponent<Image>().sprite = clearSprite;
                costObjects[shopSlotNum].GetComponent<TextMeshProUGUI>().text = "";
                player.GetComponent<PlayerCollectionData>().playerData.money -= prices[shopSlotNum];
            }

            //exiting the menu
            if(!mcs.isTouchingInvSlot && !mcs.isTouchingIDPanel && !mcs.isTouchingShopSlot && !mcs.isTouchingExtra && Input.GetMouseButtonDown(0) && !mcs.isTouchingButton)
            {
                StartCoroutine(CloseMenu(false, false));
            }
        }
    }
    public IEnumerator CloseMenu(bool goToID, bool goToGive)
    {
        if (goToID)
        {
            npcIDInvScript.OpenMenu();
        }
        else if (goToGive)
        {
            givingScript.OpenMenu();
        }
        else
        {
            mc.Find("Black").GetComponent<Image>().enabled = false;
            pc.Unpause();
        }
        
        foreach(Transform child in transform)
        {
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = false;
            }
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
            if (child.GetComponent<EventTrigger>() != null)
            {
                child.GetComponent<EventTrigger>().enabled = false;
            }
        }
        transform.Find("SlotGrid").gameObject.SetActive(false);
        transform.Find("CostGrid").gameObject.SetActive(false);
        transform.Find("NothingText").gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForEndOfFrame();
        menuIsOpen = false;
    }
    public void OpenMenu()
    {
        menuIsOpen = true;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = true;
            }
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = true;
            }
            if (child.GetComponent<EventTrigger>() != null)
            {
                child.GetComponent<EventTrigger>().enabled = true;
            }
        }
        if (currentNPC.GetComponent<NPCCollectionData>().npcData.hasShop)
        {
            SetShops();
            transform.Find("SlotGrid").gameObject.SetActive(true);
            transform.Find("CostGrid").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("NothingText").gameObject.SetActive(true);
        }
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Image>().enabled = true;
    }
    private void SetShops()
    {
        if(currentNPC == shopsScript.shop1NPC)
        {
            shopItems = shopsScript.shop1;
        }
        else if(currentNPC == shopsScript.shop2NPC)
        {
            shopItems = shopsScript.shop2;
        }

        float opnMult = 1.5f - (currentNPC.GetComponent<NPCCollectionData>().npcData.opinion / 100f);

        for(int i = 0; i < 4; i++)
        {
            try
            {
                prices[i] = Mathf.CeilToInt(shopItems[i].itemData.cost * opnMult);
                costObjects[i].GetComponent<TextMeshProUGUI>().text = "$" + prices[i].ToString();
            }
            catch
            {
                prices[i] = -1;
                costObjects[i].GetComponent<TextMeshProUGUI>().text = "";
            }
            if (prices[i] == -1)
            {
                costObjects[i].GetComponent<TextMeshProUGUI>().text = "";
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Sprite itemSprite;
            try
            {
                itemSprite = shopItems[i].itemData.sprite;
            }
            catch
            {
                itemSprite = clearSprite;
            }
            if (itemSprite == null)
            {
                itemSprite = clearSprite;
            }

            shopSlots[i].GetComponent<Image>().sprite = itemSprite;
        }
    }
}
