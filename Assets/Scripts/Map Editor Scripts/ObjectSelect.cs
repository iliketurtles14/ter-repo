using UnityEngine;
using UnityEngine.UI;

public class ObjectSelect : MonoBehaviour
{
    public MouseCollisionOnMap mcs;
    public GameObject selectedObj;
    public bool hasSelected;
    public Material outlineMat;
    public Material unlitMat;
    public GameObject highlight;

    private void Update()
    {
        string currentPanel = GetComponent<PanelSelect>().currentPanel;
        
        if(currentPanel == "ObjectsPanel" && mcs.isTouchingObject && Input.GetMouseButtonDown(0))
        {
            if(selectedObj != null) 
            {
                selectedObj.GetComponent<Image>().material = unlitMat;
            }
            SelectObject(mcs.touchedObject);
        }

        if(currentPanel != "ObjectsPanel" && hasSelected)
        {
            DeselectObject();
        }
    }
    public void SelectObject(GameObject obj)
    {
        obj.GetComponent<Image>().material = outlineMat;
        selectedObj = obj;
        hasSelected = true;

        highlight.SetActive(true);
        highlight.GetComponent<SpriteRenderer>().size = obj.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f);
        highlight.GetComponent<BoxCollider2D>().size = obj.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f) - new Vector2(.1f, .1f);
    }
    public void DeselectObject()
    {
        selectedObj.GetComponent<Image>().material = unlitMat;
        selectedObj = null;
        hasSelected = false;

        highlight.SetActive(false);
    }
}
