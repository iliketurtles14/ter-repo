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
    private DeskPickUp deskPickUpScript;
    private List<GameObject> openedDoors = new List<GameObject>();
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
        deskPickUpScript = GetComponent<DeskPickUp>();
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
        List<Collider2D> hitCols = new List<Collider2D>();
        ContactFilter2D filter = ContactFilter2D.noFilter;
        foreach(BoxCollider2D bc in doorColliders)
        {
            bc.Overlap(filter, hitCols);
            bool isHit = false;
            foreach(Collider2D col in hitCols)
            {
                if(col.gameObject.name == "Player" && !Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), bc.gameObject.layer))
                {
                    isHit = true;
                    break;
                }
            }
            if (isHit)
            {
                if (CanOpen(bc.gameObject))
                {
                    if (!bc.isTrigger)
                    {
                        PSoundController.PlaySound("door");
                    }
                    bc.isTrigger = true;
                    bc.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                    if (!openedDoors.Contains(bc.gameObject))
                    {
                        openedDoors.Add(bc.gameObject);
                    }
                }
            }
            else
            {
                bc.isTrigger = false;
                bc.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                if (openedDoors.Contains(bc.gameObject))
                {
                    openedDoors.Remove(bc.gameObject);
                }
            }
        }

        if(openedDoors.Count > 0)
        {
            deskPickUpScript.inDoor = true;
        }
        else
        {
            deskPickUpScript.inDoor = false;
        }
    }
    private bool CanOpen(GameObject door)
    {
        if (deskPickUpScript.isPickedUp)
        {
            return false;
        }
        
        switch (door.name)
        {
            case "BlankDoor":
            case "OpenCellDoor":
            case "OpenEnteranceDoor":
            case "OpenWhiteDoor":
            case "OpenUtiltiyDoor":
            case "OpenStaffDoor":
            case "OpenGuardDoor":
                return true;
            case "CellDoor":
            case "UtilityDoor":
            case "EnteranceDoor":
            case "StaffDoor":
            case "WorkDoor":
                if(door.name == "WorkDoor" && player.GetComponent<PlayerCollectionData>().playerData.job == door.GetComponent<WorkDoorContainer>().job && !string.IsNullOrEmpty(player.GetComponent<PlayerCollectionData>().playerData.job))
                {
                    return true;
                }
                else if(door.name == "WorkDoor")
                {
                    return false;
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
                try
                {
                    if (guardOutfitIDs.Contains(mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id))
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
                break;
        }
        return false;
    }
}
