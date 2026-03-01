using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTransformers : MonoBehaviour //this is for job stuff like the plate maker and the laundry machine etc...
{
    private InventorySelection selectionScript;
    private Inventory inventoryScript;
    private MouseCollisionOnItems mcs;
    private Transform ic;
    private List<GameObject> busyTransformers = new List<GameObject>();
    private List<GameObject> doneTransformers = new List<GameObject>();
    private ApplyPrisonData applyScript;
    private ItemDataCreator creator;
    private bool invIsFull;
    private List<GameObject> invSlots = new List<GameObject>();
    private Sprite ovenEmpty;
    private Sprite ovenCooking1;
    private Sprite ovenCooking2;
    private Sprite ovenDone;
    private Sprite washerEmpty;
    private Sprite washerWashing1;
    private Sprite washerWashing2;
    private Sprite washerWashing3;
    private Sprite washerWashing4;
    private Sprite washerDone;
    private void Start()
    {
        selectionScript = GetComponent<InventorySelection>();
        inventoryScript = GetComponent<Inventory>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        applyScript = GetComponent<ApplyPrisonData>();
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        creator = GetComponent<ItemDataCreator>();

        foreach(Transform child in ic.Find("GUIPanel"))
        {
            invSlots.Add(child.gameObject);
        }
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        ovenEmpty = applyScript.PrisonObjectSprites[79];
        ovenCooking1 = applyScript.PrisonObjectSprites[80];
        ovenCooking2 = applyScript.PrisonObjectSprites[81];
        ovenDone = applyScript.PrisonObjectSprites[49];
        washerEmpty = applyScript.PrisonObjectSprites[82];
        washerWashing1 = applyScript.PrisonObjectSprites[83];
        washerWashing2 = applyScript.PrisonObjectSprites[84];
        washerWashing3 = applyScript.PrisonObjectSprites[85];
        washerWashing4 = applyScript.PrisonObjectSprites[86];
        washerDone = applyScript.PrisonObjectSprites[47];
    }
    private void Update()
    {
        invIsFull = true;
        for (int i = 0; i < 6; i++)
        {
            try
            {
                if (inventoryScript.inventory[i].itemData.sprite == null)
                {
                    invIsFull = false;
                    break;
                }
            }
            catch
            {
                invIsFull = false;
                break;
            }
        }

        if (mcs.isTouchingItemTransformer && selectionScript.aSlotSelected && Input.GetMouseButtonDown(0) &&
            !busyTransformers.Contains(mcs.touchedItemTransformer) && !doneTransformers.Contains(mcs.touchedItemTransformer))
        {
            string transName = mcs.touchedItemTransformer.name;
            int id = inventoryScript.inventory[selectionScript.selectedSlotNum].itemData.id;

            if (transName == "Oven" && (id == 147 || id == 203))
            {
                inventoryScript.inventory[selectionScript.selectedSlotNum].itemData = null;
                busyTransformers.Add(mcs.touchedItemTransformer);
                StartCoroutine(AnimateTransformer(mcs.touchedItemTransformer));

                switch (id)
                {
                    case 147:
                        mcs.touchedItemTransformer.GetComponent<ItemTransformerData>().heldID = 84;
                        break;
                    case 203:
                        mcs.touchedItemTransformer.GetComponent<ItemTransformerData>().heldID = 180;
                        break;
                }
            }
            else if (transName == "Washer" && (id == 38 || id == 37))
            {
                inventoryScript.inventory[selectionScript.selectedSlotNum].itemData = null;
                busyTransformers.Add(mcs.touchedItemTransformer);
                StartCoroutine(AnimateTransformer(mcs.touchedItemTransformer));

                switch (id)
                {
                    case 38:
                        mcs.touchedItemTransformer.GetComponent<ItemTransformerData>().heldID = 39;
                        break;
                    case 37:
                        mcs.touchedItemTransformer.GetComponent<ItemTransformerData>().heldID = 29;
                        break;
                }
            }
            else if(transName == "LicensePress" && id == 130)
            {
                inventoryScript.inventory[selectionScript.selectedSlotNum].itemData = creator.CreateItemData(182);
                invSlots[selectionScript.selectedSlotNum].GetComponent<Image>().sprite = creator.CreateItemData(182).sprite;
            }
        }
        else if (mcs.isTouchingItemTransformer && doneTransformers.Contains(mcs.touchedItemTransformer) &&
            !invIsFull)
        {
            for (int i = 0; i < 6; i++)
            {
                bool canPutIn = false;
                try
                {
                    canPutIn = inventoryScript.inventory[i].itemData == null;
                }
                catch
                {
                    canPutIn = true;
                }
                if (canPutIn)
                {
                    inventoryScript.inventory[i].itemData = creator.CreateItemData(mcs.touchedItemTransformer.GetComponent<ItemTransformerData>().heldID);
                    invSlots[i].GetComponent<Image>().sprite = inventoryScript.inventory[i].itemData.sprite;
                    doneTransformers.Remove(mcs.touchedItemTransformer);
                    busyTransformers.Remove(mcs.touchedItemTransformer);
                    mcs.touchedItemTransformer.GetComponent<ItemTransformerData>().heldID = -1;
                    switch (mcs.touchedItemTransformer.name)
                    {
                        case "Washer":
                            mcs.touchedItemTransformer.GetComponent<SpriteRenderer>().sprite = washerEmpty;
                            break;
                        case "Oven":
                            mcs.touchedItemTransformer.GetComponent<SpriteRenderer>().sprite = ovenEmpty;
                            break;
                    }
                    break;
                }
            }
        }
    }
    private IEnumerator AnimateTransformer(GameObject obj)
    {
        Sprite done = null;
        Sprite empty = null;
        switch (obj.name)
        {
            case "Washer":
                done = washerDone;
                empty = washerEmpty;
                for (int i = 0; i < 20; i++)
                {
                    obj.GetComponent<SpriteRenderer>().sprite = washerWashing1;
                    yield return new WaitForSeconds(.083f);
                    obj.GetComponent<SpriteRenderer>().sprite = washerWashing2;
                    yield return new WaitForSeconds(.083f);
                    obj.GetComponent<SpriteRenderer>().sprite = washerWashing3;
                    yield return new WaitForSeconds(.083f);
                    obj.GetComponent<SpriteRenderer>().sprite = washerWashing4;
                    yield return new WaitForSeconds(.083f);
                }
                break;
            case "Oven":
                done = ovenDone;
                empty = ovenEmpty;
                for(int i = 0; i < 40; i++)
                {
                    obj.GetComponent<SpriteRenderer>().sprite = ovenCooking1;
                    yield return new WaitForSeconds(.083f);
                    obj.GetComponent<SpriteRenderer>().sprite = ovenCooking2;
                    yield return new WaitForSeconds(.083f);
                }
                break;
        }
        doneTransformers.Add(obj);
        while (doneTransformers.Contains(obj))
        {
            obj.GetComponent<SpriteRenderer>().sprite = done;
            float time = 0f;
            while(time < .3f && doneTransformers.Contains(obj))
            {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            obj.GetComponent<SpriteRenderer>().sprite = empty;
            time = 0f;
            while(time < .3f && doneTransformers.Contains(obj))
            {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
