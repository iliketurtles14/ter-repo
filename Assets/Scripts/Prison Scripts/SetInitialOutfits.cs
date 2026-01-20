using System.Collections;
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

        map = GetComponent<LoadPrison>().currentMap;
        SetOutfits();
    }
    private void SetOutfits()
    {
        //just doing the player for now since npc's arent in the game anymore

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
        if(map.mapName == "Duct Tapes are Forever")
        {
            outfitItemID = 45;
        }
        else if(map.mapName == "Santa's Sweatshop")
        {
            outfitItemID = 40;
        }
        else if(map.mapName == "Escape Team")
        {
            outfitItemID = 50;
        }

        //find the right itemData and set the stuff to the outfit slot
        ItemData data = itemDataCreatorScript.CreateItemData(outfitItemID);
        playerIDInvScript.idInv[0].itemData = data;
        mc.Find("PlayerMenuPanel").Find("Outfit").GetComponent<Image>().sprite = data.sprite;
        outfitData = data;
    }
}
