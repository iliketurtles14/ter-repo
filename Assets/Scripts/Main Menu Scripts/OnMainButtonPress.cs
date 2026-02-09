using UnityEngine;
using UnityEngine.UI;

public class OnMainButtonPress : MonoBehaviour
{
    public MouseCollisionOnButtons mouseCollisionScript;
    public NPCRename NPCRenameScript;
    public Sprite ButtonNormalSprite;
    public Sprite ButtonPressedSprite;
    private GameObject lastTouchedButton;
    private bool touchingPlayButton;
    private bool touchingOptionsButton;
    private bool touchingMapEditorButton;
    public Canvas MainMenuCanvas;
    public bool isPrisonSelectPanelOpen;


    private void Start()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        lastTouchedButton = null;
        MainMenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = false;
        MainMenuCanvas.transform.Find("PrisonSelectPanel").gameObject.SetActive(false);
        MainMenuCanvas.transform.Find("NPCCustomizePanel").gameObject.SetActive(false);
        MainMenuCanvas.transform.Find("SmallMenuPanel").gameObject.SetActive(false);
        MainMenuCanvas.transform.Find("PlayerPanel").gameObject.SetActive(false);
        isPrisonSelectPanelOpen = false;
        if (NPCRenameScript.comingFromRename)
        {
            MainMenuCanvas.transform.Find("PrisonSelectPanel").gameObject.SetActive(true);
            MainMenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = true;
            isPrisonSelectPanelOpen = true;
            foreach (Transform child in transform)
            {
                if (child.GetComponent<BoxCollider2D>() != null)
                {
                    child.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            NPCRenameScript.comingFromRename = false;
        }
    }
    private void Update()
    {
        if (mouseCollisionScript.isTouchingButton)
        {
            switch (mouseCollisionScript.touchedButton.name)
            {
                case "PlayButton":
                    mouseCollisionScript.touchedButton.GetComponent<Image>().sprite = ButtonPressedSprite;
                    lastTouchedButton = mouseCollisionScript.touchedButton;
                    touchingPlayButton = true;
                    break;
                case "OptionsButton":
                    mouseCollisionScript.touchedButton.GetComponent<Image>().sprite = ButtonPressedSprite;
                    lastTouchedButton = mouseCollisionScript.touchedButton;
                    touchingOptionsButton = true;
                    break;
                case "MapEditorButton":
                    mouseCollisionScript.touchedButton.GetComponent<Image>().sprite = ButtonPressedSprite;
                    lastTouchedButton = mouseCollisionScript.touchedButton;
                    touchingMapEditorButton = true;
                    break;
            }
        }
        else
        {
            touchingPlayButton = false;
            touchingOptionsButton = false;
            touchingMapEditorButton = false;
            if (lastTouchedButton != null) { lastTouchedButton.GetComponent<Image>().sprite = ButtonNormalSprite; }
            lastTouchedButton = null;

        }

        if(touchingPlayButton && Input.GetMouseButtonDown(0))
        {
            MainMenuCanvas.transform.Find("PrisonSelectPanel").gameObject.SetActive(true);
            MainMenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = true;
            isPrisonSelectPanelOpen = true;
            foreach(Transform child in transform)
            {
                if(child.GetComponent<BoxCollider2D>() != null)
                {
                    child.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}
