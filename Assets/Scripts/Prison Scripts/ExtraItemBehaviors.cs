using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

public class ExtraItemBehaviors : MonoBehaviour
{
    private InventorySelection selectionScript;
    private int selectedItemID;
    private Inventory inventoryScript;
    private MouseCollisionOnItems mcs;
    private List<GameObject> invSlots = new List<GameObject>();
    private Transform ic;
    private Sprite clear;
    private Transform tiles;
    private Death deathScript;
    private ItemDataCreator creator;
    private Transform player;
    private Particles particlesScript;
    private DeskStand deskStandScript;
    private StatEffects statEffectsScript;
    private MakeBadObject mbo;
    private Transform badObjects;
    private void Start()
    {
        selectionScript = GetComponent<InventorySelection>();
        inventoryScript = GetComponent<Inventory>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        deathScript = GetComponent<Death>();
        creator = GetComponent<ItemDataCreator>();
        player = RootObjectCache.GetRoot("Player").transform;
        particlesScript = GetComponent<Particles>();
        deskStandScript = GetComponent<DeskStand>();
        statEffectsScript = GetComponent<StatEffects>();
        mbo = GetComponent<MakeBadObject>();
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;
        foreach(Transform slot in ic.Find("GUIPanel"))
        {
            invSlots.Add(slot.gameObject);
        }
    }
    private void Update()
    {
        if (!selectionScript.aSlotSelected)
        {
            return;
        }
        else
        {
            try
            {
                selectedItemID = inventoryScript.inventory[selectionScript.selectedSlotNum].itemData.id;
            }
            catch
            {
                //ts causes an error for no reason sometimes
            }
        }

        if(mcs.isTouchingBars && Input.GetMouseButtonDown(0) && selectedItemID == 76)
        {
            if(Vector2.Distance(player.transform.position, mcs.touchedBars.transform.position) <= 2.4f)
            {
                PlaceBedSheet(selectionScript.selectedSlotNum, mcs.touchedBars);
            }
        }
        else if(mcs.isTouchingSittable && mcs.touchedSittable.name.StartsWith("PlayerBed") && Input.GetMouseButtonDown(0) && selectedItemID == 75)
        {
            if (Vector2.Distance(player.transform.position, mcs.touchedSittable.transform.position) <= 2.4f)
            {
                PlaceBedDummy(selectionScript.selectedSlotNum, mcs.touchedSittable);
            }
        }
        else if(mcs.isTouchingFloor && !Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground")) && Input.GetMouseButtonDown(0) && selectedItemID == 137)
        {
            if(Vector2.Distance(player.transform.position, mcs.touchedFloor.transform.position) <= 2.4f)
            {
                PlaceStepladder(selectionScript.selectedSlotNum, mcs.touchedFloor);
            }
        }
        else if(mcs.isTouchingNPC && !mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead && Input.GetMouseButtonDown(0) && (selectedItemID == 87 || selectedItemID == 240 || selectedItemID == 237))
        {
            if (Vector2.Distance(player.transform.position, mcs.touchedNPC.transform.position) <= 2.4f)
            {
                InstaKO(selectionScript.selectedSlotNum, mcs.touchedNPC);
            }
        }
        else if(mcs.isTouchingPlayer && (creator.CreateItemData(selectedItemID).health != -1 || creator.CreateItemData(selectedItemID).energy != -1) && Input.GetMouseButtonDown(0))
        {
            Eat(selectionScript.selectedSlotNum);
        }
        else if(mcs.isTouchingFloor && mcs.touchedFloor.name == "BrokenTile" && mcs.touchedFloor.GetComponent<BrokenTileConnection>().connectedTile.CompareTag("Wall") && Input.GetMouseButtonDown(0) && (selectedItemID == 154 || selectedItemID == 97 || selectedItemID == 124))
        {
            if (Vector2.Distance(player.transform.position, mcs.touchedFloor.transform.position) <= 2.4f)
            {
                PlacePatchUp("wall", selectionScript.selectedSlotNum, mcs.touchedFloor);
            }
        }
        else if((mcs.isTouchingHoleDown || mcs.isTouchingHoleUp) && Input.GetMouseButtonDown(0) && (selectedItemID == 123 || selectedItemID == 265))
        {
            if (mcs.isTouchingHoleDown)
            {
                if(Vector2.Distance(player.position, mcs.touchedHoleDown.transform.position) <= 2.4f)
                {
                    PlacePatchUp("hole", selectionScript.selectedSlotNum, mcs.touchedHoleDown);
                }
            }
            else if (mcs.isTouchingHoleUp)
            {
                if (Vector2.Distance(player.position, mcs.touchedHoleUp.transform.position) <= 2.4f)
                {
                    PlacePatchUp("hole", selectionScript.selectedSlotNum, mcs.touchedHoleUp);
                }
            }
        }
        else if((mcs.isTouchingEmptyDirt || mcs.isTouchingFloor) && Input.GetMouseButtonDown(0) && (selectedItemID == 123 || selectedItemID == 265))
        {
            if (mcs.isTouchingEmptyDirt)
            {
                if(Vector2.Distance(player.position, mcs.touchedEmptyDirt.transform.position) <= 2.4f)
                {
                    foreach (Transform obj in tiles.Find("UndergroundObjects"))
                    {
                        if (obj.name.Contains("HoleUp"))
                        {
                            float distance = Vector2.Distance(obj.position, mcs.touchedEmptyDirt.transform.position);
                            if (distance < .01f)
                            {
                                PlacePatchUp("hole", selectionScript.selectedSlotNum, obj.gameObject);
                            }
                        }
                    }
                }
            }
            else if (mcs.isTouchingFloor)
            {
                if(Vector2.Distance(player.position, mcs.touchedFloor.transform.position) <= 2.4f)
                {
                    foreach (Transform obj in tiles.Find("GroundObjects"))
                    {
                        if (obj.name.Contains("HoleDown"))
                        {
                            float distance = Vector2.Distance(obj.position, mcs.touchedFloor.transform.position);
                            if (distance < .01f)
                            {
                                PlacePatchUp("hole", selectionScript.selectedSlotNum, obj.gameObject);
                            }
                        }
                    }
                }
            }
        }
        else if(mcs.isTouchingOpenVent && Input.GetMouseButtonDown(0) && (selectedItemID == 151 || selectedItemID == 96))
        {
            if(Vector2.Distance(player.position, mcs.touchedOpenVent.transform.position) <= 2.4f)
            {
                PlacePatchUp("vent", selectionScript.selectedSlotNum, mcs.touchedOpenVent);
            }
        }
        else if(mcs.isTouchingFloor && mcs.touchedFloor.name == "BrokenTile" && mcs.touchedFloor.GetComponent<BrokenTileConnection>().connectedTile.CompareTag("Fence") && Input.GetMouseButtonDown(0) && selectedItemID == 95)
        {
            if(Vector2.Distance(player.position, mcs.touchedFloor.transform.position) <= 2.4f)
            {
                PlacePatchUp("fence", selectionScript.selectedSlotNum, mcs.touchedFloor);
            }
        }
        else if(mcs.isTouchingNPC && Input.GetMouseButtonDown(0) && (selectedItemID == 127 || selectedItemID == 105 || selectedItemID == 131))
        {
            if (mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead)
            {
                if(Vector2.Distance(player.position, mcs.touchedNPC.transform.position) <= 2.4f)
                {
                    TieUpNPC(selectionScript.selectedSlotNum, mcs.touchedNPC);
                }
            }
        }
        else if((mcs.isTouchingWall || mcs.isTouchingFence || mcs.isTouchingElectricFence || mcs.isTouchingBars) &&
            Input.GetMouseButtonDown(0) && (selectedItemID == 82 || selectedItemID == 142))
        {
            GameObject obj = null;
            if (mcs.isTouchingWall)
            {
                obj = mcs.touchedWall;
            }
            else if (mcs.isTouchingFence)
            {
                obj = mcs.touchedFence;
            }
            else if (mcs.isTouchingElectricFence)
            {
                obj = mcs.touchedElectricFence;
            }
            else if (mcs.isTouchingBars)
            {
                obj = mcs.touchedBars;
            }
            if(Vector2.Distance(player.position, obj.transform.position) <= 2.4f)
            {
                MakeShiv(selectionScript.selectedSlotNum, obj.transform.position);
            }
        }
        else if(mcs.isTouchingCamera && Input.GetMouseButtonDown(0) && selectedItemID != -1)
        {
            if(creator.CreateItemData(selectedItemID).cameraBlock != -1 && mcs.touchedCamera.GetComponent<CameraController>().camMode != 2)
            {
                if(Vector2.Distance(player.position, mcs.touchedCamera.transform.position) <= 2.4f)
                {
                    BlockCamera(selectionScript.selectedSlotNum, mcs.touchedCamera);
                }
            }
        }
    }
    private void TieUpNPC(int slot, GameObject npc)
    {
        npc.GetComponent<NPCCollectionData>().npcData.isTied = true;
        inventoryScript.inventory[slot].itemData = null;
        invSlots[slot].GetComponent<Image>().sprite = clear;
        particlesScript.CreateDust(npc.transform.position, 1);
        BadObjectData data = new BadObjectData
        {
            untie = true,
            attachedObject = npc
        };
        mbo.CreateBadObject(data, "untie");
    }
    private void PlaceBedSheet(int slot, GameObject bars)
    {
        inventoryScript.inventory[slot].itemData = null;
        invSlots[slot].GetComponent<Image>().sprite = clear;

        GameObject sheet = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Sheet"));
        sheet.name = "Sheet";
        sheet.transform.parent = tiles.Find("GroundObjects");
        sheet.transform.position = new Vector3(bars.transform.position.x, bars.transform.position.y, -1);
        sheet.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[173];

        BadObjectData data = new BadObjectData
        {
            sheets = true,
            attachedObject = sheet,
            messageType = "Sheets"
        };
        mbo.CreateBadObject(data, "sheets");
    }
    private void PlaceBedDummy(int slot, GameObject bed)
    {
        inventoryScript.inventory[slot].itemData = null;
        invSlots[slot].GetComponent<Image>().sprite = clear;

        string dummyName;
        Sprite dummySprite;
        switch (bed.name)
        {
            case "PlayerBedHorizontal":
                dummyName = "DummyHorizontal";
                dummySprite = DataSender.instance.PrisonObjectImages[263];
                break;
            case "PlayerBedVertical":
                dummyName = "DummyVertical";
                dummySprite = DataSender.instance.PrisonObjectImages[249];
                break;
            default:
                dummyName = "DummyVertical";
                dummySprite = DataSender.instance.PrisonObjectImages[249];
                break;
        }

        GameObject dummy = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/" + dummyName));
        dummy.name = dummyName;
        dummy.transform.parent = tiles.Find("GroundObjects");
        dummy.transform.position = new Vector3(bed.transform.position.x, bed.transform.position.y, -1);
        dummy.GetComponent<SpriteRenderer>().sprite = dummySprite;
    }
    private void PlacePatchUp(string type, int slot, GameObject obj) //wall, vent, hole, fence
    {
        particlesScript.CreateDust(obj.transform.position, 1);

        int id = inventoryScript.inventory[slot].itemData.id;
        string layerTilesName = "Ground";
        string layerObjectsName = "GroundObjects";
        int layerTilesSR = 2;
        int layerObjectsSR = 3;
        switch (obj.transform.parent.name)
        {
            case "Ground":
            case "GroundObjects":
                layerTilesName = "Ground";
                layerObjectsName = "GroundObjects";
                layerTilesSR = 2;
                layerObjectsSR = 3;
                break;
            case "Underground":
            case "UndergroundObjects":
                layerTilesName = "Underground";
                layerObjectsName = "UndergroundObjects";
                layerTilesSR = -5;
                layerObjectsSR = -4;
                break;
            case "Vents":
            case "VentObjects":
                layerTilesName = "Vents";
                layerObjectsName = "VentObjects";
                layerTilesSR = 9;
                layerObjectsSR = 10;
                break;
            case "Roof":
            case "RoofObjects":
                layerTilesName = "Roof";
                layerObjectsName = "RoofObjects";
                layerTilesSR = 13;
                layerObjectsSR = 14;
                break;
        }
        switch (type)//fake wall blocks and fake vent covers have 10 uses
                     //vent cover has 5% after replacing and wall blocks and fake fences have 10%
                     //when putting dirt back, make sure to have the little dirt thingy
                     //when putting stuff back, do the dust particles
        {
            case "wall":
                foreach(Transform bo in badObjects)
                {
                    if(Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, obj.transform.position) <= .1f && bo.name == "openWall")
                    {
                        Destroy(bo.gameObject);
                        break;
                    }
                }
                switch (id)
                {
                    case 124://poster
                        GameObject poster = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Poster"));
                        poster.name = "Poster";
                        poster.transform.position = obj.transform.position;
                        poster.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[180];
                        poster.transform.parent = tiles.Find(layerObjectsName);
                        poster.layer = LayerMask.NameToLayer(layerTilesName);
                        poster.GetComponent<SpriteRenderer>().sortingOrder = layerObjectsSR;
                        poster.GetComponent<PatchUpHandler>().connectedTile = obj.GetComponent<BrokenTileConnection>().connectedTile;
                        PatchUpHandler handler = poster.GetComponent<PatchUpHandler>();
                        foreach (Transform aObj in tiles.Find("GroundObjects"))
                        {
                            if (aObj.name.Contains("HoleDown") && layerTilesName == "Ground")
                            {
                                float distance = Vector2.Distance(aObj.position, poster.transform.position);
                                if (distance < .01f)
                                {
                                    handler.hasHoleUnder = true;
                                    handler.holeDurability = obj.GetComponent<TileCollectionData>().tileData.currentDurability;
                                    aObj.GetComponent<BoxCollider2D>().enabled = false;
                                    aObj.GetComponent<SpriteRenderer>().enabled = false;
                                    foreach (Transform bo in badObjects)
                                    {
                                        if (Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, obj.transform.position) <= .1f && bo.name == "openHole")
                                        {
                                            Destroy(bo.gameObject);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                handler.hasHoleUnder = false;
                            }
                        }
                        break;
                    case 97://fake wall block
                        GameObject fakeWallBlock = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/FakeWallBlock"));
                        fakeWallBlock.name = "FakeWallBlock";
                        fakeWallBlock.transform.position = obj.transform.position;
                        fakeWallBlock.transform.parent = tiles.Find(layerTilesName);
                        fakeWallBlock.layer = LayerMask.NameToLayer(layerTilesName);
                        fakeWallBlock.GetComponent<PatchUpHandler>().connectedTile = obj.GetComponent<BrokenTileConnection>().connectedTile;
                        handler = fakeWallBlock.GetComponent<PatchUpHandler>();
                        handler.durability = inventoryScript.inventory[slot].itemData.currentDurability - 10;
                        foreach (Transform aObj in tiles.Find("GroundObjects"))
                        {
                            if (aObj.name.Contains("HoleDown") && layerTilesName == "Ground")
                            {
                                float distance = Vector2.Distance(aObj.position, fakeWallBlock.transform.position);
                                if (distance < .01f)
                                {
                                    handler.hasHoleUnder = true;
                                    handler.holeDurability = obj.GetComponent<TileCollectionData>().tileData.currentDurability;
                                    aObj.GetComponent<BoxCollider2D>().enabled = false;
                                    aObj.GetComponent<SpriteRenderer>().enabled = false;
                                    foreach (Transform bo in badObjects)
                                    {
                                        if (Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, obj.transform.position) <= .1f && bo.name == "openHole")
                                        {
                                            Destroy(bo.gameObject);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                handler.hasHoleUnder = false;
                            }
                        }
                        Destroy(obj);
                        break;
                    case 154://wall block
                        GameObject connectedTile = obj.GetComponent<BrokenTileConnection>().connectedTile;
                        connectedTile.GetComponent<BoxCollider2D>().enabled = true;
                        connectedTile.GetComponent<TileCollectionData>().tileData.currentDurability = 10;
                        TileCollectionData aTCD = connectedTile.GetComponent<TileCollectionData>();
                        foreach(Transform aObj in tiles.Find("GroundObjects"))
                        {
                            if (aObj.name.Contains("HoleDown") && layerTilesName == "Ground")
                            {
                                float distance = Vector2.Distance(aObj.position, connectedTile.transform.position);
                                if(distance < .01f)
                                {
                                    aTCD.tileData.holeIsUnder = true;
                                    aTCD.tileData.holeDurability = obj.GetComponent<TileCollectionData>().tileData.currentDurability;
                                    aObj.GetComponent<BoxCollider2D>().enabled = false;
                                    aObj.GetComponent<SpriteRenderer>().enabled = false;
                                    foreach (Transform bo in badObjects)
                                    {
                                        if (Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, obj.transform.position) <= .1f && bo.name == "openHole")
                                        {
                                            Destroy(bo.gameObject);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                aTCD.tileData.holeIsUnder = false;
                            }
                        }
                        Destroy(obj);
                        break;
                }
                break;
            case "vent":
                foreach (Transform bo in badObjects)
                {
                    if (Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, obj.transform.position) <= .1f && bo.name == "openVent")
                    {
                        Destroy(bo.gameObject);
                        break;
                    }
                }
                switch (id)
                {
                    case 151://vent cover
                        GameObject vent = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Vent"));
                        vent.name = "Vent";
                        vent.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[137];
                        vent.GetComponent<TileCollectionData>().tileData = new TileData();
                        vent.GetComponent<TileCollectionData>().tileData.tileType = null;
                        vent.GetComponent<TileCollectionData>().tileData.currentDurability = 5;
                        vent.GetComponent<TileCollectionData>().tileData.holeStability = -1;
                        vent.GetComponent<SpriteRenderer>().sortingOrder = layerObjectsSR;
                        vent.layer = LayerMask.NameToLayer("VentCovers");
                        vent.transform.position = obj.transform.position;
                        vent.transform.parent = tiles.Find(layerObjectsName);
                        Destroy(obj);
                        break;
                    case 96://fake vent
                        GameObject fakeVent = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/FakeVent"));
                        fakeVent.name = "FakeVent";
                        fakeVent.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[150];
                        fakeVent.GetComponent<SpriteRenderer>().sortingOrder = layerObjectsSR;
                        fakeVent.layer = LayerMask.NameToLayer("VentCovers");
                        fakeVent.transform.position = obj.transform.position;
                        fakeVent.transform.parent = tiles.Find(layerObjectsName);
                        fakeVent.GetComponent<PatchUpHandler>().durability = inventoryScript.inventory[slot].itemData.currentDurability - 10;
                        Destroy(obj);
                        break;
                }
                break;
            case "hole":
                foreach (Transform bo in badObjects)
                {
                    if (Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, obj.transform.position) <= .1f && bo.name == "openHole")
                    {
                        Destroy(bo.gameObject);
                        break;
                    }
                }
                GameObject dirtCrumbs = new GameObject("DirtCrumbs");
                dirtCrumbs.transform.parent = tiles.Find("GroundObjects");
                dirtCrumbs.AddComponent<SpriteRenderer>().sprite = DataSender.instance.UIImages[178];
                dirtCrumbs.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
                dirtCrumbs.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);
                dirtCrumbs.GetComponent<SpriteRenderer>().sortingOrder = 3;
                dirtCrumbs.transform.position = obj.transform.position;
                dirtCrumbs.layer = LayerMask.NameToLayer("Ground");
                Vector3 renderVector = new Vector3(0, 0, 1); //to make it behind a new hole object
                dirtCrumbs.transform.position += renderVector;

                foreach(Transform tile in tiles.Find("Ground"))
                {
                    if(tile.position == obj.transform.position && tile.CompareTag("Digable"))
                    {
                        tile.GetComponent<BoxCollider2D>().enabled = true;
                        tile.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                        break;
                    }
                }
                foreach(Transform tile in tiles.Find("UndergroundObjects"))
                {
                    if(tile.position == obj.transform.position && tile.CompareTag("EmptyDirt"))
                    {
                        tile.GetComponent<BoxCollider2D>().enabled = true;
                        tile.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                    }
                }

                foreach (Transform tile in tiles.Find("GroundObjects"))
                {
                    if (tile.name.Contains("HoleDown") && new Vector2(tile.position.x, tile.position.y) == new Vector2(obj.transform.position.x, obj.transform.position.y))
                    {
                        Destroy(tile.gameObject);
                        break;
                    }
                }
                foreach (Transform tile in tiles.Find("UndergroundObjects"))
                {
                    if (tile.name.Contains("HoleUp") && new Vector2(tile.position.x, tile.position.y) == new Vector2(obj.transform.position.x, obj.transform.position.y))
                    {
                        Destroy(tile.gameObject);
                        break;
                    }
                }
                break;
            case "fence":
                foreach (Transform bo in badObjects)
                {
                    if (Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, obj.transform.position) <= .1f && bo.name == "openFence")
                    {
                        Destroy(bo.gameObject);
                        break;
                    }
                }
                GameObject aConnectedTile = obj.GetComponent<BrokenTileConnection>().connectedTile;
                aConnectedTile.GetComponent<BoxCollider2D>().enabled = true;
                aConnectedTile.GetComponent<TileCollectionData>().tileData.currentDurability = 10;
                TileCollectionData tcd = aConnectedTile.GetComponent<TileCollectionData>();
                foreach (Transform aObj in tiles.Find("GroundObjects"))
                {
                    if (aObj.name.Contains("HoleDown") && layerTilesName == "Ground")
                    {
                        float distance = Vector2.Distance(aObj.position, aConnectedTile.transform.position);
                        if (distance < .01f)
                        {
                            aConnectedTile.GetComponent<TileCollectionData>().tileData.holeIsUnder = true;
                            aConnectedTile.GetComponent<TileCollectionData>().tileData.holeDurability = obj.GetComponent<TileCollectionData>().tileData.currentDurability;
                            aObj.GetComponent<BoxCollider2D>().enabled = false;
                            aObj.GetComponent<SpriteRenderer>().enabled = false;
                            foreach(Transform bo in badObjects)
                            {
                                if (Vector2.Distance(bo.GetComponent<BadObjectData>().attachedObject.transform.position, obj.transform.position) <= .1f && bo.name == "openHole")
                                {
                                    Destroy(bo.gameObject);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        tcd.tileData.holeIsUnder = false;
                    }
                }
                Destroy(obj);
                break;
        }

        invSlots[slot].GetComponent<Image>().sprite = clear;
        inventoryScript.inventory[slot].itemData = null;
    }
    private void BlockCamera(int slot, GameObject cam)
    {
        StartCoroutine(cam.GetComponent<CameraController>().TurnOffCam(inventoryScript.inventory[slot].itemData.cameraBlock, false));
        particlesScript.CreateDust(cam.transform.position, 1);
        inventoryScript.inventory[slot].itemData.currentDurability -= inventoryScript.inventory[slot].itemData.durability;
        if (inventoryScript.inventory[slot].itemData.currentDurability <= 0)
        {
            inventoryScript.inventory[slot].itemData = null;
            invSlots[slot].GetComponent<SpriteRenderer>().sprite = clear;
            
        }
    }
    private void PlaceStepladder(int slot, GameObject floor)
    {
        inventoryScript.inventory[slot].itemData = null;
        invSlots[slot].GetComponent<Image>().sprite = clear;

        particlesScript.CreateDust(floor.transform.position, 1);

        GameObject sl = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Stepladder"));
        sl.name = "Stepladder";
        sl.transform.parent = tiles.Find("GroundObjects");
        sl.transform.position = floor.transform.position;
        sl.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[154];

        BadObjectData data = new BadObjectData
        {
            stepladder = true,
            attachedObject = sl
        };
        mbo.CreateBadObject(data, "stepladder");
    }
    private void Eat(int slot)
    {
        ItemData data = inventoryScript.inventory[slot].itemData;
        int health = data.health;
        int energy = data.energy;

        inventoryScript.inventory[slot].itemData = null;
        invSlots[slot].GetComponent<Image>().sprite = clear;

        if(health != -1)
        {
            player.GetComponent<PlayerCollectionData>().playerData.health += health;
            StartCoroutine(statEffectsScript.MakeEffect(player, "health"));
        }
        if(energy != -1)
        {
            player.GetComponent<PlayerCollectionData>().playerData.energy -= energy;
            StartCoroutine(statEffectsScript.MakeEffect(player, "energy"));
        }
    }
    private void InstaKO(int slot, GameObject npc)
    {
        inventoryScript.inventory[slot].itemData = null;
        invSlots[slot].GetComponent<Image>().sprite = clear;
        particlesScript.CreateDust(npc.transform.position, 1);
        deathScript.KillNPC(npc);
    }
    private void MakeShiv(int slot, Vector2 pos)
    {
        int id = inventoryScript.inventory[slot].itemData.id;
        int idToMake = 0;
        switch (id)
        {
            case 82:
                idToMake = 59;
                break;
            case 142:
                idToMake = 69;
                break;
        }
        ItemData data = creator.CreateItemData(idToMake);
        inventoryScript.inventory[slot].itemData = data;
        invSlots[slot].GetComponent<Image>().sprite = data.sprite;
        particlesScript.CreateDust(pos, 1);
    }
}
 