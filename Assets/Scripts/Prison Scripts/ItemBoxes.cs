using UnityEngine;

public class ItemBoxes : MonoBehaviour
{
    MouseCollisionOnItems mcs;
    ItemDataCreator creator;
    Inventory inventoryScript;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        creator = GetComponent<ItemDataCreator>();

    }
    private void Update()
    {
        if(mcs.isTouchingItemBox && Input.GetMouseButtonDown(0))
        {
            switch (mcs.touchedItemBox.name)
            {
                case "BookBox":
                    AddItem(77);
                    break;
                case "DeliveryTruckDown":
                case "DeliveryTruckUp":
                case "DeliveryTruckLeft":
                case "DeliveryTruckRight":
                    int rand = UnityEngine.Random.Range(0, 2);
                    if(rand == 0)
                    {
                        AddItem(116);
                    }
                    else
                    {
                        AddItem(117);
                    }
                    break;
                case "DirtyLaundry":
                    rand = UnityEngine.Random.Range(0, 2);
                    if(rand == 0)
                    {
                        AddItem(37);
                    }
                    else
                    {
                        AddItem(38);
                    }
                    break;
                case "Freezer":
                    AddItem(147);
                    break;
                case "MailBox":
                    AddItem(107);
                    break;
                case "MetalBox":
                    AddItem(130);
                    break;
                case "TailorBox":
                    AddItem(94);
                    break;
                case "TimberBox":
                    AddItem(139);
                    break;
            }
        }
    }
    private void AddItem(int id)
    {
        ItemData data = creator.CreateItemData(id);
        inventoryScript.Add(data);
    }
}
