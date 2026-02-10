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
    private float speed = 4f;
    private float amplitude = .15f;
    private float baseNum = 1.1f;
    private GameObject MenuCanvas;
    private GameObject InventoryCanvas;
    private GameObject tiles;
    private Inventory inventoryScript;
    private HPAChecker HPAScript;
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
    public bool deskIsFull;
    public bool invIsFull;
    private GameObject desk;
    private string deskText;
    public bool isOpening;
    private PauseController pauseController;
    public List<DeskItem> deskInv;
    public GameObject currentDesk;
    private List<GameObject> currentAnimatingSlots = new List<GameObject>();
    private SpecialMessages specialMessagesScript;
    private MissionAsk missionAskScript;
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
        HPAScript = player.GetComponent<HPAChecker>();
        specialMessagesScript = InventoryCanvas.transform.Find("SpecialMessagePanel").GetComponent<SpecialMessages>();
        missionAskScript = MenuCanvas.transform.Find("MissionPanel").GetComponent<MissionAsk>();

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
        
        HPAScript.isSearching = isOpening;
        
        if (!deskIsOpen)
        {
            currentDesk = null;
            if (mouseCollisionScript.isTouchingDesk && Input.GetMouseButtonDown(0))
            {
                desk = mouseCollisionScript.touchedDesk;
                distance = Vector2.Distance(player.transform.position, desk.transform.position);
                if(distance <= 2.4f)
                {
                    deskText = GetDeskText(desk);
                    deskInv = desk.GetComponent<DeskData>().deskInv;
                    currentDesk = desk;
                    StartCoroutine(OpenDesk());
                }
            }
        }

        if (deskIsOpen)
        {
            ///continue
            //putting items in the desk
            deskIsFull = true;
            for (int i = 0; i < deskInv.Count; i++)
            {
                try
                {
                    if (!deskInv[i].itemData.forFavor)//this messes with sprite stuff
                    {
                        if (deskInv[i].itemData.sprite == null)
                        {
                            deskIsFull = false;
                            break;
                        }
                    }
                }
                catch //if it fails, that means the itemData is null and therefore the desk is not full123123123123
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
                        deskSlots[i].GetComponent<Image>().sprite = inventoryList[invSlotNumber].itemData.sprite;
                        break;
                    }
                }
                inventoryList[invSlotNumber].itemData = null;
                mouseCollisionScript.touchedInvSlot.GetComponent<Image>().sprite = ClearSprite;
            }
            
            //putting items in the inv
            for (int i = 0; i < 6; i++)
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
                if (!deskInv[deskSlotNumber].itemData.forFavor)
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
                            slot.GetComponent<Image>().sprite = deskInv[deskSlotNumber].itemData.sprite;
                            break;
                        }
                    }
                }
                else
                {
                    int id = deskInv[deskSlotNumber].itemData.id;
                    string npcName = "";
                    foreach (Transform npc in aStar.transform)
                    {
                        if (npc.name.Contains("Inmate"))
                        {
                            if(npc.GetComponent<NPCCollectionData>().npcData.desk == desk)
                            {
                                npcName = npc.GetComponent<NPCCollectionData>().npcData.displayName;
                                Debug.Log(npcName);
                                break;
                            }
                        }
                    }
                    int cost = 0;

                    foreach(Mission mission in missionAskScript.savedMissions)
                    {
                        if(mission.target == npcName && mission.item == id)
                        {
                            Debug.Log("here");
                            cost = mission.pay;
                            missionAskScript.savedMissions.Remove(mission);
                            break;
                        }
                    }

                    StartCoroutine(specialMessagesScript.MakeMessage("You completed a Favor!\n+$" + cost, "favor"));
                    player.GetComponent<PlayerCollectionData>().playerData.money += cost;
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

        for (int i = 0; i < 20; i++)
        {
            if (deskIsOpen && deskInv[i].itemData != null && deskInv[i].itemData.forFavor && !currentAnimatingSlots.Contains(deskSlots[i]))
            {
                deskSlots[i].GetComponent<Image>().sprite = AddPaddingToSprite(deskSlots[i].GetComponent<Image>().sprite, 1);
                deskSlots[i].GetComponent<Image>().material = new Material(Resources.Load<Material>("PrisonResources/PulseMaterial"));
                currentAnimatingSlots.Add(deskSlots[i]);
                StartCoroutine(AnimateSlot(deskSlots[i]));
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
                    int inmateNumber = npc.GetComponent<NPCCollectionData>().npcData.order;
                    if (inmateNumber == deskNumber)
                    {
                        return npc.GetComponent<NPCCollectionData>().npcData.displayName.Replace("\r\n", "").Replace("\n", "").Replace("\r", "") + "'s Desk";
                    }
                }
            }
            return "Desk";

        }
    }
    private void CloseDesk()
    {
        StopAllCoroutines();
        currentAnimatingSlots = new List<GameObject>();

        foreach(GameObject slot in deskSlots)
        {
            slot.GetComponent<Image>().sprite = ClearSprite;
            slot.GetComponent<Image>().material = null;
            slot.GetComponent<BoxCollider2D>().enabled = false;
        }

        MenuCanvas.transform.Find("DeskMenuPanel").GetComponent<Image>().enabled = false;
        MenuCanvas.transform.Find("DeskMenuPanel").GetComponent<BoxCollider2D>().enabled = false;
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
        
        if (desk.name.StartsWith("NPCDesk") && !itemBehavioursScript.barIsMoving) //just checks if its a npc desk and not yours or the dev one
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
                deskSlots[i].GetComponent<Image>().sprite = deskInv[i].itemData.sprite;
            }
        }

        foreach(GameObject slot in deskSlots)
        {
            slot.GetComponent<BoxCollider2D>().enabled = true;
        }

        MenuCanvas.transform.Find("DeskMenuPanel").GetComponent<Image>().enabled = true;
        MenuCanvas.transform.Find("DeskMenuPanel").GetComponent<BoxCollider2D>().enabled = true;
        MenuCanvas.transform.Find("DeskMenuText").GetComponent<TextMeshProUGUI>().text = deskText;
        MenuCanvas.transform.Find("DeskMenuBackdrop").GetComponent<Image>().enabled = true;
        MenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = true;

        pauseController.Pause(false);

        yield return new WaitForEndOfFrame();

        deskIsOpen = true;
    }

    private IEnumerator AnimateSlot(GameObject slot)
    {
        float pulse = 1;
        while (deskIsOpen)
        {
            pulse = baseNum + Mathf.Sin(Time.time * speed) * amplitude;
            
            slot.GetComponent<Image>().material.SetFloat("_PulseScale", pulse);
            yield return null;
        }
    }
    public static Sprite AddPaddingToSprite(Sprite originalSprite, int padding)
    {
        // Get original texture and rect
        Texture2D originalTexture = originalSprite.texture;
        Rect spriteRect = originalSprite.rect;

        int originalWidth = (int)spriteRect.width;
        int originalHeight = (int)spriteRect.height;

        // Copy only sprite area (in case of atlas)
        Texture2D spriteTexture = new Texture2D(originalWidth, originalHeight, TextureFormat.RGBA32, false);
        spriteTexture.filterMode = originalTexture.filterMode; // match original
        spriteTexture.wrapMode = TextureWrapMode.Clamp;

        Color[] pixels = originalTexture.GetPixels(
            (int)spriteRect.x,
            (int)spriteRect.y,
            originalWidth,
            originalHeight
        );
        spriteTexture.SetPixels(0, 0, originalWidth, originalHeight, pixels);
        spriteTexture.Apply();

        // Create padded texture
        int newWidth = originalWidth + padding * 2;
        int newHeight = originalHeight + padding * 2;

        Texture2D paddedTexture = new Texture2D(newWidth, newHeight, originalTexture.format, false);

        // FIX: No blur
        paddedTexture.filterMode = originalTexture.filterMode; // or FilterMode.Point for pixel art
        paddedTexture.wrapMode = TextureWrapMode.Clamp;

        // Fill with transparent
        Color32[] clearPixels = new Color32[newWidth * newHeight];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = new Color32(0, 0, 0, 0);
        }
        paddedTexture.SetPixels32(clearPixels);

        // Copy original into center
        Color32[] originalPixels = spriteTexture.GetPixels32();
        for (int y = 0; y < originalHeight; y++)
        {
            for (int x = 0; x < originalWidth; x++)
            {
                Color32 pixel = originalPixels[y * originalWidth + x];
                paddedTexture.SetPixel(x + padding, y + padding, pixel);
            }
        }

        paddedTexture.Apply();

        // Create new sprite from padded texture
        Sprite paddedSprite = Sprite.Create(
            paddedTexture,
            new Rect(0, 0, newWidth, newHeight),
            new Vector2(0.5f, 0.5f), // pivot
            originalSprite.pixelsPerUnit
        );

        return paddedSprite;
    }
}
