using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCInvData : MonoBehaviour
{
    public List<NPCInvItem> npcInv = new List<NPCInvItem>();
    public NPCInvItem weapon;
    public NPCInvItem outfit;
    public int inmateCorrelationNumber = -1;
    public int guardCorrelationNumber = -1;

    private void Start()
    {
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();

        Transform aStar = RootObjectCache.GetRoot("A*").transform;

        int i = 0;
        foreach(Transform npc in aStar)
        {
            if (npc.gameObject.CompareTag("Inmate"))
            {
                npc.AddComponent<NPCInvData>();
                npc.GetComponent<NPCInvData>().inmateCorrelationNumber = i;
                npc.GetComponent<NPCInvData>().npcInv = new List<NPCInvItem>(6);
                i++;
            }
        }
        if(inmateCorrelationNumber == -1)
        {
            int j = 0;
            foreach(Transform npc in aStar)
            {
                if (npc.gameObject.CompareTag("Guard"))
                {
                    npc.AddComponent<NPCInvData>();
                    npc.GetComponent<NPCInvData>().guardCorrelationNumber = j;
                    npc.GetComponent<NPCInvData>().npcInv = new List<NPCInvItem>(6);
                    j++;
                }
            }
        }
    }
}
