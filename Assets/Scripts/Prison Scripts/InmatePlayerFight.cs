using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InmatePlayerFight : MonoBehaviour
{
    private List<Transform> inmates = new List<Transform>();
    private Transform aStar;
    private Schedule scheduleScript;
    private PauseController pc;
    private Transform player;
    private void Start()
    {
        aStar = RootObjectCache.GetRoot("A*").transform;
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        pc = GetComponent<PauseController>();
        player = RootObjectCache.GetRoot("Player").transform;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach (Transform npc in aStar)
        {
            if (npc.name.Contains("Inmate"))
            {
                inmates.Add(npc);
            }
        }
        StartCoroutine(FightLoop());
    }
    private IEnumerator FightLoop()
    {
        while (true)
        {
            if(scheduleScript.periodCode != "W")
            {
                yield return null;
                continue;
            }

            float time = 0f;
            while(time < 1000f / 45f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                if(scheduleScript.periodCode != "W")
                {
                    break;
                }
                time += Time.deltaTime;
                yield return null;
            }
            if(scheduleScript.periodCode != "W")
            {
                yield return null;
                continue;
            }

            //this is gonna be so aids to code
            while (true)
            {
                time = 0f;
                while(time < .5f)
                {
                    if (pc.isPaused)
                    {
                        yield return null;
                        continue;
                    }
                    if(scheduleScript.periodCode != "W")
                    {
                        break;
                    }
                    time += Time.deltaTime;
                    yield return null;
                }
                if(scheduleScript.periodCode != "W")
                {
                    break;
                }
                int rand = UnityEngine.Random.Range(0, 5);
                if(rand == 1)
                {
                    CheckForFight();
                }
            }
            if(scheduleScript.periodCode != "W")
            {
                yield return null;
                continue;
            }
        }
    }
    private void CheckForFight()
    {
        List<Transform> availableInmates = new List<Transform>();
        foreach(Transform inmate in inmates)
        {
            if(inmate.GetComponent<NPCCollectionData>().npcData.opinion < 15 && !inmate.GetComponent<NPCCollectionData>().npcData.isDead && !inmate.GetComponent<NPCCollectionData>().npcData.isAggro && Vector2.Distance(inmate.position, player.position) <= 7.2f)
            {
                availableInmates.Add(inmate);
            }
        }
        if(availableInmates.Count == 0)
        {
            return;
        }
        int rand = UnityEngine.Random.Range(0, availableInmates.Count);
        NPCCombat combat = availableInmates[rand].GetComponent<NPCCombat>();
        combat.isAggro = true;
        combat.target = player.gameObject;
    }
}
