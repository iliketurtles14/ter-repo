using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    public List<Sprite> outfitSprites = new List<Sprite>();
    public PrisonSelect prisonSelectScript;
    private Dictionary<List<Sprite>, string> charDict = new Dictionary<List<Sprite>, string>();

    public void OnOpen(string name, string type, string character)
    {
        MakeCharDict();
        npcName = name;
        npcType = type;
        npcCharacter = character;
        characterNum = CharacterEnumClass.GetCharacterInt(npcCharacter);

        transform.Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = npcName;

        transform.Find("SetButton").GetComponent<Image>().sprite = normalSetSprite;

        isOpen = true;
    }
    private void MakeCharDict()
    {
        charDict = new Dictionary<List<Sprite>, string>
        {
            { dataScript.RabbitSprites, "Rabbit" }, { dataScript.BaldEagleSprites, "BaldEagle" },
            { dataScript.LiferSprites, "Lifer" }, { dataScript.YoungBuckSprites, "YoungBuck" },
            { dataScript.OldTimerSprites, "OldTimer" }, { dataScript.BillyGoatSprites, "BillyGoat" },
            { dataScript.FrosephSprites, "Froseph" }, { dataScript.TangoSprites, "Tango" },
            { dataScript.MaruSprites, "Maru" }, { dataScript.BuddyWalkingSprites, "Buddy" },
            { dataScript.IceElfWalkingSprites, "IceElf" }, { dataScript.BlackElfWalkingSprites, "BlackElf" },
            { dataScript.YellowElfWalkingSprites, "YellowElf" }, { dataScript.PinkElfWalkingSprites, "PinkElf" },
            { dataScript.OrangeElfWalkingSprites, "OrangeElf" }, { dataScript. BrownElfWalkingSprites, "BrownElf" },
            { dataScript.WhiteElfWalkingSprites, "WhiteElf" }, { dataScript.GenieWalkingSprites, "Genie" },
            { dataScript.GuardElfWalkingSprites, "GuardElf" }, { dataScript.ConnellyWalkingSprites, "Connelly" },
            { dataScript.ElbrahWalkingSprites, "Elbrah" }, { dataScript.ChenWalkingSprites, "Chen" },
            { dataScript.PiersWalkingSprites, "Piers" }, { dataScript.MournWalkingSprites, "Mourn" },
            { dataScript.LazeeboiWalkingSprites, "Lazeeboi" }, { dataScript.BlondeWalkingSprites, "Blonde" },
            { dataScript.WaltonWalkingSprites, "Walton" }, { dataScript.ProwlerWalkingSprites, "Prowler" },
            { dataScript.CraneWalkingSprites, "Crane" }, { dataScript.HenchmanWalkingSprites, "Henchman" },
            { dataScript.ClintWalkingSprites, "Clint" }, { dataScript.CageWalkingSprites, "Cage" },
            { dataScript.SeanWalkingSprites, "Sean" }, { dataScript.AndyWalkingSprites, "Andy" },
            { dataScript.SoldierWalkingSprites, "Soldier" }
        };

    }
    private void Update()
    {
        if (!isOpen)
        {
            return;
        }

        if (dataScript == null)
        {
            dataScript = GetGivenData.instance.GetComponent<ApplyMainMenuData>();
        }
        var pair = charDict.ElementAt(characterNum);
        characterSprites = pair.Key;

        switch (prisonSelectScript.currentPrisonNPCOutfit)
        {
            case "Inmate":
                if (npcType == "Inmate")
                {
                    outfitSprites = dataScript.InmateOutiftSprites;
                }
                else if (npcType == "Guard")
                {
                    outfitSprites = dataScript.GuardOutfitSprites;
                }
                break;
            case "POW":
                if (npcType == "Inmate")
                {
                    outfitSprites = dataScript.POWOutfitWalkingSprites;
                }
                else if (npcType == "Guard")
                {
                    outfitSprites = dataScript.GuardOutfitSprites;
                }
                break;
            case "Elf":
                if (npcType == "Inmate")
                {
                    outfitSprites = dataScript.ElfOutfitWalkingSprites;
                }
                else if (npcType == "Guard")
                {
                    outfitSprites = dataScript.GuardElfOutfitWalkingSprites;
                }
                break;
            case "Prisoner":
                if (npcType == "Inmate")
                {
                    outfitSprites = dataScript.PrisonerOutfitWalkingSprites;
                }
                else if (npcType == "Guard")
                {
                    outfitSprites = dataScript.SoldierOutfitWalkingSprites;
                }
                break;
            case "Tux":
                if (npcType == "Inmate")
                {
                    outfitSprites = dataScript.TuxOutfitWalkingSprites;
                }
                else if (npcType == "Guard")
                {
                    outfitSprites = dataScript.HenchmanOutfitWalkingSprites;
                }
                break;
        }

        List<int> availableInmateChars = new List<int>();
        List<int> availableGuardChars = new List<int>();
        switch (prisonSelectScript.currentPrisonNPCBody)
        {
            case "Normal":
                for (int i = 0; i < 9; i++)
                {
                    availableInmateChars.Add(i);
                    availableGuardChars.Add(i);
                }
                break;
            case "SS":
                for (int i = 10; i < 18; i++)
                {
                    availableInmateChars.Add(i);
                }
                availableGuardChars.Add(18);
                break;
            case "ET":
                for (int i = 31; i < 34; i++)
                {
                    availableInmateChars.Add(i);
                }
                availableGuardChars.Add(34);
                break;
            case "DTAF":
                for (int i = 20; i < 29; i++)
                {
                    availableInmateChars.Add(i);
                }
                availableGuardChars.Add(29);
                break;
        }

        if ((npcType == "Inmate" && characterNum == availableInmateChars[0]) || (npcType == "Guard" && characterNum == availableGuardChars[0]))
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = false;
            transform.Find("LeftArrow").GetComponent<Button>().enabled = false;
        }
        else
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = true;
            transform.Find("LeftArrow").GetComponent<Button>().enabled = true;
        }

        if ((npcType == "Inmate" && characterNum == availableInmateChars[availableInmateChars.Count - 1]) || (npcType == "Guard" && characterNum == availableGuardChars[availableGuardChars.Count - 1]))
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
