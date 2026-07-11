using System.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class LadderClimb : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private GameObject ic;
    private InventorySelection selectionScript;
    private DeskStand deskStandScript;
    private GameObject player;
    private Transform tiles;
    private GameObject currentLadder;
    private Vector3 offsetVector = new Vector3(); //only for ground-to-roof operations
    private float distance;
    private HPAChecker HPAScript;
    private GameObject undergroundLight;
    private GameObject globalLight;
    private HoleClimb holeClimbScript;

    private int groundLayer;
    private int undergroundLayer;
    private int ventLayer;
    private int roofLayer;
    private int playerLayer;
    private int uiLayer;
    private int ventCoverLayer;
    public void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        ic = RootObjectCache.GetRoot("InventoryCanvas");
        selectionScript = GetComponent<InventorySelection>();
        deskStandScript = GetComponent<DeskStand>();
        player = RootObjectCache.GetRoot("Player");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        HPAScript = player.GetComponent<HPAChecker>();
        holeClimbScript = GetComponent<HoleClimb>();
        undergroundLight = RootObjectCache.GetRoot("UndergroundLight");
        globalLight = RootObjectCache.GetRoot("GlobalLight");

        groundLayer = LayerMask.NameToLayer("Ground");
        undergroundLayer = LayerMask.NameToLayer("Underground");
        ventLayer = LayerMask.NameToLayer("Vents");
        roofLayer = LayerMask.NameToLayer("Roof");
        playerLayer = LayerMask.NameToLayer("Player");
        uiLayer = LayerMask.NameToLayer("UI");
        ventCoverLayer = LayerMask.NameToLayer("VentCovers");

        offsetVector.y = 1.6f;
    }
    public void Update()
    {
        if (!HPAScript.isBusy && mcs.isTouchingGroundLadder || mcs.isTouchingVentLadder || mcs.isTouchingRoofLadder)
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

                if (!currentLadder.GetComponent<LadderConnect>().fall)
                {
                    StartCoroutine(ClimbLadder());
                }
                else if(currentLadder.GetComponent<LadderConnect>().fall)
                {
                    //fall
                }
            }
        }
    }
    public IEnumerator ClimbLadder()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForFixedUpdate();

        player.transform.position = currentLadder.GetComponent<LadderConnect>().connectedTilePos;

        switch (currentLadder.GetComponent<LadderConnect>().goToLayer)
        {
            case "ground":
                player.GetComponent<SpriteRenderer>().sortingOrder = 6;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
                tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                tiles.Find("RoofTiles").gameObject.SetActive(false);
                tiles.Find("RoofObjects").gameObject.SetActive(false);
                tiles.Find("VentTiles").gameObject.SetActive(false);
                tiles.Find("VentObjects").gameObject.SetActive(false);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(false);
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
                break;
            case "vents":
                player.GetComponent<SpriteRenderer>().sortingOrder = 11;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 12;
                tiles.Find("RoofTiles").gameObject.SetActive(false);
                tiles.Find("RoofObjects").gameObject.SetActive(false);
                tiles.Find("VentTiles").gameObject.SetActive(true);
                tiles.Find("VentObjects").gameObject.SetActive(true);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(false);
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, false);
                Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, false);
                VentEnable();
                break;
            case "roof":
                player.GetComponent<SpriteRenderer>().sortingOrder = 15;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 16;
                tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                tiles.Find("RoofTiles").gameObject.SetActive(true);
                tiles.Find("RoofObjects").gameObject.SetActive(true);
                tiles.Find("VentTiles").gameObject.SetActive(false);
                tiles.Find("VentObjects").gameObject.SetActive(false);
                tiles.Find("RoofShadowPlane").gameObject.SetActive(true);
                DisableAllLayerCollisions();
                Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, false);
                Physics2D.IgnoreLayerCollision(playerLayer, roofLayer, false);
                break;
        }
        player.transform.position = currentLadder.GetComponent<LadderConnect>().connectedTilePos;

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void DisableAllLayerCollisions()
    {
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, true);
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
    public void SendToGround()//for death and solitary and other things that need to force the player to ground
    {                         //(includes hole climb stuff too heh)
        player.GetComponent<SpriteRenderer>().sortingOrder = 6;
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
        tiles.Find("RoofTiles").gameObject.SetActive(false);
        tiles.Find("RoofObjects").gameObject.SetActive(false);
        tiles.Find("VentTiles").gameObject.SetActive(false);
        tiles.Find("VentObjects").gameObject.SetActive(false);
        tiles.Find("RoofShadowPlane").gameObject.SetActive(false);
        DisableAllLayerCollisions();
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, false);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
        tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingOrder = -1;
        tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingOrder = -1;
        tiles.Find("UndergroundObjects").gameObject.SetActive(false);
        undergroundLight.SetActive(false);
        globalLight.SetActive(true);
        
        if (deskStandScript.hasClimbed)
        {
            deskStandScript.StepOffDesk();
            deskStandScript.hasClimbed = false;
            deskStandScript.hasJumped = false;
        }
    }
}