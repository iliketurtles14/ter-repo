using NUnit.Framework;
using Pathfinding;
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
    private List<Transform> recruits = new List<Transform>();
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        combatScript = player.GetComponent<Combat>();
        playerColData = player.GetComponent<PlayerCollectionData>();
        aStar = RootObjectCache.GetRoot("A*").transform;
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
                    Recruit(recruit, false);
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
                Recruit(npc, true);
            }
            else if(npc.GetComponent<NPCCollectionData>().npcData.isRecruited)
            {
                Disband(npc);
            }
            else if(!npc.GetComponent<NPCCollectionData>().npcData.isRecruited &&
                npc.GetComponent<NPCCollectionData>().npcData.opinion < 80)
            {
                //play sound
            }
        }
    }
    public void Recruit(Transform npc, bool addToRecruitNum)
    {
        Debug.Log("recruitng");
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
}
