using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

public class ApplyPrisonData : MonoBehaviour
{
    private DataSender senderScript;
    public Transform ic;
    public Transform mc;
    public Transform perksTiles;
    public InventorySelection InventorySelection;
    public VentClimb VentClimb;
    public ItemBehaviours itemBehavioursScript;
    public HoleClimb holeClimbScript;
    public MouseOverlay mouseOverlayScript;
    public Transform PlayerDesk;
    public Pause pauseScript;
    private string aName;
    private List<Sprite> ItemSprites = new List<Sprite>();
    private List<Sprite> NPCSprites = new List<Sprite>();
    private List<Sprite> PrisonObjectSprites = new List<Sprite>();
    private List<Sprite> UISprites = new List<Sprite>();


    ///NPC SPRITE RULES:
    //start with looking east with foot out,
    //then north with foot out being the right
    //then west with foot out
    //then south with foot out being the left (yes its technically supposed to be the right foot. whatever.)
    public List<Sprite> RabbitHoldingSprites = new List<Sprite>();
    public List<Sprite> BaldEagleHoldingSprites = new List<Sprite>();
    public List<Sprite> LiferHoldingSprites = new List<Sprite>();
    public List<Sprite> YoungBuckHoldingSprites = new List<Sprite>();
    public List<Sprite> OldTimerHoldingSprites = new List<Sprite>();
    public List<Sprite> BillyGoatHoldingSprites = new List<Sprite>();
    public List<Sprite> FrosephHoldingSprites = new List<Sprite>();
    public List<Sprite> TangoHoldingSprites = new List<Sprite>();
    public List<Sprite> MaruHoldingSprites = new List<Sprite>();
    public List<Sprite> InmateOutfitHoldingSprites = new List<Sprite>();

    public List<Sprite> RabbitPunchingSprites = new List<Sprite>();
    public List<Sprite> BaldEaglePunchingSprites = new List<Sprite>();
    public List<Sprite> LiferPunchingSprites = new List<Sprite>();
    public List<Sprite> YoungBuckPunchingSprites = new List<Sprite>();
    public List<Sprite> OldTimerPunchingSprites = new List<Sprite>();
    public List<Sprite> BillyGoatPunchingSprites = new List<Sprite>();
    public List<Sprite> FrosephPunchingSprites = new List<Sprite>();
    public List<Sprite> TangoPunchingSprites = new List<Sprite>();
    public List<Sprite> MaruPunchingSprites = new List<Sprite>();
    public List<Sprite> InmateOutfitPunchingSprites = new List<Sprite>();

