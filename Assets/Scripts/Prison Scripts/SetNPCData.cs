using System.Collections;
using UnityEngine;

public class SetNPCData : MonoBehaviour
{
    public void OnEnable()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        Set();
    }
    private void Set()
    {
        if (CompareTag("Guard"))
        {
            for (int i = 1; i <= 5; i++)
            {
                if (name == "Guard" + i)
                {
                    GetComponent<NPCCollectionData>().npcData.displayName = NPCSave.instance.npcNames[i + 8];
                }
            }
        }
        else if (CompareTag("Inmate"))
        {
            for (int i = 1; i <= 9; i++)
            {
                if (name == "Inmate" + i)
                {
                    GetComponent<NPCCollectionData>().npcData.displayName = NPCSave.instance.npcNames[i - 1];
                }
            }
        }
        else if (CompareTag("Player"))
        {
            GetComponent<PlayerCollectionData>().playerData.displayName = NPCSave.instance.playerName;
        }
    }
}
