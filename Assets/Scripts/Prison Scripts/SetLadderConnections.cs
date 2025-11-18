using System.Collections;
using UnityEngine;

public class SetLadderConnections : MonoBehaviour
{
    private Transform tiles;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;

        StartCoroutine(WaitForLoads());
    }
    private IEnumerator WaitForLoads()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        ConnectLadders();
    }
    private void ConnectLadders()
    {
        foreach (Transform ladder in tiles.Find("GroundObjects"))
        {
            if (ladder.name == "LadderUp (Ground)")
            {
                bool isVents = false;
                bool isNeither = true;
                foreach (Transform tile in tiles.Find("Vents"))
                {
                    if (tile.position == ladder.position - new Vector3(0, 1.6f, 0))
                    {
                        ladder.GetComponent<LadderConnect>().connectedTilePos = tile.position;
                        ladder.GetComponent<LadderConnect>().goToLayer = "vents";
                        isVents = true;
                        isNeither = false;
                        break;
                    }
                }
                if (!isVents)
                {
                    foreach (Transform tile in tiles.Find("Roof"))
                    {
                        if (tile.position == ladder.position - new Vector3(0, 1.6f, 0))
                        {
                            ladder.GetComponent<LadderConnect>().connectedTilePos = tile.position;
                            ladder.GetComponent<LadderConnect>().goToLayer = "roof";
                            isNeither = false;
                            break;
                        }
                    }
                }
                if (isNeither)
                {
                    ladder.GetComponent<LadderConnect>().fall = true;
                }
            }
        }
        foreach(Transform ladder in tiles.Find("VentObjects"))
        {
            if(ladder.name == "LadderUp (Vent)")
            {
                bool isRoof = false;
                foreach(Transform tile in tiles.Find("Roof"))
                {
                    if(tile.position == ladder.position)
                    {
                        ladder.GetComponent<LadderConnect>().connectedTilePos = tile.position;
                        ladder.GetComponent<LadderConnect>().goToLayer = "roof";
                        isRoof = true;
                        break;
                    }
                }
                if (!isRoof)
                {
                    ladder.GetComponent<LadderConnect>().fall = true;
                }
            }
            else if(ladder.name == "LadderDown (Vent)")
            {
                ladder.GetComponent<LadderConnect>().connectedTilePos = ladder.position - new Vector3(0, 1.6f, 0);
                ladder.GetComponent<LadderConnect>().goToLayer = "ground";
            }
        }
        foreach(Transform ladder in tiles.Find("RoofObjects"))
        {
            if (ladder.name == "LadderDown (Roof)")
            {
                bool isVents = false;
                foreach (Transform tile in tiles.Find("Vents"))
                {
                    if (tile.position == ladder.position)
                    {
                        ladder.GetComponent<LadderConnect>().connectedTilePos = tile.position;
                        ladder.GetComponent<LadderConnect>().goToLayer = "vents";
                        isVents = true;
                        break;
                    }
                }
                if (!isVents)
                {
                    ladder.GetComponent<LadderConnect>().connectedTilePos = ladder.position - new Vector3(0, 1.6f, 0);
                    ladder.GetComponent<LadderConnect>().goToLayer = "ground";
                    break;
                }
            }
        }
    }
}
