using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Image = UnityEngine.UI.Image;

public class ItemBehaviours : MonoBehaviour
{
    public InventorySelection selectionScript;
    public HoleClimb holeClimbScript; //for sprites
    public GameObject perksTiles;
    private ItemData selectedItemData;
    private ItemData usedItemData;
    public Canvas InventoryCanvas;
    public int slotNumber;
    private int usedSlotNumber;
    private List<InventoryItem> inventoryList;
    public Inventory inventoryScript;
    public MouseCollisionOnItems mcs;
    public GameObject barLine;
    public GameObject actionBarPanel;
    public bool barIsMoving;
    public Transform PlayerTransform;
    public Transform oldPlayerTransform;
    private bool cancelBar;
    public TextMeshProUGUI ActionTextBox;
    private int whatSlot;
    public Sprite clearSprite;
    public Sprite hole24;
    public Sprite hole49;
    public Sprite hole74;
    public Sprite hole99;
    public Sprite holeUp24;
    public Sprite holeUp49;
    public Sprite holeUp74;
    public Sprite holeUp99;
    private string whatAction;
    //general

    //breaking
    public bool selectedChippingItem;
    public bool selectedCuttingItem;
    public bool selectedDiggingItem;
    public bool selectedVentBreakingItem;
    public bool goodForDig;
    public bool halfDug;
    public GameObject halfDugHole;
    public GameObject halfDugHoleUp;
    //roping/grapling
    public GameObject ropeTile;
    public GameObject touchedTileObject;
    public GameObject emptyTile;
    public GameObject emptyVentCover;
    public int currentHeight;
    public int ropeTileHeight;
    public bool isRoping;
    public void Start()
    {
        InventoryCanvas.transform.Find("ActionBar").GetComponent<Image>().enabled = false;
        ActionTextBox.text = "";
    }
    public void Update()
    {
        //define vars
        inventoryList = inventoryScript.inventory;

        if (selectionScript.aSlotSelected)
        {
            if (selectionScript.slot1Selected) { slotNumber = 0; }
            else if (selectionScript.slot2Selected) { slotNumber = 1; }
            else if (selectionScript.slot3Selected) { slotNumber = 2; }
            else if (selectionScript.slot4Selected) { slotNumber = 3; }
            else if (selectionScript.slot5Selected) { slotNumber = 4; }
            else if (selectionScript.slot6Selected) { slotNumber = 5; }

            selectedItemData = inventoryList[slotNumber].itemData;
        }
        else { selectedItemData = null; }


        ///BREAKING TILES
        //vars

        if (selectionScript.aSlotSelected)
        {
            if (selectedItemData.chippingPower != -1) { selectedChippingItem = true; }
            else { selectedChippingItem = false; }
            if (selectedItemData.cuttingPower != -1) { selectedCuttingItem = true; }
            else { selectedCuttingItem = false; }
            if (selectedItemData.diggingPower != -1) { selectedDiggingItem = true; }
            else { selectedDiggingItem = false; }
            if (selectedItemData.ventBreakingPower != -1) { selectedVentBreakingItem = true; }
            else { selectedVentBreakingItem = false; }
        }
        else if (!selectionScript.aSlotSelected) 
        {
            Deselect();
        }


        //chipping
        if (mcs.isTouchingWall && Input.GetMouseButtonDown(0) && selectedChippingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedWall.transform.position);
            if (distance <= 2.4f)
            {
                whatAction = "chipping";
                touchedTileObject = mcs.touchedWall.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Chipping");
                Deselect();
            }
        }
        else if(Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }
        //cutting fences
        if(mcs.isTouchingFence && Input.GetMouseButtonDown(0) && selectedCuttingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedFence.transform.position);
            if(distance <= 2.4f)
            {
                whatAction = "cutting fence";
                touchedTileObject = mcs.touchedFence.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Cutting");
                Deselect();
            }
        }else if(Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }
        //cutting bars
        if(mcs.isTouchingBars && Input.GetMouseButtonDown(0) && selectedCuttingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedBars.transform.position);
            if(distance <= 2.4f)
            {
                whatAction = "cutting bars";
                touchedTileObject = mcs.touchedBars.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Cutting");
                Deselect();
            }
        }else if(Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }
        //unscrewing vents
        if(mcs.isTouchingVentCover && Input.GetMouseButtonDown(0) && selectedVentBreakingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedVentCover.transform.position);
            if(distance <= 2.4f)
            {
                whatAction = "unscrewing vent";
                touchedTileObject = mcs.touchedVentCover.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Unscrewing");
                Deselect();
            }
        }
        else if (Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }

        //cutting vents
        if (mcs.isTouchingVentCover && Input.GetMouseButtonDown(0) && selectedCuttingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedVentCover.transform.position);
            if (distance <= 2.4f)
            {
                whatAction = "cutting vent";
                touchedTileObject = mcs.touchedVentCover.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Cutting");
                Deselect();
            }
        }
        else if (Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }

        //unscrewing slats
        if (mcs.isTouchingSlats && Input.GetMouseButtonDown(0) && selectedVentBreakingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedSlats.transform.position);
            if (distance <= 2.4f)
            {
                whatAction = "unscrewing slats";
                touchedTileObject = mcs.touchedSlats.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Unscrewing");
                Deselect();
            }
        }
        else if (Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }

        // cutting slats
        if (mcs.isTouchingSlats && Input.GetMouseButtonDown(0) && selectedCuttingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedSlats.transform.position);
            if (distance <= 2.4f)
            {
                whatAction = "cutting slats";
                touchedTileObject = mcs.touchedSlats.gameObject;
                StartCoroutine(DrawActionBar());
                CreateActionText("Cutting");
                Deselect();
            }
        }
        else if (Input.GetMouseButtonDown(0) && barIsMoving && selectionScript.aSlotSelected)
        {
            Deselect();
        }
        
        //digging down holes
        if(PlayerTransform.gameObject.layer == 3 && mcs.isTouchingFloor && Input.GetMouseButtonDown(0) && selectedDiggingItem && !barIsMoving)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedFloor.transform.position);
            if(distance <= 2.4f)
            {
                touchedTileObject = mcs.touchedFloor.gameObject;
                foreach(Transform obj in perksTiles.transform.Find("GroundObjects")) //checks if able to dig
                {
                    if (!obj.name.StartsWith("100%HoleDown"))
                    {
                        if(obj.position != touchedTileObject.transform.position)
                        {
                            goodForDig = true;
                        }
                        else if(obj.position == touchedTileObject.transform.position)
                        {
                            goodForDig = false;
                            break;
                        }
                    }
                }
                whatAction = "digging down";
                StartCoroutine(DrawActionBar());
                CreateActionText("Digging");
                Deselect();
            }
        }

        ///ROPES AND GRAPPLES
        //rope
        if (selectionScript.aSlotSelected && selectedItemData.id == 105 && Input.GetMouseButtonDown(0) && mcs.isTouchingRoofLedge && !isRoping)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedRoofLedge.transform.position);
            if(distance <= 2.4f)
            {
                touchedTileObject = mcs.touchedRoofLedge;
                StartCoroutine(Rope("rope"));
            }
        }

        //sheetrope
        if (selectionScript.aSlotSelected && selectedItemData.id == 131 && Input.GetMouseButtonDown(0) && mcs.isTouchingRoofLedge && !isRoping)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedRoofLedge.transform.position);
            if (distance <= 2.4f)
            {
                touchedTileObject = mcs.touchedRoofLedge;
                StartCoroutine(Rope("sheet"));
            }
        }

        //grapple
        if (selectionScript.aSlotSelected && selectedItemData.id == 102 && Input.GetMouseButtonDown(0) && mcs.isTouchingRoofLedge && !isRoping)
        {
            float distance = Vector2.Distance(PlayerTransform.position, mcs.touchedRoofLedge.transform.position);
            if (distance <= 2.4f)
            {
                touchedTileObject = mcs.touchedRoofLedge;
                StartCoroutine(Rope("grapple"));
            }
        }


        if (barIsMoving && oldPlayerTransform.position != PlayerTransform.position)//keep at botom
        {
            StopCoroutine(DrawActionBar());
            DestroyActionBar();
        }
    }
    public IEnumerator Rope(string identifier)
    {
        usedItemData = selectedItemData;
        usedSlotNumber = slotNumber;
        //get tile to rope to
        Vector3 ropeTilePos = new Vector3();
        if(touchedTileObject.name.StartsWith("Roofing Vertical") || touchedTileObject.name.StartsWith("Top Wall Vertical (Roof)"))
        {
            if (PlayerTransform.position.x < touchedTileObject.transform.position.x)//if player is to the left of the ledge
            {
                ropeTilePos.x = touchedTileObject.transform.position.x + 1.6f;
            }
            else if (PlayerTransform.position.x > touchedTileObject.transform.position.x)//if player is to the right of the ledge
            {
                ropeTilePos.x = touchedTileObject.transform.position.x - 1.6f;
            }
            ropeTilePos.y = touchedTileObject.transform.position.y;
            ropeTilePos.z = touchedTileObject.transform.position.z;
        }
        else if(touchedTileObject.name.StartsWith("Roofing Horizontal") || touchedTileObject.name.StartsWith("Top Wall Horizontal (Roof)"))
        {
            if (PlayerTransform.position.y < touchedTileObject.transform.position.y)//if player is below the ledge
            {
                ropeTilePos.y = touchedTileObject.transform.position.y + 1.6f;
            }
            else if (PlayerTransform.position.y > touchedTileObject.transform.position.y)//if player is above the ledge
            {
                ropeTilePos.y = touchedTileObject.transform.position.y - 1.6f;
            }
            ropeTilePos.x = touchedTileObject.transform.position.x;
            ropeTilePos.z = touchedTileObject.transform.position.z;
        }
        foreach (Transform tile in perksTiles.transform.Find("Roof"))//gets tile to rope to
        {
            if (tile.position == ropeTilePos)
            {
                ropeTile = tile.gameObject;
                break;
            }
            else if(tile.position != ropeTilePos)
            {
                ropeTile = null;
            }
        }

        if(ropeTile == null && !touchedTileObject.name.StartsWith("Top Wall"))//check if roping off the roof normally
        {
            //check if there is an obstacle in the way
            Vector3 obstacleOffset = new Vector3(0, 1.6f, 0);
            foreach(Transform tile in perksTiles.transform.Find("Ground"))
            {
                if(tile.gameObject.layer == 8 && tile.position == ropeTilePos - obstacleOffset)
                {
                    yield break;
                }
            }
            
            //move to tile
            PlayerTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            float speed = 10f;
            Vector3 direction = (ropeTilePos - PlayerTransform.position).normalized;
            isRoping = true;
            GameObject ropePrefab;
            switch (identifier)
            {
                case "rope":
                    ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/Rope");
                    break;
                case "sheet":
                    ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/SheetRope");
                    break;
                case "grapple":
                    ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/Grapple");
                    break;
                default:
                    ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/Grapple");
                    break;
            }
            GameObject ropeObject = Instantiate(ropePrefab, PlayerTransform.position, Quaternion.identity, perksTiles.transform.Find("RoofObjects"));
            Vector2 ropeSize = new Vector2(.1f, .5f);
            ropeObject.GetComponent<SpriteRenderer>().size = ropeSize;
            Quaternion ropeRotation = Quaternion.LookRotation(Vector3.forward, direction);
            ropeRotation *= Quaternion.Euler(0, 0, 90);
            ropeObject.transform.rotation = ropeRotation;
            while (Vector3.Distance(PlayerTransform.position, ropeTilePos) > 0.1f)
            {
                PlayerTransform.position += speed * Time.deltaTime * direction;

                ropeSize.x = Vector3.Distance(PlayerTransform.position, ropeTilePos);
                ropeObject.GetComponent<SpriteRenderer>().size = ropeSize;
                Vector3 midpoint = (PlayerTransform.position + ropeTilePos) / 2;
                ropeObject.transform.position = midpoint;

                yield return null;
            }
            Destroy(ropeObject);
            PlayerTransform.position = ropeTilePos;
            isRoping = false;
            RemoveItemDurability(usedItemData.currentDurability, usedItemData.durability);
            PlayerTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            yield break;
        }
        else if(ropeTile == null && touchedTileObject.name.StartsWith("Top Wall") && identifier == "grapple")//check if roping off the roof via top wall (grapple only)
        {
            //check if there is an obstacle in the way
            Vector3 obstacleOffset = new Vector3(0, 1.6f, 0);
            foreach (Transform tile in perksTiles.transform.Find("Ground"))
            {
                if (tile.gameObject.layer == 8 && tile.position == ropeTilePos - obstacleOffset)
                {
                    yield break;
                }
            }

            //move to tile
            PlayerTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            float speed = 10f;
            Vector3 direction = (ropeTilePos - PlayerTransform.position).normalized;
            isRoping = true;
            GameObject ropePrefab;
            ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/Grapple");
            GameObject ropeObject = Instantiate(ropePrefab, PlayerTransform.position, Quaternion.identity, perksTiles.transform.Find("RoofObjects"));
            Vector2 ropeSize = new Vector2(.1f, .5f);
            ropeObject.GetComponent<SpriteRenderer>().size = ropeSize;
            Quaternion ropeRotation = Quaternion.LookRotation(Vector3.forward, direction);
            ropeRotation *= Quaternion.Euler(0, 0, 90);
            ropeObject.transform.rotation = ropeRotation;
            while (Vector3.Distance(PlayerTransform.position, ropeTilePos) > 0.1f)
            {
                PlayerTransform.position += speed * Time.deltaTime * direction;

                ropeSize.x = Vector3.Distance(PlayerTransform.position, ropeTilePos);
                ropeObject.GetComponent<SpriteRenderer>().size = ropeSize;
                Vector3 midpoint = (PlayerTransform.position + ropeTilePos) / 2;
                ropeObject.transform.position = midpoint;

                yield return null;
            }
            Destroy(ropeObject);
            PlayerTransform.position = ropeTilePos;
            isRoping = false;
            RemoveItemDurability(usedItemData.currentDurability, usedItemData.durability);
            PlayerTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            yield break;
        }
        //see if possible based on height
        GameObject lastTouchedRoofFloor = PlayerTransform.GetComponent<PlayerFloorCollision>().lastTouchedRoofFloor;

        if(lastTouchedRoofFloor.name.StartsWith("Roof Floor High"))
        {
            currentHeight = 3;
        }
        else if(lastTouchedRoofFloor.name.StartsWith("Roof Floor Medium"))
        {
            currentHeight = 2;
        }
        else if(lastTouchedRoofFloor.name.StartsWith("Roof Floor Low"))
        {
            currentHeight = 1;
        }

        if (ropeTile.name.StartsWith("Roof Floor High"))
        {
            ropeTileHeight = 3;
        }
        else if (ropeTile.name.StartsWith("Roof Floor Medium"))
        {
            ropeTileHeight = 2;
        }
        else if (ropeTile.name.StartsWith("Roof Floor Low"))
        {
            ropeTileHeight = 1;
        }

        if(currentHeight >= ropeTileHeight && (identifier == "rope" || identifier == "sheet"))
        {
            //move to tile
            PlayerTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            float speed = 10f;
            Vector3 direction = (ropeTile.transform.position - PlayerTransform.position).normalized;
            isRoping = true;
            GameObject ropePrefab;
            switch (identifier)
            {
                case "rope":
                    ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/Rope");
                    break;
                case "sheet":
                    ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/SheetRope");
                    break;
                default:
                    ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/Rope");
                    break;
            }
            GameObject ropeObject = Instantiate(ropePrefab, PlayerTransform.position, Quaternion.identity, perksTiles.transform.Find("RoofObjects"));
            Vector2 ropeSize = new Vector2(.1f, .5f);
            ropeObject.GetComponent<SpriteRenderer>().size = ropeSize;
            Quaternion ropeRotation = Quaternion.LookRotation(Vector3.forward, direction);
            ropeRotation *= Quaternion.Euler(0, 0, 90);
            ropeObject.transform.rotation = ropeRotation;
            while(Vector3.Distance(PlayerTransform.position, ropeTile.transform.position) > 0.1f)
            {
                PlayerTransform.position += speed * Time.deltaTime * direction;

                ropeSize.x = Vector3.Distance(PlayerTransform.position, ropeTile.transform.position);
                ropeObject.GetComponent<SpriteRenderer>().size = ropeSize;
                Vector3 midpoint = (PlayerTransform.position + ropeTile.transform.position) / 2;
                ropeObject.transform.position = midpoint;

                yield return null;
            }
            Destroy(ropeObject);
            PlayerTransform.position = ropeTile.transform.position;
            isRoping = false;
            RemoveItemDurability(usedItemData.currentDurability, usedItemData.durability);
            PlayerTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (currentHeight <= ropeTileHeight && identifier == "grapple")
        {
            //move to tile
            PlayerTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            float speed = 10f;
            Vector3 direction = (ropeTile.transform.position - PlayerTransform.position).normalized;
            isRoping = true;
            GameObject ropePrefab;
            ropePrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/Grapple");
            GameObject ropeObject = Instantiate(ropePrefab, PlayerTransform.position, Quaternion.identity, perksTiles.transform.Find("RoofObjects"));
            Vector2 ropeSize = new Vector2(.1f, .5f);
            ropeObject.GetComponent<SpriteRenderer>().size = ropeSize;
            Quaternion ropeRotation = Quaternion.LookRotation(Vector3.forward, direction);
            ropeRotation *= Quaternion.Euler(0, 0, 90);
            ropeObject.transform.rotation = ropeRotation;
            while (Vector3.Distance(PlayerTransform.position, ropeTile.transform.position) > 0.1f)
            {
                PlayerTransform.position += speed * Time.deltaTime * direction;

                ropeSize.x = Vector3.Distance(PlayerTransform.position, ropeTile.transform.position);
                ropeObject.GetComponent<SpriteRenderer>().size = ropeSize;
                Vector3 midpoint = (PlayerTransform.position + ropeTile.transform.position) / 2;
                ropeObject.transform.position = midpoint;

                yield return null;
            }
            Destroy(ropeObject);
            PlayerTransform.position = ropeTile.transform.position;
            isRoping = false;
            RemoveItemDurability(usedItemData.currentDurability, usedItemData.durability);
            PlayerTransform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            yield break;
        }
    }
    public void DigDown(TileData touchedTileData)
    {
        foreach (Transform tile in perksTiles.transform.Find("UndergroundObjects"))
        {
            if (tile.position == touchedTileObject.transform.position && tile.name.StartsWith("HalfHoleUp"))
            {
                Destroy(tile.gameObject); break;
            }
        }
        foreach (Transform tile in perksTiles.transform.Find("GroundObjects"))
        {
            if (tile.position == touchedTileObject.transform.position && tile.name.StartsWith("HalfHoleDown"))
            {
                Destroy(tile.gameObject); break;
            }
        }

        GameObject dirtPrefab = Resources.Load<GameObject>("PerksPrefabs/Underground/Dirt");
        GameObject emptyDirtPrefab = Resources.Load<GameObject>("PerksPrefabs/Underground/DirtEmpty");
        GameObject halfHoleUpPrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/HalfHoleUp");
        GameObject halfHoleDownPrefab = Resources.Load<GameObject>("PerksPrefabs/Objects/HalfHoleDown");

        Vector3 northOffset = new Vector3(0, 1.6f);
        Vector3 southOffset = new Vector3(0, -1.6f);
        Vector3 eastOffset = new Vector3(1.6f, 0);
        Vector3 westOffset = new Vector3(-1.6f, 0);

        bool emptyNorth = false;
        bool emptySouth = false;
        bool emptyEast = false;
        bool emptyWest = false;

        GameObject dirtObject = Instantiate(emptyDirtPrefab, touchedTileObject.transform.position, Quaternion.identity, perksTiles.transform.Find("Underground"));
        GameObject halfHoleDownObject = Instantiate(halfHoleDownPrefab, touchedTileObject.transform.position, Quaternion.identity, perksTiles.transform.Find("GroundObjects"));
        GameObject halfHoleUpObject = Instantiate(halfHoleUpPrefab, touchedTileObject.transform.position, Quaternion.identity, perksTiles.transform.Find("UndergroundObjects"));

        if (touchedTileData.currentDurability <= 24 && touchedTileData.currentDurability > 0)
        {
            halfHoleDownObject.GetComponent<SpriteRenderer>().sprite = hole99;
            halfHoleUpObject.GetComponent<Light2D>().lightCookieSprite = holeUp99;
        }
        else if (touchedTileData.currentDurability >= 25 && touchedTileData.currentDurability < 49)
        {
            halfHoleDownObject.GetComponent<SpriteRenderer>().sprite = hole74;
            halfHoleUpObject.GetComponent<Light2D>().lightCookieSprite = holeUp74;
        }
        else if (touchedTileData.currentDurability >= 50 && touchedTileData.currentDurability < 74)
        {
            halfHoleDownObject.GetComponent<SpriteRenderer>().sprite = hole49;
            halfHoleUpObject.GetComponent<Light2D>().lightCookieSprite = holeUp49;
        }
        else if (touchedTileData.currentDurability >= 75 && touchedTileData.currentDurability < 99)
        {
            halfHoleDownObject.GetComponent<SpriteRenderer>().sprite = hole24;
            halfHoleUpObject.GetComponent<Light2D>().lightCookieSprite = holeUp24;
        }
        else if(touchedTileData.currentDurability <= 0)
        {
            Destroy(halfHoleDownObject);
            Destroy(halfHoleUpObject);
        }

        foreach (Transform tile in perksTiles.transform.Find("Underground"))
        {
            if (tile.position == touchedTileObject.transform.position + northOffset && tile.name.StartsWith("DirtEmpty"))
            {
                emptyNorth = true;
            }
            else if (tile.position == touchedTileObject.transform.position + southOffset && tile.name.StartsWith("DirtEmpty"))
            {
                emptySouth = true;
            }
            else if (tile.position == touchedTileObject.transform.position + eastOffset && tile.name.StartsWith("DirtEmpty"))
            {
                emptyEast = true;
            }
            else if (tile.position == touchedTileObject.transform.position + westOffset && tile.name.StartsWith("DirtEmpty"))
            {
                emptyWest = true;
            }

            if (tile.position == touchedTileObject.transform.position + northOffset && tile.name.StartsWith("Dirt(Clone)"))
            {
                Destroy(tile.gameObject);
            }
            else if (tile.position == touchedTileObject.transform.position + southOffset && tile.name.StartsWith("Dirt(Clone)"))
            {
                Destroy(tile.gameObject);
            }
            else if (tile.position == touchedTileObject.transform.position + eastOffset && tile.name.StartsWith("Dirt(Clone)"))
            {
                Destroy(tile.gameObject);
            }
            else if (tile.position == touchedTileObject.transform.position + westOffset && tile.name.StartsWith("Dirt(Clone)"))
            {
                Destroy(tile.gameObject);
            }
        }

        if (!emptyNorth)
        {
            Instantiate(dirtPrefab, touchedTileObject.transform.position + northOffset, Quaternion.identity, perksTiles.transform.Find("Underground"));
        }
        if (!emptySouth)
        {
            Instantiate(dirtPrefab, touchedTileObject.transform.position + southOffset, Quaternion.identity, perksTiles.transform.Find("Underground"));
        }
        if (!emptyEast)
        {
            Instantiate(dirtPrefab, touchedTileObject.transform.position + eastOffset, Quaternion.identity, perksTiles.transform.Find("Underground"));
        }
        if (!emptyWest)
        {
            Instantiate(dirtPrefab, touchedTileObject.transform.position + westOffset, Quaternion.identity, perksTiles.transform.Find("Underground"));
        }
        
        foreach(Transform tile1 in perksTiles.transform.Find("Underground"))
        {
            foreach(Transform tile2 in perksTiles.transform.Find("Underground"))
            {
                if(tile1 != tile2 && tile1.position == tile2.position && tile1.name == tile2.name)
                {
                    Destroy(tile2.gameObject);
                }
            }
        }
    }
    public void Deselect()
    {
        selectedChippingItem = false;
        selectedCuttingItem = false;
        selectedDiggingItem = false;
        selectedVentBreakingItem = false;
    }
    public void RemoveItemDurability(int currentDurability, int durability)
    {
        usedItemData.currentDurability = currentDurability - durability;
        if(usedItemData.currentDurability <= 0)
        {
            BreakItem();
        }
    }
    public void RemoveTileDurability(GameObject touchedTile, int currentDurability, int itemStrength)
    {
        TileData touchedTileData = touchedTile.GetComponent<TileCollectionData>().tileData;
        touchedTileData.currentDurability = currentDurability - itemStrength;
        if(whatAction == "digging down")
        {
            DigDown(touchedTileData);
        }

        if (touchedTileData.currentDurability <= 0)
        {
            BreakTile();
        }
    }
    public IEnumerator DrawActionBar()
    {
        cancelBar = false;
        barIsMoving = true;
        oldPlayerTransform.position = PlayerTransform.position;
        usedItemData = selectedItemData;
        usedSlotNumber = slotNumber;
        InventoryCanvas.transform.Find("ActionBar").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(.045f);
        if (cancelBar) { yield break; }
        for(int i = 1; i <= 49; i++)
        {
            if (cancelBar) { yield break; }
            Instantiate(barLine, actionBarPanel.transform);
            yield return new WaitForSeconds(.045f);
        }
        DestroyActionBar();
        RemoveItemDurability(usedItemData.currentDurability, usedItemData.durability);

        switch (whatAction)
        {
            case "chipping": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.chippingPower); break;
            case "cutting fence": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.cuttingPower); break;
            case "cutting bars": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.cuttingPower/2); break;
            case "unscrewing vent": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.ventBreakingPower); break;
            case "cutting vent": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.cuttingPower); break;
            case "unscrewing slats": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.ventBreakingPower); break;
            case "cutting slats": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.cuttingPower); break;
            case "digging down": RemoveTileDurability(touchedTileObject, touchedTileObject.GetComponent<TileCollectionData>().tileData.currentDurability, usedItemData.diggingPower); break;
        }

    }
    public void CreateActionText(string text)
    {
        ActionTextBox.text = text;
    }
    public void DestroyActionBar()
    {
        barIsMoving = false;
        cancelBar = true;
        foreach (Transform child in actionBarPanel.GetComponent<Transform>())
        {
            Destroy(child.gameObject);
        }
        InventoryCanvas.transform.Find("ActionBar").GetComponent<Image>().enabled = false;
        ActionTextBox.text = "";

    }
    public void BreakItem()
    {

        inventoryList[usedSlotNumber].itemData = null;
        switch (usedSlotNumber)
        {
            case 0: InventoryCanvas.transform.Find("GUIPanel").Find("Slot1").GetComponent<Image>().sprite = clearSprite; break;
            case 1: InventoryCanvas.transform.Find("GUIPanel").Find("Slot2").GetComponent<Image>().sprite = clearSprite; break;
            case 2: InventoryCanvas.transform.Find("GUIPanel").Find("Slot3").GetComponent<Image>().sprite = clearSprite; break;
            case 3: InventoryCanvas.transform.Find("GUIPanel").Find("Slot4").GetComponent<Image>().sprite = clearSprite; break;
            case 4: InventoryCanvas.transform.Find("GUIPanel").Find("Slot5").GetComponent<Image>().sprite = clearSprite; break;
            case 5: InventoryCanvas.transform.Find("GUIPanel").Find("Slot6").GetComponent<Image>().sprite = clearSprite; break;
        }
    }
    public void BreakTile()
    {
        if(whatAction == "unscrewing vent" || whatAction == "cutting vent")
        {
            Vector3 ventPosition = new Vector3(touchedTileObject.transform.position.x, touchedTileObject.transform.position.y);
            Quaternion ventRotation = Quaternion.identity;
            Destroy(touchedTileObject);
            Instantiate(emptyVentCover, ventPosition, ventRotation, perksTiles.transform.Find("VentObjects"));

            if(PlayerTransform.gameObject.layer == 15)
            {
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
        }
        else if(whatAction == "unscrewing slats" || whatAction == "cutting slats")
        {
            Vector3 slatsPosition = new Vector3(touchedTileObject.transform.position.x, touchedTileObject.transform.position.y);
            Quaternion slatsRotation = Quaternion.identity;
            Destroy(touchedTileObject);
        }
        else if(whatAction == "digging down")
        {
            touchedTileObject.GetComponent<BoxCollider2D>().enabled = false;

            Vector3 holePosition = new Vector3(touchedTileObject.transform.position.x, touchedTileObject.transform.position.y);
            Quaternion holeRotation = Quaternion.identity;
            GameObject holeObject = Resources.Load<GameObject>("PerksPrefabs/Objects/100%HoleDown");
            GameObject holeUpObject = Resources.Load<GameObject>("PerksPrefabs/Objects/100%HoleUp");
            Instantiate(holeObject, holePosition, holeRotation, perksTiles.transform.Find("GroundObjects"));
            Instantiate(holeUpObject, holePosition, holeRotation, perksTiles.transform.Find("UndergroundObjects"));
        }
        else
        {
            Vector3 tilePosition = new Vector3(touchedTileObject.transform.position.x, touchedTileObject.transform.position.y);
            Quaternion rotation = Quaternion.identity;
            Destroy(touchedTileObject);
            Instantiate(emptyTile, tilePosition, rotation, perksTiles.transform.Find("Ground"));
        }
    }
}
