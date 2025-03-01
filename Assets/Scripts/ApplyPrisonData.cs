using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class ApplyPrisonData : MonoBehaviour
{
    private DataSender senderScript;
    public Transform ic;
    public Transform mc;
    public Transform perksTiles;
    public InventorySelection InventorySelection;
    public VentClimb VentClimb;
    public Transform PlayerDesk;
    private string aName;
    private List<Sprite> ItemSprites = new List<Sprite>();
    private List<Sprite> NPCSprites = new List<Sprite>();
    private List<Sprite> PrisonObjectSprites = new List<Sprite>();
    private List<Sprite> UISprites = new List<Sprite>();

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
        InventorySelection.mouseNormalSprite = UISprites[41];
        InventorySelection.mousePurpleSprite = UISprites[73];
        //deskmenupanel
        mc.Find("DeskMenuPanel").GetComponent<Image>().sprite = UISprites[31];
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
        //barline
        Resources.Load("BarLine").GetComponent<Image>().sprite = UISprites[44];
        //desk (to be removed later)
        PlayerDesk.GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[143];
        //ventclimb script stuff
        VentClimb.mouseUpSprite = UISprites[74];
        VentClimb.mouseDownSprite = UISprites[77];
        VentClimb.mouseNormalSprite = UISprites[41];
        //vent covers
        Resources.Load("PerksPrefabs/Objects/EmptyVentCover").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[138];
        Resources.Load("PerksPrefabs/Objects/VentCover").GetComponent<SpriteRenderer>().sprite = PrisonObjectSprites[137];
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
        //medic sprites are in prison objects
    }
}
