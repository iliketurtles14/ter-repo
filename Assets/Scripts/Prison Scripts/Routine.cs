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
    private string period;
    public string periodCode;
    private Schedule scheduleScript;
    public void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        
        timeText = GetComponent<TMP_Text>();
        day = 1;
        sec = 50;
        min = 7;
        period = "Lights Out";
        periodCode = "LO";
        timeText.text = "07:50 - Lights Out (Day 1)";
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
    }
    public IEnumerator TimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            sec++;

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
