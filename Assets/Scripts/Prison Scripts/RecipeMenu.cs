using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeMenu : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Inventory invScript;
    private Map currentMap;
    private ItemDataCreator creator;
    private bool ready;
    private PauseController pc;
    private Transform mc;
    private WarningMessage warningScript;
    private CraftMenu craftMenuScript;
    private List<Transform> invSlots = new List<Transform>();
    private Sprite clear;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        invScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        craftMenuScript = mc.Find("CraftMenuPanel").GetComponent<CraftMenu>();
        clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        foreach(Transform slot in RootObjectCache.GetRoot("InventoryCanvas").transform.Find("GUIPanel"))
        {
            invSlots.Add(slot);
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
        ready = true;
        Close();
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if(mcs.isTouchingInvSlot && Input.GetMouseButtonDown(0))
        {
            int slotNum = Convert.ToInt32(mcs.touchedInvSlot.name.Replace("Slot", "")) - 1;
            if (invScript.inventory[slotNum].itemData != null)
            {
                int id = invScript.inventory[slotNum].itemData.id;
                if(id == 85)
                {
                    GetRandNote(slotNum);
                }
            }
        }
    }
    private void GetRandNote(int slotNum)
    {
        string[] notesObtained = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "CraftingNotes.ini"));
        string[] allNotes = GetINISet("Crafting Recipes", currentMap.items).ToArray();
        List<int> availableItemsToCraft = new List<int>();
        foreach(string note in allNotes)
        {
            if (note.Contains("="))
            {
                int id = -1;
                if (note.Contains(","))
                {
                    continue;
                }
                if (note.Contains(";"))
                {
                    id = Convert.ToInt32(note.Split("=")[1].Split(";")[0]);
                }
                else if (note.Contains("_"))
                {
                    id = Convert.ToInt32(note.Split("=")[1].Split("_")[0]);
                }
                else
                {
                    id = Convert.ToInt32(note.Split("=")[1]);
                }
                bool shouldAdd = true;
                foreach(string note2 in notesObtained)
                {
                    if(note2.Contains("=" + id.ToString()))
                    {
                        shouldAdd = false;
                        break;
                    }
                }
                if (shouldAdd)
                {
                    availableItemsToCraft.Add(id);
                }
            }
        }
        if(availableItemsToCraft.Count == 0)
        {
            StartCoroutine(warningScript.CreateWarningMessage("Your craft journal is complete"));
            PSoundController.PlaySound("lose");
            return;
        }
        int randID = availableItemsToCraft[UnityEngine.Random.Range(0, availableItemsToCraft.Count)];
        int id0 = -1;
        int id1 = -1;
        int id2 = -1;
        int intellectReq = 0;
        foreach(string note in allNotes)
        {
            if (note.Contains("=" + randID.ToString()))
            {
                string[] ingredients = note.Split("=")[0].Split("+");
                foreach(string str in ingredients)
                {
                    Debug.Log(str);
                }
                List<string> ingList = ingredients.ToList();
                if(ingList.Count >= 1)
                {
                    id0 = Convert.ToInt32(ingredients[0]);
                }
                if(ingList.Count >= 2)
                {
                    id1 = Convert.ToInt32(ingredients[1]);
                }
                if(ingList.Count == 3)
                {
                    id2 = Convert.ToInt32(ingredients[2]);
                }
                Debug.Log(id0);
                Debug.Log(id1);
                Debug.Log(id2);

                if (note.Contains("_"))
                {
                    intellectReq = Convert.ToInt32(note.Split("_")[1]);
                }
                break;
            }
        }

        ItemData craftData = creator.CreateItemData(randID);
        ItemData data0 = null;
        ItemData data1 = null;
        ItemData data2 = null;
        if(id0 != -1)
        {
            data0 = creator.CreateItemData(id0);
        }
        if(id1 != -1)
        {
            data1 = creator.CreateItemData(id1);
        }
        if(id2 != -1)
        {
            data2 = creator.CreateItemData(id2);
        }
        string text = "";
        text += craftData.displayName + "\n - ";
        if(data0 != null)
        {
            text += data0.displayName;
        }
        if(data1 != null)
        {
            text += ", " + data1.displayName;
        }
        if(data2 != null)
        {
            text += ", " + data2.displayName;
        }
        text += "\n - ";
        text += "Requires " + intellectReq.ToString() + " intellect";
        Sprite itemSprite = craftData.sprite;
        Open(text, itemSprite);
        List<int> _ingredients = new List<int>
        {
            id0, id1, id2
        };
        List<int> aIngredients = new List<int>();
        foreach(int ing in _ingredients)
        {
            if(ing != -1)
            {
                aIngredients.Add(ing);
            }
        }
        List<int> results = new List<int>();
        results.Add(randID);
        craftMenuScript.AddCraftNote(aIngredients, results, intellectReq);

        invScript.inventory[slotNum].itemData = null;
        invSlots[slotNum].GetComponent<Image>().sprite = clear;
    }
    public void Open(string text, Sprite itemSprite)
    {
        PSoundController.PlaySound("open");
        transform.Find("RecipeText").GetComponent<TextMeshProUGUI>().text = text;
        transform.Find("Slot").Find("Item").GetComponent<Image>().sprite = itemSprite;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        mc.Find("Black").GetComponent<Image>().enabled = true;
        pc.Pause(true);
    }
    public void Close()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
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
