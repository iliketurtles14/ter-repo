using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class LoadPrison : MonoBehaviour
{
    private AudioClip music;
    private Sprite tileset;
    private Sprite ground;
    private Sprite icon;

    private Dictionary<string, int> tilesetDict = new Dictionary<string, int>()
    {
        { "alca", 0 }, { "BC", 1 }, { "campepsilon", 2 }, { "CCL", 3 },
        { "DTAF", 4 }, { "EA", 5 }, { "escapeteam", 6 }, { "fortbamford", 7 },
        { "irongate", 12 }, { "jungle", 13 }, { "pcpen", 8 }, { "perks", 14 },
        { "sanpancho", 15 }, { "shanktonstatepen", 16 }, { "SS", 9 }, { "stalagflucht", 17 },
        { "TOL", 10 }, { "tutorial", 11 }
    };
    private Dictionary<string, int> groundDict = new Dictionary<string, int>()
    {
        { "alca", 0 }, { "BC", 1 }, { "campepsilon", 2 }, { "CCL", 3 },
        { "DTAF", 4 }, { "EA", 5 }, { "escapeteam", 6 }, { "fortbamford", 7 },
        { "irongate", 8 }, { "jungle", 9 }, { "pcpen", 10 }, { "perks", 11 },
        { "sanpancho", 12 }, { "shanktonstatepen", 13 }, { "SS", 14 }, { "stalagflucht", 15 },
        { "TOL", 16 }, { "tutorial", 17 }
    };

    private Dictionary<string, string> prisonDict = new Dictionary<string, string>() //this is for converting the result text to the real prison names
    {
        { "perks", "perks" }, { "stalag", "stalagflucht" }, { "shankton", "shanktonstatepen" },
        { "jungle", "jungle" }, { "sanpancho", "sanpancho" }, { "irongate", "irongate" },
        { "JC", "CCL" }, { "BC", "BC" }, { "london", "TOL" }, { "PCP", "pcpen" }, { "SS", "SS" },
        { "DTAF", "DTAF" }, { "ET", "escapeteam" }, { "alca", "alca" }, { "fhurst", "EA" },
        { "epsilon", "campepsilon" }, { "bamford", "fortbamford" }, { "tutorial", "tutorial" }
    };

    private GetGivenData givenDataScript;
    private void Start()
    {
        givenDataScript = GetGivenData.instance;
    }
    public void StartLoad(Map map)
    {

    }
    public Map MakeMapObject(string path)
    {
        //unzip files
        string extractPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "CustomPrisons") + Path.DirectorySeparatorChar;

        ZipFile.ExtractToDirectory(path, extractPath);

        //load Data.ini to a string[]
        string[] data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));
        File.Delete(Path.Combine(extractPath, "Data.ini"));

        //load other files to memory
        if (File.Exists(Path.Combine(extractPath, "Tiles.png")))
        {
            tileset = ConvertPNGToSprite(Path.Combine(extractPath, "Tiles.png"));
            File.Delete(Path.Combine(extractPath, "Tiles.png"));
        }
        if (File.Exists(Path.Combine(extractPath, "Ground.png")))
        {
            ground = ConvertPNGToSprite(Path.Combine(extractPath, "Ground.png"));
            File.Delete(Path.Combine(extractPath, "Ground.png"));
        }
        if (File.Exists(Path.Combine(extractPath, "Icon.png")))
        {
            icon = ConvertPNGToSprite(Path.Combine(extractPath, "Icon.png"));
            File.Delete(Path.Combine(extractPath, "Icon.png"));
        }
        if (File.Exists(Path.Combine(extractPath, "Music.mp3")))
        {
            StartCoroutine(ConvertMP3ToAudioClip("file://" + Path.Combine(extractPath, "Music.mp3")));
            File.Delete(Path.Combine(extractPath, "Music.mp3"));
        }

        string mapName = GetINIVar("Properties", "MapName", data);
        string note = Regex.Unescape(GetINIVar("Properties", "Note", data));
        string warden = GetINIVar("Properties", "Warden", data);
        int guardCount = Convert.ToInt32(GetINIVar("Properties", "Guards", data));
        int inmateCount = Convert.ToInt32(GetINIVar("Properties", "Inmates", data));
        string tilesetStr = GetINIVar("Properties", "Tileset", data);
        string groundStr = GetINIVar("Properties", "Ground", data);
        string iconStr = GetINIVar("Properties", "Icon", data);
        string musicStr = GetINIVar("Properties", "Music", data);
        if (tilesetStr != "Custom")
        {
            tileset = TextureToSprite(givenDataScript.tileTextureList[tilesetDict[prisonDict[tilesetStr]]]);
        }
        if (groundStr != "Custom")
        {
            if (groundStr == "black")
            {
                ground = TextureToSprite(givenDataScript.groundTextureList[19]);
            }
            else
            {
                ground = TextureToSprite(givenDataScript.groundTextureList[groundDict[prisonDict[groundStr]]]);
            }
        }
        //set music later. this still needs to be added to getgivendata (i think)
        if (iconStr == "None")
        {
            icon = Resources.Load<Sprite>("Map Editor Resources/UI Stuff/noicon");
        }
        int npcLevel = Convert.ToInt32(GetINIVar("Properties", "NPCLevel", data));
        string grounds = GetINIVar("Properties", "Grounds", data);
        string sizeStr = GetINIVar("Properties", "Size", data);
        string[] sizeParts = sizeStr.Split('x');
        int sizeX = Convert.ToInt32(sizeParts[0]);
        int sizeY = Convert.ToInt32(sizeParts[1]);
        string hint1 = Regex.Unescape(GetINIVar("Properties", "Hint1", data));
        string hint2 = Regex.Unescape(GetINIVar("Properties", "Hint2", data));
        string hint3 = Regex.Unescape(GetINIVar("Properties", "Hint3", data));
        string snowingStr = GetINIVar("Properties", "Snowing", data);
        string powStr = GetINIVar("Properties", "POWOutifts", data);
        string stunRodsStr = GetINIVar("Properties", "StunRods", data);
        bool snowing = false;
        bool powOutfits = false;
        bool stunRods = false;
        if (snowingStr == "True")
        {
            snowing = true;
        }
        if (powStr == "True")
        {
            powOutfits = true;
        }
        if (stunRodsStr == "True")
        {
            stunRods = true;
        }
        List<string> routineSet = GetINISet("Routine", data);
        Dictionary<int, string> routineDict = new Dictionary<int, string>();
        foreach (string str in routineSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split('=');
                int time = Convert.ToInt32(strParts[0]);
                string period = strParts[1];
                routineDict.Add(time, period);
            }
        }
        string startingJob = GetINIVar("Jobs", "StartingJob", data);
        bool janitor = false;
        bool gardening = false;
        bool laundry = false;
        bool kitchen = false;
        bool tailor = false;
        bool woodshop = false;
        bool metalshop = false;
        bool deliveries = false;
        bool mailman = false;
        bool library = false;
        if (GetINIVar("Properties", "Janitor", data) == "True")
        {
            janitor = true;
        }
        if (GetINIVar("Properties", "Gardening", data) == "True")
        {
            gardening = true;
        }
        if (GetINIVar("Properties", "Laundry", data) == "True")
        {
            laundry = true;
        }
        if (GetINIVar("Properties", "Kitchen", data) == "True")
        {
            kitchen = true;
        }
        if (GetINIVar("Properties", "Tailor", data) == "True")
        {
            tailor = true;
        }
        if (GetINIVar("Properties", "Woodshop", data) == "True")
        {
            woodshop = true;
        }
        if (GetINIVar("Properties", "Metalshop", data) == "True")
        {
            metalshop = true;
        }
        if (GetINIVar("Properties", "Deliveries", data) == "True")
        {
            deliveries = true;
        }
        if (GetINIVar("Properties", "Mailman", data) == "True")
        {
            mailman = true;
        }
        if (GetINIVar("Properties", "Library", data) == "True")
        {
            library = true;
        }
        List<int[]> tilesList = new List<int[]>();
        List<string> groundTiles = GetINISet("GroundTiles", data);
        List<string> undergroundTiles = GetINISet("UndergroundTiles", data);
        List<string> ventTiles = GetINISet("VentTiles", data);
        List<string> roofTiles = GetINISet("RoofTiles", data);
        foreach (string str in groundTiles)
        {
            string[] strParts = str.Split('=');
            string[] tileVars = strParts[1].Split(',');
            int tileID = int.Parse(tileVars[0]);
            int posX = Convert.ToInt32(tileVars[0]);
            int posY = Convert.ToInt32(tileVars[1]);
            int layer = 1;
            int[] tileArr = new int[4]
            {
                tileID, posX, posY, layer
            };
            tilesList.Add(tileArr);
        }
        foreach (string str in undergroundTiles)
        {
            string[] strParts = str.Split('=');
            string[] tileVars = strParts[1].Split(',');
            int tileID = int.Parse(tileVars[0]);
            int posX = Convert.ToInt32(tileVars[0]);
            int posY = Convert.ToInt32(tileVars[1]);
            int layer = 0;
            int[] tileArr = new int[4]
            {
                tileID, posX, posY, layer
            };
            tilesList.Add(tileArr);
        }
        foreach (string str in ventTiles)
        {
            string[] strParts = str.Split('=');
            string[] tileVars = strParts[1].Split(',');
            int tileID = int.Parse(tileVars[0]);
            int posX = Convert.ToInt32(tileVars[0]);
            int posY = Convert.ToInt32(tileVars[1]);
            int layer = 2;
            int[] tileArr = new int[4]
            {
                tileID, posX, posY, layer
            };
            tilesList.Add(tileArr);
        }
        foreach (string str in roofTiles)
        {
            string[] strParts = str.Split('=');
            string[] tileVars = strParts[1].Split(',');
            int tileID = int.Parse(tileVars[0]);
            int posX = Convert.ToInt32(tileVars[0]);
            int posY = Convert.ToInt32(tileVars[1]);
            int layer = 3;
            int[] tileArr = new int[4]
            {
                tileID, posX, posY, layer
            };
            tilesList.Add(tileArr);
        }
        List<string> groundObjSet = GetINISet("GroundObjects", data);
        List<string> undergroundObjSet = GetINISet("UndergroundObjects", data);
        List<string> ventObjSet = GetINISet("VentObjects", data);
        List<string> roofObjSet = GetINISet("RoofObjects", data);
        List<string> objNames = new List<string>();
        List<float[]> objVars = new List<float[]>();
        foreach (string str in groundObjSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split("=");
                objNames.Add(strParts[0]);
                string[] varParts = strParts[1].Split(',');
                float[] vars = new float[3]
                {
                    Convert.ToSingle(varParts[0]),
                    Convert.ToSingle(varParts[1]),
                    1
                };
            }
        }
        foreach (string str in undergroundObjSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split("=");
                objNames.Add(strParts[0]);
                string[] varParts = strParts[1].Split(',');
                float[] vars = new float[3]
                {
                    Convert.ToSingle(varParts[0]),
                    Convert.ToSingle(varParts[1]),
                    0
                };
            }
        }
        foreach (string str in ventObjSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split("=");
                objNames.Add(strParts[0]);
                string[] varParts = strParts[1].Split(',');
                float[] vars = new float[3]
                {
                    Convert.ToSingle(varParts[0]),
                    Convert.ToSingle(varParts[1]),
                    2
                };
            }
        }
        foreach (string str in roofObjSet)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] strParts = str.Split("=");
                objNames.Add(strParts[0]);
                string[] varParts = strParts[1].Split(',');
                float[] vars = new float[3]
                {
                    Convert.ToSingle(varParts[0]),
                    Convert.ToSingle(varParts[1]),
                    3
                };
            }
        }
        List<string> zoneNames = new List<string>();
        List<float[]> zoneVars = new List<float[]>();
        List<string> zoneSet = GetINISet("Zones", data);
        foreach(string str in zoneSet)
        {
            string[] strParts = str.Split('=');
            zoneNames.Add(strParts[0]);
            string[] varParts = strParts[1].Split(';');
            string[] posParts = varParts[0].Split(',');
            string[] aSizeParts = varParts[1].Split("x");
            float[] vars = new float[4]
            {
                Convert.ToSingle(posParts[0]), Convert.ToSingle(posParts[1]),
                Convert.ToSingle(aSizeParts[0]), Convert.ToSingle(aSizeParts[1])
            };
        }

        Map map = new Map(mapName, note, warden, guardCount, inmateCount, tileset, ground, music, icon, npcLevel, grounds, sizeX, sizeY, hint1, hint2, hint3, snowing, powOutfits, stunRods, routineDict, startingJob, janitor, gardening, laundry, kitchen, tailor, woodshop, metalshop, deliveries, mailman, library, tilesList, objNames, objVars, zoneNames, zoneVars);
        return map;
    }
    private Sprite ConvertPNGToSprite(string path)
    {
        byte[] pngBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(pngBytes);

        texture.filterMode = FilterMode.Point;

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100.0f);
    }
    private IEnumerator ConvertMP3ToAudioClip(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading audio: " + www.error);
            }
            else
            {
                music = DownloadHandlerAudioClip.GetContent(www);
            }
        }
    }
    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f), // pivot in the center
            100f                     // pixels per unit
        );
    }
    public List<string> GetINISet(string header, string[] file)
    {
        int startLine = -1;
        int endLine = file.Length;

        // Find the header line
        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains($"[{header}]"))
            {
                startLine = i + 1; // Start after the header
                break;
            }
        }

        if (startLine == -1)
            return new List<string>(); // Header not found

        // Find the next header or end of file
        for (int i = startLine; i < file.Length; i++)
        {
            if (file[i].StartsWith("[") && file[i].EndsWith("]"))
            {
                endLine = i;
                break;
            }
        }

        List<string> setList = new List<string>();
        for (int i = startLine; i < endLine; i++)
        {
            if (file[i].Contains('='))
            {
                setList.Add(file[i]);
            }
        }

        return setList;
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Split('=')[0] == varName)
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }



        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }

}
