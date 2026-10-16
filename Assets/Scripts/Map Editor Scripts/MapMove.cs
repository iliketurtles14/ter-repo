using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    private List<string> layers = new List<string>
    {
        "Ground", "Underground", "Vent", "Roof", "GroundObjects", "UndergroundObjects", "VentObjects", "RoofObjects"
    };
    public Transform tiles;
    public Transform canvases;
    private int moveX;
    private int moveY;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveY = 1;
            moveX = 0;
            MoveMap();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveY = -1;
            moveX = 0;
            MoveMap();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveX = -1;
            moveY = 0;
            MoveMap();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveX = 1;
            moveY = 0;
            MoveMap();
        }
    }
    private void MoveMap()
    {
        for(int i = 0; i < 8; i++)
        {
            foreach(Transform tile in tiles.Find(layers[i]))
            {
                if(tile.name != "empty")
                {
                    tile.position += new Vector3(moveX * 1.6f, moveY * 1.6f);
                }
            }
        }
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform canvas in canvases.Find(layers[i]))
            {
                canvas.position += new Vector3(moveX * 1.6f, moveY * 1.6f);
            }
        }
    }
}
