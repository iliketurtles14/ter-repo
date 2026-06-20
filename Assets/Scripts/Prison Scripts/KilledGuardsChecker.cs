using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledGuardsChecker : MonoBehaviour//.2, .4, .8
{
    private List<GameObject> guards = new List<GameObject>();
    private Transform aStar;
    private bool ready = false;
    private List<NPCCollectionData> guardColDatas = new List<NPCCollectionData>();
    public int guardKillCount;
    public int guardAliveCount;
    private Lockdown lockdownScript;
    private Takeover takeoverScript;
    private void Start()
    {
        aStar = RootObjectCache.GetRoot("A*").transform;
        lockdownScript = GetComponent<Lockdown>();
        takeoverScript = GetComponent<Takeover>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach(Transform npc in aStar)
        {
            if (npc.name.Contains("Guard"))
            {
                guards.Add(npc.gameObject);
            }
        }
        foreach(GameObject guard in guards)
        {
            guardColDatas.Add(guard.GetComponent<NPCCollectionData>());
        }
        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        guardKillCount = 0;
        guardAliveCount = guardColDatas.Count;

        foreach(NPCCollectionData colData in guardColDatas)
        {
            if (colData.npcData.isDead)
            {
                guardKillCount++;
                guardAliveCount--;
            }
        }

        if(guardKillCount >= guardColDatas.Count * .4f && !lockdownScript.lockdownIsActive)
        {
            lockdownScript.StartLockdown();
            lockdownScript.isRiotLockdown = true;
        }
        if(guardKillCount >= guardColDatas.Count * .8f && !takeoverScript.takeoverIsActive)
        {
            takeoverScript.StartTakeover();
        }
        if(guardKillCount < guardColDatas.Count * .2f && lockdownScript.lockdownIsActive)
        {
            lockdownScript.StopLockdown();
            if (takeoverScript.takeoverIsActive)
            {
                takeoverScript.StopTakeover();
            }
        }
    }
}
