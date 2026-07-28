using System;
using System.Collections;
using UnityEngine;

public class GuardKeyCheck : MonoBehaviour
{
    private NPCCollectionData npcColData;
    private bool ready;
    private int keyID;
    private Solitary solitaryScript;
    private void Start()
    {
        solitaryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Solitary>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        npcColData = GetComponent<NPCCollectionData>();
        if (!name.Contains("Guard"))
        {
            enabled = false;
            yield break;
        }
        int npcNum = Convert.ToInt32(name.Replace("Guard", ""));
        if(npcNum > 5)
        {
            enabled = false;
            yield break;
        }

        switch (npcNum)
        {
            case 1:
                keyID = 0;
                break;
            case 2:
                keyID = 2;
                break;
            case 3:
                keyID = 3;
                break;
            case 4:
                keyID = 1;
                break;
            case 5:
                keyID = 4;
                break;
        }
        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }
        if (npcColData.npcData.isDead)
        {
            return;
        }

        bool hasKey = false;
        foreach(NPCInvItem item in npcColData.npcData.inventory)
        {
            if(item.itemData != null)
            {
                if(item.itemData.id == keyID)
                {
                    hasKey = true;
                    break;
                }
            }
        }
        if (!hasKey)
        {
            StartCoroutine(solitaryScript.GoToSolitary("CaughtKey"));
        }
    }
}
