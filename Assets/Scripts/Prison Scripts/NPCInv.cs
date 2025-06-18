using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInv : MonoBehaviour
{
    public Transform aStar;
    private GameObject npc;
    private string menuText;
    private List<GameObject> npcInvSlots = new List<GameObject>();
    private List<GameObject> invSlots = new List<GameObject>();
    public Transform ic;
    private List<InventoryItem> inventoryList = new List<InventoryItem>();
    public Inventory inventoryScript;
    public Transform mc;
    private bool menuIsOpen;
    public MouseCollisionOnItems mcs;
    public GameObject player;
    private int invSlotNumber;
    public List<NPCInvItem> npcInv = new List<NPCInvItem>();
    private bool npcInvIsFull;
    public Sprite clearSprite;
    private bool invIsFull;
    private int npcInvSlotNumber;
    public PauseController pauseController;
    public NPCInvItem weapon;
    public NPCInvItem outfit;
    public void Start()
    {
        //what npc to take
        int num = 0;
        Match match = Regex.Match(name, @"\d+");
        if (match.Success)
        {
            num = int.Parse(match.Value);
        }

        if (name.StartsWith("Inmate"))
        {
            foreach(Transform child in aStar)
            {
                if(child.name == "Inmate" + num)
                {
                    npc = child.gameObject;
                    break;
                }
            }
        }
        else if (name.StartsWith("Guard"))
        {
            foreach(Transform child in aStar)
            {
                if(child.name == "Guard" + num)
                {
                    npc = child.gameObject;
                    break;
                }
            }
        }
        StartCoroutine(StartWait());

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
    public IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();

        //what text to take
        menuText = npc.name + "\'s Pockets";
    }
    public void Update()
    {
        if (!menuIsOpen)
        {
            if(mcs.isTouchingNPC && mcs.touchedNPC.name == npc.name && npc.GetComponent<NPCCollectionData>().npcData.isDead)
            {
                float distance = Vector2.Distance(player.transform.position, mcs.touchedNPC.transform.position);
                if(distance <= 2.4f && Input.GetMouseButtonDown(0))
                {
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
        foreach(GameObject slot in npcInvSlots)
        {
            slot.GetComponent<BoxCollider2D>().enabled = true;
            slot.GetComponent<Image>().enabled = true;
        }

        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Image>().enabled = true;

        mc.Find("BlacK").GetComponent<Image>().enabled = true;
        transform.Find("Weapon").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("Weapon").GetComponent<Image>().enabled = true;
        transform.Find("Outfit").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("Outfit").GetComponent<Image>().enabled = true;
        transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = menuText;

        pauseController.Pause(false);

        yield return new WaitForEndOfFrame();

        menuIsOpen = true;
    }
    public void CloseNPCInv()
    {
        foreach (GameObject slot in npcInvSlots)
        {
            slot.GetComponent<BoxCollider2D>().enabled = false;
            slot.GetComponent<Image>().enabled = false;
        }

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().enabled = false;

        mc.Find("Black").GetComponent<Image>().enabled = false;
        transform.Find("Weapon").GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Weapon").GetComponent<Image>().enabled = false;
        transform.Find("Outfit").GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Outfit").GetComponent<Image>().enabled = false;
        transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = null;

        menuIsOpen = false;

        pauseController.Unpause();
    }
}
