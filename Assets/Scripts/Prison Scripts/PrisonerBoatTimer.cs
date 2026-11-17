using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerBoatTimer : MonoBehaviour
{
    private Routine routineScript;
    private Transform tiles;
    private bool alreadyActivated = false;
    private bool alreadyDeactivated = false;
    private List<string> layers = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private void Start()
    {
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
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
        DeactivateBoats();
    }
    private void Update()
    {
        if(routineScript.min == 22 && routineScript.sec == 0 && !alreadyActivated)
        {
            alreadyActivated = true;
            ActivateBoats();
        }
        else if(routineScript.min == 6 && routineScript.sec == 0 && !alreadyDeactivated)
        {
            alreadyDeactivated = true;
            DeactivateBoats();
        }

        if(routineScript.sec != 0)
        {
            alreadyActivated = false;
            alreadyDeactivated = false;
        }
    }
    private void ActivateBoats()
    {
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(layers[i]))
            {
                if(obj.name == "AlcaBoat")
                {
                    obj.gameObject.SetActive(true);
                }
            }
        }
    }
    private void DeactivateBoats()
    {
        for (int i = 0; i < 4; i++)
        {
            foreach (Transform obj in tiles.Find(layers[i]))
            {
                if (obj.name == "AlcaBoat")
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
}
