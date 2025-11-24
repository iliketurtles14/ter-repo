using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutfitController : MonoBehaviour
{
    private ApplyPrisonData prisonDataScript;
    private ItemBehaviours itemBehavioursScript;
    public bool deskIsPickedUp;
    public int currentOutfitID;
    public int currentActionNum;
    public bool isInmateAndSleeping;
    private GameObject currentIDPanel;
    private GameObject mc;
    public string outfit;
    private Sprite clearSprite;
    public List<List<Sprite>> InmateOutfitLists = new List<List<Sprite>>();
    public List<List<Sprite>> POWOutfitLists = new List<List<Sprite>>();
    public List<List<Sprite>> GuardOutfitLists = new List<List<Sprite>>();
    public List<List<Sprite>> ElfOutfitLists = new List<List<Sprite>>();
    public List<List<Sprite>> GuardElfOutfitLists = new List<List<Sprite>>();
    public List<List<Sprite>> TuxOutfitLists = new List<List<Sprite>>();
    public List<List<Sprite>> HenchmanOutfitLists = new List<List<Sprite>>();
    public List<List<Sprite>> PrisonerOutfitLists = new List<List<Sprite>>();
    public List<List<Sprite>> SoldierOutfitLists = new List<List<Sprite>>();
    public Dictionary<string, List<List<Sprite>>> outfitDict = new Dictionary<string, List<List<Sprite>>>();
    public void Start()
    {
        prisonDataScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        itemBehavioursScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemBehaviours>();
        mc = RootObjectCache.GetRoot("MenuCanvas");
        clearSprite = Resources.Load<Sprite>("PrisonResources/UI Stuff/clear");

        InmateOutfitLists.Add(prisonDataScript.InmateOutfitSleepDeadSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitDiggingSprites);
        InmateOutfitLists.Add(DataSender.instance.InmateOutfitSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitPunchingSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitCuttingSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitRakingSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitBroomingSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitPushUpSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitBenchingSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitJumpRopeSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitPullUpSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitChippingSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitBoundSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitTraySprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitZippingSprites);
        InmateOutfitLists.Add(prisonDataScript.InmateOutfitHoldingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitSleepDeadSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitDiggingSprites);
        POWOutfitLists.Add(DataSender.instance.POWOutfitWalkingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitPunchingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitCuttingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitRakingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitBroomingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitPushUpSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitBenchingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitJumpRopeSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitPullUpSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitChippingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitBoundSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitTraySprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitZippingSprites);
        POWOutfitLists.Add(prisonDataScript.POWOutfitHoldingSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitSleepDeadSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitDiggingSprites);
        GuardOutfitLists.Add(DataSender.instance.GuardOutfitSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitPunchingSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitCuttingSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitRakingSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitBroomingSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitPushUpSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitBenchingSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitJumpRopeSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitPullUpSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitChippingSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitBoundSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitTraySprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitZippingSprites);
        GuardOutfitLists.Add(prisonDataScript.GuardOutfitHoldingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitSleepDeadSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitDiggingSprites);
        ElfOutfitLists.Add(DataSender.instance.ElfOutfitWalkingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitPunchingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitCuttingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitRakingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitBroomingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitPushUpSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitBenchingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitJumpRopeSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitPullUpSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitChippingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitBoundSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitTraySprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitZippingSprites);
        ElfOutfitLists.Add(prisonDataScript.ElfOutfitHoldingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitSleepDeadSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitDiggingSprites);
        GuardElfOutfitLists.Add(DataSender.instance.GuardElfOutfitWalkingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitPunchingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitCuttingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitRakingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitBroomingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitPushUpSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitBenchingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitJumpRopeSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitPullUpSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitChippingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitBoundSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitTraySprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitZippingSprites);
        GuardElfOutfitLists.Add(prisonDataScript.GuardElfOutfitHoldingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitSleepDeadSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitDiggingSprites);
        TuxOutfitLists.Add(DataSender.instance.TuxOutfitWalkingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitPunchingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitCuttingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitRakingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitBroomingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitPushUpSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitBenchingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitJumpRopeSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitPullUpSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitChippingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitBoundSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitTraySprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitZippingSprites);
        TuxOutfitLists.Add(prisonDataScript.TuxOutfitHoldingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitSleepDeadSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitDiggingSprites);
        HenchmanOutfitLists.Add(DataSender.instance.HenchmanOutfitWalkingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitPunchingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitCuttingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitRakingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitBroomingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitPushUpSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitBenchingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitJumpRopeSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitPullUpSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitChippingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitBoundSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitTraySprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitZippingSprites);
        HenchmanOutfitLists.Add(prisonDataScript.HenchmanOutfitHoldingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitSleepDeadSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitDiggingSprites);
        PrisonerOutfitLists.Add(DataSender.instance.PrisonerOutfitWalkingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitPunchingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitCuttingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitRakingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitBroomingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitPushUpSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitBenchingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitJumpRopeSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitPullUpSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitChippingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitBoundSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitTraySprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitZippingSprites);
        PrisonerOutfitLists.Add(prisonDataScript.PrisonerOutfitHoldingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitSleepDeadSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitDiggingSprites);
        SoldierOutfitLists.Add(DataSender.instance.SoldierOutfitWalkingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitPunchingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitCuttingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitRakingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitBroomingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitPushUpSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitBenchingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitJumpRopeSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitPullUpSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitChippingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitBoundSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitTraySprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitZippingSprites);
        SoldierOutfitLists.Add(prisonDataScript.SoldierOutfitHoldingSprites);

        outfitDict = new Dictionary<string, List<List<Sprite>>>
        {
            { "Inmate", InmateOutfitLists },
            { "POW", POWOutfitLists },
            { "Guard", GuardOutfitLists },
            { "Elf", ElfOutfitLists },
            { "GuardElf", GuardElfOutfitLists },
            { "Tux", TuxOutfitLists },
            { "Henchman", HenchmanOutfitLists },
            { "Prisoner", PrisonerOutfitLists },
            { "Soldier", SoldierOutfitLists }
        };

        currentActionNum = 2; //walking
    }
    public void Update()
    {
        if(name == "Player")
        {
            currentIDPanel = mc.transform.Find("PlayerMenuPanel").gameObject;
        }
        try
        {
            if(name == "Player")
            {
                currentOutfitID = currentIDPanel.GetComponent<PlayerIDInv>().idInv[0].itemData.id;
            }
            else
            {
                currentOutfitID = GetComponent<NPCCollectionData>().npcData.inventory[7].itemData.id;
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

        if (itemBehavioursScript.isChipping)
        {
            currentActionNum = 11;
        }
        else if (itemBehavioursScript.isCutting)
        {
            currentActionNum = 4;
        }
        else if (itemBehavioursScript.isDigging)
        {
            currentActionNum = 1;
        }
        else if (deskIsPickedUp)
        {
            currentActionNum = 15;
        }
        else
        {
            currentActionNum = 2;
        }

        if(currentOutfitID != -1)
        {
            transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled = true;
            if(name == "Player")
            {
                GetComponent<PlayerAnimation>().outfitDirSprites = outfitDict[outfit][currentActionNum];
                mc.transform.Find("PlayerMenuPanel").Find("Player").GetComponent<PlayerIDAnimation>().outfitDirSprites = outfitDict[outfit][2];
                mc.transform.Find("PlayerMenuPanel").Find("Player").Find("Outfit").GetComponent<Image>().color = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
            }
            else
            {
                GetComponent<NPCAnimation>().outfitDirSprites = outfitDict[outfit][currentActionNum];
                mc.transform.Find("NPCMenuPanel").Find("NPC").GetComponent<PlayerIDAnimation>().outfitDirSprites = outfitDict[outfit][2];
                mc.transform.Find("NPCMenuPanel").Find("NPC").Find("Outfit").GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
        else if(currentOutfitID == -1)
        {
            transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled = false; 
            if(name == "Player")
            {
                mc.transform.Find("PlayerMenuPanel").Find("Player").Find("Outfit").GetComponent<Image>().color = new Color(255 / 255, 255 / 255, 255 / 255, 0 / 255);
            }
            else
            {
                mc.transform.Find("NPCMenuPanel").Find("NPC").Find("Outfit").GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
        }
    }
}
