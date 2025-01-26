using UnityEngine;

public class CustomNPCCollectionData : MonoBehaviour
{
    public CustomNPCData customNPCData;
    public CustomNPCData[] customNPCDatas;
    public string folderPath = "Custom NPC Scriptable Objects";

    [ContextMenu("Load All Assets Into List")]
    public void LoadAllAssets()
    {
        customNPCDatas = Resources.LoadAll<CustomNPCData>(folderPath);
    }
    public void Start()
    {
        LoadAllAssets();
        foreach(CustomNPCData data in customNPCDatas)
        {
            customNPCData = Instantiate(data);
        }
    }
}
