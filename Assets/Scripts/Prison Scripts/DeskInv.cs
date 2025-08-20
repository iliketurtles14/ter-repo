using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;
using System;

public class DeskInv : MonoBehaviour
{
    private GameObject MenuCanvas;
    private GameObject InventoryCanvas;
    private GameObject tiles;
    private Inventory inventoryScript;
    private GameObject aStar;
    private ItemBehaviours itemBehavioursScript;
    private List<InventoryItem> inventoryList;
    private MouseCollisionOnItems mouseCollisionScript;
    private GameObject player;
    private Sprite ClearSprite;
    private List<GameObject> deskSlots = new List<GameObject>();
    private List<GameObject> invSlots = new List<GameObject>();
    public int invSlotNumber;
    public int deskSlotNumber;
    private float distance;
    public bool deskIsOpen;
    private GameObject timeObject;
    private bool deskIsFull;
    private bool invIsFull;
    private GameObject desk;
    private string deskText;
    private bool isOpening;
    private PauseController pauseController;
    public List<DeskItem> deskInv;
    public void Start()
    {
        //get vars
        MenuCanvas = RootObjectCache.GetRoot("MenuCanvas");
        InventoryCanvas = RootObjectCache.GetRoot("InventoryCanvas");
        tiles = RootObjectCache.GetRoot("Tiles");
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        aStar = RootObjectCache.GetRoot("A*");
        itemBehavioursScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemBehaviours>();
        mouseCollisionScript = InventoryCanvas.transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        ClearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        timeObject = InventoryCanvas.transform.Find("Time").gameObject;
        pauseController = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();

        //make slot list
        foreach (Transform child in transform)
        {
            deskSlots.Add(child.gameObject);
        }
        foreach(Transform child in InventoryCanvas.transform.Find("GUIPanel"))
        {
            invSlots.Add(child.gameObject);
        }
        inventoryList = inventoryScript.inventory;
        //disable desk
        CloseDesk();
    }
    public void Update()
    {
        if (!deskIsOpen)
        {
            if (mouseCollisionScript.isTouchingDesk && Input.GetMouseButtonDown(0))
            {
                distance = Vector2.Distance(player.transform.position, mouseCollisionScript.touchedDesk.transform.position);
                if(distance <= 2.4f)
                {
                    desk = mouseCollisionScript.touchedDesk;
                    deskText = GetDeskText(desk);
                    deskInv = desk.GetComponent<DeskData>().deskInv;
                    StartCoroutine(OpenDesk());
                }
            }
        }

        if (deskIsOpen)
        {
            ///continue
            //putting items in the desk
            for (int i = 0; i <= 19; i++)
            {
                if (deskInv[i].itemData != null)
                {
                    deskIsFull = true;
                }
                else if (deskInv[i].itemData == null)
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
            if (!mouseCollisionScript.isTouchingInvSlot && !mouseCollisionScript.isTouchingDeskPanel && !mouseCollisionScript.isTouchingDeskSlot && !mouseCollisionScript.isTouchingExtra && !mouseCollisionScript.isTouchingDesk && Input.GetMouseButtonDown(0))
            {
                CloseDesk();
            }
        }
    }
    private string GetDeskText(GameObject aDesk)
    {
        if(aDesk.name == "PlayerDesk")
        {
            return "Your Desk";
        }
        else if(aDesk.name == "DevDesk")
        {
            return "Dev Desk";
        }
        else
        {
            int deskNumber = aDesk.GetComponent<DeskData>().inmateCorrelationNumber;
            foreach(Transform npc in aStar.transform)
            {
                if (npc.name.StartsWith("Inmate"))
                {
                    int inmateNumber = Convert.ToInt32(npc.name.Replace("Inmate", ""));
                    if (inmateNumber == deskNumber)
                    {
                        return npc.GetComponent<NPCCollectionData>().npcData.displayName + "'s Desk";
                    }
                }
            }
            return "Desk";

        }
    }
    private void CloseDesk()
    {
        foreach(GameObject slot in deskSlots)
        {
            slot.GetComponent<Image>().sprite = ClearSprite;
        }
        
        MenuCanvas.transform.Find("DeskMenuPanel").gameObject.SetActive(false);
        MenuCanvas.transform.Find("DeskMenuBackdrop").GetComponent<Image>().enabled = false;
        MenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = false;
        MenuCanvas.transform.Find("DeskMenuText").GetComponent<TextMeshProUGUI>().text = null;

        deskIsOpen = false;

        pauseController.Unpause();
    }
    public IEnumerator OpenDesk()
    {
        if (isOpening)
        {
            yield break;
        }
        
        if (desk.name.StartsWith("Desk") && !itemBehavioursScript.barIsMoving) //just checks if its a npc desk and not yours or the dev one
        {
            isOpening = true;

            Vector3 oldDeskPos = new Vector3(desk.transform.position.x, desk.transform.position.y);
            
            StartCoroutine(itemBehavioursScript.DrawActionBar(false, true));
            itemBehavioursScript.CreateActionText("Opening");
            yield return new WaitForSeconds(.045f);
            if (itemBehavioursScript.cancelBar || oldDeskPos != desk.transform.position) { isOpening = false; yield break; }
            for (int i = 1; i <= 49; i++)
            {
                if (itemBehavioursScript.cancelBar || oldDeskPos != desk.transform.position) { isOpening = false; yield break; }
                yield return new WaitForSeconds(.045f);
            }
        }
        isOpening = false;

        for(int i = 0; i < 20; i++)
        {
            if (deskInv[i].itemData != null)
            {
                deskSlots[i].GetComponent<Image>().sprite = deskInv[i].itemData.icon;
            }
        }

        MenuCanvas.transform.Find("DeskMenuPanel").gameObject.SetActive(true);
        MenuCanvas.transform.Find("DeskMenuText").GetComponent<TextMeshProUGUI>().text = deskText;
        MenuCanvas.transform.Find("DeskMenuBackdrop").GetComponent<Image>().enabled = true;
        MenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = true;

        pauseController.Pause(false);

        yield return new WaitForEndOfFrame();

        deskIsOpen = true;
    }
}
