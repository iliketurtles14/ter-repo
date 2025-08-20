using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class DeskStand : MonoBehaviour
{
    private DeskPickUp deskPickUpScript;
    private GameObject player;
    private GameObject inventoryCanvas;
    private Transform tiles;
    public bool hasClimbed;
    public bool isClimbing;
    public bool hasJumped;
    public bool isPickedUp;
    private GameObject[] desks;
    public GameObject currentDesk;
    private Vector2 colliderOffset = new Vector2();
    private Vector3 playerOffset = new Vector3();
    public bool shouldStepOff = true;
    private void Start()
    {
        desks = GameObject.FindGameObjectsWithTag("Desk");
        deskPickUpScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("DeskMenuPanel").GetComponent<DeskPickUp>();
        player = RootObjectCache.GetRoot("Player");
        inventoryCanvas = RootObjectCache.GetRoot("InventoryCanvas");
        tiles = RootObjectCache.GetRoot("Tiles").transform;

        colliderOffset.y = -.05f;
        colliderOffset.x = 0;
        playerOffset.y = .5f;
        playerOffset.x = 0;
        playerOffset.z = 0;
    }
    private void Update()
    {
        if (!hasClimbed && !isClimbing && !isPickedUp)
        {
            foreach (GameObject desk in desks)
            {
                if (player.GetComponent<CapsuleCollider2D>().IsTouching(desk.transform.Find("ClimbingArea").GetComponent<BoxCollider2D>()) && Input.GetKeyDown(KeyCode.F))
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
            if (!player.GetComponent<CapsuleCollider2D>().IsTouching(currentDesk.GetComponent<BoxCollider2D>()))
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

                player.GetComponent<CapsuleCollider2D>().offset += colliderOffset;
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
        player.GetComponent<CapsuleCollider2D>().offset -= colliderOffset;
        player.GetComponent<Transform>().position -= playerOffset;
        currentDesk.GetComponent<DeskPickUp>().enabled = true;

        HideVents();

        currentDesk.GetComponent<BoxCollider2D>().isTrigger = false;
        player.layer = 3;
    }
    public void ShowVents()
    {
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = true; //enable backdrop
        Color aColor = tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color;
        aColor.a = 170f / 256f;
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color = aColor;
        tiles.Find("Vents").gameObject.SetActive(true); //enable vent tiles
        tiles.Find("VentObjects").gameObject.SetActive(true); //enable vent objects

        //set transparency of vents
        SpriteRenderer[] ventSpriteRenderers = tiles.Find("Vents").GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = tiles.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
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
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false; //diable backdrop
        tiles.Find("VentObjects").gameObject.SetActive(false); //disable vent objects
        tiles.Find("Vents").gameObject.SetActive(false); //disable vent tiles

        //undo transparency setting of vents
        SpriteRenderer[] ventSpriteRenderers = tiles.Find("Vents").GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = tiles.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
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
