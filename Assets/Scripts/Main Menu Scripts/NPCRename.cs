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

    private void OnEnable()
    {
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
        StartCoroutine(RandomizeWait());
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
        hasPressedCharacter = true;
        lastPressedCharacter = lastTouchedCharacter;
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
        Debug.Log(prisonSelectScript.currentPrisonInmateNum);
        for(int i = 0; i < (prisonSelectScript.currentPrisonInmateNum - 1 + prisonSelectScript.currentPrisonGuardNum); i++)
        {
            int rand = UnityEngine.Random.Range(1, 10);
            switch (rand)
            {
                case 1: 
                    characterSprites = dataScript.RabbitSprites;
                    setCharacter = "Rabbit";
                    break;
                case 2:
                    characterSprites = dataScript.BaldEagleSprites;
                    setCharacter = "BaldEagle";
                    break;
                case 3:
                    characterSprites = dataScript.LiferSprites;
                    setCharacter = "Lifer";
                    break;
                case 4:
                    characterSprites = dataScript.YoungBuckSprites;
                    setCharacter = "YoungBuck";
                    break;
                case 5:
                    characterSprites = dataScript.OldTimerSprites;
                    setCharacter = "OldTimer";
                    break;
                case 6: characterSprites = dataScript.BillyGoatSprites;
                    setCharacter = "BillyGoat";
                    break;
                case 7:
                    characterSprites = dataScript.FrosephSprites;
                    setCharacter = "Froseph";
                    break;
                case 8:
                    characterSprites = dataScript.TangoSprites;
                    setCharacter = "Tango";
                    break;
                case 9:
                    characterSprites = dataScript.MaruSprites;
                    setCharacter = "Maru";
                    break;
            }
            

            if(i < prisonSelectScript.currentPrisonInmateNum - 1)
            {
                transform.Find("NPCGrid").Find("Inmate" + (i + 1)).GetComponent<CustomNPCCollectionData>().customNPCData.npcType = setCharacter;
            }
            else if(i >= prisonSelectScript.currentPrisonInmateNum - 1)
            {
                transform.Find("NPCGrid").Find("Guard" + (i - (prisonSelectScript.currentPrisonInmateNum - 2))).GetComponent<CustomNPCCollectionData>().customNPCData.npcType = setCharacter;
            }
            animList[i].bodyDirSprites = characterSprites;
        }
        foreach(Transform child in NPCGrid.transform)
        {
            child.gameObject.GetComponent<NPCRenameAnim>().Randomize();
        }
        
        currentNames = new List<string>(names);

        //inmates
        for(int i = 1; i <= prisonSelectScript.currentPrisonInmateNum - 1; i++)
        {
            int rand = UnityEngine.Random.Range(0, currentNames.Count - 1);
            NPCGrid.transform.Find("Inmate" + i).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = currentNames[rand];
            currentNames.RemoveAt(rand);
        }
        //guards
        for(int i = 1; i <= prisonSelectScript.currentPrisonGuardNum; i++)
        {
            int rand = UnityEngine.Random.Range(0, currentNames.Count);
            NPCGrid.transform.Find("Guard" + i).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = "Officer " + currentNames[rand];
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
        yield return new WaitForEndOfFrame();
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
        if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.BaldEagleSprites)
        {
            character = "BaldEagle";
        }
        else if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.BillyGoatSprites)
        {
            character = "BillyGoat";
        }
        else if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.FrosephSprites)
        {
            character = "Froseph";
        }
        else if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.LiferSprites)
        {
            character = "Lifer";
        }
        else if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.MaruSprites)
        {
            character = "Maru";
        }
        else if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.OldTimerSprites)
        {
            character = "OldTimer";
        }
        else if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.RabbitSprites)
        {
            character = "Rabbit";
        }
        else if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.TangoSprites)
        {
            character = "Tango";
        }
        else if (lastPressedCharacter.GetComponent<NPCRenameAnim>().bodyDirSprites == dataScript.YoungBuckSprites)
        {
            character = "YoungBuck";
        }

        smallMenuScript.OnOpen(lastPressedCharacter.GetComponent<CustomNPCCollectionData>().customNPCData.displayName, lastPressedCharacter.tag, character);
    }
    public void Transfer()
    {
        int i = 0;
        foreach(Transform npc in transform.Find("NPCGrid"))
        {            
            setNames.Add(npc.GetComponent<CustomNPCCollectionData>().customNPCData.displayName);
            setCharacters.Add(CharacterEnumClass.GetCharacterInt(npc.GetComponent<CustomNPCCollectionData>().customNPCData.npcType));
            i++;
        }
        tileSetterScript.SetTiles(prisonSelectScript.whichPrison);
        saveScript.SetNPC(setNames, setCharacters);
        dataSenderScript.SetCurrentMapPath(prisonSelectScript.currentPrisonPath);

        Addressables.LoadSceneAsync("Prison");
    }
}
