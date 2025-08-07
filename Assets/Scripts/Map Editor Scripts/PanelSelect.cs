using UnityEngine;

public class PanelSelect : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform uic;
    public string currentPanel;
    private void Update()
    {
        if(mcs.isTouchingButton && mcs.touchedButton.name == "FileButton" && Input.GetMouseButtonDown(0))
        {
            uic.Find("TilesPanel").gameObject.SetActive(false);
            uic.Find("ObjectsPanel").gameObject.SetActive(false);
            uic.Find("PropertiesPanel").gameObject.SetActive(false);
            uic.Find("ZonesPanel").gameObject.SetActive(false);
            uic.Find("FilePanel").gameObject.SetActive(true);

            currentPanel = "FilePanel";
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "TilesButton" && Input.GetMouseButtonDown(0))
        {
            uic.Find("TilesPanel").gameObject.SetActive(true);
            uic.Find("ObjectsPanel").gameObject.SetActive(false);
            uic.Find("PropertiesPanel").gameObject.SetActive(false);
            uic.Find("ZonesPanel").gameObject.SetActive(false);
            uic.Find("FilePanel").gameObject.SetActive(false);

            currentPanel = "TilesPanel";
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "ObjectsButton" && Input.GetMouseButtonDown(0))
        {
            uic.Find("TilesPanel").gameObject.SetActive(false);
            uic.Find("ObjectsPanel").gameObject.SetActive(true);
            uic.Find("PropertiesPanel").gameObject.SetActive(false);
            uic.Find("ZonesPanel").gameObject.SetActive(false);
            uic.Find("FilePanel").gameObject.SetActive(false);

            currentPanel = "ObjectsPanel";
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "ZoneObjectsButton" && Input.GetMouseButtonDown(0))
        {
            uic.Find("TilesPanel").gameObject.SetActive(false);
            uic.Find("ObjectsPanel").gameObject.SetActive(false);
            uic.Find("PropertiesPanel").gameObject.SetActive(false);
            uic.Find("ZonesPanel").gameObject.SetActive(true);
            uic.Find("FilePanel").gameObject.SetActive(false);

            currentPanel = "ZonesPanel";
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "PropertiesButton" && Input.GetMouseButtonDown(0))
        {
            uic.Find("TilesPanel").gameObject.SetActive(false);
            uic.Find("ObjectsPanel").gameObject.SetActive(false);
            uic.Find("PropertiesPanel").gameObject.SetActive(true);
            uic.Find("ZonesPanel").gameObject.SetActive(false);
            uic.Find("FilePanel").gameObject.SetActive(false);

            currentPanel = "PropertiesPanel";
        }
    }
}
