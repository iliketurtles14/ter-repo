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
    public Transform InventorySelection;
    public Transform PlayerDesk;
    private List<Sprite> ItemSprites = new List<Sprite>();
    private List<Sprite> NPCSprites = new List<Sprite>();
    private List<Sprite> PrisonObjectSprites = new List<Sprite>();
    private List<Sprite> UISprites = new List<Sprite>();

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
        InventorySelection.GetComponent<InventorySelection>().mouseNormalSprite = UISprites[41];
        InventorySelection.GetComponent<InventorySelection>().mousePurpleSprite = UISprites[73];
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
    }
}
