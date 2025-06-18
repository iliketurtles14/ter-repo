using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InmateAI : MonoBehaviour
{
    private int whatInmate;
    private bool isFreetime;
    private Routine scheduleScript;
    private AStar aStarScript;
    private string periodCode;
    private string oldPeriod;
    private bool reachedEndOfPath;
    private float rand;
    public GameObject currentIW;
    public Transform currentIWPos;
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

        //whatInmate is defined
        switch (name)
        {
            case "Inmate1": whatInmate = 1; break;
            case "Inmate2": whatInmate = 2; break;
            case "Inmate3": whatInmate = 3; break;
            case "Inmate4": whatInmate = 4; break;
            case "Inmate5": whatInmate = 5; break;
            case "Inmate6": whatInmate = 6; break;
            case "Inmate7": whatInmate = 7; break;
            case "Inmate8": whatInmate = 8; break;
            case "Inmate9": whatInmate = 9; break;
        }

        //Make lists of waypoints
        freetimeWaypoints = GameObject.FindGameObjectsWithTag("FreeTimeWaypoint(Inmate)").ToList();
        canteenWaypoints = GameObject.FindGameObjectsWithTag("CanteenWaypoint(Inmate)").ToList();
        exerciseWaypoints = GameObject.FindGameObjectsWithTag("ExerciseWaypoint(Inmate)").ToList();
        rollcallWaypoints = GameObject.FindGameObjectsWithTag("RollcallWaypoint(Inmate)").ToList();
        showerWaypoints = GameObject.FindGameObjectsWithTag("ShowerWaypoint(Inmate)").ToList();

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
        if (isFreetime)
        {
            rand = Random.Range(1, 4);
            StartCoroutine(WaitRandomAndFreetime());
        }
        else if (!isFreetime)
        {
            Scheduled();
        }
    }
    public void Freetime()
    {
        oldPeriod = periodCode;
        
        //Pick an IW at random and set it to the currentIW
        currentIW = freetimeWaypoints[Random.Range(0, freetimeWaypoints.Count)];
        currentIWPos = currentIW.transform;

        //Enable the AStar script
        aStarScript.enabled = true;
    }
    public void Scheduled()
    {
        //Determine what IW to pick
        if (periodCode == "MR" || periodCode == "ER")
        {
            switch (whatInmate)
            {
                case 1: currentIW = rollcallWaypoints[0]; break;
                case 2: currentIW = rollcallWaypoints[1]; break;
                case 3: currentIW = rollcallWaypoints[2]; break;
                case 4: currentIW = rollcallWaypoints[3]; break;
                case 5: currentIW = rollcallWaypoints[4]; break;
                case 6: currentIW = rollcallWaypoints[5]; break;
                case 7: currentIW = rollcallWaypoints[6]; break;
                case 8: currentIW = rollcallWaypoints[7]; break;
                case 9: currentIW = rollcallWaypoints[8]; break;
            }
        }
        else if (periodCode == "B" || periodCode == "L" || periodCode == "D")
        {
            switch (whatInmate)
            {
                case 1: currentIW = canteenWaypoints[0]; break;
                case 2: currentIW = canteenWaypoints[1]; break;
                case 3: currentIW = canteenWaypoints[2]; break;
                case 4: currentIW = canteenWaypoints[3]; break;
                case 5: currentIW = canteenWaypoints[4]; break;
                case 6: currentIW = canteenWaypoints[5]; break;
                case 7: currentIW = canteenWaypoints[6]; break;
                case 8: currentIW = canteenWaypoints[7]; break;
                case 9: currentIW = canteenWaypoints[8]; break;
            }
        }
        else if (periodCode == "EP")
        {
            switch (whatInmate)
            {
                case 1: currentIW = exerciseWaypoints[0]; break;
                case 2: currentIW = exerciseWaypoints[1]; break;
                case 3: currentIW = exerciseWaypoints[2]; break;
                case 4: currentIW = exerciseWaypoints[3]; break;
                case 5: currentIW = exerciseWaypoints[4]; break;
                case 6: currentIW = exerciseWaypoints[5]; break;
                case 7: currentIW = exerciseWaypoints[6]; break;
                case 8: currentIW = exerciseWaypoints[7]; break;
                case 9: currentIW = exerciseWaypoints[8]; break;
            }
        }
        else if (periodCode == "S")
        {
            switch (whatInmate)
            {
                case 1: currentIW = showerWaypoints[0]; break;
                case 2: currentIW = showerWaypoints[1]; break;
                case 3: currentIW = showerWaypoints[2]; break;
                case 4: currentIW = showerWaypoints[3]; break;
                case 5: currentIW = showerWaypoints[4]; break;
                case 6: currentIW = showerWaypoints[5]; break;
                case 7: currentIW = showerWaypoints[6]; break;
                case 8: currentIW = showerWaypoints[7]; break;
                case 9: currentIW = showerWaypoints[8]; break;
            }
        }
        else { return; }
        currentIWPos = currentIW.transform;
        oldPeriod = periodCode;

        //enable the aStar script
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
        
        if (!isFreetime && reachedEndOfPath)
        {
            //aStarScript.enabled = false;
            if (oldPeriod != periodCode)
            {
                Switcher();
            }
            else if (oldPeriod == periodCode) { return; }
        }
        else if (isFreetime && reachedEndOfPath)
        {
            Switcher();
        }
        else { return; }
    }
}
