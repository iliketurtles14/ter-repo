using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    private string[] speechFile;
    private Map currentMap;
    private Transform aStar;
    private ApplyPrisonData applyScript;
    private List<int> guardItems = new List<int>();
    private List<int> stolenItems = new List<int>();
    private List<int> giveItems = new List<int>();
    private List<string> missions = new List<string>
    {
        "inmateBeat",
        "guardBeat",
        "distract",
        "give",
        "stealInmateBody",
        "stealDesk",
        "stealGuardBody"
    };
    private void Start()
    {
        aStar = RootObjectCache.GetRoot("A*").transform;
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
        speechFile = currentMap.speech;

        foreach(Transform npc in aStar)
        {
            npc.Find("IconCanvas").Find("MissionIcon").gameObject.SetActive(false);
        }

        MakeItemLists();
        StartCoroutine(MissionsLoop());
    }
    private IEnumerator MissionsLoop()
    {
        List<GameObject> availableInmates = new List<GameObject>();
        foreach(Transform npc in aStar)
        {
            if (npc.name.Contains("Inmate"))
            {
                npc.GetComponent<NPCCollectionData>().npcData.mission = new Mission("", -1, "", "", "", "");
                availableInmates.Add(npc.gameObject);
            }
        }
        while (true) //a bunch of gross magic numbers. sorryyyy
        {
            int available = 0;
            int amountOfMissions = 0;
            foreach(GameObject npc in availableInmates)
            {
                if(String.IsNullOrEmpty(npc.GetComponent<NPCCollectionData>().npcData.mission.type))
                {
                    available++;
                }
                else
                {
                    amountOfMissions++;
                }
            }
            if (available > 0 && amountOfMissions < 4)
            {
                float rand = UnityEngine.Random.Range(30f, 80f);
                yield return new WaitForSeconds(rand * .75f);
                int randInt = UnityEngine.Random.Range(0, availableInmates.Count);
                MakeMission(availableInmates[randInt]);
                availableInmates.RemoveAt(randInt);
            }
            yield return null;
        }
    }
    public Mission MakeMission(GameObject inmate)
    {
        string type = null;
        int item = -1;
        string giver = inmate.GetComponent<NPCCollectionData>().npcData.displayName;
        string target = null;
        string message = null;
        string period = null;

        int rand = UnityEngine.Random.Range(0, 7);

        type = missions[rand];

        switch (type)
        {
            case "inmateBeat":
                int count = Convert.ToInt32(GetINIVar("Missions_InmateBeat", "Count", speechFile));
                rand = UnityEngine.Random.Range(1, count + 1);
                message = GetINIVar("Missions_InmateBeat", rand.ToString(), speechFile);
                List<string> remainingInmates = GetRemainingNPCs(giver, "Inmate");
                rand = UnityEngine.Random.Range(0, remainingInmates.Count);
                target = remainingInmates[rand];
                break;
            case "guardBeat":
                count = Convert.ToInt32(GetINIVar("Missions_GuardBeat", "Count", speechFile));
                rand = UnityEngine.Random.Range(1, count + 1);
                message = GetINIVar("Missions_GuardBeat", rand.ToString(), speechFile);
                List<string> remainingGuards = GetRemainingNPCs(giver, "Guard");
                rand = UnityEngine.Random.Range(0, remainingGuards.Count);
                target = remainingGuards[rand];
                break;
            case "distract":
                count = Convert.ToInt32(GetINIVar("Missions_Distract", "Count", speechFile));
                rand = UnityEngine.Random.Range(1, count + 1);
                message = GetINIVar("Missions_Distract", rand.ToString(), speechFile);
                List<string> periods = GetAvailablePeriods();
                rand = UnityEngine.Random.Range(0, periods.Count);
                period = periods[rand];
                break;
            case "give":
                count = Convert.ToInt32(GetINIVar("Missions_Give", "Count", speechFile));
                rand = UnityEngine.Random.Range(1, count + 1);
                message = GetINIVar("Missions_Give", rand.ToString(), speechFile);
                rand = UnityEngine.Random.Range(0, giveItems.Count);
                item = giveItems[rand];
                break;
            case "stealDesk":
            case "stealInmateBody":
                count = Convert.ToInt32(GetINIVar("Missions_StealInmate", "Count", speechFile));
                rand = UnityEngine.Random.Range(1, count + 1);
                message = GetINIVar("Missions_StealInmate", rand.ToString(), speechFile);
                rand = UnityEngine.Random.Range(0, stolenItems.Count);
                item = stolenItems[rand];
                remainingInmates = GetRemainingNPCs(giver, "Inmate");
                rand = UnityEngine.Random.Range(0, remainingInmates.Count);
                target = remainingInmates[rand];
                break;
            case "stealGuardBody":
                count = Convert.ToInt32(GetINIVar("Missions_StealGuard", "Count", speechFile));
                rand = UnityEngine.Random.Range(1, count + 1);
                message = GetINIVar("Missions_StealGuard", rand.ToString(), speechFile);
                rand = UnityEngine.Random.Range(0, guardItems.Count);
                item = guardItems[rand];
                remainingGuards = GetRemainingNPCs(giver, "Guard");
                rand = UnityEngine.Random.Range(0, remainingGuards.Count);
                target = remainingGuards[rand];
                break;
        }
        Mission mission = new Mission(type, item, giver, target, message, period);

        SetMissionIcon(inmate);

        return mission;
    }
    private void SetMissionIcon(GameObject inmate)
    {
        inmate.transform.Find("IconCanvas").Find("MissionIcon").GetComponent<Image>().sprite = applyScript.UISprites[188];
        inmate.transform.Find("IconCanvas").Find("MissionIcon").gameObject.SetActive(true);
    }
    private void MakeItemLists()
    {
        for(int i = 0; i < 273; i++)
        {
            string str = i.ToString();
            if(str.Length == 1)
            {
                str = "00" + str;
            }
            else if(str.Length == 2)
            {
                str = "0" + str;
            }

            if(GetINIVar(str, "InItemFetchFavors", currentMap.items) == "True")
            {
                giveItems.Add(i);
            }
            if(GetINIVar(str, "InGuardStolenFavors", currentMap.items) == "True")
            {
                guardItems.Add(i);
            }
            if(GetINIVar(str, "InStolenItemFavors", currentMap.items) == "True")
            {
                stolenItems.Add(i);
            }
        }
    }
    private List<string> GetRemainingNPCs(string inmateName, string type) //gets DISPLAYNAME!!!!
    {
        List<string> list = new List<string>();
        foreach(Transform npc in aStar)
        {
            string name = npc.GetComponent<NPCCollectionData>().npcData.displayName;
            string objName = npc.name;
            if(objName.StartsWith(type) && name != inmateName)
            {
                list.Add(name);
            }
        }
        return list;
    }
    private List<string> GetAvailablePeriods()
    {
        List<string> periodCodes = new List<string>();
        
        for(int i = 0; i < 24; i++)
        {
            periodCodes.Add(currentMap.routineDict[i]);
        }

        List<string> periods = new List<string>();

        foreach(string periodCode in periodCodes)
        {
            string period = null;
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

            if (!periods.Contains(period))
            {
                periods.Add(period);
            }
        }
        return periods;
    }
    public List<string> GetINISet(string header, string[] file) //man i love copy and pasting these methods everywhere
    {
        int startLine = -1;
        int endLine = file.Length;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains($"[{header}]"))
            {
                startLine = i + 1;
                break;
            }
        }

        if (startLine == -1)
            return new List<string>();

        for (int i = startLine; i < file.Length; i++)
        {
            if (file[i].StartsWith("[") && file[i].EndsWith("]"))
            {
                endLine = i;
                break;
            }
        }

        List<string> setList = new List<string>();
        for (int i = startLine; i < endLine; i++)
        {
            if (file[i].Contains('='))
            {
                setList.Add(file[i]);
            }
        }

        return setList;
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Split('=')[0] == varName)
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }

        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }
}
