using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Routine : MonoBehaviour
{
    public int min = 7;
    public int sec = 50;
    private float interval = 0.75f;
    public string time;
    private bool withMinZero;
    private bool withSecZero;
    private TMP_Text timeText;
    public int day;
    private string period = "";
    public string periodCode = "";
    private Schedule scheduleScript;
    public bool isSpeedingUp;
    public bool isFrozen = true;
    public int startingMin;
    public int doorsOpenMin;
    public int doorsCloseMin;
    private Map currentMap;
    private CreateNote noteScript;
    private PauseController pc;
    private Transform tiles;
    private Transform badObjects;
    private MakeBadObject mbo;
    public void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        noteScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<CreateNote>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;
        mbo = RootObjectCache.GetRoot("ScriptObject").GetComponent<MakeBadObject>();

        timeText = GetComponent<TMP_Text>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
        bool gotStartingMin = false;
        for(int i = 0; i < 24; i++)
        {
            if (currentMap.routineDict[i] != "LO" && !gotStartingMin)
            {
                startingMin = i - 1;
                if(startingMin < 0)
                {
                    startingMin = 0;
                }
                gotStartingMin = true;
            }
            if (currentMap.routineDict[i] == "LO" && gotStartingMin)
            {
                doorsCloseMin = i - 1;
                doorsOpenMin = startingMin + 3;
            }
        }
        day = 1;
        sec = 50;
        min = startingMin;
        StartCoroutine(TimerCoroutine());
    }
    private void Update()
    {
        period = scheduleScript.period;
        periodCode = scheduleScript.periodCode;

        if (isSpeedingUp)
        {
            interval = .01f;
        }
        else
        {
            interval = .75f;
        }
    }
    public IEnumerator TimerCoroutine()
    {
        while (true)
        {
            float aTime = 0f;
            while(aTime < interval)
            {
                if (pc.isPaused || isFrozen)
                {
                    yield return null;
                    continue;
                }
                aTime += Time.deltaTime;
                yield return null;
            }

            sec++;

            if(sec == 55 && min == startingMin && day == 1)
            {
                if(string.IsNullOrEmpty(currentMap.note) && string.IsNullOrEmpty(currentMap.warden))
                {
                    //do nothing
                }
                else
                {
                    noteScript.CreateWardenNote("day1", currentMap.note, currentMap.warden);
                }
            }

            if(sec == 60)
            {
                sec = 0;
                min++;
            }

            if(min == 24)
            {
                min = 0;
            }

            if(sec == 0 && min == 0)
            {
                day++;

                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.CompareTag("Item"))
                    {
                        bool shouldPutBO = true;
                        foreach(Transform bo in badObjects)
                        {
                            if(bo.GetComponent<BadObjectData>().attachedObject == obj.gameObject)
                            {
                                shouldPutBO = false;
                                break;
                            }
                        }
                        if (shouldPutBO)
                        {
                            BadObjectData data = new BadObjectData
                            {
                                item = true,
                                attachedObject = obj.gameObject
                            };
                            mbo.CreateBadObject(data, "item");
                        }
                    }
                }
            }

            if(min < 10)
            {
                withMinZero = true;
            }
            else
            {
                withMinZero = false;
            }

            if(sec < 10)
            {
                withSecZero = true;
            }
            else
            {
                withSecZero = false;
            }

            if(withMinZero == true && withSecZero == true)
            {
                time = "0" + min + ":" + "0" + sec;
            }else if(withMinZero == true && withSecZero == false)
            {
                time = "0" + min + ":" + sec;
            }else if(withMinZero == false && withSecZero == true)
            {
                time = min + ":" + "0" + sec;
            }else if(withMinZero == false && withSecZero == false)
            {
                time = min + ":" + sec;
            }

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            timeText.text = time + " - " + period + " (" + "Day " + day + ")";
        }
    }
}
