using Ookii.Dialogs;
using SFB;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MEButtonController : MonoBehaviour
{
    public Transform uic;
    private PropertiesController pc;
    private SubMenuController smc;
    private PanelSelect panelSelectScript;
    private LayerController layerControllerScript;
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
                    break;
                case "ground":
                    uic.Find("PropertiesPanel").Find("GroundResultText").GetComponent<TextMeshProUGUI>().text = smc.prisonDict[whatPrison];
                    smc.groundPath = null;
                    break;
                case "music":
                    uic.Find("PropertiesPanel").Find("MusicResultText").GetComponent<TextMeshProUGUI>().text = smc.prisonDict[whatPrison];
                    smc.musicPath = null;
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

                    break;
                case "ground":
                    extensions = new[]
                    {
                                new ExtensionFilter("Image Files", "png")
                            };
                    title = "Select a custom ground.";
                    resultTextField = "GroundResultText";

                    smc.groundPath = StandaloneFileBrowser.OpenFilePanel(title, "", extensions, false);

                    break;
                case "music":
                    extensions = new[]
                    {
                                new ExtensionFilter("Sound Files", "mp3")
                            };
                    title = "Select custom music.";
                    resultTextField = "MusicResultText";

                    smc.musicPath = StandaloneFileBrowser.OpenFilePanel(title, "", extensions, false);

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

        if (x >= 25 && x <= 150 && y >= 25 && x <= 150) //check to see if the map will be too small or big
        {
            smc.gridScript.DrawGrid(x, y);
            smc.groundSizeSetScript.SetSize();
            uic.Find("PropertiesPanel").Find("SizeResultText").GetComponent<TextMeshProUGUI>().text = x + "x" + y;
            uic.Find("Black").gameObject.SetActive(false);
            uic.Find("SizePanel").gameObject.SetActive(false);
            smc.ReactivateButtons();
        }
        else
        {
            Debug.Log("Map size is too big or too small.");
        }
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
        uic.Find("ObjectsPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(true);

        panelSelectScript.currentPanel = "FilePanel";
    }
    public void PanelTiles()
    {
        uic.Find("TilesPanel").gameObject.SetActive(true);
        uic.Find("ObjectsPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(false);

        panelSelectScript.currentPanel = "TilesPanel";
    }
    public void PanelObjects()
    {
        uic.Find("TilesPanel").gameObject.SetActive(false);
        uic.Find("ObjectsPanel").gameObject.SetActive(true);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(false);

        panelSelectScript.currentPanel = "ObjectsPanel";
    }
    public void PanelZoneObjects()
    {
        uic.Find("TilesPanel").gameObject.SetActive(false);
        uic.Find("ObjectsPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(false);
        uic.Find("ZonesPanel").gameObject.SetActive(true);
        uic.Find("FilePanel").gameObject.SetActive(false);

        panelSelectScript.currentPanel = "ZonesPanel";
    }
    public void PanelProperties()
    {
        uic.Find("TilesPanel").gameObject.SetActive(false);
        uic.Find("ObjectsPanel").gameObject.SetActive(false);
        uic.Find("PropertiesPanel").gameObject.SetActive(true);
        uic.Find("ZonesPanel").gameObject.SetActive(false);
        uic.Find("FilePanel").gameObject.SetActive(false);

        panelSelectScript.currentPanel = "PropertiesPanel";
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
}
