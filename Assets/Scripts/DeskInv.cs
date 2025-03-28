using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeskInv : MonoBehaviour
{
    public Canvas MenuCanvas;
    public Canvas InventoryCanvas;
    public Inventory inventoryScript;
    private List<InventoryItem> inventoryList;
    public MouseCollisionOnItems mouseCollisionScript;
    public List<DeskItem> deskInv = new List<DeskItem>();
    public GameObject player;
    public Sprite ClearSprite;
    private List<GameObject> deskSlots = new List<GameObject>();
    private List<GameObject> invSlots = new List<GameObject>();
    public int invSlotNumber;
    public int deskSlotNumber;
    private float distance;
    public bool deskIsOpen;
    public GameObject timeObject;
    private bool deskIsFull;
    private bool invIsFull;
    public void Start()
    {
        //make slot list
        foreach(Transform child in transform)
        {
            deskSlots.Add(child.gameObject);
        }
        foreach(Transform child in InventoryCanvas.transform.Find("GUIPanel"))
        {
            invSlots.Add(child.gameObject);
        }
        inventoryList = inventoryScript.inventory;
        //disable desk
        foreach(GameObject slot in deskSlots)
        {
            slot.GetComponent<BoxCollider2D>().enabled = false;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().enabled = false;
    }
    public void Update()
    {
        if (!deskIsOpen)
        {
            if (mouseCollisionScript.isTouchingDesk)
            {
                distance = Vector2.Distance(player.transform.position, mouseCollisionScript.touchedDesk.transform.position);
            }
            if (mouseCollisionScript.isTouchingDesk && Input.GetMouseButtonDown(0) && distance <= 2.4f)
            {
                StartCoroutine(OpenDesk());
            }
        }

        if (deskIsOpen)
        {


            ///continue
            //putting items in the desk
            for(int i = 0; i <= 19; i++)
            {
                if (deskInv[i].itemData != null)
                {
                    deskIsFull = true;
                }
                else if(deskInv[i].itemData == null)
                {
                    deskIsFull = false;
                    break;
                }
            }

            if (mouseCollisionScript.isTouchingInvSlot)
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (mouseCollisionScript.touchedInvSlot.name == "Slot" + i)
                    {
                        invSlotNumber = i - 1;
                        break;
                    }
                }
            }

            if (mouseCollisionScript.isTouchingInvSlot && inventoryList[invSlotNumber].itemData != null && Input.GetMouseButtonDown(0) && !deskIsFull)
            {
                for (int i = 0; i < deskInv.Count; i++)
                {
                    if (deskInv[i].itemData == null)
                    {
                        deskInv[i].itemData = inventoryList[invSlotNumber].itemData;
                        deskSlots[i].GetComponent<Image>().sprite = inventoryList[invSlotNumber].itemData.icon;
                        break;
                    }
                }
                inventoryList[invSlotNumber].itemData = null;
                mouseCollisionScript.touchedInvSlot.GetComponent<Image>().sprite = ClearSprite;
            }
            
            //putting items in the inv
            for (int i = 0; i <= 5; i++)
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

            if (mouseCollisionScript.isTouchingDeskSlot)
            {
                for (int i = 1; i <= 20; i++)
                {
                    if (mouseCollisionScript.touchedDeskSlot.name == "Slot" + i)
                    {
                        deskSlotNumber = i - 1;
                        break;
                    }
                }
            }

            if (mouseCollisionScript.isTouchingDeskSlot && deskInv[deskSlotNumber].itemData != null && Input.GetMouseButtonDown(0) && !invIsFull)
            {
                foreach (InventoryItem slot in inventoryList)
                {
                    if (slot.itemData == null)
                    {
                        slot.itemData = deskInv[deskSlotNumber].itemData;
                        break;
                    }
                }
                foreach (GameObject slot in invSlots)
                {
                    if (slot.GetComponent<Image>().sprite == ClearSprite)
                    {
                        slot.GetComponent<Image>().sprite = deskInv[deskSlotNumber].itemData.icon;
                        break;
                    }
                }
                deskInv[deskSlotNumber].itemData = null;
                mouseCollisionScript.touchedDeskSlot.GetComponent<Image>().sprite = ClearSprite;
            }
            //exiting the desk
            if (!mouseCollisionScript.isTouchingInvSlot && !mouseCollisionScript.isTouchingDeskPanel && !mouseCollisionScript.isTouchingDeskSlot && Input.GetMouseButtonDown(0))
            {
                foreach (GameObject slot in deskSlots)
                {
                    slot.GetComponent<BoxCollider2D>().enabled = false;
                    slot.GetComponent<Image>().enabled = false;
                }
                
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Image>().enabled = false;

                deskIsOpen = false;

                ///what to enable when having a desk/menu close
                mouseCollisionScript.EnableTag("Bars");
                mouseCollisionScript.EnableTag("Fence");
                mouseCollisionScript.EnableTag("ElectricFence");
                mouseCollisionScript.EnableTag("Digable");
                mouseCollisionScript.EnableTag("Wall");

                //player movement
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                player.GetComponent<PlayerCtrl>().enabled = true;

                //NPC movement
                foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
                {
                    guard.GetComponent<AILerp>().speed = 10;
                }
                foreach (GameObject inmate in GameObject.FindGameObjectsWithTag("Inmate"))
                {
                    inmate.GetComponent<AILerp>().speed = 10;
                }

                //time
                timeObject.GetComponent<Routine>().enabled = true;

                return;
            }
        }
    }
    public IEnumerator OpenDesk()
    {
        foreach (GameObject slot in deskSlots)
        {
            slot.GetComponent<BoxCollider2D>().enabled = true;
            slot.GetComponent<Image>().enabled = true;
        }
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Image>().enabled = true;

        ///what to disable when having a desk/menu open
        //player movement
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerCtrl>().enabled = false;

        mouseCollisionScript.DisableTag("Bars");
        mouseCollisionScript.DisableTag("Fence");
        mouseCollisionScript.DisableTag("ElectricFence");
        mouseCollisionScript.DisableTag("Digable");
        mouseCollisionScript.DisableTag("Wall");

        //NPC movement
        foreach (GameObject guard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            guard.GetComponent<AILerp>().speed = 0;
        }
        foreach (GameObject inmate in GameObject.FindGameObjectsWithTag("Inmate"))
        {
            inmate.GetComponent<AILerp>().speed = 0;
        }

        //time
        timeObject.GetComponent<Routine>().enabled = false;

        yield return new WaitForEndOfFrame();

        deskIsOpen = true;
    }
}
