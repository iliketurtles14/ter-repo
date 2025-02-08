using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class SetSprites : MonoBehaviour
{
    public GameObject CenterPerksTiles;
    private string tileName;

    //private int[] collisionNums = new int[]
    //{
    //    78, 88, 63, 67, 73, 79, 35, 43, 39, 20, 4, 0, 12, 8, 16, 74, 70, 66, 51, 55, 47, 
    //    84, 94, 73, 86, 61, 57, 82, 53, 49, 69, 90, 67, 83, 99, 75, 91, 95, 85, 89, 33, 
    //    37, 77, 81, 40, 76, 44, 64, 68, 36, 52, 56, 60, 72, 48, 6, 45, 9, 22, 18, 2, 10, 
    //    14, 41, 65
    //};
    //private int[] noCollisionNums = new int[]
    //{
    //    96, 97, 98, 92, 88, 84, 24, 38, 34, 26, 30, 31, 27, 23, 17, 13, 15, 19, 5, 28, 1, 
    //    21, 29, 25
    //};

    private Dictionary<string, int> perksTilesetDict = new Dictionary<string, int>() //this was a bitch to code holy shit
    {
        { "Bars", 78 }, { "Bottom Wall Left", 88 }, { "Bottom Wall Middle", 63 }, { "Bottom Wall Right", 71 },
        { "Box", 73 }, { "Bush", 79 }, { "Concrete", 35 }, { "Electric Horizontal", 43 },
        { "Electric Vertical", 39 }, { "Fence Horizontal", 20 }, { "Fence NE Corner", 4 }, { "Fence NW Corner", 0 },
        { "Fence SE Corner", 12 }, { "Fence SW Corner", 8 }, { "Fence Vertical", 16 }, { "Garage", 74 },
        { "Hard Wall Horizontal", 70 }, { "Hard Wall Vertical", 66 }, { "Mask Left", 51 }, { "Mask Middle", 55 },
        { "Mask Right", 47 }, { "Obstacle", 84 }, { "Roofing E End", 94 }, { "Roofing Horizontal", 73 },
        { "Roofing N End", 86 }, { "Roofing NE Corner", 61 }, { "Roofing NW Corner", 57 }, { "Roofing S End", 82 },
        { "Roofing SE Corner", 53 }, { "Roofing SW Corner", 49 }, { "Roofing Vertical", 69 }, { "Roofing W End", 90 },
        { "Top Wall Horizontal", 67 }, { "Top Wall NE Corner", 83 }, { "Top Wall NW Corner", 99 }, { "Top Wall SE Corner", 75 },
        { "Top Wall SW Corner", 91 }, { "Top Wall Vertical", 95 }, { "Vent E", 85 }, { "Vent N", 89 },
        { "Vent NE Corner", 33 }, { "Vent NW Corner", 37 }, { "Vent S", 77 }, { "Vent W", 81 },
        { "Wall E T-Shape", 40 }, { "Wall Horizontal", 76 }, { "Wall N T-Shape", 44 }, { "Wall NE Corner", 64 },
        { "Wall NW Corner", 68 }, { "Wall Plus Shape", 36 }, { "Wall S T-Shape", 52 }, { "Wall SE Corner", 56 },
        { "Wall SW Corner", 60 }, { "Wall Vertical", 72 }, { "Wall W T-Shape", 48 }, { "Water", 6 },
        { "Water E", 45 }, { "Water N", 9 }, { "Water NE Corner", 22 }, { "Water NW Corner", 18 },
        { "Water S", 2 }, { "Water SE Corner", 10 }, { "Water SW Corner", 14 }, { "Water W", 41 }, { "Window", 65 },
        { "Floor 1", 96 }, { "Floor 2", 97 }, { "Floor 3", 98 }, { "Floor 4", 92 },
        { "Floor 5", 88 }, { "Floor 6", 84 }, { "Floor 7", 24 }, { "Floor 7 NE", 38 },
        { "Floor 7 NW", 34 }, { "Floor 7 SE", 26 }, { "Floor 7 SW", 30 }, { "Floor 8", 31 },
        { "Floor 9", 27 }, { "Floor 10", 23 }, { "Floor 11 Horizontal", 17 }, { "Floor 11 Vertical", 13 },
        { "Floor 12 Horizontal", 15 }, { "Floor 12 Vertical", 19 }, { "Floor 13", 5 }, { "Floor 14", 28 },
        { "GrassPlaceholder", 1 }, { "Roof Floor High", 21 }, { "Roof Floor Low", 29 }, { "Roof Floor Medium", 25 }
    }; 
    private void Start()
    {
        SetTiles();
    }
    private void SetTiles()
    {
        foreach(Transform child in CenterPerksTiles.transform)
        {
            if(child.name.IndexOf(" (") != -1)
            {
                int index = child.name.IndexOf(" (");
                name = child.name.Remove(index, child.name.Length - index);
            }
            else
            {
                name = child.name;
            }
            child.GetComponent<SpriteRenderer>().sprite = DataSender.instance.GetComponent<DataSender>().PerksList[perksTilesetDict[name]];
        }
    }
}
