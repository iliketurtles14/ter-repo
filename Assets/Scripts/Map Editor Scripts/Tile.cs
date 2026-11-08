
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile
{
    public int type;
    public int roofLedgeType;
    public int roofFloorHeight;
    public bool drawsShads;
    public bool drawAbovePlayer;
    public bool canPath;
    public bool belowShads;
    public bool seeThrough;
    public bool isBroken;
    public Tile(int type,
        int roofLedgeType,
        int roofFloorHeight,
        bool drawsShads,
        bool drawAbovePlayer,
        bool canPath,
        bool belowShads,
        bool seeThrough,
        bool isBroken)
    {
        this.type = type;
        this.roofLedgeType = roofLedgeType;
        this.roofFloorHeight = roofFloorHeight;
        this.drawsShads = drawsShads;
        this.drawAbovePlayer = drawAbovePlayer;
        this.canPath = canPath;
        this.belowShads = belowShads;
        this.seeThrough = seeThrough;
        this.isBroken = isBroken;
    }

    public string GetTileType()
    {
        Dictionary<int, string> dict = new Dictionary<int, string>
        {
            { 0, "wall" }, { 1, "fence" }, { 2, "elecFence" }, { 3, "bars" },
            { 4, "inFloor" }, { 5, "outFloor" }, { 6, "obstacle" }
        };
        return dict[this.type];
    }
    public string GetRoofLedgeType()
    {
        Dictionary<int, string> dict = new Dictionary<int, string>
        {
            { 0, "none" }, { 1, "horiLedge" }, { 2, "vertLedge" }, { 3, "horiHigh" },
            { 4, "vertHigh" }
        };
        return dict[this.roofLedgeType];
    }
    public string GetRoofFloorHeight()
    {
        Dictionary<int, string> dict = new Dictionary<int, string>
        {
            { 0, "lowFloor" }, { 1, "medFloor" }, { 2, "highFloor" }
        };
        return dict[this.roofFloorHeight];
    }
}
public static class TileTools
{
    public static Tile GetTileObj(GameObject obj, Map map)
    {
        if(obj.name == "BrokenTile")
        {
            return map.brokenTile;
        }
        else if(obj.name == "Empty")
        {
            return new Tile(5, 0, 1, false, false, true, true, true, false);
        }

        int tileNum = 0;
        try
        {
            tileNum = Convert.ToInt32(obj.name);
        }
        catch
        {
            return null;
        }
        if(tileNum >= 100 || tileNum < 0)
        {
            return null;
        }

        return map.tilesetData[tileNum];
    }
    public static Sprite GetBrokenTileSprite(Map map)
    {
        bool useBasicBrokenTile = true;
        int tileIndex = 0;
        foreach(Tile tile in map.tilesetData)
        {
            if (tile.isBroken)
            {
                useBasicBrokenTile = false;
                break;
            }
            tileIndex++;
        }

        if (useBasicBrokenTile)
        {
            return Resources.Load<Sprite>("PrisonResources/Object Sprites/EmptyTileSprite");
        }
        else
        {
            return SliceAndDice(map.tileset)[tileIndex];
        }
    }
    private static List<Sprite> SliceAndDice(Sprite tileset)
    {
        Texture2D tilesetTex = tileset.texture;
        List<Sprite> tileList = new List<Sprite>();

        int tileWidth = 16;
        int tileHeight = 16;

        // Start from the top row and move down
        for (int y = tilesetTex.height - tileHeight; y >= 0; y -= tileHeight)
        {
            for (int x = 0; x < tilesetTex.width; x += tileWidth)
            {
                Rect rect = new Rect(x, y, tileWidth, tileHeight);
                Sprite subSprite = Sprite.Create(tilesetTex, rect, new Vector2(.5f, .5f), 100f);
                tileList.Add(subSprite);
            }
        }

        return tileList;
    }
}
