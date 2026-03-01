using UnityEngine;

public class FoodTableCounter : MonoBehaviour
{
    public int foodCount;
    private string period;
    private Schedule scheduleScript;
    private bool shouldMakeFood;
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
    }
    private void Update()
    {
        period = scheduleScript.periodCode;
        GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[61 + foodCount];

        if(foodCount < 0)
        {
            foodCount = 0;
        }
        else if(foodCount > 5)
        {
            foodCount = 5;
        }

        if(period != "L" && period != "B" && period != "D" && !shouldMakeFood)
        {
            shouldMakeFood = true;
        }
        else if((period == "L" ||  period == "B" || period == "D") && shouldMakeFood)
        {
            foodCount = 5;
            shouldMakeFood = false;
        }
    }
}
