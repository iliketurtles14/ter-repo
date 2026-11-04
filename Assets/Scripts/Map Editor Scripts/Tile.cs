
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
}
