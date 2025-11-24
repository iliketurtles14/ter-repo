using NUnit.Framework;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCAI : MonoBehaviour
{
    private string npcType;
    private int npcNum;
    private bool isImportantGuard;
    private Seeker seeker;
    private string period;
    private Schedule scheduleScript;
    private Transform tiles;
    private List<Transform> currentPossibleWaypoints;
    private bool isMoving;
    private Transform currentWaypoint;
    private bool isFreeWalking;
    private void Start()
    {
        //get npctype and num
        if (name.StartsWith("Inamte"))
        {
            npcType = "Inmate";

            string[] parts = name.Split('e');
            npcNum = Convert.ToInt32(parts[1]);
        }
        else if (name.StartsWith("Guard"))
        {
            npcType = "Guard";

            string[] parts = name.Split('d');
            npcNum = Convert.ToInt32(parts[1]);
        }

        if(npcType == "Guard" && npcNum < 4)
        {
            isImportantGuard = true;
        }

        seeker = GetComponent<Seeker>();
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
    }
    private void Update()
    {
        //get period
        period = scheduleScript.periodCode;

        if (!isMoving)
        {
            currentWaypoint = null;
            SetCurrentPossibleWaypoints();
            SetCurrentWaypoint();
            seeker.StartPath(transform.position, currentWaypoint.position);
            isMoving = true;
        }
        else
        {
            float distance = Vector3.Distance(transform.position, currentWaypoint.position);
            if(distance < .01f)
            {
                StartCoroutine(FinishMovement());
            }
        }
    }
    private void SetCurrentPossibleWaypoints()
    {
        currentPossibleWaypoints = new List<Transform>();

        isFreeWalking = false;
        foreach (Transform waypoint in tiles.Find("GroundObjects"))
        {
            if (waypoint.CompareTag("Waypoint"))
            {
                switch (period)
                {
                    case "LO":
                    case "W":
                    case "FT":
                        if (npcType == "Inmate" && waypoint.name == "InmateWaypoint")
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "InmateWaypoint")
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "R":
                        if (npcType == "Inmate" && waypoint.name == "InmateRollcall")
                        {
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardRollcall" && isImportantGuard)
                        {
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint" && !isImportantGuard)
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "B":
                    case "L":
                    case "D":
                        if (npcType == "Inmate")
                        {
                            InmateCanteen();
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardCanteen" && isImportantGuard)
                        {
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint" && !isImportantGuard)
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "E":
                        if (npcType == "Inmate")
                        {
                            InmateExercise();
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardGym" && isImportantGuard)
                        {
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint" && !isImportantGuard)
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "S":
                        if (npcType == "Inmate" && waypoint.name == "InmateShower")
                        {
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardShower" && isImportantGuard)
                        {
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint" && !isImportantGuard)
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                }
            }
        }
    }
    private void SetCurrentWaypoint()
    {
        if (isFreeWalking)
        {
            int rand = UnityEngine.Random.Range(0, currentPossibleWaypoints.Count);
            currentWaypoint = currentPossibleWaypoints[rand];
        }
        else
        {
            currentWaypoint = currentPossibleWaypoints[npcNum - 1];
        }
    }
    private IEnumerator FinishMovement()
    {
        seeker.CancelCurrentPathRequest(true);
        int rand = UnityEngine.Random.Range(0, 4);
        yield return new WaitForSeconds(rand);
        isMoving = false;
    }
    private void InmateCanteen()
    {
        //canteen stuff
        isMoving = false;
    }
    private void InmateExercise()
    {
        //exercise stuff
        isMoving = false;
    }
}
