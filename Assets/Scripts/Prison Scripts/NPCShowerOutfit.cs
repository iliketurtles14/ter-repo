using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShowerOutfit : MonoBehaviour
{
    private List<BoxCollider2D> showerZones = new List<BoxCollider2D>();
    private Transform tiles;
    private Schedule scheduleScript;
    private SpriteRenderer outfitSR;
    private NPCCollectionData npcColData;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        outfitSR = transform.Find("Outfit").GetComponent<SpriteRenderer>();
        npcColData = GetComponent<NPCCollectionData>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach(Transform zone in tiles.Find("Zones"))
        {
            if(zone.name == "Showers")
            {
                showerZones.Add(zone.GetComponent<BoxCollider2D>());
            }
        }
    }
    private void Update()
    {
        if(scheduleScript.periodCode != "S")
        {
            try
            {
                if (npcColData.npcData.inventory[7].itemData != null)
                {
                    outfitSR.enabled = true;
                }
            }
            catch
            {

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Showers" && scheduleScript.periodCode == "S")
        {
            outfitSR.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Showers")
        {
            if (npcColData.npcData.inventory[7].itemData != null)
            {
                outfitSR.enabled = true;
            }
        }
    }
}
