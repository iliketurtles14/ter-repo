using UnityEngine;
using UnityEngine.UIElements;

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
    public void Start()
    {
        iniFile = new IniFile(System.IO.Path.Combine(Application.streamingAssetsPath, "CTFAK", "config.ini"));

        if (iniFile.Read("NormalizePlayerMovement", "Settings") == "true")
        {
            normalizePlayerMovement = true;
        }
        else if(iniFile.Read("NormalizePlayerMovement", "Settings") == "false")
        {
            normalizePlayerMovement = false;
        }
        
        foreach (Transform child in transform)
        {
            if(child.name == "NormalizeCheckBox")
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
        
    }
}
