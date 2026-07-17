using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GuardFlashlight : MonoBehaviour
{
    private float lookAngle;
    private Vector2 oldPos;
    private Vector2 currentPos;
    private Routine routineScript;
    private Transform flashlightObj;
    private Light2D light2d;
    private void Start()
    {
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        flashlightObj = transform.Find("FlashlightObj");
        light2d = flashlightObj.Find("Flashlight").GetComponent<Light2D>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        bool isGood = false;
        if (name.Contains("Guard"))
        {
            int num = Convert.ToInt32(name.Replace("Guard", ""));
            if(num >= 1 && num <= 5)
            {
                isGood = true;
            }
        }
        if (!isGood)
        {
            enabled = false;
            yield break;
        }
        flashlightObj.Find("Flashlight").GetComponent<Light2D>().lightCookieSprite = DataSender.instance.PrisonObjectImages[100];
        StartCoroutine(FlashlightLoop());
    }
    private IEnumerator FlashlightLoop()
    {
        while (true)
        {
            int min = routineScript.min;
            int sec = routineScript.sec;
            if((min == 22 && sec >= 20) || min == 23 || (min >= 0 && min <= 6) || (min == 7 && sec <= 50))
            {
                light2d.intensity = 10;
            }
            else
            {
                light2d.intensity = 0;
            }
            oldPos = transform.position;
            yield return new WaitForEndOfFrame();
            currentPos = transform.position;
            if (oldPos == currentPos)
            {
                continue;
            }
            Vector2 dir = currentPos - oldPos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            flashlightObj.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
