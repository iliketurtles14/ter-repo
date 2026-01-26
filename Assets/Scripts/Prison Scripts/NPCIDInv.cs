using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCIDInv : MonoBehaviour
{
    private GameObject mc;
    private MouseCollisionOnItems mcs;
    public GameObject outfitSlot;
    public GameObject weaponSlot;
    public int invSlotNumber;
    public bool idIsOpen;
    private PauseController pc;

    public void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas");
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();

        StartCoroutine(Wait());
        CloseMenu();
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        CloseMenu();
    }
    public void Update()
    {
        if (!idIsOpen)
        {
            if(mcs.isTouchingNPC && Input.GetMouseButtonDown(1))
            {
                StartCoroutine(OpenMenu(mcs.touchedNPC));
            }
        }
        if (idIsOpen)
        {
            if(!mcs.isTouchingIDPanel && !mcs.isTouchingButton && !mcs.isTouchingInvSlot && !mcs.isTouchingExtra && !mcs.isTouchingIDSlot && Input.GetMouseButtonDown(0))
            {
                CloseMenu();
                pc.Unpause();
            }
        }
    }
    private void SetNPCSpecifics(GameObject npc)
    {
        string npcName = npc.GetComponent<NPCCollectionData>().npcData.displayName;
        transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = npcName;

        transform.Find("NPC").GetComponent<NPCIDAnimation>().bodyDirSprites = npc.GetComponent<NPCAnimation>().bodyDirSprites;
        transform.Find("NPC").GetComponent<NPCIDAnimation>().outfitDirSprites = npc.GetComponent<NPCAnimation>().outfitDirSprites;
    }
    public IEnumerator OpenMenu(GameObject npc)
    {
        SetNPCSpecifics(npc);
        
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

        pc.Pause(true);

        yield return new WaitForEndOfFrame();

        idIsOpen = true;
    }
    public void CloseMenu()
    {
        foreach(Transform child in transform)
        {
            if(child.GetComponent<Image>() != null)
            {
                child.GetComponent<Image>().enabled = false;
            }
            if(child.GetComponent<BoxCollider>() != null)
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
        mc.transform.Find("Black").GetComponent<Image>().enabled = false;
        idIsOpen = false;
    }
}
