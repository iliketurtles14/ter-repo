using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeskData : MonoBehaviour
{
    public List<DeskItem> deskInv = new List<DeskItem>();
    public int inmateCorrelationNumber = -1;

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            DeskItem nullItem = new DeskItem();
            deskInv.Add(nullItem);
        }
    }
    private void Update()
    {
        for(int i = 0; i < 20; i++)
        {
            try
            {
                if (deskInv[i].itemData.sprite == null)
                {
                    deskInv[i] = new DeskItem();
                }
            }
            catch 
            {
                deskInv[i] = new DeskItem();
            }
        }
    }
}
