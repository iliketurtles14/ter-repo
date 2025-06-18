using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCIDInv : MonoBehaviour
{
    public GameObject mc;
    public GameObject ic;
    public Inventory inventoryScript;
    public MouseCollisionOnItems mcs;
    public List<IDItem> idInv = new List<IDItem>();
    public GameObject player;
    public Sprite ClearSprite;
    public GameObject outfitSlot;
    public GameObject weaponSlot;
    public int invSlotNumber;
    public bool idIsOpen;
    public GameObject timeObject;
    public PauseController pc;

    public void Start()
    {
        StartCoroutine(Wait());
        CloseMenu();
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = NPCSave.instance.playerName;
    }
    public void Update()
    {
        if (!idIsOpen)
        {
            if(mcs.isTouchingNPC && Input.GetMouseButtonDown(1))
            {
                StartCoroutine(OpenMenu());
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
    public IEnumerator OpenMenu()
    {
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
        idIsOpen = false;
    }
}
