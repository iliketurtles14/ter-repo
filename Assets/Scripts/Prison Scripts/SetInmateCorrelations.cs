using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetInmateCorrelations : MonoBehaviour //this is for correlations with desks and beds, not for seats at food periods and such
{
    private List<Transform> beds = new List<Transform>();
    private List<Transform> desks = new List<Transform>();
    private List<Transform> inmates = new List<Transform>();
    private Transform aStar;
    private Transform tiles;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        aStar = RootObjectCache.GetRoot("A*").transform;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GetLists();
        SetInmateOrder();
        SetInmateBed();
        ConnectDeskToBed();
        SetInmateDesk();
    }
    private void GetLists()
    {
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name.StartsWith("Bed"))
            {
                beds.Add(obj);
            }
            else if(obj.name == "NPCDesk")
            {
                desks.Add(obj);
            }
        }
        foreach(Transform npc in aStar)
        {
            if (npc.name.StartsWith("Inmate"))
            {
                inmates.Add(npc);
            }
        }
    }
    private void SetInmateOrder()
    {
        foreach(Transform npc in inmates)
        {
            int num = Convert.ToInt32(npc.name.Replace("Inmate", "")) - 1;
            npc.GetComponent<NPCCollectionData>().npcData.order = num;
        }
    }
    private void ConnectDeskToBed()
    {
        for(int i = 0; i < beds.Count; i++)
        {
            Transform bed = beds[i];
            List<float> distances = new List<float>();
            foreach(Transform desk in desks)
            {
                float distance = Vector2.Distance(bed.position, desk.position);
                distances.Add(distance);
            }
            float lowestDistance = distances.Min();
            foreach(Transform desk in desks)
            {
                float distance = Vector2.Distance(bed.position, desk.position);
                if(Mathf.Abs(distance - lowestDistance) < .01f)
                {
                    desk.GetComponent<DeskData>().inmateCorrelationNumber = i;
                    desks.Remove(desk);
                    break;
                }
            }
        }
    }
    private void SetInmateBed()
    {
        foreach(Transform npc in inmates)
        {
            try
            {
                npc.GetComponent<NPCCollectionData>().npcData.bed = beds[npc.GetComponent<NPCCollectionData>().npcData.order].gameObject;
            }
            catch { }
        }
    }
    private void SetInmateDesk()
    {
        foreach(Transform npc in inmates)
        {
            try
            {
                npc.GetComponent<NPCCollectionData>().npcData.desk = desks[npc.GetComponent<NPCCollectionData>().npcData.order].gameObject;
            }
            catch { }
        }
    }
}
