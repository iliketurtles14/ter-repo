using UnityEngine;

public class FoodPickUp : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Transform player;
    private Schedule scheduleScript;

    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
    }
    private void Update()
    {
        if(mcs.isTouchingFoodTable && Input.GetMouseButtonDown(1) && mcs.touchedFoodTable.GetComponent<FoodTableCounter>().foodCount > 0 && !player.GetComponent<PlayerCollectionData>().playerData.hasFood)
        {
            TakeFood(mcs.touchedFoodTable);
        }

        if(scheduleScript.periodCode != "B" &&  scheduleScript.periodCode != "L" &&  scheduleScript.periodCode != "D")
        {
            player.GetComponent<PlayerCollectionData>().playerData.hasFood = false;
        }
    }
    public void TakeFood(GameObject foodTable)
    {
        foodTable.GetComponent<FoodTableCounter>().foodCount--;
        player.GetComponent<PlayerCollectionData>().playerData.hasFood = true;
    }
}
