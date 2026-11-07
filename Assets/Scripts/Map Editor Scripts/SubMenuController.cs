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
    public bool manure;
    public bool taskmaster;
    public bool toymaker;
    public bool letter;
    public bool snowing;
    public bool ssVisitors;
    public bool stunRods;
    public bool perksMsgs;
    public bool tutorial;
    public bool customTileAttributes;
    //IF YOU MAKE A NEW CHECKBOX, PLEASE ADD IT TO THE RenewCheckboxes() FUNCTION BELOW!!!!
    public Dictionary<string, string> prisonDict = new Dictionary<string, string>()
    {
        { "Center Perks", "perks" }, { "Stalag Flucht", "stalag" }, { "Shankton State Pen", "shankton" },
        { "Jungle Compound", "jungle" }, { "San Pancho", "sanpancho" }, { "HMP Irongate", "irongate" },
        { "Jingle Cells", "JC" }, { "Banned Camp", "BC" }, { "London Tower", "london" },
        { "Paris Central Pen", "PCP" }, { "Santa's Sweatshop", "SS" }, { "Duct Tapes are Forever", "DTAF" },
        { "Escape Team", "ET" }, { "Alcatraz", "alca" }, { "Fhurst Peak Correctional", "fhurst" },
        { "Camp Epsilon", "epsilon" }, { "Fort Bamford", "bamford" }
    };
    public void RenewCheckboxes() //this not only puts the right sprite for the checkbox, but it also activates anything it needs to when true or false
    {
        //if a certain checkbox uses a different sprite than that in the dict, specify later.
        Dictionary<bool, Sprite> checkDict = new Dictionary<bool, Sprite>
        {
            { true, checkedBoxSprite }, { false, uncheckedBoxSprite }
        };
        Transform jobPanel = uic.Find("JobPanel");
        Transform jobGrid1 = jobPanel.Find("CheckBoxGrid1");
        Transform jobGrid2 = jobPanel.Find("CheckBoxGrid2");
        jobGrid1.Find("JanitorCheckbox").GetComponent<Image>().sprite = checkDict[janitor];
        jobGrid1.Find("GardeningCheckbox").GetComponent<Image>().sprite = checkDict[gardening];
        jobGrid1.Find("LaundryCheckbox").GetComponent<Image>().sprite = checkDict[laundry];
        jobGrid1.Find("KitchenCheckbox").GetComponent<Image>().sprite = checkDict[kitchen];
        jobGrid1.Find("TailorCheckbox").GetComponent<Image>().sprite = checkDict[tailor];
        jobGrid1.Find("ManureControlCheckbox").GetComponent<Image>().sprite = checkDict[manure];
        jobGrid1.Find("ToymakerCheckbox").GetComponent<Image>().sprite = checkDict[toymaker];
        jobGrid2.Find("WoodshopCheckbox").GetComponent<Image>().sprite = checkDict[woodshop];
        jobGrid2.Find("MetalshopCheckbox").GetComponent<Image>().sprite = checkDict[metalshop];
        jobGrid2.Find("DeliveriesCheckbox").GetComponent<Image>().sprite = checkDict[deliveries];
        jobGrid2.Find("MailmanCheckbox").GetComponent<Image>().sprite = checkDict[mailman];
        jobGrid2.Find("LibraryCheckbox").GetComponent<Image>().sprite = checkDict[library];
        jobGrid2.Find("TaskmasterCheckbox").GetComponent<Image>().sprite = checkDict[taskmaster];
        jobGrid2.Find("LetterOrganizationCheckbox").GetComponent<Image>().sprite = checkDict[letter];

        Transform extrasPanel = uic.Find("ExtrasPanel");
        extrasPanel.Find("SSVisitorsCheckbox").GetComponent<Image>().sprite = checkDict[ssVisitors];
        extrasPanel.Find("SnowingCheckbox").GetComponent<Image>().sprite = checkDict[snowing];
        extrasPanel.Find("StunRodCheckbox").GetComponent<Image>().sprite = checkDict[stunRods];
        extrasPanel.Find("PerksMessagesCheckbox").GetComponent<Image>().sprite = checkDict[perksMsgs];
        extrasPanel.Find("TutorialCheckbox").GetComponent<Image>().sprite = checkDict[tutorial];

        Transform tilesPanel = uic.Find("TilesPanel");
        if (customTileAttributes)
        {
            tilesPanel.Find("CustomCheckbox").GetComponent<Image>().sprite = RemoveBottomTwoRows(checkedBoxSprite);
            tilesPanel.Find("EditButton").gameObject.SetActive(true);
        }
        else
        {
            tilesPanel.Find("CustomCheckbox").GetComponent<Image>().sprite = RemoveBottomTwoRows(uncheckedBoxSprite);
            tilesPanel.Find("EditButton").gameObject.SetActive(false);
        }
    }
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
    private Sprite RemoveBottomTwoRows(Sprite original) //ts ai
    {
        Texture2D texture = original.texture;

        // Get the sprite's pixels (accounting for its position in the texture atlas)
        Rect rect = original.textureRect;
        Color[] pixels = texture.GetPixels(
            (int)rect.x,
            (int)rect.y,
            (int)rect.width,
            (int)rect.height
        );

        int width = (int)rect.width;
        int height = (int)rect.height;

        // Make the bottom two rows transparent
        for (int y = 0; y < 2 && y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[y * width + x].a = 0f;
            }
        }

        // Create a new texture and sprite
        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        newTexture.filterMode = FilterMode.Point;
        newTexture.SetPixels(pixels);
        newTexture.Apply();

        return Sprite.Create(
            newTexture,
            new Rect(0, 0, width, height),
            original.pivot / new Vector2(width, height),
            original.pixelsPerUnit
        );
    }
}
