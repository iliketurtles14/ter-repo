using UnityEngine;

public class PlayerCollectionData : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerData[] playerDatas;
    public string folderPath = "Player Scriptable Objects";

    [ContextMenu("Load All Assets Into List")]
    public void LoadAllAssets()
    {
        playerDatas = Resources.LoadAll<PlayerData>(folderPath);
    }
    public void Start()
    {
        LoadAllAssets();
        foreach(PlayerData data in playerDatas)
        {
            playerData = Instantiate(data);
        }
    }
}
