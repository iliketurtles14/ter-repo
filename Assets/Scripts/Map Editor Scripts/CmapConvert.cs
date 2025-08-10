using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

public class CmapConvert : MonoBehaviour
{
    private string[] cmapFile;
    private string convertedText;
    public string[] ConvertCmap()
    {
        ExtensionFilter[] extensions = new ExtensionFilter[]
        {
            new ExtensionFilter("Cmap Files", "cmap")
        };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a cmap file.", "", extensions, false);
        if(paths.Length < 0)
        {
            return null;
        }

        cmapFile = File.ReadAllLines(paths[0]);

        GetProperties();
        GetZones();

    }
    private void GetProperties()
    {
        string mapName = GetINIVar("Info", "MapName", cmapFile);
        string note = Regex.Escape(GetINIVar("Info", "Intro", cmapFile).Replace("#", "\n"));
        string warden = GetINIVar("Info", "Warden", cmapFile);
        int guards = Convert.ToInt32(GetINIVar("Info", "Guards", cmapFile).Replace("\n", "").Replace("\r", ""));
        int inmates = Convert.ToInt32(GetINIVar("Info", "Inmates", cmapFile).Replace("\n", "").Replace("\r", ""));
        string tileset = GetINIVar("Info", "Tileset", cmapFile);
        string ground = GetINIVar("Info", "Floor", cmapFile);
        string music = GetINIVar("Info", "Music", cmapFile);
        string grounds = GetINIVar("Info", "MapType", cmapFile);
        if (grounds == "InsideOutside")
        {
            grounds = "In/Out";
        }
        string routineType = GetINIVar("Info", "RoutineSet", cmapFile);
        int npcLevel = Convert.ToInt32(GetINIVar("Info", "NPClvl", cmapFile).Replace("\n", "").Replace("\r", ""));

        bool laundry = false;
        bool gardening = false;
        bool janitor = false;
        bool woodshop = false;
        bool metalshop = false;
        bool kitchen = false;
        bool deliveries = false;
        bool tailor = false;
        bool mailman = false;
        bool library = false;

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

        string startingJob = GetINIVar("Jobs", "StartingJob", cmapFile);
    }
    private void GetZones()
    {
        List<string> zoneSet = GetINISet("Zones", cmapFile);
        string zoneText = "";
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
                //get pos and size
                int rawPosX = Convert.ToInt32(varParts[0]);
                int rawPosY = Convert.ToInt32(varParts[1]);
                int rawSizeX = Convert.ToInt32(varParts[2]);
                int rawSizeY = Convert.ToInt32(varParts[3]);
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
                for (int j = i; j < file.Length - i; j++)
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

        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');

        return parts[1];
    }
}
