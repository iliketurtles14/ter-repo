using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MEDeskListContainer : MonoBehaviour
{
    public List<int> ids = new List<int>();
    private void OnEnable()
    {
        for(int i = 0; i < 20; i++)
        {
            ids.Add(-1);
        }
    }
}
