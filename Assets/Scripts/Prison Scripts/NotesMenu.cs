using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesMenu : MonoBehaviour
{
    private PauseController pc;
    private Transform mc;
    private CraftMenu craftMenuScript;
    private ItemDataCreator creator;
    private void Start()
    {
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        craftMenuScript = mc.Find("CraftMenuPanel").GetComponent<CraftMenu>();
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();

        CloseMenu(false);
    }
    public void OpenMenu()
    {
        LoadNotes();
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("NotesScrollRect").gameObject.SetActive(true);
        transform.Find("NotesText").gameObject.SetActive(true);
        transform.Find("CraftingButton").gameObject.SetActive(true);
        mc.Find("Black").GetComponent<Image>().enabled = true;
        pc.Pause(true);
    }
    public void CloseMenu(bool goToCraft)
    {
        if (goToCraft)
        {
            craftMenuScript.OpenMenu();
        }
        else
        {
            mc.Find("Black").GetComponent<Image>().enabled = false;
            pc.Unpause();
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("NotesScrollRect").gameObject.SetActive(false);
        transform.Find("NotesText").gameObject.SetActive(false);
        transform.Find("CraftingButton").gameObject.SetActive(false);
    }
    private void LoadNotes()
    {
        string[] playerData;
        try
        {
            playerData = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, "PlayerData.ini"));
        }
        catch { return; }

        List<string> rawNotes = GetINISet("CraftingNotes", playerData);
        List<string> tempList = new List<string>();
        foreach(string line in rawNotes)
        {
            if (line.Contains('='))
            {
                tempList.Add(line);
            }
        }
        rawNotes = tempList;

        foreach(string line in rawNotes)
        {
            string rawIngredients = line.Split('=')[0];
            string[] ingredients = rawIngredients.Split('+');

            List<string> ingredientNameList = new List<string>();
            foreach(string ingredient in ingredients)
            {
                string itemName = creator.CreateItemData(Convert.ToInt32(ingredient)).displayName;
                ingredientNameList.Add(itemName);
            }
            ItemData resultItemData = creator.CreateItemData(Convert.ToInt32(line.Split('=')[1].Split('_')[0]));
            string resultName = resultItemData.displayName;
            Sprite resultSprite = resultItemData.sprite;
            string reqInt = line.Split('_')[1];

            string recipeStr = "";
            foreach(string item in ingredientNameList)
            {
                recipeStr += item + ", ";
            }
            recipeStr = recipeStr.Substring(0, recipeStr.Length - 2);
            recipeStr += " (" + reqInt + " Int.)";

            GameObject noteObj = Instantiate(transform.Find("NotesScrollRect").Find("ViewPort").Find("Content").Find("PlaceholderNote")).gameObject;
            noteObj.SetActive(true);
            noteObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            noteObj.name = resultName + " Note";
            noteObj.transform.Find("Slot").Find("Item").GetComponent<Image>().sprite = resultSprite;
            noteObj.transform.Find("TextBoxes").Find("CraftedItemText").GetComponent<TextMeshProUGUI>().text = resultName;
            noteObj.transform.Find("TextBoxes").Find("RecipeText").GetComponent<TextMeshProUGUI>().text = recipeStr;
        }
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
