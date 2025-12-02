using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedController : MonoBehaviour //also for spills
{
    private int weedAmount;
    private int spillAmount;
    private Transform tiles;
    private List<Vector3> weedLocations;
    private List<Vector3> spillLocations;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        MakeLists();
    }
    private void Update()
    {
        if(weedAmount < 2)
        {
            CreateWeed();
        }
        if(spillAmount < 2)
        {
            CreateSpill();
        }
    }
    private void MakeLists()
    {
        List<Vector3> possibleWeedLocations = new List<Vector3>();
        List<Vector3> actualWeedLocations = new List<Vector3>();
        List<Vector3> possibleSpillLocations = new List<Vector3>();
        List<Vector3> actualSpillLocations = new List<Vector3>();
        foreach (Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name == "InmateWaypoint")
            {
                possibleWeedLocations.Add(obj.position);
                possibleSpillLocations.Add(obj.position);
            }
        }
        foreach (Vector3 pos in possibleWeedLocations)
        {
            foreach (Transform tile in tiles.Find("Ground"))
            {
                if (tile.position == pos && tile.name == "Empty")
                {
                    actualWeedLocations.Add(pos);
                }
            }
        }
        foreach(Vector3 pos in possibleSpillLocations)
        {
            foreach(Transform tile in tiles.Find("Ground"))
            {
                if(tile.position == pos && tile.CompareTag("Digable") && tile.name != "Empty")
                {
                    actualSpillLocations.Add(pos);
                }
            }
        }

        weedLocations = actualWeedLocations;
        spillLocations = actualSpillLocations;
    }
    public void CreateWeed()
    {
        int rand = UnityEngine.Random.Range(0, weedLocations.Count);
        
        GameObject weed = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Weed"));
        weed.transform.position = weedLocations[rand];

        weedAmount++;
    }
    public void CreateSpill()
    {
        int rand = UnityEngine.Random.Range(0, spillLocations.Count);

        GameObject spill = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Spill"));
        spill.transform.position = spillLocations[rand];

        spillAmount++;
    }
}
