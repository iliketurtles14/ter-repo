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
    private Map map;
    private LoadPrison loadPrisonScript;
    private bool canStart;
    private MissionAsk missionAskScript;
    private bool isOnFavorPeriod;
    private string favorPeriod;
    void Start()
    {
        TimeObject = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").gameObject;
        timeScript = TimeObject.GetComponent<Routine>();
        period = "Lights Out";
        periodCode = "LO";
        loadPrisonScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>();
        missionAskScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("MissionPanel").GetComponent<MissionAsk>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        map = loadPrisonScript.currentMap;
        canStart = true;

        for(int i = 0; i < 24; i++)
        {
            Debug.Log(map.routineDict[i] + " --> " + i);
        }
    }

    void Update()
    {
        if (!canStart)
        {
            return;
        }
        
        //favor stuff
        foreach(Mission mission in missionAskScript.savedMissions)
        {
            if(mission.type == "distract" && mission.period == period)
            {
                isOnFavorPeriod = true;
                favorPeriod = mission.period;
                break;
            }
        }
        
        time = timeScript.time;

        switch (time)
        {
            case "00:00":
                periodCode = map.routineDict[0];
                break;
            case "01:00":
                periodCode = map.routineDict[1];
                break;
            case "02:00":
                periodCode = map.routineDict[2];
                break;
            case "03:00":
                periodCode = map.routineDict[3];
                break;
            case "04:00":
                periodCode = map.routineDict[4];
                break;
            case "05:00":
                periodCode = map.routineDict[5];
                break;
            case "06:00":
                periodCode = map.routineDict[6];
                break;
            case "07:00":
                periodCode = map.routineDict[7];
                break;
            case "08:00":
                periodCode = map.routineDict[8];
                break;
            case "09:00":
                periodCode = map.routineDict[9];
                break;
            case "10:00":
                periodCode = map.routineDict[10];
                break;
            case "11:00":
                periodCode = map.routineDict[11];
                break;
            case "12:00":
                periodCode = map.routineDict[12];
                break;
            case "13:00":
                periodCode = map.routineDict[13];
                break;
            case "14:00":
                periodCode = map.routineDict[14];
                break;
            case "15:00":
                periodCode = map.routineDict[15];
                break;
            case "16:00":
                periodCode = map.routineDict[16];
                break;
            case "17:00":
                periodCode = map.routineDict[17];
                break;
            case "18:00":
                periodCode = map.routineDict[18];
                break;
            case "19:00":
                periodCode = map.routineDict[19];
                break;
            case "20:00":
                periodCode = map.routineDict[20];
                break;
            case "21:00":
                periodCode = map.routineDict[21];
                break;
            case "22:00":
                periodCode = map.routineDict[22];
                break;
            case "23:00":
                periodCode = map.routineDict[23];
                break;
        }

        switch (periodCode)
        {
            case "LO":
                period = "Lights Out";
                break;
            case "R":
                period = "Rollcall";
                break;
            case "B":
                period = "Breakfast";
                break;
            case "L":
                period = "Lunch";
                break;
            case "D":
                period = "Dinner";
                break;
            case "W":
                period = "Work";
                break;
            case "E":
                period = "Gym";
                break;
            case "S":
                period = "Showers";
                break;
            case "FT":
                period = "Free Time";
                break;
        }

        if(period != favorPeriod && isOnFavorPeriod)
        {
            List<Mission> missions = new List<Mission>(missionAskScript.savedMissions);
            foreach(Mission mission in missions)
            {
                if(mission.type == "distract" && mission.period == favorPeriod)
                {
                    Debug.Log("Removing distraction mission.");
                    missionAskScript.savedMissions.Remove(mission);
                }
            }
            isOnFavorPeriod = false;
        }

    }
}
