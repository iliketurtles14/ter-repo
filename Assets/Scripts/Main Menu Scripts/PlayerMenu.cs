using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    public MouseCollisionOnButtons mcs;
    public string playerName;
    public string playerCharacter;
    public int characterNum;
    public Canvas MainMenuCanvas;
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public string setName;
    public int setCharacter;

    public void OnEnable()
    {
        characterNum = Random.Range(1, 9);
        switch (characterNum)
        {
            case 1: playerCharacter = "Rabbit"; break;
            case 2: playerCharacter = "BaldEagle"; break;
            case 3: playerCharacter = "Lifer"; break;
            case 4: playerCharacter = "YoungBuck"; break;
            case 5: playerCharacter = "OldTimer"; break;
            case 6: playerCharacter = "BillyGoat"; break;
            case 7: playerCharacter = "Froseph"; break;
            case 8: playerCharacter = "Tango"; break;
            case 9: playerCharacter = "Maru"; break;
        }
        transform.Find("NameText").GetComponent<TMP_InputField>().text = playerCharacter;
        transform.Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = playerCharacter;
    }
    public void Update()
    {
        if(mcs.isTouchingButton && mcs.touchedButton.name == "LeftArrow" && Input.GetMouseButtonDown(0))
        {
            characterNum -= 1;
        }
        else if(mcs.isTouchingButton && mcs.touchedButton.name == "RightArrow" && Input.GetMouseButtonDown(0))
        {
            characterNum += 1;
        }
        switch (characterNum)
        {
            case 1: playerCharacter = "Rabbit"; break;
            case 2: playerCharacter = "BaldEagle"; break;
            case 3: playerCharacter = "Lifer"; break;
            case 4: playerCharacter = "YoungBuck"; break;
            case 5: playerCharacter = "OldTimer"; break;
            case 6: playerCharacter = "BillyGoat"; break;
            case 7: playerCharacter = "Froseph"; break;
            case 8: playerCharacter = "Tango"; break;
            case 9: playerCharacter = "Maru"; break;
        }
        switch (playerCharacter)
        {
            case "Rabbit": characterNum = 1; break;
            case "BaldEagle": characterNum = 2; break;
            case "Lifer": characterNum = 3; break;
            case "YoungBuck": characterNum = 4; break;
            case "OldTimer": characterNum = 5; break;
            case "BillyGoat": characterNum = 6; break;
            case "Froseph": characterNum = 7; break;
            case "Tango": characterNum = 8; break;
            case "Maru": characterNum = 9; break;
        }
        if (characterNum == 1)
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = false;
            transform.Find("LeftArrow").GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = true;
            transform.Find("LeftArrow").GetComponent<BoxCollider2D>().enabled = true;
        }

        if (characterNum == 9)
        {
            transform.Find("RightArrow").GetComponent<Image>().enabled = false;
            transform.Find("RightArrow").GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            transform.Find("RightArrow").GetComponent<Image>().enabled = true;
            transform.Find("RightArrow").GetComponent<BoxCollider2D>().enabled = true;
        }
        
        if(mcs.isTouchingButton && (mcs.touchedButton.name == "LeftArrow" || mcs.touchedButton.name == "RightArrow") && Input.GetMouseButtonDown(0))
        {
            transform.Find("NameText").GetComponent<TMP_InputField>().text = playerCharacter;
            transform.Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = playerCharacter;
        }

        if(mcs.isTouchingButton && mcs.touchedButton.name == "NameBox" && Input.GetMouseButtonDown(0))
        {
            transform.Find("NameText").GetComponent<TMP_InputField>().ActivateInputField();
            transform.Find("NameText").GetComponent<TMP_InputField>().caretPosition = 22;
        }
        else if((mcs.isTouchingButton && mcs.touchedButton.name != "NameBox" && Input.GetMouseButtonDown(0)) ||
            !mcs.isTouchingButton && Input.GetMouseButtonDown(0))
        {
            transform.Find("NameText").GetComponent<TMP_InputField>().DeactivateInputField();
        }

        if(mcs.isTouchingButton && (mcs.touchedButton.name == "BackButton" || mcs.touchedButton.name == "ContinueButton"))
        {
            mcs.touchedButton.GetComponent<Image>().sprite = pressedSprite;
            if (Input.GetMouseButtonDown(0) && mcs.touchedButton.name == "BackButton")
            {
                MainMenuCanvas.transform.Find("PrisonSelectPanel").gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
            else if(Input.GetMouseButtonDown(0) && mcs.touchedButton.name == "ContinueButton")
            {
                setName = transform.Find("NameText").GetComponent<TMP_InputField>().text;
                setCharacter = characterNum;
                
                MainMenuCanvas.transform.Find("NPCCustomizePanel").gameObject.SetActive(true);
                MainMenuCanvas.transform.Find("SmallMenuPanel").gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        else
        {
            transform.Find("BackButton").GetComponent<Image>().sprite = normalSprite;
            transform.Find("ContinueButton").GetComponent<Image>().sprite = normalSprite;
        }
    }
}
