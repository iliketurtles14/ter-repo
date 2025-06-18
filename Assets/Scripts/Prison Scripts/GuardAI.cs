using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GuardAI : MonoBehaviour
{
    private int whatGuard;
    private bool isMain;
    private bool isFreetime;
    private Routine scheduleScript;
    private AStar aStarScript;
    private string periodCode;
    private string oldPeriod;
    private bool reachedEndOfPath;
    private float rand;
    public GameObject currentGW;
    public Transform currentGWPos;
    private List<GameObject> freetimeWaypoints;
    private List<GameObject> canteenWaypoints;
    private List<GameObject> exerciseWaypoints;
    private List<GameObject> rollcallWaypoints;
    private List<GameObject> showerWaypoints;
    public void Start()
    {
        //get aStarScript
        aStarScript = GetComponent<AStar>();

        //disable the aStarScript
        aStarScript.enabled = false;
        
        //whatGuard and isMain are defined
        switch (name)
        {
            case "Guard1": whatGuard = 1; isMain = true; break;
            case "Guard2": whatGuard = 2; isMain = true; break;
            case "Guard3": whatGuard = 3; isMain = true; break;
            default: whatGuard = 0; isMain = false; break;
        }

        //Make lists of waypoints
        freetimeWaypoints = GameObject.FindGameObjectsWithTag("FreeTimeWaypoint(Guard)").ToList();
        canteenWaypoints = GameObject.FindGameObjectsWithTag("CanteenWaypoint(Guard)").ToList();
        exerciseWaypoints = GameObject.FindGameObjectsWithTag("ExerciseWaypoint(Guard)").ToList();
        rollcallWaypoints = GameObject.FindGameObjectsWithTag("RollcallWaypoint(Guard)").ToList();
        showerWaypoints = GameObject.FindGameObjectsWithTag("ShowerWaypoint(Guard)").ToList();

        //Go to the main method (Switcher)
        
        Switcher();
    }
    public void Switcher()
    {
        
        //Disable the AI script
        aStarScript.enabled = false;

        //Check what period type it is and go to the correct method based on that
        switch (periodCode)
        {
            case "MR": isFreetime = false; break;
            case "B": isFreetime = false; break;
            case "FP": isFreetime = true; break;
            case "L": isFreetime = false; break;
            case "W": isFreetime = true; break;
            case "EP": isFreetime = false; break;
            case "S": isFreetime = false; break;
            case "D": isFreetime = false; break;
            case "EFP": isFreetime = true; break;
            case "ER": isFreetime = false; break;
            case "LO": isFreetime = true; break;
            default: isFreetime = true; break;
        }
        if (isFreetime || !isMain)
        {
            rand = Random.Range(1, 4);
            StartCoroutine(WaitRandomAndFreetime());
        }
        else if (!isFreetime && isMain)
        {
            Scheduled();
        }
        
    }
    public void Freetime()
    {        
        oldPeriod = periodCode;

        //Pick a GW at random and set it to the currentGW
        currentGW = freetimeWaypoints[Random.Range(0, freetimeWaypoints.Count)];
        currentGWPos = currentGW.transform;

        //Enable the AStar Script
        aStarScript.enabled = true;

    }
    public void Scheduled()
    {
        //Determine what GW to pick
        if(periodCode == "MR" || periodCode == "ER")
        {
            switch (whatGuard)
            {
                case 1: currentGW = rollcallWaypoints[0]; break;
                case 2: currentGW = rollcallWaypoints[1]; break;
                case 3: currentGW = rollcallWaypoints[2]; break;
            }
        }
        else if(periodCode == "B" || periodCode == "L" || periodCode == "D")
        {
            switch (whatGuard)
            {
                case 1: currentGW = canteenWaypoints[0]; break;
                case 2: currentGW = canteenWaypoints[1]; break;
                case 3: currentGW = canteenWaypoints[2]; break;
            }
        }
        else if(periodCode == "EP")
        {
            switch (whatGuard)
            {
                case 1: currentGW = exerciseWaypoints[0]; break;
                case 2: currentGW = exerciseWaypoints[1]; break;
                case 3: currentGW = exerciseWaypoints[2]; break;
            }
        }
        else if(periodCode == "S")
        {
            switch (whatGuard)
            {
                case 1: currentGW = showerWaypoints[0]; break;
                case 2: currentGW = showerWaypoints[1]; break;
                case 3: currentGW = showerWaypoints[2]; break;
            }
        }
        else { return; }
        currentGWPos = currentGW.transform;
        oldPeriod = periodCode;

        //enable the aStar Script
        aStarScript.enabled = true;


    }
    private IEnumerator WaitRandomAndFreetime()
    {
        yield return new WaitForSeconds(rand);
        Freetime();
    }
    public void Update()
    {
        scheduleScript = GameObject.Find("Time").GetComponent<Routine>();

        //Define periodCode and reachedEndOfPath
        periodCode = scheduleScript.periodCode;
        reachedEndOfPath = aStarScript.reachedEndOfPath;

        //check if reachedEndOfPath = true
        if (reachedEndOfPath)
        {
            if (!isFreetime && isMain)
            {
                //aStarScript.enabled = false;
                if(oldPeriod != periodCode)
                {
                    Switcher();
                }
                else if(oldPeriod == periodCode) { return; }
            }
            else if (isFreetime || !isMain)
            {
                Switcher();
            }
            else { return; }
        }
        else { return; }
    }
}