using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class DeskStand : MonoBehaviour
{
    public DeskPickUp deskPickUpScript;
    public GameObject player;
    public Canvas inventoryCanvas;
    public GameObject perksTiles;
    public bool hasClimbed;
    public bool isClimbing;
    public bool hasJumped;
    private GameObject[] desks;
    public GameObject currentDesk;
    private Vector2 colliderOffset = new Vector2();
    private Vector3 playerOffset = new Vector3();
    public bool shouldStepOff = true;
    private void Start()
    {
        desks = GameObject.FindGameObjectsWithTag("Desk");

        colliderOffset.y = -.5f;
        colliderOffset.x = 0;
        playerOffset.y = .5f;
        playerOffset.x = 0;
        playerOffset.z = 0;
    }
    private void Update()
    {
        if (!hasClimbed && !isClimbing && !deskPickUpScript.isPickedUp)
        {
            foreach (GameObject desk in desks)
            {
                if (player.GetComponent<CircleCollider2D>().IsTouching(desk.transform.Find("ClimbingArea").GetComponent<BoxCollider2D>()) && Input.GetKeyDown(KeyCode.F))
                {
                    currentDesk = desk;
                    isClimbing = true;
                    StartCoroutine(ClimbDesk(desk));
                    break;
                }
            }
        }
        else if (hasClimbed && !isClimbing && shouldStepOff)
        {
            if (!player.GetComponent<PolygonCollider2D>().IsTouching(currentDesk.GetComponent<BoxCollider2D>()))
            {
                hasClimbed = false;
                hasJumped = false;
                StepOffDesk();
            }
        }
    }
    private IEnumerator ClimbDesk(GameObject desk)
    {
        player.GetComponent<PlayerCtrl>().enabled = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        desk.GetComponent<DeskPickUp>().enabled = false;
        yield return new WaitForEndOfFrame();
        desk.GetComponent<BoxCollider2D>().isTrigger = true;

        Vector3 deskVector = desk.transform.position;
        float speed = 5f;
        Vector3 direction = ((deskVector) - player.transform.position).normalized;
        while(Vector3.Distance(player.transform.position, deskVector + playerOffset) > 0.1f)//move player
        {
            player.transform.position += speed * Time.deltaTime * direction;

            //little offset to show that the player is on the desk
            if (player.transform.Find("MidBox").GetComponent<BoxCollider2D>().IsTouching(desk.GetComponent<BoxCollider2D>()) && !hasJumped)
            {
                hasJumped = true;

                player.GetComponent<PolygonCollider2D>().offset += colliderOffset;
                player.transform.position += playerOffset;
                player.layer = 15;
                ShowVents();
            }

            yield return null;
        }
        player.transform.position = deskVector + playerOffset;
        player.GetComponent<PlayerCtrl>().enabled = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        isClimbing = false;
        hasClimbed = true;
    }
    private void StepOffDesk()
    {
        player.GetComponent<PolygonCollider2D>().offset -= colliderOffset;
        player.GetComponent<Transform>().position -= playerOffset;
        currentDesk.GetComponent<DeskPickUp>().enabled = true;

        HideVents();

        currentDesk.GetComponent<BoxCollider2D>().isTrigger = false;
        player.layer = 3;
    }
    public void ShowVents()
    {
        perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = true; //enable backdrop
        Color aColor = perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().color;
        aColor.a = 170f / 256f;
        perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().color = aColor;
        perksTiles.transform.Find("Vents").gameObject.SetActive(true); //enable vent tiles
        perksTiles.transform.Find("VentObjects").gameObject.SetActive(true); //enable vent objects

        //set transparency of vents
        SpriteRenderer[] ventSpriteRenderers = perksTiles.transform.Find("Vents").GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = perksTiles.transform.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in ventSpriteRenderers)
        {
            Color color = sr.color;
            color.a = .75f;
            sr.color = color;
        }
        foreach (SpriteRenderer sr in ventObjectSpriteRenderers)
        {
            Color color = sr.color;
            color.a = .75f;
            sr.color = color;
        }
    }
    private void HideVents()
    {
        perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false; //diable backdrop
        perksTiles.transform.Find("VentObjects").gameObject.SetActive(false); //disable vent objects
        perksTiles.transform.Find("Vents").gameObject.SetActive(false); //disable vent tiles

        //undo transparency setting of vents
        SpriteRenderer[] ventSpriteRenderers = perksTiles.transform.Find("Vents").GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = perksTiles.transform.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in ventSpriteRenderers)
        {
            Color color = sr.color;
            color.a = 1;
            sr.color = color;
        }
        foreach(SpriteRenderer sr in ventObjectSpriteRenderers)
        {
            Color color = sr.color;
            color.a = 1;
            sr.color = color;
        }
    }
}
