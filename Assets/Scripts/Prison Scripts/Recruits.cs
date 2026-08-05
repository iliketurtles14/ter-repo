using NUnit.Framework;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Recruits : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Transform player;
    private Combat combatScript;
    private PlayerCollectionData playerColData;
    private Transform aStar;
    public List<Transform> recruits = new List<Transform>();
    private WarningMessage warningScript;
    private Map currentMap;
    private string[] speechFile;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        combatScript = player.GetComponent<Combat>();
        playerColData = player.GetComponent<PlayerCollectionData>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        warningScript = GetComponent<WarningMessage>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = GetComponent<LoadPrison>().currentMap;
        speechFile = currentMap.speech;
    }
    private void Update()
    {
        playerColData.playerData.friends = recruits.Count;

        List<Transform> recruitsToDisband = new List<Transform>();
        foreach(Transform recruit in recruits)
        {
            if (recruit.GetComponent<NPCCollectionData>().npcData.isDead)
            {
                recruitsToDisband.Add(recruit);
            }
        }
        foreach(Transform recruit in recruitsToDisband)
        {
            Disband(recruit);
        }

        if(recruits.Count > 0 && Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground")))
        {
            foreach(Transform recruit in recruits)
            {
                Disband(recruit);
            }
        }
        
        if (playerColData.playerData.friends > 0 && combatScript.isLockedOn)
        {
            foreach(Transform recruit in recruits)
            {
                NPCCombat npcCombat = recruit.GetComponent<NPCCombat>();
                npcCombat.isAggro = true;
                npcCombat.target = combatScript.targetNPC;
            }
        }

        foreach(Transform recruit in recruits)
        {
            if (!recruit.GetComponent<NPCCombat>().isAggro)
            {
                if (recruit.GetComponent<NPCCollectionData>().npcData.opinion < 80 ||
                    recruit.GetComponent<NPCCollectionData>().npcData.isDead)
                {
                    Disband(recruit);
                }
                else
                {
                    Recruit(recruit, false, false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && mcs.isTouchingNPC && mcs.touchedNPC.name.Contains("Inmate") &&
            !mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isDead &&
            !mcs.touchedNPC.GetComponent<NPCCollectionData>().npcData.isAggro)
        {
            Transform npc = mcs.touchedNPC.transform;
            if (!npc.GetComponent<NPCCollectionData>().npcData.isRecruited &&
                npc.GetComponent<NPCCollectionData>().npcData.opinion >= 80)
            {
                Recruit(npc, true, false);
            }
            else if(npc.GetComponent<NPCCollectionData>().npcData.isRecruited)
            {
                PSoundController.PlaySound("step");
                StartCoroutine(warningScript.CreateWarningMessage(GetMessage("Dismiss")));
                Disband(npc);
            }
        }
    }
    public void Recruit(Transform npc, bool addToRecruitNum, bool isLoading)
    {
        Debug.Log("recruitng");
        if (addToRecruitNum && !isLoading)
        {
            StartCoroutine(warningScript.CreateWarningMessage(GetMessage("Follow")));
            PSoundController.PlaySound("step");
        }
        npc.GetComponent<NPCAI>().enabled = false;
        npc.GetComponent<AILerp>().enabled = false;
        npc.GetComponent<NavMeshAgent>().enabled = true;
        npc.GetComponent<NavMeshAgent>().destination = player.position;
        npc.GetComponent<NavMeshAgent>().stoppingDistance = 2.8f;

        NPCCollectionData npcColData = npc.GetComponent<NPCCollectionData>();
        npcColData.npcData.hasFood = false;
        npcColData.npcData.isSleeping = false;

        if (addToRecruitNum)
        {
            recruits.Add(npc);
        }

        combatScript.LockOff();

        npcColData.npcData.isRecruited = true;

    }
    public void Disband(Transform npc)
    {
        Debug.Log("disbanding");
        npc.GetComponent<NavMeshAgent>().stoppingDistance = 0;
        npc.GetComponent<NavMeshAgent>().enabled = false;
        if (!npc.GetComponent<NPCCollectionData>().npcData.isDead)
        {
            npc.GetComponent<AILerp>().enabled = true;
            npc.GetComponent<NPCAI>().enabled = true;
        }

        npc.GetComponent<NPCCollectionData>().npcData.isRecruited = false;

        recruits.Remove(npc);
    }
    public string GetMessage(string messageType)
    {
        int count = Convert.ToInt32(GetINIVar(messageType, "Count", speechFile));
        int rand = UnityEngine.Random.Range(1, count + 1);
        return GetINIVar(messageType, rand.ToString(), speechFile);
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Split('=')[0] == varName)
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }



        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }
}
