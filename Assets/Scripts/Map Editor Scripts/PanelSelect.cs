using UnityEngine;

public class PanelSelect : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public Transform uic;
    private void Update()
    {
        if(mcs.isTouchingButton && mcs.touchedButton.name == "LoadButton" && Input.GetMouseButtonDown(0))
        {

        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "TilesButton" && Input.GetMouseButtonDown(0))
        {
            uic.Find("TilesPanel").gameObject.SetActive(true);
            uic.Find("ObjectsPanel").gameObject.SetActive(false);
            uic.Find("PropertiesPanel").gameObject.SetActive(false);
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "ObjectsButton" && Input.GetMouseButtonDown(0))
        {
            uic.Find("TilesPanel").gameObject.SetActive(false);
            uic.Find("ObjectsPanel").gameObject.SetActive(true);
            uic.Find("PropertiesPanel").gameObject.SetActive(false);
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "PropertiesButton" && Input.GetMouseButtonDown(0))
        {
            uic.Find("TilesPanel").gameObject.SetActive(false);
            uic.Find("ObjectsPanel").gameObject.SetActive(false);
            uic.Find("PropertiesPanel").gameObject.SetActive(true);
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "ExportButton" && Input.GetMouseButtonDown(0))
        {

        }
        
    }
}
