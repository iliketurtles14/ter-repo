using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class METilesetMenu : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform uic;
    private Transform tilesetPanel = null;
    public int currentTileIndex;
    public List<Tile> tileList = null;
    private void OnEnable()
    {
        StartCoroutine(EnableWait());
    }
    private void OnDisable()
    {
        SelectTile(tilesetPanel.Find("TileGrid").Find("tile0").gameObject);
    }
    private IEnumerator EnableWait()
    {
        if (tilesetPanel == null)
        {
            tilesetPanel = uic.Find("TilesetPanel");
        }
        if (tileList == null)
        {
            tileList = MakeDefaultList();
        }
        LoadTileInfo(tileList[0]);
        yield return new WaitForEndOfFrame();
        SelectTile(tilesetPanel.Find("TileGrid").Find("tile0").gameObject);
    }
    private void Update()
    {
        if(mcs.isTouchingEditTile && mcs.touchedEditTile.name.Contains("tile") && Input.GetMouseButtonDown(0))
        {
            SelectTile(mcs.touchedEditTile);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveTileset();
        }
    }
    private void SaveTileset()
    {
        Debug.Log("saving tileset");
        Transform properties = uic.Find("PropertiesPanel");
        string mapName = properties.Find("NameInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        string text = "[" + mapName + "]\n";
        for(int i = 0; i < 100; i++)
        {
            Tile tile = tileList[i];
            text += "tile" + i.ToString() + "=" + tile.type.ToString() + ";" + tile.roofLedgeType.ToString() + ";" + tile.roofFloorHeight.ToString() + ";" + tile.drawsShads + ";" + tile.drawAbovePlayer + ";" + tile.canPath + ";" + tile.belowShads + ";" + tile.seeThrough + ";" + tile.isBroken + "\n";
        }
        File.AppendAllText("C:\\Users\\ilike\\OneDrive\\Desktop\\tilesetstuff.txt", text);
    }
    private void SelectTile(GameObject tileObj)
    {
        SaveTileInfo(currentTileIndex);
        tilesetPanel.Find("Outline").position = tileObj.transform.position;
        currentTileIndex = Convert.ToInt32(tileObj.name.Replace("tile", ""));
        LoadTileInfo(tileList[currentTileIndex]);
    }
    private void LoadTileInfo(Tile tile) //idk why this takes a Tile and the SaveTileInfo() function takes an int lol
    {
        DataSender ds = DataSender.instance;
        Dictionary<bool, Sprite> checkboxDict = new Dictionary<bool, Sprite>
        {
            { true, ds.UIImages[448] }, { false, ds.UIImages[447] }
        };
        
        tilesetPanel.Find("TileTypePicker").GetComponent<Picker>().currentIndex = tile.type;
        tilesetPanel.Find("RoofLedgePicker").GetComponent<Picker>().currentIndex = tile.roofLedgeType;
        tilesetPanel.Find("RoofFloorPicker").GetComponent<Picker>().currentIndex = tile.roofFloorHeight;
        tilesetPanel.Find("DrawsShadCheckbox").GetComponent<Image>().sprite = checkboxDict[tile.drawsShads];
        tilesetPanel.Find("DrawAboveCheckbox").GetComponent<Image>().sprite = checkboxDict[tile.drawAbovePlayer];
        tilesetPanel.Find("CanPathCheckbox").GetComponent<Image>().sprite = checkboxDict[tile.canPath];
        tilesetPanel.Find("ShadCheckbox").GetComponent<Image>().sprite = checkboxDict[tile.belowShads];
        tilesetPanel.Find("SeeThroughCheckbox").GetComponent<Image>().sprite = checkboxDict[tile.seeThrough];
        tilesetPanel.Find("BrokenCheckbox").GetComponent<Image>().sprite = checkboxDict[tile.isBroken];
    }
    public void SaveTileInfo(int index)
    {
        int type = tilesetPanel.Find("TileTypePicker").GetComponent<Picker>().currentIndex;
        int roofLedgeType = tilesetPanel.Find("RoofLedgePicker").GetComponent<Picker>().currentIndex;
        int roofFloorHeight = tilesetPanel.Find("RoofFloorPicker").GetComponent<Picker>().currentIndex;
        DataSender ds = DataSender.instance;
        bool drawsShads = tilesetPanel.Find("DrawsShadCheckbox").GetComponent<Image>().sprite == ds.UIImages[448];
        bool drawAbove = tilesetPanel.Find("DrawAboveCheckbox").GetComponent<Image>().sprite == ds.UIImages[448];
        bool canPath = tilesetPanel.Find("CanPathCheckbox").GetComponent<Image>().sprite == ds.UIImages[448];
        bool belowShads = tilesetPanel.Find("ShadCheckbox").GetComponent<Image>().sprite == ds.UIImages[448];
        bool seeThrough = tilesetPanel.Find("SeeThroughCheckbox").GetComponent<Image>().sprite == ds.UIImages[448];
        bool broken = tilesetPanel.Find("BrokenCheckbox").GetComponent<Image>().sprite == ds.UIImages[448];

        tileList[currentTileIndex] = new Tile(type, roofLedgeType, roofFloorHeight, drawsShads, drawAbove, canPath, belowShads, seeThrough, broken);
    }
    private List<Tile> MakeDefaultList()
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
        for (int i = 0; i < 100; i++)
        {
            Tile tile = null;
            switch (i)
            {
                case 0: //regular in floors
                case 1:
                case 2:
                case 4:
                case 8:
                case 68:
                case 71:
                case 75:
                case 79:
                case 83:
                case 87:
                    tile = new Tile(4, 0, 1, false, false, true, true, true, false);
                    break;
                case 47: //regular out floors
                case 51:
                case 55:
                case 62:
                case 66:
                case 70:
                case 72:
                case 74:
                case 81:
                case 85:
                case 93:
                case 97:
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
                case 54:
                case 56:
                case 58:
                case 60:
                case 61:
                case 64:
                case 65:
                case 67:
                case 84:
                case 88:
                case 91:
                case 92:
                case 95:
                case 96:
                case 99:
                    tile = new Tile(6, 0, 0, true, false, false, false, false, false);
                    break;
                case 23: //obstacles with shads ontop
                    tile = new Tile(6, 0, 0, true, false, false, true, false, false);
                    break;
                case 78: //obstacles with shads ontop making no shads (water)
                case 82:
                case 86:
                case 89:
                case 90:
                case 94:
                case 98:
                case 53:
                case 57:
                    tile = new Tile(6, 0, 0, false, false, false, true, false, false);
                    break;
                case 20: //chippable walls
                case 24:
                    tile = new Tile(0, 0, 0, true, false, false, false, false, false);
                    break;
                case 76: //fence
                case 80:
                    tile = new Tile(1, 0, 0, true, false, false, false, true, false);
                    break;
                case 59: //elec fence
                case 63:
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
                case 69: //low roof floor
                    tile = new Tile(5, 0, 0, false, false, true, true, true, false);
                    break;
                case 73: //med roof floor
                    tile = new Tile(5, 0, 1, false, false, true, true, true, false);
                    break;
                case 77: //high roof floor
                    tile = new Tile(5, 0, 2, false, false, true, true, true, false);
                    break;
            }
            tileList.Add(tile);
        }
        return tileList;
    }
}
