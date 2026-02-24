using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shops : MonoBehaviour
{
    public List<NPCInvItem> shop1 = new List<NPCInvItem>();
    public List<NPCInvItem> shop2 = new List<NPCInvItem>();
    public GameObject shop1NPC;
    public GameObject shop2NPC;
    private List<GameObject> npcs = new List<GameObject>();
    private Transform aStar;
    private List<int> shopItemIDs = new List<int>();
    private Map currentMap;
    private bool ready;
    private ItemDataCreator creator;
    private ApplyPrisonData applyScript;
    private void Start()
    {
        aStar = RootObjectCache.GetRoot("A*").transform;
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        foreach(Transform npc in aStar)
        {
            if (npc.name.StartsWith("Inmate"))
            {
                npcs.Add(npc.gameObject);
                npc.Find("IconCanvas").Find("ShopIcon").GetComponent<Image>().sprite = applyScript.UISprites[154];
            }
        }
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;

        string[] itemsFile = currentMap.items;
        for(int i = 0; i < 274; i++)
        {
            string str = i.ToString();
            if(str.Length == 1)
            {
                str = "00" + str;
            }
            else if(str.Length == 2)
            {
                str = "0" + str;
            }

            if(GetINIVar(str, "Cost", itemsFile) != "-1")
            {
                shopItemIDs.Add(i);
            }
        }

        foreach (Transform npc in aStar)
        {
            if (npc.name.StartsWith("Guard"))
            {
                npc.Find("IconCanvas").Find("ShopIcon").gameObject.SetActive(false);
            }
        }

        ready = true;
        StartCoroutine(ShopLoop());
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        foreach(GameObject npc in npcs)
        {
            if (!npc.GetComponent<NPCCollectionData>().npcData.hasShop)
            {
                npc.transform.Find("IconCanvas").Find("ShopIcon").gameObject.SetActive(false);
            }
        }
    }
    private IEnumerator ShopLoop()
    {
        while (true)
        {
            SetShops();
            yield return new WaitForSeconds(180);
        }
    }
    public void SetShops()
    {
        shop1.Clear();
        shop2.Clear();
        foreach(GameObject npc in npcs)
        {
            npc.GetComponent<NPCCollectionData>().npcData.hasShop = false;
        }
        
        int count = npcs.Count;
        if(count == 0)
        {
            return;
        }
        int rand1 = UnityEngine.Random.Range(0, count);
        int rand2 = UnityEngine.Random.Range(0, count);
        bool oneShop = rand1 == rand2;

        for(int i = 0; i < 4; i++)
        {
            int rand3 = UnityEngine.Random.Range(0, shopItemIDs.Count);
            int id = shopItemIDs[rand3];

            ItemData data = creator.CreateItemData(id);
            NPCInvItem item = new NPCInvItem();
            item.itemData = data;

            shop1.Add(item);
        }
        if (!oneShop)
        {
            for (int i = 0; i < 4; i++)
            {
                int rand3 = UnityEngine.Random.Range(0, shopItemIDs.Count);
                int id = shopItemIDs[rand3];

                ItemData data = creator.CreateItemData(id);
                NPCInvItem item = new NPCInvItem();
                item.itemData = data;

                shop2.Add(item);
            }
        }

        shop1NPC = npcs[rand1];
        shop1NPC.GetComponent<NPCCollectionData>().npcData.hasShop = true;
        if (!oneShop)
        {
            shop2NPC = npcs[rand2];
            shop2NPC.GetComponent<NPCCollectionData>().npcData.hasShop = true;
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
