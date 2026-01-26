using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    private bool inEnterMode;
    private string idStr;
    List<KeyCode> numKeys = new List<KeyCode>()
    {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
    };
    private ItemDataCreator creatorScript;
    private Inventory inventoryScript;
    private void Start()
    {
        creatorScript = GetComponent<ItemDataCreator>();
        inventoryScript = GetComponent<Inventory>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Awaiting item ID...\nPress Enter when done or press Escape to cancel.");
            inEnterMode = true;
        }

        if (inEnterMode && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Canceled.");
            inEnterMode = false;
            idStr = "";
        }
        else if (inEnterMode && !Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.Return))
        {
            int i = 0;
            foreach(KeyCode key in numKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    idStr += i.ToString();
                }
                i++;
            }
        }
        else if(inEnterMode && Input.GetKeyDown(KeyCode.Return))
        {
            int id = Convert.ToInt32(idStr);
            if(id > 273)
            {
                Debug.Log("Invalid ID.");
                return;
            }

            Debug.Log("Creating item: " + id);
            ItemData data = creatorScript.CreateItemData(id);
            inventoryScript.Add(data);

            inEnterMode = false;
            idStr = "";
        }
    }
}
