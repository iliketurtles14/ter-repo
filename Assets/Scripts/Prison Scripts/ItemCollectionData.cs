using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEditor;
public class ItemCollectionData : MonoBehaviour 
{
    public ItemData itemData;
    private LoadPrison loadPrisonScript;
    private ItemDataCreator creator;
    private void Awake()
    {
        loadPrisonScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>();
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();
        StartCoroutine(Wait());
    }
    private void SetData()
    {
        string str;
        if (name.Contains("(Clone)"))
        {
            str = name.Replace("(Clone)", "");
        }
        else
        {
            str = name;
        }

        int id = Convert.ToInt32(str);
        itemData = creator.CreateItemData(id);
    }
    private IEnumerator Wait()
    {
        while (true)
        {
            if(loadPrisonScript.currentMap != null)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }
        SetData();
    }
}
