using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class VentClimb : MonoBehaviour
{
    private DeskStand deskStandScript;
    private MouseCollisionOnItems mcs;
    private GameObject ic;
    private InventorySelection selectionScript;
    private GameObject player;
    private Transform tiles;
    private GameObject currentOpenVent;
    private Vector3 offsetVector = new Vector3();
    private Vector2 colliderOffset = new Vector2();
    private Vector3 playerOffset = new Vector3();
    private bool cameFromDesk;
    private bool deskIsUnder;
    private GameObject deskUnder;
    private HPAChecker HPAScript;
    private GameObject[] desks;
    private int groundLayer;
    private int undergroundLayer;
    private int ventLayer;
    private int roofLayer;
    private int playerLayer;
    private int uiLayer;
    private int ventCoverLayer;
    public void Start()
    {
        deskStandScript = GetComponent<DeskStand>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        ic = RootObjectCache.GetRoot("InventoryCanvas");
        selectionScript = GetComponent<InventorySelection>();
        player = RootObjectCache.GetRoot("Player");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        HPAScript = player.GetComponent<HPAChecker>();
        
        offsetVector.y = 1.6f;

        colliderOffset.y = -.05f;
        colliderOffset.x = 0;
        playerOffset.y = .5f;
        playerOffset.x = 0;
        playerOffset.z = 0;

        groundLayer = LayerMask.NameToLayer("Ground");
        undergroundLayer = LayerMask.NameToLayer("Underground");
        ventLayer = LayerMask.NameToLayer("Vents");
        roofLayer = LayerMask.NameToLayer("Roof");
        playerLayer = LayerMask.NameToLayer("Player");
        uiLayer = LayerMask.NameToLayer("UI");
        ventCoverLayer = LayerMask.NameToLayer("VentCovers");

        cameFromDesk = false;
    }
    public void Update()
    {
        if (!HPAScript.isBusy && deskStandScript.hasClimbed)//when the player is on a desk
        {
            if (mcs.isTouchingOpenVent)
            {
                float distance = Vector2.Distance(player.transform.position, mcs.touchedOpenVent.transform.position);
                desks = deskStandScript.desks;
                bool deskIsUnderVent = false;
                foreach(GameObject desk in desks)
                {
                    if(desk.transform.position + offsetVector == mcs.touchedOpenVent.transform.position)
                    {
                        deskIsUnderVent = true;
                        break;
                    }
                }
                if (Input.GetMouseButtonDown(0) && distance <= 2.4f && !deskStandScript.isClimbing && deskIsUnderVent)
                {
                    currentOpenVent = mcs.touchedOpenVent;
                    cameFromDesk = true;
                    StartCoroutine(ClimbVentUp());
                }
            }
        }
        else if (!HPAScript.isBusy && !Physics2D.GetIgnoreLayerCollision(playerLayer, ventLayer)) //when the player is in a vent
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
            
            player.GetComponent<CapsuleCollider2D>().offset -= colliderOffset;
            player.GetComponent<Transform>().position -= playerOffset;
        }

    }
    public IEnumerator ClimbVentUp()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForFixedUpdate();
        player.transform.position = currentOpenVent.transform.position;
        deskStandScript.hasClimbed = false;
        VentEnable();
        DisableAllLayerCollisions();
        Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, false);
        Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, false);
        Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, false);
        tiles.Find("VentTiles").gameObject.SetActive(true);
        tiles.Find("VentObjects").gameObject.SetActive(true);
        player.GetComponent<SpriteRenderer>().sortingOrder = 11;
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 12;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public IEnumerator ClimbVentDown()
    {
        yield return new WaitForEndOfFrame();

        foreach(Transform desk in tiles.Find("GroundObjects"))
        {
            if (!desk.CompareTag("Desk"))
            {
                continue;
            }

            float distance = Vector2.Distance(desk.position + offsetVector, currentOpenVent.transform.position);
            if(distance < .01f)
            {
                deskIsUnder = true;
                deskUnder = desk.gameObject;
                break;
            }
            else
            {
                deskIsUnder = false;
            }
        }
        DisableAllLayerCollisions();
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, false);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);

        if (deskIsUnder)//pulled from the deskstand script
        {            
            deskStandScript.shouldStepOff = false;
            yield return new WaitForFixedUpdate();
            player.GetComponent<PlayerCtrl>().enabled = false;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            deskStandScript.hasJumped = true;
            yield return new WaitForFixedUpdate();
            player.GetComponent<CapsuleCollider2D>().offset += colliderOffset;
            player.transform.position += playerOffset;
            player.GetComponent<SpriteRenderer>().sortingOrder = 6;
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
            deskStandScript.ShowVents();
            player.transform.position = deskUnder.transform.position + playerOffset;
            player.GetComponent<PlayerCtrl>().enabled = true;
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            deskStandScript.hasClimbed = true;
            deskStandScript.isClimbing = false;
            yield return new WaitForFixedUpdate();
            deskStandScript.shouldStepOff = true;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, false);
            Color color = tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color;
            color.a = 170f / 256f;
            tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color = color;

            player.transform.position = currentOpenVent.transform.position - offsetVector;

            tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;

            tiles.Find("VentTiles").gameObject.SetActive(false);
            tiles.Find("VentObjects").gameObject.SetActive(false);

            player.GetComponent<SpriteRenderer>().sortingOrder = 6;
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;

            desks = deskStandScript.desks;
            foreach(GameObject desk in desks)
            {
                desk.GetComponent<DeskPickUp>().enabled = true;
            }
        }
    }
    private void DisableAllLayerCollisions()
    {
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, roofLayer, true);
    }
    public void VentEnable()
    {
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = true;
        Color color = tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color;
        color.a = 235f / 256f;
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color = color;

        SpriteRenderer ventTilesSpriteRenderer = tiles.Find("VentTiles").GetComponent<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = tiles.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
        ventTilesSpriteRenderer.color = new Color(ventTilesSpriteRenderer.color.r, ventTilesSpriteRenderer.color.g, ventTilesSpriteRenderer.color.b, 1);
        foreach (SpriteRenderer sr in ventObjectSpriteRenderers)
        {
            Color aColor = sr.color;
            aColor.a = 1;
            sr.color = aColor;
        }
    }
}