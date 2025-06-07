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
    private string oldPeriod;
    private string oldPeriodCode;
    public void Start()
    {
        timeText = GetComponent<TMP_Text>();
        day = 1;
        sec = 50;
        min = 7;
        period = "Lights Out";
        periodCode = "LO";
        oldPeriod = period;
        oldPeriodCode = periodCode;
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
            if (time == "08:00")
            {
                period = "Morning Rollcall";
                periodCode = "MR";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "09:00")
            {
                period = "Breakfast";
                periodCode = "B";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "10:00")
            {
                period = "Free Period";
                periodCode = "FP";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "12:00")
            {
                period = "Lunch";
                periodCode = "L";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "13:00")
            {
                period = "Work Period";
                periodCode = "W";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "16:00")
            {
                period = "Exercise Period";
                periodCode = "EP";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "17:00")
            {
                period = "Showers";
                periodCode = "S";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "18:00")
            {
                period = "Dinner";
                periodCode = "D";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "19:00")
            {
                period = "Evening Free Period";
                periodCode = "EFP";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "22:00")
            {
                period = "Evening Rollcall";
                periodCode = "ER";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if (time == "23:00")
            {
                period = "Lights Out";
                periodCode = "LO";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else if(time == "07:50")
            {
                period = "Lights Out";
                periodCode = "LO";
                oldPeriod = period;
                oldPeriodCode = periodCode;
            }
            else
            {
                period = oldPeriod;
                periodCode = oldPeriodCode;
            }

            timeText.text = time + " - " + period + " (" + "Day " + day + ")";
        }
    }
}
