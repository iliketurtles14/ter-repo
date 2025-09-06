using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Schedule : MonoBehaviour
{
    private string time;
    private Routine timeScript;
    public string period;
    private GameObject TimeObject;
    public string periodCode;
    void Start()
    {
        TimeObject = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").gameObject;
        timeScript = TimeObject.GetComponent<Routine>();
        period = "Lights Out";
        periodCode = "LO";
    }

    void Update()
    {
        time = timeScript.time;

        if (time == "08:00")
        {
            period = "Morning Rollcall";
            periodCode = "MR";
        }
        else if (time == "09:00")
        {
            period = "Breakfast";
            periodCode = "B";
        }
        else if (time == "10:00")
        {
            period = "Free Period";
            periodCode = "FP";
        }
        else if (time == "12:00")
        {
            period = "Lunch";
            periodCode = "L";
        }
        else if (time == "13:00")
        {
            period = "Leisure / Work Period";
            periodCode = "W";
        }
        else if (time == "16:00")
        {
            period = "Exercise Period";
            periodCode = "EP";
        }
        else if (time == "17:00")
        {
            period = "Showers";
            periodCode = "S";
        }
        else if (time == "18:00")
        {
            period = "Dinner";
            periodCode = "D";
        }
        else if (time == "19:00")
        {
            period = "Evening Free Period";
            periodCode = "EFP";
        }
        else if (time == "22:00")
        {
            period = "Evening Rollcall";
            periodCode = "ER";
        }
        else if (time == "23:00")
        {
            period = "Lights Out";
            periodCode = "LO";
        }
        return;
    }
}
