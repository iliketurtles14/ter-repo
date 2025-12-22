using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropertiesController : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform uic;
    public string buttonType;
    private void Update()
    {
        if(mcs.isTouchingButton && Input.GetMouseButtonDown(0))
        {
            if (mcs.touchedButton.name == "TilesetButton")
            {
                buttonType = "tileset";
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("PrisonSelectMenu").gameObject.SetActive(true);
            }
            else if (mcs.touchedButton.name == "GroundButton" && mcs.touchedButton.transform.parent.name == "PropertiesPanel")
            {
                buttonType = "ground";
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("PrisonSelectMenu").gameObject.SetActive(true);
            }
            else if (mcs.touchedButton.name == "MusicButton")
            {
                buttonType = "music";
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("PrisonSelectMenu").gameObject.SetActive(true);
            }
            else if (mcs.touchedButton.name == "GroundsButton")
            {
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("GroundsSelectMenu").gameObject.SetActive(true);
            }
            else if (mcs.touchedButton.name == "WardenNoteButton")
            {
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("NotePanel").gameObject.SetActive(true);
            }
            else if (mcs.touchedButton.name == "RoutineButton")
            {
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("RoutinePanel").gameObject.SetActive(true);
            }
            else if (mcs.touchedButton.name == "HintButton")
            {
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("HintPanel").gameObject.SetActive(true);
            }
            else if (mcs.touchedButton.name == "JobButton")
            {
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("JobPanel").gameObject.SetActive(true);
            }
            else if (mcs.touchedButton.name == "ExtrasButton")
            {
                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                uic.Find("ExtrasPanel").gameObject.SetActive(true);
            }
            else if(mcs.touchedButton.name == "SizeButton")
            {
                Transform sizePanel = uic.Find("SizePanel");

                DeactivateButtons();
                uic.Find("Black").gameObject.SetActive(true);
                sizePanel.gameObject.SetActive(true);

                string currentResultText = uic.Find("PropertiesPanel").Find("SizeResultText").GetComponent<TextMeshProUGUI>().text;
                string[] parts = currentResultText.Split('x');

                int currentX = int.Parse(parts[0]);
                int currentY = int.Parse(parts[1]);

                sizePanel.Find("SizeX").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = currentX.ToString();
                sizePanel.Find("SizeY").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = currentY.ToString();
            }
        }
    }
    public void DeactivateButtons()
    {
        Transform propertiesPanel = uic.Find("PropertiesPanel");

        propertiesPanel.Find("WardenNoteButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("WardenNoteButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("RoutineButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("RoutineButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("HintButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("HintButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("JobButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("JobButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("ExtrasButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("ExtrasButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("TilesetButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("TilesetButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("GroundButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("GroundButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("MusicButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("MusicButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("IconButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("IconButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("GroundsButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("GroundsButton").GetComponent<EventTrigger>().enabled = false;
        propertiesPanel.Find("GuardsNum").GetComponent<TMP_InputField>().enabled = false;
        propertiesPanel.Find("InmatesNum").GetComponent<TMP_InputField>().enabled = false;
        propertiesPanel.Find("NPCLevelNum").GetComponent<TMP_InputField>().enabled = false;
        propertiesPanel.Find("NameInputField").GetComponent<TMP_InputField>().enabled = false;
        propertiesPanel.Find("SizeButton").GetComponent<Button>().enabled = false;
        propertiesPanel.Find("SizeButton").GetComponent<EventTrigger>().enabled = false;

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
    }
}
