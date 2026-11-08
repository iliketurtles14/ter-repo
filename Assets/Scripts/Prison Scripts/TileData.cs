using System;
using UnityEngine;
[Serializable]
[CreateAssetMenu(menuName = "Tile Data")]
public class TileData : ScriptableObject
{
    public int currentDurability;
    public int holeStability;
    public bool holeIsUnder;//for patchups
    public int holeDurability;//for patchups
    public bool isOutside = true; //for fences and bad objects
}
