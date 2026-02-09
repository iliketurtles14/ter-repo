using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInvRNG : MonoBehaviour
{
    private Transform aStar;
    private Map currentMap;
    private ItemDataCreator creator;
    public List<int> itemsInInmates = new List<int>();
    public List<int> weaponsInInmates = new List<int>();
    private void Start()
    {
        aStar = RootObjectCache.GetRoot("A*").transform;
        creator = GetComponent<ItemDataCreator>();

        weaponsInInmates.Add(-1);
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        currentMap = GetComponent<LoadPrison>().currentMap;
        GetItemList();

        foreach(Transform npc in aStar)
        {
            if (npc.name.StartsWith("Inmate"))
            {
                RandomizeNPCInv(npc.gameObject);
            }
            else if (npc.name.StartsWith("Guard"))
            {
                SetGuardInv(npc.gameObject);
            }
        }
    }
    private IEnumerator RNGLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(180);
            foreach (Transform npc in aStar)
            {
                if (npc.name.StartsWith("Inmate"))
                {
                    RandomizeNPCInv(npc.gameObject);
                }
            }
        }
    }
    public void RandomizeNPCInv(GameObject npc)
    {
        int rand = UnityEngine.Random.Range(0, 5); //randomize the amount of items

        for(int i = 0; i < rand; i++)
        {
            int idRand = UnityEngine.Random.Range(0, itemsInInmates.Count);
            ItemData data = creator.CreateItemData(itemsInInmates[idRand]);
            NPCInvItem item = new NPCInvItem();
            item.itemData = data;

            npc.GetComponent<NPCCollectionData>().npcData.inventory[i] = item;
        }

        int weaponRand = UnityEngine.Random.Range(0, weaponsInInmates.Count);
        ItemData aData;
        if(weaponRand != -1)
        {
            aData = creator.CreateItemData(weaponsInInmates[weaponRand]);
        }
        else
        {
            aData = null;
        }
        NPCInvItem aItem = new NPCInvItem();
        aItem.itemData = aData;

        npc.GetComponent<NPCCollectionData>().npcData.inventory[6] = aItem;
    }
    private void SetGuardInv(GameObject npc)
    {
        int guardNum = Convert.ToInt32(npc.name.Split("d")[1]);

        ItemData data = null;
        switch (guardNum)
        {
            case 1:
                data = creator.CreateItemData(0);
                break;
            case 2:
                data = creator.CreateItemData(2);
                break;
            case 3:
                data = creator.CreateItemData(3);
                break;
            case 4:
                data = creator.CreateItemData(1);
                break;
            case 5:
                data = creator.CreateItemData(4);
                break;
        }
        NPCInvItem key = new NPCInvItem();
        key.itemData = data;
        npc.GetComponent<NPCCollectionData>().npcData.inventory[0] = key;

        if (currentMap.stunRods)
        {
            ItemData aData = creator.CreateItemData(207);
            NPCInvItem rod = new NPCInvItem();
            rod.itemData = aData;
            npc.GetComponent<NPCCollectionData>().npcData.inventory[6] = rod;
        }
        else
        {
            ItemData bData = creator.CreateItemData(56);
            NPCInvItem baton = new NPCInvItem();
            baton.itemData = bData;
            npc.GetComponent<NPCCollectionData>().npcData.inventory[6] = baton;
        }
    }
    private void GetItemList()
    {
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

            bool itemIsIn = GetINIVar(str, "InInmateInv", currentMap.items) == "True";
            bool weaponIsIn = GetINIVar(str, "InmateWeapon", currentMap.items) == "True";
            if (itemIsIn)
            {
                itemsInInmates.Add(i);
            }
            if (weaponIsIn)
            {
                weaponsInInmates.Add(i);
            }
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
