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
    public List<Sprite> difficultySprites = new List<Sprite>();
    public TextMeshProUGUI prisonText;
    public GameObject CurrentPrisonObject;
    public GameObject CurrentDifficultyObject;
    public GameObject LeftArrow;
    public GameObject RightArrow;
    private int whichPrison;
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
        CurrentDifficultyObject.GetComponent<Image>().sprite = difficultySprites[0];
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
            case 0: prisonText.text = "TUTORIAL"; break;
            case 1: prisonText.text = "CENTER PERKS"; break;
            case 2: prisonText.text = "STALAG FLUCHT"; break;
            case 3: prisonText.text = "SHANKTON STATE PEN"; break;
            case 4: prisonText.text = "JUNGLE COMPOUND"; break;
            case 5: prisonText.text = "SAN PANCHO"; break;
            case 6: prisonText.text = "HMP IRONGATE"; break;
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
                    CurrentDifficultyObject.GetComponent<Image>().sprite = difficultySprites[whichPrison];
                    break;
                case "RightArrow":
                    whichPrison += 1;
                    CurrentPrisonObject.GetComponent<Image>().sprite = prisonSprites[whichPrison];
                    CurrentDifficultyObject.GetComponent<Image>().sprite = difficultySprites[whichPrison];
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
