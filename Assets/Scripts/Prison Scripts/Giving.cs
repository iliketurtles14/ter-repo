using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Giving : MonoBehaviour
{
    public bool slotIsFull;
    public NPCInvItem item; //just using npcinvitem cuz i dont wanna make another type of item
    private Transform slot;
    public bool menuIsOpen;
    private Sprite clearSprite;
    private MouseCollisionOnItems mcs;
    private int invSlotNumber;
    private Inventory inventoryScript;
    private List<GameObject> invSlots = new List<GameObject>();
    private Transform ic;
    private Transform mc;
    private bool invIsFull;
    private NPCIDInv npcIDInvScript;
    public GameObject currentNPC;
    private PauseController pc;
    public bool canChangeMoney;
    private int money;
    private Transform player;
    private SpecialMessages specialMessagesScript;
    private MissionAsk missionAskScript;
    private ShopMenu shopMenuScript;
    private void Start()
    {
        clearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        slot = transform.Find("GiveSlot").Find("Item");
        mc = transform.parent.transform;
        npcIDInvScript = mc.Find("NPCMenuPanel").GetComponent<NPCIDInv>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        player = RootObjectCache.GetRoot("Player").transform;
        specialMessagesScript = ic.Find("SpecialMessagePanel").GetComponent<SpecialMessages>();
        missionAskScript = mc.Find("MissionPanel").GetComponent<MissionAsk>();
        shopMenuScript = mc.Find("NPCShopMenuPanel").GetComponent<ShopMenu>();

        canChangeMoney = true;

        foreach(Transform child in ic.Find("GUIPanel"))
        {
            invSlots.Add(child.gameObject);
        }
        StartCoroutine(CloseMenu(false, false));
    }
    private void Update()
    {
        //update money counter
        transform.Find("MoneyText").GetComponent<TextMeshProUGUI>().text = "$" + money.ToString();
        
        try
        {
            if (item.itemData.sprite == null)
            {
                item = new NPCInvItem();
            }
        }
        catch { }

        if (menuIsOpen)
        {
            //load item
            Sprite itemSprite;
            try
            {
                itemSprite = item.itemData.sprite;
            }
            catch
            {
                itemSprite = clearSprite;
            }
            if(itemSprite == null)
            {
                itemSprite = clearSprite;
            }

            transform.Find("GiveSlot").Find("Item").GetComponent<Image>().sprite = itemSprite;

            //putting in items
            slotIsFull = true;
            try
            {
                if (item.itemData == null)
                {
                    slotIsFull = false;
                }
            }
            catch
            {
                slotIsFull = false;
            }

            if (mcs.isTouchingInvSlot)
            {
                invSlotNumber = Convert.ToInt32(mcs.touchedInvSlot.name.Replace("Slot", "")) - 1;
            }

            if(mcs.isTouchingInvSlot && Input.GetMouseButtonDown(0) && !slotIsFull)
            {
                item = new NPCInvItem();
                item.itemData = inventoryScript.inventory[invSlotNumber].itemData;
                slot.GetComponent<Image>().sprite = inventoryScript.inventory[invSlotNumber].itemData.sprite;
                inventoryScript.inventory[invSlotNumber].itemData = null;
                mcs.touchedInvSlot.GetComponent<Image>().sprite = clearSprite;
            }
            //putting items in inv
            for(int i = 0; i < 6; i++)
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

            if (mcs.isTouchingGiveSlot && mcs.touchedGiveSlot.name == "Item" && slotIsFull && Input.GetMouseButtonDown(0) && !invIsFull)
            {
                foreach (InventoryItem slot in inventoryScript.inventory)
                {
                    if (slot.itemData == null)
                    {
                        slot.itemData = item.itemData;
                        break;
                    }
                }
                foreach (GameObject slot in invSlots)
                {
                    if (slot.GetComponent<Image>().sprite == clearSprite)
                    {
                        slot.GetComponent<Image>().sprite = item.itemData.sprite;
                        break;
                    }
                }

                item.itemData = null;
                slot.GetComponent<Image>().sprite = clearSprite;
            }
            //exiting the menu
            if(!mcs.isTouchingInvSlot && !mcs.isTouchingIDPanel && !mcs.isTouchingGiveSlot && !mcs.isTouchingExtra && Input.GetMouseButtonDown(0) && !mcs.isTouchingButton)
            {
                StartCoroutine(CloseMenu(false, false));
            }

            //adding/subtracting money
            if (mcs.isTouchingButton && canChangeMoney && Input.GetMouseButton(0))
            {
                if(mcs.touchedButton.name == "AddButton" && money < player.GetComponent<PlayerCollectionData>().playerData.money)
                {
                    StartCoroutine(ChangeMoney(true));
                }
                else if(mcs.touchedButton.name == "SubtractButton" && money > 0)
                {
                    StartCoroutine(ChangeMoney(false));
                }
            }
        }
    }
    private IEnumerator ChangeMoney(bool add)
    {
        canChangeMoney = false;
        if (add)
        {
            money++;
        }
        else
        {
            money--;
        }
        yield return new WaitForSeconds(.05f);
        canChangeMoney = true;
    }
    public void OpenMenu()
    {
        menuIsOpen = true;
        foreach(Transform child in transform)
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
        transform.Find("MoneyText").gameObject.SetActive(true);
        transform.Find("GiveButton").Find("GiveText").gameObject.SetActive(true);
        transform.Find("GiveSlot").Find("Item").GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public IEnumerator CloseMenu(bool goToID, bool goToShop)
    {
        GiveBackOnClose();
        
        if (goToID)
        {
            npcIDInvScript.OpenMenu();
        }
        else if (goToShop)
        {
            shopMenuScript.OpenMenu();
        }
        else
        {
            mc.Find("Black").GetComponent<Image>().enabled = false;
            pc.Unpause();
        }
        foreach (Transform child in transform)
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
        transform.Find("MoneyText").gameObject.SetActive(false);
        transform.Find("GiveButton").Find("GiveText").gameObject.SetActive(false);
        transform.Find("GiveSlot").Find("Item").GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForEndOfFrame();
        menuIsOpen = false;
        money = 0;
    }
    private void GiveBackOnClose()
    {
        if (!slotIsFull)
        {
            return;
        }

        foreach (InventoryItem slot in inventoryScript.inventory)
        {
            if (slot.itemData == null)
            {
                slot.itemData = item.itemData;
                break;
            }
        }
        foreach (GameObject slot in invSlots)
        {
            if (slot.GetComponent<Image>().sprite == clearSprite)
            {
                slot.GetComponent<Image>().sprite = item.itemData.sprite;
                break;
            }
        }

        item.itemData = null;
        slot.GetComponent<Image>().sprite = clearSprite;
    }
    public void Give()
    {
        int opinionToGive;
        int moneyOpn = Mathf.FloorToInt(money / 10f);
        int itemOpn = 0;
        try
        {
            itemOpn = item.itemData.opinion * 3;
        }
        catch { }
        opinionToGive = moneyOpn + itemOpn;

        currentNPC.GetComponent<NPCCollectionData>().npcData.opinion += opinionToGive;

        player.GetComponent<PlayerCollectionData>().playerData.money -= money;

        if (slotIsFull)
        {
            bool gaveItem = false;
            for (int i = 0; i < 6; i++)
            {
                if (currentNPC.GetComponent<NPCCollectionData>().npcData.inventory[i].itemData == null)
                {
                    currentNPC.GetComponent<NPCCollectionData>().npcData.inventory[i].itemData = item.itemData;
                    gaveItem = true;
                    break;
                }
            }
            if (!gaveItem)
            {
                currentNPC.GetComponent<NPCCollectionData>().npcData.inventory[5].itemData = item.itemData;
            }

            GiftFavor();
        }

        item = null;
        money = 0;
    }
    private void GiftFavor()
    {
        List<Mission> giftMissions = new List<Mission>();
        foreach(Mission mission in missionAskScript.savedMissions)
        {
            if(mission.type == "give")
            {
                giftMissions.Add(mission);
            }
        }

        if(giftMissions.Count == 0)
        {
            return;
        }

        foreach(Mission mission in giftMissions)
        {
            if (mission.target == currentNPC.GetComponent<NPCCollectionData>().npcData.displayName &&
                mission.item == item.itemData.id)
            {
                StartCoroutine(specialMessagesScript.MakeMessage("You completed a Favor!\n+$" + mission.pay, "favor"));
                player.GetComponent<PlayerCollectionData>().playerData.money += mission.pay;
                missionAskScript.savedMissions.Remove(mission);
            }
        }
    }
}
