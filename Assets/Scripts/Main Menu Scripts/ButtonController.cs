using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ButtonController : MonoBehaviour
{
    public Transform mmc;
    public Sprite checkedBoxSprite;
    public Sprite uncheckedBoxSprite;
    private IniFile iniFile;
    private int whichPrison;
    public PrisonSelect prisonSelectScript;
    public PlayerMenu playerMenuScript;
    public SmallMenu smallMenuScript;
    public NPCRename npcRenameScript;
    public void MainPlayGame()
    {
        mmc.Find("PrisonSelectPanel").gameObject.SetActive(true);
        mmc.Find("Black").GetComponent<Image>().enabled = true;
        foreach(Transform child in mmc.Find("TitlePanel"))
        {
            if(child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().enabled = false;
                child.GetComponent<EventTrigger>().enabled = false;
            }
        }
    }
    public void MainPatchNotes()
    {
        mmc.Find("PatchNotesPanel").gameObject.SetActive(true);
        mmc.Find("Black").GetComponent<Image>().enabled = true;
        foreach (Transform child in mmc.Find("TitlePanel"))
        {
            if (child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().enabled = false;
                child.GetComponent<EventTrigger>().enabled = false;
            }
        }
    }
    public void MainOptions()
    {
        Transform optionsPanel = mmc.Find("OptionsPanel");
        
        optionsPanel.gameObject.SetActive(true);
        mmc.Find("Black").GetComponent<Image>().enabled = true;
        foreach (Transform child in mmc.Find("TitlePanel"))
        {
            if (child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().enabled = false;
                child.GetComponent<EventTrigger>().enabled = false;
            }
        }

        //load config file
        iniFile = new IniFile(Path.Combine(Application.streamingAssetsPath, "CTFAK", "config.ini"));
    
        foreach(Transform child in optionsPanel)
        {
            if(child.name == "NormalizeCheckBox")
            {
                if(iniFile.Read("NormalizePlayerMovement", "Settings") == "true")
                {
                    child.GetComponent<Image>().sprite = checkedBoxSprite;
                }
                else
                {
                    child.GetComponent<Image>().sprite = uncheckedBoxSprite;
                }
            }
        }
    }
    public void MainMapEditor()
    {
        foreach (Transform child in mmc.Find("TitlePanel"))
        {
            if (child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().enabled = false;
                child.GetComponent<EventTrigger>().enabled = false;
            }
        }

        Addressables.LoadSceneAsync("Map Editor");
    }
    public void PatchBack()
    {
        foreach(Transform child in mmc.Find("TitlePanel"))
        {
            if(child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().enabled = true;
                child.GetComponent<EventTrigger>().enabled = true;
            }
        }
        mmc.Find("Black").GetComponent<Image>().enabled = false;
        mmc.Find("PatchNotesPanel").gameObject.SetActive(false);
    }
    public void OptionsNormalize()
    {
        Transform normalizeBox = mmc.Find("OptionsPanel").Find("NormalizeCheckBox");

        if(normalizeBox.GetComponent<Image>().sprite == checkedBoxSprite)
        {
            normalizeBox.GetComponent<Image>().sprite = uncheckedBoxSprite;
        }
        else
        {
            normalizeBox.GetComponent<Image>().sprite = checkedBoxSprite;
        }
    }
    public void OptionsSave()
    {
        foreach(Transform child in mmc.Find("OptionsPanel"))
        {
            if(child.name == "NormalizeCheckBox")
            {
                if(child.GetComponent<Image>().sprite == checkedBoxSprite)
                {
                    iniFile.Write("NormalizePlayerMovement", "true", "Settings");
                }
                else
                {
                    iniFile.Write("NormalizePlayerMovement", "false", "Settings");
                }
            }
        }
    }
    public void OptionsBack()
    {
        mmc.Find("OptionsPanel").gameObject.SetActive(false);
        mmc.Find("Black").GetComponent<Image>().enabled = false;
        foreach(Transform child in mmc.Find("TitlePanel"))
        {
            if(child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().enabled = true;
            }
        }
    }
    public void PrisonSelectLeft()
    {
        prisonSelectScript.whichPrison--;
    }
    public void PrisonSelectRight()
    {
        prisonSelectScript.whichPrison++;
    }
    public void PrisonSelectBack()
    {
        mmc.Find("PrisonSelectPanel").gameObject.SetActive(false);
        mmc.Find("Black").GetComponent<Image>().enabled = false;
        foreach(Transform child in mmc.Find("TitlePanel"))
        {
            if(child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().enabled = true;
            }
        }
    }
    public void PrisonSelectContinue()
    {
        mmc.Find("PlayerPanel").gameObject.SetActive(true);
        mmc.Find("PrisonSelectPanel").gameObject.SetActive(false);
    }
    public void PlayerMenuLeft()
    {
        playerMenuScript.characterNum--;
        mmc.Find("PlayerMenu").Find("NameText").GetComponent<TMP_InputField>().text = playerMenuScript.playerCharacter;
        mmc.Find("PlayerMenu").Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = playerMenuScript.playerCharacter;
    }
    public void PlayerMenuRight()
    {
        playerMenuScript.characterNum++;
        mmc.Find("PlayerMenu").Find("NameText").GetComponent<TMP_InputField>().text = playerMenuScript.playerCharacter;
        mmc.Find("PlayerMenu").Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = playerMenuScript.playerCharacter;
    }
    public void PlayerMenuBack()
    {
        mmc.Find("PrisonSelectPanel").gameObject.SetActive(true);
        mmc.Find("PlayerMenu").gameObject.SetActive(false);
    }
    public void PlayerMenuContinue()
    {
        string setName = mmc.Find("PlayerMenu").Find("NameText").GetComponent<TMP_InputField>().text;
        int setCharacter = playerMenuScript.characterNum;
        playerMenuScript.saveScript.SetPlayer(setName, setCharacter);

        mmc.Find("NPCCustomizePanel").gameObject.SetActive(true);
        mmc.Find("SmallMenuPanel").gameObject.SetActive(true);
        mmc.Find("PlayerMenu").gameObject.SetActive(false);
    }
    public void SmallMenuLeft()
    {
        smallMenuScript.characterNum--;
    }
    public void SmallMenuRight()
    {
        smallMenuScript.characterNum++;
    }
    public void SmallMenuSet()
    {
        string name = smallMenuScript.NPCRenameScript.pressedNPCName;
        string displayName = mmc.Find("SmallMenuPanel").Find("NameText").GetComponent<TMP_InputField>().text;
        string character = smallMenuScript.npcCharacter;

        smallMenuScript.SetNPC(name, displayName, character);
    }
    public void NPCRenameBack()
    {
        mmc.Find("PlayerPanel").gameObject.SetActive(true);
        mmc.Find("SmallMenuPanel").gameObject.SetActive(false);
        mmc.Find("NPCCUstomizePanel").gameObject.SetActive(false);
    }
    public void NPCRenameStart()
    {
        if (!npcRenameScript.isStarting)
        {
            npcRenameScript.isStarting = true;
            npcRenameScript.Transfer();
        }
    }
    public void NPCRenameRandom()
    {
        StartCoroutine(npcRenameScript.RandomizeWait());
    }
    public void NPCRenameNPC()
    {
        npcRenameScript.OnNPCClick();
    }
}
