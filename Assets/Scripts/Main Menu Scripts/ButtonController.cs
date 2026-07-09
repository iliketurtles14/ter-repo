using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public Transform mmc;
    public Sprite checkedBoxSprite;
    public Sprite uncheckedBoxSprite;
    private IniFile iniFile;
    private int whichPrison;
    public PrisonSelect prisonSelectScript;
    public PlayerMenu PlayerMenuScript;
    public SmallMenu smallMenuScript;
    public NPCRename npcRenameScript;
    public Warnings warningsScript;
    public MMSoundController sc;
    public Credits creditsScript;
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

        sc.PlaySound("open");
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

        sc.PlaySound("open");
    }
    public void MainCredits()
    {
        StartCoroutine(creditsScript.StartCredits());
        sc.PlaySound("rumble");
    }
    public void MainOptions()
    {
        mmc.Find("OptionsPanel").GetComponent<Options>().Open("gameplay");
        sc.PlaySound("open");
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

        sc.PlaySound("rumble");

        Addressables.LoadSceneAsync("Map Editor");
    }
    public void MainQuit()
    {
#if     UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
        sc.PlaySound("close");
    }
    public void OptionsCheckBox(BaseEventData data)
    {
        var pd = data as PointerEventData;
        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;

        if(clicked.GetComponent<Image>().sprite == checkedBoxSprite)
        {
            clicked.GetComponent<Image>().sprite = uncheckedBoxSprite;
        }
        else
        {
            clicked.GetComponent<Image>().sprite = checkedBoxSprite;
        }

        sc.PlaySound("plip");
    }
    public void OptionsSave()
    {
        mmc.Find("OptionsPanel").GetComponent<Options>().Save();
        sc.PlaySound("plip");
    }
    public void OptionsBack()
    {
        mmc.Find("OptionsPanel").GetComponent<Options>().Close();
        sc.PlaySound("close");
    }
    public void OptionsGameplay()
    {
        mmc.Find("OptionsPanel").GetComponent<Options>().Open("gameplay");
        sc.PlaySound("plip");
    }
    public void OptionsVisual()
    {
        mmc.Find("OptionsPanel").GetComponent<Options>().Open("visual");
        sc.PlaySound("plip");
    }
    public void OptionsAudio()
    {
        mmc.Find("OptionsPanel").GetComponent<Options>().Open("audio");
        sc.PlaySound("plip");
    }
    public void PrisonSelectLeft()
    {
        prisonSelectScript.whichPrison--;
        sc.PlaySound("plip");
    }
    public void PrisonSelectRight()
    {
        prisonSelectScript.whichPrison++;
        sc.PlaySound("plip");
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
                child.GetComponent<EventTrigger>().enabled = true;
            }
        }

        sc.PlaySound("close");
    }
    public void PrisonSelectContinue()
    {
        mmc.Find("PlayerPanel").gameObject.SetActive(true);
        mmc.Find("PrisonSelectPanel").gameObject.SetActive(false);
        sc.PlaySound("open");
    }
    public void PrisonSelectMainPrisons()
    {
        prisonSelectScript.whichTab = 0;
        prisonSelectScript.whichPrison = 0;
        StartCoroutine(prisonSelectScript.ReloadGrid());
        sc.PlaySound("plip");
    }
    public void PrisonSelectBonusPrisons()
    {
        prisonSelectScript.whichTab = 1;
        prisonSelectScript.whichPrison = 0;
        StartCoroutine(prisonSelectScript.ReloadGrid());
        sc.PlaySound("plip");
    }
    public void PrisonSelectCustomPrisons()
    {
        prisonSelectScript.whichTab = 2;
        prisonSelectScript.whichPrison = 0;
        StartCoroutine(prisonSelectScript.ReloadGrid());
        sc.PlaySound("plip");
    }
    public void PrisonSelectReloadCustomPrisons()
    {
        prisonSelectScript.ReloadPrisons(true);
        sc.PlaySound("plip");
    }
    public void PrisonSelectFile()
    {
        string customPrisonsPath = Path.Combine(Application.streamingAssetsPath, "Prisons", "CustomPrisons");

        if (!Directory.Exists(customPrisonsPath))
        {
            Directory.CreateDirectory(customPrisonsPath);
        }

        try
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = customPrisonsPath,
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(startInfo);
            }
            else if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor)
            {
                System.Diagnostics.Process.Start("xdg-open", customPrisonsPath);
            }
            else
            {
                Debug.LogWarning("Opening file explorer is not supported on this platform.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open file explorer: " + e.Message);
        }
        sc.PlaySound("plip");
    }
    public void PrisonSelectSingleView()
    {
        StartCoroutine(prisonSelectScript.HideGrid());
        sc.PlaySound("plip");
    }
    public void PrisonSelectMultiView()
    {
        StartCoroutine(prisonSelectScript.ShowGrid());
        sc.PlaySound("plip");
    }
    public void PrisonSelectGridPrisonSelect(BaseEventData data)
    {
        var pd = data as PointerEventData;
        if(pd == null)
        {
            return;
        }

        var clicked = pd.pointerPress ?? pd.pointerCurrentRaycast.gameObject ?? gameObject;
        prisonSelectScript.SelectGridPrison(clicked);
        sc.PlaySound("plip");
    }
    public void PlayerMenuLeft()
    {
        sc.PlaySound("plip");
        StartCoroutine(PlayerMenuLeftWait());
    }
    private IEnumerator PlayerMenuLeftWait()
    {
        PlayerMenuScript.characterNum--;
        yield return new WaitForEndOfFrame();
        mmc.Find("PlayerPanel").Find("NameText").GetComponent<TMP_InputField>().text = PlayerMenuScript.playerCharacter;
        mmc.Find("PlayerPanel").Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = PlayerMenuScript.playerCharacter;
    }
    public void PlayerMenuRight()
    {
        sc.PlaySound("plip");
        StartCoroutine(PlayerMenuRightWait());
    }
    private IEnumerator PlayerMenuRightWait()
    {
        PlayerMenuScript.characterNum++;
        yield return new WaitForEndOfFrame();
        mmc.Find("PlayerPanel").Find("NameText").GetComponent<TMP_InputField>().text = PlayerMenuScript.playerCharacter;
        mmc.Find("PlayerPanel").Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = PlayerMenuScript.playerCharacter;
    }
    public void PlayerMenuBack()
    {
        mmc.Find("PrisonSelectPanel").gameObject.SetActive(true);
        mmc.Find("PlayerPanel").gameObject.SetActive(false);
        sc.PlaySound("close");
    }
    public void PlayerMenuContinue()
    {
        string setName = mmc.Find("PlayerPanel").Find("NameText").GetComponent<TMP_InputField>().text;
        int setCharacter = PlayerMenuScript.characterNum;
        PlayerMenuScript.saveScript.SetPlayer(setName, setCharacter);

        mmc.Find("NPCCustomizePanel").gameObject.SetActive(true);
        mmc.Find("SmallMenuPanel").gameObject.SetActive(true);
        mmc.Find("PlayerPanel").gameObject.SetActive(false);
        sc.PlaySound("open");
    }
    public void SmallMenuLeft()
    {
        smallMenuScript.characterNum--;
        sc.PlaySound("plip");
    }
    public void SmallMenuRight()
    {
        smallMenuScript.characterNum++;
        sc.PlaySound("plip");
    }
    public void SmallMenuSet()
    {
        string name = smallMenuScript.NPCRenameScript.pressedNPCName;
        string displayName = mmc.Find("SmallMenuPanel").Find("NameText").GetComponent<TMP_InputField>().text;
        string character = smallMenuScript.npcCharacter;

        smallMenuScript.SetNPC(name, displayName, character);
        sc.PlaySound("plip");
    }
    public void NPCRenameBack()
    {
        mmc.Find("PlayerPanel").gameObject.SetActive(true);
        mmc.Find("SmallMenuPanel").gameObject.SetActive(false);
        mmc.Find("NPCCustomizePanel").gameObject.SetActive(false);
        sc.PlaySound("close");
    }
    public void NPCRenameStart()
    {
        if (!npcRenameScript.isStarting)
        {
            npcRenameScript.isStarting = true;
            npcRenameScript.Transfer();
            sc.PlaySound("rumble");
        }
    }
    public void NPCRenameRandom()
    {
        StartCoroutine(npcRenameScript.RandomizeWait());
        sc.PlaySound("plip");
    }
    public void NPCRenameNPC()
    {
        npcRenameScript.OnNPCClick();
        sc.PlaySound("plip");
    }
}
