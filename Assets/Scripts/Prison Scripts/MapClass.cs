using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.VisualStyles;
using UnityEngine;
using UnityEngine.Rendering;

public class Map
{
    public string mapName;
    public string note;
    public string warden;
    public int guardCount;
    public int inmateCount;
    public string tilesetStr;
    public string groundStr;
    public string musicStr;
    public string speechStr;//
    public string itemsStr;//
    public string tooltipsStr;//
    public Sprite tileset;
    public Sprite ground;
    public Sprite icon;
    public string[] speech;//
    public string[] items;//
    public string[] tooltips;//
    public List<AudioClip> music;//
    public Dictionary<int, Sprite> customItemSpriteDict;//
    public int npcLevel;
    public string grounds;
    public int sizeX;
    public int sizeY;
    public string hint1;
    public string hint2;
    public string hint3;
    public bool snowing;
    public bool powOutfits;
    public bool stunRods;
    public Dictionary<int, string> routineDict;
    public string startingJob;
    public bool janitor;
    public bool gardening;
    public bool laundry;
    public bool kitchen;
    public bool tailor;
    public bool woodshop;
    public bool metalshop;
    public bool deliveries;
    public bool mailman;
    public bool library;
    public List<int[]> tilesList; //each int[] is like this: [tile],[posx],[posy],[layer (0, 1, 2, 3)]
    public List<string> objNames; //ground, underground, vent, roof (order)
    public List<float[]> objVars; //[posx],[posy],[layer]
    public List<string> zoneNames;
    public List<float[]> zoneVars; //[posx],[posy],[sizex],[sizey]

    public Map(
        string mapName,
        string note,
        string warden,
        int guardCount,
        int inmateCount,
        string tilesetStr,
        string groundStr,
        string musicStr,
        string speechStr,
        string itemsStr,
        string tooltipsStr,
        Sprite tileset,
        Sprite ground,
        Sprite icon,
        string[] speech,
        string[] items,
        string[] tooltips,
        List<AudioClip> music,
        Dictionary<int, Sprite> customItemSpriteDict,
        int npcLevel,
        string grounds,
        int sizeX,
        int sizeY,
        string hint1,
        string hint2,
        string hint3,
        bool snowing,
        bool powOutfits,
        bool stunRods,
        Dictionary<int, string> routineDict,
        string startingJob,
        bool janitor,
        bool gardening,
        bool laundry,
        bool kitchen,
        bool tailor,
        bool woodshop,
        bool metalshop,
        bool deliveries,
        bool mailman,
        bool library,
        List<int[]> tilesList,
        List<string> objNames,
        List<float[]> objVars,
        List<string> zoneNames,
        List<float[]> zoneVars
    )
    {
        this.mapName = mapName;
        this.note = note;
        this.warden = warden;
        this.guardCount = guardCount;
        this.inmateCount = inmateCount;
        this.tilesetStr = tilesetStr;
        this.groundStr = groundStr;
        this.musicStr = musicStr;
        this.speechStr = speechStr;
        this.itemsStr = itemsStr;
        this.tooltipsStr = tooltipsStr;
        this.tileset = tileset;
        this.ground = ground;
        this.icon = icon;
        this.speech = speech;
        this.items = items;
        this.tooltips = tooltips;
        this.music = music;
        this.customItemSpriteDict = customItemSpriteDict;
        this.npcLevel = npcLevel;
        this.grounds = grounds;
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.hint1 = hint1;
        this.hint2 = hint2;
        this.hint3 = hint3;
        this.snowing = snowing;
        this.powOutfits = powOutfits;
        this.stunRods = stunRods;
        this.routineDict = routineDict;
        this.startingJob = startingJob;
        this.janitor = janitor;
        this.gardening = gardening;
        this.laundry = laundry;
        this.kitchen = kitchen;
        this.tailor = tailor;
        this.woodshop = woodshop;
        this.metalshop = metalshop;
        this.deliveries = deliveries;
        this.mailman = mailman;
        this.library = library;
        this.tilesList = tilesList;
        this.objNames = objNames;
        this.objVars = objVars;
        this.zoneNames = zoneNames;
        this.zoneVars = zoneVars;
    }
}
