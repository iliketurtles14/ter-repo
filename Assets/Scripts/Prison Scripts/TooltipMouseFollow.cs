using UnityEngine;

public class TooltipMouseFollow : MonoBehaviour
{
    private GameObject InventoryCanvas;
    private GameObject TooltipPanel;
    private GameObject TooltipTextBox;//TooltipText(Clone)
    private RectTransform TooltipPanelRectTransform;
    private RectTransform TooltipTextBoxRectTransform;
    private RectTransform MouseOverlayRectTransform;
    private GameObject MouseOverlayObject;
    private bool showingTooltip;
    private Tooltips tooltipScript;

    public void Start()
    {
        InventoryCanvas = RootObjectCache.GetRoot("InventoryCanvas");
        tooltipScript = GetComponent<Tooltips>();
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
