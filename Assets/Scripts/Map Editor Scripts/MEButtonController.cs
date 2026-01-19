using Ookii.Dialogs;
using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MEButtonController : MonoBehaviour
{
    public Transform uic;
    public Transform tiles;
    public Transform camera;
    private PropertiesController pc;
    private SubMenuController smc;
    private PanelSelect panelSelectScript;
    private LayerController layerControllerScript;
    public RuntimeGrid gridScript;
    public string speechPath;
    public string tooltipsPath;
    public string musicPath;
    public string itemsPath;
    public Transform canvases;
    //private Dictionary<string, int> tilesetDict = new Dictionary<string, int>()
    //{
    //    { "alca", 0 }, { "BC", 1 }, { "campepsilon", 2 }, { "CCL", 3 },
    //    { "DTAF", 4 }, { "EA", 5 }, { "escapeteam", 6 }, { "fortbamford", 7 },
    //    { "irongate", 12 }, { "jungle", 13 }, { "pcpen", 8 }, { "perks", 14 },
    //    { "sanpancho", 15 }, { "shanktonstatepen", 16 }, { "SS", 9 }, { "stalagflucht", 17 },
    //    { "TOL", 10 }, { "tutorial", 11 }
    //};
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
    private void Start()
    {
        pc = GetComponent<PropertiesController>();
        smc = GetComponent<SubMenuController>();
        panelSelectScript = GetComponent<PanelSelect>();
        layerControllerScript = GetComponent<LayerController>();
    }
    public void FileLoad()
    {
        GetComponent<LoadMap>().StartLoad(false);
    }
    public void FileExport()
    {
        GetComponent<ExportMap>().Export();
    }
    public void FileNew()
    {
        GetComponent<LoadMap>().StartLoad(true);
    }
    public void FileConvert()
    {
        GetComponent<CmapConvert>().ConvertCmap();
    }
    public void PlaceZone(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if(pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;
        string name = clicked.name;

        GetComponent<ZonePlacer>().PlaceZone(name);
    }
    public void ZoneDelete()
    {
        ZonePlacer zonePlacerScript = GetComponent<ZonePlacer>();

        zonePlacerScript.inDeleteMode = true;

        foreach(Transform zone in zonePlacerScript.zonesLayer)
        {
            if(zone.name != "empty")
            {
                zone.Find("Mover").GetComponent<SpriteRenderer>().sprite = zonePlacerScript.deleteZoneSprite;
            }
        }
    }
    public void ZoneMove()
    {
        ZonePlacer zonePlacerScript = GetComponent<ZonePlacer>();

        zonePlacerScript.inDeleteMode = true;

        foreach (Transform zone in zonePlacerScript.zonesLayer)
        {
            if (zone.name != "empty")
            {
                zone.Find("Mover").GetComponent<SpriteRenderer>().sprite = zonePlacerScript.moveZoneSprite;
            }
        }
    }
    public void PropertiesTileset()
    {
        pc.buttonType = "tileset";
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("PrisonSelectMenu").gameObject.SetActive(true);
    }
    public void PropertiesGround()
    {
        pc.buttonType = "ground";
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("PrisonSelectMenu").gameObject.SetActive(true);
    }
    public void PropertiesMusic()
    {
        pc.buttonType = "music";
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("PrisonSelectMenu").gameObject.SetActive(true);
    }
    public void PropertiesGrounds()
    {
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("GroundsSelectMenu").gameObject.SetActive(true);
    }
    public void PropertiesNote()
    {
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("NotePanel").gameObject.SetActive(true);
    }
    public void PropertiesRoutine()
    {
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("RoutinePanel").gameObject.SetActive(true);
    }
    public void PropertiesHint()
    {
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("HintPanel").gameObject.SetActive(true);
    }
    public void PropertiesJob()
    {
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("JobPanel").gameObject.SetActive(true);
    }
    public void PropertiesExtras()
    {
        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("ExtrasPanel").gameObject.SetActive(true);
    }
    public void PropertiesSize()
    {
        Transform sizePanel = uic.Find("SizePanel");

        pc.DeactivateButtons();
        uic.Find("Black").gameObject.SetActive(true);
        sizePanel.gameObject.SetActive(true);

        string currentResultText = uic.Find("PropertiesPanel").Find("SizeResultText").GetComponent<TextMeshProUGUI>().text;
        string[] parts = currentResultText.Split('x');

        int currentX = int.Parse(parts[0]);
        int currentY = int.Parse(parts[1]);

        sizePanel.Find("SizeX").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = currentX.ToString();
        sizePanel.Find("SizeY").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = currentY.ToString();

    }
    public void PropertiesIcon()
    {
        ExtensionFilter[] extensions = new ExtensionFilter[]
        {
            new ExtensionFilter("Image Files", "png")
        };
        smc.iconPath = StandaloneFileBrowser.OpenFilePanel("Select a custom icon.", "", extensions, false);

        uic.Find("PropertiesPanel").Find("IconResultText").GetComponent<TextMeshProUGUI>().text = "Custom";
    }
    public void PropertiesIconCancel()
    {
        smc.iconPath = null;
        uic.Find("PropertiesPanel").Find("IconResultText").GetComponent<TextMeshProUGUI>().text = "None";
    }
    public void NoteContinue()
    {
        uic.Find("NotePanel").gameObject.SetActive(false);
        uic.Find("Black").gameObject.SetActive(false);
        smc.ReactivateButtons();
    }
    public void RoutineClose()
    {
        uic.Find("RoutinePanel").gameObject.SetActive(false);
        uic.Find("Black").gameObject.SetActive(false);
        smc.ReactivateButtons();
    }
    public void HintClose()
    {
        uic.Find("HintPanel").gameObject.SetActive(false);
        uic.Find("Black").gameObject.SetActive(false);
        smc.ReactivateButtons();
    }
    public void Checkbox(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        if(clicked.GetComponent<Image>().sprite == smc.checkedBoxSprite)
        {
            clicked.GetComponent<Image>().sprite = smc.uncheckedBoxSprite;

            switch (clicked.name)
            {
                case "JanitorCheckbox":
                    smc.janitor = false;
                    break;
                case "GardeningCheckbox":
                    smc.gardening = false;
                    break;
                case "LaundryCheckbox":
                    smc.laundry = false;
                    break;
                case "KitchenCheckbox":
                    smc.kitchen = false;
                    break;
                case "TailorCheckbox":
                    smc.tailor = false;
                    break;
                case "WoodshopCheckbox":
                    smc.woodshop = false;
                    break;
                case "MetalshopCheckbox":
                    smc.metalshop = false;
                    break;
                case "DeliveriesCheckbox":
                    smc.deliveries = false;
                    break;
                case "MailmanCheckbox":
                    smc.mailman = false;
                    break;
                case "LibraryCheckbox":
                    smc.library = false;
                    break;
                case "SnowingCheckbox":
                    smc.snowing = false;
                    break;
                case "POWCheckbox":
                    smc.powOutfits = false;
                    break;
                case "StunRodCheckbox":
                    smc.stunRods = false;
                    break;
            }
        }
        else if(clicked.GetComponent<Image>().sprite == smc.uncheckedBoxSprite)
        {
            clicked.GetComponent<Image>().sprite = smc.checkedBoxSprite;

            switch (clicked.name)
            {
                case "JanitorCheckbox":
                    smc.janitor = true;
                    break;
                case "GardeningCheckbox":
                    smc.gardening = true;
                    break;
                case "LaundryCheckbox":
                    smc.laundry = true;
                    break;
                case "KitchenCheckbox":
                    smc.kitchen = true;
                    break;
                case "TailorCheckbox":
                    smc.tailor = true;
                    break;
                case "WoodshopCheckbox":
                    smc.woodshop = true;
                    break;
                case "MetalshopCheckbox":
                    smc.metalshop = true;
                    break;
                case "DeliveriesCheckbox":
                    smc.deliveries = true;
                    break;
                case "MailmanCheckbox":
                    smc.mailman = true;
                    break;
                case "LibraryCheckbox":
                    smc.library = true;
                    break;
                case "SnowingCheckbox":
                    smc.snowing = true;
                    break;
                case "POWCheckbox":
                    smc.powOutfits = true;
                    break;
                case "StunRodCheckbox":
                    smc.stunRods = true;
                    break;
            }
        }
    }
    public void JobClose()
    {
        uic.Find("JobPanel").gameObject.SetActive(false);
        uic.Find("Black").gameObject.SetActive(false);
        smc.ReactivateButtons();
    }
    public void ExtrasClose()
    {
        uic.Find("ExtrasPanel").gameObject.SetActive(false);
        uic.Find("Black").gameObject.SetActive(false);
        smc.ReactivateButtons();
    }
    public void PrisonSelect(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        if (clicked.name != "CustomButton") //actual prison
        {
            string whatPrison = "";

            foreach(Transform text in clicked.transform) //get prison name. doing this way because idk the name of the child
            {
                whatPrison = text.GetComponent<TextMeshProUGUI>().text;
                break;
            }

            switch (GetComponent<PropertiesController>().buttonType) //make changes to result texts and close menu
            {
                case "tileset":
                    uic.Find("PropertiesPanel").Find("TilesetResultText").GetComponent<TextMeshProUGUI>().text = smc.prisonDict[whatPrison];
                    smc.tilesPath = null;

                    Texture2D tileset;
                    string prison = prisonDict[smc.prisonDict[whatPrison]];
                    GetGivenData getGivenDataScript = GetGivenData.instance;
                    tileset = getGivenDataScript.tileTextureList[groundDict[prison]];

                    GetComponent<TileSpriteSetter>().SetSprites(tileset);
                    break;
                case "ground":
                    uic.Find("PropertiesPanel").Find("GroundResultText").GetComponent<TextMeshProUGUI>().text = smc.prisonDict[whatPrison];
                    smc.groundPath = null;

                    Sprite groundSprite;
                    string aPrison = prisonDict[smc.prisonDict[whatPrison]];
                    GetGivenData aGetGivenDataScript = GetGivenData.instance;
                    groundSprite = TextureToSprite(textureCornerGet(aGetGivenDataScript.groundTextureList[groundDict[aPrison]], 16, 16));

                    bool isTiled = true;
                    if(aPrison == "DTAF" || aPrison == "BC" || aPrison == "CCL")
                    {
                        isTiled = false;
                    }

                    if (isTiled)
                    {
                        groundSprite = TextureToSprite(textureCornerGet(aGetGivenDataScript.groundTextureList[groundDict[aPrison]], 16, 16));
                    }
                    else
                    {
                        groundSprite = TextureToSprite(aGetGivenDataScript.groundTextureList[groundDict[aPrison]]);
                    }

                        GetComponent<GroundSpriteSetter>().SetGround(groundSprite, isTiled);
                    break;
            }

            uic.Find("PrisonSelectMenu").gameObject.SetActive(false);
            uic.Find("Black").gameObject.SetActive(false);
            smc.ReactivateButtons();
        }
        else if (clicked.name == "CustomButton") //custom stuff
        {
            ExtensionFilter[] extensions = null;
            string title = "";
            string resultTextField = "";

            switch (GetComponent<PropertiesController>().buttonType) //open file dialog to select the custom file
            {
                case "tileset":
                    extensions = new[]
                    {
                        new ExtensionFilter("Image Files", "png")
                    };
                    title = "Select a custom tileset.";
                    resultTextField = "TilesetResultText";

                    smc.tilesPath = StandaloneFileBrowser.OpenFilePanel(title, "", extensions, false);

                    Texture2D tileset = ConvertPNGToSprite(smc.tilesPath[0]).texture;
                    GetComponent<TileSpriteSetter>().SetSprites(tileset);
                    break;
                case "ground":
                    extensions = new[]
                    {
                        new ExtensionFilter("Image Files", "png")
                    };
                    title = "Select a custom ground.";
                    resultTextField = "GroundResultText";

                    smc.groundPath = StandaloneFileBrowser.OpenFilePanel(title, "", extensions, false);

                    Sprite ground = ConvertPNGToSprite(smc.groundPath[0]);
                    GetComponent<GroundSpriteSetter>().SetGround(ground, false);
                    break;
            }
            uic.Find("PropertiesPanel").Find(resultTextField).GetComponent<TextMeshProUGUI>().text = "Custom";
            uic.Find("PrisonSelectMenu").gameObject.SetActive(false);
            uic.Find("Black").gameObject.SetActive(false);
            smc.ReactivateButtons();
        }
    }
    public void GroundsSelect(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        switch (clicked.name)
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
        smc.ReactivateButtons();
    }
    public void SizeSet()
    {
        Transform sizePanel = uic.Find("SizePanel");
        string rawX = sizePanel.Find("SizeX").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;
        string rawY = sizePanel.Find("SizeY").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text;

        rawX = Regex.Replace(rawX, @"[^\d]", "");
        rawY = Regex.Replace(rawY, @"[^\d]", "");

        int x = Convert.ToInt32(rawX);
        int y = Convert.ToInt32(rawY);

        smc.gridScript.DrawGrid(x, y);
        smc.groundSizeSetScript.SetSize();
        uic.Find("PropertiesPanel").Find("SizeResultText").GetComponent<TextMeshProUGUI>().text = x + "x" + y;
        uic.Find("Black").gameObject.SetActive(false);
        uic.Find("SizePanel").gameObject.SetActive(false);
        smc.ReactivateButtons();
    }
    public void SizeCancel()
    {
        uic.Find("Black").gameObject.SetActive(false);
        uic.Find("SizePanel").gameObject.SetActive(false);
        smc.ReactivateButtons();
    }
    public void PanelFile()
    {
        uic.Find("TilesPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(true);
        uic.Find("AdvancedPanel").gameObject.SetActive(false);

        GetComponent<ObjectsPanelController>().canBeShown = false;

        panelSelectScript.currentPanel = "FilePanel";
    }
    public void PanelTiles()
    {
        uic.Find("TilesPanel").gameObject.SetActive(true);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(false);
        uic.Find("AdvancedPanel").gameObject.SetActive(false);

        GetComponent<ObjectsPanelController>().canBeShown = false;

        panelSelectScript.currentPanel = "TilesPanel";
    }
    public void PanelObjects()
    {
        uic.Find("TilesPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(false);
        uic.Find("AdvancedPanel").gameObject.SetActive(false);

        GetComponent<ObjectsPanelController>().canBeShown = true;

        panelSelectScript.currentPanel = "ObjectsPanel";
    }
    public void PanelZoneObjects()
    {
        uic.Find("TilesPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(true);
        uic.Find("FilePanel").gameObject.SetActive(false);
        uic.Find("AdvancedPanel").gameObject.SetActive(false);

        GetComponent<ObjectsPanelController>().canBeShown = false;

        panelSelectScript.currentPanel = "ZonesPanel";
    }
    public void PanelProperties()
    {
        uic.Find("TilesPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(true);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(false);
        uic.Find("AdvancedPanel").gameObject.SetActive(false);

        GetComponent<ObjectsPanelController>().canBeShown = false;

        panelSelectScript.currentPanel = "PropertiesPanel";
    }
    public void PanelAdvanced()
    {
        uic.Find("TilesPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(false);
        uic.Find("AdvancedPanel").gameObject.SetActive(true);

        GetComponent<ObjectsPanelController>().canBeShown = false;

        panelSelectScript.currentPanel = "AdvancedPanel";
    }
    public void LayerGround()
    {
        layerControllerScript.currentLayer = 1;
    }
    public void LayerUnderground()
    {
        layerControllerScript.currentLayer = 0;
    }
    public void LayerVents()
    {
        layerControllerScript.currentLayer = 2;
    }
    public void LayerRoof()
    {
        layerControllerScript.currentLayer = 3;
    }
    public void LayerZones()
    {
        layerControllerScript.currentLayer = 4;
    }
    public void ObjectsLeft()
    {
        GetComponent<ObjectsPanelController>().objectPanelNum--;
        if(GetComponent<ObjectsPanelController>().objectPanelNum < 0)
        {
            GetComponent<ObjectsPanelController>().objectPanelNum = 14;
        }
    }
    public void ObjectsRight()
    {
        GetComponent<ObjectsPanelController>().objectPanelNum++;
        if(GetComponent<ObjectsPanelController>().objectPanelNum > 14)
        {
            GetComponent<ObjectsPanelController>().objectPanelNum = 0;
        }
    }
    public void AdvancedHelp()
    {
        DeactivateAdvancedButtons();
        uic.Find("Black").gameObject.SetActive(true);
        uic.Find("AdvancedHelpPanel").gameObject.SetActive(true);
    }
    public void AdvancedButton(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        string msg = "";
        switch (clicked.name)
        {
            case "SpeechButton":
                msg = "Select a custom speech file.";
                break;
            case "TooltipsButton":
                msg = "Select a custom tooltips file.";
                break;
            case "ItemsButton":
                msg = "Select a custom items file.";
                break;
        }

        ExtensionFilter[] extensions = new ExtensionFilter[]
        {
            new ExtensionFilter("Text Files", "ini")
        };

        switch (clicked.name)
        {
            case "SpeechButton":
                try
                {
                    speechPath = StandaloneFileBrowser.OpenFilePanel(msg, "", extensions, false)[0];
                }
                catch
                {
                    speechPath = null;
                }
                if (!string.IsNullOrEmpty(speechPath))
                {
                    uic.Find("AdvancedPanel").Find("SpeechResultText").GetComponent<TextMeshProUGUI>().text = "Custom";
                }
                else
                {
                    uic.Find("AdvancedPanel").Find("SpeechResultText").GetComponent<TextMeshProUGUI>().text = "Normal";
                }
                break;
            case "TooltipsButton":
                try
                {
                    tooltipsPath = StandaloneFileBrowser.OpenFilePanel(msg, "", extensions, false)[0];
                }
                catch
                {
                    tooltipsPath = null;
                }
                if (!string.IsNullOrEmpty(tooltipsPath))
                {
                    uic.Find("AdvancedPanel").Find("TooltipsResultText").GetComponent<TextMeshProUGUI>().text = "Custom";
                }
                else
                {
                    uic.Find("AdvancedPanel").Find("TooltipsResultText").GetComponent<TextMeshProUGUI>().text = "Normal";
                }
                break;
            case "Items":
                try
                {
                    itemsPath = StandaloneFileBrowser.OpenFilePanel(msg, "", extensions, false)[0];
                }
                catch
                {
                    itemsPath = null;
                }
                if (!string.IsNullOrEmpty(itemsPath))
                {
                    uic.Find("AdvancedPanel").Find("ItemsResultText").GetComponent<TextMeshProUGUI>().text = "Custom";
                }
                else
                {
                    uic.Find("AdvancedPanel").Find("ItemsResultText").GetComponent<TextMeshProUGUI>().text = "Normal";
                }
                break;
        }
    }
    public void AdvancedCancel(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        switch (clicked.name)
        {
            case "SpeechCancel":
                uic.Find("AdvancedPanel").Find("SpeechResultText").GetComponent<TextMeshProUGUI>().text = "Normal";
                break;
            case "TooltipsCancel":
                uic.Find("AdvancedPanel").Find("TooltipsResultText").GetComponent<TextMeshProUGUI>().text = "Normal";
                break;
            case "ItemsCancel":
                uic.Find("AdvancedPanel").Find("ItemsResultText").GetComponent<TextMeshProUGUI>().text = "Normal";
                break;
        }
    }
    public void AdvancedAdd(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        switch (clicked.name)
        {
            case "SpeechAdd":
                string content = Resources.Load<TextAsset>("Speech").text;

                var extensions = new ExtensionFilter[]
                {
                    new ExtensionFilter("Text Files", "ini"),
                    new ExtensionFilter("All Files", "*")
                };

                string path = StandaloneFileBrowser.SaveFilePanel(
                    "Download Speech File",
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Speech",
                    extensions
                );

                if (!string.IsNullOrEmpty(path))
                {
                    File.WriteAllText(path, content, Encoding.UTF8);
                }
                break;
            case "TooltipsAdd":
                string content1 = Resources.Load<TextAsset>("Tooltips").text;

                var extensions1 = new ExtensionFilter[]
                {
                    new ExtensionFilter("Text Files", "ini"),
                    new ExtensionFilter("All Files", "*")
                };

                string path1 = StandaloneFileBrowser.SaveFilePanel(
                    "Download Tooltips File",
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Tooltips",
                    extensions1
                );

                if (!string.IsNullOrEmpty(path1))
                {
                    File.WriteAllText(path1, content1, Encoding.UTF8);
                }
                break;
            case "ItemsAdd":
                string content2 = Resources.Load<TextAsset>("Items").text;

                var extensions2 = new ExtensionFilter[]
                {
                    new ExtensionFilter("Text Files", "ini"),
                    new ExtensionFilter("All Files", "*")
                };

                string path2 = StandaloneFileBrowser.SaveFilePanel(
                    "Download Items File",
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Items",
                    extensions2
                );

                if (!string.IsNullOrEmpty(path2))
                {
                    File.WriteAllText(path2, content2, Encoding.UTF8);
                }
                break;
        }
    }
    public void AdvancedMusic()
    {
        uic.Find("MusicSelectMenu").gameObject.SetActive(true);
        DeactivateAdvancedButtons();
        uic.Find("Black").gameObject.SetActive(true);
    }
    public void MusicSelect(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        if (clicked.name != "CustomButton")
        {
            string whatPrison = "";
            foreach (Transform child in clicked.transform)
            {
                whatPrison = child.GetComponent<TextMeshProUGUI>().text;
                break;
            }

            uic.Find("AdvancedPanel").Find("MusicResultText").GetComponent<TextMeshProUGUI>().text = smc.prisonDict[whatPrison];
        }
        else
        {
            ExtensionFilter[] extensions = new ExtensionFilter[]
            {
                new ExtensionFilter("Zip Files", "zip")
            };

            try
            {
                musicPath = StandaloneFileBrowser.OpenFilePanel("Select a music pack.", "", extensions, false)[0];
            }
            catch 
            { 
                musicPath = null; 
            }

            if (!string.IsNullOrEmpty(musicPath))
            {
                uic.Find("AdvancedPanel").Find("MusicResultText").GetComponent<TextMeshProUGUI>().text = "Custom";
            }
        }

        uic.Find("Black").gameObject.SetActive(false);
        uic.Find("MusicSelectMenu").gameObject.SetActive(false);
        ReactivateAdvancedButtons();
    }
    public void HelpClose()
    {
        ReactivateAdvancedButtons();
        uic.Find("Black").gameObject.SetActive(false);
        uic.Find("AdvancedHelpPanel").gameObject.SetActive(false);
    }
    public void Origin()
    {
        camera.position = new Vector3(((gridScript.sizeX * 1.6f) / 2f) - .8f, ((gridScript.sizeY * 1.6f) / 2f) - .8f, -1);
    }
    public void SpecialObject(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        string type = clicked.GetComponent<SpecialButtonTypeContainer>().type; //desk, blueSign, whiteSign, item

        Transform currentObject = null;
        Transform currentCanvas = clicked.transform.parent;

        string layer = "";
        switch (layerControllerScript.currentLayer)
        {
            case 0:
                layer = "Underground";
                break;
            case 1:
                layer = "Ground";
                break;
            case 2:
                layer = "Vent";
                break;
            case 3:
                layer = "Roof";
                break;
        }

        foreach(Transform obj in tiles.Find(layer + "Objects"))
        {
            float distance = Vector2.Distance(obj.position, currentCanvas.position);
            if(distance <= .01f)
            {
                currentObject = obj;
                break;
            }
        }

        switch (type)
        {
            case "item":
                GetComponent<MEItemController>().currentItem = currentObject;
                uic.Find("ItemSpecialPanel").gameObject.SetActive(true);
                break;
            case "desk":
                GetComponent<MEDeskController>().currentDesk = currentObject;
                GetComponent<MEDeskController>().SetIDs();
                uic.Find("DeskPanel").gameObject.SetActive(true);
                break;
            case "blueSign":
                GetComponent<MESignController>().currentSign = currentObject;
                uic.Find("BlueSignPanel").gameObject.SetActive(true);
                break;
            case "whiteSign":
                GetComponent<MESignController>().currentSign = currentObject;
                uic.Find("WhiteSignPanel").gameObject.SetActive(true);
                break;
        }
        uic.Find("Black").gameObject.SetActive(true);
        DeactivateButtonsForSpecialObjects();
    }
    public void ItemSpecialChange()
    {
        GetComponent<MEItemController>().currentItem.GetComponent<MEItemIDContainer>().id = Convert.ToInt32(uic.Find("ItemSpecialPanel").Find("IDInput").GetComponent<TMP_InputField>().text);
        ItemSpecialCancel();
    }
    public void ItemSpecialCancel()
    {
        uic.Find("ItemSpecialPanel").gameObject.SetActive(false);
        uic.Find("Black").gameObject.SetActive(false);
        ActivateButtonsForSpecialObjects();
    }
    public void DeskSlot(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if (pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        int slotNum = Convert.ToInt32(clicked.name);
        GetComponent<MEDeskController>().SelectSlot(slotNum);
    }

    private Sprite ConvertPNGToSprite(string path)
    {
        byte[] pngBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(pngBytes);

        texture.filterMode = FilterMode.Point;

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f), 100.0f);
    }
    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f), // pivot in the center
            100f,                     // pixels per unit
            0,
            SpriteMeshType.FullRect
        );
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
    private void DeactivateAdvancedButtons()
    {
        Transform advanced = uic.Find("AdvancedPanel");

        advanced.Find("SpeechButton").GetComponent<Button>().enabled = false;
        advanced.Find("SpeechCancel").GetComponent<Button>().enabled = false;
        advanced.Find("SpeechAdd").GetComponent<Button>().enabled = false;
        advanced.Find("TooltipsButton").GetComponent<Button>().enabled = false;
        advanced.Find("TooltipsCancel").GetComponent<Button>().enabled = false;
        advanced.Find("TooltipsAdd").GetComponent<Button>().enabled = false;
        advanced.Find("MusicButton").GetComponent<Button>().enabled = false;
        advanced.Find("ItemsButton").GetComponent<Button>().enabled = false;
        advanced.Find("ItemsCancel").GetComponent<Button>().enabled = false;
        advanced.Find("ItemsAdd").GetComponent<Button>().enabled = false;
        advanced.Find("HelpButton").GetComponent<Button>().enabled = false;
        advanced.Find("SpeechButton").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("SpeechCancel").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("SpeechAdd").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("TooltipsButton").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("TooltipsCancel").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("TooltipsAdd").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("MusicButton").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("ItemsButton").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("ItemsCancel").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("ItemsAdd").GetComponent<EventTrigger>().enabled = false;
        advanced.Find("HelpButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("FileButton").GetComponent<Button>().enabled = false;
        uic.Find("FileButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("TilesButton").GetComponent<Button>().enabled = false;
        uic.Find("TilesButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("ObjectsButton").GetComponent<Button>().enabled = false;
        uic.Find("ObjectsButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("PropertiesButton").GetComponent<Button>().enabled = false;
        uic.Find("PropertiesButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("GroundButton").GetComponent<Button>().enabled = false;
        uic.Find("GroundButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("UndergroundButton").GetComponent<Button>().enabled = false;
        uic.Find("UndergroundButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("VentsButton").GetComponent<Button>().enabled = false;
        uic.Find("VentsButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("RoofButton").GetComponent<Button>().enabled = false;
        uic.Find("RoofButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("ZoneObjectsButton").GetComponent<Button>().enabled = false;
        uic.Find("ZoneObjectsButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("AdvancedButton").GetComponent<Button>().enabled = false;
        uic.Find("AdvancedButton").GetComponent<EventTrigger>().enabled = false;
        canvases.gameObject.SetActive(false);
    }
    private void ReactivateAdvancedButtons()
    {
        Transform advanced = uic.Find("AdvancedPanel");

        advanced.Find("SpeechButton").GetComponent<Button>().enabled = true;
        advanced.Find("SpeechCancel").GetComponent<Button>().enabled = true;
        advanced.Find("SpeechAdd").GetComponent<Button>().enabled = true;
        advanced.Find("TooltipsButton").GetComponent<Button>().enabled = true;
        advanced.Find("TooltipsCancel").GetComponent<Button>().enabled = true;
        advanced.Find("TooltipsAdd").GetComponent<Button>().enabled = true;
        advanced.Find("MusicButton").GetComponent<Button>().enabled = true;
        advanced.Find("ItemsButton").GetComponent<Button>().enabled = true;
        advanced.Find("ItemsCancel").GetComponent<Button>().enabled = true;
        advanced.Find("ItemsAdd").GetComponent<Button>().enabled = true;
        advanced.Find("HelpButton").GetComponent<Button>().enabled = true;
        advanced.Find("SpeechButton").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("SpeechCancel").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("SpeechAdd").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("TooltipsButton").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("TooltipsCancel").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("TooltipsAdd").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("MusicButton").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("ItemsButton").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("ItemsCancel").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("ItemsAdd").GetComponent<EventTrigger>().enabled = true;
        advanced.Find("HelpButton").GetComponent<EventTrigger>().enabled = true;
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
        canvases.gameObject.SetActive(true);
    }
    private void DeactivateButtonsForSpecialObjects()
    {
        uic.Find("FileButton").GetComponent<Button>().enabled = false;
        uic.Find("FileButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("TilesButton").GetComponent<Button>().enabled = false;
        uic.Find("TilesButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("ObjectsButton").GetComponent<Button>().enabled = false;
        uic.Find("ObjectsButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("PropertiesButton").GetComponent<Button>().enabled = false;
        uic.Find("PropertiesButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("GroundButton").GetComponent<Button>().enabled = false;
        uic.Find("GroundButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("UndergroundButton").GetComponent<Button>().enabled = false;
        uic.Find("UndergroundButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("VentsButton").GetComponent<Button>().enabled = false;
        uic.Find("VentsButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("RoofButton").GetComponent<Button>().enabled = false;
        uic.Find("RoofButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("ZoneObjectsButton").GetComponent<Button>().enabled = false;
        uic.Find("ZoneObjectsButton").GetComponent<EventTrigger>().enabled = false;
        uic.Find("AdvancedButton").GetComponent<Button>().enabled = false;
        uic.Find("AdvancedButton").GetComponent<EventTrigger>().enabled = false;
        try
        {
            uic.Find(panelSelectScript.currentPanel).gameObject.SetActive(false);
        }
        catch { }
        if(panelSelectScript.currentPanel == "ObjectsPanel")
        {
            GetComponent<ObjectsPanelController>().canBeShown = false;
        }
        canvases.gameObject.SetActive(false);
    }
    private void ActivateButtonsForSpecialObjects()
    {
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
        try
        {
            uic.Find(panelSelectScript.currentPanel).gameObject.SetActive(true);
        }
        catch { }
        if (panelSelectScript.currentPanel == "ObjectsPanel")
        {
            GetComponent<ObjectsPanelController>().canBeShown = true;
        }
        canvases.gameObject.SetActive(true);
    }
}
