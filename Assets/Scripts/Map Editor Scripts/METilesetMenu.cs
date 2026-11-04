using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class METilesetMenu : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform uic;
    private Transform tilesetPanel;
    private int currentTileIndex;
    private void Start()
    {
        tilesetPanel = uic.Find("TilesetPanel");
    }
    private void OnEnable()
    {
        SelectTile(tilesetPanel.Find("TileGrid").Find("tile0").gameObject);
    }
    private void Update()
    {
        if(mcs.isTouchingButton && mcs.touchedButton.name.Contains("tile") && Input.GetMouseButtonDown(0))
        {
            SelectTile(mcs.touchedButton);
        }
    }
    private void SelectTile(GameObject tileObj)
    {
        tilesetPanel.Find("Outline").position = tileObj.transform.position;
        currentTileIndex = Convert.ToInt32(tileObj.name.Replace("tile", ""));
    }
    private TilesetPreset MakeDefaultPreset() //ended last time at 59
    {
        List<Tile> tileList = new List<Tile>(); 
        //0: wall
        //1: fence
        //2: elec
        //3: bars
        //4: in floor
        //5: out floor
        //6: obst

        //0: none
        //1: hori ledge
        //2: vert ledge
        //3: hori high
        //4: vert high
        for(int i = 0; i < 100; i++)
        {
            Tile tile;
            switch (i)
            {
                case 0: //regular in floors
                case 1:
                case 2:
                case 4:
                case 8:
                    tile = new Tile(4, 0, 1, false, false, true, true, true, false);
                    break;
                case 47: //regular out floors
                case 51:
                case 55:
                    tile = new Tile(5, 0, 1, false, false, true, true, true, false);
                    break;
                case 3: //obstacles
                case 5:
                case 6:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 21:
                case 26:
                case 27:
                case 28:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 48:
                case 49:
                case 50:
                case 52:
                case 53:
                case 54:
                case 57:
                case 58:
                    tile = new Tile(6, 0, 0, true, false, false, false, false, false);
                    break;
                case 23: //obstacles with shads ontop
                    tile = new Tile(6, 0, 0, true, false, false, true, false, false);
                    break;
                case 20: //chippable walls
                case 24:
                    tile = new Tile(0, 0, 0, true, false, false, false, false, false);
                    break;
                case 59: //elec fence
                    tile = new Tile(2, 0, 0, true, false, false, false, true, false);
                    break;
                case 22: //bars
                    tile = new Tile(3, 0, 0, true, false, false, false, true, false);
                    break;
                case 7: //high wall vert
                    tile = new Tile(6, 4, 0, true, false, false, false, false, false);
                    break;
                case 35: //high wall hori
                    tile = new Tile(6, 3, 0, true, false, false, false, false, false);
                    break;
                case 25: //ledge hori
                    tile = new Tile(6, 1, 0, true, false, false, false, false, false);
                    break;
                case 29: //ledge vert
                    tile = new Tile(6, 2, 0, true, false, false, false, false, false);
                    break;
            }
        }
    }
}
