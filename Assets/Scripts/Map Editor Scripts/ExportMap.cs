using UnityEngine;
using System.IO;
using System.IO.Compression;
using TMPro;
using System.Text.RegularExpressions;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering.UI;
using System.Collections;

public class ExportMap : MonoBehaviour
{
    public Transform tiles;
    private string text;
    public Transform uic;

    private Vector2 mapSize;

    public void Export()
    {
        text = "";
        SetProperties();
        SetRoutine();
        SetJobs();
        SetTiles();
        SetZones();
        SetObjectProperties();
        ZipFiles();
    }
    private void SetProperties()
    {
        Transform properties = uic.Find("PropertiesPanel");
        Transform advanced = uic.Find("AdvancedPanel");
        SubMenuController subMenuControllerScript = GetComponent<SubMenuController>();

        text += "[Properties]\n";
        text += "Version=0.0.7\n";
        text += "MapName=" + properties.Find("NameInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Note=" + Regex.Escape(uic.Find("NotePanel").Find("NoteInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text) + "\n";
        text += "Warden=" + uic.Find("NotePanel").Find("WardenInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Guards=" + properties.Find("GuardsNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Inmates=" + properties.Find("InmatesNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Tileset=" + properties.Find("TilesetResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Ground=" + properties.Find("GroundResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Music=" + advanced.Find("MusicResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Speech=" + advanced.Find("SpeechResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Tooltips=" + advanced.Find("TooltipsResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Items=" + advanced.Find("ItemsResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Icon=" + properties.Find("IconResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "NPCLevel=" + properties.Find("NPCLevelNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Grounds=" + properties.Find("GroundsResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Size=" + properties.Find("SizeResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Hint1=" + Regex.Escape(uic.Find("HintPanel").Find("Hint1Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text) + "\n";
        text += "Hint2=" + Regex.Escape(uic.Find("HintPanel").Find("Hint2Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text) + "\n";
        text += "Hint3=" + Regex.Escape(uic.Find("HintPanel").Find("Hint3Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text) + "\n";
        text += "Snowing=" + subMenuControllerScript.snowing + "\n";
        text += "POWOutfits=" + subMenuControllerScript.powOutfits + "\n";
        text += "StunRods=" + subMenuControllerScript.stunRods + "\n";

        //set the mapSize variable for empty tile placing
        string sizeStr = properties.Find("SizeResultText").GetComponent<TextMeshProUGUI>().text;
        string[] sizeParts = sizeStr.Split('x');
        int sizeX = Convert.ToInt32(sizeParts[0]);
        int sizeY = Convert.ToInt32(sizeParts[1]);
        mapSize = new Vector2(sizeX, sizeY);
    }
    private void SetRoutine()
    {
        Transform routinePanel = uic.Find("RoutinePanel");
        Transform routineGrid1 = routinePanel.Find("RoutineInputGrid1");
        Transform routineGrid2 = routinePanel.Find("RoutineInputGrid2");

        text += "\n";
        text += "[Routine]\n";
        for(int i = 0; i < 24; i++)
        {
            string str = i.ToString();
            if(str.Length < 2)
            {
                str = "0" + str;
            }
            if(i < 12)
            {
                text += str + "=" + routineGrid1.Find(str + ":00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
            }
            else
            {
                text += str + "=" + routineGrid2.Find(str + ":00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
            }

        }
    }
    private void SetJobs()
    {
        Transform jobPanel = uic.Find("JobPanel");
        SubMenuController subMenuControllerScript = GetComponent<SubMenuController>();

        text += "\n";
        text += "[Jobs] \n";
        text += "StartingJob=" + jobPanel.Find("StartingJobInput").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Janitor=" + subMenuControllerScript.janitor + "\n";
        text += "Gardening=" + subMenuControllerScript.gardening + "\n";
        text += "Laundry=" + subMenuControllerScript.laundry + "\n";
        text += "Kitchen=" + subMenuControllerScript.kitchen + "\n";
        text += "Tailor=" + subMenuControllerScript.tailor + "\n";
        text += "Woodshop=" + subMenuControllerScript.woodshop + "\n";
        text += "Metalshop=" + subMenuControllerScript.metalshop + "\n";
        text += "Deliveries=" + subMenuControllerScript.deliveries + "\n";
        text += "Mailman=" + subMenuControllerScript.mailman + "\n";
        text += "Library=" + subMenuControllerScript.library + "\n";
    }
    private void SetTiles()
    {
        Debug.Log("Setting tiles...");

        text += "\n";
        
        text += "[GroundTiles]\n";
        foreach (Transform tile in tiles.Find("Ground"))
        {
            if(tile.name != "empty")
            {
                text += tile.name + "=" + Math.Round((tile.position.x + 1.6f) / 1.6f, 1) + "," + Math.Round((tile.position.y + 1.6f) / 1.6f, 1) + "\n";
            }
        }

        //set the empty tiles

        List<Vector2> posList = new List<Vector2>();

        foreach (Transform tile in tiles.Find("Ground"))
        {
            posList.Add(new Vector2(
                (float)Math.Round((tile.position.x + 1.6f) / 1.6f, 1),
                (float)Math.Round((tile.position.y + 1.6f) / 1.6f, 1)
            ));
        }

        Vector2 currentPos;
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                currentPos = new Vector2(x, y);
                if (!posList.Contains(currentPos))
                {
                    text += "tile100=" + currentPos.x + "," + currentPos.y + "\n";
                }
            }
        }

        text += "\n";
        text += "[VentTiles]\n";
        foreach (Transform tile in tiles.Find("Vent"))
        {
            if (tile.name != "empty")
            {
                text += tile.name + "=" + Math.Round((tile.position.x + 1.6f) / 1.6f, 1) + "," + Math.Round((tile.position.y + 1.6f) / 1.6f, 1) + "\n";
            }
        }
        text += "\n";
        text += "[RoofTiles]\n";
        foreach (Transform tile in tiles.Find("Roof"))
        {
            if(tile.name != "empty")
            {
                text += tile.name + "=" + Math.Round((tile.position.x + 1.6f) / 1.6f, 1) + "," + Math.Round((tile.position.y + 1.6f) / 1.6f, 1) + "\n";
            }
        }
        text += "\n";
        text += "[UndergroundTiles]\n";
        foreach (Transform tile in tiles.Find("Underground"))
        {
            if(tile.name != "empty")
            {
                text += tile.name + "=" + Math.Round((tile.position.x + 1.6f) / 1.6f, 1) + "," + Math.Round((tile.position.y + 1.6f) / 1.6f, 1) + "\n";
            }
        }
        text += "\n";
        text += "[GroundObjects]\n";
        foreach (Transform tile in tiles.Find("GroundObjects"))
        {
            if(tile.name != "empty")
            {
                text += tile.name + "=" + ((tile.position.x + 1.6f) / 1.6f) + "," + ((tile.position.y + 1.6f) / 1.6f) + "\n";
            }
        }
        text += "\n";
        text += "[VentObjects]\n";
        foreach (Transform tile in tiles.Find("VentObjects"))
        {
            if(tile.name != "empty")
            {
                text += tile.name + "=" + ((tile.position.x + 1.6f) / 1.6f) + "," + ((tile.position.y + 1.6f) / 1.6f) + "\n";
            }
        }
        text += "\n";
        text += "[RoofObjects]\n";
        foreach (Transform tile in tiles.Find("RoofObjects"))
        {
            if(tile.name != "empty")
            {
                text += tile.name + "=" + ((tile.position.x + 1.6f) / 1.6f) + "," + ((tile.position.y + 1.6f) / 1.6f) + "\n";
            }
        }
        text += "\n";
        text += "[UndergroundObjects]\n";
        foreach (Transform tile in tiles.Find("UndergroundObjects"))
        {
            if(tile.name != "empty")
            {
                text += tile.name + "=" + ((tile.position.x + 1.6f) / 1.6f) + "," + ((tile.position.y + 1.6f) / 1.6f) + "\n";
            }
        }

    }
    private void SetZones()
    {
        text += "\n";
        text += "[Zones]\n";

        Transform zonesLayer = tiles.Find("Zones");
        foreach(Transform zone in zonesLayer)
        {
            if(zone.name != "empty")
            {
                float posX = Convert.ToSingle(Math.Round((zone.position.x + 1.6f) / 1.6f, 1));
                float posY = Convert.ToSingle(Math.Round((zone.position.y + 1.6f) / 1.6f, 1));
                float sizeX = Convert.ToSingle(Math.Round(zone.GetComponent<SpriteRenderer>().size.x / .16f, 1));
                float sizeY = Convert.ToSingle(Math.Round(zone.GetComponent<SpriteRenderer>().size.y / .16f, 1));
                
                text += zone.name + "=" + posX + "," + posY + ";" + sizeX + "x" + sizeY + "\n";
            }
        }
    }
    private void SetObjectProperties()
    {
        text += "\n";
        text += "[GroundObjectProperties]\n";
        foreach (Transform obj in tiles.Find("GroundObjects"))
        {
            float posX = (obj.position.x + 1.6f) / 1.6f;
            float posY = (obj.position.y + 1.6f) / 1.6f;
            switch (obj.name)
            {
                case "Item":
                    int id = obj.GetComponent<MEItemIDContainer>().id;
                    text += "Item=" + posX + "," + posY + ";" + id + "\n";
                    break;
                case "ChristmasDesk":
                case "DTAFSpecialDesk":
                case "ETSpecialDesk":
                    text += obj.name + "=" + posX + "," + posY + ";";
                    foreach (int num in obj.GetComponent<MEDeskListContainer>().ids)
                    {
                        text += num + ",";
                    }
                    text = text.Substring(0, text.Length - 1); //remove last comma
                    text += "\n";
                    break;
                case "DTAFSign":
                case "SSSign":
                case "DTAFPlaque":
                    text += obj.name + "=" + posX + "," + posY + ";";
                    string header = obj.GetComponent<MESignTextContainer>().header;
                    string body = obj.GetComponent<MESignTextContainer>().body;
                    text += "{HEADER}:" + Regex.Escape(header) + "{BODY}:" + Regex.Escape(body) + "\n";
                    break;
            }
        }
        text += "\n";
        text += "[UndergroundObjectProperties]\n";
        foreach (Transform obj in tiles.Find("UndergroundObjects"))
        {
            float posX = (obj.position.x + 1.6f) / 1.6f;
            float posY = (obj.position.y + 1.6f) / 1.6f;
            switch (obj.name)
            {
                case "Item":
                    int id = obj.GetComponent<MEItemIDContainer>().id;
                    text += "Item=" + posX + "," + posY + ";" + id + "\n";
                    break;
                case "ChristmasDesk":
                case "DTAFSpecialDesk":
                case "ETSpecialDesk":
                    text += obj.name + "=" + posX + "," + posY + ";";
                    foreach (int num in obj.GetComponent<MEDeskListContainer>().ids)
                    {
                        text += num + ",";
                    }
                    text = text.Substring(0, text.Length - 1); //remove last comma
                    text += "\n";
                    break;
                case "DTAFSign":
                case "SSSign":
                case "DTAFPlaque":
                    text += obj.name + "=" + posX + "," + posY + ";";
                    string header = obj.GetComponent<MESignTextContainer>().header;
                    string body = obj.GetComponent<MESignTextContainer>().body;
                    text += "{HEADER}:" + Regex.Escape(header) + "{BODY}:" + Regex.Escape(body) + "\n";
                    break;
            }
        }
        text += "\n";
        text += "[VentObjectProperties]\n";
        foreach (Transform obj in tiles.Find("VentObjects"))
        {
            float posX = (obj.position.x + 1.6f) / 1.6f;
            float posY = (obj.position.y + 1.6f) / 1.6f;
            switch (obj.name)
            {
                case "Item":
                    int id = obj.GetComponent<MEItemIDContainer>().id;
                    text += "Item=" + posX + "," + posY + ";" + id + "\n";
                    break;
                case "ChristmasDesk":
                case "DTAFSpecialDesk":
                case "ETSpecialDesk":
                    text += obj.name + "=" + posX + "," + posY + ";";
                    foreach (int num in obj.GetComponent<MEDeskListContainer>().ids)
                    {
                        text += num + ",";
                    }
                    text = text.Substring(0, text.Length - 1); //remove last comma
                    text += "\n";
                    break;
                case "DTAFSign":
                case "SSSign":
                case "DTAFPlaque":
                    text += obj.name + "=" + posX + "," + posY + ";";
                    string header = obj.GetComponent<MESignTextContainer>().header;
                    string body = obj.GetComponent<MESignTextContainer>().body;
                    text += "{HEADER}:" + Regex.Escape(header) + "{BODY}:" + Regex.Escape(body) + "\n";
                    break;
            }
        }
        text += "\n";
        text += "[RoofObjectProperties]\n";
        foreach (Transform obj in tiles.Find("RoofObjects"))
        {
            float posX = (obj.position.x + 1.6f) / 1.6f;
            float posY = (obj.position.y + 1.6f) / 1.6f;
            switch (obj.name)
            {
                case "Item":
                    int id = obj.GetComponent<MEItemIDContainer>().id;
                    text += "Item=" + posX + "," + posY + ";" + id + "\n";
                    break;
                case "ChristmasDesk":
                case "DTAFSpecialDesk":
                case "ETSpecialDesk":
                    text += obj.name + "=" + posX + "," + posY + ";";
                    foreach (int num in obj.GetComponent<MEDeskListContainer>().ids)
                    {
                        text += num + ",";
                    }
                    text = text.Substring(0, text.Length - 1); //remove last comma
                    text += "\n";
                    break;
                case "DTAFSign":
                case "SSSign":
                case "DTAFPlaque":
                    text += obj.name + "=" + posX + "," + posY + ";";
                    string header = obj.GetComponent<MESignTextContainer>().header;
                    string body = obj.GetComponent<MESignTextContainer>().body;
                    text += "{HEADER}:" + Regex.Escape(header) + "{BODY}:" + Regex.Escape(body) + "\n";
                    break;
            }
        }
    }
    private void ZipFiles()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Prisons", "CustomPrisons") + Path.DirectorySeparatorChar;

        SubMenuController subMenuControllerScript = GetComponent<SubMenuController>();

        string[] tilesPath = subMenuControllerScript.tilesPath;
        string[] groundPath = subMenuControllerScript.groundPath;
        string[] musicPath = subMenuControllerScript.musicPath;
        string[] iconPath = subMenuControllerScript.iconPath;

        List<string> filesToZip = new List<string>();

        File.WriteAllText(path + "Data.ini", text); //make Data.ini file
        filesToZip.Add(path + "Data.ini");

        //copy files to location temporarily
        try
        {
            File.Copy(tilesPath[0], path + "Tiles.png", true);
            filesToZip.Add(path + "Tiles.png");
        }
        catch { }
        try
        {
            File.Copy(groundPath[0], path + "Ground.png", true);
            filesToZip.Add(path + "Ground.png");
        }
        catch { }
        try
        {
            File.Copy(musicPath[0], path + "Music.mp3", true);
            filesToZip.Add(path + "Music.mp3");
        }
        catch { }
        try
        {
            File.Copy(iconPath[0], path + "Icon.png", true);
            filesToZip.Add(path + "Icon.png");
        }
        catch { }

        //zip files
        string mapName = uic.Find("PropertiesPanel").Find("NameInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;

        using (FileStream zipToOpen = new FileStream(path + mapName + ".zip", FileMode.Create))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
            {
                foreach(string filePath in filesToZip)
                {
                    archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                }
            }
        }

        //delete others
        if(File.Exists(path + "Tiles.png"))
        {
            File.Delete(path + "Tiles.png");
        }
        if (File.Exists(path + "Ground.png"))
        {
            File.Delete(path + "Ground.png");
        }
        if (File.Exists(path + "Music.mp3"))
        {
            File.Delete(path + "Music.mp3");
        }
        if (File.Exists(path + "Icon.png"))
        {
            File.Delete(path + "Icon.png");
        }
        File.Delete(path + "Data.ini");

        //rename zip to zmap
        string newPath = Path.ChangeExtension(path + mapName + ".zip", ".zmap");

        int num = 1;
        try
        {
            File.Move(path + mapName + ".zip", newPath);
        }
        catch
        {
            while (true)
            {
                try
                {
                    File.Move(path + mapName + " " + num.ToString() + ".zip", newPath);
                    break;
                }
                catch
                {
                    num++;
                }
            }
        }

    }
}
