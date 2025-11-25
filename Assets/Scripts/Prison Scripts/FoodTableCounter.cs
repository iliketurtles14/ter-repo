using UnityEngine;

public class FoodTableCounter : MonoBehaviour
{
    public int foodCount;

    private void Update()
    {
        GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[61 + foodCount];

        if(foodCount < 0)
        {
            foodCount = 0;
        }
        else if(foodCount > 5)
        {
            foodCount = 5;
        }
    }
}
