using System.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class LadderClimb : MonoBehaviour
{
    public MouseCollisionOnItems mcs;
    public Canvas ic;
    public InventorySelection selectionScript;
    public DeskStand deskStandScript;
    public GameObject player;
    public GameObject perksTiles;
    private GameObject currentLadder;
    public bool hasPickedUp;
    private Vector3 offsetVector = new Vector3(); //only for ground-to-roof operations
    private float distance;
    public void Start()
    {
        offsetVector.y = 1.6f;
    }
    public void Update()
    {
        if (mcs.isTouchingGroundLadder || mcs.isTouchingVentLadder || mcs.isTouchingRoofLadder)
        {
            ic.transform.Find("MouseOverlay").GetComponent<RectTransform>().sizeDelta = new Vector2(45, 50);
            if (mcs.isTouchingGroundLadder)
            {
                distance = Vector2.Distance(player.transform.position, mcs.touchedGroundLadder.transform.position);
            }
            else if (mcs.isTouchingVentLadder)
            {
                distance = Vector2.Distance(player.transform.position, mcs.touchedVentLadder.transform.position);
            }
            else if (mcs.isTouchingRoofLadder)
            {
                distance = Vector2.Distance(player.transform.position, mcs.touchedRoofLadder.transform.position);
            }
            if(Input.GetMouseButtonDown(0) && distance <= 2.4f && player.layer != 15)
            {
                foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Desk"))
                {
                    if(obj.layer == 14)
                    {
                        hasPickedUp = true;
                        break;
                    }
                    else
                    {
                        hasPickedUp = false;
                    }
                }
                
                if (mcs.isTouchingGroundLadder)
                {
                    currentLadder = mcs.touchedGroundLadder;
                }
                else if (mcs.isTouchingVentLadder)
                {
                    currentLadder = mcs.touchedVentLadder;
                }
                else if (mcs.isTouchingRoofLadder)
                {
                    currentLadder = mcs.touchedRoofLadder;
                }

                if (!hasPickedUp)
                {
                    StartCoroutine(ClimbLadder());
                }
            }
        }
    }
    public IEnumerator ClimbLadder()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForEndOfFrame();

        player.transform.position = currentLadder.GetComponent<LadderConnect>().connectedLadder.transform.position;

        switch (currentLadder.GetComponent<LadderConnect>().connectedLadder.layer)
        {
            case 10:
                foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Desk"))
                {
                    obj.GetComponent<DeskPickUp>().enabled = true;
                }
                player.layer = 3;
                player.GetComponent<SpriteRenderer>().sortingOrder = 6;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
                perksTiles.transform.Find("Roof").gameObject.SetActive(false);
                perksTiles.transform.Find("RoofObjects").gameObject.SetActive(false);
                perksTiles.transform.Find("Vents").gameObject.SetActive(false);
                perksTiles.transform.Find("VentObjects").gameObject.SetActive(false);
                perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                foreach (Transform child in perksTiles.transform.Find("GroundObjects"))
                {
                    if (child.gameObject.CompareTag("Item"))
                    {
                        child.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
                EnableTags();
                break;
            case 12:
                player.layer = 12;
                player.GetComponent<SpriteRenderer>().sortingOrder = 11;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 12;
                perksTiles.transform.Find("Roof").gameObject.SetActive(false);
                perksTiles.transform.Find("RoofObjects").gameObject.SetActive(false);
                perksTiles.transform.Find("Vents").gameObject.SetActive(true);
                perksTiles.transform.Find("VentObjects").gameObject.SetActive(true);
                foreach (Transform child in perksTiles.transform.Find("GroundObjects"))
                {
                    if (child.gameObject.CompareTag("Item"))
                    {
                        child.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                VentEnable();
                DisableTags();
                break;
            case 13:
                player.layer = 13;
                player.GetComponent<SpriteRenderer>().sortingOrder = 15;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 16;
                perksTiles.transform.Find("Roof").gameObject.SetActive(true);
                perksTiles.transform.Find("RoofObjects").gameObject.SetActive(true);
                perksTiles.transform.Find("Vents").gameObject.SetActive(false);
                perksTiles.transform.Find("VentObjects").gameObject.SetActive(false);
                perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                foreach (Transform child in perksTiles.transform.Find("GroundObjects"))
                {
                    if (child.gameObject.CompareTag("Item"))
                    {
                        child.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                DisableTags();
                break;
        }
        player.transform.position = currentLadder.GetComponent<LadderConnect>().connectedLadder.transform.position;

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void DisableTags()
    {
        mcs.DisableTag("Bars");
        mcs.DisableTag("Fence");
        mcs.DisableTag("ElectricFence");
        mcs.DisableTag("Digable");
        mcs.DisableTag("Wall");
        mcs.DisableTag("Ladder(Ground)");
        mcs.DisableTag("Desk");//currently the only menu
    }
    public void EnableTags()
    {
        mcs.EnableTag("Bars");
        mcs.EnableTag("Fence");
        mcs.EnableTag("ElectricFence");
        mcs.EnableTag("Digable");
        mcs.EnableTag("Wall");
        mcs.EnableTag("Ladder(Ground)");
        mcs.EnableTag("Desk");//currently the only menu
    }
    public void VentEnable()
    {
        perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = true;
        Color color = perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().color;
        color.a = 235f / 256f;
        perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().color = color;

        SpriteRenderer[] ventSpriteRenderers = perksTiles.transform.Find("Vents").GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = perksTiles.transform.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in ventSpriteRenderers)
        {
            Color aColor = sr.color;
            aColor.a = 1;
            sr.color = aColor;
        }
        foreach (SpriteRenderer sr in ventObjectSpriteRenderers)
        {
            Color aColor = sr.color;
            aColor.a = 1;
            sr.color = aColor;
        }
    }
}