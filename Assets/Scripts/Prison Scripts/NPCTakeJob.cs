using System.Collections.Generic;
using UnityEngine;

public class NPCTakeJob : MonoBehaviour
{
    private Schedule scheduleScript;
    private NPCAI aiScript;
    private bool inJobPeriod;
    private NPCCollectionData npcColData;
    private Transform mc;
    private Dictionary<string, int> jobQuotaDict = new Dictionary<string, int>
    {
        { "Janitor", 5 }, { "Gardening", 5 }, { "Mailman", 3 }, { "Library", 3 }, { "Laundry", 15 },
        { "Deliveries", 7 }, { "Kitchen", 10 }, { "Tailor", 15 }, { "Woodshop", 15 }, { "Metalshop", 10 }
    };
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        aiScript = GetComponent<NPCAI>();
        npcColData = GetComponent<NPCCollectionData>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
    }
    private void Update()
    {
        if (string.IsNullOrEmpty(npcColData.npcData.job))
        {
            return;
        }
        if (!inJobPeriod && scheduleScript.periodCode == "W")
        {
            inJobPeriod = true;
        }
        if(inJobPeriod && scheduleScript.periodCode != "W")
        {
            if(aiScript.jobQuota < jobQuotaDict[npcColData.npcData.job])
            {
                npcColData.npcData.job = "";
                mc.Find("JobMenuPanel").GetComponent<JobMenu>().ResetJobButtons();
            }
            inJobPeriod = false;
        }
    }
}
