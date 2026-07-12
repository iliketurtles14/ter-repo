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
    private NPCAI aiScript;
    private Schedule scheduleScript;
    private PauseController pc;
    private List<Transform> inmates = new List<Transform>();
    private Transform aStar;
    private NPCCombat combatScript;
    private NPCCollectionData npcColData;
    private InmateFightCooldown cooldownScript;
    private void Start()
    {
        aiScript = GetComponent<NPCAI>();
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        combatScript = GetComponent<NPCCombat>();
        npcColData = GetComponent<NPCCollectionData>();
        cooldownScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<InmateFightCooldown>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (!name.Contains("Inmate"))
        {
            enabled = false;
            yield break;
        }
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
            if(!aiScript.enabled || scheduleScript.periodCode == "R" || scheduleScript.periodCode == "LO" || scheduleScript.periodCode == "LD" || npcColData.npcData.isDead)
            {
                yield return null;
                continue;
            }
            //figure out what to do here. the problem is the cooldown int. it should be above the inmates and not on each of them.
            List<Transform> availableInmates = new List<Transform>();
            foreach(Transform inmate in inmates)
            {
                if (inmate.GetComponent<NPCCollectionData>().npcData.isDead)
                {
                    continue;
                }
                if(Vector2.Distance(inmate.position, transform.position) <= 7.2f)
                {
                    availableInmates.Add(inmate);
                }
            }
            if(availableInmates.Count == 0)
            {
                yield return null;
                continue;
            }

            float time = 0f;
            while(time < 1f / 45f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }
            int rand = UnityEngine.Random.Range(0, 3);
            cooldownScript.cooldown += rand;

            if(cooldownScript.cooldown >= 4000)
            {
                combatScript.isAggro = true;
                rand = UnityEngine.Random.Range(0, availableInmates.Count);
                combatScript.target = availableInmates[rand].gameObject;
                cooldownScript.cooldown = 0;
            }
        }
    }
}
