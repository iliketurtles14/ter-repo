using UnityEngine;

public class NPCCollectionData : MonoBehaviour
{
    public NPCData npcData;
    public NPCData[] npcDatas;
    public string folderPath = "NPC Scriptable Objects";

    [ContextMenu("Load All Assets Into List")]
    public void LoadAllAssets()
    {
        npcDatas = Resources.LoadAll<NPCData>(folderPath);
    }
    public void Start()
    {
        LoadAllAssets();
        foreach(NPCData data in npcDatas)
        {
            npcData = Instantiate(data);
        }
    }
}
