using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EscapeObjectController : MonoBehaviour
{
    private Transform tiles;
    private MouseCollisionOnItems mcs;
    private InventorySelection selectionScript;
    private Inventory inventoryScript;
    private Particles particlesScript;
    private Escaping escapeScript;
    private ApplyPrisonData applyScript;
    private SignMenu signMenuScript;
    private Sprite clear;
    private Transform escObjMenuPanel;
    private Transform mc;
    private bool leftPCDone;
    private bool downPCDone;
    private bool rightPCDone;
    private bool inMenu;
    private Transform player;
    private PauseController pc;
    private List<GameObject> escObjs = new List<GameObject>();
    private List<GameObject> invSlots = new List<GameObject>();
    private List<string> layers = new List<string>()
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        selectionScript = GetComponent<InventorySelection>();
        inventoryScript = GetComponent<Inventory>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        particlesScript = GetComponent<Particles>();
        clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        escapeScript = GetComponent<Escaping>();
        applyScript = GetComponent<ApplyPrisonData>();
        signMenuScript = GetComponent<SignMenu>();
        escObjMenuPanel = RootObjectCache.GetRoot("MenuCanvas").transform.Find("EscapeObjectMenuPanel");
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        pc = GetComponent<PauseController>();
        player = RootObjectCache.GetRoot("Player").transform;
        foreach(Transform slot in RootObjectCache.GetRoot("InventoryCanvas").transform.Find("GUIPanel"))
        {
            invSlots.Add(slot.gameObject);
        }
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SetEscapeObjects();
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(layers[i]))
            {
                if (obj.CompareTag("EscapeObject"))
                {
                    escObjs.Add(obj.gameObject);
                }
            }
        }
        CloseMenu();
    }
    private void SetEscapeObjects()
    {
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(layers[i]))
            {
                if (!obj.CompareTag("EscapeObject"))
                {
                    continue;
                }

                switch (obj.name)
                {
                    case "DTAFComputerLeft":
                        List<string> messages = new List<string>()
                        {
                            "Insert the launch keycard into the slot to activate!"
                        };
                        List<string> headers = new List<string>()
                        {
                            "NOTICE"
                        };
                        List<int> items = new List<int>()
                        {
                            181
                        };
                        obj.GetComponent<EscapeObjectHandler>().escObj = new EscapeObject(items, messages, headers);
                        break;
                    case "DTAFComputerDown":
                        messages = new List<string>()
                        {
                            "Only the soft, velvety tones of a loyal henchman will enable this!"
                        };
                        headers = new List<string>()
                        {
                            "NOTICE"
                        };
                        items = new List<int>()
                        {
                            212
                        };

                        obj.GetComponent<EscapeObjectHandler>().escObj = new EscapeObject(items, messages, headers);
                        break;
                    case "DTAFComputerRight":
                        messages = new List<string>()
                        {
                            "Only the fingerprint of the villain in charge can activate this!"
                        };
                        headers = new List<string>()
                        {
                            "NOTICE"
                        };
                        items = new List<int>()
                        {
                            171
                        };

                        obj.GetComponent<EscapeObjectHandler>().escObj = new EscapeObject(items, messages, headers);
                        break;
                    case "JingleSantaSleigh":
                        messages = new List<string>()
                        {
                            "Santa's sleigh took quite a beating during the crash. It'll need some repairs before you can fly it out of here. Firstly, a new thruster!\n\nThruster = Fuel + Metal Tubing + Lighter",
                            "The sleigh's GPS Navigation system is down. You'll have to make yourself a compass to find your way home.\n\nMakeshift Compass = Bowl Of Water + Magnetized Needle + Cork",
                            "The sleigh's high-beam headlamps were smashed during the crash. Without adequate lighting you'll be flying blind!\n\nNavigation Lighting = Work Hat + High Beam Flashlight + Duct Tape",
                            "The sleigh looks ready to leave, but the stubborn reindeers are refusing to travel on an empty stomach. Looks like you'll have to cook them their favorite snack..."
                        };
                        headers = new List<string>()
                        {
                            "SANTA'S SLEIGH",
                            "SANTA'S SLEIGH",
                            "SANTA'S SLEIGH",
                            "SANTA'S SLEIGH"
                        };
                        items = new List<int>()
                        {
                            210, 185, 198, 180
                        };

                        obj.GetComponent<EscapeObjectHandler>().escObj = new EscapeObject(items, messages, headers);
                        break;
                    case "SSTree":
                        messages = new List<string>()
                        {
                            "Let's decorate this thing!\nFirst you'll need some Tinsel!\n\nTinsel = Glitter + Toilet Roll + Glue",
                            "Next you'll need some Fairy Lights!\n\nFairy Lights = Bulbs + Wire + Battery",
                            "Now some Presents!\n\nPresents = Wooden Doll + Wrapping Paper + Sticky Tape",
                            "The tree looks magical - Santa will hate it!\n\nA strange slot at the base of the tree catches your eye. It looks like some kind of lever goes in there.",
                        };
                        headers = new List<string>()
                        {
                            "FESTIVE HINT!",
                            "FESTIVE HINT!",
                            "FESTIVE HINT!",
                            "FESTIVE HINT!",
                        };
                        items = new List<int>()
                        {
                            211, 170, 201, 161
                        };

                        obj.GetComponent<EscapeObjectHandler>().escObj = new EscapeObject(items, messages, headers);
                        break;
                    case "ETTank":
                        messages = new List<string>()
                        {
                            "First you will need a Makeshift Tank Turret.\nYou can craft one with the following elements: Makeshift Tank Firing Base, Makeshift Tank Barrel, Duct Tape",
                            "Next you will need a Makeshift Explosive Round.\nYou can craft one with the following elements: Metal Cone, Makeshift Explosive Compound",
                            "Finally you will need a Makeshift Fuse.\nYou can craft one with the following elements: Taper, Lighter"
                        };
                        headers = new List<string>()
                        {
                            "REPAIR NOTES",
                            "REPAIR NOTES",
                            "REPAIR NOTES"
                        };
                        items = new List<int>()
                        {
                            191, 186, 187
                        };

                        obj.GetComponent<EscapeObjectHandler>().escObj = new EscapeObject(items, messages, headers);
                        break;
                }
            }
        }
    }
    private void Update()
    {
        int selectedID;
        if (selectionScript.aSlotSelected)
        {
            selectedID = inventoryScript.inventory[selectionScript.selectedSlotNum].itemData.id;
        }
        else
        {
            selectedID = -1;
        }

        if (mcs.isTouchingEscapeObject && selectedID != -1 && Input.GetMouseButtonDown(0))
        {
            if (!player.Find("OutBox").GetComponent<BoxCollider2D>().IsTouching(mcs.touchedEscapeObject.GetComponent<BoxCollider2D>()))
            {
                return;
            }

            EscapeObjectHandler handler = mcs.touchedEscapeObject.GetComponent<EscapeObjectHandler>();
            EscapeObject escObj = handler.escObj;
            int objCleared = handler.objectivesCleared;

            if(selectedID != escObj.items[objCleared])
            {
                return;
            }

            StartCoroutine(particlesScript.CreateDust(mcs.touchedEscapeObject.transform.position, 1));
            inventoryScript.inventory[selectionScript.selectedSlotNum].itemData = null;
            invSlots[selectionScript.selectedSlotNum].GetComponent<Image>().sprite = clear;
            handler.objectivesCleared++;
            switch (mcs.touchedEscapeObject.name)
            {
                case "DTAFComputerDown":
                    downPCDone = true;
                    mcs.touchedEscapeObject.tag = "Untagged";
                    mcs.touchedEscapeObject.GetComponent<DTAFComputerAnimation>().enabled = true;
                    break;
                case "DTAFComputerLeft":
                    leftPCDone = true;
                    mcs.touchedEscapeObject.tag = "Untagged";
                    mcs.touchedEscapeObject.GetComponent<DTAFComputerAnimation>().enabled = true;
                    break;
                case "DTAFComputerRight":
                    rightPCDone = true;
                    mcs.touchedEscapeObject.tag = "Untagged";
                    mcs.touchedEscapeObject.GetComponent<DTAFComputerAnimation>().enabled = true;
                    break;
                case "SSTree":
                    switch (handler.objectivesCleared)
                    {
                        case 1:
                            mcs.touchedEscapeObject.GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[355];
                            break;
                        case 2:
                            mcs.touchedEscapeObject.GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[354];
                            break;
                        case 3:
                            mcs.touchedEscapeObject.GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[357];
                            break;
                        case 4:
                            mcs.touchedEscapeObject.GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[358];
                            mcs.touchedEscapeObject.tag = "Untagged";
                            for (int i = 0; i < 4; i++)
                            {
                                foreach (Transform obj in tiles.Find(layers[i]))
                                {
                                    if (obj.name == "Wall")
                                    {
                                        Destroy(obj.gameObject);
                                        StartCoroutine(particlesScript.CreateDust(obj.position, 2));
                                    }
                                }
                            }
                            break;
                    }
                    break;
                case "ETTank":
                    if(handler.objectivesCleared == 1)
                    {
                        mcs.touchedEscapeObject.GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[275];
                    }
                    if(handler.objectivesCleared == 3)
                    {
                        mcs.touchedEscapeObject.tag = "Untagged";
                        for (int i = 0; i < 4; i++)
                        {
                            foreach (Transform obj in tiles.Find(layers[i]))
                            {
                                if (obj.name == "Concrete")
                                {
                                    Destroy(obj.gameObject);
                                    StartCoroutine(particlesScript.CreateDust(obj.position, 2));
                                }
                            }
                        }
                    }
                    break;
                case "JingleSantaSleigh":
                    switch (handler.objectivesCleared)
                    {
                        case 1:
                            mcs.touchedEscapeObject.GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[379];
                            break;
                        case 2:
                            mcs.touchedEscapeObject.GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[380];
                            break;
                    }
                    if (handler.objectivesCleared == 4)
                    {
                        mcs.touchedEscapeObject.tag = "Untagged";
                        StartCoroutine(escapeScript.Escape());
                    }
                    break;
            }
            if(downPCDone && leftPCDone && rightPCDone)
            {
                StartCoroutine(escapeScript.Escape());
            }
        }
        if(mcs.isTouchingEscapeObject && selectedID == -1 && Input.GetMouseButtonDown(0))
        {
            if (!player.Find("OutBox").GetComponent<BoxCollider2D>().IsTouching(mcs.touchedEscapeObject.GetComponent<BoxCollider2D>()))
            {
                return;
            }

            EscapeObject escObj = mcs.touchedEscapeObject.GetComponent<EscapeObjectHandler>().escObj;
            int objCleared = mcs.touchedEscapeObject.GetComponent<EscapeObjectHandler>().objectivesCleared;
            if (mcs.touchedEscapeObject.name.Contains("DTAF"))
            {
                signMenuScript.OpenMenu("white", escObj.headers[objCleared], escObj.messages[objCleared]);
            }
            else
            {
                OpenMenu(escObj.headers[objCleared], escObj.messages[objCleared]);
            }
        }
    }
    public void OpenMenu(string header, string message)
    {
        mc.Find("Black").GetComponent<Image>().enabled = true;
        foreach (Transform child in escObjMenuPanel)
        {
            child.gameObject.SetActive(true);
        }
        escObjMenuPanel.Find("HeaderText").GetComponent<TextMeshProUGUI>().text = header;
        escObjMenuPanel.Find("BodyText").GetComponent<TextMeshProUGUI>().text = message;
        escObjMenuPanel.GetComponent<Image>().enabled = true;
        escObjMenuPanel.GetComponent<BoxCollider2D>().enabled = true;
        inMenu = true;
        pc.Pause(true);
    }
    public void CloseMenu()
    {
        mc.Find("Black").GetComponent<Image>().enabled = false;
        pc.Unpause();
        foreach(Transform child in escObjMenuPanel)
        {
            child.gameObject.SetActive(false);
        }
        escObjMenuPanel.GetComponent<Image>().enabled = false;
        escObjMenuPanel.GetComponent<BoxCollider2D>().enabled = false;
        inMenu = false;
    }
}
