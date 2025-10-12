using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeskData : MonoBehaviour
{
    public List<DeskItem> deskInv = new List<DeskItem>();
    public int inmateCorrelationNumber;

    private void Start()
    {
        DeskItem nullItem = new DeskItem();
        nullItem.itemData = null;
        for (int i = 0; i < 20; i++)
        {
            deskInv.Add(nullItem);
        }
    }
}
