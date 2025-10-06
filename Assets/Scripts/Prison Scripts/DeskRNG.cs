using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DeskRNG : MonoBehaviour
{
    private Sprite clearSprite;
    private List<ItemData> itemList = new List<ItemData>();
    private Transform tiles;
    private LoadPrison loadPrisonScript;
    private SetDeskNum setDeskNumScript;
    private Dictionary<string, List<int>> percentageDict = new Dictionary<string, List<int>>()
    {
        { "Center Perks", new List<int>() { 0, 5, 5, 40, 30, 20 } },
        { "Stalag Flucht", new List<int>() { 0, 10, 10, 30, 30, 20 } },
        { "Shankton State Pen", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "Jungle Compound", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "San Pancho", new List<int>() { 15, 20, 25, 20, 10, 10 } },
        { "HMP Irongate", new List<int>() { 5, 10, 15, 25, 25, 20 } },
        { "Jingle Cells", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "Banned Camp", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "London Tower", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "Paris Central Pen", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "Santas Sweatshop", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "Duct Tapes Are Forever", new List<int>() { 10, 15, 20, 20, 30, 5 } },
        { "Escape Team", new List<int>() { 5, 10, 15, 30, 30, 10 } },
        { "Alcatraz", new List<int>() { 5, 10, 15, 25, 25, 20 } },
        { "Fhurst Peak Correctional", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "Camp Epsilon", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "Fort Bamford", new List<int>() { 5, 15, 20, 20, 30, 10 } },
        { "Custom", new List<int>() {5, 15, 20, 20, 30, 10} }
    };
    private List<string> prisonNames = new List<string>()
    {
        "Center Perks", "Stalag Flucht", "Shankton State Pen", "Jungle Compound",
        "San Pancho", "HMP Irongate", "Jingle Cells", "Banned Camp",
        "London Tower", "Paris Central Pen", "Santas Sweatshop", "Duct Tapes Are Forever",
        "Escape Team", "Alcatraz", "Fhurst Peak Correctional", "Camp Epsilon",
        "Fort Bamford"
    };
    private List<int> percentages = new List<int>();//dependent on prison
    private List<GameObject> deskSlots = new List<GameObject>();
    private int tokens;
    //public int tokenBase;
    //public int tokenRoof;

    private List<int> tier1Items = new List<int>();
    private List<int> tier2Items = new List<int>();
    private List<int> tier3Items = new List<int>();
    private List<int> tier4Items = new List<int>();
    private List<int> tier5Items = new List<int>();
    private List<int> tier6Items = new List<int>();
    private void Start()
    {
        clearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");

        itemList = Resources.LoadAll<ItemData>("Item Scriptable Objects").ToList();

        loadPrisonScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>();
        setDeskNumScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<SetDeskNum>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        while (true)
        {
            if (!setDeskNumScript.isReadyForRNG)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            else
            {
                break;
            }
        }

        string currentMapName = loadPrisonScript.currentMap.mapName;
        if (prisonNames.Contains(currentMapName))
        {
            percentages = percentageDict[loadPrisonScript.currentMap.mapName];
        }
        else
        {
            percentages = percentageDict["Custom"];
        }
        Debug.Log("PLEASE DONT FORGET TO CHANGE THIS");
        tokens = 20;

        foreach (Transform child in transform)
        {
            deskSlots.Add(child.gameObject);
        }

        foreach (ItemData item in itemList)
        {
            switch (item.token)
            {
                case 1: tier1Items.Add(item.id); break;
                case 2: tier2Items.Add(item.id); break;
                case 3: tier3Items.Add(item.id); break;
                case 4: tier4Items.Add(item.id); break;
                case 5: tier5Items.Add(item.id); break;
                case 6: tier6Items.Add(item.id); break;
            }
        }

        foreach (Transform obj in tiles)
        {
            if (obj.gameObject.CompareTag("Desk"))
            {
                RandomizeDesk(obj.gameObject);
            }
        }
    }
    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F1))
    //    {
    //        RandomizeDesk();
    //    }
    //}
    public void RandomizeDesk(GameObject desk)
    {
        List<DeskItem> deskInv = desk.GetComponent<DeskData>().deskInv;
        string deskName = desk.name;

        // Clear the desk
        for (int i = 0; i < deskSlots.Count; i++)
        {
            deskInv[i].itemData = null;
            deskSlots[i].GetComponent<UnityEngine.UI.Image>().sprite = clearSprite;
        }

        // Add predefined items
        if(deskName != "DevDesk")
        {
            AddItem(82, deskInv);
            AddItem(146, deskInv);
            AddItem(128, deskInv);
            AddItem(134, deskInv);
        }

        if (deskName == "DevDesk")
        {
            AddItem(012, deskInv);
            AddItem(015, deskInv);
            AddItem(018, deskInv);
            AddItem(019, deskInv);
            AddItem(000, deskInv);
            AddItem(021, deskInv);
            AddItem(026, deskInv);
            AddItem(027, deskInv);
            AddItem(028, deskInv);
            AddItem(026, deskInv);
            AddItem(027, deskInv);
            AddItem(028, deskInv);
            AddItem(105, deskInv);
            AddItem(102, deskInv);
            AddItem(131, deskInv);
            AddItem(140, deskInv);
            AddItem(140, deskInv);
            AddItem(140, deskInv);
            AddItem(140, deskInv);
            AddItem(140, deskInv);
            return;
        }

        if (deskName == "PlayerDesk")
        {
            return;
        }

        int amountOfItems = 6;
        tokens = UnityEngine.Random.Range(8, 13);

        for(int i = 0; i < amountOfItems && tokens != 0; i++)
        {
            bool isTier1 = false;
            bool isTier2 = false;
            bool isTier3 = false;
            bool isTier4 = false;
            bool isTier5 = false;
            bool isTier6 = false;

            int randPercent = UnityEngine.Random.Range(1, 100);

            if (randPercent <= percentages[0] && tokens >= 6)
            {
                isTier6 = true;
                tokens -= 6;
            }
            else if (randPercent <= percentages[0] + percentages[1] && randPercent > percentages[0] && tokens >= 5)
            {
                isTier5 = true;
                tokens -= 5;
            }
            else if (randPercent <= percentages[0] + percentages[1] + percentages[2] && randPercent > percentages[0] + percentages[1] && tokens >= 4)
            {
                isTier4 = true;
                tokens -= 4;
            }
            else if (randPercent <= percentages[0] + percentages[1] + percentages[2] + percentages[3] && randPercent > percentages[0] + percentages[1] + percentages[2] && tokens >= 3)
            {
                isTier3 = true;
                tokens -= 3;
            }
            else if (randPercent <= percentages[0] + percentages[1] + percentages[2] + percentages[3] + percentages[4] && randPercent > percentages[0] + percentages[1] + percentages[2] + percentages[3] && tokens >= 2)
            {
                isTier2 = true;
                tokens -= 2;
            }
            else if (randPercent <= percentages[0] + percentages[1] + percentages[2] + percentages[3] + percentages[4] + percentages[5] && randPercent > percentages[0] + percentages[1] + percentages[2] + percentages[3] + percentages[4] && tokens >= 1)
            {
                isTier1 = true;
                tokens -= 1;
            }
            else
            {
                i--;
                continue;
            }

            if (isTier1)
            {
                int rand = UnityEngine.Random.Range(0, tier1Items.Count - 1);
                AddItem(tier1Items[rand], deskInv);
            }
            else if (isTier2)
            {
                int rand = UnityEngine.Random.Range(0, tier2Items.Count - 1);
                AddItem(tier2Items[rand], deskInv);
            }
            else if (isTier3)
            {
                int rand = UnityEngine.Random.Range(0, tier3Items.Count - 1);
                AddItem(tier3Items[rand], deskInv);
            }
            else if (isTier4)
            {
                int rand = UnityEngine.Random.Range(0, tier4Items.Count - 1);
                AddItem(tier4Items[rand], deskInv);
            }
            else if (isTier5)
            {
                int rand = UnityEngine.Random.Range(0, tier5Items.Count - 1);
                AddItem(tier5Items[rand], deskInv);
            }
            else if (isTier6)
            {
                int rand = UnityEngine.Random.Range(0, tier6Items.Count - 1);
                AddItem(tier6Items[rand], deskInv);
            }
        }
    }
    public void AddItem(int id, List<DeskItem> deskInv)
    {
        for (int i = 0; i < deskInv.Count; i++)
        {
            if (deskInv[i].itemData == null)
            {
                deskInv[i].itemData = itemList[id];
                deskSlots[i].GetComponent<UnityEngine.UI.Image>().sprite = itemList[id].icon;
                break;
            }
        }
    }
}