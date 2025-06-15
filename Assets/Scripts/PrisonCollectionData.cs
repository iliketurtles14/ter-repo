using UnityEngine;

public class PrisonCollectionData : MonoBehaviour
{
    public PrisonData prisonData;
    public PrisonData[] prisonDatas;
    public string folderPath = "Prison Scriptable Objects";

    [ContextMenu("Load All Assets Into List")]
    public void LoadAllAssets()
    {
        prisonDatas = Resources.LoadAll<PrisonData>(folderPath);
    }
    public void Start()
    {
        LoadAllAssets();
        foreach(PrisonData data in prisonDatas)
        {
            prisonData = Instantiate(data);
        }
    }
}
