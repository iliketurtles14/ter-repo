using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIDInv : MonoBehaviour
{
    public Canvas MenuCanvas;
    public Canvas InventoryCanvas;
    public Inventory inventoryScript;
    private List<InventoryItem> inventoryList;
    public MouseCollisionOnItems mcs;
    public List<IDItem> idInv = new List<IDItem>();
    public GameObject player;
    public Sprite ClearSprite;
    public GameObject outfitSlot;
    public GameObject weaponSlot;
    private List<GameObject> invSlots = new List<GameObject>();
    public int invSlotNumber;
    public bool idIsOpen;
    public GameObject timeObject;
    private bool outfitIsFull;
    private bool weaponIsFull;
    private bool invIsFull;
    public void Start()
    {
        StartCoroutine(Wait());
        foreach(Transform child in InventoryCanvas.transform.Find("GUIPanel"))
        {
            invSlots.Add(child.gameObject);
        }
        inventoryList = inventoryScript.inventory;
        //diable id menu
        CloseMenu();
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = NPCSave.instance.playerName;
    }
    public void Update()
    {
        if (!idIsOpen)
        {
            if(mcs.isTouchingButton && mcs.touchedButton.name == "PlayerIDButton" && Input.GetMouseButtonDown(0))
            {
                StartCoroutine(OpenMenu());
            }
        }
        if (idIsOpen)
        {
            ///continue
            //putting items in slots
            if (idInv[0].itemData != null)
            {
                outfitIsFull = true;
            }
            else
            {
                outfitIsFull = false;
            }
            if (idInv[1].itemData != null)
            {
                weaponIsFull = true;
            }
            else
            {
                weaponIsFull = false;
            }

            if (mcs.isTouchingInvSlot)
            {
                for(int i = 1; i <= 6; i++)
                {
                    if(mcs.touchedInvSlot.name == "Slot" + i)
                    {
                        invSlotNumber = i - 1;
                        break;
                    }
                }
            }

            if(mcs.isTouchingInvSlot && inventoryList[invSlotNumber].itemData != null && inventoryList[invSlotNumber].itemData.defence != -1 && Input.GetMouseButtonDown(0) && !outfitIsFull)
            {
                idInv[0].itemData = inventoryList[invSlotNumber].itemData;
                outfitSlot.GetComponent<Image>().sprite = inventoryList[invSlotNumber].itemData.icon;
                inventoryList[invSlotNumber].itemData = null;
                mcs.touchedInvSlot.GetComponent<Image>().sprite = ClearSprite;
            }
            else if (mcs.isTouchingInvSlot && inventoryList[invSlotNumber].itemData != null && inventoryList[invSlotNumber].itemData.strength != -1 && Input.GetMouseButtonDown(0) && !weaponIsFull)
            {
                idInv[1].itemData = inventoryList[invSlotNumber].itemData;
                weaponSlot.GetComponent<Image>().sprite = inventoryList[invSlotNumber].itemData.icon;
                inventoryList[invSlotNumber].itemData = null;
                mcs.touchedInvSlot.GetComponent<Image>().sprite = ClearSprite;
            }


            //putting items in the inv
            for (int i = 0; i <= 5; i++)
            {
                if(inventoryList[i].itemData != null)
                {
                    invIsFull = true;
                }
                else if (inventoryList[i].itemData == null)
                {
                    invIsFull = false;
                    break;
                }
            }

            if (mcs.isTouchingIDSlot && mcs.touchedIDSlot.name == "Outfit" && idInv[0].itemData != null && Input.GetMouseButtonDown(0) && !invIsFull)
            {
                foreach(InventoryItem slot in inventoryList)
                {
                    if(slot.itemData == null)
                    {
                        slot.itemData = idInv[0].itemData;
                        break;
                    }
                }
                foreach(GameObject slot in invSlots)
                {
                    if(slot.GetComponent<Image>().sprite == ClearSprite)
                    {
                        slot.GetComponent<Image>().sprite = idInv[0].itemData.icon;
                        break;
                    }
                }
                idInv[0].itemData = null;
                outfitSlot.GetComponent<Image>().sprite = ClearSprite;
            }
            else if(mcs.isTouchingIDSlot && mcs.touchedIDSlot.name == "Weapon" && idInv[1].itemData != null && Input.GetMouseButtonDown(0) && !invIsFull)
            {
                foreach (InventoryItem slot in inventoryList)
                {
                    if (slot.itemData == null)
                    {
                        slot.itemData = idInv[1].itemData;
                        break;
                    }
                }
                foreach (GameObject slot in invSlots)
                {
                    if (slot.GetComponent<Image>().sprite == ClearSprite)
                    {
                        slot.GetComponent<Image>().sprite = idInv[1].itemData.icon;
                        break;
                    }
                }
                idInv[1].itemData = null;
                weaponSlot.GetComponent<Image>().sprite = ClearSprite;
            }

            if(!mcs.isTouchingIDPanel && !mcs.isTouchingButton && Input.GetMouseButtonDown(0))
            {
                //exiting the idmenu
                CloseMenu();

                ///what to enabled when having a menu close
                mcs.EnableTag("Bars");
                mcs.EnableTag("Fence");
                mcs.EnableTag("ElectricFence");
                mcs.EnableTag("Digable");
                mcs.EnableTag("Wall");
                mcs.EnableTag("Item");
                mcs.EnableTag("Desk");

                //player movement
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                player.GetComponent<PlayerCtrl>().enabled = true;
                player.GetComponent<PlayerAnimation>().enabled = true;

                //NPC movement
                foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
                {
                    guard.GetComponent<AILerp>().speed = 10;
                    guard.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    guard.GetComponent<NPCAnimation>().enabled = true;
                }
                foreach (GameObject inmate in GameObject.FindGameObjectsWithTag("Inmate"))
                {
                    inmate.GetComponent<AILerp>().speed = 10;
                    inmate.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    inmate.GetComponent<NPCAnimation>().enabled = true;
                }
                //time
                timeObject.GetComponent<Routine>().enabled = true;

                return;
            }
        }
    }
    public IEnumerator OpenMenu()
    {
        foreach(Transform child in transform)
        {
            if(child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = true;
            }
            if(child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        transform.Find("WeaponText").gameObject.SetActive(true);
        transform.Find("OutfitText").gameObject.SetActive(true);
        transform.Find("NameText").gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Image>().enabled = true;
        transform.Find("Player").Find("Outfit").GetComponent<Image>().enabled = true;

        ///what to disable when having a desk/menu open
        //player movement
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerCtrl>().enabled = false;
        player.GetComponent<PlayerAnimation>().enabled = false;

        mcs.DisableTag("Bars");
        mcs.DisableTag("Fence");
        mcs.DisableTag("ElectricFence");
        mcs.DisableTag("Digable");
        mcs.DisableTag("Wall");
        mcs.DisableTag("Item");
        mcs.DisableTag("Desk");


        //NPC movement
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            guard.GetComponent<AILerp>().speed = 0;
            guard.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            guard.GetComponent<NPCAnimation>().enabled = false;
        }
        foreach (GameObject inmate in GameObject.FindGameObjectsWithTag("Inmate"))
        {
            inmate.GetComponent<AILerp>().speed = 0;
            inmate.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            inmate.GetComponent<NPCAnimation>().enabled = false;
        }

        //time
        timeObject.GetComponent<Routine>().enabled = false;

        yield return new WaitForEndOfFrame();

        idIsOpen = true;
    }
    public void CloseMenu()
    {
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
        }
        foreach (Transform child in transform.Find("StrengthPanel"))
        {
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = false;
            }
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        foreach (Transform child in transform.Find("SpeedPanel"))
        {
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = false;
            }
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        foreach (Transform child in transform.Find("IntellectPanel"))
        {
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = false;
            }
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        transform.Find("WeaponText").gameObject.SetActive(false);
        transform.Find("OutfitText").gameObject.SetActive(false);
        transform.Find("NameText").gameObject.SetActive(false);
        transform.Find("Player").Find("Outfit").GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().enabled = false;
        idIsOpen = false;
    }
}
