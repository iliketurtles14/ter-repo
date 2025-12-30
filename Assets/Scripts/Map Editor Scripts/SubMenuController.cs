using SFB;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubMenuController : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform uic;
    public Sprite uncheckedBoxSprite;
    public Sprite checkedBoxSprite;
    public RuntimeGrid gridScript;
    public GroundSizeSet groundSizeSetScript;
    public Transform canvases;

    public string[] tilesPath = null;
    public string[] groundPath = null;
    public string[] musicPath = null;
    public string[] iconPath = null;

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
    public bool snowing;
    public bool powOutfits;
    public bool stunRods;

    public Dictionary<string, string> prisonDict = new Dictionary<string, string>()
    {
        { "Center Perks", "perks" }, { "Stalag Flucht", "stalag" }, { "Shankton State Pen", "shankton" },
        { "Jungle Compound", "jungle" }, { "San Pancho", "sanpancho" }, { "HMP Irongate", "irongate" },
        { "Jingle Cells", "JC" }, { "Banned Camp", "BC" }, { "London Tower", "london" },
        { "Paris Central Pen", "PCP" }, { "Santa's Sweatshop", "SS" }, { "Duct Tapes are Forever", "DTAF" },
        { "Escape Team", "ET" }, { "Alcatraz", "alca" }, { "Fhurst Peak Correctional", "fhurst" },
        { "Camp Epsilon", "epsilon" }, { "Fort Bamford", "bamford" }
    };

    public void ReactivateButtons()
    {
        Transform propertiesPanel = uic.Find("PropertiesPanel");

        propertiesPanel.Find("WardenNoteButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("WardenNoteButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("RoutineButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("RoutineButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("HintButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("HintButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("JobButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("JobButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("ExtrasButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("ExtrasButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("TilesetButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("TilesetButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("GroundButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("GroundButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("IconButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("IconButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("GroundsButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("GroundsButton").GetComponent<EventTrigger>().enabled = true;
        propertiesPanel.Find("GuardsNum").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("InmatesNum").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("NPCLevelNum").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("NameInputField").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("SizeButton").GetComponent<Button>().enabled = true;
        propertiesPanel.Find("SizeButton").GetComponent<EventTrigger>().enabled = true;

        uic.Find("FileButton").GetComponent<Button>().enabled = true;
        uic.Find("FileButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("TilesButton").GetComponent<Button>().enabled = true;
        uic.Find("TilesButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("ObjectsButton").GetComponent<Button>().enabled = true;
        uic.Find("ObjectsButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("PropertiesButton").GetComponent<Button>().enabled = true;
        uic.Find("PropertiesButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("GroundButton").GetComponent<Button>().enabled = true;
        uic.Find("GroundButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("UndergroundButton").GetComponent<Button>().enabled = true;
        uic.Find("UndergroundButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("VentsButton").GetComponent<Button>().enabled = true;
        uic.Find("VentsButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("RoofButton").GetComponent<Button>().enabled = true;
        uic.Find("RoofButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("ZoneObjectsButton").GetComponent<Button>().enabled = true;
        uic.Find("ZoneObjectsButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("AdvancedButton").GetComponent<Button>().enabled = true;
        uic.Find("AdvancedButton").GetComponent<EventTrigger>().enabled = true;
        uic.Find("OriginButton").GetComponent<Button>().enabled = true;
        uic.Find("OriginButton").GetComponent<EventTrigger>().enabled = true;
        canvases.gameObject.SetActive(true);
    }

}
