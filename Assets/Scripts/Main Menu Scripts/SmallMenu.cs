using NUnit.Framework;
using System.Collections.Generic;
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
    public ApplyMainMenuData dataScript;
    public Canvas MainMenuCanvas;
    public Sprite clearSprite;
    public Sprite normalSetSprite;
    public Sprite pressedSetSprite;
    public Sprite sadSetSprite;
    public List<Sprite> characterSprites = new List<Sprite>();

    public void OnOpen(string name, string type, string character)
    {
        npcName = name;
        npcType = type;
        npcCharacter = character;
        characterNum = CharacterEnumClass.GetCharacterInt(npcCharacter);

        transform.Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = npcName;

        transform.Find("SetButton").GetComponent<Image>().sprite = normalSetSprite;

        isOpen = true;
    }
    private void Update()
    {
        if (!isOpen)
        {
            return;
        }

        switch (characterNum)
        {
            case 0:
                characterSprites = dataScript.RabbitSprites;
                break;
            case 1:
                characterSprites = dataScript.BaldEagleSprites;
                break;
            case 2:
                characterSprites = dataScript.LiferSprites;
                break;
            case 3:
                characterSprites = dataScript.YoungBuckSprites;
                break;
            case 4:
                characterSprites = dataScript.OldTimerSprites;
                break;
            case 5:
                characterSprites = dataScript.BillyGoatSprites;
                break;
            case 6:
                characterSprites = dataScript.FrosephSprites;
                break;
            case 7:
                characterSprites = dataScript.TangoSprites;
                break;
            case 8:
                characterSprites = dataScript.MaruSprites;
                break;
        }

        if (characterNum == 0)
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = false;
            transform.Find("LeftArrow").GetComponent<Button>().enabled = false;
        }
        else
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = true;
            transform.Find("LeftArrow").GetComponent<Button>().enabled = true;
        }

        if (characterNum == 8)
        {
            transform.Find("RightArrow").GetComponent<Image>().enabled = false;
            transform.Find("RightArrow").GetComponent<Button>().enabled = false;
        }
        else
        {
            transform.Find("RightArrow").GetComponent<Image>().enabled = true;
            transform.Find("RightArrow").GetComponent<Button>().enabled = true;
        }

        npcCharacter = CharacterEnumClass.GetCharacterString(characterNum);

        if (!transform.Find("SetButton").GetComponent<Button>().enabled)
        {
            transform.Find("SetButton").GetComponent<Image>().sprite = sadSetSprite;
        }
    }
    public void SetNPC(string name, string displayName, string character)
    {
        MainMenuCanvas.transform.Find("NPCCustomizePanel").Find("NPCGrid").Find(name).GetComponent<NPCRenameAnim>().bodyDirSprites = characterSprites;
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
