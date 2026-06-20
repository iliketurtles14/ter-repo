using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoors : MonoBehaviour
{
    private Transform tiles;
    private List<GameObject> yellowDoors = new List<GameObject>();
    private List<GameObject> orangeDoors = new List<GameObject>();
    private List<GameObject> redDoors = new List<GameObject>();
    private List<GameObject> greenDoors = new List<GameObject>();
    private List<GameObject> purpleDoors = new List<GameObject>();
    private List<GameObject> whiteDoors = new List<GameObject>();
    private List<GameObject> guardDoors = new List<GameObject>();
    private Dictionary<string, List<GameObject>> typeDict;
    private Dictionary<string, Sprite> spriteDict;
    private Sprite unlockedSprite;

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
    private void MakeLists()
    {
        List<string> layers = new List<string>
        {
            "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
        };
        
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(layers[i]))
            {
                switch (obj.name)
                {
                    case "CellDoor":
                        yellowDoors.Add(obj.gameObject);
                        break;
                    case "UtilityDoor":
                        orangeDoors.Add(obj.gameObject);
                        break;
                    case "EnteranceDoor":
                        purpleDoors.Add(obj.gameObject);
                        break;
                    case "WorkDoor":
                        greenDoors.Add(obj.gameObject);
                        break;
                    case "StaffDoor":
                        redDoors.Add(obj.gameObject);
                        break;
                    case "WhiteDoor":
                        whiteDoors.Add(obj.gameObject);
                        break;
                    case "GuardDoor":
                        guardDoors.Add(obj.gameObject);
                        break;
                }
            }
        }

        typeDict = new Dictionary<string, List<GameObject>>
        {
            { "yellow", yellowDoors }, { "orange", orangeDoors }, { "red", redDoors }, { "green", greenDoors },
            { "purple", purpleDoors }, { "white", whiteDoors }, { "guard", guardDoors }
        };
        List<Sprite> objSprites = new List<Sprite>(DataSender.instance.PrisonObjectImages);
        spriteDict = new Dictionary<string, Sprite>
        {
            { "yellow", objSprites[178] }, { "orange", objSprites[175] }, { "red", objSprites[176] },
            { "green", objSprites[174] }, { "purple", objSprites[177] }, { "white", objSprites[190] },
            { "guard", objSprites[327] }
        };
        unlockedSprite = objSprites[179];
    }
    public void LockDoors(List<string> types) //yellow, orange, red, green, purple, white, guard
    {
        for(int i = 0; i < types.Count; i++)
        {
            foreach(GameObject door in typeDict[types[i]])
            {
                door.name = door.name.Replace("Open", "");
                door.GetComponent<SpriteRenderer>().sprite = spriteDict[types[i]];
            }
        }
    }
    public void aUnlockDoors(List<string> types) //yellow, orange, red, green, purple, white, guard
    {
        for(int i = 0; i < types.Count; i++)
        {
            foreach(GameObject door in typeDict[types[i]])
            {
                door.name = "Open" + door.name;
                door.GetComponent<SpriteRenderer>().sprite = unlockedSprite;
            }
        }
    }
}
