using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SetInitialOutfits : MonoBehaviour
{
    private Map map;
    private PlayerIDInv playerIDInvScript;
    private Transform mc;
    public ItemData outfitData;
    private ItemDataCreator itemDataCreatorScript;
    private Transform aStar;
    private Dictionary<string, int> inmateOutfitDict = new Dictionary<string, int>
    {
        { "Inmate", 29 }, { "POW", 33 }, { "Elf", 40 }, { "Prisoner", 50 }, { "Tux", 45 }
    };
    private Dictionary<string, int> guardOutfitDict = new Dictionary<string, int>
    {
        { "Inmate", 39 }, { "POW", 39 }, { "Elf", 44 }, { "Prisoner", 54 }, { "Tux", 49 }
    };

    private void Start()
    {
        playerIDInvScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        itemDataCreatorScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        aStar = RootObjectCache.GetRoot("A*").transform;

        map = GetComponent<LoadPrison>().currentMap;
        SetOutfits();
    }
    private void SetOutfits()
    {
        int outfitItemID = inmateOutfitDict[map.playerOutfit];

        //find the right itemData and set the stuff to the outfit slot
        ItemData data = itemDataCreatorScript.CreateItemData(outfitItemID);
        playerIDInvScript.idInv[0].itemData = data;
        mc.Find("PlayerMenuPanel").Find("Outfit").GetComponent<Image>().sprite = data.sprite;
        outfitData = data;


        foreach(Transform npc in aStar)
        {
            if(!npc.name.Contains("Inmate") && !npc.name.Contains("Guard"))
            {
                continue;
            }
            
            NPCData npcData = npc.GetComponent<NPCCollectionData>().npcData;
            
            
            npcData.inventory = new List<NPCInvItem>();
            
            // Ensure inventory has 8 slots
            for(int i = 0; i < 8; i++)
            {
                npcData.inventory.Add(new NPCInvItem());
            }


            if (npc.name.Contains("Guard"))
            {
                outfitItemID = guardOutfitDict[map.npcOutfit];

                data = itemDataCreatorScript.CreateItemData(outfitItemID);
                npcData.inventory[7].itemData = data;

            }
            else if (npc.name.Contains("Inmate"))
            {
                outfitItemID = inmateOutfitDict[map.npcOutfit];

                npcData.inventory[7].itemData = itemDataCreatorScript.CreateItemData(outfitItemID);
            }
        }
        
    }
}
