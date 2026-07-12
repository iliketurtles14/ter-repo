using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class IGTimer : MonoBehaviour
{
    public float igt;
    private float pausedIGT;
    private TextMeshProUGUI tmp;
    private TextMeshProUGUI pausedTMP;
    private PauseController pc;
    private void Start()
    {
        tmp = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("IGT").GetComponent<TextMeshProUGUI>();
        pausedTMP = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("PausedIGT").GetComponent<TextMeshProUGUI>();
        pc = GetComponent<PauseController>();
        StartCoroutine(TimerLoop());
    }
    private IEnumerator TimerLoop()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1);
        while (true)
        {
            igt += Time.deltaTime;
            if (!pc.isPaused)
            {
                pausedIGT += Time.deltaTime;
            }

            igt = Convert.ToSingle(Math.Round(Convert.ToDouble(igt), 3));
            string dec = "";
            if (igt.ToString().Contains("."))
            {
                dec = igt.ToString().Split(".")[1];
            }
            else
            {
                dec = "0";
            }

            string igtStr = "";
            if(Mathf.FloorToInt(igt / 60f) == 0)
            {
                igtStr =(Mathf.FloorToInt(igt) % 60f).ToString() + "." + dec;
            }
            else
            {
                igtStr = Mathf.FloorToInt(igt / 60f).ToString() + ":" + (Mathf.FloorToInt(igt) % 60f).ToString() + "." + dec;
            }
            tmp.text = igtStr;

            pausedIGT = Convert.ToSingle(Math.Round(Convert.ToDouble(pausedIGT), 3));
            dec = "";
            if (pausedIGT.ToString().Contains("."))
            {
                dec = pausedIGT.ToString().Split(".")[1];
            }
            else
            {
                dec = "0";
            }

            if(Mathf.FloorToInt(pausedIGT / 60f) == 0)
            {
                igtStr = (Mathf.FloorToInt(pausedIGT) % 60f).ToString() + "." + dec;
            }
            else
            {
                igtStr = Mathf.FloorToInt(pausedIGT / 60f).ToString() + ":" + (Mathf.FloorToInt(pausedIGT) % 60f).ToString() + "." + dec;
            }
            pausedTMP.text = igtStr;
            yield return null;
        }
    }
}
