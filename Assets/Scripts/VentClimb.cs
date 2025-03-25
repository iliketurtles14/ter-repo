using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VentClimb : MonoBehaviour
{
    public DeskStand deskStandScript;
    public MouseCollisionOnItems mcs;
    public Canvas ic;
    public InventorySelection selectionScript;
    public GameObject player;
    public GameObject perksTiles;
    private GameObject currentOpenVent;
    private Vector3 offsetVector = new Vector3();
    private Vector2 colliderOffset = new Vector2();
    private Vector3 playerOffset = new Vector3();
    private bool cameFromDesk;
    private bool deskIsUnder;
    private GameObject deskUnder;
    private bool hasDisabledTags;
    public void Start()
    {
        offsetVector.y = 1.6f;

        colliderOffset.y = -.5f;
        colliderOffset.x = 0;
        playerOffset.y = .5f;
        playerOffset.x = 0;
        playerOffset.z = 0;

        cameFromDesk = false;
    }
    public void Update()
    {
        if (deskStandScript.hasClimbed)//when the player is on a desk
        {
            if (mcs.isTouchingOpenVent)
            {
                float distance = Vector2.Distance(player.transform.position, mcs.touchedOpenVent.transform.position);
                if (Input.GetMouseButtonDown(0) && distance <= 2.4f && !deskStandScript.isClimbing && deskStandScript.currentDesk.transform.position + offsetVector == mcs.touchedOpenVent.transform.position)
                {
                    currentOpenVent = mcs.touchedOpenVent;
                    cameFromDesk = true;
                    StartCoroutine(ClimbVentUp());
                }
            }
        }
        else if (player.layer == 12) //when the player is in a vent
        {
            if (mcs.isTouchingOpenVent)
            {                
                float distance = Vector2.Distance(player.transform.position, mcs.touchedOpenVent.transform.position);
                if (Input.GetMouseButtonDown(0) && distance <= 2.4f)
                {
                    currentOpenVent = mcs.touchedOpenVent;
                    StartCoroutine(ClimbVentDown());
                }
            }
        }

        if (cameFromDesk)//reset desk stuff
        {
            cameFromDesk = false;

            deskStandScript.hasClimbed = false;
            deskStandScript.hasJumped = false;
            
            player.GetComponent<PolygonCollider2D>().offset -= colliderOffset;
            player.GetComponent<Transform>().position -= playerOffset;

            deskStandScript.currentDesk.GetComponent<BoxCollider2D>().isTrigger = false;
        }

        if (player.layer == 12 && !hasDisabledTags)//no collision on walls, fences, etc (and desks and other menus (hopefully i remember that later into development))
        {
            mcs.DisableTag("Bars");
            mcs.DisableTag("Fence");
            mcs.DisableTag("ElectricFence");
            mcs.DisableTag("Digable");
            mcs.DisableTag("Wall");
            mcs.DisableTag("Ladder(Ground)");
            mcs.DisableTag("Desk");//currently the only menu
            foreach(Transform child in perksTiles.transform.Find("GroundObjects"))
            {
                if (child.CompareTag("Item"))
                {
                    child.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            hasDisabledTags = true;
        }
        else if(player.layer != 12 && player.layer != 13 && player.layer != 11 && hasDisabledTags)//renable tags when out of vent
        {
            mcs.EnableTag("Bars");
            mcs.EnableTag("Fence");
            mcs.EnableTag("ElectricFence");
            mcs.EnableTag("Digable");
            mcs.EnableTag("Wall");
            mcs.EnableTag("Ladder(Ground)");
            mcs.EnableTag("Desk");//currently the only menu
            foreach (Transform child in perksTiles.transform.Find("GroundObjects"))
            {
                if (child.CompareTag("Item"))
                {
                    child.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
            hasDisabledTags = false;
        }
    }
    public IEnumerator ClimbVentUp()
    {
        yield return new WaitForEndOfFrame();


        deskStandScript.hasClimbed = false;

        player.transform.position = currentOpenVent.transform.position;

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

        player.layer = 12;

        player.GetComponent<SpriteRenderer>().sortingOrder = 11;
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 12;


    }
    public IEnumerator ClimbVentDown()
    {
        yield return new WaitForEndOfFrame();

        foreach(GameObject desk in GameObject.FindGameObjectsWithTag("Desk"))
        {
            if(desk.transform.position + offsetVector == currentOpenVent.transform.position)
            {
                deskIsUnder = true;
                deskUnder = desk;
                break;
            }
            else
            {
                deskIsUnder = false;
            }
        }

        if (deskIsUnder)//pulled from the deskstand script
        {
            Debug.Log("Here");
            deskStandScript.shouldStepOff = false;
            yield return new WaitForEndOfFrame();
            player.GetComponent<PlayerCtrl>().enabled = false;
            deskUnder.GetComponent<BoxCollider2D>().isTrigger = true;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            deskUnder.GetComponent<DeskPickUp>().enabled = false;
            deskStandScript.hasJumped = true;
            player.GetComponent<PolygonCollider2D>().offset += colliderOffset;
            player.transform.position += playerOffset;
            player.layer = 15;
            player.GetComponent<SpriteRenderer>().sortingOrder = 6;
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
            deskStandScript.ShowVents();
            player.transform.position = deskUnder.transform.position + playerOffset;
            player.GetComponent<PlayerCtrl>().enabled = true;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            deskStandScript.hasClimbed = true;
            deskStandScript.isClimbing = false;
            deskStandScript.currentDesk = deskUnder;
            yield return new WaitForEndOfFrame();
            deskStandScript.shouldStepOff = true;
        }
        else
        {
            Color color = perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().color;
            color.a = 170f / 256f;
            perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().color = color;

            player.transform.position = currentOpenVent.transform.position - offsetVector;

            perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;

            perksTiles.transform.Find("Vents").gameObject.SetActive(false);
            perksTiles.transform.Find("VentObjects").gameObject.SetActive(false);

            player.layer = 3;

            player.GetComponent<SpriteRenderer>().sortingOrder = 6;
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;

            deskStandScript.currentDesk.GetComponent<DeskPickUp>().enabled = true;
        }
    }
}