using UnityEngine;

public class SetNPCData : MonoBehaviour
{
    public void OnEnable()
    {
        if(tag == "Guard")
        {
            for(int i = 1; i <= 5; i++)
            {
                if(name == "Guard" + i)
                {
                    GetComponent<NPCCollectionData>().npcData.displayName = NPCSave.instance.npcNames[i + 8];
                }
            }
        }
        else if(tag == "Inmate")
        {
            for (int i = 1; i <= 9; i++)
            {
                if(name == "Inmate" + i)
                {
                    GetComponent<NPCCollectionData>().npcData.displayName = NPCSave.instance.npcNames[i - 1];
                }
            }
        }
        else if(tag == "Player")
        {
            GetComponent<PlayerCollectionData>().playerData.displayName = NPCSave.instance.playerName;
        }
    }
}
