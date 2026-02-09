using UnityEngine;

public class TooltipMouseFollow : MonoBehaviour
{
    public Canvas InventoryCanvas;
    private GameObject TooltipPanel;
    private GameObject TooltipTextBox;//TooltipText(Clone)
    private RectTransform TooltipPanelRectTransform;
    private RectTransform TooltipTextBoxRectTransform;
    private RectTransform MouseOverlayRectTransform;
    private GameObject MouseOverlayObject;
    private bool showingTooltip;
    public Tooltips tooltipScript;

    public void Start()
    {
        
    }
    public void Update()
    {
        //define vars
        if(InventoryCanvas.transform.Find("TooltipText(Clone)") != null)
        {
            TooltipTextBox = InventoryCanvas.transform.Find("TooltipText(Clone)").gameObject;
        }
        TooltipPanel = InventoryCanvas.transform.Find("TooltipPanel").gameObject;
        MouseOverlayObject = InventoryCanvas.transform.Find("MouseOverlay").gameObject;
        MouseOverlayRectTransform = MouseOverlayObject.GetComponent<RectTransform>();
        showingTooltip = tooltipScript.showingTooltip;


        //make it follow mouse overlay
        if (showingTooltip)
        {
            TooltipPanelRectTransform = TooltipPanel.GetComponent<RectTransform>();
            TooltipTextBoxRectTransform = TooltipTextBox.GetComponent<RectTransform>();

            Vector3 offset = new Vector3(1.46f, -.9f); //offset for the textbox
            TooltipPanelRectTransform.position = MouseOverlayRectTransform.position;
            TooltipTextBoxRectTransform.position = MouseOverlayRectTransform.position + offset;
        }
    }
}
