using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropertiesController : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform canvases;
    public Transform uic;
    public string buttonType;
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
        uic.Find("AdvancedButton").GetComponent<Button>().enabled = false;
        uic.Find("AdvancedButton").GetComponent<EventTrigger>().enabled = false;
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
        uic.Find("OriginButton").GetComponent<Button>().enabled = false;
        uic.Find("OriginButton").GetComponent<EventTrigger>().enabled = false;
        canvases.gameObject.SetActive(false);
    }
}
