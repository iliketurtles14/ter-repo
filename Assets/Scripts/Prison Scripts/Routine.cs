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
    public void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        noteScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<CreateNote>();

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
    }
    public void OnEnable()
    {
        StartCoroutine(TimerCoroutine());
    }
    public void OnDisable()
    {
        StopAllCoroutines();
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
            if (isFrozen)
            {
                yield return null;
                continue;
            }
            yield return new WaitForSeconds(interval);

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
