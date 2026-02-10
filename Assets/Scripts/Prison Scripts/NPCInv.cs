using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInv : MonoBehaviour
{
    private Transform aStar;
    public GameObject npc;
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
    public List<NPCInvItem> npcInv;
    private bool npcInvIsFull;
    private Sprite clearSprite;
    private bool invIsFull;
    private int npcInvSlotNumber;
    private PauseController pauseController;
    private NPCInvItem weapon;
    private NPCInvItem outfit;
    private Transform npcInvMenu;
    private MissionAsk missionAskScript;
    private SpecialMessages specialMessagesScript;
    private List<GameObject> currentAnimatingSlots = new List<GameObject>();
    public void Start()
    {
        aStar = RootObjectCache.GetRoot("A*").transform;
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        mcs = ic.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        clearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        pauseController = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        npcInvMenu = mc.Find("NPCInvMenu");
        missionAskScript = mc.Find("MissionPanel").GetComponent<MissionAsk>();
        specialMessagesScript = ic.Find("SpecialMessagePanel").GetComponent<SpecialMessages>();

        //make slot list
        foreach (Transform child in npcInvMenu.Find("ItemPanel"))
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
        for(int i = 0; i < 8; i++)
        {
            try
            {
                if (npcInv[i].itemData.sprite == null)
                {
                    npcInv[i] = new NPCInvItem();
                }
            }
            catch { }
        }
        
        if (!menuIsOpen)
        {
            npc = null;
            if(mcs.isTouchingNPC && mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead)
            {
                npc = mcs.touchedNPC;
                float distance = Vector2.Distance(player.transform.position, npc.transform.position);
                if(distance <= 2.4f && Input.GetMouseButtonDown(0))
                {
                    npc = mcs.touchedNPC;
                    menuText = npc.GetComponent<NPCCollectionData>().npcData.displayName + "'s Pockets";
                    npcInv = npc.GetComponent<NPCCollectionData>().npcData.inventory;
                    weapon = npcInv[6];
                    outfit = npcInv[7];
                    StartCoroutine(OpenNPCInv());
                }
            }
        }
        if (menuIsOpen)
        {
            //loading items
            for(int i = 0; i < 6; i++)
            {
                Sprite itemSprite;
                try
                {
                    itemSprite = npcInv[i].itemData.sprite;
                }
                catch
                {
                    itemSprite = clearSprite;
                }
                if(itemSprite == null)
                {
                    itemSprite = clearSprite;
                }

                npcInvSlots[i].GetComponent<Image>().sprite = itemSprite;
            }
            Sprite weaponSprite;
            Sprite outfitSprite;

            try
            {
                weaponSprite = npc.GetComponent<NPCCollectionData>().npcData.inventory[6].itemData.sprite;
            }
            catch
            {
                weaponSprite = clearSprite;
            }
            try
            {
                outfitSprite = npc.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData.sprite;
            }
            catch
            {
                outfitSprite = clearSprite;
            }

            if (weaponSprite == null)
            {
                weaponSprite = clearSprite;
            }
            if (outfitSprite == null)
            {
                outfitSprite = clearSprite;
            }

            transform.Find("Weapon").GetComponent<Image>().sprite = weaponSprite;
            transform.Find("Outfit").GetComponent<Image>().sprite = outfitSprite;

            //putting items in menu
            for (int i = 0; i <= 5; i++)
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
                        npcInvSlots[i].GetComponent<Image>().sprite = inventoryList[invSlotNumber].itemData.sprite;
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
                if (!npcInv[npcInvSlotNumber].itemData.forFavor)
                {
                    foreach (InventoryItem slot in inventoryList)
                    {
                        if (slot.itemData == null)
                        {
                            slot.itemData = npcInv[npcInvSlotNumber].itemData;
                            break;
                        }
                    }
                    foreach (GameObject slot in invSlots)
                    {
                        if (slot.GetComponent<Image>().sprite == clearSprite)
                        {
                            slot.GetComponent<Image>().sprite = npcInv[npcInvSlotNumber].itemData.sprite;
                            break;
                        }
                    }
                }
                else
                {
                    int id = npcInv[npcInvSlotNumber].itemData.id;
                    string npcName = npc.GetComponent<NPCCollectionData>().npcData.displayName;
                    int cost = 0;

                    foreach(Mission mission in missionAskScript.savedMissions)
                    {
                        if(mission.target == npcName && mission.item == id)
                        {
                            cost = mission.pay;
                            missionAskScript.savedMissions.Remove(mission);
                            break;
                        }
                    }

                    StartCoroutine(specialMessagesScript.MakeMessage("You completed a Favor!\n+$" + cost, "favor"));
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
                        break;
                    }
                }
                foreach(GameObject slot in invSlots)
                {
                    if(slot.GetComponent<Image>().sprite == clearSprite)
                    {
                        slot.GetComponent<Image>().sprite = item.itemData.sprite;
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

        for(int i = 0; i < 6; i++)
        {
            if(menuIsOpen && npcInv[i].itemData != null && npcInv[i].itemData.forFavor && !currentAnimatingSlots.Contains(npcInvSlots[i]))
            {
                npcInvSlots[i].GetComponent<Image>().sprite = AddPaddingToSprite(npcInvSlots[i].GetComponent<Image>().sprite, 1);
                npcInvSlots[i].GetComponent<Image>().material = new Material(Resources.Load<Material>("PrisonResources/PulseMaterial"));
                currentAnimatingSlots.Add(npcInvSlots[i]);
                StartCoroutine(AnimateSlot(npcInvSlots[i]));
            }
        }
    }
    private IEnumerator AnimateSlot(GameObject slot)
    {
        float pulse = 1f;
        float baseNum = 1.1f;
        float speed = 4f;
        float amplitude = .15f;
        while (menuIsOpen)
        {
            pulse = baseNum + Mathf.Sin(Time.time * speed) * amplitude;

            slot.GetComponent<Image>().material.SetFloat("_PulseScale", pulse);
            yield return null;
        }
    }
    public IEnumerator OpenNPCInv()
    {
        for(int i = 0; i < 6; i++)
        {
            if (npcInv[i].itemData != null)
            {
                npcInvSlots[i].GetComponent<Image>().sprite = npcInv[i].itemData.sprite;
            }
        }
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
        }
        foreach(Transform child in transform.Find("ItemPanel"))
        {
            if (child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = true;
            }
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        transform.Find("Outfit").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("Weapon").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("Outfit").GetComponent<Image>().enabled = true;
        transform.Find("Weapon").GetComponent<Image>().enabled = true;
        transform.Find("NameText").gameObject.SetActive(true);
        transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = menuText.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
        mc.Find("Black").GetComponent<Image>().enabled = true;
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;

        pauseController.Pause(false);

        yield return new WaitForEndOfFrame();

        menuIsOpen = true;
    }
    public void CloseNPCInv()
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
        foreach (Transform child in transform.Find("ItemPanel"))
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
        transform.Find("Outfit").GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Weapon").GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Outfit").GetComponent<Image>().enabled = false;
        transform.Find("Weapon").GetComponent<Image>().enabled = false;
        transform.Find("NameText").gameObject.SetActive(false);
        mc.Find("Black").GetComponent<Image>().enabled = false;
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        menuIsOpen = false;

        pauseController.Unpause();
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
