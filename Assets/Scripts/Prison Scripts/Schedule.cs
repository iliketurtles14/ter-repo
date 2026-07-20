using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Schedule : MonoBehaviour
{
    private int min;
    private int sec;
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
    private BellRing ringScript;
    private bool changedPeriod;
    private List<string> validJobs = new List<string>()
    {
        "Janitor", "Gardening", "Kitchen", "Woodshop", "Metalshop", "Mailman", "Deliveries",
        "Tailor", "Laundry", "Library"
    };
    private Lockdown lockdownScript;
    private PlayerCollectionData playerColData;
    private List<string> allowedPeriods = new List<string>
    {
        "B", "L", "D", "R", "E", "FT", "LO", "S", "W"
    };
    void Start()
    {
        TimeObject = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").gameObject;
        timeScript = TimeObject.GetComponent<Routine>();
        period = "Lights Out";
        periodCode = "LO";
        loadPrisonScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>();
        missionAskScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("MissionPanel").GetComponent<MissionAsk>();
        lockdownScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Lockdown>();
        playerColData = RootObjectCache.GetRoot("Player").GetComponent<PlayerCollectionData>();
        ringScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<BellRing>();

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
        
        min = timeScript.min;
        sec = timeScript.sec;

        if (lockdownScript.lockdownIsActive)
        {
            periodCode = "LD";
        }
        else if (!changedPeriod && sec == 0 && ((min - 1 != -1 && map.routineDict[min] != map.routineDict[min - 1]) || (min - 1 == -1 && map.routineDict[min] != map.routineDict[23])))
        {
            changedPeriod = true;
            ringScript.RingBell();
            periodCode = map.routineDict[min];
            if (!allowedPeriods.Contains(periodCode))
            {
                periodCode = "FT";
            }
        }

        if(sec != 0)
        {
            changedPeriod = false;
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
                if (validJobs.Contains(playerColData.playerData.job))
                {
                    period = playerColData.playerData.job + " Job";
                }
                else
                {
                    period = "Work";
                }
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
            case "LD":
                period = "Lockdown";
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
    public void ChangePeriod()//only for Lockdown.cs
    {
        periodCode = map.routineDict[min];
        if (!allowedPeriods.Contains(periodCode))
        {
            periodCode = "FT";
        }
    }
}
