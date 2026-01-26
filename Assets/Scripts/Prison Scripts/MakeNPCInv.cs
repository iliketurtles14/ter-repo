using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MakeNPCInv : MonoBehaviour
{
    private void Update()
    {
        for (int i = 0; i < 8; i++)
        {
            try
            {
                if (GetComponent<NPCCollectionData>().npcData.inventory[i].itemData.sprite == null)
                {
                    GetComponent<NPCCollectionData>().npcData.inventory[i] = new NPCInvItem();
                }
            }
            catch { }
        }
    }
}
