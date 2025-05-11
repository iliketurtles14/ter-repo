using System;
using UnityEngine;

public class StatController : MonoBehaviour
{
    public GameObject playerMenuPanel;
    private GameObject intBar;
    private GameObject spdBar;
    private GameObject strBar;
    private PlayerData data;
    private bool isSet;
    public void Start()
    {
        data = GetComponent<PlayerCollectionData>().playerData;
        
        data.intellect = 30;
        data.strength = 30;
        data.speed = 30;

        intBar = Resources.Load<GameObject>("IntellectBar");
        spdBar = Resources.Load<GameObject>("SpeedBar");
        strBar = Resources.Load<GameObject>("StrengthBar");

    }
    public void Update()
    {
        if (!playerMenuPanel.GetComponent<PlayerIDInv>().idIsOpen)
        {
            isSet = false;
            return;
        }

        if (!isSet)
        {
            foreach (Transform bar in playerMenuPanel.transform.Find("StrengthPanel"))
            {
                Destroy(bar.gameObject);
            }
            foreach (Transform bar in playerMenuPanel.transform.Find("SpeedPanel"))
            {
                Destroy(bar.gameObject);
            }
            foreach (Transform bar in playerMenuPanel.transform.Find("IntellectPanel"))
            {
                Destroy(bar.gameObject);
            }

            for (int i = 0; i < Mathf.Floor(data.strength / 2); i++)
            {
                Instantiate(strBar, playerMenuPanel.transform.Find("StrengthPanel"));
            }
            for (int i = 0; i < Mathf.Floor(data.speed / 2); i++)
            {
                Instantiate(spdBar, playerMenuPanel.transform.Find("SpeedPanel"));
            }
            for (int i = 0; i < Mathf.Floor(data.intellect / 2); i++)
            {
                Instantiate(intBar, playerMenuPanel.transform.Find("IntellectPanel"));
            }

            isSet = true;
        }
    }
}
