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
        if(mcs.isTouchingObject && Input.GetMouseButtonDown(0))
        {
            if(selectedObj != null) 
            {
                selectedObj.GetComponent<Image>().material = unlitMat;
            }
            SelectObject(mcs.touchedObject);
        }
    }
    public void SelectObject(GameObject obj)
    {
        obj.GetComponent<Image>().material = outlineMat;
        selectedObj = obj;
        hasSelected = true;

        highlight.GetComponent<SpriteRenderer>().size = obj.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f);
        highlight.GetComponent<BoxCollider2D>().size = obj.GetComponent<BoxCollider2D>().size / new Vector2(50f, 50f) - new Vector2(.1f, .1f);
    }
}
