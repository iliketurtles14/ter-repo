using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalController : MonoBehaviour
{
    public List<Sprite> healthList = new List<Sprite>();
    public List<Sprite> energyList = new List<Sprite>();
    public List<Sprite> moneyList = new List<Sprite>();
    public List<Sprite> heatList = new List<Sprite>();
    public List<Sprite> friendsList = new List<Sprite>();
    public Sprite energyPercentage;
    public Sprite heatPercentage;
    private PlayerData data;
    public float energyRate;
    public float healthRate;
    private Vector2 sizeVector;
    public Transform ic;
    private int oldHealth;
    private int oldEnergy;
    private int oldMoney;
    private int oldHeat;
    private int oldFriends;


    public void Start()
    {
        energyRate = 5;
        healthRate = 3;
        data = GetComponent<PlayerCollectionData>().playerData;
        StartCoroutine(EnergyDeplete());
        StartCoroutine(HealthGain());
        StartCoroutine(HeatDeplete());
        data.health = 15;
        data.energy = 20;
        data.money = 10;
        data.heat = 0;
        oldHealth = data.health;
        oldEnergy = data.energy;
        oldMoney = data.money;
        oldHeat = data.heat;
    }

    public void Update()
    {
        if(oldHealth != data.health || oldEnergy != data.energy || oldMoney != data.money || oldHeat != data.heat)
        {
            oldHealth = data.health;
            oldEnergy = data.energy;
            oldMoney = data.money;
            oldHeat = data.heat;
            SetVitals();
        }
    }
    public void SetVitals()
    {
        foreach(Transform obj in ic.Find("EnergyPanel"))
        {
            Destroy(obj.gameObject);
        }
        foreach(Transform obj in ic.Find("HealthPanel"))
        {
            Destroy(obj.gameObject);
        }
        foreach(Transform obj in ic.Find("HeatPanel"))
        {
            Destroy(obj.gameObject);
        }
        foreach(Transform obj in ic.Find("MoneyPanel"))
        {
            Destroy(obj.gameObject);
        }
        foreach (char c in Convert.ToString(data.health))
        {
            int num = c - '0';
            if (num == 1)
            {
                sizeVector = new Vector2(30, 55);
            }
            else
            {
                sizeVector = new Vector2(40, 55);
            }

            GameObject letter = Instantiate(Resources.Load<GameObject>("VitalObj"), ic.Find("HealthPanel"));
            letter.GetComponent<Image>().sprite = healthList[num];
            letter.GetComponent<RectTransform>().sizeDelta = sizeVector;
        }
        foreach (char c in Convert.ToString(data.energy))
        {
            int num = c - '0';
            if (num == 1)
            {
                sizeVector = new Vector2(30, 55);
            }
            else
            {
                sizeVector = new Vector2(40, 55);
            }

            GameObject letter = Instantiate(Resources.Load<GameObject>("VitalObj"), ic.Find("EnergyPanel"));
            letter.GetComponent<Image>().sprite = energyList[num];
            letter.GetComponent<RectTransform>().sizeDelta = sizeVector;
        }
        GameObject energyPerc = Instantiate(Resources.Load<GameObject>("VitalObj"), ic.Find("EnergyPanel"));
        energyPerc.GetComponent<Image>().sprite = energyPercentage;
        energyPerc.GetComponent<RectTransform>().sizeDelta = new Vector2(45, 55);
        foreach (char c in Convert.ToString(data.heat))
        {
            int num = c - '0';
            if (num == 1)
            {
                sizeVector = new Vector2(30, 55);
            }
            else
            {
                sizeVector = new Vector2(40, 55);
            }

            GameObject letter = Instantiate(Resources.Load<GameObject>("VitalObj"), ic.Find("HeatPanel"));
            letter.GetComponent<Image>().sprite = heatList[num];
            letter.GetComponent<RectTransform>().sizeDelta = sizeVector;
        }
        GameObject heatPerc = Instantiate(Resources.Load<GameObject>("VitalObj"), ic.Find("HeatPanel"));
        heatPerc.GetComponent<Image>().sprite = heatPercentage;
        heatPerc.GetComponent<RectTransform>().sizeDelta = new Vector2(45, 55);
        foreach (char c in Convert.ToString(data.money))
        {
            int num = c - '0';
            if (num == 1)
            {
                sizeVector = new Vector2(30, 55);
            }
            else
            {
                sizeVector = new Vector2(40, 55);
            }

            GameObject letter = Instantiate(Resources.Load<GameObject>("VitalObj"), ic.Find("MoneyPanel"));
            letter.GetComponent<Image>().sprite = moneyList[num];
            letter.GetComponent<RectTransform>().sizeDelta = sizeVector;
        }
    }
    public IEnumerator EnergyDeplete()
    {
        while (true)
        {
            yield return new WaitForSeconds(energyRate);
            if(data.energy > 0)
            {
                data.energy--;
            }
            yield return null;
        }
    }//s = 2h
    public IEnumerator HealthGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(healthRate);
            if(data.health < Mathf.Floor(data.strength / 2))
            {
                data.health++;
            }
            yield return null;
        }
    }
    public IEnumerator HeatDeplete()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            if(data.heat > 0)
            {
                data.heat--;
            }
            yield return null;
        }
    }
}
