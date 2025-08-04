using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuController : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform uic;
    public Sprite uncheckedBoxSprite;
    public Sprite checkedBoxSprite;

    public Dictionary<string, string> prisonDict = new Dictionary<string, string>()
    {
        { "Center Perks", "perks" }, { "Stalag Flucht", "stalag" }, { "Shankton State Pen", "shankton" },
        { "Jungle Compound", "jungle" }, { "San Pancho", "sanpancho" }, { "HMP Irongate", "irongate" },
        { "Jingle Cells", "JC" }, { "Banned Camp", "BC" }, { "London Tower", "london" },
        { "Paris Central Pen", "PCP" }, { "Santa's Sweatshop", "SS" }, { "Duct Tapes Are Forever", "DTAF" },
        { "Escape Team", "ET" }, { "Alcatraz", "alca" }, { "Fhurst Peak Correctional", "fhurst" },
        { "Camp Epsilon", "epsilon" }, { "Fort Bamford", "bamford" }
    };

    private void Update()
    {
        if (mcs.isTouchingButton)
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
            }
            else if (mcs.touchedButton.name.EndsWith("Checkbox") && mcs.touchedButton.GetComponent<Image>().sprite == uncheckedBoxSprite)
            {
                mcs.touchedButton.GetComponent<Image>().sprite = checkedBoxSprite;
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
            else if(mcs.touchedButton.name.EndsWith("Button") && mcs.touchedButton.transform.parent.parent.name == "PrisonSelectMenu")
            {

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
        propertiesPanel.Find("SizeX").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("SizeY").GetComponent<TMP_InputField>().enabled = true;
        propertiesPanel.Find("NameInputField").GetComponent<TMP_InputField>().enabled = true;

        uic.Find("LoadButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("TilesButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("ObjectsButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("PropertiesButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("ExportButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("GroundButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("UndergroundButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("VentsButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("RoofButton").GetComponent<BoxCollider2D>().enabled = true;
        uic.Find("ZonesButton").GetComponent<BoxCollider2D>().enabled = true;
    }
}
