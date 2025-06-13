using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;

public class DeskInv : MonoBehaviour
{
    public Canvas MenuCanvas;
    public Canvas InventoryCanvas;
    public GameObject perksTiles;
    public Inventory inventoryScript;
    public GameObject aStar;
    public ItemBehaviours itemBehavioursScript;
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
    public GameObject desk;
    public string deskText;
    private bool isOpening;
    public PauseController pauseController;
    public void Start()
    {
        //what desk to take
        if(name == "PlayerDeskMenuPanel")
        {
            desk = perksTiles.transform.Find("GroundObjects").Find("PlayerDesk").gameObject;
        }
        else if (name.StartsWith("DeskMenuPanel"))
        {
            int num = 0;
            Match match = Regex.Match(name, @"\d+");
            if (match.Success)
            {
                num = int.Parse(match.Value);
            }

            foreach(Transform child in perksTiles.transform.Find("GroundObjects"))
            {
                if(child.name == "Desk" + num.ToString())
                {
                    desk = child.gameObject;
                    break;
                }
            }
        }
        else if(name == "DevDeskMenuPanel")
        {
            desk = perksTiles.transform.Find("GroundObjects").Find("DevDesk").gameObject;
        }

        StartCoroutine(StartWait());

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
        foreach(GameObject slot in deskSlots)
        {
            slot.GetComponent<BoxCollider2D>().enabled = false;
            slot.GetComponent<Image>().enabled = false;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().enabled = false;

        MenuCanvas.transform.Find("DeskMenuBackdrop").GetComponent<Image>().enabled = false;
        MenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = false;
        MenuCanvas.transform.Find("DeskMenuText").GetComponent<TextMeshProUGUI>().text = null;
    }
    public IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();

        //what text to take
        if (name == "PlayerDeskMenuPanel")
        {
            deskText = "Your Desk";
        }
        else if (name.StartsWith("DeskMenuPanel"))
        {
            int num = 0;
            Match match = Regex.Match(name, @"\d+");
            if (match.Success)
            {
                num = int.Parse(match.Value);
            }
            foreach (Transform child in aStar.transform)
            {
                if (child.name == "Inmate" + num.ToString())
                {
                    deskText = (child.GetComponent<NPCCollectionData>().npcData.displayName.ToString() + "'s Desk").Replace("\r\n", "").Replace("\n", "").Replace("\r", ""); ;
                    break;
                }
            }
        }
        else if(name == "DevDeskMenuPanel")
        {
            deskText = "Dev Desk";
        }
    }
    public void Update()
    {
        
        if (!deskIsOpen)
        {
            if (mouseCollisionScript.isTouchingDesk && mouseCollisionScript.touchedDesk.name == desk.name)
            {
                distance = Vector2.Distance(player.transform.position, mouseCollisionScript.touchedDesk.transform.position);
            }
            if (mouseCollisionScript.isTouchingDesk && mouseCollisionScript.touchedDesk.name == desk.name && Input.GetMouseButtonDown(0) && distance <= 2.4f)
            {
                StartCoroutine(OpenDesk());
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
                foreach (GameObject slot in deskSlots)
                {
                    slot.GetComponent<BoxCollider2D>().enabled = false;
                    slot.GetComponent<Image>().enabled = false;
                }
                
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Image>().enabled = false;

                MenuCanvas.transform.Find("DeskMenuBackdrop").GetComponent<Image>().enabled = false;
                MenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = false;
                MenuCanvas.transform.Find("DeskMenuText").GetComponent<TextMeshProUGUI>().text = null;

                deskIsOpen = false;

                pauseController.Unpause();

                return;
            }
        }
    }
    public IEnumerator OpenDesk()
    {
        if (isOpening)
        {
            yield break;
        }
        
        if (desk.name.StartsWith("Desk") && !itemBehavioursScript.barIsMoving)
        {
            isOpening = true;

            Vector3 oldDeskPos = new Vector3(desk.transform.position.x, desk.transform.position.y, desk.gameObject.transform.position.z);
            
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

        foreach (GameObject slot in deskSlots)
        {
            slot.GetComponent<BoxCollider2D>().enabled = true;
            slot.GetComponent<Image>().enabled = true;
        }
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Image>().enabled = true;
        MenuCanvas.transform.Find("DeskMenuText").GetComponent<TextMeshProUGUI>().text = deskText;


        MenuCanvas.transform.Find("DeskMenuBackdrop").GetComponent<Image>().enabled = true;
        MenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = true;

        pauseController.Pause(false);

        yield return new WaitForEndOfFrame();

        deskIsOpen = true;
    }
}