    private void Start()
    {
        senderScript = DataSender.instance;

        ItemSprites = senderScript.ItemImages;
        NPCSprites = senderScript.NPCImages;
        PrisonObjectSprites = senderScript.PrisonObjectImages;
        UISprites = senderScript.UIImages;

        LoadImages();
    }
    private Sprite Cutter(Sprite sprite, int x, int y, int width, int height)
    {
        Rect rect = new Rect(x, y, width, height);
        Texture2D texture = sprite.texture;
        Sprite newSprite = Sprite.Create(texture, rect, new Vector2(.5f, .5f), sprite.pixelsPerUnit);
        return newSprite;
    }
    private void LoadImages()
    {
        //tooltip panel
        ic.Find("ActionBar").GetComponent<Image>().sprite = UISprites[193];
        //mouse
        ic.Find("MouseOverlay").GetComponent<Image>().sprite = UISprites[41];
        mouseOverlayScript.mouseNormal = UISprites[41];
        mouseOverlayScript.mousePurple = UISprites[73];
        mouseOverlayScript.mouseUp = UISprites[74];
        mouseOverlayScript.mouseDown = UISprites[77];
        //deskmenupanels
        mc.Find("PlayerDeskMenuPanel").GetComponent<Image>().sprite = UISprites[31];
        mc.Find("DevDeskMenuPanel").GetComponent<Image>().sprite = UISprites[31];
        foreach(Transform child in mc)
        {
            if (child.name.StartsWith("DeskMenuPanel"))
            {
                child.GetComponent<Image>().sprite = UISprites[31];
            }
        }
        //inventory ui (heart, energy, etc.)
        ic.Find("HealthSprite").GetComponent<Image>().sprite = Cutter(UISprites[63], 99, 78, 9, 11);
        ic.Find("HeatSprite").GetComponent<Image>().sprite = Cutter(UISprites[64], 99, 79, 9, 10);
        ic.Find("MoneySprite").GetComponent<Image>().sprite = Cutter(UISprites[45], 37, 78, 8, 11);
        ic.Find("EnergySprite").GetComponent<Image>().sprite = Cutter(UISprites[150], 98, 78, 10, 11);
        ic.Find("FriendsSprite").GetComponent<Image>().sprite = Cutter(UISprites[273], 100, 78, 8, 11);
        //playermenu stat icons
        mc.Find("PlayerMenuPanel").Find("StrengthIcon").GetComponent<Image>().sprite = UISprites[450];
        mc.Find("PlayerMenuPanel").Find("SpeedIcon").GetComponent<Image>().sprite = UISprites[449];
        mc.Find("PlayerMenuPanel").Find("IntellectIcon").GetComponent<Image>().sprite = UISprites[451];
        //playermenu intellect bar (the others are custom)
        mc.Find("PlayerMenuPanel").Find("IntellectPanel").Find("IntellectBar").GetComponent<Image>().sprite = UISprites[44];
        //playermenu player backdrop
        mc.Find("PlayerMenuPanel").Find("PlayerBackdrop").GetComponent<Image>().sprite = UISprites[255];
        //playermenu item slots
        mc.Find("PlayerMenuPanel").Find("OutfitBackdrop").GetComponent<Image>().sprite = UISprites[33];
        mc.Find("PlayerMenuPanel").Find("WeaponBackdrop").GetComponent<Image>().sprite = UISprites[33];
        //pause menu stuff
        mc.Find("PauseMenuPanel").GetComponent<Image>().sprite = UISprites[357];
        mc.Find("PauseMenuPanel").Find("ContinueButton").GetComponent<Image>().sprite = UISprites[359];
        mc.Find("PauseMenuPanel").Find("OptionsButton").GetComponent<Image>().sprite = UISprites[359];
        mc.Find("PauseMenuPanel").Find("HelpButton").GetComponent<Image>().sprite = UISprites[359];
        mc.Find("PauseMenuPanel").Find("QuitButton").GetComponent<Image>().sprite = UISprites[359];
        pauseScript.buttonNormalSprite = UISprites[359];
        pauseScript.buttonPressedSprite = UISprites[360];

        //barline
        Resources.Load("BarLine").GetComponent<Image>().sprite = UISprites[44];
        //desks
        perksTiles.Find("GroundObjects").Find("PlayerDesk").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[143];
        foreach(Transform child in perksTiles.Find("GroundObjects"))
        {
            if (child.name.StartsWith("Desk"))
            {
                child.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[51];
            }
        }
        //vent covers
        Resources.Load("PerksPrefabs/Objects/EmptyVentCover").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[138];
        Resources.Load("PerksPrefabs/Objects/VentCover").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[137];
        //rope and grapple
        Resources.Load("PerksPrefabs/Objects/SheetRope").GetComponent<SpriteRenderer>().sprite = UISprites[162];
        Resources.Load("PerksPrefabs/Objects/Rope").GetComponent<SpriteRenderer>().sprite = UISprites[163];
        Resources.Load("PerksPrefabs/Objects/Grapple").GetComponent<SpriteRenderer>().sprite = UISprites[163];
        //holes
        Resources.Load("PerksPrefabs/Objects/100%HoleDown").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[132];
        Resources.Load("PerksPrefabs/Objects/100%HoleUp").GetComponent<Light2D>().lightCookieSprite = UISprites[38];
        itemBehavioursScript.hole24 = PrisonObjectSprites[136];
        itemBehavioursScript.hole49 = PrisonObjectSprites[135];
        itemBehavioursScript.hole74 = PrisonObjectSprites[134];
        itemBehavioursScript.hole99 = PrisonObjectSprites[133];
        itemBehavioursScript.holeUp24 = UISprites[34];
        itemBehavioursScript.holeUp49 = UISprites[35];
        itemBehavioursScript.holeUp74 = UISprites[36];
        itemBehavioursScript.holeUp99 = UISprites[37];
        //dirt
        Resources.Load("PerksPrefabs/Underground/Dirt").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[24];
        Resources.Load("PerksPrefabs/Underground/DirtEmpty").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[25];
        perksTiles.transform.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[24];
        perksTiles.transform.Find("UndergroundPlane").GetComponent<SpriteRenderer>().size = new Vector2(20, 20);
        //brace
        Resources.Load("PerksPrefabs/Objects/Brace").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[96];
        //rock
        Resources.Load("PerksPrefabs/Objects/Rock").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[41];
        //other vent objects
        perksTiles.Find("VentObjects").gameObject.SetActive(true);
        foreach (Transform child in perksTiles.Find("VentObjects"))
        {
            if (child.name.IndexOf(" (") != -1)
            {
                int index = child.name.IndexOf(" (");
                aName = child.name.Remove(index, child.name.Length - index);
            }
            else
            {
                aName = child.name;
            }
            switch (aName)
            {
                case "LadderDown":
                    child.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[140];
                    break;
                case "LadderUp":
                    child.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[147];
                    break;
                case "SlatsHorizontal":
                    child.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[139];
                    break;
                case "SlatsVertical":
                    child.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[40];
                    break;
                case "VentCover":
                    child.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[137];
                    break;
            }
        }
        perksTiles.Find("VentObjects").gameObject.SetActive(false);
        //ground objects
        foreach(Transform child in perksTiles.Find("GroundObjects"))
        {
            if (child.name.IndexOf(" (") != -1)
            {
                int index = child.name.IndexOf(" (");
                aName = child.name.Remove(index, child.name.Length - index);
            }
            else
            {
                aName = child.name;
            }
            switch (aName)
            {
                case "LadderUp":
                    child.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[147];
                    break;
            }
        }
        //roof objects
        perksTiles.Find("RoofObjects").gameObject.SetActive(true);
        foreach (Transform child in perksTiles.Find("RoofObjects"))
        {
            if (child.name.IndexOf(" (") != -1)
            {
                int index = child.name.IndexOf(" (");
                aName = child.name.Remove(index, child.name.Length - index);
            }
            else
            {
                aName = child.name;
            }
            switch (aName)
            {
                case "LadderDown":
                    child.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[140];
                    break;
            }
        }
        perksTiles.Find("RoofObjects").gameObject.SetActive(false);
        //holding sprites
        RabbitHoldingSprites.Add(NPCSprites[127]);
        RabbitHoldingSprites.Add(NPCSprites[128]);
        RabbitHoldingSprites.Add(NPCSprites[133]);
        RabbitHoldingSprites.Add(NPCSprites[134]);
        RabbitHoldingSprites.Add(NPCSprites[129]);
        RabbitHoldingSprites.Add(NPCSprites[130]);
        RabbitHoldingSprites.Add(NPCSprites[131]);
        RabbitHoldingSprites.Add(NPCSprites[132]);
        BaldEagleHoldingSprites.Add(NPCSprites[404]);
        BaldEagleHoldingSprites.Add(NPCSprites[405]);
        BaldEagleHoldingSprites.Add(NPCSprites[410]);
        BaldEagleHoldingSprites.Add(NPCSprites[411]);
        BaldEagleHoldingSprites.Add(NPCSprites[406]);
        BaldEagleHoldingSprites.Add(NPCSprites[407]);
        BaldEagleHoldingSprites.Add(NPCSprites[409]);
        BaldEagleHoldingSprites.Add(NPCSprites[408]);
        LiferHoldingSprites.Add(NPCSprites[444]);
        LiferHoldingSprites.Add(NPCSprites[445]);
        LiferHoldingSprites.Add(NPCSprites[450]);
        LiferHoldingSprites.Add(NPCSprites[451]);
        LiferHoldingSprites.Add(NPCSprites[446]);
        LiferHoldingSprites.Add(NPCSprites[447]);
        LiferHoldingSprites.Add(NPCSprites[449]);
        LiferHoldingSprites.Add(NPCSprites[448]);
        YoungBuckHoldingSprites.Add(NPCSprites[412]);
        YoungBuckHoldingSprites.Add(NPCSprites[413]);
        YoungBuckHoldingSprites.Add(NPCSprites[416]);
        YoungBuckHoldingSprites.Add(NPCSprites[417]);
        YoungBuckHoldingSprites.Add(NPCSprites[415]);
        YoungBuckHoldingSprites.Add(NPCSprites[414]);
        YoungBuckHoldingSprites.Add(NPCSprites[419]);
        YoungBuckHoldingSprites.Add(NPCSprites[418]);
        OldTimerHoldingSprites.Add(NPCSprites[436]);
        OldTimerHoldingSprites.Add(NPCSprites[437]);
        OldTimerHoldingSprites.Add(NPCSprites[442]);
        OldTimerHoldingSprites.Add(NPCSprites[443]);
        OldTimerHoldingSprites.Add(NPCSprites[438]);
        OldTimerHoldingSprites.Add(NPCSprites[439]);
        OldTimerHoldingSprites.Add(NPCSprites[441]);
        OldTimerHoldingSprites.Add(NPCSprites[442]);
        BillyGoatHoldingSprites.Add(NPCSprites[428]);
        BillyGoatHoldingSprites.Add(NPCSprites[429]);
        BillyGoatHoldingSprites.Add(NPCSprites[432]);
        BillyGoatHoldingSprites.Add(NPCSprites[433]);
        BillyGoatHoldingSprites.Add(NPCSprites[430]);
        BillyGoatHoldingSprites.Add(NPCSprites[431]);
        BillyGoatHoldingSprites.Add(NPCSprites[435]);
        BillyGoatHoldingSprites.Add(NPCSprites[434]);
        FrosephHoldingSprites.Add(NPCSprites[455]);
        FrosephHoldingSprites.Add(NPCSprites[454]);
        FrosephHoldingSprites.Add(NPCSprites[458]);
        FrosephHoldingSprites.Add(NPCSprites[459]);
        FrosephHoldingSprites.Add(NPCSprites[456]);
        FrosephHoldingSprites.Add(NPCSprites[457]);
        FrosephHoldingSprites.Add(NPCSprites[453]);
        FrosephHoldingSprites.Add(NPCSprites[452]);
        TangoHoldingSprites.Add(NPCSprites[420]);
        TangoHoldingSprites.Add(NPCSprites[421]);
        TangoHoldingSprites.Add(NPCSprites[424]);
        TangoHoldingSprites.Add(NPCSprites[425]);
        TangoHoldingSprites.Add(NPCSprites[423]);
        TangoHoldingSprites.Add(NPCSprites[422]);
        TangoHoldingSprites.Add(NPCSprites[427]);
        TangoHoldingSprites.Add(NPCSprites[426]);
        MaruHoldingSprites.Add(NPCSprites[1283]);
        MaruHoldingSprites.Add(NPCSprites[1284]);
        MaruHoldingSprites.Add(NPCSprites[1287]);
        MaruHoldingSprites.Add(NPCSprites[1288]);
        MaruHoldingSprites.Add(NPCSprites[1281]);
        MaruHoldingSprites.Add(NPCSprites[1282]);
        MaruHoldingSprites.Add(NPCSprites[1286]);
        MaruHoldingSprites.Add(NPCSprites[1285]);
        InmateOutfitHoldingSprites.Add(NPCSprites[122]);
        InmateOutfitHoldingSprites.Add(NPCSprites[121]);
        InmateOutfitHoldingSprites.Add(NPCSprites[125]);
        InmateOutfitHoldingSprites.Add(NPCSprites[126]);
        InmateOutfitHoldingSprites.Add(NPCSprites[123]);
        InmateOutfitHoldingSprites.Add(NPCSprites[124]);
        InmateOutfitHoldingSprites.Add(NPCSprites[120]);
        InmateOutfitHoldingSprites.Add(NPCSprites[119]);

        //punching sprites (only on intervals of one instead of two)
        RabbitPunchingSprites.Add(NPCSprites[359]);
        RabbitPunchingSprites.Add(NPCSprites[348]);
        RabbitPunchingSprites.Add(NPCSprites[360]);
        RabbitPunchingSprites.Add(NPCSprites[333]);
        BaldEaglePunchingSprites.Add(NPCSprites[372]);
        BaldEaglePunchingSprites.Add(NPCSprites[353]);
        BaldEaglePunchingSprites.Add(NPCSprites[373]);
        BaldEaglePunchingSprites.Add(NPCSprites[358]);
        LiferPunchingSprites.Add(NPCSprites[361]);
        LiferPunchingSprites.Add(NPCSprites[367]);
        LiferPunchingSprites.Add(NPCSprites[362]);
        LiferPunchingSprites.Add(NPCSprites[363]);
        YoungBuckPunchingSprites.Add(NPCSprites[346]);
        YoungBuckPunchingSprites.Add(NPCSprites[349]);
        YoungBuckPunchingSprites.Add(NPCSprites[347]);
        YoungBuckPunchingSprites.Add(NPCSprites[357]);
        OldTimerPunchingSprites.Add(NPCSprites[340]);
        OldTimerPunchingSprites.Add(NPCSprites[352]);
        OldTimerPunchingSprites.Add(NPCSprites[341]);
        OldTimerPunchingSprites.Add(NPCSprites[356]);
        BillyGoatPunchingSprites.Add(NPCSprites[342]);
        BillyGoatPunchingSprites.Add(NPCSprites[351]);
        BillyGoatPunchingSprites.Add(NPCSprites[343]);
        BillyGoatPunchingSprites.Add(NPCSprites[355]);
        FrosephPunchingSprites.Add(NPCSprites[369]);
        FrosephPunchingSprites.Add(NPCSprites[368]);
        FrosephPunchingSprites.Add(NPCSprites[370]);
        FrosephPunchingSprites.Add(NPCSprites[371]);
        TangoPunchingSprites.Add(NPCSprites[344]);
        TangoPunchingSprites.Add(NPCSprites[350]);
        TangoPunchingSprites.Add(NPCSprites[345]);
        TangoPunchingSprites.Add(NPCSprites[354]);
        MaruPunchingSprites.Add(NPCSprites[337]);
        MaruPunchingSprites.Add(NPCSprites[336]);
        MaruPunchingSprites.Add(NPCSprites[338]);
        MaruPunchingSprites.Add(NPCSprites[339]);
        InmateOutfitPunchingSprites.Add(NPCSprites[365]);
        InmateOutfitPunchingSprites.Add(NPCSprites[364]);
        InmateOutfitPunchingSprites.Add(NPCSprites[366]);
        InmateOutfitPunchingSprites.Add(NPCSprites[63]); //holy fuck why was this placed in a completely different place


        //medic sprites are in prison objects
    }
}
