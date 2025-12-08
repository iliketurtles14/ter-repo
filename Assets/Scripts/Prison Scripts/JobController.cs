using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobController : MonoBehaviour
{
    private Map map;
    private LoadPrison loadPrisonScript;
    private Transform aStar;
    private Transform player;
    private void Start()
    {
        loadPrisonScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>();
        aStar = RootObjectCache.GetRoot("A*").transform;
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
        yield return new WaitForEndOfFrame();

        map = loadPrisonScript.currentMap;
        SetInitialJobs();
    }
    private void SetInitialJobs()
    {
        List<string> availableJobs = new List<string>();
        if (map.janitor)
        {
            availableJobs.Add("Janitor");
        }
        if (map.gardening)
        {
            availableJobs.Add("Gardening");
        }
        if (map.tailor)
        {
            availableJobs.Add("Tailorshop");
        }
        if (map.laundry)
        {
            availableJobs.Add("Laundry");
        }
        if (map.woodshop)
        {
            availableJobs.Add("Woodshop");
        }
        if (map.library)
        {
            availableJobs.Add("Library");
        }
        if (map.deliveries)
        {
            availableJobs.Add("Deliveries");
        }
        if (map.mailman)
        {
            availableJobs.Add("Mailman");
        }
        if (map.kitchen)
        {
            availableJobs.Add("Kitchen");
        }
        if (map.metalshop)
        {
            availableJobs.Add("Metalshop");
        }

        for(int i = 0; i < availableJobs.Count; i++)
        {
            if (availableJobs[i] == map.startingJob)
            {
                availableJobs.Remove(availableJobs[i]);
                break;
            }
        }

        foreach(Transform npc in aStar)
        {
            if(availableJobs.Count > 0 && !npc.GetComponent<NPCCollectionData>().npcData.isGuard)
            {
                int rand = UnityEngine.Random.Range(0, availableJobs.Count);
                npc.GetComponent<NPCCollectionData>().npcData.job = availableJobs[rand];
                availableJobs.Remove(availableJobs[rand]);
            }
            else
            {
                break;
            }
        }

        player.GetComponent<PlayerCollectionData>().playerData.job = map.startingJob;
    }
}
