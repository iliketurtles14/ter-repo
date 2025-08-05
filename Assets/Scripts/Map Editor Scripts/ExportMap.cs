using UnityEngine;
using System.IO;
using TMPro;
using System.Text.RegularExpressions;

public class ExportMap : MonoBehaviour
{
    public Transform tiles;
    private string text;
    public Transform uic;

    public void Export()
    {
        string path = Application.streamingAssetsPath;

        text = "";
        SetProperties();
        SetRoutine();
        SetJobs();
        SetTiles();
    }
    private void SetProperties()
    {
        Transform properties = uic.Find("PropertiesPanel");
        SubMenuController subMenuControllerScript = GetComponent<SubMenuController>();

        text += "[Properties]\n";
        text += "Version=0.0.7\n";
        text += "MapName=" + properties.Find("NameInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Note=" + Regex.Unescape(uic.Find("NotePanel").Find("NoteInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text) + "\n";
        text += "Warden=" + uic.Find("NotePanel").Find("WardenInputField").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Guards=" + properties.Find("GuardsNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Inmates=" + properties.Find("InmatesNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Tileset=" + properties.Find("TilesetResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Ground=" + properties.Find("GroundResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Music=" + properties.Find("MusicResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Icon=" + properties.Find("IconResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "NPCLevel=" + properties.Find("NPCLevelNum").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Grounds=" + properties.Find("GroundsResultText").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Size=" + properties.Find("SizeX").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "," + properties.Find("SizeY").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text + "\n";
        text += "Hint1=" + Regex.Unescape(uic.Find("HintPanel").Find("Hint1Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text) + "\n";
        text += "Hint2=" + Regex.Unescape(uic.Find("HintPanel").Find("Hint2Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text) + "\n";
        text += "Hint3=" + Regex.Unescape(uic.Find("HintPanel").Find("Hint3Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text) + "\n";
        text += "Snowing=" + subMenuControllerScript.snowing;
        text += "POWOutfits=" + subMenuControllerScript.powOutfits;
        text += "StunRods=" + subMenuControllerScript.stunRods;
    }
    private void SetRoutine()
    {
        Transform routinePanel = uic.Find("RoutinePanel");
        Transform routineGrid1 = routinePanel.Find("RoutineInputGrid1");
        Transform routineGrid2 = routinePanel.Find("RoutineInputGrid2");

        text += "\n";
        text += "[Routine]";
        text += "00:00 - " + routineGrid1.Find("00:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "01:00 - " + routineGrid1.Find("01:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "02:00 - " + routineGrid1.Find("02:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "03:00 - " + routineGrid1.Find("03:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "04:00 - " + routineGrid1.Find("04:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "05:00 - " + routineGrid1.Find("05:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "06:00 - " + routineGrid1.Find("06:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "07:00 - " + routineGrid1.Find("07:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "08:00 - " + routineGrid1.Find("08:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "09:00 - " + routineGrid1.Find("09:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "10:00 - " + routineGrid1.Find("10:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "11:00 - " + routineGrid1.Find("11:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "12:00 - " + routineGrid2.Find("12:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "13:00 - " + routineGrid2.Find("13:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "14:00 - " + routineGrid2.Find("14:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "15:00 - " + routineGrid2.Find("15:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "16:00 - " + routineGrid2.Find("16:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "17:00 - " + routineGrid2.Find("17:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "18:00 - " + routineGrid2.Find("18:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "19:00 - " + routineGrid2.Find("19:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "20:00 - " + routineGrid2.Find("20:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "21:00 - " + routineGrid2.Find("21:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "22:00 - " + routineGrid2.Find("22:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "23:00 - " + routineGrid2.Find("23:00Input").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
    }
    private void SetJobs()
    {
        Transform jobPanel = uic.Find("JobPanel");
        SubMenuController subMenuControllerScript = GetComponent<SubMenuController>();

        text += "\n";
        text += "[Jobs]";
        text += "StartingJob=" + jobPanel.Find("StartingJobInput").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        text += "Janitor=" + subMenuControllerScript.janitor;
        text += "Gardening=" + subMenuControllerScript.gardening;
        text += "Laundry=" + subMenuControllerScript.laundry;
        text += "Kitchen=" + subMenuControllerScript.kitchen;
        text += "Tailor=" + subMenuControllerScript.tailor;
        text += "Woodshop=" + subMenuControllerScript.woodshop;
        text += "Metalshop=" + subMenuControllerScript.metalshop;
        text += "Deliveries=" + subMenuControllerScript.deliveries;
        text += "Mailman=" + subMenuControllerScript.mailman;
        text += "Library=" + subMenuControllerScript.library;
    }
    private void SetTiles()
    {
        text += "\n";
        
        text += "[GroundTiles]\n";
        foreach (Transform tile in tiles.Find("Ground"))
        {
            text += tile.name + " | " + (tile.position.x + .8f) / 1.6f + "," + (tile.position.y + .8f) / 1.6f + "\n";
        }
        text += "\n";
        text += "[VentTiles]\n";
        foreach (Transform tile in tiles.Find("Vent"))
        {
            text += tile.name + " | " + (tile.position.x + .8f) / 1.6f + "," + (tile.position.y + .8f) / 1.6f + "\n";
        }
        text += "\n";
        text += "[RoofTiles]\n";
        foreach (Transform tile in tiles.Find("Roof"))
        {
            text += tile.name + " | " + (tile.position.x + .8f) / 1.6f + "," + (tile.position.y + .8f) / 1.6f + "\n";
        }
        text += "\n";
        text += "[UndergroundTiles]\n";
        foreach (Transform tile in tiles.Find("Underground"))
        {
            text += tile.name + " | " + (tile.position.x + .8f) / 1.6f + "," + (tile.position.y + .8f) / 1.6f + "\n";
        }
        text += "\n";
        text += "[GroundObjects]\n";
        foreach (Transform tile in tiles.Find("GroundObjects"))
        {
            text += tile.name + " | " + (tile.position.x + .8f) / 1.6f + "," + (tile.position.y + .8f) / 1.6f + "\n";
        }
        text += "\n";
        text += "[VentObjects]\n";
        foreach (Transform tile in tiles.Find("VentObjects"))
        {
            text += tile.name + " | " + (tile.position.x + .8f) / 1.6f + "," + (tile.position.y + .8f) / 1.6f + "\n";
        }
        text += "\n";
        text += "[RoofObjects]\n";
        foreach (Transform tile in tiles.Find("RoofObjects"))
        {
            text += tile.name + " | " + (tile.position.x + .8f) / 1.6f + "," + (tile.position.y + .8f) / 1.6f + "\n";
        }
        text += "\n";
        text += "[UndergroundObjects]\n";
        foreach (Transform tile in tiles.Find("UndergroundObjects"))
        {
            text += tile.name + " | " + (tile.position.x + .8f) / 1.6f + "," + (tile.position.y + .8f) / 1.6f + "\n";
        }
    }
}
