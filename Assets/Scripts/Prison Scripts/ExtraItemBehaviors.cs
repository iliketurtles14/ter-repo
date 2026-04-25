using NUnit.Framework;
using System.Collections.Generic;
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
            selectedItemID = inventoryScript.inventory[selectionScript.selectedSlotNum].itemData.id;
        }

        if(mcs.isTouchingBars && Input.GetMouseButtonDown(0) && selectedItemID == 76)
        {
            PlaceBedSheet(selectionScript.selectedSlotNum, mcs.touchedBars);
        }
        else if(mcs.isTouchingSittable && mcs.touchedSittable.name.StartsWith("PlayerBed") && Input.GetMouseButtonDown(0) && selectedItemID == 75)
        {
            PlaceBedDummy(selectionScript.selectedSlotNum, mcs.touchedSittable);
        }
        else if(mcs.isTouchingFloor && Input.GetMouseButtonDown(0) && selectedItemID == 137)
        {
            PlaceStepladder(selectionScript.selectedSlotNum, mcs.touchedFloor);
        }
        else if(mcs.isTouchingNPC && !mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead && Input.GetMouseButtonDown(0) && (selectedItemID == 87 || selectedItemID == 240 || selectedItemID == 237))
        {
            InstaKO(selectionScript.selectedSlotNum, mcs.touchedNPC);
        }
        else if(mcs.isTouchingPlayer && (creator.CreateItemData(selectedItemID).health != -1 || creator.CreateItemData(selectedItemID).energy != -1) && Input.GetMouseButtonDown(0))
        {
            Eat(selectionScript.selectedSlotNum);
        }
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
    private void PlacePatchUp()//
    {

    }
    private void Zipline()//
    {

    }
    private void BlockCamera()//
    {

    }
    private void PlaceStepladder(int slot, GameObject floor)
    {
        inventoryScript.inventory[slot].itemData = null;
        invSlots[slot].GetComponent<Image>().sprite = clear;

        GameObject sl = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Stepladder"));
        sl.name = "Stepladder";
        sl.transform.parent = tiles.Find("GroundObjects");
        sl.transform.position = floor.transform.position;
        sl.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[154];
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
        }
        if(energy != -1)
        {
            player.GetComponent<PlayerCollectionData>().playerData.energy -= energy;
        }
    }
    private void InstaKO(int slot, GameObject npc)
    {
        inventoryScript.inventory[slot].itemData = null;
        invSlots[slot].GetComponent<Image>().sprite = clear;

        deathScript.KillNPC(npc);
    }
}
