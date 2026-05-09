using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFacingDirections : MonoBehaviour //this is for what direction npcs face at when standing in a certain period
{
    private Transform tiles;
    private List<Transform> guardRollcallWPs = new List<Transform>();
    private List<Transform> guardCanteenWPs = new List<Transform>();
    private List<Transform> guardGymWPs = new List<Transform>();
    private List<Transform> guardShowerWPs = new List<Transform>();
    private List<Transform> inmateRollcallWPs = new List<Transform>();
    private List<Transform> inmateShowerWPs = new List<Transform>();
    private List<Transform> canteenChairs = new List<Transform>();
    private List<Transform> gymEquipment = new List<Transform>();

    private List<Transform> allChairs = new List<Transform>();
    private List<Transform> tables = new List<Transform>();
    private List<Transform> playerVisitorChairs = new List<Transform>();
    private List<Transform> npcVisitorChairs = new List<Transform>();
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
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
        SetRollcallDirs();
        SetCanteenDirs();
        SetGymDirs();
        SetShowerDirs();
        SetChairDirs();
    }
    private void GetLists()
    {
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            switch (obj.name)
            {
                case "GuardRollcall":
                    guardRollcallWPs.Add(obj);
                    break;
                case "GuardCanteen":
                    guardCanteenWPs.Add(obj);
                    break;
                case "GuardGym":
                    guardGymWPs.Add(obj);
                    break;
                case "GuardShower":
                    guardShowerWPs.Add(obj);
                    break;
                case "InmateRollcall":
                    inmateRollcallWPs.Add(obj);
                    break;
            }
        }

        bool canteenZoneExists = false;
        Transform canteenZone = null;
        foreach(Transform zone in tiles.Find("Zones"))
        {
            if(zone.name == "Canteen")
            {
                canteenZoneExists = true;
                canteenZone = zone;
                break;
            }
        }

        if (canteenZoneExists)
        {
            foreach (Transform obj in tiles.Find("GroundObjects"))
            {
                if (obj.name == "Seat")
                {
                    //checking if the seat is within the zone
                    Vector2 zoneSize = canteenZone.GetComponent<BoxCollider2D>().size;
                    Vector2 zonePos = canteenZone.position;
                    float xDif = Mathf.Abs(obj.position.x - zonePos.x);
                    float yDif = Mathf.Abs(obj.position.y - zonePos.y);
                    if((zoneSize.x / 2f) - xDif >= 0 &&
                        (zoneSize.y / 2f) - yDif >= 0)
                    {
                        canteenChairs.Add(obj);
                    }
                }
            }
        }
        else
        {
            foreach(Transform obj in tiles.Find("GroundObjects"))
            {
                if(obj.name == "Seat")
                {
                    canteenChairs.Add(obj);
                }
            }
        }

        bool gymZoneExists = false;
        Transform gymZone = null;
        foreach(Transform zone in tiles.Find("Zones"))
        {
            if(zone.name == "Gym")
            {
                gymZoneExists = true;
                gymZone = zone;
                break;
            }
        }

        if (gymZoneExists)
        {
            foreach(Transform obj in tiles.Find("GroundObjects"))
            {
                if (obj.CompareTag("Equipment"))
                {
                    //checking if the equipment is within the zone
                    Vector2 zoneSize = gymZone.GetComponent<BoxCollider2D>().size;
                    Vector2 zonePos = gymZone.position;
                    float xDif = Mathf.Abs(obj.position.x - zonePos.x);
                    float yDif = Mathf.Abs(obj.position.y - zonePos.y);
                    if ((zoneSize.x / 2f) - xDif >= 0 &&
                        (zoneSize.y / 2f) - yDif >= 0)
                    {
                        gymEquipment.Add(obj);
                    }
                }
            }
        }
        else
        {
            foreach(Transform obj in tiles.Find("GroundObjects"))
            {
                if (obj.CompareTag("Equipment"))
                {
                    gymEquipment.Add(obj);
                }
            }
        }

        bool showerZoneExists = false;
        Transform showerZone = null;
        foreach (Transform zone in tiles.Find("Zones"))
        {
            if (zone.name == "Showers")
            {
                showerZoneExists = true;
                showerZone = zone;
                break;
            }
        }

        if (showerZoneExists)
        {
            foreach (Transform obj in tiles.Find("GroundObjects"))
            {
                if (obj.name == "InmateShower")
                {
                    //checking if the shower wp is within the zone
                    Vector2 zoneSize = showerZone.GetComponent<BoxCollider2D>().size;
                    Vector2 zonePos = showerZone.position;
                    float xDif = Mathf.Abs(obj.position.x - zonePos.x);
                    float yDif = Mathf.Abs(obj.position.y - zonePos.y);
                    if ((zoneSize.x / 2f) - xDif >= 0 &&
                        (zoneSize.y / 2f) - yDif >= 0)
                    {
                        inmateShowerWPs.Add(obj);
                    }
                }
            }
        }
        else
        {
            foreach (Transform obj in tiles.Find("GroundObjects"))
            {
                if (obj.name == "InmateShower")
                {
                    inmateShowerWPs.Add(obj);
                }
            }
        }

        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "Seat")
            {
                allChairs.Add(obj);
            }
        }
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "Table")
            {
                tables.Add(obj);
            }
        }
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "VisitorPlayer")
            {
                playerVisitorChairs.Add(obj);
            }
        }
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "VisitorNPC")
            {
                npcVisitorChairs.Add(obj);
            }
        }
    }
    private void SetRollcallDirs()
    {
        ///guards
        //get position of the closest inmate wp

        foreach(Transform guardWP in tiles.Find("GroundObjects"))
        {
            if(guardWP.name == "GuardRollcall")
            {
                float lowestDistance = -1f;
                Transform lowestDistanceWP = null;
                foreach(Transform inmateWP in tiles.Find("GroundObjects"))
                {
                    if(inmateWP.name == "InmateRollcall")
                    {
                        if(lowestDistance == -1f)
                        {
                            lowestDistance = Vector2.Distance(guardWP.position, inmateWP.position);
                            lowestDistanceWP = inmateWP;
                        }
                        else if(Vector2.Distance(guardWP.position, inmateWP.position) < lowestDistance)
                        {
                            lowestDistance = Vector2.Distance(guardWP.position, inmateWP.position);
                            lowestDistanceWP = inmateWP;
                        }
                    }
                }

                guardWP.GetComponent<WaypointData>().dir = GetDirection(guardWP.position, lowestDistanceWP.position);
            }
        }

        ///inmates
        //get position of the closest inmate wp

        foreach (Transform inmateWP in tiles.Find("GroundObjects"))
        {
            if (inmateWP.name == "InmateRollcall")
            {
                float lowestDistance = -1f;
                Transform lowestDistanceWP = null;
                foreach (Transform guardWP in tiles.Find("GroundObjects"))
                {
                    if (guardWP.name == "GuardRollcall")
                    {
                        if (lowestDistance == -1f)
                        {
                            lowestDistance = Vector2.Distance(inmateWP.position, guardWP.position);
                            lowestDistanceWP = guardWP;
                        }
                        else if (Vector2.Distance(inmateWP.position, guardWP.position) < lowestDistance)
                        {
                            lowestDistance = Vector2.Distance(inmateWP.position, guardWP.position);
                            lowestDistanceWP = guardWP;
                        }
                    }
                }

                inmateWP.GetComponent<WaypointData>().dir = GetDirection(inmateWP.position, lowestDistanceWP.position);
            }
        }
    }
    private void SetCanteenDirs()
    {
        //get average position of the inmate chairs
        float sumX = 0f;
        float sumY = 0f;
        float count = canteenChairs.Count;
        foreach(Transform chair in canteenChairs)
        {
            sumX += chair.position.x;
            sumY += chair.position.y;
        }
        float avgX = sumX / count;
        float avgY = sumY / count;
        Vector2 avg = new Vector2(avgX, avgY);

        //get and set the dirs
        foreach(Transform wp in guardCanteenWPs)
        {
            wp.GetComponent<WaypointData>().dir = GetDirection(wp.position, avg);
        }
    }
    private void SetGymDirs()
    {
        //get average position of the equipment
        float sumX = 0f;
        float sumY = 0f;
        float count = gymEquipment.Count;
        foreach (Transform chair in gymEquipment)
        {
            sumX += chair.position.x;
            sumY += chair.position.y;
        }
        float avgX = sumX / count;
        float avgY = sumY / count;
        Vector2 avg = new Vector2(avgX, avgY);

        //get and set the dirs
        foreach (Transform wp in guardGymWPs)
        {
            wp.GetComponent<WaypointData>().dir = GetDirection(wp.position, avg);
        }
    }
    private void SetShowerDirs()
    {
        //get average position of the shower wps
        float sumX = 0f;
        float sumY = 0f;
        float count = inmateShowerWPs.Count;
        foreach (Transform chair in inmateShowerWPs)
        {
            sumX += chair.position.x;
            sumY += chair.position.y;
        }
        float avgX = sumX / count;
        float avgY = sumY / count;
        Vector2 avg = new Vector2(avgX, avgY);

        //get and set the dirs
        foreach (Transform wp in guardShowerWPs)
        {
            wp.GetComponent<WaypointData>().dir = GetDirection(wp.position, avg);
        }
    }
    private void SetChairDirs()
    {
        //get a table if its within 1.7 of the chair
        foreach(Transform chair in allChairs)
        {
            foreach(Transform table in tables)
            {
                if(Vector2.Distance(chair.position, table.position) < 1.7f)
                {
                    chair.GetComponent<WaypointData>().dir = GetDirection(chair.position, table.position);
                    break;
                }
            }
        }

        //visitor stuff
        foreach (Transform playerChair in playerVisitorChairs)
        {
            float lowestDistance = -1f;
            Transform lowestDistanceChair = null;
            foreach (Transform npcChair in npcVisitorChairs)
            {
                if (lowestDistance == -1f)
                {
                    lowestDistance = Vector2.Distance(playerChair.position, npcChair.position);
                    lowestDistanceChair = npcChair;
                }
                else if (Vector2.Distance(playerChair.position, npcChair.position) < lowestDistance)
                {
                    lowestDistance = Vector2.Distance(playerChair.position, npcChair.position);
                    lowestDistanceChair = npcChair;
                }
            }
            playerChair.GetComponent<WaypointData>().dir = GetDirection(playerChair.position, lowestDistanceChair.position);
        }
        foreach (Transform npcChair in npcVisitorChairs)
        {
            float lowestDistance = -1f;
            Transform lowestDistanceChair = null;
            foreach (Transform playerChair in playerVisitorChairs)
            {
                if (lowestDistance == -1f)
                {
                    lowestDistance = Vector2.Distance(playerChair.position, npcChair.position);
                    lowestDistanceChair = playerChair;
                }
                else if (Vector2.Distance(playerChair.position, npcChair.position) < lowestDistance)
                {
                    lowestDistance = Vector2.Distance(playerChair.position, npcChair.position);
                    lowestDistanceChair = playerChair;
                }
            }
            npcChair.GetComponent<WaypointData>().dir = GetDirection(npcChair.position, lowestDistanceChair.position);
        }
    }
    public string GetDirection(Vector2 startPos, Vector2 endPos)
    {
        float xDif = endPos.x - startPos.x;
        float yDif = endPos.y - startPos.y;

        // If the vertical distance is greater than or equal to the horizontal distance
        if (Mathf.Abs(yDif) >= Mathf.Abs(xDif))
        {
            if (yDif >= 0)
                return "up";
            else
                return "down";
        }
        // If the horizontal distance is greater
        else
        {
            if (xDif >= 0)
                return "right";
            else
                return "left";
        }
    }
}
