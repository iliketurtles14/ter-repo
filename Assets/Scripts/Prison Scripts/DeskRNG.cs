using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeskRNG : MonoBehaviour
{
    private Sprite clearSprite;
    private List<ItemData> itemList = new List<ItemData>();
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
        { "Fort Bamford", new List<int>() { 5, 15, 20, 20, 30, 10 } }
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
        clearSprite = GetComponent<DeskInv>().ClearSprite;

        itemList = Resources.LoadAll<ItemData>("Item Scriptable Objects").ToList();

        percentages = percentageDict[SceneManager.GetActiveScene().name];
        tokens = 20;

        foreach (Transform child in transform)
        {
            deskSlots.Add(child.gameObject);
        }

        foreach(ItemData item in itemList)
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

        RandomizeDesk();
    }
    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F1))
    //    {
    //        RandomizeDesk();
    //    }
    //}
    public void RandomizeDesk()
    {
        // Clear the desk
        for (int i = 0; i < deskSlots.Count; i++)
        {
            GetComponent<DeskInv>().deskInv[i].itemData = null;
            deskSlots[i].GetComponent<Image>().sprite = clearSprite;
        }

        // Add predefined items
        if(name != "DevDeskMenuPanel")
        {
            AddItem(82);
            AddItem(146);
            AddItem(128);
            AddItem(134);
        }

        if (name == "DevDeskMenuPanel")
        {
            AddItem(012);
            AddItem(015);
            AddItem(018);
            AddItem(019);
            AddItem(000);
            AddItem(021);
            AddItem(026);
            AddItem(027);
            AddItem(028);
            AddItem(026);
            AddItem(027);
            AddItem(028);
            AddItem(105);
            AddItem(102);
            AddItem(131);
            AddItem(140);
            AddItem(140);
            AddItem(140);
            AddItem(140);
            AddItem(140);
            return;
        }

        if (name == "PlayerDeskMenuPanel")
        {
            return;
        }

        int amountOfItems = 6;
        tokens = UnityEngine.Random.Range(8, 13);//designed for center perks

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
                AddItem(tier1Items[rand]);
            }
            else if (isTier2)
            {
                int rand = UnityEngine.Random.Range(0, tier2Items.Count - 1);
                AddItem(tier2Items[rand]);
            }
            else if (isTier3)
            {
                int rand = UnityEngine.Random.Range(0, tier3Items.Count - 1);
                AddItem(tier3Items[rand]);
            }
            else if (isTier4)
            {
                int rand = UnityEngine.Random.Range(0, tier4Items.Count - 1);
                AddItem(tier4Items[rand]);
            }
            else if (isTier5)
            {
                int rand = UnityEngine.Random.Range(0, tier5Items.Count - 1);
                AddItem(tier5Items[rand]);
            }
            else if (isTier6)
            {
                int rand = UnityEngine.Random.Range(0, tier6Items.Count - 1);
                AddItem(tier6Items[rand]);
            }
        }
    }
    public void AddItem(int id)
    {
        for (int i = 0; i < GetComponent<DeskInv>().deskInv.Count; i++)
        {
            if (GetComponent<DeskInv>().deskInv[i].itemData == null)
            {
                GetComponent<DeskInv>().deskInv[i].itemData = itemList[id];
                deskSlots[i].GetComponent<Image>().sprite = itemList[id].icon;
                break;
            }
        }
    }
}