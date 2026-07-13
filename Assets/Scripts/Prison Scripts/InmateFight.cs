using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class InmateFight : MonoBehaviour
{
    //for inmate to player:          (this is not in this script; its in InmatePlayerFight.cs)
    //only during work period
    //every 1000/45 seconds, do a 1/5 chance every .5 seconds if the player is within a 4.5 tile radius of an inmate with an opinion less than 15
    //if the chance hits, choose an inmate within range to attack the player
    //wait the 1000/45 seconds again and repeat.
    //make sure inmate isnt dead or already attacking someone

    //for inmate to inmate:
    //only if not fighting and not lockdown/rollcall/lights out
    //start an integer at 0
    //every 1/45 seconds increase it by Random(3) if two inmates are within a 4.5 tile radius of each other
    //once this int is equal or greater than 4000, choose inmate to attack within range
    //make this int 0 and repeat
    private Schedule scheduleScript;
    private PauseController pc;
    private List<Transform> inmates = new List<Transform>();
    private Transform aStar;
    private int cooldown;
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        aStar = RootObjectCache.GetRoot("A*").transform;
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
            if(npc.name.Contains("Inmate"))
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
            if(scheduleScript.periodCode == "R" || scheduleScript.periodCode == "LO" || scheduleScript.periodCode == "LD")
            {
                yield return null;
                continue;
            }

            float time = 0f;
            while (time < 1f / 45f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }

            List<Transform> availableInmates = new List<Transform>();
            foreach(Transform inmate1 in inmates)
            {
                NPCCollectionData npcColData = inmate1.GetComponent<NPCCollectionData>();
                if (npcColData.npcData.isDead || npcColData.npcData.isAggro)
                {
                    continue;
                }
                foreach(Transform inmate2 in inmates)
                {
                    npcColData = inmate2.GetComponent<NPCCollectionData>();
                    if (npcColData.npcData.isDead || npcColData.npcData.isAggro)
                    {
                        continue;
                    }
                    if(inmate1.name != inmate2.name && Vector2.Distance(inmate1.position, inmate2.position) <= 7.2f)
                    {
                        availableInmates.Add(inmate1);
                    }
                }
            }
            if(availableInmates.Count < 2)
            {
                yield return null;
                continue;
            }

            int rand = UnityEngine.Random.Range(0, 3);
            cooldown += rand;

            if(cooldown >= 4000)
            {
                Transform inmate1;
                Transform inmate2;

                List<Transform> randInmates = new List<Transform>(availableInmates);

                rand = UnityEngine.Random.Range(0, randInmates.Count);
                inmate1 = randInmates[rand];

                randInmates.Remove(inmate1);

                rand = UnityEngine.Random.Range(0, randInmates.Count);
                inmate2 = randInmates[rand];

                NPCCombat combatScript = inmate1.GetComponent<NPCCombat>();
                
                combatScript.isAggro = true;
                combatScript.target = inmate2.gameObject;

                cooldown = 0;
            }
        }
    }
}
