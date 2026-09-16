using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpReindeers : MonoBehaviour
{
    private List<string> objLayers = new List<string>
    {
        "GroundObjects", "UndergroundObjects", "VentObjects", "RoofObjects"
    };
    private List<string> reindeerNames = new List<string> //reindeer names reindeer games !!
    {
        "Dasher", "Dancer", "Prancer", "Vixen", "Comet", "Cupid", "Donner", "Blitzen", "Rudolph", "Olive", "Bobtail", "Donder"
    };
    private Transform tiles;
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
        ReindeerSetUp();
    }
    private void ReindeerSetUp()
    {
        List<GameObject> reindeers = new List<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(objLayers[i]))
            {
                if(obj.name == "Reindeer")
                {
                    reindeers.Add(obj.gameObject);
                }
            }
        }

        foreach(GameObject deer in reindeers)
        {
            if(reindeerNames.Count < 0)
            {
                reindeerNames = new List<string>
                {
                    "Dasher", "Dancer", "Prancer", "Vixen", "Comet", "Cupid", "Donner", "Blitzen", "Rudolph", "Olive", "Bobtail", "Donder"
                };
            }
            int rand = UnityEngine.Random.Range(0, reindeerNames.Count);
            deer.GetComponent<ReindeerHandler>().displayName = reindeerNames[rand];
            reindeerNames.RemoveAt(rand);
        }
    }
}
