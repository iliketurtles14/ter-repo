using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class NPCOutfitController : MonoBehaviour
{
    private OutfitController outfitControllerScript;
    private ItemBehaviours itemBehavioursScript;
    public int currentActionNum;
    public string outfit;
    private GameObject currentIDPanel;
    private GameObject mc;
    private int order;
    private bool doneWaiting;
    public int currentOutfitID;
    public Dictionary<string, List<List<Sprite>>> outfitDict = new Dictionary<string, List<List<Sprite>>>();

    public void Start()
    {
        itemBehavioursScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemBehaviours>();
        outfitControllerScript = RootObjectCache.GetRoot("Player").GetComponent<OutfitController>();
        mc = RootObjectCache.GetRoot("MenuCanvas");

        outfitDict = outfitControllerScript.outfitDict;
        
        StartCoroutine(WaitForOrder());
    }
    public IEnumerator WaitForOrder()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        order = GetComponent<NPCCollectionData>().npcData.order;
        doneWaiting = true;
    }
    public void Update()
    {
        int frameNumber = 1;
        
        if (!doneWaiting)
        {
            return;
        }
        
        currentIDPanel = mc.transform.Find("NPCMenuPanel" + order).gameObject;
        try
        {
            if(frameNumber > 1)
            {
                currentOutfitID = GetComponent<NPCInvData>().outfit.itemData.id;
            }
        }
        catch
        {
            currentOutfitID = -1;
        }

        switch (currentOutfitID)
        {
            case 29:
            case 30:
            case 31:
            case 32:
                outfit = "Inmate";
                break;
            case 33:
            case 34:
            case 35:
            case 36:
                outfit = "POW";
                break;
            case 39:
                outfit = "Guard";
                break;
            case 40:
            case 41:
            case 42:
            case 43:
                outfit = "Elf";
                break;
            case 44:
                outfit = "GuardElf";
                break;
            case 45:
            case 46:
            case 47:
            case 48:
                outfit = "Tux";
                break;
            case 49:
                outfit = "Henchman";
                break;
            case 50:
            case 51:
            case 52:
            case 53:
                outfit = "Prisoner";
                break;
            case 54:
                outfit = "Soldier";
                break;
            default:
                outfit = null;
                break;
        }

        currentActionNum = 2;

        if(currentOutfitID != -1)
        {
            transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<NPCAnimation>().outfitDirSprites = outfitDict[outfit][currentActionNum];
        }
        else if(currentOutfitID == -1)
        {
            transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled = false;
        }

        frameNumber++;
    }
}
