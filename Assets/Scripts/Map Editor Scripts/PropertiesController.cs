using TMPro;
using UnityEngine;

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
    private void DeactivateButtons()
    {
        Transform propertiesPanel = uic.Find("PropertiesPanel");

        propertiesPanel.Find("WardenNoteButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("RoutineButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("HintButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("JobButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("ExtrasButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("TilesetButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("GroundButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("MusicButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("IconButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("GroundsButton").GetComponent<BoxCollider2D>().enabled = false;
        propertiesPanel.Find("GuardsNum").GetComponent<TMP_InputField>().enabled = false;
        propertiesPanel.Find("InmatesNum").GetComponent<TMP_InputField>().enabled = false;
        propertiesPanel.Find("NPCLevelNum").GetComponent<TMP_InputField>().enabled = false;
        propertiesPanel.Find("NameInputField").GetComponent<TMP_InputField>().enabled = false;
        propertiesPanel.Find("SizeButton").GetComponent<BoxCollider2D>().enabled = false;

        uic.Find("FileButton").GetComponent<BoxCollider2D>().enabled = false;
        uic.Find("TilesButton").GetComponent<BoxCollider2D>().enabled = false;
        uic.Find("ObjectsButton").GetComponent<BoxCollider2D>().enabled = false;
        uic.Find("PropertiesButton").GetComponent<BoxCollider2D>().enabled = false;
        uic.Find("GroundButton").GetComponent<BoxCollider2D>().enabled = false;
        uic.Find("UndergroundButton").GetComponent<BoxCollider2D>().enabled = false;
        uic.Find("VentsButton").GetComponent<BoxCollider2D>().enabled = false;
        uic.Find("RoofButton").GetComponent<BoxCollider2D>().enabled = false;
        uic.Find("ZoneObjectsButton").GetComponent<BoxCollider2D>().enabled = false;
    }
}
