using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using System.IO;
using System.Linq;
using NUnit.Framework.Constraints;

public class NPCRename : MonoBehaviour
{
    public MouseCollisionOnButtons mouseCollisionScript;
    public SmallMenu smallMenuScript;
    public NPCSave saveScript;
    public PlayerMenu playerMenuScript;
    public ApplyMainMenuData dataScript;
    public DataSender dataSenderScript;
    public TileSetter tileSetterScript;
    public PrisonSelect prisonSelectScript;
    public Sprite BackButtonNormalSprite;
    public Sprite BackButtonPressedSprite;
    public Sprite RandomButtonNormalSprite;
    public Sprite RandomButtonPressedSprite;
    public Sprite StartButtonNormalSprite;
    public Sprite StartButtonPressedSprite;
    public Sprite SelectHoverSprite;
    public Sprite SelectPressedSprite;
    public Sprite ClearSprite;
    public Sprite LeftArrowSprite;
    public Sprite RightArrowSprite;
    public Sprite SetButtonSadSprite;
    private GameObject lastTouchedButton;
    private GameObject lastTouchedCharacter;
    private GameObject lastPressedCharacter;
    public TextMeshProUGUI NameText;
    private bool hasPressedCharacter;
    private int pressedCharacterAmount = 0;
    private bool touchingBackButton;
    private bool touchingRandomButton;
    private bool touchingStartButton;
    public Canvas MainMenuCanvas;
    private int selectionNum;
    private int pressedNum;
    public bool comingFromRename;
    private GameObject NPCGrid;
    public List<NPCRenameAnim> animList;
    private List<Sprite> characterSprites = new List<Sprite>();
    public List<string> names = new List<string>();
    public List<string> currentNames = new List<string>();
    private bool hasRandomized;
    private string currentText;
    private string character;
    public string pressedNPCName;
    private List<string> setNames = new List<string>();
    private List<int> setCharacters = new List<int>();
    private string setCharacter;
    public bool isStarting;
    public MMSoundController sc;
    public Transform blocker;
    public int currentSave = -1;
    private string npcOutfitType;
    private string npcBodyType;
    private Dictionary<List<Sprite>, string> charDict = new Dictionary<List<Sprite>, string>();
    private List<string> etNames = new List<string>
    {
        "Cage", "Sean", "Andy"
    };
    private List<string> dtafNames = new List<string>
    {
        "Elbrah", "Chen", "Piers", "Mourn", "Lazeeboi", "Blonde", "Walton", "Prowler", "Crane"
    };
    private void OnEnable()
    {
        if(dataScript == null)
        {
            dataScript = GetGivenData.instance.GetComponent<ApplyMainMenuData>();
        }
        
        if(charDict.Count == 0)
        {
            MakeCharDict();
        }

        ClearPanel();
        ResetNPCGrid();
        LoadNPCGrid(prisonSelectScript.currentPrisonGuardNum + prisonSelectScript.currentPrisonInmateNum - 1);
        MakeList();
        lastTouchedButton = null;
        lastTouchedCharacter = null;
        foreach (Transform child in transform.Find("NPCSelectionGrid"))
        {
            child.GetComponent<Image>().sprite = ClearSprite;
            SpriteState spriteState = new SpriteState();
            spriteState.highlightedSprite = SelectHoverSprite;
            spriteState.pressedSprite = SelectPressedSprite;
            child.GetComponent<Button>().spriteState = spriteState;
        }
        foreach(Transform child in transform.Find("NPCGrid"))
        {
            child.GetComponent<Image>().sprite = ClearSprite;
        }
        NameText.text = "";
        StartCoroutine(RandomizeWait());
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
    private void OnDisable()
    {
        ClearPanel();
        ResetNPCGrid();
    }
    private void Update()
    {        
        if(mouseCollisionScript.isTouchingInmate || mouseCollisionScript.isTouchingGuard)
        {
            if (mouseCollisionScript.isTouchingInmate)
            {
                if (!hasPressedCharacter) 
                {
                    NameText.text = mouseCollisionScript.touchedInmate.GetComponent<CustomNPCCollectionData>().customNPCData.displayName;
                    currentText = NameText.text;
                }
                lastTouchedCharacter = mouseCollisionScript.touchedInmate;
                for (int i = 1; i <= prisonSelectScript.currentPrisonInmateNum - 1; i++)
                {
                    if (mouseCollisionScript.touchedInmate.name == "Inmate" + i)
                    {
                        selectionNum = i;
                        break;
                    }
                }
            }
            else if (mouseCollisionScript.isTouchingGuard)
            {
                if (!hasPressedCharacter)
                {
                    NameText.text = mouseCollisionScript.touchedGuard.GetComponent<CustomNPCCollectionData>().customNPCData.displayName;
                    currentText = NameText.text;
                }
                lastTouchedCharacter = mouseCollisionScript.touchedGuard;
                for (int i = 1; i <= prisonSelectScript.currentPrisonGuardNum; i++)
                {
                    if (mouseCollisionScript.touchedGuard.name == "Guard" + i)
                    {
                        selectionNum = i + prisonSelectScript.currentPrisonInmateNum - 1;
                        break;
                    }
                }
            }
        }
        else
        {
            lastTouchedCharacter = null;
            
            if (!hasPressedCharacter)
            {
                NameText.text = "";
                currentText = null;
                ClearPanel();
            }
            if(Input.GetMouseButtonDown(0) && mouseCollisionScript.isTouchingButton && mouseCollisionScript.touchedButton.name == "NameBox")
            {
                return;
            }
            else if (Input.GetMouseButtonDown(0) && !mouseCollisionScript.isTouchingSmallMenuPanel && !mouseCollisionScript.isTouchingGuard && !mouseCollisionScript.isTouchingInmate)
            {
                if(!mouseCollisionScript.isTouchingButton && lastPressedCharacter != null)
                {
                    sc.PlaySound("close");
                }

                hasPressedCharacter = false;
                lastPressedCharacter = null;
                pressedCharacterAmount = 0;
                pressedNum = 0;
                foreach (Transform child in transform.Find("NPCSelectionGrid"))
                {
                    child.GetComponent<Image>().sprite = ClearSprite;
                    SpriteState aSpriteState = new SpriteState();
                    aSpriteState.highlightedSprite = SelectHoverSprite;
                    aSpriteState.pressedSprite = SelectPressedSprite;
                    child.GetComponent<Button>().spriteState = aSpriteState;
                }
            }
        }
    }
    public void OnNPCClick()
    {
        lastPressedCharacter = lastTouchedCharacter;
        try //just a check for if this is an acutal npc button or not tstssssssfas;lkdfj;alskdf;
        {
            NameText.text = lastPressedCharacter.GetComponent<CustomNPCCollectionData>().customNPCData.displayName;
        }
        catch
        {
            return;
        }
        hasPressedCharacter = true;
        pressedNum = selectionNum;
        pressedCharacterAmount++;
        NameText.text = lastPressedCharacter.GetComponent<CustomNPCCollectionData>().customNPCData.displayName;
        SendData();
        foreach(Transform child in transform.Find("NPCSelectionGrid"))
        {
            child.GetComponent<Image>().sprite = ClearSprite;
            SpriteState aSpriteState = new SpriteState();
            aSpriteState.highlightedSprite = SelectHoverSprite;
            aSpriteState.pressedSprite = SelectPressedSprite;
            child.GetComponent<Button>().spriteState = aSpriteState;
        }
        transform.Find("NPCSelectionGrid").Find("Selection" + pressedNum).GetComponent<Image>().sprite = SelectPressedSprite;
        SpriteState spriteState = transform.Find("NPCSelectionGrid").Find("Selection" + pressedNum).GetComponent<Button>().spriteState;
        spriteState.highlightedSprite = SelectPressedSprite;
        transform.Find("NPCSelectionGrid").Find("Selection" + pressedNum).GetComponent<Button>().spriteState = spriteState;
        if (mouseCollisionScript.isTouchingInmate)
        {
            pressedNPCName = mouseCollisionScript.touchedInmate.name;
            OpenPanel(mouseCollisionScript.touchedInmate.GetComponent<CustomNPCCollectionData>().customNPCData.displayName);
        }
        else if (mouseCollisionScript.isTouchingGuard)
        {
            pressedNPCName = mouseCollisionScript.touchedGuard.name;
            OpenPanel(mouseCollisionScript.touchedGuard.GetComponent<CustomNPCCollectionData>().customNPCData.displayName);
        }
    }
    private void ResetNPCGrid()
    {
        foreach(Transform npc in transform.Find("NPCGrid"))
        {
            Destroy(npc.gameObject);
        }
        foreach(Transform selection in transform.Find("NPCSelectionGrid"))
        {
            Destroy(selection.gameObject);
        }
    }
    private void LoadNPCGrid(int npcCount)
    {
        Vector2 gridSize = Vector2.zero;

        if(npcCount <= 5)
        {
            gridSize = new Vector2(5, 1);
        }
        else if(npcCount >= 6 && npcCount <= 10)
        {
            gridSize = new Vector2(5, 2);
        }
        else if(npcCount >= 11 && npcCount <= 15)
        {
            gridSize = new Vector2(5, 3);
        }
        else if(npcCount >= 16 && npcCount <= 21)
        {
            gridSize = new Vector2(7, 3);
        }
        else if(npcCount >= 22 && npcCount <= 25)
        {
            gridSize = new Vector2(8, 4);
        }
        else if(npcCount >= 26)
        {
            int rows = npcCount / 9;

            gridSize = new Vector2(9, rows);
        }

        transform.Find("NPCGrid").GetComponent<GridLayoutGroup>().constraintCount = Convert.ToInt32(gridSize.x);
        transform.Find("NPCSelectionGrid").GetComponent<GridLayoutGroup>().constraintCount = Convert.ToInt32(gridSize.x);

        int inmateCount = prisonSelectScript.currentPrisonInmateNum - 1;
        int guardCount = prisonSelectScript.currentPrisonGuardNum;

        for(int i = 1; i <= inmateCount; i++)
        {
            GameObject inmate = Instantiate(Resources.Load<GameObject>("Main Menu Resources/Inmate"));
            inmate.name = "Inmate" + i;
            inmate.GetComponent<NPCRenameAnim>().dataScript = dataScript;
            inmate.transform.parent = transform.Find("NPCGrid");
            inmate.transform.localScale = new Vector3(1, 1, 1);
        }
        for(int i = 1; i <= guardCount; i++)
        {
            GameObject guard = Instantiate(Resources.Load<GameObject>("Main Menu Resources/Guard"));
            guard.name = "Guard" + i;
            guard.GetComponent<NPCRenameAnim>().dataScript = dataScript;
            guard.transform.parent = transform.Find("NPCGrid");
            guard.transform.localScale = new Vector3(1, 1, 1);
        }
        for (int i = 1; i <= npcCount; i++)
        {
            GameObject selection = Instantiate(transform.Find("Selection").gameObject);
            selection.name = "Selection" + i;
            selection.transform.parent = transform.Find("NPCSelectionGrid");
            selection.transform.localScale = new Vector3(1, 1, 1);

            selection.GetComponent<Image>().enabled = true;
            selection.GetComponent<Button>().enabled = true;
            selection.GetComponent<EventTrigger>().enabled = true;
        }
    }
    private void Randomize()
    {
        animList.Clear();
        NPCGrid = transform.Find("NPCGrid").gameObject;
        foreach(Transform child in NPCGrid.transform)
        {
            animList.Add(child.gameObject.GetComponent<NPCRenameAnim>());
        }
        List<int> availableInmateChars = new List<int>();
        List<int> availableGuardChars = new List<int>();
        switch (prisonSelectScript.currentPrisonNPCBody)
        {
            case "Normal":
                for(int i = 0; i < 9; i++)
                {
                    availableInmateChars.Add(i);
                    availableGuardChars.Add(i);
                }
                break;
            case "SS":
                for(int i = 10; i < 18; i++)
                {
                    availableInmateChars.Add(i);
                }
                availableGuardChars.Add(18);
                break;
            case "ET":
                for(int i = 31; i < 34; i++)
                {
                    availableInmateChars.Add(i);
                }
                availableGuardChars.Add(34);
                break;
            case "DTAF":
                for(int i = 20; i < 29; i++)
                {
                    availableInmateChars.Add(i);
                }
                availableGuardChars.Add(29);
                break;
        }
        for(int i = 0; i < (prisonSelectScript.currentPrisonInmateNum - 1 + prisonSelectScript.currentPrisonGuardNum); i++)
        {
            bool isInmate = i < prisonSelectScript.currentPrisonInmateNum - 1;
            bool isGuard = i >= prisonSelectScript.currentPrisonInmateNum - 1;

            int rand = 0;
            if (isInmate)
            {
                rand = UnityEngine.Random.Range(0, availableInmateChars.Count);
                rand += availableInmateChars[0];
            }
            else if (isGuard)
            {
                rand = UnityEngine.Random.Range(0, availableGuardChars.Count);
                rand += availableGuardChars[0];
            }

            var pair = charDict.ElementAt(rand);
            if (isInmate)
            {
                switch (prisonSelectScript.currentPrisonNPCBody)
                {
                    case "ET":
                        int index = i;
                        if (index >= 3)
                        {
                            index %= 3;
                        }
                        pair = charDict.ElementAt(index + 31);
                        break;
                    case "DTAF":
                        index = i;
                        if (index >= 9)
                        {
                            index %= 9;
                        }
                        pair = charDict.ElementAt(index + 20);
                        break;
                    case "SS":
                        index = i;
                        if (index >= 8)
                        {
                            index %= 8;
                        }
                        pair = charDict.ElementAt(index + 10);
                        break;
                }
            }
            setCharacter = pair.Value;
            characterSprites = pair.Key;

            if (isInmate)
            {
                transform.Find("NPCGrid").Find("Inmate" + (i + 1)).GetComponent<CustomNPCCollectionData>().customNPCData.npcType = setCharacter;
            }
            else if(isGuard)
            {
                transform.Find("NPCGrid").Find("Guard" + (i - (prisonSelectScript.currentPrisonInmateNum - 2))).GetComponent<CustomNPCCollectionData>().customNPCData.npcType = setCharacter;
            }
            animList[i].bodyDirSprites = characterSprites;

            switch (prisonSelectScript.currentPrisonNPCOutfit)
            {
                case "Inmate":
                    if (isInmate)
                    {
                        animList[i].outfitDirSprites = dataScript.InmateOutiftSprites;
                    }
                    else if (isGuard)
                    {
                        animList[i].outfitDirSprites = dataScript.GuardOutfitSprites;
                    }
                    break;
                case "POW":
                    if (isInmate)
                    {
                        animList[i].outfitDirSprites = dataScript.POWOutfitWalkingSprites;
                    }
                    else if (isGuard)
                    {
                        animList[i].outfitDirSprites = dataScript.GuardOutfitSprites;
                    }
                    break;
                case "Elf":
                    if (isInmate)
                    {
                        animList[i].outfitDirSprites = dataScript.ElfOutfitWalkingSprites;
                    }
                    else if (isGuard)
                    {
                        animList[i].outfitDirSprites = dataScript.GuardElfOutfitWalkingSprites;
                    }
                    break;
                case "Prisoner":
                    if (isInmate)
                    {
                        animList[i].outfitDirSprites = dataScript.PrisonerOutfitWalkingSprites;
                    }
                    else if (isGuard)
                    {
                        animList[i].outfitDirSprites = dataScript.SoldierOutfitWalkingSprites;
                    }
                    break;
                case "Tux":
                    if (isInmate)
                    {
                        animList[i].outfitDirSprites = dataScript.TuxOutfitWalkingSprites;
                    }
                    else if (isGuard)
                    {
                        animList[i].outfitDirSprites = dataScript.HenchmanOutfitWalkingSprites;
                    }
                    break;
            }
        }
        foreach (Transform child in NPCGrid.transform)
        {
            child.gameObject.GetComponent<NPCRenameAnim>().Randomize();
        }

        currentNames = new List<string>(names);

        //inmates
        for(int i = 0; i <= prisonSelectScript.currentPrisonInmateNum - 2; i++)
        {
            if(prisonSelectScript.currentPrisonNPCBody != "ET" && prisonSelectScript.currentPrisonNPCBody != "DTAF")
            {
                int rand = UnityEngine.Random.Range(0, currentNames.Count - 1);
                NPCGrid.transform.Find("Inmate" + (i + 1).ToString()).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = currentNames[rand];
                currentNames.RemoveAt(rand);
            }
            else
            {
                switch (prisonSelectScript.currentPrisonNPCBody)
                {
                    case "ET":
                        int index = i;
                        if (index >= 3)
                        {
                            index %= 3;
                        }
                        NPCGrid.transform.Find("Inmate" + (i + 1).ToString()).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = etNames[index];
                        break;
                    case "DTAF":
                        index = i;
                        if (index >= 9)
                        {
                            index %= 9;
                        }
                        NPCGrid.transform.Find("Inmate" + (i + 1).ToString()).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = dtafNames[index];
                        break;
                }
            }
        }
        //guards
        for(int i = 0; i <= prisonSelectScript.currentPrisonGuardNum - 1; i++)
        {
            int rand = UnityEngine.Random.Range(0, currentNames.Count);
            NPCGrid.transform.Find("Guard" + (i + 1).ToString()).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = "Officer " + currentNames[rand];
            currentNames.RemoveAt(rand);
        }
        currentNames = names;
    }
    private void MakeList()
    {
        TextAsset namesFile = Resources.Load<TextAsset>("NPC Names");

        names = new List<string>(namesFile.text.Split('\n'));
    }
    public IEnumerator RandomizeWait()
    {
        yield return new WaitForEndOfFrame();
        Randomize();
    }
    private void ClearPanel()
    {
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NPC").GetComponent<SmallMenuAnim>().enabled = false;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NPC").GetComponent<Image>().sprite = ClearSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NPC").Find("Outfit").GetComponent<Image>().sprite = ClearSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("RightArrow").GetComponent<Image>().sprite = ClearSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("RightArrow").GetComponent<Button>().enabled = false;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<Image>().sprite = ClearSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<Button>().enabled = false;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NameText").gameObject.SetActive(false);
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("SetButton").GetComponent<Button>().enabled = false;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("SetButton").GetComponent<Image>().sprite = SetButtonSadSprite;
    }
    private void OpenPanel(string name)
    {
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NPC").GetComponent<SmallMenuAnim>().enabled = true;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("RightArrow").GetComponent<Image>().sprite = RightArrowSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("RightArrow").GetComponent<Button>().enabled = true;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<Image>().sprite = LeftArrowSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<Button>().enabled = true;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NameText").gameObject.SetActive(true);
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("SetButton").GetComponent<Button>().enabled = true;
        if (lastPressedCharacter.tag == "Inmate")
        {
            int startIndex = name.IndexOf("") + "".Length;
            string result = startIndex < name.Length ? name.Substring(startIndex).Trim() : name;
            MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NameText").GetComponent<TMP_InputField>().text = result;
            MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = result;
        }
        else if(lastPressedCharacter.tag == "Guard")
        {
            int startIndex = name.IndexOf("Officer") + "Officer".Length;
            string result = startIndex < name.Length ? name.Substring(startIndex).Trim() : name;
            MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NameText").GetComponent<TMP_InputField>().text = result;
            MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = result;
        }
    }
    private void SendData()
    {
        character = charDict[lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites];
        smallMenuScript.OnOpen(lastPressedCharacter.GetComponent<CustomNPCCollectionData>().customNPCData.displayName, lastPressedCharacter.tag, character);
    }
    public IEnumerator Transfer()
    {
        blocker.GetComponent<Animator>().enabled = true;
        blocker.GetComponent<Animator>().Rebind();
        blocker.GetComponent<Animator>().Update(0f);
        blocker.GetComponent<Animator>().Play(0, 0, 0f);
        yield return new WaitForSeconds(.6f);
        
        int i = 0;
        foreach(Transform npc in transform.Find("NPCGrid"))
        {            
            setNames.Add(npc.GetComponent<CustomNPCCollectionData>().customNPCData.displayName.Replace("\n", ""));
            setCharacters.Add(CharacterEnumClass.GetCharacterInt(npc.GetComponent<CustomNPCCollectionData>().customNPCData.npcType));
            i++;
        }
        tileSetterScript.SetTiles(prisonSelectScript.whichPrison);
        saveScript = NPCSave.instance;
        saveScript.SetNPC(setNames, setCharacters);
        dataSenderScript = DataSender.instance;
        dataSenderScript.SetCurrentMapPath(prisonSelectScript.currentPrisonPath);
        dataSenderScript.currentSave = currentSave;
        Addressables.LoadSceneAsync("Prison");
    }
    public IEnumerator LoadTransfer()
    {
        blocker.GetComponent<Animator>().enabled = true;
        blocker.GetComponent<Animator>().Rebind();
        blocker.GetComponent<Animator>().Update(0f);
        blocker.GetComponent<Animator>().Play(0, 0, 0f);
        yield return new WaitForSeconds(.6f);

        string[] saveFile = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "Saves", "Save" + currentSave.ToString() + ".ini"));
        int type = Convert.ToInt32(GetINIVar("Map", "Type", saveFile));
        string fileName = GetINIVar("Map", "FileName", saveFile);
        Dictionary<int, string> pathDict = new Dictionary<int, string>
        {
            { 0, "MainPrisons" }, { 1, "BonusPrisons" }, { 2, "CustomPrisons" }
        };
        string path = Path.Combine(Application.streamingAssetsPath, "Prisons", pathDict[type], fileName + ".zmap");
        dataSenderScript = DataSender.instance;
        dataSenderScript.SetCurrentMapPath(path);
        dataSenderScript.currentSave = currentSave;
        saveScript = NPCSave.instance;
        saveScript.SetPlayer(GetINIVar("Player", "Name", saveFile), Convert.ToInt32(GetINIVar("Player", "Character", saveFile)));
        Addressables.LoadSceneAsync("Prison");
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Contains("[") && file[j].Contains("]") && j != i)
                    {
                        line = null;
                        break;
                    }
                    if (file[j].Split('=')[0] == varName)
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }



        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }
}
