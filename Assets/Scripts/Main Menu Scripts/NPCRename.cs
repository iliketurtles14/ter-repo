using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCRename : MonoBehaviour
{
    public MouseCollisionOnButtons mouseCollisionScript;
    public SmallMenu smallMenuScript;
    public NPCSave saveScript;
    public PlayerMenu playerMenuScript;
    public ApplyMainMenuData dataScript;
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
    private bool isStarting;

    private void Start()
    {
        OnEnable();
    }
    private void OnEnable()
    {
        ClearPanel();
        MakeList();
        //HidePanel();
        lastTouchedButton = null;
        lastTouchedCharacter = null;
        foreach (Transform child in transform.Find("NPCSelectionGrid"))
        {
            child.GetComponent<Image>().sprite = ClearSprite;
        }
        StartCoroutine(RandomizeWait());
    }
    private void OnDisable()
    {
        ClearPanel();
    }
    private void Update()
    {

        if (mouseCollisionScript.isTouchingButton)
        {
            switch (mouseCollisionScript.touchedButton.name)
            {
                case "BackButton":
                    mouseCollisionScript.touchedButton.GetComponent<Image>().sprite = BackButtonPressedSprite;
                    lastTouchedButton = mouseCollisionScript.touchedButton;
                    touchingBackButton = true;
                    break;
                case "RandomButton":
                    mouseCollisionScript.touchedButton.GetComponent<Image>().sprite = RandomButtonPressedSprite;
                    lastTouchedButton = mouseCollisionScript.touchedButton;
                    touchingRandomButton = true;
                    break;
                case "StartGameButton":
                    mouseCollisionScript.touchedButton.GetComponent<Image>().sprite = StartButtonPressedSprite;
                    lastTouchedButton = mouseCollisionScript.touchedButton;
                    touchingStartButton = true;
                    break;
            }
        }
        else
        {
            touchingBackButton = false;
            touchingRandomButton = false;
            touchingStartButton = false;
            if(lastTouchedButton != null)
            {
                switch (lastTouchedButton.name)
                {
                    case "BackButton":
                        lastTouchedButton.GetComponent<Image>().sprite = BackButtonNormalSprite; break;
                    case "RandomButton":
                        lastTouchedButton.GetComponent<Image>().sprite = RandomButtonNormalSprite; break;
                    case "StartGameButton":
                        lastTouchedButton.GetComponent<Image>().sprite = StartButtonNormalSprite; break;
                }

            }
            lastTouchedButton = null;
        }
        if(touchingBackButton && Input.GetMouseButtonDown(0))
        {
            MainMenuCanvas.transform.Find("PlayerPanel").gameObject.SetActive(true);
            MainMenuCanvas.transform.Find("SmallMenuPanel").gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else if(touchingStartButton && Input.GetMouseButtonDown(0))
        {
            if (!isStarting)
            {
                isStarting = true;
                Transfer();
            }
        }
        else if(touchingRandomButton && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(RandomizeWait());
        }
        
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
                for (int i = 1; i <= 9; i++)
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
                for (int i = 1; i <= 5; i++)
                {
                    if (mouseCollisionScript.touchedGuard.name == "Guard" + i)
                    {
                        selectionNum = i + 9;
                        break;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                hasPressedCharacter = true;
                lastPressedCharacter = lastTouchedCharacter;
                pressedNum = selectionNum;
                pressedCharacterAmount++;
                NameText.text = lastPressedCharacter.GetComponent<CustomNPCCollectionData>().customNPCData.displayName;
                SendData();
                foreach (Transform child in transform.Find("NPCSelectionGrid"))
                {
                    child.GetComponent<Image>().sprite = ClearSprite;
                }
                transform.Find("NPCSelectionGrid").Find("Selection" + selectionNum).GetComponent<Image>().sprite = SelectHoverSprite;
                transform.Find("NPCSelectionGrid").Find("Selection" + pressedNum).GetComponent<Image>().sprite = SelectPressedSprite;
                transform.Find("NPCSelectionGrid").Find("Selection" + selectionNum).GetComponent<Image>().sprite = SelectPressedSprite;
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
            else if (!Input.GetMouseButtonDown(0) && transform.Find("NPCSelectionGrid").Find("Selection" + selectionNum).GetComponent<Image>().sprite == ClearSprite)
            {
                if (mouseCollisionScript.isTouchingInmate)
                {
                    lastTouchedCharacter = mouseCollisionScript.touchedInmate;
                }
                else if (mouseCollisionScript.isTouchingGuard)
                {
                    lastTouchedCharacter = mouseCollisionScript.touchedGuard;
                }
                transform.Find("NPCSelectionGrid").Find("Selection" + selectionNum).GetComponent<Image>().sprite = SelectHoverSprite;
            }

            if (pressedCharacterAmount > 1)
            {
                foreach(Transform child in transform.Find("NPCSelectionGrid"))
                {
                    child.GetComponent<Image>().sprite = ClearSprite;
                }
                transform.Find("NPCSelectionGrid").Find("Selection" + selectionNum).GetComponent<Image>().sprite = SelectPressedSprite;
                pressedNum = selectionNum;
                pressedCharacterAmount = 1;
            }
            if (hasPressedCharacter)
            {
                foreach(Transform child in transform.Find("NPCSelectionGrid"))
                {
                    child.GetComponent<Image>().sprite = ClearSprite;
                }
                transform.Find("NPCSelectionGrid").Find("Selection" + selectionNum).GetComponent<Image>().sprite = SelectHoverSprite;
                transform.Find("NPCSelectionGrid").Find("Selection" + pressedNum).GetComponent<Image>().sprite = SelectPressedSprite;
            }
        }
        else
        {
            if (!hasPressedCharacter)
            {
                NameText.text = "";
                currentText = null;
                ClearPanel();
            }

            if (lastTouchedCharacter != null)
            {
                if (!hasPressedCharacter)
                {
                    transform.Find("NPCSelectionGrid").Find("Selection" + selectionNum).GetComponent<Image>().sprite = ClearSprite;
                }
                else if (hasPressedCharacter)
                {
                    foreach (Transform child in transform.Find("NPCSelectionGrid"))
                    {
                        child.GetComponent<Image>().sprite = ClearSprite;
                    }
                    transform.Find("NPCSelectionGrid").Find("Selection" + pressedNum).GetComponent<Image>().sprite = SelectPressedSprite;
                }
                else { return; }
            }
            lastTouchedCharacter = null;
            if(Input.GetMouseButtonDown(0) && mouseCollisionScript.isTouchingButton && mouseCollisionScript.touchedButton.name == "NameBox")
            {
                return;
            }
            else if (Input.GetMouseButtonDown(0) && !mouseCollisionScript.isTouchingSmallMenuPanel && !mouseCollisionScript.isTouchingGuard && !mouseCollisionScript.isTouchingInmate)
            {
                foreach (Transform child in transform.Find("NPCSelectionGrid"))
                {
                    child.GetComponent<Image>().sprite = ClearSprite;
                }
                hasPressedCharacter = false;
                lastPressedCharacter = null;
                pressedCharacterAmount = 0;
                pressedNum = 0;
                //HidePanel();
            }
        }
        /*for(int i = 1; i <= 14; i++)
        {

        }*/
    }
    private void Randomize()
    {
        animList.Clear();
        NPCGrid = transform.Find("NPCGrid").gameObject;
        foreach(Transform child in NPCGrid.transform)
        {
            animList.Add(child.gameObject.GetComponent<NPCRenameAnim>());
        }
        for(int i = 0; i <= 13; i++)
        {
            int rand = UnityEngine.Random.Range(1, 10);
            switch (rand)
            {
                case 1: characterSprites = dataScript.RabbitSprites;
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
            

            if(i <= 8)
            {
                transform.Find("NPCGrid").Find("Inmate" + (i + 1)).GetComponent<CustomNPCCollectionData>().customNPCData.npcType = setCharacter;
            }
            else if(i > 8)
            {
                transform.Find("NPCGrid").Find("Guard" + (i - 8)).GetComponent<CustomNPCCollectionData>().customNPCData.npcType = setCharacter;
            }
            animList[i].bodyDirSprites = characterSprites;
        }
        foreach(Transform child in NPCGrid.transform)
        {
            child.gameObject.GetComponent<NPCRenameAnim>().Randomize();
        }
        
        currentNames = new List<string>(names);

        //inmates
        for(int i = 1; i <= 9; i++)
        {
            int rand = UnityEngine.Random.Range(0, currentNames.Count - 1);
            NPCGrid.transform.Find("Inmate" + i).GetComponent<CustomNPCCollectionData>().customNPCData.displayName = currentNames[rand];
            currentNames.RemoveAt(rand);
        }
        //guards
        for(int i = 1; i <= 5; i++)
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
    private IEnumerator RandomizeWait()
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
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("RightArrow").GetComponent<BoxCollider2D>().enabled = false;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<Image>().sprite = ClearSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<BoxCollider2D>().enabled = false;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NameText").gameObject.SetActive(false);
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("SetButton").GetComponent<BoxCollider2D>().enabled = false;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("SetButton").GetComponent<Image>().sprite = SetButtonSadSprite;
    }
    private void OpenPanel(string name)
    {
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NPC").GetComponent<SmallMenuAnim>().enabled = true;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("RightArrow").GetComponent<Image>().sprite = RightArrowSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("RightArrow").GetComponent<BoxCollider2D>().enabled = true;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<Image>().sprite = LeftArrowSprite;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<BoxCollider2D>().enabled = true;
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("NameText").gameObject.SetActive(true);
        MainMenuCanvas.transform.Find("SmallMenuPanel").Find("SetButton").GetComponent<BoxCollider2D>().enabled = true;
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
    private void Transfer()
    {
        foreach(Transform npc in transform.Find("NPCGrid"))
        {
            setNames.Add(npc.GetComponent<CustomNPCCollectionData>().customNPCData.displayName);
            switch (npc.GetComponent<CustomNPCCollectionData>().customNPCData.npcType)
            {
                case "Rabbit": setCharacters.Add(1); break;
                case "BaldEagle": setCharacters.Add(2); break;
                case "Lifer": setCharacters.Add(3); break;
                case "YoungBuck": setCharacters.Add(4); break;
                case "OldTimer": setCharacters.Add(5); break;
                case "BillyGoat": setCharacters.Add(6); break;
                case "Froseph": setCharacters.Add(7); break;
                case "Tango": setCharacters.Add(8); break;
                case "Maru": setCharacters.Add(9); break;
            }
        }
        tileSetterScript.SetTiles(prisonSelectScript.whichPrison);
        saveScript.SetNPC(setNames, setCharacters);
        saveScript.SetPlayer(playerMenuScript.setName, playerMenuScript.setCharacter);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        loadOperation.completed += (AsyncOperation op) =>
        {
            SceneManager.UnloadSceneAsync(0);
        };
    }
}
