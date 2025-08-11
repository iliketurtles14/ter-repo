using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

public class CmapConvert : MonoBehaviour
{
    public Transform uic;
    
    private string[] cmapFile;
    private string convertedText;
    private string zoneText;

    private string routineType;
    private string routineText;

    private string groundTileText;
    private string undergroundTileText;
    private string ventTileText;
    private string roofTileText;
    private string groundObjText;
    private string undergroundObjText;
    private string ventObjText;
    private string roofObjText;

    private string mapName;
    private string note;
    private string warden;
    private int guards;
    private int inmates;
    private string tileset;
    private string ground;
    private string music;
    private string grounds;
    private int npcLevel;
    private bool laundry;
    private bool gardening;
    private bool janitor;
    private bool woodshop;
    private bool metalshop;
    private bool kitchen;
    private bool deliveries;
    private bool tailor;
    private bool mailman;
    private bool library;
    private string startingJob;
    public void ConvertCmap()
    {
        ExtensionFilter[] extensions = new ExtensionFilter[]
        {
            new ExtensionFilter("Cmap Files", "cmap")
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a cmap file.", "", extensions, false);
        if(paths.Length < 0)
        {
            return;
        }

        cmapFile = File.ReadAllLines(paths[0]);
        for(int i = 0; i < cmapFile.Length; i++)
        {
            cmapFile[i] = cmapFile[i].Replace("\n", "").Replace("\r", "");
        }

        GetProperties();
        GetZones();
        GetTiles();
        GetObjects();
        GetRoutine();
        CombineText();

        ExtensionFilter[] saveExtensions = new ExtensionFilter[]
        {
            new ExtensionFilter("Zmap Files", "zmap")
        };
        string savePath = StandaloneFileBrowser.SaveFilePanel("Save converted map as...", "", mapName, saveExtensions);
        if(paths.Length < 0)
        {
            return;
        }

        string saveDir = Path.GetDirectoryName(savePath);
        File.WriteAllText(Path.Combine(saveDir, "Data.ini"), convertedText);

        List<string> filesToZip = new List<string>();
        filesToZip.Add(Path.Combine(saveDir, "Data.ini"));

        using (FileStream zipToOpen = new FileStream(savePath, FileMode.Create))
        {
            using(ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
            {
                foreach(string filePath in filesToZip)
                {
                    archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                }
            }
        }
        File.Delete(Path.Combine(saveDir, "Data.ini"));
    }
    private void GetProperties()
    {
        mapName = GetINIVar("Info", "MapName", cmapFile);
        note = Regex.Escape(GetINIVar("Info", "Intro", cmapFile).Replace("#", "\n"));
        warden = GetINIVar("Info", "Warden", cmapFile);
        guards = Convert.ToInt32(GetINIVar("Info", "Guards", cmapFile));
        inmates = Convert.ToInt32(GetINIVar("Info", "Inmates", cmapFile));
        tileset = GetINIVar("Info", "Tileset", cmapFile);
        ground = GetINIVar("Info", "Floor", cmapFile);
        music = GetINIVar("Info", "Music", cmapFile);
        grounds = GetINIVar("Info", "MapType", cmapFile);
        if (grounds == "InsideOutside")
        {
            grounds = "In/Out";
        }
        routineType = GetINIVar("Info", "RoutineSet", cmapFile);
        npcLevel = Convert.ToInt32(GetINIVar("Info", "NPClvl", cmapFile));

        laundry = false;
        gardening = false;
        janitor = false;
        woodshop = false;
        metalshop = false;
        kitchen = false;
        deliveries = false;
        tailor = false;
        mailman = false;
        library = false;

        if (GetINIVar("Jobs", "Laundry", cmapFile) == "1")
        {
            laundry = true;
        }
        if (GetINIVar("Jobs", "Gardening", cmapFile) == "1")
        {
            gardening = true;
        }
        if (GetINIVar("Jobs", "Janitor", cmapFile) == "1")
        {
            janitor = true;
        }
        if (GetINIVar("Jobs", "Woodshop", cmapFile) == "1")
        {
            woodshop = true;
        }
        if (GetINIVar("Jobs", "Metalshop", cmapFile) == "1")
        {
            metalshop = true;
        }
        if (GetINIVar("Jobs", "Kitchen", cmapFile) == "1")
        {
            kitchen = true;
        }
        if (GetINIVar("Jobs", "Deliveries", cmapFile) == "1")
        {
            deliveries = true;
        }
        if (GetINIVar("Jobs", "Tailor", cmapFile) == "1")
        {
            tailor = true;
        }
        if (GetINIVar("Jobs", "Mailman", cmapFile) == "1")
        {
            mailman = true;
        }
        if (GetINIVar("Jobs", "Library", cmapFile) == "1")
        {
            library = true;
        }

        startingJob = GetINIVar("Jobs", "StartingJob", cmapFile);
    }
    private void GetZones()
    {
        List<string> zoneSet = GetINISet("Zones", cmapFile);
        zoneText = "";
        foreach(string zone in zoneSet)
        {
            if (zone.Contains('_')) //check to see if it actually exists
            {
                string[] parts = zone.Split('=');
                string zoneName = parts[0];
                string zoneVars = parts[1];

                //remove numbers after specific zones and rename some
                if (zoneName.StartsWith("Cells"))
                {
                    zoneName = "Cell";
                }
                else if (zoneName.StartsWith("Safe"))
                {
                    zoneName = "Safe";
                }
                else if (zoneName == "SHU")
                {
                    zoneName = "Solitary";
                }

                string[] varParts = zoneVars.Split('_');
                //get pos and size (the rounding here is done to snap any off-tile things onto a tile, since ter doesnt support that
                int rawPosX = Convert.ToInt32(Math.Round(Convert.ToSingle(varParts[0])));
                int rawPosY = Convert.ToInt32(Math.Round(Convert.ToSingle(varParts[1])));
                int rawSizeX = Convert.ToInt32(Math.Round(Convert.ToSingle(varParts[2])));
                int rawSizeY = Convert.ToInt32(Math.Round(Convert.ToSingle(varParts[3])));
                int posX;
                int posY;
                int sizeX;
                int sizeY;

                //convert to tile pos
                rawPosX = rawPosX / 16;
                rawPosY = rawPosY / 16;

                //make the (0, 0) point in the bottom left instead of top left
                //x stays the same since it doesnt change because of this
                posX = rawPosX;
                posY = 108 - rawPosY;

                //convert to tile size
                sizeX = rawSizeX / 16;
                sizeY = rawSizeY / 16;

                zoneText += zoneName + "=" + posX + "," + posY + ";" + sizeX + "x" + sizeY + "\n";
            }
        }
    }
    private void GetTiles()
    {
        groundTileText = "";
        ventTileText = "";
        roofTileText = "";
        undergroundTileText = "";

        List<string> layerList = new List<string>()
        {
            "[Tiles]", "[Vents]", "[Roof]", "[Underground]"
        };

        Dictionary<int, int> tilesetDict = new Dictionary<int, int>() //this is from te1 tileset mapping to ter tileset mapping
        {
            { 1, 0 }, { 2, 4 }, { 3, 8 }, { 4, 12 }, { 5, 16 }, { 6, 20  }, { 7, 24 },
            { 8, 28 }, { 9, 32 }, { 10, 36 }, { 11, 40 }, { 12, 44 }, { 13, 48 },
            { 14, 52 }, { 15, 56 }, { 16, 60 }, { 17, 64 }, { 18, 68 }, { 19, 72 },
            { 20, 76 }, { 21, 80 }, { 22, 84 }, { 23, 88 }, { 24, 92 }, { 25, 96 },
            { 26, 1 }, { 27, 5 }, { 28, 9 }, { 29, 13 }, { 30, 17 }, { 31, 21 },
            { 32, 25 }, { 33, 29 }, { 34, 33 }, { 35, 37 }, { 36, 41 }, { 37, 45 },
            { 38, 49 }, { 39, 53 }, { 40, 57 }, { 41, 61 }, { 42, 65 }, { 43, 69 },
            { 44, 73 }, { 45, 77 }, { 46, 81 }, { 47, 85 }, { 48, 89 }, { 49, 93 },
            { 50, 97 }, { 51, 2 }, { 52, 6 }, { 53, 10 }, { 54, 14 }, { 55, 18 },
            { 56, 22 }, { 57, 26 }, { 58, 30 }, { 59, 34 }, { 60, 38 }, { 61, 42 },
            { 62, 46 }, { 63, 50 }, { 64, 54 }, { 65, 58 }, { 66, 62 }, { 67, 66 },
            { 68, 70 }, { 69, 74 }, { 70, 78 }, { 71, 82 }, { 72, 86 }, { 73, 90 },
            { 74, 94 }, { 75, 98 }, { 76, 3 }, { 77, 7 }, { 78, 11 }, { 79, 15 },
            { 80, 19 }, { 81, 23 }, { 82, 27 }, { 83, 31 }, { 84, 35 }, { 85, 39 },
            { 86, 43 }, { 87, 47 }, { 88, 51 }, { 89, 55 }, { 90, 59 }, { 91, 63 },
            { 92, 67 }, { 93, 71 }, { 94, 75 }, { 95, 79 }, { 96, 83 }, { 97, 87 },
            { 98, 91 }, { 99, 95 }, { 100, 99 }
        };

        foreach (string layer in layerList)
        {
            for (int i = 0; i < 108; i++) //searches rows
            {
                string currentLine = GetINIVar(layer, i.ToString(), cmapFile);
                currentLine = currentLine.Substring(0, currentLine.Length - 1);
                string[] tileArray = currentLine.Split('_');

                for (int j = 0; j < 108; j++) //searches columns
                {
                    if (tileArray[j] != "0" && tileArray[j] != "\n" && tileArray[j] != "\r") //not an empty tile
                    {
                        string tileName = null;
                        try
                        {
                            tileName = "tile" + tilesetDict[Convert.ToInt32(tileArray[j])];
                        }
                        catch //sometimes there are weird tiles that just dont exist?? skip them.
                        {
                            continue;
                        }

                        int posX = j;
                        int posY = 108 - i;

                        switch (layer)
                        {
                            case "[Tiles]":
                                groundTileText += tileName + "=" + posX + "," + posY + "\n";
                                break;
                            case "[Vents]":
                                ventTileText += tileName + "=" + posX + "," + posY + "\n";
                                break;
                            case "[Roof]":
                                roofTileText += tileName + "=" + posX + "," + posY + "\n";
                                break;
                            case "[Underground]":
                                undergroundTileText += tileName + "=" + posX + "," + posY + "\n";
                                break;
                        }
                    }
                }
            }
        }
    }
    private void GetObjects()
    {
        groundObjText = "";
        undergroundObjText = "";
        ventObjText = "";
        roofObjText = "";

        Dictionary<int, string> objectDict = new Dictionary<int, string>() //maps the te1 obj ID to the obj name in TER
        {
            { 1, "BedVertical" }, { 2, "Seat" }, { 3, "ToiletRight" }, { 4, "Oven" }, { 5, "Washer" },
            { 6, "Bookshelf" }, { 7, "Treadmill" }, { 8, "Benchpress" }, { 9, "NPCDesk" }, { 10, "Freezer" },
            { 11, "FoodTable" }, { 12, "MedicBed" }, { 13, "Table" }, { 14, "GuardShower" }, { 15, "CutleryTable" },
            { 16, "InmateRollcall" }, { 17, "GuardRollcall" }, { 18, "InmateWaypoint" }, { 19, "GuardWaypoint" }, { 21, "InmateShower" },
            { 22, "GuardCanteen" }, { 23, "InmateCanteen" }, { 24, "CellDoor" }, { 25, "EnteranceDoor" }, { 26, "UtilityDoor" }, { 27, "WorkDoor" },
            { 28, "WorkDoor" }, { 29, "WorkDoor" }, { 30, "StaffDoor" }, { 31, "WorkDoor" }, { 32, "DetectorHorizontal" },
            { 33, "Camera" }, { 34, "Sink" }, { 35, "DirtyLaundry" }, { 36, "CleanLaundry" }, { 37, "WorkDoor" },
            { 38, "WorkDoor" }, { 39, "GuardGym" }, { 40, "Mines" }, { 41, "TimberBox" }, { 42, "FurnitureBox" },
            { 43, "MetalBox" }, { 44, "PlatesBox" }, { 45, "LicensePress" }, { 46, "JobBoard" }, { 47, "JanitorDesk" },
            { 48, "MedicDesk" }, { 49, "Light" }, { 50, "Vent" }, { 51, "SlatsHorizontal" }, { 52, "LadderUp" },
            { 53, "LadderDown" }, { 54, "PlayerBedVertical" }, { 55, "PlayerDesk" }, { 56, "GuardBed" }, { 57, "Sniper" },
            { 59, "SolitaryBed" }, { 60, "SpotLight" }, { 61, "Stash" }, { 62, "CheckpointCharlie" }, { 63, "JeepRight" },
            { 64, "JeepDown" }, { 65, "Generator" }, { 66, "JeepLeft" }, { 67, "JeepUp" }, { 68, "Locker" },
            { 69, "ComputerTable" }, { 71, "VisitorPlayer" }, { 70, "VisitorNPC" }, { 72, "NPCSpawnpoint" }, { 73, "JobWaypoint" },
            { 74, "Payphone" }, { 75, "WhiteDoor" }, { 76, "ZipUp" }, { 77, "ZipEnd" }, { 78, "DeliveryTruckLeft" },
            { 79, "TailorBox" }, { 80, "ClothesBox" }, { 81, "BookBox" }, { 82, "MailBox" }, { 83, "WorkDoor" },
            { 84, "WorkDoor" }, { 85, "WorkDoor" }, { 86, "WorkDoor" }, { 87, "TV" }, { 88, "Lounger" },
            { 89, "YardWorkBox" }, { 90, "RunningMat" }, { 91, "PushupMat" }, { 92, "RedBox" }, { 93, "BlueBox" },
            { 94, "MedicWaypoint" }, { 95, "CharlieGate" }, { 96, "JumpropeMat" }, { 97, "PunchingMat" }, { 98, "SpeedBag" },
            { 99, "ZipDown" }, { 100, "ZipRight" }, { 101, "ZipLeft" }, { 102, "PullupBar" }, { 103, "UtilityDoor" },
            { 104, "DetectorVertical" }, { 105, "ToiletLeft" }, { 106, "ToiletDown" }, { 107, "SlatsVertical" }, { 108, "DeliveryTruckRight" },
            { 109, "BlankDoor" }, { 110, "BedHorizontal" }, { 111, "PlayerBedHorizontal" }
        };

        List<string> objectSet = GetINISet("Objects", cmapFile);

        foreach (string objectStr in objectSet)
        {
            string[] parts = objectStr.Split('=');
            string objectVars = parts[1];
            if(objectVars != "0x0x0x1") //that weird object in the corner of the map
            {
                string[] varParts = objectVars.Split('x');
                int rawPosX = Convert.ToInt32(Math.Round(Convert.ToSingle(varParts[0])));
                int rawPosY = 108 - Convert.ToInt32(Math.Round(Convert.ToSingle(varParts[1])));
                int objID = Convert.ToInt32(varParts[2]);
                int layerID = Convert.ToInt32(varParts[3]);
                float posX;
                float posY;

                string layer = null;
                switch (layerID)
                {
                    case 1:
                        layer = "GroundObjects";
                        break;
                    case 2:
                        layer = "VentObjects";
                        break;
                    case 3:
                        layer = "RoofObjects";
                        break;
                    case 4:
                        layer = "UndergroundObjects";
                        break;
                }

                string objName = objectDict[objID];

                Vector2 objSize = new Vector2(0f, 0f);
                foreach(Transform obj in uic.Find("ObjectsPanel").Find("ObjectsScrollRect").Find("Viewport").Find("Content"))
                {
                    if(obj.name == objName)
                    {
                        objSize = obj.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f);
                    }
                }

                posX = rawPosX + (objSize.x / 2) - .8f;
                posY = rawPosY + (objSize.y / 2) - .8f;

                switch (layer)
                {
                    case "GroundObjects":
                        groundObjText += objName + "=" + posX + "," + posY + "\n";
                        break;
                    case "VentObjects":
                        ventObjText += objName + "=" + posX + "," + posY + "\n";
                        break;
                    case "RoofObjects":
                        roofObjText += objName + "=" + posX + "," + posY + "\n";
                        break;
                    case "UndergroundObjects":
                        undergroundObjText += objName + "=" + posX + "," + posY + "\n";
                        break;
                }
            }
        }
    }
    private void GetRoutine()
    {
        routineText = "";
        
        switch (routineType)
        {
            case "MinSec":
                routineText += "00=LO\n";
                routineText += "01=LO\n";
                routineText += "02=LO\n";
                routineText += "03=LO\n";
                routineText += "04=LO\n";
                routineText += "05=LO\n";
                routineText += "06=LO\n";
                routineText += "07=LO\n";
                routineText += "08=R\n";
                routineText += "09=B\n";
                routineText += "10=FT\n";
                routineText += "11=FT\n";
                routineText += "12=L\n";
                routineText += "13=W\n";
                routineText += "14=W\n";
                routineText += "15=W\n";
                routineText += "16=E\n";
                routineText += "17=S\n";
                routineText += "18=D\n";
                routineText += "19=FT\n";
                routineText += "20=FT\n";
                routineText += "21=FT\n";
                routineText += "22=R\n";
                routineText += "23=LO\n";
                break;
            case "MedSec":
                routineText += "00=LO\n";
                routineText += "01=LO\n";
                routineText += "02=LO\n";
                routineText += "03=LO\n";
                routineText += "04=LO\n";
                routineText += "05=LO\n";
                routineText += "06=LO\n";
                routineText += "07=LO\n";
                routineText += "08=R\n";
                routineText += "09=B\n";
                routineText += "10=W\n";
                routineText += "11=W\n";
                routineText += "12=W\n";
                routineText += "13=R\n";
                routineText += "14=FT\n";
                routineText += "15=FT\n";
                routineText += "16=D\n";
                routineText += "17=E\n";
                routineText += "18=S\n";
                routineText += "19=FT\n";
                routineText += "20=FT\n";
                routineText += "21=FT\n";
                routineText += "22=R\n";
                routineText += "23=LO\n";
                break;
            case "MaxSec":
                routineText += "00=LO\n";
                routineText += "01=LO\n";
                routineText += "02=LO\n";
                routineText += "03=LO\n";
                routineText += "04=LO\n";
                routineText += "05=LO\n";
                routineText += "06=LO\n";
                routineText += "07=LO\n";
                routineText += "08=LO\n";
                routineText += "09=R\n";
                routineText += "10=B\n";
                routineText += "11=W\n";
                routineText += "12=W\n";
                routineText += "13=W\n";
                routineText += "14=R\n";
                routineText += "15=FT\n";
                routineText += "16=E\n";
                routineText += "17=D\n";
                routineText += "18=S\n";
                routineText += "19=R\n";
                routineText += "20=LO\n";
                routineText += "21=LO\n";
                routineText += "22=LO\n";
                routineText += "23=LO\n";
                break;
            case "Camp":
                routineText += "00=LO\n";
                routineText += "01=LO\n";
                routineText += "02=LO\n";
                routineText += "03=LO\n";
                routineText += "04=LO\n";
                routineText += "05=LO\n";
                routineText += "06=LO\n";
                routineText += "07=LO\n";
                routineText += "08=R\n";
                routineText += "09=B\n";
                routineText += "10=W\n";
                routineText += "11=W\n";
                routineText += "12=W\n";
                routineText += "13=E\n";
                routineText += "14=FT\n";
                routineText += "15=FT\n";
                routineText += "16=FT\n";
                routineText += "17=D\n";
                routineText += "18=W\n";
                routineText += "19=W\n";
                routineText += "20=W\n";
                routineText += "21=S\n";
                routineText += "22=R\n";
                routineText += "23=LO\n";
                break;

        }
    }
    private void CombineText()
    {
        convertedText = "";
        convertedText += "[Properties]\n";
        convertedText += "Version=0.0.7\n";
        convertedText += "MapName=" + mapName + "\n";
        convertedText += "Note=" + note + "\n";
        convertedText += "Warden=" + warden + "\n";
        convertedText += "Guards=" + guards + "\n";
        convertedText += "Inmates=" + inmates + "\n";
        convertedText += "Tileset=" + tileset + "\n";
        convertedText += "Ground=" + ground + "\n";
        convertedText += "Music=" + music + "\n";
        convertedText += "Icon=None\n";
        convertedText += "NPCLevel=" + npcLevel + "\n";
        convertedText += "Grounds=" + grounds + "\n";
        convertedText += "Size=108x108\n";
        convertedText += "Hint1=\n";
        convertedText += "Hint2=\n";
        convertedText += "Hint3=\n";
        convertedText += "Snowing=False\n";
        if(routineType == "Camp")
        {
            convertedText += "POWOutfits=True\n";
        }
        else
        {
            convertedText += "POWOutfits=False\n";
        }
        convertedText += "StunRods=False\n";
        convertedText += "\n";
        convertedText += "[Routine]\n";
        convertedText += routineText;
        convertedText += "\n";
        convertedText += "[Jobs]\n";
        convertedText += "StartingJob=" + startingJob + "\n";
        convertedText += "Janitor=" + janitor + "\n";
        convertedText += "Gardening=" + gardening + "\n";
        convertedText += "Laundry=" + laundry + "\n";
        convertedText += "Kitchen=" + kitchen + "\n";
        convertedText += "Tailor=" + tailor + "\n";
        convertedText += "Woodshop=" + woodshop + "\n";
        convertedText += "Metalshop=" + metalshop + "\n";
        convertedText += "Deliveries=" + deliveries + "\n";
        convertedText += "Mailman=" + mailman + "\n";
        convertedText += "Library=" + library + "\n";
        convertedText += "\n";
        convertedText += "[GroundTiles]\n";
        convertedText += groundTileText;
        convertedText += "\n";
        convertedText += "[VentTiles]\n";
        convertedText += ventTileText;
        convertedText += "\n";
        convertedText += "[RoofTiles]\n";
        convertedText += roofTileText;
        convertedText += "\n";
        convertedText += "[UndergroundTiles]\n";
        convertedText += undergroundTileText;
        convertedText += "\n";
        convertedText += "[GroundObjects]\n";
        convertedText += groundObjText;
        convertedText += "\n";
        convertedText += "[VentObjects]\n";
        convertedText += ventObjText;
        convertedText += "\n";
        convertedText += "[RoofObjects]\n";
        convertedText += roofObjText;
        convertedText += "\n";
        convertedText += "[UndergroundObjects]\n";
        convertedText += undergroundObjText;
        convertedText += "\n";
        convertedText += "[Zones]\n";
        convertedText += zoneText;
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
