using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepladderPickup : MonoBehaviour
{
    private Transform player;
    private MouseCollisionOnItems mcs;
    private Inventory invScript;
    private Particles particlesScript;
    private ItemDataCreator creator;
    private List<GameObject> slots = new List<GameObject>();
    private Transform badObjects;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        invScript = GetComponent<Inventory>();
        particlesScript = GetComponent<Particles>();
        creator = GetComponent<ItemDataCreator>();
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;
        foreach(Transform slot in RootObjectCache.GetRoot("InventoryCanvas").transform.Find("GUIPanel"))
        {
            slots.Add(slot.gameObject);
        }
    }
    private void Update()
    {
        if(mcs.isTouchingStepladder && Input.GetMouseButtonDown(1))
        {
            if(Vector2.Distance(player.position, mcs.touchedStepladder.transform.position) <= 2.4f)
            {
                PickupSL(mcs.touchedStepladder);
            }
        }
    }
    public void PickupSL(GameObject sl)
    {
        bool invIsFull = true;
        for(int i = 0; i < 6; i++)
        {
            if (invScript.inventory[i].itemData == null)
            {
                invIsFull = false;
                break;
            }
        }
        if (invIsFull)
        {
            return;
        }

        particlesScript.CreateDust(sl.transform.position, 1, sl.GetComponent<SpriteRenderer>().sortingLayerName);
        ItemData data = creator.CreateItemData(137);
        for (int i = 0; i < 6; i++)
        {
            if (invScript.inventory[i].itemData == null)
            {
                invScript.inventory[i].itemData = data;
                slots[i].GetComponent<Image>().sprite = data.sprite;
                break;
            }
        }
        Destroy(sl);
        foreach(Transform bo in badObjects)
        {
            if(bo.GetComponent<BadObjectData>().attachedObject == sl && bo.name == "stepladder")
            {
                Destroy(bo.gameObject);
                break;
            }
        }
    }
}
