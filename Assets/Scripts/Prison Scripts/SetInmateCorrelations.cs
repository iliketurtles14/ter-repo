using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetInmateCorrelations : MonoBehaviour //this is for correlations with desks and beds, not for seats at food periods and such
{
    public List<Transform> beds = new List<Transform>();
    public List<Transform> desks = new List<Transform>();
    public List<Transform> inmates = new List<Transform>();
    private Transform aStar;
    private Transform tiles;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        aStar = RootObjectCache.GetRoot("A*").transform;
    }
    public void Set()
    {
        GetLists();
        SetInmateOrder();
        SetInmateBed();
        ConnectDeskToBed();
        SetInmateDesk();
        CheckForWeirdStuff();
    }
    private void GetLists()
    {
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name.StartsWith("Bed"))
            {
                beds.Add(obj);
            }
            else if(obj.name == "NPCDesk" || obj.name == "ETNPCDesk")
            {
                desks.Add(obj);
            }
        }
        foreach(Transform npc in aStar)
        {
            if (npc.name.StartsWith("Inmate") && !npc.GetComponent<NPCAI>().hasSetPath)
            {
                inmates.Add(npc);
            }
        }
    }
    private void SetInmateOrder()
    {
        for(int i = 0; i < inmates.Count; i++)
        {
            inmates[i].GetComponent<NPCCollectionData>().npcData.order = i;
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
            catch(Exception ex)
            { 
                Debug.Log(ex.Message);
                Debug.Log(beds.Count + ", " + npc.GetComponent<NPCCollectionData>().npcData.order);
            }
        }
    }
    private void SetInmateDesk()
    {
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "NPCDesk" || obj.name == "ETNPCDesk")
            {
                int deskNum = obj.GetComponent<DeskData>().inmateCorrelationNumber;

                foreach(Transform npc in inmates)
                {
                    if(npc.GetComponent<NPCCollectionData>().npcData.order == deskNum)
                    {
                        npc.GetComponent<NPCCollectionData>().npcData.desk = obj.gameObject;
                        break;
                    }
                }
            }
        }
    }
    private void CheckForWeirdStuff() //checks for stuff like too many inmates for desks and stuff like that
    {
        if(inmates.Count < desks.Count)
        {
            int max = inmates.Count;
            int leftOver = desks.Count - inmates.Count;

            for(int i = 0; i < leftOver; i++)
            {
                desks[i + max].GetComponent<DeskData>().inmateCorrelationNumber = -1;
            }
        }
    }
}
