using SFB;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System;

public class LoadMap : MonoBehaviour
{
    public Transform uic;
    public Transform tilesParent;
    public Transform groundsParent;
    public Transform gridLines;

    private GetGivenData givenDataScript;
    
    public AudioClip music = null;
    public Sprite tiles = null;
    public Sprite ground = null;
    public Sprite icon = null;
    public string[] data = null;

    private Dictionary<string, int> tilesetDict = new Dictionary<string, int>()
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

    public Sprite checkedBox;
    public Sprite uncheckedBox;
    private void Start()
    {
        givenDataScript = GetGivenData.instance;
    }
    public void StartLoad()
    {
        ExtensionFilter[] extensions = new ExtensionFilter[]
        {
            new ExtensionFilter("Map Files", "zmap")
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a map file.", "", extensions, false);
        if(paths.Length < 0) //if nothign was selected
        {
            return;
        }

        //unzip files
        string extractPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "CustomPrisons") + Path.DirectorySeparatorChar;

        ZipFile.ExtractToDirectory(paths[0], extractPath);

        //load Data.ini to a string[]
        data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));
        File.Delete(Path.Combine(extractPath, "Data.ini"));

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i].Replace("\n", "").Replace("\r", "");
        }
        //load other files to memory
        if (File.Exists(Path.Combine(extractPath, "Tiles.png")))
        {
            tiles = ConvertPNGToSprite(Path.Combine(extractPath, "Tiles.png"));
            File.Delete(Path.Combine(extractPath, "Tiles.png"));
        }
        if(File.Exists(Path.Combine(extractPath, "Ground.png")))
        {
            ground = ConvertPNGToSprite(Path.Combine(extractPath, "Ground.png"));
            File.Delete(Path.Combine(extractPath, "Ground.png"));
        }
        if(File.Exists(Path.Combine(extractPath, "Icon.png")))
        {
            icon = ConvertPNGToSprite(Path.Combine(extractPath, "Icon.png"));
            File.Delete(Path.Combine(extractPath, "Icon.png"));
        }
        if (File.Exists(Path.Combine(extractPath, "Music.mp3")))
        {
            StartCoroutine(ConvertMP3ToAudioClip("file://" + Path.Combine(extractPath, "Music.mp3")));
            File.Delete(Path.Combine(extractPath, "Music.mp3"));
        }

        LoadProperties();
        DeleteTiles();
        LoadTiles();
        LoadObjects();
        LoadGround();

        //set sizes of ground and grid
        string rawSizeStr = GetINIVar("Properties", "Size", data);
        string[] parts = rawSizeStr.Split('x');
        int gridX = Convert.ToInt32(parts[0]);
        int gridY = Convert.ToInt32(parts[1]);

        gridLines.GetComponent<RuntimeGrid>().DrawGrid(gridX, gridY);
        groundsParent.GetComponent<GroundSizeSet>().SetSize();

        LoadZones();
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
    private void LoadProperties()
    {
        Transform properties = uic.Find("PropertiesPanel");

        properties.Find("NameInputField").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "MapName", data);
        properties.Find("GuardsNum").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "Guards", data);
        properties.Find("InmatesNum").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "Inmates", data);
        properties.Find("NPCLevelNum").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "NPCLevel", data);
        properties.Find("TilesetResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Tileset", data);
        properties.Find("GroundResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Ground", data);
        properties.Find("MusicResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Music", data);
        properties.Find("IconResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Icon", data);
        properties.Find("GroundsResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Grounds", data);
        properties.Find("SizeResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Size", data);
        uic.Find("NotePanel").Find("NoteInputField").GetComponent<TMP_InputField>().text = Regex.Unescape(GetINIVar("Properties", "Note", data));
        uic.Find("NotePanel").Find("WardenInputField").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "Warden", data);
        Transform routineInputGrid1 = uic.Find("RoutinePanel").Find("RoutineInputGrid1");
        for(int i = 0; i <= 11; i++)
        {
            string time = i.ToString();
            if(time.Length == 1)
            {
                time = "0" + time;
            }

            routineInputGrid1.Find(time + ":00Input").GetComponent<TMP_InputField>().text = GetINIVar("Properties", time, data);
        }
        Transform routineInputGrid2 = uic.Find("RoutinePanel").Find("RoutineInputGrid2");
        for(int i = 12; i <= 23; i++)
        {
            string time = i.ToString();

            routineInputGrid2.Find(time + ":00Input").GetComponent<TMP_InputField>().text = GetINIVar("Routine", time, data);
        }
        Transform hintPanel = uic.Find("HintPanel");
        hintPanel.Find("Hint1Input").GetComponent<TMP_InputField>().text = Regex.Unescape(GetINIVar("Properties", "Hint1", data));
        hintPanel.Find("Hint2Input").GetComponent<TMP_InputField>().text = Regex.Unescape(GetINIVar("Properties", "Hint2", data));
        hintPanel.Find("Hint3Input").GetComponent<TMP_InputField>().text = Regex.Unescape(GetINIVar("Properties", "Hint3", data));
        Transform jobPanel = uic.Find("JobPanel");
        jobPanel.Find("StartingJobInput").GetComponent<TMP_InputField>().text = GetINIVar("Jobs", "StartingJob", data);
        Transform checkBoxGrid1 = jobPanel.Find("CheckBoxGrid1");
        Transform checkBoxGrid2 = jobPanel.Find("CheckBoxGrid2");
        foreach(Transform box in checkBoxGrid1)
        {
            if(GetINIVar("Jobs", box.name.Replace("CheckBox", ""), data) == "True")
            {
                box.GetComponent<Image>().sprite = checkedBox;
            }
            else if(GetINIVar("Jobs", box.name.Replace("CheckBox", ""), data) == "False")
            {
                box.GetComponent<Image>().sprite = uncheckedBox;
            }
        }
        foreach(Transform box in checkBoxGrid2)
        {
            if (GetINIVar("Jobs", box.name.Replace("CheckBox", ""), data) == "True")
            {
                box.GetComponent<Image>().sprite = checkedBox;
            }
            else if (GetINIVar("Jobs", box.name.Replace("CheckBox", ""), data) == "False")
            {
                box.GetComponent<Image>().sprite = uncheckedBox;
            }
        }
        Transform extrasPanel = uic.Find("ExtrasPanel");
        if (GetINIVar("Properties", "Snowing", data) == "True")
        {
            extrasPanel.Find("SnowingCheckbox").GetComponent<Image>().sprite = checkedBox;
        }
        else
        {
            extrasPanel.Find("SnowingCheckbox").GetComponent<Image>().sprite = uncheckedBox;
        }
        if (GetINIVar("Properties", "POWOutfits", data) == "True")
        {
            extrasPanel.Find("POWCheckbox").GetComponent<Image>().sprite = checkedBox;
        }
        else
        {
            extrasPanel.Find("POWCheckbox").GetComponent<Image>().sprite = uncheckedBox;
        }
        if (GetINIVar("Properties", "StunRods", data) == "True")
        {
            extrasPanel.Find("StunRodCheckbox").GetComponent<Image>().sprite = checkedBox;
        }
        else
        {
            extrasPanel.Find("StunRodCheckbox").GetComponent<Image>().sprite = uncheckedBox;
        }
    }
    private void DeleteTiles()
    {
        foreach(Transform tileGroup in tilesParent)
        {
            foreach(Transform tile in tileGroup)
            {
                Destroy(tile.gameObject);
            }

            GameObject empty = new GameObject();
            empty.name = "empty";
            empty.transform.position = new Vector3(9999, 9999);
            empty.transform.SetParent(tileGroup);
            empty.AddComponent<BoxCollider2D>();
        }
    }
    private void LoadTiles()
    {
        Debug.Log("Loading tiles...");
        Texture2D tileset;
        string tilesetChoice = GetINIVar("Properties", "Tileset", data);

        if(tilesetChoice != "Custom")
        {
            int prisonIndex = tilesetDict[prisonDict[tilesetChoice]];
            tileset = givenDataScript.tileTextureList[prisonIndex];
        }
        else
        {
            tileset = tiles.texture;
        }

        GetComponent<TileSpriteSetter>().SetSprites(tileset);
        
        List<string> groundTiles = GetINISet("GroundTiles", data);
        List<string> ventTiles = GetINISet("VentTiles", data);
        List<string> roofTiles = GetINISet("RoofTiles", data);
        List<string> undergroundTiles = GetINISet("UndergroundTiles", data);

        List<List<string>> Listlist = new List<List<string>>()
        {
            groundTiles, ventTiles, roofTiles, undergroundTiles
        };

        foreach(List<string> list in Listlist)
        {
            string layer = null;
            if(list == groundTiles)
            {
                layer = "Ground";
            }
            else if(list == ventTiles)
            {
                layer = "Vent";
            }
            else if(list == roofTiles)
            {
                layer = "Roof";
            }
            else if(list == undergroundTiles)
            {
                layer = "Underground";
            }
            
            for (int i = 0; i < list.Count; i++)
            {
                string[] pair = list[i].Split('=');
                string rawPos = pair[1];
                string[] parts = rawPos.Split(',');

                string name = pair[0];
                Vector2 tilePos = new Vector2(Convert.ToSingle(parts[0]), Convert.ToSingle(parts[1]));

                //convert tilePos to actual world space
                float tilePosX = (tilePos.x * 1.6f) - 1.6f;
                float tilePosY = (tilePos.y * 1.6f) - 1.6f;

                tilePos = new Vector2(tilePosX, tilePosY);

                PlaceTile(layer, tilePos, name);
            }
        }
    }
    private void LoadObjects()
    {
        List<string> groundObjects = GetINISet("GroundObjects", data);
        List<string> ventObjects = GetINISet("VentObjects", data);
        List<string> roofObjects = GetINISet("RoofObjects", data);
        List<string> undergroundObjects = GetINISet("UndergroundObjects", data);

        List<List<string>> Listlist = new List<List<string>>()
        {
            groundObjects, ventObjects, roofObjects, undergroundObjects
        };

        foreach(List<string> list in Listlist)
        {
            string layer = null;
            if (list == groundObjects)
            {
                layer = "GroundObjects";
            }
            else if (list == ventObjects)
            {
                layer = "VentObjects";
            }
            else if (list == roofObjects)
            {
                layer = "RoofObjects";
            }
            else if (list == undergroundObjects)
            {
                layer = "UndergroundObjects";
            }
            
            for(int i = 0; i < list.Count; i++)
            {
                string[] pair = list[i].Split('=');
                string rawPos = pair[1];
                string[] parts = rawPos.Split(',');

                string name = pair[0];
                Vector2 objPos = new Vector2(Convert.ToSingle(parts[0]), Convert.ToSingle(parts[1]));

                //convert objPos to actual world space
                float objPosX = (objPos.x * 1.6f) - 1.6f;
                float objPosY = (objPos.y * 1.6f) - 1.6f;

                objPos = new Vector2 (objPosX, objPosY);

                PlaceObj(layer, objPos, name);
            }
        }
    }
    private void LoadGround()
    {
        Texture2D groundTex;
        string groundChoice = GetINIVar("Properties", "Ground", data);

        if(groundChoice != "Custom")
        {
            int prisonIndex = tilesetDict[prisonDict[groundChoice]];
            groundTex = givenDataScript.groundTextureList[prisonIndex];
        }
        else
        {
            groundTex = ground.texture;
        }

        groundTex.filterMode = FilterMode.Point;

        bool isTiled = false;
        if (groundChoice != "BC" && groundChoice != "JC" && groundChoice != "DTAF" && groundChoice != "Custom")
        {
            groundTex = textureCornerGet(groundTex, 16, 16);
            isTiled = true;
        }

        Transform groundTransform = groundsParent.Find("Ground");

        Sprite groundSprite = Sprite.Create(groundTex, new Rect(0, 0, groundTex.width, groundTex.height), new Vector2(.5f, .5f), 100.0f);

        groundTransform.GetComponent<SpriteRenderer>().sprite = groundSprite;

        if (isTiled)
        {
            groundTransform.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
        }
        else
        {
            groundTransform.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            groundTransform.GetComponent<SpriteRenderer>().size = new Vector2(groundTex.width * .01f, groundTex.height * .01f);
        }
    }
    private void LoadZones()
    {
        List<string> zoneSet = GetINISet("Zones", data);

        for(int i = 0; i < zoneSet.Count; i++)
        {
            string[] pair = zoneSet[i].Split('=');
            string zoneName = pair[0];
            string zoneVars = pair[1];

            string[] zoneParts = zoneVars.Split(";");

            //instantiate zone
            GameObject zoneObj = Instantiate(Resources.Load<GameObject>("MapEditorPrefabs/ZoneObject"));
            zoneObj.transform.SetParent(tilesParent.Find("Zones"));
            zoneObj.name = zoneName;
            Transform nw = zoneObj.transform.Find("NW");
            Transform ne = zoneObj.transform.Find("NE");
            Transform sw = zoneObj.transform.Find("SW");
            Transform se = zoneObj.transform.Find("SE");
            Transform mover = zoneObj.transform.Find("Mover");
            Transform nameText = zoneObj.transform.Find("NameText");
            nameText.GetComponent<TextMeshPro>().text = zoneName;
            nameText.GetComponent<MeshRenderer>().sortingOrder = 10;

            float posX = Convert.ToSingle(zoneParts[0].Split(',')[0]) * 1.6f - 1.6f;
            float posY = Convert.ToSingle(zoneParts[0].Split(',')[1]) * 1.6f - 1.6f;
            float sizeX = Convert.ToSingle(zoneParts[1].Split('x')[0]) * .16f;
            float sizeY = Convert.ToSingle(zoneParts[1].Split('x')[1]) * .16f;

            Vector2 zonePos = new Vector2(posX, posY);
            Vector2 zoneSize = new Vector2(sizeX, sizeY);

            //set pos and size
            zoneObj.transform.position = zonePos;
            zoneObj.GetComponent<SpriteRenderer>().size = zoneSize;

            //set pos of handles
            nw.localPosition = new Vector3(zoneSize.x / -2f, zoneSize.y / 2f);
            ne.localPosition = new Vector3(zoneSize.x / 2f, zoneSize.y / 2f);
            sw.localPosition = new Vector3(zoneSize.x / -2f, zoneSize.y / -2f);
            se.localPosition = new Vector3(zoneSize.x / 2f, zoneSize.y / -2f);
        }
    }
    private void PlaceTile(string layer, Vector2 pos, string name)
    {        
        List<Sprite> sprites = GetComponent<TileSpriteSetter>().sprites;

        int tileIndex = Convert.ToInt32(name.Replace("tile", ""));
        Sprite tileSprite = sprites[tileIndex];

        GameObject placedTile = new GameObject();
        placedTile.AddComponent<SpriteRenderer>();
        placedTile.AddComponent<BoxCollider2D>();
        placedTile.name = name;
        placedTile.transform.position = pos;
        placedTile.transform.SetParent(tilesParent.Find(layer));
        placedTile.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 1.6f);
        placedTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        placedTile.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        placedTile.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);

        if(layer == "Ground" || layer == "Underground")
        {
            placedTile.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            placedTile.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }
    private void PlaceObj(string layer, Vector2 pos, string name)
    {
        Sprite objSprite = null;
        GameObject objToPlace = null;
        foreach(Transform obj in uic.Find("ObjectsPanel").Find("ObjectsScrollRect").Find("Viewport").Find("Content"))
        {
            if(obj.name == name)
            {
                objSprite = obj.GetComponent<Image>().sprite;
                objToPlace = obj.gameObject;
                break;
            }
        }
        
        GameObject placedObj = new GameObject();
        placedObj.AddComponent<SpriteRenderer>();
        placedObj.AddComponent<BoxCollider2D>();
        placedObj.name = name;
        placedObj.transform.position = pos;
        placedObj.transform.SetParent(tilesParent.Find(layer));
        placedObj.GetComponent<BoxCollider2D>().size = objToPlace.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f);
        placedObj.GetComponent<SpriteRenderer>().sprite = GetComponent<ObjectPlacer>().RemovePaddingToSprite(objSprite, 1);
        placedObj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        placedObj.GetComponent<SpriteRenderer>().size = objToPlace.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f);
        
        if(layer == "GroundObjects" || layer == "UndergroundObjects")
        {
            placedObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        else
        {
            placedObj.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
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
        
        for(int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for(int j = i; j < file.Length - i; j++)
                {
                    if (file[j].Contains(varName))
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }

        if(line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');

        return parts[1];
    }
    private Texture2D textureCornerGet(Texture2D source, int sizeX, int sizeY)
    {
        Color[] pixels = source.GetPixels(0, 0, sizeX, sizeY);

        Texture2D cornerTex = new Texture2D(sizeX, sizeY, source.format, false);
        cornerTex.SetPixels(pixels);
        cornerTex.Apply();
        cornerTex.filterMode = FilterMode.Point;

        return cornerTex;
    }
}
