using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
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
    private Dictionary<int, int> itemDeskTierDict = new Dictionary<int, int>();
    private Transform tiles;
    private LoadPrison loadPrisonScript;
    private SetDeskNum setDeskNumScript;
    private ItemDataCreator creator;
    private ApplyPrisonData applyScript;
    private Transform mc;
    private List<DeskItem> deskInv;
    private Dictionary<int, int> itemSpriteDict = new Dictionary<int, int>() //this was even more of a bitch to code
    {
        { 0, 6 }, { 1, 7 }, { 2, 64 }, { 3, 4 }, { 4, 113 }, { 5, 68 }, { 6, 67 }, { 7, 66 },
        { 8, 69 }, { 9, 114 }, { 10, 5 }, { 11, 5 }, { 12, 5 }, { 13, 31 }, { 14, 31 }, {15, 31 },
        { 16, 103 }, { 17, 103 }, { 18, 103 }, { 19, 173 }, { 20, 19 }, { 21, 177 }, { 22, 32 },
        { 23, 176 }, { 24, 46 }, { 25, 20 }, { 26, 98 }, { 27, 97 }, { 28, 96 }, { 29, 9 }, {30, 133 },
        { 31, 132 }, { 32, 131 }, { 33, 112 }, { 34, 130 }, { 35, 128 }, { 36, 129 }, { 37, 54 },
        { 38, 53 }, { 39, 235 }, { 40, 235 }, { 41, 235 }, { 42, 235 }, { 43, 235 }, { 44, 255 },
        { 45, 212 }, { 46, 212 }, { 47, 212 }, { 48, 212 }, { 49, 225 }, { 50, 207 }, { 51, 207 },
        { 52, 207 }, { 53, 207 }, { 54, 208 }, { 55, 259 }, { 56, 22 }, { 57, 24 }, { 58, 109 },
        { 59, 90 }, { 60, 26 }, { 61, 123 }, { 62, 92 }, { 63, 27 }, { 64, 99 }, { 65, 77 }, { 66, 86 },
        { 67, 174 }, { 68, 86 }, { 69, 79 }, { 70, 91 }, { 71, 172 }, { 72, 15 }, { 73, 36 }, { 74, 56 },
        { 75, 111 }, { 76, 51 }, { 77, 40 }, { 78, 0 }, { 79, 11 }, { 80, 100 }, { 81, 44 }, { 82, 12 },
        { 83, 110 }, { 84, 48 }, { 85, 106 }, { 86, 72 }, { 87, 87 }, { 88, 16 }, { 89, 147 }, { 90, 85 },
        { 91, 187 }, { 92, 175 }, { 93, 44 }, { 94, 122 }, { 95, 170 }, { 96, 104 }, { 97, 102 }, { 98, 82 },
        { 99, 89 }, { 100, 13 }, { 101, 93 }, { 102, 94 }, { 103, 30 }, { 104, 28 }, { 105, 34 }, { 106, 220 },
        { 107, 127 }, { 108, 3 }, { 109, 168 }, { 110, 116 }, { 111, 65 }, { 112, 167 }, { 113, 121 },
        { 114, 115 }, { 115, 39 }, { 116, 124 }, { 117, 125 }, { 118, 126 }, { 119, 75 }, { 120, 83 },
        { 121, 188 }, { 122, 62 }, { 123, 21 }, { 124, 81 }, { 125, 18 }, { 126, 76 }, { 127, 14 }, { 128, 37 },
        { 129, 183 }, { 130, 59 }, { 131, 63 }, { 132, 120 }, { 133, 73 }, { 134, 38 }, { 135, 57 }, { 136, 23 },
        { 137, 50 }, { 138, 169 }, { 139, 1 }, { 140, 55 }, { 141, 105 }, { 142, 78 }, { 143, 29 }, { 144, 42 },
        { 145, 41 }, { 146, 35 }, { 147, 47 }, { 148, 119 }, { 149, 58 }, { 150, 44 }, { 151, 49 }, { 152, 118 },
        { 153, 43 }, { 154, 101 }, { 155, 2 }, { 156, 80 }, { 157, 44 }, { 158, 256 }, { 159, 184 },
        { 160, 236 }, { 161, 240 }, { 162, 189 }, { 163, 153 }, { 164, 261 }, { 165, 192 }, { 166, 216 },
        { 167, 262 }, { 168, 193 }, { 169, 196 }, { 170, 238 }, { 171, 218 }, { 172, 194 }, { 173, 222 },
        { 174, 45 }, { 175, 266 }, { 176, 239 }, { 177, 268 }, { 178, 117 }, { 179, 267 }, { 180, 274 },
        { 181, 217 }, { 182, 60 }, { 183, 260 }, { 184, 265 }, { 185, 275 }, { 186, 195 }, { 187, 197 },
        { 188, 181 }, { 189, 198 }, { 190, 199 }, { 191, 200 }, { 192, 211 }, { 193, 201 }, { 194, 229 },
        { 195, 202 }, { 196, 191 }, { 197, 251 }, { 198, 269 }, { 199, 264 }, { 200, 252 }, { 201, 243 },
        { 202, 180 }, { 203, 273 }, { 204, 276 }, { 205, 270 }, { 206, 215 }, { 207, 182 }, { 208, 209 },
        { 209, 204 }, { 210, 277 }, { 211, 245 }, { 212, 210 }, { 213, 233 }, { 214, 246 }, { 215, 247 },
        { 216, 248 }, { 217, 254 }, { 218, 249 }, { 219, 263 }, { 220, 250 }, { 221, 137 }, { 222, 146 },
        { 223, 136 }, { 224, 149 }, { 225, 152 }, { 226, 138 }, { 227, 151 }, { 228, 150 }, { 229, 145 },
        { 230, 135 }, { 231, 148 }, { 232, 134 }, { 233, 219 }, { 234, 228 }, { 235, 221 }, { 236, 224 },
        { 237, 232 }, { 238, 223 }, { 239, 230 }, { 240, 234 }, { 241, 227 }, { 242, 231 }, { 243, 206 },
        { 244, 203 }, { 245, 205 }, { 246, 178 }, { 247, 179 }, { 248, 139 }, { 249, 158 }, { 250, 140 },
        { 251, 143 }, { 252, 142 }, { 253, 159 }, { 254, 144 }, { 255, 166 }, { 256, 141 }, { 257, 157 },
        { 258, 237 }, { 259, 241 }, { 260, 242 }, { 261, 244 }, { 262, 164 }, { 263, 163 }, { 264, 160 },
        { 265, 161 }, { 266, 162 }, { 267, 156 }, { 268, 154 }, { 269, 153 }, { 270, 155 }, { 271, 272 },
        { 272, 271 }, { 273, 214 }, { 274, 165 }
    };
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
        "London Tower", "Paris Central Pen", "Santa's Sweatshop", "Duct Tapes are Forever",
        "Escape Team", "Alcatraz", "Fhurst Peak Correctional", "Camp Epsilon",
        "Fort Bamford"
    };
    private List<int> percentages = new List<int>();//dependent on prison
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

        loadPrisonScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>();
        setDeskNumScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<SetDeskNum>();
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();

        tiles = RootObjectCache.GetRoot("Tiles").transform;

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

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        MakeItemDeskTierDict();
        deskInv = GetComponent<DeskData>().deskInv;

        if(name != "DevDesk" && name != "PlayerDesk")
        {
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

            for(int i = 0; i < 274; i++)
            {
                switch (itemDeskTierDict[i])
                {
                    case 1: tier1Items.Add(i); break;
                    case 2: tier2Items.Add(i); break;
                    case 3: tier3Items.Add(i); break;
                    case 4: tier4Items.Add(i); break;
                    case 5: tier5Items.Add(i); break;
                    case 6: tier6Items.Add(i); break;
                }
            }
        }
        RandomizeDesk();
    }
    public void RandomizeDesk()
    {
        string deskName = name;

        // Clear the desk
        for (int i = 0; i < deskInv.Count; i++)
        {
            deskInv[i].itemData = null;
        }

        // Add predefined items
        if(deskName == "PlayerDesk" || deskName == "NPCDesk")
        {
            AddItem(82);
            AddItem(146);
            AddItem(128);
            AddItem(134);
        }

        if (deskName == "DevDesk")
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

        if (deskName == "PlayerDesk" || deskName == "DevDesk")
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
        for (int i = 0; i < deskInv.Count; i++)
        {
            if (deskInv[i].itemData == null)
            {
                deskInv[i].itemData = creator.CreateItemData(id);
                break;
            }
        }
    }
    private void MakeItemDeskTierDict()
    {
        for(int i = 0; i < 274; i++)
        {
            string str;
            if(i.ToString().Length == 1)
            {
                str = "00" + i.ToString();
            }
            else if(i.ToString().Length == 2)
            {
                str = "0" + i.ToString();
            }
            else
            {
                str = i.ToString();
            }

            itemDeskTierDict.Add(i, Convert.ToInt32(GetINIVar(str, "DeskTier", loadPrisonScript.currentMap.items)));
        }
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