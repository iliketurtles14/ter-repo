using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCIDInv : MonoBehaviour
{
    private GameObject mc;
    private MouseCollisionOnItems mcs;
    //public GameObject outfitSlot;
    //public GameObject weaponSlot;
    public int invSlotNumber;
    private bool idIsOpen;
    private PauseController pc;
    private Sprite clearSprite;
    public GameObject currentNPC;
    private Giving givingScript;
    private ShopMenu shopMenuScript;

    public void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas");
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        clearSprite = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        givingScript = mc.transform.Find("NPCGiveMenuPanel").GetComponent<Giving>();
        shopMenuScript = mc.transform.Find("NPCShopMenuPanel").GetComponent<ShopMenu>();

        StartCoroutine(Wait());
        StartCoroutine(CloseMenu(false, false));
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }
    public void Update()
    {
        givingScript.currentNPC = currentNPC;
        shopMenuScript.currentNPC = currentNPC;

        if (!idIsOpen && !givingScript.menuIsOpen && !shopMenuScript.menuIsOpen)
        {
            currentNPC = null;
            if(mcs.isTouchingNPC && Input.GetMouseButtonDown(1))
            {
                currentNPC = mcs.touchedNPC;
                OpenMenu();
            }
        }
        if (idIsOpen)
        {
            if(!mcs.isTouchingIDPanel && !mcs.isTouchingButton && !mcs.isTouchingInvSlot && !mcs.isTouchingExtra && !mcs.isTouchingIDSlot && Input.GetMouseButtonDown(0))
            {
                StartCoroutine(CloseMenu(false, false));
            }
        }
    }
    private void SetNPCSpecifics(GameObject npc)
    {
        string npcName = npc.GetComponent<NPCCollectionData>().npcData.displayName;
        transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = npcName;

        transform.Find("NPC").GetComponent<NPCIDAnimation>().bodyDirSprites = new List<Sprite>(npc.GetComponent<NPCAnimation>().bodyDirSprites);
        transform.Find("NPC").GetComponent<NPCIDAnimation>().outfitDirSprites = new List<Sprite>(npc.GetComponent<NPCAnimation>().outfitDirSprites);

        transform.Find("NPC").GetComponent<NPCIDAnimation>().currentNPC = npc;

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

        if(weaponSprite == null)
        {
            weaponSprite = clearSprite;
        }
        if(outfitSprite == null)
        {
            outfitSprite = clearSprite;
        }

        transform.Find("Weapon").GetComponent<Image>().sprite = weaponSprite;
        transform.Find("Outfit").GetComponent<Image>().sprite = outfitSprite;
    }
    public void OpenMenu()
    {
        SetNPCSpecifics(currentNPC);

        idIsOpen = true;

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
        transform.Find("NPC").Find("Outfit").GetComponent<Image>().enabled = true;
        mc.transform.Find("Black").GetComponent<Image>().enabled = true;
        pc.Pause(false);
    }
    public IEnumerator CloseMenu(bool goToGive, bool goToShop)
    {
        if (goToGive)
        {
            givingScript.OpenMenu();
        }
        else if (goToShop)
        {
            shopMenuScript.OpenMenu();
        }
        else
        {
            mc.transform.Find("Black").GetComponent<Image>().enabled = false;
            pc.Unpause();
        }
        
        foreach(Transform child in transform)
        {
            if(child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = false;
            }
            if(child.GetComponent<BoxCollider2D>() != null)
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
        foreach (Transform child in transform.Find("OpinionPanel"))
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
        transform.Find("NPC").Find("Outfit").GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().enabled = false;
        yield return new WaitForEndOfFrame();
        idIsOpen = false;
    }
}
