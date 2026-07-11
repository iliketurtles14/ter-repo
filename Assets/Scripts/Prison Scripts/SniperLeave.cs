using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperLeave : MonoBehaviour
{
    private Schedule scheduleScript;
    private List<SpriteRenderer> srList = new List<SpriteRenderer>();
    private List<BoxCollider2D> bcList = new List<BoxCollider2D>();
    private Transform tiles;
    private bool ready = false;
    private Takeover takeoverScript;
    private List<string> objLayers = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        takeoverScript = GetComponent<Takeover>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(objLayers[i]))
            {
                if(obj.name == "Sniper")
                {
                    srList.Add(obj.GetComponent<SpriteRenderer>());
                    bcList.Add(obj.GetComponent<BoxCollider2D>());
                }
            }
        }
        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if(scheduleScript.periodCode == "LO" || takeoverScript.takeoverIsActive)
        {
            foreach(SpriteRenderer sr in srList)
            {
                sr.enabled = false;
            }
            foreach(BoxCollider2D bc in bcList)
            {
                bc.enabled = false;
            }
        }
        else
        {
            foreach (SpriteRenderer sr in srList)
            {
                sr.enabled = true;
            }
            foreach (BoxCollider2D bc in bcList)
            {
                bc.enabled = true;
            }
        }
    }
}
