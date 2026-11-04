using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class WeedController : MonoBehaviour //also for spills
{
    public int weedAmount;
    public int spillAmount;
    public int poopAmount;
    private Transform tiles;
    private List<Vector3> weedLocations;
    private List<Vector3> spillLocations;
    private List<Vector3> poopLocations;
    private ApplyPrisonData applyPrisonDataScript;
    private bool ready;
    private bool hasReindeer;
    private Map currentMap;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        applyPrisonDataScript = GetComponent<ApplyPrisonData>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = GetComponent<LoadPrison>().currentMap;
        hasReindeer = false;
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "Reindeer")
            {
                hasReindeer = true;
                break;
            }
        }
        ready = true;
        MakeLists();
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }
        
        if(weedAmount < 2 && weedLocations.Count != 0)
        {
            CreateWeed();
        }
        if(spillAmount < 2 && spillLocations.Count != 0)
        {
            CreateSpill();
        }
        if((currentMap.manure || hasReindeer) && poopAmount < 2 && poopLocations.Count != 0)
        {
            CreatePoop();
        }
    }
    private void MakeLists()
    {
        bool shouldDoPoop = false;
        if(currentMap.manure || hasReindeer)
        {
            shouldDoPoop = true;
        }
        
        List<Vector3> possibleWeedLocations = new List<Vector3>();
        List<Vector3> actualWeedLocations = new List<Vector3>();
        List<Vector3> possibleSpillLocations = new List<Vector3>();
        List<Vector3> actualSpillLocations = new List<Vector3>();
        List<Vector3> possiblePoopLocations = new List<Vector3>();
        List<Vector3> actualPoopLocations = new List<Vector3>();
        foreach (Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name == "InmateWaypoint")
            {
                possibleWeedLocations.Add(obj.position);
                possibleSpillLocations.Add(obj.position);
                if (shouldDoPoop)
                {
                    possiblePoopLocations.Add(obj.position);
                }
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
        if (shouldDoPoop)
        {
            foreach (Vector3 pos in possiblePoopLocations)
            {
                foreach (Transform tile in tiles.Find("Ground"))
                {
                    if (tile.position == pos && tile.name == "Empty")
                    {
                        actualPoopLocations.Add(pos);
                    }
                }
            }
        }

        weedLocations = actualWeedLocations;
        spillLocations = actualSpillLocations;
        if (shouldDoPoop)
        {
            poopLocations = actualPoopLocations;
        }
    }
    public void CreateWeed()
    {
        int rand = UnityEngine.Random.Range(0, weedLocations.Count);
        
        GameObject weed = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Weed"));
        weed.name = "Weed";
        weed.transform.parent = tiles.Find("GroundObjects");
        weed.transform.position = weedLocations[rand];

        weedAmount++;
    }
    public void CreateSpill()
    {
        int rand = UnityEngine.Random.Range(0, spillLocations.Count);

        GameObject spill = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Spill"));
        spill.name = "Spill";
        spill.transform.parent = tiles.Find("GroundObjects");
        spill.transform.position = spillLocations[rand];
        spill.GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[33];

        spillAmount++;
    }
    public void CreatePoop()
    {
        if(!currentMap.manure && !hasReindeer)
        {
            return;
        }

        int rand = UnityEngine.Random.Range(0, poopLocations.Count);

        GameObject poop = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Poop"));
        poop.name = "Poop";
        poop.transform.parent = tiles.Find("GroundObjects");
        poop.transform.position = poopLocations[rand];
        poop.GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[345];

        poopAmount++;
    }
}
