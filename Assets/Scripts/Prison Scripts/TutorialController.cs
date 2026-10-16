using Ookii.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.XR;

public class TutorialController : MonoBehaviour
{
    private Map currentMap;
    private Transform tutorialCanvas;
    private Transform player;
    private Transform aStar;
    private Sprite holeUp100;
    private Sprite hole100;
    private Transform tiles;
    private Coroutine arrowCoroutine;
    private Transform tutArrow;
    private PauseController pc;
    private Transform tmp;
    private Transform mc;
    public bool canStart = false;
    private bool menuIsOpen;
    private Dictionary<int, List<Sprite>> tutBoxDict;
    private Transform ic;
    private GameObject playerDesk;
    private DeskInv deskInvScript;
    private DeskData deskData;
    private Camera cam;
    private GameObject placeholderDesk;
    private DeskStand deskStandScript;
    private Inventory invScript;
    private InventorySelection selectionScript;
    private GameObject ventObj;
    private GameObject durdenVent;
    private Zones zonesScript;
    private NPCIDInv npcIDInvScript;
    private Giving givingScript;
    private ShopMenu shopMenuScript;
    private GameObject durden;
    private Shops shopsScript;
    private NPCCollectionData durdColData;
    private CraftMenu craftMenuScript;
    private PlayerIDInv playerMenuScript;
    private SaveMenu saveMenuScript;
    private FavorMenu favorMenuScript;
    private GameObject officerFragile;
    private NPCInv npcInvScript;
    private PlayerCollectionData playerColData;
    private UnlockDoors unlockDoorsScript;
    private GameObject theHole;
    private GameObject shovel;
    private GameObject firstDirt;
    private GameObject dirtFlashObj;
    private Coroutine dirtFlashAnimCoroutine;
    private Dictionary<int, Vector2> tutBoxSizeDict = new Dictionary<int, Vector2> //(before scale for ui)
    {
        { 1, new Vector2(210, 30) }, { 2, new Vector2(210, 42) },
        { 3, new Vector2(210, 54) }, { 4, new Vector2(240, 70) }
    };
    private Dictionary<int, int> tutBoxPosDict = new Dictionary<int, int>
    {
        { 1, 460 }, { 2, 430 }, { 3, 400 }, { 4, 360 }
    };
    private Dictionary<string, string> npcDict = new Dictionary<string, string>
    {
        { "Inmate1", "Durden" }, { "Inmate2", "Kel" }, { "Inmate3", "Spud" },
        { "Inmate4", "Morris" }, { "Inmate5", "Ben" }, { "Inmate6", "Chris" },
        { "Inmate7", "Martin" }, { "Inmate8", "Luke" }, { "Inmate9", "Michael" },
        { "Guard1", "Officer Fragile" }, { "Guard2", "Officer Loiter" }, { "Guard3", "Officer Blase" },
        { "Guard4", "Officer Slack" }
    };
    private void Start()
    {
        tutorialCanvas = RootObjectCache.GetRoot("TutorialCanvas").transform;
        player = RootObjectCache.GetRoot("Player").transform;
        aStar = RootObjectCache.GetRoot("A*").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        tutArrow = RootObjectCache.GetRoot("TutArrow").transform;
        pc = GetComponent<PauseController>();
        tmp = RootObjectCache.GetRoot("MenuCanvas").transform.Find("TutorialMenuPanel");
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        deskInvScript = mc.Find("DeskMenuPanel").GetComponent<DeskInv>();
        cam = Camera.main;
        deskStandScript = GetComponent<DeskStand>();
        invScript = GetComponent<Inventory>();
        selectionScript = GetComponent<InventorySelection>();
        zonesScript = GetComponent<Zones>();
        npcIDInvScript = mc.Find("NPCMenuPanel").GetComponent<NPCIDInv>();
        givingScript = mc.Find("NPCGiveMenuPanel").GetComponent<Giving>();
        shopMenuScript = mc.Find("NPCShopMenuPanel").GetComponent<ShopMenu>();
        shopsScript = GetComponent<Shops>();
        craftMenuScript = mc.Find("CraftMenuPanel").GetComponent<CraftMenu>();
        playerMenuScript = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
        saveMenuScript = mc.Find("SaveMenuPanel").GetComponent<SaveMenu>();
        favorMenuScript = mc.Find("FavorMenuPanel").GetComponent<FavorMenu>();
        npcInvScript = mc.Find("NPCInvMenu").GetComponent<NPCInv>();
        playerColData = player.GetComponent<PlayerCollectionData>();
        unlockDoorsScript = GetComponent<UnlockDoors>();
        CloseTutNote();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = GetComponent<LoadPrison>().currentMap;
        if (!currentMap.tutorial)
        {
            Destroy(tutorialCanvas.gameObject); //get rid of vignette
            yield break;
        }
        DataSender ds = DataSender.instance;
        hole100 = ds.PrisonObjectImages[132];
        holeUp100 = ds.UIImages[38];
        tutBoxDict = new Dictionary<int, List<Sprite>>
        {
            { 1, new List<Sprite>{ ds.UIImages[420], ds.UIImages[421] } },
            { 2, new List<Sprite>{ ds.UIImages[417], ds.UIImages[416] } },
            { 3, new List<Sprite>{ ds.UIImages[419], ds.UIImages[418] } },
            { 4, new List<Sprite>{ ds.UIImages[414], ds.UIImages[415] } }
        };
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "PlayerDesk")
            {
                playerDesk = obj.gameObject;
                break;
            }
        }
        foreach(Transform obj in tiles.Find("VentObjects"))
        {
            if(obj.name == "Vent" && Vector2.Distance(obj.position, new Vector2(102.4f, 88)) <= .01f)
            {
                ventObj = obj.gameObject;
                break;
            }
        }
        foreach(Transform obj in tiles.Find("VentObjects"))
        {
            if(obj.name == "Vent" && Vector2.Distance(obj.position, new Vector2(73.6f, 104)) <= .01f)
            {
                durdenVent = obj.gameObject;
                break;
            }
        }
        foreach(Transform npc in aStar)
        {
            if(npc.name == "Inmate1")
            {
                durden = npc.gameObject;
                break;
            }
        }
        foreach(Transform npc in aStar)
        {
            if(npc.name == "Guard1")
            {
                officerFragile = npc.gameObject;
                break;
            }
        }
        durdColData = durden.GetComponent<NPCCollectionData>();
        deskData = playerDesk.GetComponent<DeskData>();
        shovel = tiles.Find("UndergroundObjects").Find("012(Clone)").gameObject;
        SetTutorialStuff();
        StartCoroutine(TutorialSteps());
    }
    private void SetTutorialStuff()
    {
        //activate tutarrow
        tutArrow.gameObject.SetActive(true);
        tutArrow.Find("Canvas").Find("Panel").GetComponent<Image>().sprite = DataSender.instance.UIImages[398];
        StartCoroutine(GreenArrowAnim());

        //fix vignette
        tutorialCanvas.Find("Panel").transform.localScale = new Vector2(640, 450);

        //no saving or loading
        DataSender.instance.currentSave = -1;

        //craft notes disable
        mc.Find("CraftMenuPanel").Find("NotesButton").GetComponent<Image>().enabled = false;
        mc.Find("CraftMenuPanel").Find("NotesButton").GetComponent<Button>().enabled = false;
        mc.Find("CraftMenuPanel").Find("NotesButton").GetComponent<EventTrigger>().enabled = false;

        //stats
        PlayerCollectionData pcd = player.GetComponent<PlayerCollectionData>();
        pcd.playerData.strength = 100;
        pcd.playerData.speed = 100;
        pcd.playerData.intellect = 100;

        //player name
        pcd.playerData.displayName = "Prisoner";

        //npc names
        foreach(Transform npc in aStar)
        {
            if (npcDict.ContainsKey(npc.name))
            {
                npc.GetComponent<NPCCollectionData>().npcData.displayName = npcDict[npc.name];
            }
        }

        //turn off favor and shop icons on npcs
        foreach(Transform npc in aStar)
        {
            if(npc.name != "Inmate1")
            {
                npc.Find("IconCanvas").Find("ShopIcon").gameObject.SetActive(false);
            }
            npc.Find("IconCanvas").Find("MissionIcon").gameObject.SetActive(false);
        }

        //make hole at 80,121.6 (taken from Saving.cs mostly)
        Vector2 holePos = new Vector2(80, 121.6f);
        GameObject obj = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/100%HoleUp"));
        obj.GetComponent<Light2D>().lightCookieSprite = holeUp100;
        obj.GetComponent<Light2D>().intensity = 15;
        GameObject downObj = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/100%HoleDown"));
        downObj.GetComponent<SpriteRenderer>().sprite = hole100;
        obj.transform.position = holePos;
        downObj.transform.position = holePos;
        obj.transform.parent = tiles.Find("UndergroundObjects");
        downObj.transform.parent = tiles.Find("GroundObjects");
        obj.name = "100%HoleUp(Clone)";
        downObj.name = "100%HoleDown(Clone)";

        theHole = downObj;

        //make empty dirt objects at (80,121.6) and (80,123.2) also taken mostly from Saving.cs
        List<Vector2> dirtPositions = new List<Vector2>
        {
            new Vector2(80, 121.6f), new Vector2(80, 123.2f)
        };
        List<int> stabilities = new List<int>
        {
            3, 2
        };
        DataSender ds = DataSender.instance;
        for(int i = 0; i < 2; i++)
        {
            GameObject aObj = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Underground/DirtEmpty"));
            aObj.GetComponent<SpriteRenderer>().sprite = ds.PrisonObjectImages[25];
            aObj.transform.position = dirtPositions[i];
            aObj.transform.parent = tiles.Find("UndergroundObjects");
            aObj.name = "EmptyDirt(Clone)";
        }

        //make dirt object at (80, 124.8)
        GameObject bObj = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Underground/Dirt"));
        bObj.transform.position = new Vector2(80, 124.8f);
        bObj.name = "Dirt(Clone)";
        firstDirt = bObj;

        //zone thing
        zonesScript.tutorialPointPos = new Vector2(73.6f, 104);

        //make the dirt flash obj
        GameObject flashy = new GameObject("DirtFlash");
        flashy.AddComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        flashy.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);
        flashy.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 128);
        flashy.GetComponent<SpriteRenderer>().sortingLayerName = "UndergroundVisible";
        flashy.GetComponent<SpriteRenderer>().sortingOrder = 3;
        flashy.transform.position = new Vector2(0, 0);
        flashy.transform.parent = tiles.Find("UndergroundObjects");
        dirtFlashObj = flashy;
        dirtFlashAnimCoroutine = StartCoroutine(DirtFlashObjAnim());
    }
    private IEnumerator TutorialSteps()
    {
        while (!canStart)
        {
            yield return null;
        }

        OpenTutNote();

        while (menuIsOpen)
        {
            yield return null;
        }

        StartCoroutine(SetTutTextBox("Move to your desk and LEFT CLICK to open it. Take all the items from inside.", 1));
        SetGreenArrow(playerDesk, .8f);
        bool isAlreadyInDesk = false;
        bool isAlreadyOutOfDesk = true;
        while (!DeskIsEmpty())
        {
            if(deskInvScript.deskIsOpen && (!isAlreadyInDesk || Input.GetMouseButtonDown(0)))
            {
                yield return new WaitForEndOfFrame();
                if (deskInvScript.deskIsOpen)
                {
                    isAlreadyInDesk = true;
                    isAlreadyOutOfDesk = false;
                    SetGreenArrow(GetDeskSlotObj(20, 135), .8f);
                }
            }
            else if(!isAlreadyOutOfDesk && !deskInvScript.deskIsOpen)
            {
                isAlreadyOutOfDesk = true;
                isAlreadyInDesk = false;
                SetGreenArrow(playerDesk, .8f);
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("RIGHT CLICK is used to pick up and drop desks. Move your desk to the highlighted position.", 2));
        SetGreenArrow(playerDesk, .8f);
        Coroutine placeholderDeskCoroutine = StartCoroutine(DeskPlaceholderAnim());
        while (true)
        {
            if (Vector2.Distance(playerDesk.transform.position, placeholderDesk.transform.position) <= .01f)
            {
                StopCoroutine(placeholderDeskCoroutine);
                yield return new WaitForEndOfFrame();
                Destroy(placeholderDesk);
                break;
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("Stand beside the desk and press F to climb it. This allows you to see the vent layer above.", 2));
        SetGreenArrow(playerDesk, .8f);
        while (true)
        {
            if (deskStandScript.hasClimbed)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("LEFT CLICK on the screwdriver in your inventory, then on the vent above to begin unscrewing it.", 2));
        bool alreadySetToInv = false;
        bool alreadySetToVent = false;
        while (true)
        {
            if (ventObj == null)
            {
                break;
            }
            if (selectionScript.aSlotSelected)
            {
                if (invScript.inventory[selectionScript.selectedSlotNum].itemData.id == 20)
                {
                    if (!alreadySetToVent)
                    {
                        SetGreenArrow(ventObj, .8f);
                        alreadySetToVent = true;
                        alreadySetToInv = false;
                    }
                }
                else
                {
                    if (!alreadySetToInv)
                    {
                        SetGreenArrow(GetInvSlotObj(20, -1), .8f);
                        alreadySetToInv = true;
                        alreadySetToVent = false;
                    }
                }
            }
            else
            {
                if (!alreadySetToInv)
                {
                    SetGreenArrow(GetInvSlotObj(20, -1), .8f);
                    alreadySetToInv = true;
                    alreadySetToVent = false;
                }
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("Climb up into the open vent by LEFT CLICKing on it. Once inside make your way to a nearby cell.", 1));
        SetGreenArrow(durdenVent, .8f);
        zonesScript.shouldGoToTutorialPoint = true;
        bool hasDeactivatedVent = false;
        while (true)
        {
            if(!hasDeactivatedVent && !Physics2D.GetIgnoreLayerCollision(player.gameObject.layer, LayerMask.NameToLayer("Vent")))
            {
                hasDeactivatedVent = true;
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "EmptyVentCover(Clone)")
                    {
                        obj.GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    }
                }
            }

            if(hasDeactivatedVent && !Physics2D.GetIgnoreLayerCollision(player.gameObject.layer, LayerMask.NameToLayer("Ground")))
            {
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "EmptyVentCover(Clone)")
                    {
                        obj.GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    }
                }
                break;
            }
            yield return null;
        }
        zonesScript.shouldGoToTutorialPoint = false;

        StartCoroutine(SetTutTextBox("The flashing bag icon over Durden's head means he has items to sell. RIGHT CLICK on him to open up his profile.", 1));
        bool alreadyOnShop = false;
        bool alreadyOnSlot = false;
        bool alreadyOnDurd = false;
        while (true)
        {
            if(!alreadyOnShop && ((npcIDInvScript.idIsOpen && npcIDInvScript.currentNPC.name == "Inmate1") ||
                (givingScript.menuIsOpen && givingScript.currentNPC.name == "Inmate1")))
            {
                SetGreenArrow(mc.Find("NPCMenuPanel").Find("ShopButton").gameObject, .8f);
                alreadyOnShop = true;
                alreadyOnDurd = false;
                alreadyOnSlot = false;
            }
            else if(!alreadyOnSlot && shopMenuScript.menuIsOpen && shopMenuScript.currentNPC.name == "Inmate1")
            {
                SetGreenArrow(mc.Find("NPCShopMenuPanel").Find("SlotGrid").Find("Slot0").gameObject, .8f);
                alreadyOnSlot = true;
                alreadyOnShop = false;
                alreadyOnDurd = false;
            }
            else if(!alreadyOnDurd)
            {
                SetGreenArrow(durden, 1.6f);
                alreadyOnDurd = true;
                alreadyOnShop = false;
                alreadyOnSlot = false;
            }

            if (shopsScript.shop1[0].itemData == null)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("Great, now you can craft a weapon! Open the CRAFT menu and add the two items.", 1));
        bool hasCraftedSockMace = false;
        bool alreadyOnSockMace = false;
        bool alreadyOnCraftButton = false;
        bool alreadyOnCRAFTButton = false;
        alreadyOnSlot = false;
        GameObject currentSlot = null;
        while (true)
        {
            if(!alreadyOnSockMace && craftMenuScript.menuIsOpen && craftMenuScript.craftedItem != null && craftMenuScript.craftedItem.id == 66)
            {
                alreadyOnSockMace = true;
                SetGreenArrow(mc.Find("CraftMenuPanel").Find("CraftedItem").gameObject, .8f);
                hasCraftedSockMace = true;
            }
            else if(craftMenuScript.menuIsOpen && !hasCraftedSockMace)
            {
                GameObject slot = GetInvSlotObj(135, 134);
                if(slot == null)
                {
                    currentSlot = null;
                    List<ItemData> items = new List<ItemData>
                    {
                        craftMenuScript.item0, craftMenuScript.item1, craftMenuScript.item2
                    };
                    bool gotSock = false;
                    bool gotSoap = false;
                    bool gotNull = false;
                    foreach(ItemData data in items)
                    {
                        if(data != null)
                        {
                            if (!gotSock)
                            {
                                gotSock = data.id == 135;
                            }
                            if (!gotSoap)
                            {
                                gotSoap = data.id == 134;
                            }
                        }
                        else
                        {
                            gotNull = true;
                        }
                    }
                    if(gotSock && gotSoap && gotNull && !alreadyOnCRAFTButton)
                    {
                        alreadyOnSockMace = false;
                        alreadyOnCraftButton = false;
                        alreadyOnCRAFTButton = true;
                        alreadyOnSlot = false;
                        SetGreenArrow(mc.Find("CraftMenuPanel").Find("CraftButton").gameObject, .8f);
                    }
                    else
                    {
                        HideGreenArrow();
                    }
                }
                else
                {
                    if(currentSlot != slot && !alreadyOnSlot)
                    {
                        currentSlot = slot;
                        alreadyOnSlot = true;
                        alreadyOnCRAFTButton = false;
                        alreadyOnCraftButton = false;
                        alreadyOnSockMace = false;
                        SetGreenArrow(slot, .8f);
                    }
                }
            }
            else if(!alreadyOnCraftButton)
            {
                alreadyOnCraftButton = true;
                alreadyOnCRAFTButton = false;
                alreadyOnSlot = false;
                alreadyOnSockMace = false;
                SetGreenArrow(ic.Find("CraftButton").gameObject, .8f);
            }

            if(!craftMenuScript.menuIsOpen && hasCraftedSockMace)
            {
                break;
            }

            yield return null;
        }

        StartCoroutine(SetTutTextBox("To use a weapon you first need to equip it. So open up your profile and let's do that.", 1));
        bool alreadyOnInvIDButton = false;
        bool alreadyOnIDButton = false;
        alreadyOnSockMace = false;
        while (true)
        {
            //playermenupaenl, savemenuapelna, favor
            if (playerMenuScript.idIsOpen)
            {
                GameObject slot = GetInvSlotObj(68, -1);
                if(slot != null && !alreadyOnSockMace)
                {
                    alreadyOnSockMace = true;
                    alreadyOnInvIDButton = false;
                    alreadyOnIDButton = false;
                    SetGreenArrow(slot, .8f);
                }
                else
                {
                    HideGreenArrow();
                }
            }
            else if(!alreadyOnInvIDButton && !playerMenuScript.idIsOpen && !saveMenuScript.menuIsOpen && !favorMenuScript.favorMenuIsOpen)
            {
                alreadyOnInvIDButton = true;
                alreadyOnIDButton = false;
                alreadyOnSockMace = false;
                SetGreenArrow(ic.Find("PlayerIDButton").gameObject, .8f);
            }
            else if(!alreadyOnIDButton && (saveMenuScript.menuIsOpen || favorMenuScript.favorMenuIsOpen))
            {
                alreadyOnIDButton = true;
                alreadyOnInvIDButton = false;
                alreadyOnSockMace = false;
                SetGreenArrow(mc.Find("SaveMenuPanel").Find("IDButton").gameObject, .8f);
            }

            if (playerMenuScript.idInv[1] != null && playerMenuScript.idInv[1].itemData != null && playerMenuScript.idInv[1].itemData.id == 68)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("Let's try it out! Press SPACE to toggle combat mode on and off. With it on, click on the guard to target him.", 1));
        bool alreadyOnFragile = false;
        NPCCollectionData fragileColData = officerFragile.GetComponent<NPCCollectionData>();
        while (true)
        {
            if(!alreadyOnFragile && !fragileColData.npcData.isDead)
            {
                alreadyOnFragile = true;
                SetGreenArrow(officerFragile, .8f);
            }

            if (fragileColData.npcData.isDead)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("Ouch! If you stand next to the unconcious guard and LEFT CLICK on him you can loot his items. Grab his outfit!", 1));
        alreadyOnFragile = false;
        bool alreadyOnFragileOutfit = false;
        while (true)
        {
            if(!alreadyOnFragileOutfit && npcInvScript.menuIsOpen)
            {
                SetGreenArrow(mc.Find("NPCInvMenu").Find("Outfit").gameObject, .8f);
                alreadyOnFragile = false;
                alreadyOnFragileOutfit = true;
            }
            else if(!alreadyOnFragile && !npcInvScript.menuIsOpen)
            {
                SetGreenArrow(officerFragile, .8f);
                alreadyOnFragile = true;
                alreadyOnFragileOutfit = false;
            }

            if (fragileColData.npcData.inventory[7] == null || fragileColData.npcData.inventory[7].itemData == null)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("Let's play dress-up. Equip the guard outfit in your profile.", 1));
        alreadyOnIDButton = false;
        bool alreadyOnOutfitSlot = false;
        bool alreadyOnGuardOutfit = false;
        while (true)
        {
            if (!playerMenuScript.idIsOpen && !alreadyOnIDButton)
            {
                SetGreenArrow(ic.Find("PlayerIDButton").gameObject, .8f);
                alreadyOnIDButton = true;
                alreadyOnGuardOutfit = false;
                alreadyOnOutfitSlot = false;
            }
            else if(!alreadyOnOutfitSlot && playerMenuScript.idIsOpen && playerMenuScript.idInv[1] != null && playerMenuScript.idInv[1].itemData != null && playerMenuScript.idInv[1].itemData.id == 29)
            {
                SetGreenArrow(mc.Find("PlayerMenuPanel").Find("Outfit").gameObject, .8f);
                alreadyOnIDButton = false;
                alreadyOnOutfitSlot = true;
                alreadyOnGuardOutfit = false;
            }
            else if(!alreadyOnGuardOutfit && playerMenuScript.idIsOpen && (playerMenuScript.idInv[1] == null || playerMenuScript.idInv[1].itemData == null))
            {
                GameObject slot = GetInvSlotObj(39, -1);
                if(slot == null)
                {
                    HideGreenArrow();
                }
                else
                {
                    alreadyOnGuardOutfit = true;
                    alreadyOnOutfitSlot = false;
                    alreadyOnIDButton = false;
                    SetGreenArrow(slot, .8f);
                }
            }
            
            if(playerMenuScript.idInv[1] != null && playerMenuScript.idInv[1].itemData != null && playerMenuScript.idInv[1].itemData.id == 39)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(SetTutTextBox("Lookin' good! Now go outside to the right and head north past the guards, where your exit awaits...", 1));
        zonesScript.tutorialPointPos = new Vector2(80, 121.6f);
        zonesScript.shouldGoToTutorialPoint = true;
        unlockDoorsScript.aUnlockDoors(new List<string> { "purple" });
        bool alreadyOnHole = false;
        while (true)
        {
            if (!alreadyOnHole)
            {
                SetGreenArrow(theHole, .8f);
            }

            if(!Physics2D.GetIgnoreLayerCollision(player.gameObject.layer, LayerMask.NameToLayer("Underground")))
            {
                break;
            }
            yield return null;
        }
        foreach(Transform obj in tiles.Find("UndergroundObjects"))
        {
            if (obj.name.Contains("HoleUp"))
            {
                obj.GetComponent<BoxCollider2D>().enabled = false;
                break;
            }
        }
        zonesScript.shouldGoToTutorialPoint = false;

        StartCoroutine(SetTutTextBox("Dark down here isn't it? RIGHT CLICK to pick up that shovel and let's get digging!", 1));
        bool alreadyOnGroundShovel = false;
        bool alreadyOnDirt = false;
        bool alreadyOnShovel = false;
        while (true)
        {
            if(firstDirt == null)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame(); //make sure next dirt spawns
                break;
            }
            
            if(!alreadyOnGroundShovel && GetInvSlotObj(12, -1) == null) //checks if the shovel isnt in the inv
            {
                SetGreenArrow(shovel, .8f);
                alreadyOnGroundShovel = true;
                alreadyOnDirt = false;
                alreadyOnShovel = false;
            }
            else if(!alreadyOnShovel && ((GetInvSlotObj(12, -1) != null && !selectionScript.aSlotSelected) || 
                (GetInvSlotObj(12, -1) != null && selectionScript.aSlotSelected && invScript.inventory[selectionScript.selectedSlotNum].itemData.id != 12)))
            {
                SetGreenArrow(GetInvSlotObj(12, -1), .8f); //this slot obj is guaranteed not to be null here
                alreadyOnShovel = true;
                alreadyOnDirt = false;
                alreadyOnGroundShovel = false;
            }
            
            if(!alreadyOnDirt && selectionScript.aSlotSelected && invScript.inventory[selectionScript.selectedSlotNum].itemData.id == 12)
            {
                SetGreenArrow(firstDirt, .8f);
                dirtFlashObj.transform.position = new Vector2(80, 124.8f);
                alreadyOnDirt = true;
                alreadyOnGroundShovel = false;
                alreadyOnShovel = false;
            }
            else
            {
                dirtFlashObj.transform.position = Vector2.zero;
            }
            yield return null;
        }
        GameObject secondDirt = null;
        foreach(Transform obj in tiles.Find("UndergroundObjects"))
        {
            if(obj.name.Contains("DirtEmpty") && Vector2.Distance(obj.position, new Vector2(80, 126.4f)) <= .1f)
            {
                secondDirt = obj.gameObject;
                break;
            }
        }
        if(secondDirt == null)
        {
            Debug.Log("oh no its null!!!");
            yield break;
        }
        dirtFlashObj.transform.position = Vector2.zero;

        StartCoroutine(SetTutTextBox("Digging on an empty space allows you to dig UPWARDS and emerge from the underground.", 1));
        alreadyOnShovel = false;
        alreadyOnDirt = false;
        Collider2D dirtCol = secondDirt.GetComponent<Collider2D>();
        while (true)
        {
            if (!alreadyOnShovel && ((!selectionScript.aSlotSelected) || //remember you cant drop in tutorial so the check for the slot being null doesnt apply here since its guaranteed the player has the shovel
                (selectionScript.aSlotSelected && invScript.inventory[selectionScript.selectedSlotNum].itemData.id != 12)))
            {
                SetGreenArrow(GetInvSlotObj(12, -1), .8f); //this slot obj is guaranteed not to be null here
                alreadyOnShovel = true;
                alreadyOnDirt = false;
            }
            
            if(!alreadyOnDirt && selectionScript.aSlotSelected && invScript.inventory[selectionScript.selectedSlotNum].itemData.id == 12)
            {
                SetGreenArrow(secondDirt, .8f);
                dirtFlashObj.transform.position = new Vector2(80, 126.4f);
                alreadyOnDirt = true;
                alreadyOnShovel = false;
            }
            else
            {
                dirtFlashObj.transform.position = Vector2.zero;
            }

            if (!dirtCol.enabled) //signifies that the tile has been broken (see ItemBehaviours.cs at BreakTile())
            {
                break;
            }
            yield return null;
        }
        StopCoroutine(dirtFlashAnimCoroutine);
        Destroy(dirtFlashObj);

        int playerLayer = player.gameObject.layer;
        int ugLayer = LayerMask.NameToLayer("Underground");
        while (!Physics2D.GetIgnoreLayerCollision(playerLayer, ugLayer))
        {
            yield return null;
        }
        StartCoroutine(SetTutTextBox("That wasn't too tough was it? All you have to do now is walk on out. Freedom has never tasted so good!", 1));
    }
    private bool DeskIsEmpty()
    {
        bool isEmpty = true;
        foreach (DeskItem item in deskData.deskInv)
        {
            if(item != null && item.itemData != null)
            {
                isEmpty = false;
                break;
            }
        }
        return isEmpty;
    }
    private GameObject GetDeskSlotObj(int itemID, int backupID)
    {
        int index = 0;
        bool foundIt = false;
        foreach(DeskItem item in deskData.deskInv)
        {
            if(item == null || item.itemData == null)
            {
                continue;
            }
            
            if(item.itemData.id == itemID)
            {
                foundIt = true;
                break;
            }
            index++;
        }
        if (!foundIt && backupID != -1)
        {
            index = 0;
            foreach (DeskItem item in deskData.deskInv)
            {
                if (item == null || item.itemData == null)
                {
                    continue;
                }

                if (item.itemData.id == backupID)
                {
                    foundIt = true;
                    break;
                }
                index++;
            }
        }
        if (!foundIt)
        {
            return null;
        }
        index++;
        return mc.Find("DeskMenuPanel").Find("Slot" + index.ToString()).gameObject;
    }
    private GameObject GetInvSlotObj(int itemID, int backupID)
    {
        int index = 0;
        bool foundIt = false;
        foreach(InventoryItem item in invScript.inventory)
        {
            if(item == null || item.itemData == null)
            {
                continue;
            }
            if(item.itemData.id == itemID)
            {
                foundIt = true;
                break;
            }
            index++;
        }
        if (!foundIt && backupID != -1)
        {
            index = 0;
            foreach (InventoryItem item in invScript.inventory)
            {
                if (item == null || item.itemData == null)
                {
                    continue;
                }

                if (item.itemData.id == backupID)
                {
                    foundIt = true;
                    break;
                }
                index++;
            }
        }
        if (!foundIt)
        {
            return null;
        }
        index++;
        return ic.Find("GUIPanel").Find("Slot" + index.ToString()).gameObject;
    }
    private void OpenTutNote()
    {
        foreach(Transform child in tmp)
        {
            child.gameObject.SetActive(true);
        }
        tmp.GetComponent<Image>().enabled = true;
        tmp.GetComponent<BoxCollider2D>().enabled = true;
        mc.Find("Black").GetComponent<Image>().enabled = true;
        pc.Pause(true);
        PSoundController.PlaySound("open");
        menuIsOpen = true;
    }
    public void CloseTutNote()
    {
        foreach (Transform child in tmp)
        {
            child.gameObject.SetActive(false);
        }
        tmp.GetComponent<Image>().enabled = false;
        tmp.GetComponent<BoxCollider2D>().enabled = false;
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
        menuIsOpen = false;
    }
    private IEnumerator SetTutTextBox(string msg, int size) //1, 2, 3, 4
    {
        Vector2 sizeVec = new Vector2(tutBoxSizeDict[size].x * 5, tutBoxSizeDict[size].y * 5);
        Sprite spr = tutBoxDict[size][0];
        Sprite sprFlash = tutBoxDict[size][1];
        int yPos = tutBoxPosDict[size];

        //430
        ic.Find("HintPanel").GetComponent<RectTransform>().sizeDelta = sizeVec;
        ic.Find("HintPanel").transform.localPosition = new Vector2(430, yPos);
        Image img = ic.Find("HintPanel").GetComponent<Image>();
        //.217 twice start with flash
        float time = 0f;
        img.sprite = sprFlash;
        ic.Find("HintPanel").Find("Text").GetComponent<TextMeshProUGUI>().text = msg;
        ic.Find("HintPanel").gameObject.SetActive(true);
        while (time <= .217f)
        {
            if (pc.isPaused)
            {
                yield return null;
                continue;
            }
            time += Time.deltaTime;
            yield return null;
        }
        time = 0f;
        img.sprite = spr;
        while (time <= .217f)
        {
            if (pc.isPaused)
            {
                yield return null;
                continue;
            }
            time += Time.deltaTime;
            yield return null;
        }
        time = 0f;
        img.sprite = sprFlash;
        while (time <= .217f)
        {
            if (pc.isPaused)
            {
                yield return null;
                continue;
            }
            time += Time.deltaTime;
            yield return null;
        }
        img.sprite = spr;
    }
    private void SetGreenArrow(GameObject objToFollow, float distFromObj)
    {
        if(objToFollow == null)
        {
            return;
        }
        
        if(arrowCoroutine != null)
        {
            StopCoroutine(arrowCoroutine);
        }
        arrowCoroutine = StartCoroutine(GreenArrowFollow(objToFollow, distFromObj));
    }
    private IEnumerator GreenArrowFollow(GameObject objToFollow, float distFromObj)
    {
        RectTransform rect = null;
        bool doRectStuff = false;
        if (objToFollow.GetComponent<RectTransform>())
        {
            doRectStuff = true;
            rect = objToFollow.GetComponent<RectTransform>();
        }

        while (true)
        {
            Vector3 pos = objToFollow.transform.position;
            if (doRectStuff)
            {
                pos = cam.ScreenToWorldPoint(RectTransformUtility.WorldToScreenPoint(null, rect.position));
            }
            
            tutArrow.position = pos + new Vector3(0, distFromObj, 0);
            yield return null;
        }
    }
    private void HideGreenArrow()
    {
        tutArrow.gameObject.SetActive(false);
    }
    private IEnumerator GreenArrowAnim()
    {
        float time = 0f;
        float dist = 0;
        while (true)
        {
            //.1cos(2pix)
            time += Time.deltaTime;
            dist = .1f * Mathf.Cos(2f * Mathf.PI * time);
            tutArrow.Find("Canvas").Find("Panel").transform.localPosition = new Vector2(0, dist);
            yield return null;
        }
    }
    private IEnumerator DeskPlaceholderAnim()
    {
        MakeDeskPlaceholderObj();
        float time = 0;
        while (true)
        {
            if (pc.isPaused)
            {
                yield return null;
                continue;
            }
            //cos(2pix)*.05 - .05
            time += Time.deltaTime;
            float sizeDiff = .05f * Mathf.Cos(2 * Mathf.PI * time) - .05f;
            float scale = sizeDiff + 1;
            placeholderDesk.transform.localScale = new Vector2(scale, scale);
            yield return null;
        }
    }
    private void MakeDeskPlaceholderObj()
    {
        //102.4, 86.4
        placeholderDesk = new GameObject("PlaceholderDesk");
        placeholderDesk.layer = LayerMask.NameToLayer("Ground");
        SpriteRenderer sr = placeholderDesk.AddComponent<SpriteRenderer>();
        sr.sprite = playerDesk.GetComponent<SpriteRenderer>().sprite;
        sr.color = new Color(1, 1, 1, .5f);
        sr.sortingOrder = 2;
        sr.sortingLayerName = "Ground";
        placeholderDesk.transform.position = new Vector3(102.4f, 86.4f, 0);
        placeholderDesk.transform.parent = tiles.Find("GroundObjects");
    }
    private IEnumerator DirtFlashObjAnim()
    {
        float time = 0f;
        SpriteRenderer sr = dirtFlashObj.GetComponent<SpriteRenderer>();
        while (true)
        {//255, 0, 0; 243, 119, 0; .217
            sr.color = new Color(243, 119, 0, 128);
            time = 0f;
            while (time <= .217f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }

            sr.color = new Color(255, 0, 0, 128);
            time = 0f;
            while (time <= .217f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }

            yield return null;
        }
    }
}
