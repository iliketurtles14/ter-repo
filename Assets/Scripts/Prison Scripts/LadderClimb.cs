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
    public bool hasPickedUp;
    private Vector3 offsetVector = new Vector3(); //only for ground-to-roof operations
    private float distance;
    private HPAChecker HPAScript;
    public void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        ic = RootObjectCache.GetRoot("InventoryCanvas");
        selectionScript = GetComponent<InventorySelection>();
        deskStandScript = GetComponent<DeskStand>();
        player = RootObjectCache.GetRoot("Player");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        HPAScript = player.GetComponent<HPAChecker>();

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

                if (!hasPickedUp && !currentLadder.GetComponent<LadderConnect>().fall)
                {
                    StartCoroutine(ClimbLadder());
                }
                else if(!hasPickedUp && currentLadder.GetComponent<LadderConnect>().fall)
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
                foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Desk"))
                {
                    if(obj.GetComponent<DeskPickUp>() != null)
                    {
                        obj.GetComponent<DeskPickUp>().enabled = true;
                    }
                }
                player.layer = 3;
                player.GetComponent<SpriteRenderer>().sortingOrder = 6;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
                tiles.Find("Roof").gameObject.SetActive(false);
                tiles.Find("RoofObjects").gameObject.SetActive(false);
                tiles.Find("Vents").gameObject.SetActive(false);
                tiles.Find("VentObjects").gameObject.SetActive(false);
                tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                foreach (Transform child in tiles.Find("GroundObjects"))
                {
                    if (child.gameObject.CompareTag("Item"))
                    {
                        child.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
                EnableTags();
                break;
            case "vents":
                player.layer = 12;
                player.GetComponent<SpriteRenderer>().sortingOrder = 11;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 12;
                tiles.Find("Roof").gameObject.SetActive(false);
                tiles.Find("RoofObjects").gameObject.SetActive(false);
                tiles.Find("Vents").gameObject.SetActive(true);
                tiles.Find("VentObjects").gameObject.SetActive(true);
                foreach (Transform child in tiles.Find("GroundObjects"))
                {
                    if (child.gameObject.CompareTag("Item"))
                    {
                        child.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                VentEnable();
                DisableTags();
                break;
            case "roof":
                player.layer = 13;
                player.GetComponent<SpriteRenderer>().sortingOrder = 15;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 16;
                tiles.Find("Roof").gameObject.SetActive(true);
                tiles.Find("RoofObjects").gameObject.SetActive(true);
                tiles.Find("Vents").gameObject.SetActive(false);
                tiles.Find("VentObjects").gameObject.SetActive(false);
                tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = false;
                foreach (Transform child in tiles.Find("GroundObjects"))
                {
                    if (child.gameObject.CompareTag("Item"))
                    {
                        child.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                DisableTags();
                break;
        }
        player.transform.position = currentLadder.GetComponent<LadderConnect>().connectedTilePos;

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void DisableTags()
    {
        mcs.EnableAllTags();
        
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
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = true;
        Color color = tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color;
        color.a = 235f / 256f;
        tiles.Find("Backdrop").GetComponent<SpriteRenderer>().color = color;

        SpriteRenderer[] ventSpriteRenderers = tiles.Find("Vents").GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] ventObjectSpriteRenderers = tiles.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
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