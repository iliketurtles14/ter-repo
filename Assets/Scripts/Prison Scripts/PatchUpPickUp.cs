using JetBrains.Annotations;
using NavMeshPlus.Components;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatchUpPickUp : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Inventory inventoryScript;
    private Transform tiles;
    private ItemDataCreator creator;
    private List<GameObject> invSlots = new List<GameObject>();
    private Sprite clear;
    private Particles particlesScript;
    private WarningMessage warningScript;
    private MakeBadObject mbo;
    private Transform player;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        inventoryScript = GetComponent<Inventory>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        creator = GetComponent<ItemDataCreator>();
        clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        particlesScript = GetComponent<Particles>();
        warningScript = GetComponent<WarningMessage>();
        mbo = GetComponent<MakeBadObject>();
        player = RootObjectCache.GetRoot("Player").transform;
        foreach(Transform slot in RootObjectCache.GetRoot("InventoryCanvas").transform.Find("GUIPanel"))
        {
            invSlots.Add(slot.gameObject);
        }
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(1) && mcs.isTouchingPatchUp)
        {
            bool invIsFull = true;
            for(int i = 0; i < 6; i++)
            {
                if (inventoryScript.inventory[i].itemData == null)
                {
                    invIsFull = false;
                    break;
                }
            }
            int layer = 1;
            switch (mcs.touchedPatchUp.transform.parent.name)
            {
                case "GroundObjects":
                    layer = 1;
                    break;
                case "UndergroundObjects":
                    layer = 0;
                    break;
                case "VentObjects":
                    layer = 2;
                    break;
                case "RoofObjects":
                    layer = 3;
                    break;
            }
            int id = 0;
            switch (mcs.touchedPatchUp.name)
            {
                case "FakeWallBlock":
                    id = 97;
                    break;
                case "Poster":
                    id = 124;
                    break;
                case "FakeVent":
                    id = 96;
                    break;
            }
            ItemData data = creator.CreateItemData(id);
            bool breakItem = false;
            if (data.durability != -1)
            {
                data.currentDurability = mcs.touchedPatchUp.GetComponent<PatchUpHandler>().durability;
                if(data.currentDurability <= 0)
                {
                    breakItem = true;
                    StartCoroutine(warningScript.CreateWarningMessage("The " + data.displayName.ToLower() + "breaks..."));
                }
            }

            if (invIsFull && !breakItem)
            {
                GameObject itemObj = Instantiate(data.prefab);
                itemObj.transform.position = mcs.touchedPatchUp.transform.position;
                itemObj.GetComponent<ItemCollectionData>().itemData = data;
                itemObj.GetComponent<SpriteRenderer>().sprite = data.sprite;
                switch (layer)
                {
                    case 0:
                        itemObj.transform.parent = tiles.Find("UndergroundObjects");
                        itemObj.layer = LayerMask.NameToLayer("Underground");
                        itemObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        itemObj.GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                        break;
                    case 1:
                        itemObj.transform.parent = tiles.Find("GroundObjects");
                        itemObj.layer = LayerMask.NameToLayer("Ground");
                        itemObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        itemObj.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                        break;
                    case 2:
                        itemObj.transform.parent = tiles.Find("VentObjects");
                        itemObj.layer = LayerMask.NameToLayer("Vents");
                        itemObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        itemObj.GetComponent<SpriteRenderer>().sortingLayerName = "Vents";
                        break;
                    case 3:
                        itemObj.transform.parent = tiles.Find("RoofObjects");
                        itemObj.layer = LayerMask.NameToLayer("Roof");
                        itemObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        itemObj.GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
                        break;
                }
            }
            else if(!invIsFull && !breakItem)
            {
                int emptySlot = 0;
                foreach (InventoryItem item in inventoryScript.inventory)
                {
                    if(item.itemData == null)
                    {
                        item.itemData = data;
                        break;
                    }
                    emptySlot++;
                }
                invSlots[emptySlot].GetComponent<Image>().sprite = data.sprite;
            }

            if (mcs.touchedPatchUp.name == "FakeVent")
            {
                GameObject vent = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/EmptyVentCover"));
                vent.name = "Vent";
                switch (layer)
                {
                    case 0:
                        vent.layer = LayerMask.NameToLayer("Underground");
                        vent.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        vent.GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                        vent.transform.parent = tiles.Find("Underground");
                        break;
                    case 1:
                        vent.layer = LayerMask.NameToLayer("Ground");
                        vent.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        vent.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                        vent.transform.parent = tiles.Find("Ground");
                        break;
                    case 2:
                        vent.layer = LayerMask.NameToLayer("Vents");
                        vent.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        vent.GetComponent<SpriteRenderer>().sortingLayerName = "Vents";
                        vent.transform.parent = tiles.Find("Vents");
                        break;
                    case 3:
                        vent.layer = LayerMask.NameToLayer("Roof");
                        vent.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        vent.GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
                        vent.transform.parent = tiles.Find("Roof");
                        break;
                }
                vent.transform.position = mcs.touchedPatchUp.transform.position;
                vent.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[138];
                if (!Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground")))
                {
                    Color color = vent.GetComponent<SpriteRenderer>().color;
                    color.a = .75f;
                    vent.GetComponent<SpriteRenderer>().color = color;
                }

                BadObjectData boData = new BadObjectData
                {
                    solitary = true,
                    attachedObject = vent
                };
                mbo.CreateBadObject(boData, "openVent");
            }
            else if (mcs.touchedPatchUp.name != "Poster")
            {
                GameObject emptyTile = new GameObject("BrokenTile");
                emptyTile.AddComponent<TileCollectionData>().tileData = new TileData();
                emptyTile.GetComponent<TileCollectionData>().tileData.tileType = "inFloor";
                emptyTile.AddComponent<BoxCollider2D>().isTrigger = true;
                emptyTile.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 1.6f);
                if (mcs.touchedPatchUp.GetComponent<PatchUpHandler>().hasHoleUnder)
                {
                    emptyTile.GetComponent<TileCollectionData>().tileData.currentDurability = mcs.touchedPatchUp.GetComponent<PatchUpHandler>().holeDurability;
                    if(emptyTile.GetComponent<TileCollectionData>().tileData.currentDurability <= 0)
                    {
                        emptyTile.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                else
                {
                    emptyTile.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                }
                emptyTile.GetComponent<TileCollectionData>().tileData.holeStability = -1;
                emptyTile.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PrisonResources/Object Sprites/EmptyTileSprite");
                emptyTile.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
                emptyTile.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);
                emptyTile.AddComponent<NavMeshModifier>();
                emptyTile.AddComponent<BrokenTileConnection>().connectedTile = mcs.touchedPatchUp.GetComponent<PatchUpHandler>().connectedTile;
                emptyTile.tag = "Digable";
                switch (layer)
                {
                    case 0:
                        emptyTile.layer = LayerMask.NameToLayer("Underground");
                        emptyTile.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        emptyTile.GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
                        emptyTile.transform.parent = tiles.Find("Underground");
                        break;
                    case 1:
                        emptyTile.layer = LayerMask.NameToLayer("Ground");
                        emptyTile.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        emptyTile.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                        emptyTile.transform.parent = tiles.Find("Ground");
                        break;
                    case 2:
                        emptyTile.layer = LayerMask.NameToLayer("Vents");
                        emptyTile.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        emptyTile.GetComponent<SpriteRenderer>().sortingLayerName = "Vents";
                        emptyTile.transform.parent = tiles.Find("Vents");
                        break;
                    case 3:
                        emptyTile.layer = LayerMask.NameToLayer("Roof");
                        emptyTile.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        emptyTile.GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
                        emptyTile.transform.parent = tiles.Find("Roof");
                        break;
                }
                emptyTile.transform.position = mcs.touchedPatchUp.transform.position;
                Vector3 renderVector = new Vector3(0, 0, -1);
                emptyTile.transform.position += renderVector;

                BadObjectData boData = new BadObjectData
                {
                    solitary = true,
                    attachedObject = emptyTile
                };
                mbo.CreateBadObject(boData, "openWall");
            }
            else if(mcs.touchedPatchUp.name == "Poster")
            {
                BadObjectData boData = new BadObjectData
                {
                    solitary = true,
                    attachedObject = mcs.touchedPatchUp.GetComponent<PatchUpHandler>().connectedTile
                };
                mbo.CreateBadObject(boData, "openWall");
            }

            if (mcs.touchedPatchUp.GetComponent<PatchUpHandler>().hasHoleUnder && layer == 1)
            {
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name.Contains("HoleDown"))
                    {
                        float distance = Vector2.Distance(obj.position, mcs.touchedPatchUp.transform.position);
                        if(distance < .01f)
                        {
                            obj.GetComponent<BoxCollider2D>().enabled = true;
                            obj.GetComponent<SpriteRenderer>().enabled = true;

                            GameObject boAttachment = new GameObject("BOAttachment");
                            boAttachment.transform.position = obj.position;
                            boAttachment.transform.parent = tiles.Find("GroundObjects");
                            BadObjectData aBOData = new BadObjectData
                            {
                                solitary = true,
                                attachedObject = boAttachment //this is super aids. super duper aids. this is js cuz i dont have a reference to the original tile at this point in the code unless if i do a search on all tiles which i dont wanna do so sorry!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            };
                            mbo.CreateBadObject(aBOData, "openHole");
                            break;
                        }
                    }
                }
            }

            particlesScript.CreateDust(mcs.touchedPatchUp.transform.position, 1, player.GetComponent<SpriteRenderer>().sortingLayerName);
            Destroy(mcs.touchedPatchUp);
        }
    }
}
