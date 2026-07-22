using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stashes : MonoBehaviour
{
    private List<GameObject> stashes = new List<GameObject>();
    private List<string> objLayers = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private Transform tiles;
    private bool ready;
    private MouseCollisionOnItems mcs;
    private Transform player;
    private Inventory invScript;
    private List<Transform> invSlots = new List<Transform>();
    private ItemDataCreator creator;
    private List<int> items = new List<int>
    {
        20, 25, 105, 9, 24, 7, 6, 5, 8, 11, 97, 96, 13, 16, 85, 83
    };
    private MakeBadObject mbo;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        foreach(Transform slot in RootObjectCache.GetRoot("InventoryCanvas").transform.Find("GUIPanel"))
        {
            invSlots.Add(slot);
        }
        mbo = GetComponent<MakeBadObject>();
        creator = GetComponent<ItemDataCreator>();
        invScript = GetComponent<Inventory>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(objLayers[i]))
            {
                if(obj.name == "Stash")
                {
                    obj.gameObject.SetActive(false);
                    stashes.Add(obj.gameObject);
                }
            }
        }
        if(stashes.Count > 0)
        {
            SetStashes();
        }
    }
    private void SetStashes()
    {
        int stashesToAdd = UnityEngine.Random.Range(0, 10) + 5;
        for(int i = 0; i < stashesToAdd; i++)
        {
            int rand = UnityEngine.Random.Range(0, stashes.Count);
            stashes[rand].SetActive(true);
            stashes.RemoveAt(rand);
        }
        foreach(GameObject stash in stashes)
        {
            Destroy(stash);
        }
        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if(mcs.isTouchingStash && Input.GetMouseButtonDown(0))
        {
            if(Vector2.Distance(player.position, mcs.touchedStash.transform.position) <= 2.4f)
            {
                PickupStash(mcs.touchedStash);
            }
        }
    }
    private void PickupStash(GameObject stash)
    {
        PSoundController.PlaySound("pickup");
        
        bool invIsFull = true;
        for(int i = 0; i < 6; i++)
        {
            if (invScript.inventory[i].itemData == null)
            {
                invIsFull = false;
                break;
            }
        }

        ItemData data = creator.CreateItemData(items[UnityEngine.Random.Range(0, items.Count)]);
        if (invIsFull)
        {
            string parent = LayerMask.LayerToName(stash.layer) + "Objects";
            if(LayerMask.LayerToName(stash.layer) == "Vents")
            {
                parent = "VentObjects";
            }
            GameObject droppedItem = Instantiate(data.prefab, stash.transform.position, Quaternion.identity, tiles.Find(parent));
            droppedItem.GetComponent<ItemCollectionData>().itemData = data;
            droppedItem.GetComponent<SpriteRenderer>().sprite = data.sprite;
            droppedItem.GetComponent<SpriteRenderer>().sortingOrder = 2;
            droppedItem.GetComponent<SpriteRenderer>().sortingLayerName = LayerMask.LayerToName(stash.layer);
            if (data.isContraband)
            {
                BadObjectData aData = new BadObjectData
                {
                    item = true,
                    attachedObject = droppedItem
                };
                mbo.CreateBadObject(aData, "item");
            }
        }
        else
        {
            for(int i = 0; i < 6; i++)
            {
                if (invScript.inventory[i].itemData == null)
                {
                    invScript.inventory[i].itemData = data;
                    invSlots[i].GetComponent<Image>().sprite = data.sprite;
                    break;
                }
            }
        }
        stash.GetComponent<BoxCollider2D>().enabled = false;
        //animate
        StartCoroutine(OpenAnim(stash));
    }
    private IEnumerator OpenAnim(GameObject stash)
    {
        stash.GetComponent<StashAnim>().shouldStop = true;
        Sprite s = DataSender.instance.PrisonObjectImages[228];
        SpriteRenderer sr = stash.GetComponent<SpriteRenderer>();
        //5 , .15
        sr.sprite = s;
        for (int i = 0; i < 5; i++)
        {
            sr.enabled = true;
            yield return new WaitForSeconds(.15f);
            sr.enabled = false;
            yield return new WaitForSeconds(.15f);
        }
        Destroy(stash);
    }
}
