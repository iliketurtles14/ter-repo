using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    private GameObject playerMenuPanel;
    private PlayerData data;
    private bool isSet;
    private bool isReady;
    public void Start()
    {
        playerMenuPanel = RootObjectCache.GetRoot("MenuCanvas").transform.Find("PlayerMenuPanel").gameObject;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        data = GetComponent<PlayerCollectionData>().playerData;
        data.intellect = 30;
        data.strength = 30;
        data.speed = 30;
        isReady = true;
    }
    public void Update()
    {
        if (!isReady)
        {
            return;
        }
        
        if (!playerMenuPanel.GetComponent<PlayerIDInv>().idIsOpen)
        {
            isSet = false;
            return;
        }

        if (!isSet)
        {
            playerMenuPanel.transform.Find("StrengthPanel").Find("StrengthBar").GetComponent<Image>().enabled = true;
            playerMenuPanel.transform.Find("SpeedPanel").Find("SpeedBar").GetComponent<Image>().enabled = true;
            playerMenuPanel.transform.Find("IntellectPanel").Find("IntellectBar").GetComponent<Image>().enabled = true;
            playerMenuPanel.transform.Find("StrengthPanel").Find("StrengthBar").GetComponent<RectTransform>().sizeDelta = new Vector2(data.strength / 2 * 5, 25);    // holy magic numbers lmao
            playerMenuPanel.transform.Find("StrengthPanel").Find("StrengthBar").GetComponent<RectTransform>().anchoredPosition = new Vector2(data.strength / 2 * 2.5f + 454.5f, -102);
            playerMenuPanel.transform.Find("SpeedPanel").Find("SpeedBar").GetComponent<RectTransform>().sizeDelta = new Vector2(data.speed / 2 * 5, 25);
            playerMenuPanel.transform.Find("SpeedPanel").Find("SpeedBar").GetComponent<RectTransform>().anchoredPosition = new Vector2(data.speed / 2 * 2.5f + 454.5f, -157.5f);
            playerMenuPanel.transform.Find("IntellectPanel").Find("IntellectBar").GetComponent<RectTransform>().sizeDelta = new Vector2(data.intellect / 2 * 5, 25);
            playerMenuPanel.transform.Find("IntellectPanel").Find("IntellectBar").GetComponent<RectTransform>().anchoredPosition = new Vector2(data.intellect / 2 * 2.5f + 454.5f, -213);

            isSet = true;
        }
    }
}
