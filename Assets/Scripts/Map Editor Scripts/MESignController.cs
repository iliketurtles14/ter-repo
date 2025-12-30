using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MESignController : MonoBehaviour
{
    public Transform uic;
    private Transform currentPanel;
    public string currentHeader;
    public string currentBody;
    public Transform currentSign;
    private bool hasSetText;
    public PanelSelect panelSelectScript;
    public Transform canvases;
    private void Update()
    {
        if (uic.Find("BlueSignPanel").gameObject.activeInHierarchy)
        {
            currentPanel = uic.Find("BlueSignPanel");
        }
        else if (uic.Find("WhiteSignPanel").gameObject.activeInHierarchy)
        {
            currentPanel = uic.Find("WhiteSignPanel");
        }
        else
        {
            currentPanel = null;
        }

        if(currentPanel != null && !hasSetText)
        {
            currentHeader = currentSign.GetComponent<MESignTextContainer>().header;
            currentBody = currentSign.GetComponent<MESignTextContainer>().body;
            
            currentPanel.Find("HeaderInput").GetComponent<TMP_InputField>().text = currentHeader;
            currentPanel.Find("BodyInput").GetComponent<TMP_InputField>().text = currentBody;
            hasSetText = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && currentPanel != null)
        {
            SetText();
            uic.Find("Black").gameObject.SetActive(false);
            uic.Find("BlueSignPanel").gameObject.SetActive(false);
            uic.Find("WhiteSignPanel").gameObject.SetActive(false);
            hasSetText = false;
            ActivateButtonsForSpecialObjects();
        }
    }
    private void SetText()
    {
        currentSign.GetComponent<MESignTextContainer>().header = currentPanel.Find("HeaderInput").GetComponent<TMP_InputField>().text;
        currentSign.GetComponent<MESignTextContainer>().body = currentPanel.Find("BodyInput").GetComponent<TMP_InputField>().text;
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
        GetComponent<ObjectsPanelController>().canBeShown = true;
        canvases.gameObject.SetActive(true);
    }
}
