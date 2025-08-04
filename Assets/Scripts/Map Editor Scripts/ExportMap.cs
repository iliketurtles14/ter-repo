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
        SetTiles();
    }
    private void SetProperties()
    {
        Transform properties = uic.Find("PropertiesPanel");

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
