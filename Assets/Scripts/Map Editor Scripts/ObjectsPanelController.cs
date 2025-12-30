using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPanelController : MonoBehaviour
{
    public int objectPanelNum = 0;
    private Dictionary<int, string> panelNameDict = new Dictionary<int, string> //relates the objectPanelNum to what the panel's name is
    {
        { 0, "WaypointsPanel" }, { 1, "CellsPanel" }, { 2, "SecurityPanel" },
        { 3, "JobsPanel" }, { 4, "GymPanel" }, { 5, "MiscPanel" }, { 6, "DoorsPanel" },
        { 7, "ZiplinePanel" }, { 8, "SpecialPanel" }, { 9, "ETPanel" }, { 10, "DTAF1Panel" },
        { 11, "DTAF2Panel" }, { 12, "Christmas1Panel" }, { 13, "Christmas2Panel" }, { 14, "ItemsPanel" }
    };
    private List<Transform> panelList = new List<Transform>();
    public Transform uic;
    public int oldObjectPanelNum;
    public bool canBeShown = false;
    private bool hasHiddenPanels;
    public string currentObjectPanel;
    private void Update()
    {
        currentObjectPanel = panelNameDict[objectPanelNum];
    }
    private void Start()
    {
        for(int i = 0; i < 15; i++) //lol just didnt wanna code out adding all the panels
        {
            panelList.Add(uic.Find(panelNameDict[i]));
        }
        StartCoroutine(ObjectsPanelLoop());
    }
    private IEnumerator ObjectsPanelLoop()
    {
        while (true)
        {
            if (!canBeShown)
            {
                if (!hasHiddenPanels)
                {
                    HideObjectPanel();
                }
                yield return null;
                continue;
            }
            oldObjectPanelNum = objectPanelNum;
            yield return new WaitForEndOfFrame();
            if(oldObjectPanelNum != objectPanelNum && canBeShown)
            {
                oldObjectPanelNum = objectPanelNum;
                ChangeObjectPanel(objectPanelNum);
            }
            else if (canBeShown)
            {
                ChangeObjectPanel(objectPanelNum);
            }
        }
    }
    private void HideObjectPanel()
    {
        foreach(Transform panel in panelList)
        {
            panel.gameObject.SetActive(false);
        }
        uic.Find("ObjectsArrowsPanel").gameObject.SetActive(false);
        hasHiddenPanels = true;
    }
    private void ChangeObjectPanel(int panelNum)
    {
        foreach(Transform panel in panelList)
        {
            if(panel.name == panelNameDict[panelNum])
            {
                panel.gameObject.SetActive(true);
            }
            else
            {
                panel.gameObject.SetActive(false);
            }
        }
        uic.Find("ObjectsArrowsPanel").gameObject.SetActive(true);
        hasHiddenPanels = false;
    }
}
