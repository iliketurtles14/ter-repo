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
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();

        Transform tiles = RootObjectCache.GetRoot("Tiles").transform;

        int i = 0;
        foreach(Transform desk in tiles.Find("GroundObjects"))
        {
            if (desk.gameObject.CompareTag("Desk"))
            {
                desk.AddComponent<DeskData>();
                desk.GetComponent<DeskData>().inmateCorrelationNumber = i;
                desk.GetComponent<DeskData>().deskInv = new List<DeskItem>(20);
                i++;
            }
        }
    }
}
