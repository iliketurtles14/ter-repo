using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public MouseCollisionOnButtons mcs;
    public GameObject titlePanel;
    public GameObject black;
    public Sprite backButtonNormalSprite;
    public Sprite backButtonPressedSprite;
    private GameObject lastTouchedButton;
    public Sprite checkedBoxSprite;
    public Sprite uncheckedBoxSprite;

    public bool normalizePlayerMovement;

    private IniFile iniFile;
    public void OnEnable()
    {
        iniFile = new IniFile(System.IO.Path.Combine(Application.streamingAssetsPath, "CTFAK", "config.ini"));

        if (iniFile.Read("NormalizePlayerMovement", "Settings") == "true")
        {
            normalizePlayerMovement = true;
        }
        else if (iniFile.Read("NormalizePlayerMovement", "Settings") == "false")
        {
            normalizePlayerMovement = false;
        }

        foreach (Transform child in transform)
        {
            if (child.name == "NormalizeCheckBox")
            {
                if (normalizePlayerMovement)
                {
                    child.GetComponent<Image>().sprite = checkedBoxSprite;
                }
                else if (!normalizePlayerMovement)
                {
                    child.GetComponent<Image>().sprite = uncheckedBoxSprite;
                }
            }
        }
    }
    public void Update()
    {
        if(mcs.isTouchingButton && (mcs.touchedButton.name == "BackButton" || mcs.touchedButton.name == "SaveButton"))
        {
            mcs.touchedButton.GetComponent<Image>().sprite = backButtonPressedSprite; //both the save and back buttons use this sprite
            lastTouchedButton = mcs.touchedButton;
            if(mcs.touchedButton.name == "BackButton" && Input.GetMouseButtonDown(0))
            {
                foreach(Transform child in titlePanel.transform)
                {
                    if(child.GetComponent<BoxCollider2D>() != null)
                    {
                        child.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
                black.GetComponent<Image>().enabled = false;
                gameObject.SetActive(false);
            }
            else if(mcs.touchedButton.name == "SaveButton" && Input.GetMouseButtonDown(0))
            {
                Save();
            }
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "NormalizeCheckBox" && Input.GetMouseButtonDown(0))
        {
            if(mcs.touchedButton.GetComponent<Image>().sprite == checkedBoxSprite)
            {
                mcs.touchedButton.GetComponent<Image>().sprite = uncheckedBoxSprite;
            }
            else if(mcs.touchedButton.GetComponent<Image>().sprite == uncheckedBoxSprite)
            {
                mcs.touchedButton.GetComponent<Image>().sprite = checkedBoxSprite;
            }
        }
        // vvv (keep at bottom) vvv
        else if(!mcs.isTouchingButton && lastTouchedButton != null && (lastTouchedButton.name == "BackButton" || lastTouchedButton.name == "SaveButton"))
        {
            lastTouchedButton.GetComponent<Image>().sprite = backButtonNormalSprite;
        }
    }
    public void Save()
    {
        foreach(Transform child in transform)
        {
            if(child.name == "NormalizeCheckBox")
            {
                if(child.GetComponent<Image>().sprite == checkedBoxSprite)
                {
                    normalizePlayerMovement = true;
                    iniFile.Write("NormalizePlayerMovement", "true", "Settings");
                }
                else if(child.GetComponent<Image>().sprite == uncheckedBoxSprite)
                {
                    normalizePlayerMovement = false;
                    iniFile.Write("NormalizePlayerMovement", "false", "Settings");
                }
            }
        }
    }
}
