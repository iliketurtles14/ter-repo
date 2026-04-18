using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CraftMenu : MonoBehaviour
{
    private bool menuIsOpen;
    private bool menuIsFull;
    private bool invIsFull;
    public ItemData item0;
    public ItemData item1;
    public ItemData item2;
    private GameObject slot0;
    private GameObject slot1;
    private GameObject slot2;
    private GameObject craftedSlot;
    public ItemData craftedItem;
    private MouseCollisionOnItems mcs;
    private Sprite clear;
    private int invSlotNumber;
    private Inventory inventoryScript;
    private List<GameObject> invSlots = new List<GameObject>();
    private Transform ic;
    private Map currentMap;
    private Transform player;
    private List<List<int>> craftingRecipes = new List<List<int>>();
    private ItemDataCreator creator;
    private PauseController pc;
    private Transform mc;
    private NotesMenu notesMenuScript;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        clear = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");
        inventoryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        player = RootObjectCache.GetRoot("Player").transform;
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        notesMenuScript = mc.Find("NotesMenuPanel").GetComponent<NotesMenu>();

        slot0 = transform.Find("ItemGrid").Find("Item0").gameObject;
        slot1 = transform.Find("ItemGrid").Find("Item1").gameObject;
        slot2 = transform.Find("ItemGrid").Find("Item2").gameObject;
        craftedSlot = transform.Find("CraftedItem").gameObject;

        foreach(Transform slot in ic.Find("GUIPanel"))
        {
            invSlots.Add(slot.gameObject);
        }

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;

        GetCraftingRecipes();
        CloseMenu(false);
    }
    private void Update()
    {
        if(item0 != null && item1 != null && item2 != null)
        {
            menuIsFull = false;
        }
        else
        {
            menuIsFull = false;
        }

        for(int i = 0; i < 6; i++)
        {
            if (inventoryScript.inventory[i].itemData != null)
            {
                invIsFull = false;
            }
            else if (inventoryScript.inventory[i].itemData == null)
            {
                invIsFull = false;
                break;
            }
        }

        if(menuIsOpen && mcs.isTouchingInvSlot)
        {
            for(int i = 1; i <= 6; i++)
            {
                if(mcs.touchedInvSlot.name == "Slot" + i)
                {
                    invSlotNumber = i - 1;
                    break;
                }
            }
        }

        if(menuIsOpen && Input.GetMouseButtonDown(0) && mcs.isTouchingInvSlot && !menuIsFull)
        {
            if(item0 == null)
            {
                item0 = inventoryScript.inventory[invSlotNumber].itemData;
                slot0.GetComponent<Image>().sprite = item0.sprite;
            }
            else if(item1 == null)
            {
                item1 = inventoryScript.inventory[invSlotNumber].itemData;
                slot1.GetComponent<Image>().sprite = item1.sprite;
            }
            else if(item2 == null)
            {
                item2 = inventoryScript.inventory[invSlotNumber].itemData;
                slot2.GetComponent<Image>().sprite = item2.sprite;
            }
            inventoryScript.inventory[invSlotNumber].itemData = null;
            invSlots[invSlotNumber].GetComponent<Image>().sprite = clear;
        }

        if(menuIsOpen && Input.GetMouseButtonDown(0) && mcs.isTouchingCraftSlot && !invIsFull)
        {
            ItemData touchedItem = null;
            switch (mcs.touchedCraftSlot.name)
            {
                case "Item0":
                    touchedItem = item0;
                    break;
                case "Item1":
                    touchedItem = item1;
                    break;
                case "Item2":
                    touchedItem = item2;
                    break;
                case "CraftedItem":
                    touchedItem = craftedItem;
                    break;
            }
            
            foreach(InventoryItem slot in inventoryScript.inventory)
            {
                if(slot.itemData == null)
                {
                    slot.itemData = touchedItem;
                    break;
                }
            }
            foreach(GameObject slot in invSlots)
            {
                if(slot.GetComponent<Image>().sprite == clear)
                {
                    slot.GetComponent<Image>().sprite = touchedItem.sprite;
                    break;
                }
            }

            switch (mcs.touchedCraftSlot.name)
            {
                case "Item0":
                    item0 = null;
                    break;
                case "Item1":
                    item1 = null;
                    break;
                case "Item2":
                    item2 = null;
                    break;
                case "CraftedItem":
                    craftedItem = null;
                    break;
            }
            mcs.touchedCraftSlot.GetComponent<Image>().sprite = clear;
        }

        if (!mcs.isTouchingButton && !mcs.isTouchingInvSlot && !mcs.isTouchingIDPanel && !mcs.isTouchingCraftSlot && !mcs.isTouchingExtra && Input.GetMouseButtonDown(0))
        {
            CloseMenu(false);
        }
    }
    private void GetCraftingRecipes()
    {
        List<string> crSet = GetINISet("Crafting Recipes", currentMap.items);
        foreach(string str in crSet)
        {
            if (str.Contains('='))
            {
                string rawRecipe = str.Split('=')[0];
                List<int> ids = new List<int>();
                string[] idStrings = rawRecipe.Split('+');
                foreach(string idString in idStrings)
                {
                    ids.Add(Convert.ToInt32(idString));
                }
                craftingRecipes.Add(ids);
            }
        }
    }
    public void Craft()
    {
        Debug.Log("Trying to craft...");

        List<string> crSet = GetINISet("Crafting Recipes", currentMap.items);
        List<string> tempList = new List<string>();
        foreach (string str in crSet)
        {
            if (str.Contains('='))
            {
                tempList.Add(str);
            }
        }
        crSet = new List<string>(tempList);

        List<int> currentIDs = new List<int>();//ids that are in the menu
        if (item0 != null)
        {
            currentIDs.Add(item0.id);
        }
        if (item1 != null)
        {
            currentIDs.Add(item1.id);
        }
        if (item2 != null)
        {
            currentIDs.Add(item2.id);
        }

        int i = 0;
        bool matches = false;
        foreach (List<int> lists in craftingRecipes)
        {
            bool same = currentIDs.OrderBy(x => x).SequenceEqual(lists.OrderBy(x => x));
            if (same)
            {
                matches = true;
                break;
            }
            i++;
        }

        if (!matches)
        {
            Debug.Log("No crafting recipes match the given crafting input. Returning...");
            return;
        }

        string rawResult = crSet[i].Split('=')[1];
        List<int> results = new List<int>();
        int returnID = -1;
        int reqInt = 0;

        string rawResults;
        if (rawResult.Contains(';'))
        {
            rawResults = rawResult.Split(';')[0];
        }
        else if (rawResult.Contains('_'))
        {
            rawResults = rawResult.Split('_')[0];
        }
        else
        {
            rawResults = rawResult;
        }

        string[] individualResults = rawResults.Split(',');
        foreach (string result in individualResults)
        {
            results.Add(Convert.ToInt32(result));
        }

        if (rawResult.Contains(';'))
        {
            returnID = Convert.ToInt32(rawResult.Split(';')[1].Split('_')[0]);
        }
        if (rawResult.Contains('_'))
        {
            reqInt = Convert.ToInt32(rawResult.Split('_')[1]);
        }

        if (reqInt > player.GetComponent<PlayerCollectionData>().playerData.intellect)
        {
            Debug.Log("Player is too stupid to craft this. Returning...");
            return;
        }

        AddCraftNote(currentIDs, results, reqInt);
        StartCoroutine(CraftMenuAnim());

        item0 = null;
        item1 = null;
        item2 = null;
        slot0.GetComponent<Image>().sprite = clear;
        slot1.GetComponent<Image>().sprite = clear;
        slot2.GetComponent<Image>().sprite = clear;
        craftedItem = creator.CreateItemData(results[UnityEngine.Random.Range(0, results.Count)]);
        craftedSlot.GetComponent<Image>().sprite = craftedItem.sprite;

        if (returnID != -1)
        {
            item0 = creator.CreateItemData(returnID);
            slot0.GetComponent<Image>().sprite = item0.sprite;
            Debug.Log("Returning an item...");
        }
        Debug.Log("Finished crafting.");
    }
    public void AddCraftNote(List<int> ingredients, List<int> results, int intellect)
    {
        string[] paths = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "CraftingNotes"));
        string cnPath = "";
        foreach(string path in paths)
        {
            if(Path.GetFileNameWithoutExtension(path) == currentMap.fileName)
            {
                cnPath = path;
                break;
            }
        }
        if (string.IsNullOrEmpty(cnPath))
        {
            cnPath = Path.Combine(Application.streamingAssetsPath, "GlobalCraftingNotes.ini");
        }

        string cnFile = File.ReadAllText(cnPath);

        string cnToAdd = "";
        foreach(int ingredient in ingredients)
        {
            cnToAdd += ingredient.ToString() + "+";
        }
        cnToAdd = cnToAdd.Substring(0, cnToAdd.Length - 1);
        cnToAdd += "=";
        string recipe = cnToAdd;
        cnToAdd = "";
        foreach(int result in results)
        {
            cnToAdd += recipe + result.ToString() + "_" + intellect.ToString() + "\n";
        }
        cnToAdd = cnToAdd.Substring(0, cnToAdd.Length - 1);

        if (!cnFile.Contains(cnToAdd))
        {
            File.AppendAllText(cnPath, "\n" + cnToAdd);
        }
    }
    private IEnumerator CraftMenuAnim()
    {
        Debug.Log("DO THISSSSSSSSS");
        yield break;
    }
    public void OpenMenu()
    {
        menuIsOpen = true;
        transform.Find("CraftButton").gameObject.SetActive(true);
        transform.Find("ItemGrid").gameObject.SetActive(true);
        transform.Find("CraftedItem").gameObject.SetActive(true);
        transform.Find("NotesButton").gameObject.SetActive(true);
        transform.Find("CraftingText").gameObject.SetActive(true);
        transform.Find("CraftDarkCover").gameObject.SetActive(true);
        transform.GetComponent<Image>().enabled = true;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        mc.Find("Black").GetComponent<Image>().enabled = true;
        pc.Pause(false);
    }
    public void CloseMenu(bool goToNotes)
    {
        if (goToNotes)
        {
            notesMenuScript.OpenMenu();
        }
        else
        {
            mc.Find("Black").GetComponent<Image>().enabled = false;
            pc.Unpause();
        }
        GiveBackItems();
        menuIsOpen = false;
        transform.Find("CraftButton").gameObject.SetActive(false);
        transform.Find("ItemGrid").gameObject.SetActive(false);
        transform.Find("CraftedItem").gameObject.SetActive(false);
        transform.Find("NotesButton").gameObject.SetActive(false);
        transform.Find("CraftingText").gameObject.SetActive(false);
        transform.Find("CraftDarkCover").gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
    private void GiveBackItems()
    {
        List<ItemData> datas = new List<ItemData>()
        {
            item0, item1, item2, craftedItem
        };
        
        foreach(ItemData data in datas)
        {
            if (data != null)
            {    
                foreach (InventoryItem item in inventoryScript.inventory)
                {
                    if (item.itemData == null)
                    {
                        item.itemData = data;
                    }
                }
                foreach (GameObject slot in invSlots)
                {
                    if (slot.GetComponent<Image>().sprite == clear)
                    {
                        slot.GetComponent<Image>().sprite = data.sprite;
                    }
                }
            }
        }
        item0 = null;
        item1 = null;
        item2 = null;
        craftedItem = null;
        slot0.GetComponent<Image>().sprite = clear;
        slot1.GetComponent<Image>().sprite = clear;
        slot2.GetComponent<Image>().sprite = clear;
        craftedSlot.GetComponent<Image>().sprite = clear;
    }
    public List<string> GetINISet(string header, string[] file)
    {
        int startLine = -1;
        int endLine = file.Length;

        // Find the header line
        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains($"[{header}]"))
            {
                startLine = i + 1; // Start after the header
                break;
            }
        }

        if (startLine == -1)
            return new List<string>(); // Header not found

        // Find the next header or end of file
        for (int i = startLine; i < file.Length; i++)
        {
            if (file[i].StartsWith("[") && file[i].EndsWith("]"))
            {
                endLine = i;
                break;
            }
        }

        List<string> setList = new List<string>();
        for (int i = startLine; i < endLine; i++)
        {
            if (file[i].Contains('='))
            {
                setList.Add(file[i]);
            }
        }

        return setList;
    }
}
