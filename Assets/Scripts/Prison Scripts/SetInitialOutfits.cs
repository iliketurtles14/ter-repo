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
        int outfitItemID;

        if (!map.powOutfits)
        {
            outfitItemID = 29;
        }
        else
        {
            outfitItemID = 33;
        }

        //check if special prison (DTAF, SS, etc...)
        switch (map.mapName)
        {
            case "Duct Tapes are Forever":
                outfitItemID = 45;
                break;
            case "Santa's Sweatshop":
                outfitItemID = 40;
                break;
            case "Escape Team":
                outfitItemID = 50;
                break;
        }

        //find the right itemData and set the stuff to the outfit slot
        ItemData data = itemDataCreatorScript.CreateItemData(outfitItemID);
        playerIDInvScript.idInv[0].itemData = data;
        mc.Find("PlayerMenuPanel").Find("Outfit").GetComponent<Image>().sprite = data.sprite;
        outfitData = data;

        Debug.Log($"Setting outfits for {aStar.childCount} NPCs");

        foreach(Transform npc in aStar)
        {
            NPCData npcData = npc.GetComponent<NPCCollectionData>().npcData;
            
            Debug.Log($"Processing {npc.name}, inventory is null? {npcData.inventory == null}");
            
            npcData.inventory = new List<NPCInvItem>();
            
            // Ensure inventory has 8 slots
            for(int i = 0; i < 8; i++)
            {
                npcData.inventory.Add(new NPCInvItem());
            }
            
            Debug.Log($"{npc.name} inventory count: {npcData.inventory.Count}");
            
            if (npc.name.Contains("Guard"))
            {
                outfitItemID = 39;
                switch (map.mapName)
                {
                    case "Duct Tapes are Forever":
                        outfitItemID = 49;
                        break;
                    case "Santa's Sweatshop":
                        outfitItemID = 44;
                        break;
                    case "Escape Team":
                        outfitItemID = 54;
                        break;
                }

                data = itemDataCreatorScript.CreateItemData(outfitItemID);
                npcData.inventory[7].itemData = data;
                Debug.Log($"Set guard outfit for {npc.name}, itemData is null? {npcData.inventory[7].itemData == null}");

            }
            else if (npc.name.Contains("Inmate"))
            {
                if (!map.powOutfits)
                {
                    outfitItemID = 29;
                }
                else
                {
                    outfitItemID = 33;
                }
                switch (map.mapName)
                {
                    case "Duct Tapes are Forever":
                        outfitItemID = 45;
                        break;
                    case "Santa's Sweatshop":
                        outfitItemID = 40;
                        break;
                    case "Escape Team":
                        outfitItemID = 50;
                        break;
                }

                npcData.inventory[7].itemData = itemDataCreatorScript.CreateItemData(outfitItemID);
                Debug.Log($"Set inmate outfit for {npc.name}, itemData is null? {npcData.inventory[7].itemData == null}");
            }
        }
        
        Debug.Log("SetOutfits complete");
    }
}
