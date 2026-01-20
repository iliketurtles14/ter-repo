using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIDInv : MonoBehaviour
{
    private GameObject MenuCanvas;
    private GameObject InventoryCanvas;
    private Inventory inventoryScript;
    private List<InventoryItem> inventoryList;
    private MouseCollisionOnItems mcs;
    private PauseController pauseScript;
    public List<IDItem> idInv = new List<IDItem>();
    private GameObject player;
    private Sprite ClearSprite;
    private GameObject outfitSlot;
    private GameObject weaponSlot;
    private List<GameObject> invSlots = new List<GameObject>();
    public int invSlotNumber;
    public bool idIsOpen;
    private GameObject timeObject;
    private bool outfitIsFull;
    private bool weaponIsFull;
    private bool invIsFull;
    public void Start()
    {
        MenuCanvas = RootObjectCache.GetRoot("MenuCanvas");
        InventoryCanvas = RootObjectCache.GetRoot("InventoryCanvas");
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        mcs = InventoryCanvas.transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        ClearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        outfitSlot = transform.Find("Outfit").gameObject;
        weaponSlot = transform.Find("Weapon").gameObject;
        timeObject = InventoryCanvas.transform.Find("Time").gameObject;
        pauseScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        
        StartCoroutine(Wait());
        foreach(Transform child in InventoryCanvas.transform.Find("GUIPanel"))
        {
            invSlots.Add(child.gameObject);
        }
        inventoryList = inventoryScript.inventory;
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = NPCSave.instance.playerName;
        CloseMenu();
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

            if(mcs.isTouchingInvSlot && inventoryList[invSlotNumber].itemData != null && inventoryList[invSlotNumber].itemData.defense != -1 && Input.GetMouseButtonDown(0) && !outfitIsFull)
            {
                idInv[0].itemData = inventoryList[invSlotNumber].itemData;
                outfitSlot.GetComponent<Image>().sprite = inventoryList[invSlotNumber].itemData.sprite;
                inventoryList[invSlotNumber].itemData = null;
                mcs.touchedInvSlot.GetComponent<Image>().sprite = ClearSprite;
            }
            else if (mcs.isTouchingInvSlot && inventoryList[invSlotNumber].itemData != null && inventoryList[invSlotNumber].itemData.strength != -1 && Input.GetMouseButtonDown(0) && !weaponIsFull)
            {
                idInv[1].itemData = inventoryList[invSlotNumber].itemData;
                weaponSlot.GetComponent<Image>().sprite = inventoryList[invSlotNumber].itemData.sprite;
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
                        slot.GetComponent<Image>().sprite = idInv[0].itemData.sprite;
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
                        slot.GetComponent<Image>().sprite = idInv[1].itemData.sprite;
                        break;
                    }
                }
                idInv[1].itemData = null;
                weaponSlot.GetComponent<Image>().sprite = ClearSprite;
            }

            if(!mcs.isTouchingIDPanel && !mcs.isTouchingButton && !mcs.isTouchingInvSlot && !mcs.isTouchingExtra && !mcs.isTouchingIDSlot && Input.GetMouseButtonDown(0))
            {
                //exiting the idmenu
                CloseMenu();

                pauseScript.Unpause();

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
        if(name == "PlayerMenuPanel")
        {
            transform.Find("Player").Find("Outfit").GetComponent<Image>().enabled = true;
        }
        else if(name == "NPCMenuPanel")
        {
            transform.Find("NPC").Find("Outfit").GetComponent<Image>().enabled = true;
        }

        MenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = true;

        pauseScript.Pause(false);

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
        if(name == "NPCMenuPanel")
        {
            transform.Find("NPC").Find("Outfit").GetComponent<Image>().enabled = false;
        }
        else if(name == "PlayerMenuPanel")
        {
            transform.Find("Player").Find("Outfit").GetComponent<Image>().enabled = false;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().enabled = false;
        MenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = false;
        idIsOpen = false;
    }
}
