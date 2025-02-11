using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrisonSelect : MonoBehaviour
{
    public MouseCollisionOnButtons mouseCollisionScript;
    public List<Sprite> prisonSprites = new List<Sprite>();
    public TextMeshProUGUI prisonText;
    public TextMeshProUGUI difficultyText;
    public GameObject CurrentPrisonObject;
    public GameObject CurrentDifficultyObject;
    public GameObject LeftArrow;
    public GameObject RightArrow;
    public int whichPrison;
    public GameObject TitlePanel;
    public GameObject PrisonSelectPanel;
    public Sprite ButtonNormalSprite;
    public Sprite ButtonPressedSprite;
    private GameObject lastTouchedButton;
    public OnMainButtonPress titlePanelScript;
    public Canvas MainMenuCanvas;

    public void Start()
    {
        whichPrison = 0;
        CurrentPrisonObject.GetComponent<Image>().sprite = prisonSprites[0];
        LeftArrow.GetComponent<Image>().enabled = false;
        LeftArrow.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void Update()
    {

        if(whichPrison == 0)
        {
            LeftArrow.GetComponent<Image>().enabled = false;
            LeftArrow.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            LeftArrow.GetComponent<Image>().enabled = true;
            LeftArrow.GetComponent<BoxCollider2D>().enabled = true;
        }

        if (whichPrison == 6)
        {
            RightArrow.GetComponent<Image>().enabled = false;
            RightArrow.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            RightArrow.GetComponent<Image>().enabled = true;
            RightArrow.GetComponent<BoxCollider2D>().enabled = true;
        }

        switch (whichPrison)
        {
            case 0:
                prisonText.text = "TUTORIAL";
                difficultyText.text = "";
                break;
            case 1:
                prisonText.text = "CENTER PERKS";
                difficultyText.text = "(Very Easy)";
                difficultyText.color = new Color(12f / 255f, 255f / 255f, 0f / 255f); // Green color
                break;
            case 2:
                prisonText.text = "STALAG FLUCHT";
                difficultyText.text = "(Easy)";
                difficultyText.color = new Color(9f / 255f, 180f / 255f, 0f / 255f); // Dark Green color
                break;
            case 3:
                prisonText.text = "SHANKTON STATE PEN";
                difficultyText.text = "(Moderate)";
                difficultyText.color = new Color(255f / 255f, 191f / 255f, 0f / 255f); // Orange color
                break;
            case 4:
                prisonText.text = "JUNGLE COMPOUND";
                difficultyText.text = "(Moderate)";
                difficultyText.color = new Color(255f / 255f, 191f / 255f, 0f / 255f); // Orange color
                break;
            case 5:
                prisonText.text = "SAN PANCHO";
                difficultyText.text = "(Hard)";
                difficultyText.color = new Color(255f / 255f, 63f / 255f, 0f / 255f); // Red-Orange color
                break;
            case 6:
                prisonText.text = "HMP IRONGATE";
                difficultyText.text = "(Very Hard)";
                difficultyText.color = new Color(166f / 255f, 0f / 255f, 0f / 255f); // Dark Red color
                break;
        }

        if (mouseCollisionScript.isTouchingButton)
        {
            switch (mouseCollisionScript.touchedButton.name)
            {
                case "BackButton":
                    mouseCollisionScript.touchedButton.GetComponent<Image>().sprite = ButtonPressedSprite;
                    lastTouchedButton = mouseCollisionScript.touchedButton;
                    break;
                case "ContinueButton":
                    mouseCollisionScript.touchedButton.GetComponent<Image>().sprite = ButtonPressedSprite;
                    lastTouchedButton = mouseCollisionScript.touchedButton;
                    break;
            }
        }
        else
        {
            if(lastTouchedButton != null)
            {
                lastTouchedButton.GetComponent<Image>().sprite = ButtonNormalSprite;
            }
            lastTouchedButton = null;
        }
        if(mouseCollisionScript.isTouchingButton && Input.GetMouseButtonDown(0))
        {
            switch (mouseCollisionScript.touchedButton.name)
            {
                case "LeftArrow":
                    whichPrison -= 1;
                    CurrentPrisonObject.GetComponent<Image>().sprite = prisonSprites[whichPrison];
                    break;
                case "RightArrow":
                    whichPrison += 1;
                    CurrentPrisonObject.GetComponent<Image>().sprite = prisonSprites[whichPrison];
                    break;
                case "BackButton":
                    PrisonSelectPanel.SetActive(false);
                    titlePanelScript.isPrisonSelectPanelOpen = false;
                    MainMenuCanvas.transform.Find("Black").GetComponent<Image>().enabled = false;
                    foreach(Transform child in TitlePanel.transform)
                    {
                        if (child.GetComponent<BoxCollider2D>() != null)
                        {
                            child.GetComponent<BoxCollider2D>().enabled = true;
                        }
                    }
                    break;
                case "ContinueButton":
                    MainMenuCanvas.transform.Find("PlayerPanel").gameObject.SetActive(true);
                    PrisonSelectPanel.SetActive(false);
                    break;
            }
        }
    }
}
