using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoors : MonoBehaviour
{
    private Transform tiles;
    private List<GameObject> yellowDoors = new List<GameObject>();
    private List<GameObject> orangeDoors = new List<GameObject>();
    private List<GameObject> redDoors = new List<GameObject>();
    private List<GameObject> greenDoors = new List<GameObject>();
    private List<GameObject> purpleDoors = new List<GameObject>();
    private List<GameObject> whiteDoors = new List<GameObject>();
    private List<GameObject> guardDoors = new List<GameObject>();
    private Dictionary<string, List<GameObject>> typeDict;
    private Dictionary<string, Sprite> spriteDict;
    private Sprite unlockedSprite;
    private Schedule scheduleScript;
    private Routine routineScript;
    private List<string> unlockedTypes = new List<string>();
    private bool ready;
    private bool doingCellLockWait;
    private Transform aStar;
    private PauseController pc;
    private List<NPCCollectionData> npcColDatas = new List<NPCCollectionData>();
    private Zones zonesScript;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        pc = GetComponent<PauseController>();
        zonesScript = GetComponent<Zones>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach (Transform npc in aStar)
        {
            if (npc.name.Contains("Inmate"))
            {
                npcColDatas.Add(npc.GetComponent<NPCCollectionData>());
            }
        }
        MakeLists();
        ready = true;
    } 
    private void MakeLists()
    {
        List<string> layers = new List<string>
        {
            "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
        };
        
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(layers[i]))
            {
                switch (obj.name)
                {
                    case "CellDoor":
                        yellowDoors.Add(obj.gameObject);
                        break;
                    case "UtilityDoor":
                        orangeDoors.Add(obj.gameObject);
                        break;
                    case "EnteranceDoor":
                        purpleDoors.Add(obj.gameObject);
                        break;
                    case "WorkDoor":
                        greenDoors.Add(obj.gameObject);
                        break;
                    case "StaffDoor":
                        redDoors.Add(obj.gameObject);
                        break;
                    case "WhiteDoor":
                        whiteDoors.Add(obj.gameObject);
                        break;
                    case "GuardDoor":
                        guardDoors.Add(obj.gameObject);
                        break;
                }
            }
        }

        typeDict = new Dictionary<string, List<GameObject>>
        {
            { "yellow", yellowDoors }, { "orange", orangeDoors }, { "red", redDoors }, { "green", greenDoors },
            { "purple", purpleDoors }, { "white", whiteDoors }, { "guard", guardDoors }
        };
        List<Sprite> objSprites = new List<Sprite>(DataSender.instance.PrisonObjectImages);
        spriteDict = new Dictionary<string, Sprite>
        {
            { "yellow", objSprites[178] }, { "orange", objSprites[175] }, { "red", objSprites[176] },
            { "green", objSprites[174] }, { "purple", objSprites[177] }, { "white", objSprites[190] },
            { "guard", objSprites[327] }
        };
        unlockedSprite = objSprites[179];
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if(scheduleScript.periodCode != "LO" && !unlockedTypes.Contains("yellow"))
        {
            aUnlockDoors(new List<string> { "yellow" });
        }
        if(routineScript.doorsOpenMin == routineScript.min && !unlockedTypes.Contains("purple"))
        {
            aUnlockDoors(new List<string> { "purple" });
        }
        if(routineScript.doorsCloseMin == routineScript.min && unlockedTypes.Contains("purple"))
        {
            LockDoors(new List<string> { "purple" });
        }
        if(unlockedTypes.Contains("yellow") && scheduleScript.periodCode == "LO" && !doingCellLockWait)
        {
            doingCellLockWait = true;
            StartCoroutine(CellLockWait());
        }
    }
    private IEnumerator CellLockWait()
    {
        while (true)//wait till all npcs are in bed
        {
            bool notSleeping = false;
            foreach(NPCCollectionData npcColData in npcColDatas)
            {
                if (!npcColData.npcData.isSleeping)
                {
                    notSleeping = true;
                    break;
                }
            }
            if (notSleeping)
            {
                yield return null;
                continue;
            }
            else
            {
                break;
            }
        }
        float time = 0;
        while (true)//wait 14 seconds for the player to get in bed
        {
            if (zonesScript.isTouchingYourCell)
            {
                break;
            }
            else
            {
                yield return null;
                if (!pc.isPaused)
                {
                    time += Time.deltaTime;
                }
                if (time >= 14)
                {
                    break;
                }
                continue;
            }
        }
        time = 0;
        while (true) //wait a safety second for when touching cell zone
        {
            yield return null;
            if (!pc.isPaused)
            {
                time += Time.deltaTime;
                if(time >= 1)
                {
                    break;
                }
            }
        }
        LockDoors(new List<string> { "yellow" }); //lock doors
        doingCellLockWait = false;
    }
    public void LockDoors(List<string> types) //yellow, orange, red, green, purple, white, guard
    {
        for(int i = 0; i < types.Count; i++)
        {
            if (unlockedTypes.Contains(types[i]))
            {
                unlockedTypes.Remove(types[i]);
            }
            foreach(GameObject door in typeDict[types[i]])
            {
                door.name = door.name.Replace("Open", "");
                door.GetComponent<SpriteRenderer>().sprite = spriteDict[types[i]];
            }
        }
    }
    public void aUnlockDoors(List<string> types) //yellow, orange, red, green, purple, white, guard
    {
        for(int i = 0; i < types.Count; i++)
        {
            unlockedTypes.Add(types[i]);
            foreach(GameObject door in typeDict[types[i]])
            {
                door.name = "Open" + door.name;
                door.GetComponent<SpriteRenderer>().sprite = unlockedSprite;
            }
        }
    }
}
