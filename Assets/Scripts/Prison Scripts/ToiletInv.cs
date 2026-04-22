using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ToiletInv : MonoBehaviour
{
    public List<ItemData> toiletInv = new List<ItemData>()
    {
        null, null, null
    };
    public int flushTimer;
    public bool isClogged;

    private void Update()
    {
        for(int i = 0; i < 3; i++)
        {
            try
            {
                if (string.IsNullOrEmpty(toiletInv[i].displayName))
                {
                    toiletInv[i] = null;
                }
            }
            catch
            {
                toiletInv[i] = null;
            }
        }
    }
}
