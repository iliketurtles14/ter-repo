using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ApplyMainMenuData : MonoBehaviour
{
    public MemoryMappedFileReader mmfrScript;
    public LoadingPanel loadScript;
    public DataSender senderScript;
    private List<Sprite> ItemSprites;
    private List<Sprite> NPCSprites;
    private List<Sprite> PrisonObjectSprites;
    private List<Sprite> UISprites;
    public Transform mmc;
    private bool hasApplied;
    public List<Sprite> InmateOutiftSprites = new List<Sprite>();
    public List<Sprite> GuardOutfitSprites = new List<Sprite>();
    public List<Sprite> RabbitSprites = new List<Sprite>();
    public List<Sprite> BaldEagleSprites = new List<Sprite>();
    public List<Sprite> LiferSprites = new List<Sprite>();
    public List<Sprite> YoungBuckSprites = new List<Sprite>();
    public List<Sprite> OldTimerSprites = new List<Sprite>();
    public List<Sprite> BillyGoatSprites = new List<Sprite>();
    public List<Sprite> FrosephSprites = new List<Sprite>();
    public List<Sprite> TangoSprites = new List<Sprite>();
    public List<Sprite> MaruSprites = new List<Sprite>();

    public void Update()
    {
        if(mmfrScript.canApply && !hasApplied)
        {

            hasApplied = true;

            ItemSprites = ConvertTexture2DListToSpriteList(mmfrScript.ItemImages);
            NPCSprites = ConvertTexture2DListToSpriteList(mmfrScript.NPCImages);
            PrisonObjectSprites = ConvertTexture2DListToSpriteList(mmfrScript.PrisonObjectImages);
            UISprites = ConvertTexture2DListToSpriteList(mmfrScript.UIImages);

            senderScript.SetFullLists(ItemSprites, NPCSprites, PrisonObjectSprites, UISprites);

            LoadImages();
            loadScript.LogLoad("Images Applied");

        }
    }
    private List<Sprite> ConvertTexture2DListToSpriteList(List<Texture2D> textures)
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach (Texture2D texture in textures)
        {
            Sprite sprite = Texture2DToSprite(texture);
            sprites.Add(sprite);
        }
        return sprites;
    }

    private Sprite Texture2DToSprite(Texture2D texture)
    {
        // Set the filter mode to Point (no filter) for pixel-perfect rendering
        texture.filterMode = FilterMode.Point;

        // Create the sprite with the appropriate pixels per unit
        float pixelsPerUnit = 100f; // Adjust this value based on your texture resolution and desired size
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
    }
    public void LoadImages()
    {
        //mouse
        mmc.Find("MouseOverlay").GetComponent<Image>().sprite = UISprites[41];
        //main menu buttons
        mmc.Find("TitlePanel").Find("PlayButton").GetComponent<Image>().sprite = UISprites[198];
        mmc.Find("TitlePanel").Find("OptionsButton").GetComponent<Image>().sprite = UISprites[198];
        mmc.Find("TitlePanel").Find("MapEditorButton").GetComponent<Image>().sprite = UISprites[198];
        //prison select backdrop
        mmc.Find("PrisonSelectPanel").GetComponent<Image>().sprite = UISprites[208];
        //arrows
        mmc.Find("PrisonSelectPanel").Find("LeftArrow").GetComponent<Image>().sprite = UISprites[203];
        mmc.Find("PrisonSelectPanel").Find("RightArrow").GetComponent<Image>().sprite = UISprites[200];
        //prison select buttons
        mmc.Find("PrisonSelectPanel").Find("BackButton").GetComponent<Image>().sprite = UISprites[248];
        mmc.Find("PrisonSelectPanel").Find("ContinueButton").GetComponent<Image>().sprite = UISprites[248];
        //prison image
        mmc.Find("PrisonSelectPanel").Find("PrisonImage").GetComponent<Image>().sprite = UISprites[519];
        //player menu buttons
        mmc.Find("PlayerPanel").Find("BackButton").GetComponent<Image>().sprite = UISprites[248];
        mmc.Find("PlayerPanel").Find("ContinueButton").GetComponent<Image>().sprite = UISprites[248];
        //arrows
        mmc.Find("PlayerPanel").Find("LeftArrow").GetComponent<Image>().sprite = UISprites[203];
        mmc.Find("PlayerPanel").Find("RightArrow").GetComponent<Image>().sprite = UISprites[200];
        //player menu backdrop
        mmc.Find("PlayerPanel").GetComponent<Image>().sprite = UISprites[184];
        //player backdrop
        mmc.Find("PlayerPanel").Find("NPCBackdrop").GetComponent<Image>().sprite = UISprites[255];
        //name box
        mmc.Find("PlayerPanel").Find("NameBox").GetComponent<Image>().sprite = UISprites[186];
        //small menu panel set button
        mmc.Find("SmallMenuPanel").Find("SetButton").GetComponent<Image>().sprite = UISprites[326];
        //arros
        mmc.Find("SmallMenuPanel").Find("LeftArrow").GetComponent<Image>().sprite = UISprites[203];
        mmc.Find("SmallMenuPanel").Find("RightArrow").GetComponent<Image>().sprite = UISprites[200];
        //npc backdrop
        mmc.Find("SmallMenuPanel").Find("NPCBackdrop").GetComponent<Image>().sprite = UISprites[255];
        //name box
        mmc.Find("SmallMenuPanel").Find("NameBox").GetComponent<Image>().sprite = UISprites[186];
        //npc customize panel backdrop
        mmc.Find("NPCCustomizePanel").GetComponent<Image>().sprite = UISprites[320];
        //npc backdrop
        mmc.Find("NPCCustomizePanel").Find("NPCBackdrop").GetComponent<Image>().sprite = UISprites[321];
        //name box
        mmc.Find("NPCCustomizePanel").Find("NameBox").GetComponent<Image>().sprite = UISprites[322];
        //npc customize panel buttons
        mmc.Find("NPCCustomizePanel").Find("BackButton").GetComponent<Image>().sprite = UISprites[326];
        mmc.Find("NPCCustomizePanel").Find("RandomButton").GetComponent<Image>().sprite = UISprites[330];
        mmc.Find("NPCCustomizePanel").Find("StartGameButton").GetComponent<Image>().sprite = UISprites[328];
        //script images
        mmc.Find("TitlePanel").GetComponent<OnMainButtonPress>().ButtonNormalSprite = UISprites[198];
        mmc.Find("TitlePanel").GetComponent<OnMainButtonPress>().ButtonPressedSprite = UISprites[199];
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().prisonSprites.Add(UISprites[518]);
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().prisonSprites.Add(UISprites[519]);
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().prisonSprites.Add(UISprites[520]);
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().prisonSprites.Add(UISprites[521]);
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().prisonSprites.Add(UISprites[522]);
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().prisonSprites.Add(UISprites[523]);
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().prisonSprites.Add(UISprites[524]);
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().ButtonNormalSprite = UISprites[248];
        mmc.Find("PrisonSelectPanel").GetComponent<PrisonSelect>().ButtonPressedSprite = UISprites[249];
        mmc.Find("PlayerPanel").GetComponent<PlayerMenu>().normalSprite = UISprites[248];
        mmc.Find("PlayerPanel").GetComponent<PlayerMenu>().pressedSprite = UISprites[249];
        mmc.Find("SmallMenuPanel").GetComponent<SmallMenu>().normalSetSprite = UISprites[326];
        mmc.Find("SmallMenuPanel").GetComponent<SmallMenu>().pressedSetSprite = UISprites[327];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().BackButtonNormalSprite = UISprites[326];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().BackButtonPressedSprite = UISprites[327];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().RandomButtonNormalSprite = UISprites[330];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().RandomButtonPressedSprite = UISprites[331];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().StartButtonNormalSprite = UISprites[328];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().StartButtonPressedSprite = UISprites[329];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().SelectHoverSprite = UISprites[319];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().SelectPressedSprite = UISprites[318];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().LeftArrowSprite = UISprites[203];
        mmc.Find("NPCCustomizePanel").GetComponent<NPCRename>().RightArrowSprite = UISprites[200];
        //npc lists
        BaldEagleSprites.Add(NPCSprites[92]);
        BaldEagleSprites.Add(NPCSprites[91]);
        BaldEagleSprites.Add(NPCSprites[70]);
        BaldEagleSprites.Add(NPCSprites[71]);
        BaldEagleSprites.Add(NPCSprites[93]);
        BaldEagleSprites.Add(NPCSprites[94]);
        BaldEagleSprites.Add(NPCSprites[68]);
        BaldEagleSprites.Add(NPCSprites[69]);
        BillyGoatSprites.Add(NPCSprites[206]);
        BillyGoatSprites.Add(NPCSprites[207]);
        BillyGoatSprites.Add(NPCSprites[257]);
        BillyGoatSprites.Add(NPCSprites[256]);
        BillyGoatSprites.Add(NPCSprites[204]);
        BillyGoatSprites.Add(NPCSprites[205]);
        BillyGoatSprites.Add(NPCSprites[202]);
        BillyGoatSprites.Add(NPCSprites[203]);
        FrosephSprites.Add(NPCSprites[167]);
        FrosephSprites.Add(NPCSprites[168]);
        FrosephSprites.Add(NPCSprites[173]);
        FrosephSprites.Add(NPCSprites[174]);
        FrosephSprites.Add(NPCSprites[169]);
        FrosephSprites.Add(NPCSprites[170]);
        FrosephSprites.Add(NPCSprites[171]);
        FrosephSprites.Add(NPCSprites[172]);
        LiferSprites.Add(NPCSprites[135]);
        LiferSprites.Add(NPCSprites[136]);
        LiferSprites.Add(NPCSprites[141]);
        LiferSprites.Add(NPCSprites[142]);
        LiferSprites.Add(NPCSprites[137]);
        LiferSprites.Add(NPCSprites[138]);
        LiferSprites.Add(NPCSprites[139]);
        LiferSprites.Add(NPCSprites[140]);
        MaruSprites.Add(NPCSprites[87]);
        MaruSprites.Add(NPCSprites[88]);
        MaruSprites.Add(NPCSprites[66]);
        MaruSprites.Add(NPCSprites[67]);
        MaruSprites.Add(NPCSprites[89]);
        MaruSprites.Add(NPCSprites[90]);
        MaruSprites.Add(NPCSprites[64]);
        MaruSprites.Add(NPCSprites[65]);
        OldTimerSprites.Add(NPCSprites[237]);
        OldTimerSprites.Add(NPCSprites[238]);
        OldTimerSprites.Add(NPCSprites[245]);
        OldTimerSprites.Add(NPCSprites[246]);
        OldTimerSprites.Add(NPCSprites[239]);
        OldTimerSprites.Add(NPCSprites[240]);
        OldTimerSprites.Add(NPCSprites[233]);
        OldTimerSprites.Add(NPCSprites[234]);
        RabbitSprites.Add(NPCSprites[75]);
        RabbitSprites.Add(NPCSprites[76]);
        RabbitSprites.Add(NPCSprites[81]);
        RabbitSprites.Add(NPCSprites[82]);
        RabbitSprites.Add(NPCSprites[78]);
        RabbitSprites.Add(NPCSprites[77]);
        RabbitSprites.Add(NPCSprites[79]);
        RabbitSprites.Add(NPCSprites[80]);
        TangoSprites.Add(NPCSprites[188]);
        TangoSprites.Add(NPCSprites[189]);
        TangoSprites.Add(NPCSprites[196]);
        TangoSprites.Add(NPCSprites[197]);
        TangoSprites.Add(NPCSprites[190]);
        TangoSprites.Add(NPCSprites[191]);
        TangoSprites.Add(NPCSprites[184]);
        TangoSprites.Add(NPCSprites[185]);
        YoungBuckSprites.Add(NPCSprites[221]);
        YoungBuckSprites.Add(NPCSprites[222]);
        YoungBuckSprites.Add(NPCSprites[227]);
        YoungBuckSprites.Add(NPCSprites[228]);
        YoungBuckSprites.Add(NPCSprites[223]);
        YoungBuckSprites.Add(NPCSprites[224]);
        YoungBuckSprites.Add(NPCSprites[217]);
        YoungBuckSprites.Add(NPCSprites[218]);
        InmateOutiftSprites.Add(NPCSprites[83]);
        InmateOutiftSprites.Add(NPCSprites[84]);
        InmateOutiftSprites.Add(NPCSprites[61]);
        InmateOutiftSprites.Add(NPCSprites[62]);
        InmateOutiftSprites.Add(NPCSprites[85]);
        InmateOutiftSprites.Add(NPCSprites[86]);
        InmateOutiftSprites.Add(NPCSprites[73]);
        InmateOutiftSprites.Add(NPCSprites[72]);
        GuardOutfitSprites.Add(NPCSprites[639]);
        GuardOutfitSprites.Add(NPCSprites[42]);
        GuardOutfitSprites.Add(NPCSprites[43]);
        GuardOutfitSprites.Add(NPCSprites[44]);
        GuardOutfitSprites.Add(NPCSprites[40]);
        GuardOutfitSprites.Add(NPCSprites[41]);
        GuardOutfitSprites.Add(NPCSprites[45]);
        GuardOutfitSprites.Add(NPCSprites[332]);

        senderScript.SetNPCLists(InmateOutiftSprites, GuardOutfitSprites, RabbitSprites, BaldEagleSprites,
            LiferSprites, YoungBuckSprites, OldTimerSprites, BillyGoatSprites, FrosephSprites,
            TangoSprites, MaruSprites);
    }

}