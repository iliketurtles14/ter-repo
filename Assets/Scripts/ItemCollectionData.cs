using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEditor;
public class ItemCollectionData : MonoBehaviour 
{
    public ItemData itemData;
    public ItemData[] itemDatas;
    public string folderPath = "Item Scriptable Objects";
    private Inventory inventoryScript;

    [ContextMenu("Load All Assets Into List")]
    public void LoadAllAssets()
    {
        itemDatas = Resources.LoadAll<ItemData>(folderPath);
        GameObject inventoryObject = GameObject.Find("Inventory");
        inventoryScript = inventoryObject.GetComponent<Inventory>();
    }

    public void Start()
    {
        LoadAllAssets();
        string gameObjectNameSubstring = name.Substring(0, Mathf.Min(3, name.Length));
        foreach (ItemData data in itemDatas)
        {
            if (data.name == gameObjectNameSubstring)
            {
                itemData = Instantiate(data);
                if (inventoryScript.isDropped)
                {
                    itemData.currentDurability = inventoryScript.droppedDurability;
                }
                break;
            }
        }

        foreach(ItemData data in itemDatas)
        {
            data.currentDurability = 100;
        }
    }
}
