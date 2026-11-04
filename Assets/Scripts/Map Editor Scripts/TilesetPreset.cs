using System.Collections.Generic;

public class TilesetPreset
{
    public List<Tile> tiles;
    public string name;
    public int index;
    public TilesetPreset(List<Tile> tiles, string name, int index)
    {
        this.tiles = tiles;
        this.name = name;
        this.index = index;
    }
}
