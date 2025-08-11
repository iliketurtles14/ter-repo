using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System;
using System.Text.RegularExpressions;

public class SubMenuController : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform uic;
    public Sprite uncheckedBoxSprite;
    public Sprite checkedBoxSprite;
    public RuntimeGrid gridScript;
    public GroundSizeSet groundSizeSetScript;

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

    private void Update()
    {
        if (mcs.isTouchingButton && Input.GetMouseButtonDown(0))
        {
            if (mcs.touchedButton.name == "ContinueButton" && mcs.touchedButton.transform.parent.name == "NotePanel")
            {
                uic.Find("NotePanel").gameObject.SetActive(false);
                uic.Find("Black").gameObject.SetActive(false);
                ReactivateButtons();
            }
            else if (mcs.touchedButton.name == "CloseButton" && mcs.touchedButton.transform.parent.name == "RoutinePanel")
            {
                uic.Find("RoutinePanel").gameObject.SetActive(false);
                uic.Find("Black").gameObject.SetActive(false);
                ReactivateButtons();
            }
            else if (mcs.touchedButton.name == "CloseButton" && mcs.touchedButton.transform.parent.name == "HintPanel")
            {
                uic.Find("HintPanel").gameObject.SetActive(false);
                uic.Find("Black").gameObject.SetActive(false);
                ReactivateButtons();
            }
            else if (mcs.touchedButton.name.EndsWith("Checkbox") && mcs.touchedButton.GetComponent<Image>().sprite == checkedBoxSprite)
            {
                mcs.touchedButton.GetComponent<Image>().sprite = uncheckedBoxSprite;

                switch (mcs.touchedButton.name)
                {
                    case "JanitorCheckbox":
                        janitor = false;
                        break;
                    case "GardeningCheckbox":
                        gardening = false;
                        break;
                    case "LaundryCheckbox":
                        laundry = false;
                        break;
                    case "KitchenCheckbox":
                        kitchen = false;
                        break;
                    case "TailorCheckbox":
                        tailor = false;
                        break;
                    case "WoodshopCheckbox":
                        woodshop = false;
                        break;
                    case "MetalshopCheckbox":
                        metalshop = false;
                        break;
                    case "DeliveriesCheckbox":
                        deliveries = false;
                        break;
                    case "MailmanCheckbox":
                        mailman = false;
                        break;
                    case "LibraryCheckbox":
                        library = false;
                        break;
                    case "SnowingCheckbox":
                        snowing = false;
                        break;
                    case "POWCheckbox":
                        powOutfits = false;
                        break;
                    case "StunRodCheckbox":
                        stunRods = false;
                        break;
                }
            }
            else if (mcs.touchedButton.name.EndsWith("Checkbox") && mcs.touchedButton.GetComponent<Image>().sprite == uncheckedBoxSprite)
            {
                mcs.touchedButton.GetComponent<Image>().sprite = checkedBoxSprite;

                switch (mcs.touchedButton.name)
                {
                    case "JanitorCheckbox":
                        janitor = true;
                        break;
                    case "GardeningCheckbox":
                        gardening = true;
                        break;
                    case "LaundryCheckbox":
                        laundry = true;
                        break;
                    case "KitchenCheckbox":
                        kitchen = true;
                        break;
                    case "TailorCheckbox":
                        tailor = true;
                        break;
                    case "WoodshopCheckbox":
                        woodshop = true;
                        break;
                    case "MetalshopCheckbox":
                        metalshop = true;
                        break;
                    case "DeliveriesCheckbox":
                        deliveries = true;
                        break;
                    case "MailmanCheckbox":
                        mailman = true;
                        break;
                    case "LibraryCheckbox":
                        library = true;
                        break;
                    case "SnowingCheckbox":
                        snowing = true;
                        break;
                    case "POWCheckbox":
                        powOutfits = true;
                        break;
                    case "StunRodCheckbox":
                        stunRods = true;
                        break;
                }
            }
            else if(mcs.touchedButton.name == "CloseButton" && mcs.touchedButton.transform.parent.name == "JobPanel")
            {
                uic.Find("JobPanel").gameObject.SetActive(false);
                uic.Find("Black").gameObject.SetActive(false);
                ReactivateButtons();
            }
            else if(mcs.touchedButton.name == "CloseButton" && mcs.touchedButton.transform.parent.name == "ExtrasPanel")
            {
                uic.Find("ExtrasPanel").gameObject.SetActive(false);
                uic.Find("Black").gameObject.SetActive(false);
                ReactivateButtons();
            }
            else if(mcs.touchedButton.transform.parent.parent.name == "PrisonSelectMenu")
            {
                if(mcs.touchedButton.name != "CustomButton") //actual prison
                {
                    string whatPrison = "";

                    foreach(Transform child in uic.Find("PrisonSelectMenu").Find("NameGrid")) //get the name of the prison you selected
                    {
                        if(child.position == mcs.touchedButton.transform.position)
                        {
                            whatPrison = child.GetComponent<TextMeshProUGUI>().text;
                            Debug.Log(whatPrison);
                        }
                    }

                    switch (GetComponent<PropertiesController>().buttonType) //make changes to result texts and close menu
                    {                        
                        case "tileset":
                            uic.Find("PropertiesPanel").Find("TilesetResultText").GetComponent<TextMeshProUGUI>().text = prisonDict[whatPrison];
                            tilesPath = null;
                            break;
                        case "ground":
                            uic.Find("PropertiesPanel").Find("GroundResultText").GetComponent<TextMeshProUGUI>().text = prisonDict[whatPrison];
                            groundPath = null;
                            break;
                        case "music":
                            uic.Find("PropertiesPanel").Find("MusicResultText").GetComponent<TextMeshProUGUI>().text = prisonDict[whatPrison];
                            musicPath = null;
                            break;
                    }

                    uic.Find("PrisonSelectMenu").gameObject.SetActive(false);
                    uic.Find("Black").gameObject.SetActive(false);
                    ReactivateButtons();
                }
                else if(mcs.touchedButton.name == "CustomButton") //custom stuff
                {
                    ExtensionFilter[] extensions = null;
                    string title = "";
                    string resultTextField = "";

                    switch (GetComponent<PropertiesController>().buttonType) //open file dialog to select the custom file
                    {
                        case "tileset":
                            extensions = new []
                            {
                                new ExtensionFilter("Image Files", "png")
                            };
                            title = "Select a custom tileset.";
                            resultTextField = "TilesetResultText";

                            tilesPath = StandaloneFileBrowser.OpenFilePanel(title, "", extensions, false);

                            break;
                        case "ground":
                            extensions = new []
                            {
                                new ExtensionFilter("Image Files", "png")
                            };
                            title = "Select a custom ground.";
                            resultTextField = "GroundResultText";

                            groundPath = StandaloneFileBrowser.OpenFilePanel(title, "", extensions, false);

                            break;
                        case "music":
                            extensions = new []
                            {
                                new ExtensionFilter("Sound Files", "mp3")
                            };
                            title = "Select custom music.";
                            resultTextField = "MusicResultText";

                            musicPath = StandaloneFileBrowser.OpenFilePanel(title, "", extensions, false);

                            break;
                    }
                    uic.Find("PropertiesPanel").Find(resultTextField).GetComponent<TextMeshProUGUI>().text = "Custom";
                    uic.Find("PrisonSelectMenu").gameObject.SetActive(false);
                    uic.Find("Black").gameObject.SetActive(false);
                    ReactivateButtons();
                }
            }
            else if(mcs.touchedButton.transform.parent.name == "GroundsSelectMenu")
            {
                switch (mcs.touchedButton.name)
                {
                    case "InsideButton":
                        uic.Find("PropertiesPanel").Find("GroundsResultText").GetComponent<TextMeshProUGUI>().text = "Inside";
                        break;
                    case "InsideOutsideButton":
                        uic.Find("PropertiesPanel").Find("GroundsResultText").GetComponent<TextMeshProUGUI>().text = "In/Out";
                        break;
                    case "OutsideButton":
                        uic.Find("PropertiesPanel").Find("GroundsResultText").GetComponent<TextMeshProUGUI>().text = "Outside";
                        break;
                }
                uic.Find("GroundsSelectMenu").gameObject.SetActive(false);
                uic.Find("Black").gameObject.SetActive(false);
                ReactivateButtons();
            }
            else if(mcs.touchedButton.name == "IconButton")
            {
                ExtensionFilter[] extensions = new ExtensionFilter[]
                {
                    new ExtensionFilter("Image Files", "png")
                };
                iconPath = StandaloneFileBrowser.OpenFilePanel("Select a custom icon.", "", extensions, false);

                uic.Find("PropertiesPanel").Find("IconResultText").GetComponent<TextMeshProUGUI>().text = "Custom";
            }
            else if(mcs.touchedButton.name == "IconCancel")
            {
                iconPath = null;
                
                uic.Find("PropertiesPanel").Find("IconResultText").GetComponent<TextMeshProUGUI>().text = "None";
            }
            else if(mcs.touchedButton.name == "ExportButton")
            {
                ExportMap exportScript = GetComponent<ExportMap>();
                exportScript.Export();
            }
            else if(mcs.touchedButton.name == "LoadButton")
            {
                GetComponent<LoadMap>().StartLoad();
            }
            else if(mcs.touchedButton.name == "ConvertButton")
            {
                GetComponent<CmapConvert>().ConvertCmap();
            }
            else if(mcs.touchedButton.name == "SetButton" && mcs.touchedButton.transform.parent.name == "SizePanel")
            {
                Transform sizePanel = uic.Find("SizePanel");
                string rawX = sizePanel.Find("SizeX").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
                string rawY = sizePanel.Find("SizeY").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;

                rawX = Regex.Replace(rawX, @"[^\d]", "");
                rawY = Regex.Replace(rawY, @"[^\d]", "");

                int x = Convert.ToInt32(rawX);
                int y = Convert.ToInt32(rawY);
            
                if(x >= 25 && x <= 150 && y >= 25 && x <= 150) //check to see if the map will be too small or big
                {
                    gridScript.DrawGrid(x, y);
                    groundSizeSetScript.SetSize();
                    uic.Find("PropertiesPanel").Find("SizeResultText").GetComponent<TextMeshProUGUI>().text = x + "x" + y;
                    uic.Find("Black").gameObject.SetActive(false);
                    uic.Find("SizePanel").gameObject.SetActive(false);
                    ReactivateButtons();
                }
                else
                {
                    Debug.Log("Map size is too big or too small.");
                }
            }
            else if(mcs.touchedButton.name == "CancelButton" && mcs.touchedButton.transform.parent.name == "SizePanel")
            {
                uic.Find("Black").gameObject.SetActive(false);
                uic.Find("SizePanel").gameObject.SetActive(false);
                ReactivateButtons();
            }
        }
    }
    private void ReactivateButtons()
    {
        Transform propertiesPanel = uic.Find("PropertiesPanel");

        propertiesPanel.Find("WardenNoteButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("RoutineButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("HintButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("JobButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("ExtrasButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("TilesetButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("GroundButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("MusicButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("IconButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("GroundsButton").GetComponent<BoxCollider2D>().enabled = true;
        propertiesPanel.Find("GuardsNum").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("InmatesNum").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("NPCLevelNum").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("NameInputField").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("SizeButton").GetComponent<BoxCollider2D>().enabled = true;

        uic.Find("FileButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("TilesButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("ObjectsButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("PropertiesButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("GroundButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("UndergroundButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("VentsButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("RoofButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("ZoneObjectsButton").GetComponent<BoxCollider2D>().enabled = true;
    }
    
}
