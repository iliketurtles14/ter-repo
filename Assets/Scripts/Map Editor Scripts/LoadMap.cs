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
using System.Runtime.CompilerServices;

public class LoadMap : MonoBehaviour
{
    public Transform uic;
    public Transform tilesParent;
    public Transform groundsParent;
    public Transform gridLines;
    public Transform canvases;

    private GetGivenData givenDataScript;

    public List<AudioClip> music = null;
    public Dictionary<int, Sprite> customItemSprites = null;
    public Sprite tiles = null;
    public Sprite ground = null;
    public Sprite icon = null;
    public string[] data = null;
    public string[] speech = null;
    public string[] items = null;
    public string[] tooltips = null;

    private List<string> musicNames = new List<string>
    {
        "chow.mp3", "escaped.mp3", "lightsout.mp3", "lockdown.mp3", "rollcall.mp3",
        "shower.mp3", "work.mp3", "workout.mp3", "freetime.mp3"
    };

    private Dictionary<string, int> groundDict = new Dictionary<string, int>() //also for tilesets
    {
        { "alca", 14 }, { "BC", 8 }, { "campepsilon", 16 }, { "CCL", 7 },
        { "DTAF", 12 }, { "EA", 15 }, { "escapeteam", 13 }, { "fortbamford", 17 },
        { "irongate", 6 }, { "jungle", 4 }, { "pcpen", 10 }, { "perks", 1 },
        { "sanpancho", 5 }, { "shanktonstatepen", 3 }, { "SS", 11 }, { "stalagflucht", 2 },
        { "TOL", 9 }, { "tutorial", 0 }
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
    public void StartLoad(bool isNewMap)
    {
        if (isNewMap)
        {
            TextAsset iniFile = Resources.Load<TextAsset>("Data");
            data = iniFile.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }
        else
        {
            ExtensionFilter[] extensions = new ExtensionFilter[]
            {
                new ExtensionFilter("Map Files", "zmap")
            };
            string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a map file.", "", extensions, false);
            if (paths.Length < 0) //if nothign was selected
            {
                return;
            }

            //unzip files
            string extractPath = Path.Combine(Application.streamingAssetsPath, "temp") + Path.DirectorySeparatorChar;

            ZipFile.ExtractToDirectory(paths[0], extractPath);

            //load Data.ini to a string[]
            data = File.ReadAllLines(Path.Combine(extractPath, "Data.ini"));
            File.Delete(Path.Combine(extractPath, "Data.ini"));

            //load other files to memory
            if (File.Exists(Path.Combine(extractPath, "Tiles.png")))
            {
                tiles = ConvertPNGToSprite(Path.Combine(extractPath, "Tiles.png"));
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
            if (File.Exists(Path.Combine(extractPath, "Speech.ini")))
            {
                speech = File.ReadAllLines(Path.Combine(extractPath, "Speech.ini"));
                File.Delete(Path.Combine(extractPath, "Speech.ini"));
            }
            if (File.Exists(Path.Combine(extractPath, "Tooltips.ini")))
            {
                tooltips = File.ReadAllLines(Path.Combine(extractPath, "Tooltips.ini"));
                File.Delete(Path.Combine(extractPath, "Tooltips.ini"));
            }
            if (File.Exists(Path.Combine(extractPath, "Items.ini")))
            {
                items = File.ReadAllLines(Path.Combine(extractPath, "Items.ini"));
                File.Delete(Path.Combine(extractPath, "Items.ini"));
            }
            if (File.Exists(Path.Combine(extractPath, "Music.zip")))
            {
                ZipFile.ExtractToDirectory(Path.Combine(extractPath, "Music.zip"), extractPath);
                StartCoroutine(ConvertMP3ToAudioClip("file://" + extractPath));
                File.Delete(Path.Combine(extractPath, "Music.zip"));
                //it deletes the files in the coroutine
            }
            if(Directory.Exists(Path.Combine(extractPath, "Items")))
            {
                foreach(string file in Directory.GetFiles(Path.Combine(extractPath, "Items")))
                {
                    int id = Convert.ToInt32(Path.GetFileName(file).Split('.')[0]);
                    Sprite sprite = ConvertPNGToSprite(file);
                    customItemSprites.Add(id, sprite);
                }

                Directory.Delete(Path.Combine(extractPath, "Items"));
            }
        }

        for (int i = 0; i < data.Length; i++)
        {
            data[i] = data[i].Replace("\n", "").Replace("\r", "");
        }

        LoadProperties();
        DeleteTiles();
        LoadTiles();
        LoadObjects();
        LoadObjectProperties();
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
        foreach(string name in musicNames)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(Path.Combine(path, name), AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error loading audio: " + www.error);
                }
                else
                {
                    music.Add(DownloadHandlerAudioClip.GetContent(www));
                }
            }
            File.Delete(Path.Combine(path, name));
        }
    }
    private void LoadProperties()
    {
        Transform properties = uic.Find("PropertiesPanel");
        Transform advanced = uic.Find("AdvancedPanel");

        properties.Find("NameInputField").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "MapName", data);
        properties.Find("GuardsNum").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "Guards", data);
        properties.Find("InmatesNum").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "Inmates", data);
        properties.Find("NPCLevelNum").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "NPCLevel", data);
        properties.Find("TilesetResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Tileset", data);
        properties.Find("GroundResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Ground", data);
        properties.Find("IconResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Icon", data);
        properties.Find("GroundsResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Grounds", data);
        properties.Find("SizeResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Size", data);
        advanced.Find("MusicResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Music", data);
        advanced.Find("SpeechResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Speech", data);
        advanced.Find("TooltipsResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Tooltips", data);
        advanced.Find("ItemsResultText").GetComponent<TextMeshProUGUI>().text = GetINIVar("Properties", "Items", data);
        uic.Find("NotePanel").Find("NoteInputField").GetComponent<TMP_InputField>().text = Regex.Unescape(GetINIVar("Properties", "Note", data));
        uic.Find("NotePanel").Find("WardenInputField").GetComponent<TMP_InputField>().text = GetINIVar("Properties", "Warden", data);
        Transform routinePanel = uic.Find("RoutinePanel");
        Transform routineGrid1 = routinePanel.Find("RoutineInputGrid1");
        Transform routineGrid2 = routinePanel.Find("RoutineInputGrid2");
        List<string> routineText = GetINISet("Routine", data);
        string[] parts = routineText.ToArray();
        int i = 0;
        foreach (Transform child in routineGrid1)
        {
            child.GetComponent<TMP_InputField>().text = parts[i].Split("=")[1];
            i++;
        }
        int j = i;
        foreach (Transform child in routineGrid2)
        {
            child.GetComponent<TMP_InputField>().text = parts[j].Split("=")[1];
            j++;
        }
        Transform hintPanel = uic.Find("HintPanel");
        hintPanel.Find("Hint1Input").GetComponent<TMP_InputField>().text = Regex.Unescape(GetINIVar("Properties", "Hint1", data));
        hintPanel.Find("Hint2Input").GetComponent<TMP_InputField>().text = Regex.Unescape(GetINIVar("Properties", "Hint2", data));
        hintPanel.Find("Hint3Input").GetComponent<TMP_InputField>().text = Regex.Unescape(GetINIVar("Properties", "Hint3", data));
        Transform jobPanel = uic.Find("JobPanel");
        jobPanel.Find("StartingJobInput").GetComponent<TMP_InputField>().text = GetINIVar("Jobs", "StartingJob", data);

        SubMenuController smc = GetComponent<SubMenuController>();
        if (GetINIVar("Jobs", "Janitor", data) == "True")
        {
            smc.janitor = true;
        }
        else
        {
            smc.janitor = false;
        }
        if (GetINIVar("Jobs", "Gardening", data) == "True")
        {
            smc.gardening = true;
        }
        else
        {
            smc.gardening = false;
        }
        if (GetINIVar("Jobs", "Laundry", data) == "True")
        {
            smc.laundry = true;
        }
        else
        {
            smc.laundry = false;
        }
        if (GetINIVar("Jobs", "Kitchen", data) == "True")
        {
            smc.kitchen = true;
        }
        else
        {
            smc.kitchen = false;
        }
        if (GetINIVar("Jobs", "Tailor", data) == "True")
        {
            smc.tailor = true;
        }
        else
        {
            smc.tailor = false;
        }
        if (GetINIVar("Jobs", "Woodshop", data) == "True")
        {
            smc.woodshop = true;
        }
        else
        {
            smc.woodshop = false;
        }
        if (GetINIVar("Jobs", "Metalshop", data) == "True")
        {
            smc.metalshop = true;
        }
        else
        {
            smc.metalshop = false;
        }
        if (GetINIVar("Jobs", "Deliveries", data) == "True")
        {
            smc.deliveries = true;
        }
        else
        {
            smc.deliveries = false;
        }
        if (GetINIVar("Jobs", "Mailman", data) == "True")
        {
            smc.mailman = true;
        }
        else
        {
            smc.mailman = false;
        }
        if (GetINIVar("Jobs", "Library", data) == "True")
        {
            smc.library = true;
        }
        else
        {
            smc.library = false;
        }

        Transform extrasPanel = uic.Find("ExtrasPanel");
        if (GetINIVar("Properties", "Snowing", data) == "True")
        {
            smc.snowing = true;
        }
        else
        {
            smc.snowing = false;
        }
        if (GetINIVar("Properties", "POWOutfits", data) == "True")
        {
            smc.powOutfits = true;
        }
        else
        {
            smc.powOutfits = false;
        }
        if (GetINIVar("Properties", "StunRods", data) == "True")
        {
            smc.stunRods = true;
        }
        else
        {
            smc.stunRods = false;
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
            int prisonIndex = groundDict[prisonDict[tilesetChoice]];
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
    private void LoadObjectProperties()
    {
        List<string> groundList = GetINISet("GroundObjectProperties", data);
        List<string> undergroundList = GetINISet("UndergroundObjectProperties", data);
        List<string> ventList = GetINISet("VentObjectProperties", data);
        List<string> roofList = GetINISet("RoofObjectProperties", data);

        List<string> layerList = new List<string>
        {
            "GroundObjects", "UndergroundObjects", "VentObjects", "RoofObjects"
        };
        List<List<string>> listList = new List<List<string>>
        {
            groundList, undergroundList, ventList, roofList
        };

        for(int i = 0; i < 4; i++)
        {
            string layer = layerList[i];
            List<string> currentList = listList[i];
            foreach (Transform obj in tilesParent.Find(layer))
            {
                switch (obj.name)
                {
                    case "Item":
                        foreach (string line in currentList)
                        {
                            string name = line.Split("=")[0];
                            if (name != obj.name)
                            {
                                continue;
                            }
                            string position = line.Split("=")[1].Split(";")[0];
                            int id = Convert.ToInt32(line.Split(";")[1]);
                            float posX = Convert.ToSingle(position.Split(",")[0]);
                            float posY = Convert.ToSingle(position.Split(",")[1]);
                            posX = (posX * 1.6f) - 1.6f;
                            posY = (posY * 1.6f) - 1.6f;
                            Vector2 pos = new Vector2(posX, posY);

                            float distance = Vector2.Distance(obj.position, pos);
                            if (distance < .01f)
                            {
                                obj.GetComponent<MEItemIDContainer>().id = id;
                                break;
                            }
                        }
                        break;
                    case "ChristmasDesk":
                    case "DTAFSpecialDesk":
                    case "ETSpecialDesk":
                        foreach (string line in currentList)
                        {
                            string name = line.Split("=")[0];
                            if(name != obj.name)
                            {
                                continue;
                            }
                            string position = line.Split("=")[1].Split(";")[0];
                            float posX = Convert.ToSingle(position.Split(",")[0]);
                            float posY = Convert.ToSingle(position.Split(",")[1]);
                            posX = (posX * 1.6f) - 1.6f;
                            posY = (posY * 1.6f) - 1.6f;
                            Vector2 pos = new Vector2(posX, posY);
                            string idListStr = line.Split(";")[1];
                            string[] idStrArr = idListStr.Split(",");
                            List<int> idList = new List<int>();
                            foreach (string idStr in idStrArr)
                            {
                                int id = Convert.ToInt32(idStr);
                                idList.Add(id);
                            }

                            float distance = Vector2.Distance(obj.position, pos);
                            if (distance < .01f)
                            {
                                for (int j = 0; j < 20; j++)
                                {
                                    obj.GetComponent<MEDeskListContainer>().ids[j] = idList[j];
                                }
                                break;
                            }
                        }
                        break;
                    case "DTAFSign":
                    case "SSSign":
                    case "DTAFPlaque":
                        foreach (string line in currentList)
                        {
                            string name = line.Split("=")[0];
                            if (name != obj.name)
                            {
                                continue;
                            }
                            string position = line.Split("=")[1].Split(";")[0];
                            float posX = Convert.ToSingle(position.Split(",")[0]);
                            float posY = Convert.ToSingle(position.Split(",")[1]);
                            posX = (posX * 1.6f) - 1.6f;
                            posY = (posY * 1.6f) - 1.6f;
                            Vector2 pos = new Vector2(posX, posY);
                            string header = line.Split("{HEADER}:")[1].Split("{BODY}:")[0];
                            string body = line.Split("{BODY}:")[1];
                            header = Regex.Unescape(header);
                            body = Regex.Unescape(body);

                            float distance = Vector2.Distance(obj.position, pos);
                            if(distance < .01f)
                            {
                                obj.GetComponent<MESignTextContainer>().header = header;
                                obj.GetComponent<MESignTextContainer>().body = body;
                                break;
                            }
                        }
                        break;
                }
            }
        }
    }
    private void LoadGround()
    {
        Texture2D groundTex;
        string groundChoice = GetINIVar("Properties", "Ground", data);

        if(groundChoice != "Custom")
        {
            if(groundChoice == "black")
            {
                groundTex = givenDataScript.groundTextureList[19];
            }
            else
            {
                int prisonIndex = groundDict[prisonDict[groundChoice]];
                groundTex = givenDataScript.groundTextureList[prisonIndex];
            }
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
        Sprite tileSprite = null;
        if(tileIndex != 100)
        {
            tileSprite = sprites[tileIndex];
        }
        else
        {
            return;
        }

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
        Debug.Log("Placing Object: " + name);
        
        List<string> objectPanels = new List<string>
        {
            "WaypointsPanel", "CellsPanel", "SecurityPanel", "JobsPanel", "GymPanel",
            "MiscPanel", "DoorsPanel", "ZiplinePanel", "SpecialPanel", "ETPanel", "DTAF1Panel",
            "DTAF2Panel", "Christmas1Panel", "Christmas2Panel", "ItemsPanel"
        };

        Sprite objSprite = null;
        GameObject objToPlace = null;
        foreach(string panel in objectPanels)
        {
            foreach (Transform obj in uic.Find(panel))
            {
                if (obj.name == name)
                {
                    objSprite = obj.GetComponent<Image>().sprite;
                    objToPlace = obj.gameObject;
                    break;
                }
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
        placedObj.GetComponent<BoxCollider2D>().enabled = true;
        
        if(layer == "GroundObjects" || layer == "UndergroundObjects")
        {
            placedObj.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        else
        {
            placedObj.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }

        //special properties for certain objects
        string realLayer = layer.Replace("Objects", "");
        switch (objToPlace.name)
        {
            case "Item":
                GameObject canvas = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                canvas.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "item";
                canvas.transform.parent = canvases.Find(realLayer);
                canvas.transform.position = placedObj.transform.position;
                canvas.name = "SpecialObjectCanvas";
                placedObj.AddComponent<MEItemIDContainer>();
                break;
            case "ChristmasDesk":
            case "DTAFSpecialDesk":
            case "ETSpecialDesk":
                GameObject canvas1 = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                canvas1.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "desk";
                canvas1.transform.parent = canvases.Find(realLayer);
                canvas1.transform.position = placedObj.transform.position;
                canvas1.name = "SpecialObjectCanvas";
                placedObj.AddComponent<MEDeskListContainer>();
                break;
            case "DTAFSign":
                GameObject canvas2 = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                canvas2.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "whiteSign";
                canvas2.transform.parent = canvases.Find(realLayer);
                canvas2.transform.position = placedObj.transform.position;
                canvas2.name = "SpecialObjectCanvas";
                placedObj.AddComponent<MESignTextContainer>();
                break;
            case "SSSign":
                GameObject canvas3 = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                canvas3.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "blueSign";
                canvas3.transform.parent = canvases.Find(realLayer);
                canvas3.transform.position = placedObj.transform.position;
                canvas3.name = "SpecialObjectCanvas";
                placedObj.AddComponent<MESignTextContainer>();
                break;
            case "DTAFPlaque":
                GameObject canvas4 = Instantiate(canvases.Find("SpecialObjectCanvas").gameObject);
                canvas4.transform.Find("Button").GetComponent<SpecialButtonTypeContainer>().type = "blueSign";
                canvas4.transform.parent = canvases.Find(realLayer);
                canvas4.transform.position = placedObj.transform.position;
                canvas4.name = "SpecialObjectCanvas";
                canvas4.GetComponent<RectTransform>().sizeDelta = new Vector2(1.6f, 3.2f);
                canvas4.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 3.2f);
                placedObj.AddComponent<MESignTextContainer>();
                break;
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
