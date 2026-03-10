using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private Transform player;
    private Transform tiles;
    private List<BoxCollider2D> doorColliders = new List<BoxCollider2D>();
    private Inventory inventoryScript;
    private Transform mc;
    private Dictionary<string, List<int>> doorKeyDict = new Dictionary<string, List<int>>() //go from door name to id's of keys that work on that door
    {
        { "CellDoor", new List<int>(){0,5} }, { "UtilityDoor", new List<int>(){2,7} },
        { "EnteranceDoor", new List<int>{3,8} }, { "StaffDoor", new List<int>(){1,6} },
        { "WorkDoor", new List<int>(){4,9} }
    };
    private List<int> guardOutfitIDs = new List<int>()
    {
        39, 44, 49, 54
    };
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        inventoryScript = GetComponent<Inventory>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach(Transform layer in tiles)
        {
            if (layer.name.Contains("Objects"))
            {
                foreach(Transform obj in layer)
                {
                    if (obj.gameObject.CompareTag("Door"))
                    {
                        doorColliders.Add(obj.GetComponent<BoxCollider2D>());
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        foreach(BoxCollider2D bc in doorColliders)
        {
            if (player.GetComponent<CapsuleCollider2D>().IsTouching(bc))
            {
                if (CanOpen(bc.gameObject))
                {
                    bc.isTrigger = true;
                    bc.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                }
            }
            else
            {
                bc.isTrigger = false;
                bc.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }
    private bool CanOpen(GameObject door)
    {
        switch (door.name)
        {
            case "BlankDoor":
            case "OpenCellDoor":
            case "OpenEnteranceDoor":
            case "OpenWhiteDoor":
                return true;
            case "CellDoor":
            case "UtilityDoor":
            case "EnteranceDoor":
            case "StaffDoor":
            case "WorkDoor":
                if(door.name == "WorkDoor" && player.GetComponent<PlayerCollectionData>().playerData.job == door.GetComponent<WorkDoorContainer>().job)
                {
                    return true;
                }
                for(int i = 0; i < 6; i++)
                {
                    try
                    {
                        if (doorKeyDict[door.name].Contains(inventoryScript.inventory[i].itemData.id))
                        {
                            inventoryScript.inventory[i].itemData.currentDurability -= inventoryScript.inventory[i].itemData.durability;
                            if (inventoryScript.inventory[i].itemData.currentDurability <= 0)
                            {
                                inventoryScript.inventory[i].itemData = null;
                            }
                            return true;
                        }
                    }
                    catch { }
                }
                break;
            case "WhiteDoor":
                return false;
            case "GuardDoor":
                if (guardOutfitIDs.Contains(mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id))
                {
                    return true;
                }
                break;
        }
        return false;
    }
}
