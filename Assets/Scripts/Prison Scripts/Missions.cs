using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{
    private string[] speechFile;
    private Map currentMap;
    private Transform aStar;
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
    }
    private void MakeMission(GameObject inmate)
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
                message = GetINIVar("Missions_InmateBeat", rand.ToString(), speechFile);
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
            //case "give":
            //    count = Convert.ToInt32(GetINIVar("Missions_Give", "Count", speechFile));
            //    rand = UnityEngine.Random.Range(1, count + 1);
            //    message = GetINIVar("Missions_Give", rand.ToString(), speechFile);

        }
        Mission mission = new Mission(type, item, giver, target, message, period);
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
