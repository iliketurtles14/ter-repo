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

        StartCoroutine(CloseMenu(false, false));
    }
    public void Update()
    {
        givingScript.currentNPC = currentNPC;
        shopMenuScript.currentNPC = currentNPC;

        if (!idIsOpen && !givingScript.menuIsOpen && !shopMenuScript.menuIsOpen)
        {
            currentNPC = null;
            if(mcs.isTouchingNPC && Input.GetMouseButtonDown(1) && !mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead)
            {
                PSoundController.PlaySound("open");
                currentNPC = mcs.touchedNPC;
                OpenMenu();
            }
        }
        if (idIsOpen)
        {
            if(!mcs.isTouchingStatBar && !mcs.isTouchingIDNPC && !mcs.isTouchingIDPanel && !mcs.isTouchingButton && !mcs.isTouchingInvSlot && !mcs.isTouchingExtra && !mcs.isTouchingIDSlot && Input.GetMouseButtonDown(0))
            {
                PSoundController.PlaySound("close");
                StartCoroutine(CloseMenu(false, false));
            }
        }
    }
    private void SetNPCSpecifics(GameObject npc)
    {
        NPCIDAnimation animScript = transform.Find("NPC").GetComponent<NPCIDAnimation>();
        animScript.character = npc.GetComponent<BodyController>().character;
        if (npc.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData == null)
        {
            animScript.outfit = "empty";
        }
        else
        {
            animScript.outfit = npc.GetComponent<OutfitController>().outfit;
        }
        animScript.currentNPC = npc;

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

        IDNPCName nameScript = mc.transform.Find("NPCMenuPanel").Find("NPC").GetComponent<IDNPCName>();
        nameScript.aName = currentNPC.GetComponent<NPCCollectionData>().npcData.displayName;
        nameScript.color = currentNPC.GetComponent<NPCNameColor>().currentColor;

        //stats
        Transform panel = mc.transform.Find("NPCMenuPanel");
        NPCData data = currentNPC.GetComponent<NPCCollectionData>().npcData;
        panel.Find("StrengthPanel").Find("StrengthBar").GetComponent<Image>().enabled = true;
        panel.Find("SpeedPanel").Find("SpeedBar").GetComponent<Image>().enabled = true;
        panel.Find("IntellectPanel").Find("IntellectBar").GetComponent<Image>().enabled = true;
        panel.Find("OpinionPanel").Find("OpinionBar").GetComponent<Image>().enabled = true;
        panel.Find("StrengthPanel").Find("StrengthBar").GetComponent<RectTransform>().sizeDelta = new Vector2(data.strength / 2 * 5, 25);    // holy magic numbers lmao
        panel.Find("StrengthPanel").Find("StrengthBar").GetComponent<RectTransform>().anchoredPosition = new Vector2(data.strength / 2 * 2.5f + 454.5f, -102);
        panel.Find("SpeedPanel").Find("SpeedBar").GetComponent<RectTransform>().sizeDelta = new Vector2(data.speed / 2 * 5, 25);
        panel.Find("SpeedPanel").Find("SpeedBar").GetComponent<RectTransform>().anchoredPosition = new Vector2(data.speed / 2 * 2.5f + 454.5f, -157.5f);
        panel.Find("IntellectPanel").Find("IntellectBar").GetComponent<RectTransform>().sizeDelta = new Vector2(data.intellect / 2 * 5, 25);
        panel.Find("IntellectPanel").Find("IntellectBar").GetComponent<RectTransform>().anchoredPosition = new Vector2(data.intellect / 2 * 2.5f + 454.5f, -212);
        panel.Find("OpinionPanel").Find("OpinionBar").GetComponent<RectTransform>().sizeDelta = new Vector2(data.opinion / 2 * 5, 25);
        panel.Find("OpinionPanel").Find("OpinionBar").GetComponent<RectTransform>().anchoredPosition = new Vector2(data.opinion / 2 * 2.5f + 454.5f, -267);
        panel.Find("StrengthPanel").Find("StrengthBar").GetComponent<BoxCollider2D>().size = new Vector2(data.strength / 2 * 5, 25);
        panel.Find("SpeedPanel").Find("SpeedBar").GetComponent<BoxCollider2D>().size = new Vector2(data.speed / 2 * 5, 25);
        panel.Find("IntellectPanel").Find("IntellectBar").GetComponent<BoxCollider2D>().size = new Vector2(data.intellect / 2 * 5, 25);
        panel.Find("OpinionPanel").Find("OpinionBar").GetComponent<BoxCollider2D>().size = new Vector2(data.opinion / 2 * 5, 25);
        panel.Find("StrengthPanel").Find("StrengthBar").GetComponent<StatBarHandler>().stat = data.strength;
        panel.Find("SpeedPanel").Find("SpeedBar").GetComponent<StatBarHandler>().stat = data.speed;
        panel.Find("IntellectPanel").Find("IntellectBar").GetComponent<StatBarHandler>().stat = data.intellect;
        panel.Find("OpinionPanel").Find("OpinionBar").GetComponent<StatBarHandler>().stat = data.opinion;
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
        transform.Find("StrengthPanel").Find("StrengthBar").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("SpeedPanel").Find("SpeedBar").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("IntellectPanel").Find("IntellectBar").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("OpinionPanel").Find("OpinionBar").GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("WeaponText").gameObject.SetActive(true);
        transform.Find("OutfitText").gameObject.SetActive(true);
        string job = currentNPC.GetComponent<NPCCollectionData>().npcData.job;
        if (string.IsNullOrEmpty(job))
        {
            transform.Find("JobText").GetComponent<TextMeshProUGUI>().text = "Unemployed";
        }
        else
        {
            transform.Find("JobText").GetComponent<TextMeshProUGUI>().text = job;
        }
        transform.Find("JobText").gameObject.SetActive(true);
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
        transform.Find("JobText").gameObject.SetActive(false);
        transform.Find("NPC").Find("Outfit").GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().enabled = false;
        yield return new WaitForEndOfFrame();
        idIsOpen = false;
    }
}
