using UnityEngine;

public class TileCollectionData : MonoBehaviour
{
    public TileData tileData;
    public TileData[] tileDatas;
    public string folderPath = "Tile Scriptable Objects";

    [ContextMenu("Load All Assets Into List")]
    public void LoadAllAssets()
    {
        tileDatas = Resources.LoadAll<TileData>(folderPath);
    }
    public void Start()
    {
        LoadAllAssets();
    }
}
