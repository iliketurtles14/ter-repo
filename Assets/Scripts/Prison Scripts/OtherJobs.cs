using UnityEngine;

public class OtherJobs : MonoBehaviour //this is for all jobs that arent tied to job boxes, so like
                                       //mailman, kitchen, library 7
{
    private Schedule scheduleScript;
    private Transform player;
    private MouseCollisionOnItems mcs;
    private InventorySelection selectionScript;
    private Inventory inventoryScript;
    private Transform aStar;
    private Jobs jobsScript;
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        player = RootObjectCache.GetRoot("Player").transform;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        selectionScript = GetComponent<InventorySelection>();
        inventoryScript = GetComponent<Inventory>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        jobsScript = GetComponent<Jobs>();
    }
    private void Update()
    {
        if(scheduleScript.periodCode == "W")
        {
            switch (player.GetComponent<PlayerCollectionData>().playerData.job)
            {
                case "Mailman":
                case "Library":
                    int id;
                    if(player.GetComponent<PlayerCollectionData>().playerData.job == "Mailman")
                    {
                        id = 107;
                    }
                    else
                    {
                        id = 77;
                    }

                    if(selectionScript.aSlotSelected && inventoryScript.inventory[selectionScript.selectedSlotNum].itemData.id == id &&
                        mcs.isTouchingDesk && Input.GetMouseButtonDown(0))
                    {
                        ItemData selectedItem = inventoryScript.inventory[selectionScript.selectedSlotNum].itemData;
                        string targetNPC = selectedItem.inmateGiveName;
                        if(targetNPC == null)
                        {
                            return;
                        }

                        bool rightDesk = false;
                        foreach(Transform npc in aStar)
                        {
                            if(npc.GetComponent<NPCCollectionData>().npcData.desk == mcs.touchedDesk &&
                                targetNPC == npc.GetComponent<NPCCollectionData>().npcData.displayName)
                            {
                                rightDesk = true;
                                break;
                            }
                        }
                        if (!rightDesk)
                        {
                            return;
                        }

                        inventoryScript.inventory[selectionScript.selectedSlotNum].itemData = null;
                        jobsScript.AddToQuota(1, 7);
                    }
                    break;
                case "Kitchen":
                    if(selectionScript.aSlotSelected && inventoryScript.inventory[selectionScript.selectedSlotNum].itemData.id == 84 &&
                        mcs.isTouchingFoodTable && Input.GetMouseButtonDown(0))
                    {
                        inventoryScript.inventory[selectionScript.selectedSlotNum].itemData = null;
                        mcs.touchedFoodTable.GetComponent<FoodTableCounter>().foodCount++;
                        jobsScript.AddToQuota(1, 11);
                    }
                    break;
            }
        }
    }
}
