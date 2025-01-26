using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmallMenu : MonoBehaviour
{
    public string npcName;
    public string npcType;
    public string npcCharacter;
    public int characterNum;
    private bool isOpen;
    public MouseCollisionOnButtons mouseCollisionScript;
    public NPCRename NPCRenameScript;
    public Canvas MainMenuCanvas;
    public Sprite clearSprite;
    public Sprite normalSetSprite;
    public Sprite pressedSetSprite;
    public Sprite sadSetSprite;

    public void OnOpen(string name, string type, string character)
    {
        npcName = name;
        npcType = type;
        npcCharacter = character;

        transform.Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = npcName;

        isOpen = true;
    }
    private void Update()
    {
        if (!isOpen)
        {
            return;
        }

        //get character num
        switch (npcCharacter)
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


        if (mouseCollisionScript.isTouchingButton && mouseCollisionScript.touchedButton.name == "LeftArrow" && Input.GetMouseButtonDown(0))
        {
            characterNum -= 1;
        }
        else if (mouseCollisionScript.isTouchingButton && mouseCollisionScript.touchedButton.name == "RightArrow" && Input.GetMouseButtonDown(0))
        {
            characterNum += 1;
        }
        switch (characterNum)
        {
            case 1: npcCharacter = "Rabbit"; break;
            case 2: npcCharacter = "BaldEagle"; break;
            case 3: npcCharacter = "Lifer"; break;
            case 4: npcCharacter = "YoungBuck"; break;
            case 5: npcCharacter = "OldTimer"; break;
            case 6: npcCharacter = "BillyGoat"; break;
            case 7: npcCharacter = "Froseph"; break;
            case 8: npcCharacter = "Tango"; break;
            case 9: npcCharacter = "Maru"; break;
        }

        if(mouseCollisionScript.isTouchingButton && mouseCollisionScript.touchedButton.name == "NameBox" && Input.GetMouseButtonDown(0))
        {
            transform.Find("NameText").GetComponent<TMP_InputField>().ActivateInputField();
            transform.Find("NameText").GetComponent<TMP_InputField>().caretPosition = 22;
        }
        else if((mouseCollisionScript.isTouchingButton && mouseCollisionScript.touchedButton.name != "NameBox" && Input.GetMouseButtonDown(0)) ||
            !mouseCollisionScript.isTouchingButton && Input.GetMouseButtonDown(0))
        {
            transform.Find("NameText").GetComponent<TMP_InputField>().DeactivateInputField();
        }

        if(mouseCollisionScript.isTouchingButton && mouseCollisionScript.touchedButton.name == "SetButton")
        {
            transform.Find("SetButton").GetComponent<Image>().sprite = pressedSetSprite;
        }
        else
        {
            transform.Find("SetButton").GetComponent<Image>().sprite = normalSetSprite;
        }

        if (!transform.Find("SetButton").GetComponent<BoxCollider2D>().enabled)
        {
            transform.Find("SetButton").GetComponent<Image>().sprite = sadSetSprite;
        }

        if(mouseCollisionScript.isTouchingButton && mouseCollisionScript.touchedButton.name == "SetButton" && Input.GetMouseButtonDown(0))
        {
            SetNPC(NPCRenameScript.pressedNPCName, transform.Find("NameText").GetComponent<TMP_InputField>().text, npcCharacter);
        }
    }
    private void SetNPC(string name, string displayName, string character)
    {
        MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NPCGrid").Find(name).GetComponent<NPCRenameAnim>().bodyDirSpritesPath = "NPC Sprites/" + character + "/Movement";
        MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NPCGrid").Find(name).GetComponent<CustomNPCCollectionData>().customNPCData.npcType = character;
        if (MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NPCGrid").Find(name).tag == "Inmate")
        {
            MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NPCGrid").Find(name).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = displayName;
            MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NameText").GetComponent<TextMeshProUGUI>().text = displayName;
        }
        else if(MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NPCGrid").Find(name).tag == "Guard")
        {
            MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NPCGrid").Find(name).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = "Officer " + displayName;
            MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NameText").GetComponent<TextMeshProUGUI>().text = "Officer " + displayName;
        }
    }
}
