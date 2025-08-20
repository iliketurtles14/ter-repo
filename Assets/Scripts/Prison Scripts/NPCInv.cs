using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInv : MonoBehaviour
{
    private Transform aStar;
    private GameObject npc;
    private string menuText;
    private List<GameObject> npcInvSlots = new List<GameObject>();
    private List<GameObject> invSlots = new List<GameObject>();
    private Transform ic;
    private List<InventoryItem> inventoryList = new List<InventoryItem>();
    private Inventory inventoryScript;
    private Transform mc;
    private bool menuIsOpen;
    private MouseCollisionOnItems mcs;
    private GameObject player;
    private int invSlotNumber;
    public List<NPCInvItem> npcInv = new List<NPCInvItem>();
    private bool npcInvIsFull;
    private Sprite clearSprite;
    private bool invIsFull;
    private int npcInvSlotNumber;
    private PauseController pauseController;
    private NPCInvItem weapon;
    private NPCInvItem outfit;
    public void Start()
    {
        aStar = RootObjectCache.GetRoot("A*").transform;
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        mc = RootObjectCache.GetRoot("MainCanvas").transform;
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        mcs = mc.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        clearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        pauseController = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();

        //make slot list
        foreach(Transform child in transform.Find("ItemPanel"))
        {
            npcInvSlots.Add(child.gameObject);
        }
        foreach(Transform child in ic.Find("GUIPanel"))
        {
            invSlots.Add(child.gameObject);
        }
        inventoryList = inventoryScript.inventory;
        //disable menu
        CloseNPCInv();
    }
    public void Update()
    {
        if (!menuIsOpen)
        {
            if(mcs.isTouchingNPC && mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead)
            {
                float distance = Vector2.Distance(player.transform.position, mcs.touchedNPC.transform.position);
                if(distance <= 2.4f && Input.GetMouseButtonDown(0))
                {
                    npc = mcs.touchedNPC;
                    menuText = npc.GetComponent<NPCCollectionData>().npcData.displayName + "'s Pockets";
                    npcInv = npc.GetComponent<NPCInvData>().npcInv;
                    weapon = npc.GetComponent<NPCInvData>().weapon;
                    outfit = npc.GetComponent<NPCInvData>().outfit;
                    StartCoroutine(OpenNPCInv());
                }
            }
        }
        if (menuIsOpen)
        {
            //putting items in menu
            for(int i = 0; i <= 5; i++)
            {
                if (npcInv[i].itemData != null)
                {
                    npcInvIsFull = true;
                }
                else if (npcInv[i].itemData == null)
                {
                    npcInvIsFull = false;
                    break;
                }
            }
            
            if (mcs.isTouchingInvSlot)
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (mcs.touchedInvSlot.name == "Slot" + i)
                    {
                        invSlotNumber = i - 1;
                        break;
                    }
                }
            }

            if(mcs.isTouchingInvSlot && inventoryList[invSlotNumber].itemData != null && Input.GetMouseButtonDown(0) && !npcInvIsFull)
            {
                for(int i = 0; i < npcInv.Count; i++)
                {
                    if (npcInv[i].itemData == null)
                    {
                        npcInv[i].itemData = inventoryList[invSlotNumber].itemData;
                        npcInvSlots[i].GetComponent<Image>().sprite = inventoryList[invSlotNumber].itemData.icon;
                        break;
                    }
                }
                inventoryList[invSlotNumber].itemData = null;
                mcs.touchedInvSlot.GetComponent<Image>().sprite = clearSprite;
            }

            //putting items in the player inv
            for(int i = 0; i <= 5; i++)
            {
                if (inventoryList[i].itemData != null)
                {
                    invIsFull = true;
                }
                else if (inventoryList[i].itemData == null)
                {
                    invIsFull = false;
                    break;
                }
            }

            if (mcs.isTouchingNPCInvSlot && mcs.touchedNPCInvSlot.name.StartsWith("Slot"))
            {
                for(int i = 1; i <= 6; i++)
                {
                    if(mcs.touchedNPCInvSlot.name == "Slot" + i)
                    {
                        npcInvSlotNumber = i - 1;
                        break;
                    }
                }
            }

            if(mcs.isTouchingNPCInvSlot && mcs.touchedNPCInvSlot.name.StartsWith("Slot") && npcInv[npcInvSlotNumber].itemData != null && Input.GetMouseButtonDown(0) && !invIsFull)
            {
                foreach(InventoryItem slot in inventoryList)
                {
                    if(slot.itemData == null)
                    {
                        slot.itemData = npcInv[npcInvSlotNumber].itemData;
                        break;
                    }
                }
                foreach(GameObject slot in invSlots)
                {
                    if(slot.GetComponent<Image>().sprite == clearSprite)
                    {
                        slot.GetComponent<Image>().sprite = npcInv[npcInvSlotNumber].itemData.icon;
                        break;
                    }
                }
                npcInv[npcInvSlotNumber].itemData = null;
                mcs.touchedNPCInvSlot.GetComponent<Image>().sprite = clearSprite;
            }

            if(mcs.isTouchingNPCInvSlot && ((mcs.touchedNPCInvSlot.name == "Weapon" && weapon.itemData != null) || (mcs.touchedNPCInvSlot.name == "Outfit" && outfit.itemData != null)) && Input.GetMouseButtonDown(0) && !invIsFull)
            {
                NPCInvItem item;
                if(mcs.touchedNPCInvSlot.name == "Weapon")
                {
                    item = weapon;
                }
                else if(mcs.touchedNPCInvSlot.name == "Outfit")
                {
                    item = outfit;
                }
                else
                {
                    item = null;
                }
                foreach(InventoryItem slot in inventoryList)
                {
                    if(slot.itemData == null)
                    {
                        slot.itemData = item.itemData;
                    }
                }
                foreach(GameObject slot in invSlots)
                {
                    if(slot.GetComponent<Image>().sprite == clearSprite)
                    {
                        slot.GetComponent<Image>().sprite = item.itemData.icon;
                        break;
                    }
                }
                if(mcs.touchedNPCInvSlot.name == "Weapon")
                {
                    weapon.itemData = null;
                    transform.Find("Weapon").GetComponent<Image>().sprite = clearSprite;
                }
                else if(mcs.touchedNPCInvSlot.name == "Outfit")
                {
                    outfit.itemData = null;
                    transform.Find("Outfit").GetComponent<Image>().sprite = clearSprite;
                }
            }
            //exiting the menu
            if(!mcs.isTouchingInvSlot && !mcs.isTouchingNPCInvPanel && !mcs.isTouchingNPCInvSlot && !mcs.isTouchingExtra && Input.GetMouseButtonDown(0))
            {
                CloseNPCInv();
            }
        }
    }
    public IEnumerator OpenNPCInv()
    {
        for(int i = 0; i < 6; i++)
        {
            if (npcInv[i].itemData != null)
            {
                npcInvSlots[i].GetComponent<Image>().sprite = npcInv[i].itemData.icon;
            }
        }

        mc.Find("Black").GetComponent<Image>().enabled = true;
        mc.Find("NPCInvMenu").gameObject.SetActive(true);

        pauseController.Pause(false);

        yield return new WaitForEndOfFrame();

        menuIsOpen = true;
    }
    public void CloseNPCInv()
    {
        foreach (GameObject slot in npcInvSlots)
        {
            slot.GetComponent<Image>().sprite = clearSprite;
        }

        mc.Find("Black").GetComponent<Image>().enabled = false;
        mc.Find("NPCInvMenu").gameObject.SetActive(false);

        menuIsOpen = false;

        pauseController.Unpause();
    }
}
