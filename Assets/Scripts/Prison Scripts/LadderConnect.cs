using UnityEngine;

public class LadderConnect : MonoBehaviour
{
    public Vector3 connectedTilePos;
    public string goToLayer; // ground, vents, roof
    public bool fall = false; // if there isnt a tile above the ladder at all
}
