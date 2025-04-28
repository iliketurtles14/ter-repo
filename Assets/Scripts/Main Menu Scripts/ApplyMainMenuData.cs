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
    public List<Sprite> POWOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> MedicOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> SoldierOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> PrisonerOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> TuxOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> HenchmanOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> ElfOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> GuardElfOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> RabbitSprites = new List<Sprite>();
    public List<Sprite> BaldEagleSprites = new List<Sprite>();
    public List<Sprite> LiferSprites = new List<Sprite>();
    public List<Sprite> YoungBuckSprites = new List<Sprite>();
    public List<Sprite> OldTimerSprites = new List<Sprite>();
    public List<Sprite> BillyGoatSprites = new List<Sprite>();
    public List<Sprite> FrosephSprites = new List<Sprite>();
    public List<Sprite> TangoSprites = new List<Sprite>();
    public List<Sprite> MaruSprites = new List<Sprite>();
    public List<Sprite> BuddyWalkingSprites = new List<Sprite>();
    public List<Sprite> ClintWalkingSprites = new List<Sprite>();
    public List<Sprite> ConnellyWalkingSprites = new List<Sprite>();
    public List<Sprite> AndyWalkingSprites = new List<Sprite>();
    public List<Sprite> BlackElfWalkingSprites = new List<Sprite>();
    public List<Sprite> BlondeWalkingSprites = new List<Sprite>();
    public List<Sprite> BrownElfWalkingSprites = new List<Sprite>();
    public List<Sprite> CageWalkingSprites = new List<Sprite>();
    public List<Sprite> ChenWalkingSprites = new List<Sprite>();
    public List<Sprite> CraneWalkingSprites = new List<Sprite>();
    public List<Sprite> ElbrahWalkingSprites = new List<Sprite>();
    public List<Sprite> GenieWalkingSprites = new List<Sprite>();
    public List<Sprite> HenchmanWalkingSprites = new List<Sprite>();
    public List<Sprite> IceElfWalkingSprites = new List<Sprite>();
    public List<Sprite> LazeeboiWalkingSprites = new List<Sprite>();
    public List<Sprite> MournWalkingSprites = new List<Sprite>();
    public List<Sprite> OrangeElfWalkingSprites = new List<Sprite>();
    public List<Sprite> PiersWalkingSprites = new List<Sprite>();
    public List<Sprite> PinkElfWalkingSprites = new List<Sprite>();
    public List<Sprite> ProwlerWalkingSprites = new List<Sprite>();
    public List<Sprite> SeanWalkingSprites = new List<Sprite>();
    public List<Sprite> SoldierWalkingSprites = new List<Sprite>();
    public List<Sprite> GuardElfWalkingSprites = new List<Sprite>();
    public List<Sprite> WaltonWalkingSprites = new List<Sprite>();
    public List<Sprite> WhiteElfWalkingSprites = new List<Sprite>();
    public List<Sprite> YellowElfWalkingSprites = new List<Sprite>();


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
        mmc.Find("TitlePanel").Find("PatchNotesButton").GetComponent<Image>().sprite = UISprites[328];
        //options stuff
        mmc.Find("OptionsPanel").Find("BackButton").GetComponent<Image>().sprite = UISprites[326];
        mmc.Find("OptionsPanel").Find("NormalizeCheckBox").GetComponent<Image>().sprite = UISprites[215];
        mmc.Find("OptionsPanel").Find("OptionsTextBackdrop").GetComponent<Image>().sprite = UISprites[186];
        mmc.Find("OptionsPanel").Find("SaveButton").GetComponent<Image>().sprite = UISprites[326];
        mmc.Find("OptionsPanel").GetComponent<Image>().sprite = UISprites[184];
        //patch notes buttosns
        mmc.Find("PatchNotesPanel").Find("BackButton").GetComponent<Image>().sprite = UISprites[248];
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
        mmc.Find("TitlePanel").GetComponent<OnMainButtonPress>().PatchNotesButtonNormalSprite = UISprites[328];
        mmc.Find("TitlePanel").GetComponent<OnMainButtonPress>().PatchNotesButtonPressedSprite = UISprites[329];
        mmc.Find("OptionsPanel").GetComponent<Options>().backButtonNormalSprite = UISprites[326];
        mmc.Find("OptionsPanel").GetComponent<Options>().backButtonPressedSprite = UISprites[327];
        mmc.Find("OptionsPanel").GetComponent<Options>().checkedBoxSprite = UISprites[216];
        mmc.Find("OptionsPanel").GetComponent<Options>().uncheckedBoxSprite = UISprites[215];
        mmc.Find("PatchNotesPanel").GetComponent<PatchNotes>().buttonNormalSprite = UISprites[248];
        mmc.Find("PatchNotesPanel").GetComponent<PatchNotes>().buttonPressedSprite = UISprites[249];
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
        BuddyWalkingSprites.Add(NPCSprites[2233]);
        BuddyWalkingSprites.Add(NPCSprites[2234]);
        BuddyWalkingSprites.Add(NPCSprites[2240]);
        BuddyWalkingSprites.Add(NPCSprites[2239]);
        BuddyWalkingSprites.Add(NPCSprites[2235]);
        BuddyWalkingSprites.Add(NPCSprites[2236]);
        BuddyWalkingSprites.Add(NPCSprites[2237]);
        BuddyWalkingSprites.Add(NPCSprites[2238]);
        ClintWalkingSprites.Add(NPCSprites[1490]);
        ClintWalkingSprites.Add(NPCSprites[1491]);
        ClintWalkingSprites.Add(NPCSprites[1496]);
        ClintWalkingSprites.Add(NPCSprites[1497]);
        ClintWalkingSprites.Add(NPCSprites[1492]);
        ClintWalkingSprites.Add(NPCSprites[1493]);
        ClintWalkingSprites.Add(NPCSprites[1495]);
        ClintWalkingSprites.Add(NPCSprites[1496]);
        ConnellyWalkingSprites.Add(NPCSprites[2002]);
        ConnellyWalkingSprites.Add(NPCSprites[2003]);
        ConnellyWalkingSprites.Add(NPCSprites[2007]);
        ConnellyWalkingSprites.Add(NPCSprites[2006]);
        ConnellyWalkingSprites.Add(NPCSprites[2004]);
        ConnellyWalkingSprites.Add(NPCSprites[2005]);
        ConnellyWalkingSprites.Add(NPCSprites[2008]);
        ConnellyWalkingSprites.Add(NPCSprites[2009]);
        AndyWalkingSprites.Add(NPCSprites[1515]);
        AndyWalkingSprites.Add(NPCSprites[1514]);
        AndyWalkingSprites.Add(NPCSprites[1521]);
        AndyWalkingSprites.Add(NPCSprites[1520]);
        AndyWalkingSprites.Add(NPCSprites[1516]);
        AndyWalkingSprites.Add(NPCSprites[1517]);
        AndyWalkingSprites.Add(NPCSprites[1519]);
        AndyWalkingSprites.Add(NPCSprites[1518]);
        BlackElfWalkingSprites.Add(NPCSprites[2265]);
        BlackElfWalkingSprites.Add(NPCSprites[2266]);
        BlackElfWalkingSprites.Add(NPCSprites[2272]);
        BlackElfWalkingSprites.Add(NPCSprites[2271]);
        BlackElfWalkingSprites.Add(NPCSprites[2267]);
        BlackElfWalkingSprites.Add(NPCSprites[2268]);
        BlackElfWalkingSprites.Add(NPCSprites[2270]);
        BlackElfWalkingSprites.Add(NPCSprites[2269]);
        BlondeWalkingSprites.Add(NPCSprites[1845]);
        BlondeWalkingSprites.Add(NPCSprites[1846]);
        BlondeWalkingSprites.Add(NPCSprites[1852]);
        BlondeWalkingSprites.Add(NPCSprites[1851]);
        BlondeWalkingSprites.Add(NPCSprites[1847]);
        BlondeWalkingSprites.Add(NPCSprites[1848]);
        BlondeWalkingSprites.Add(NPCSprites[1849]);
        BlondeWalkingSprites.Add(NPCSprites[1850]);
        BrownElfWalkingSprites.Add(NPCSprites[2297]);
        BrownElfWalkingSprites.Add(NPCSprites[2298]);
        BrownElfWalkingSprites.Add(NPCSprites[2302]);
        BrownElfWalkingSprites.Add(NPCSprites[2301]);
        BrownElfWalkingSprites.Add(NPCSprites[2299]);
        BrownElfWalkingSprites.Add(NPCSprites[2300]);
        BrownElfWalkingSprites.Add(NPCSprites[2303]);
        BrownElfWalkingSprites.Add(NPCSprites[2304]);
        CageWalkingSprites.Add(NPCSprites[1499]);
        CageWalkingSprites.Add(NPCSprites[1498]);
        CageWalkingSprites.Add(NPCSprites[1504]);
        CageWalkingSprites.Add(NPCSprites[1505]);
        CageWalkingSprites.Add(NPCSprites[1500]);
        CageWalkingSprites.Add(NPCSprites[1501]);
        CageWalkingSprites.Add(NPCSprites[1503]);
        CageWalkingSprites.Add(NPCSprites[1504]);
        ChenWalkingSprites.Add(NPCSprites[1821]);
        ChenWalkingSprites.Add(NPCSprites[1822]);
        ChenWalkingSprites.Add(NPCSprites[1826]);
        ChenWalkingSprites.Add(NPCSprites[1825]);
        ChenWalkingSprites.Add(NPCSprites[1823]);
        ChenWalkingSprites.Add(NPCSprites[1824]);
        ChenWalkingSprites.Add(NPCSprites[1828]);
        ChenWalkingSprites.Add(NPCSprites[1827]);
        CraneWalkingSprites.Add(NPCSprites[1869]);
        CraneWalkingSprites.Add(NPCSprites[1870]);
        CraneWalkingSprites.Add(NPCSprites[1876]);
        CraneWalkingSprites.Add(NPCSprites[1875]);
        CraneWalkingSprites.Add(NPCSprites[1871]);
        CraneWalkingSprites.Add(NPCSprites[1872]);
        CraneWalkingSprites.Add(NPCSprites[1873]);
        CraneWalkingSprites.Add(NPCSprites[1874]);
        ElbrahWalkingSprites.Add(NPCSprites[1813]);
        ElbrahWalkingSprites.Add(NPCSprites[1814]);
        ElbrahWalkingSprites.Add(NPCSprites[1820]);
        ElbrahWalkingSprites.Add(NPCSprites[1819]);
        ElbrahWalkingSprites.Add(NPCSprites[1815]);
        ElbrahWalkingSprites.Add(NPCSprites[1816]);
        ElbrahWalkingSprites.Add(NPCSprites[1818]);
        ElbrahWalkingSprites.Add(NPCSprites[1817]);
        GenieWalkingSprites.Add(NPCSprites[2313]);
        GenieWalkingSprites.Add(NPCSprites[2314]);
        GenieWalkingSprites.Add(NPCSprites[2318]);
        GenieWalkingSprites.Add(NPCSprites[2317]);
        GenieWalkingSprites.Add(NPCSprites[2315]);
        GenieWalkingSprites.Add(NPCSprites[2316]);
        GenieWalkingSprites.Add(NPCSprites[2319]);
        GenieWalkingSprites.Add(NPCSprites[2320]);
        HenchmanWalkingSprites.Add(NPCSprites[2122]);
        HenchmanWalkingSprites.Add(NPCSprites[2123]);
        HenchmanWalkingSprites.Add(NPCSprites[2127]);
        HenchmanWalkingSprites.Add(NPCSprites[2126]);
        HenchmanWalkingSprites.Add(NPCSprites[2124]);
        HenchmanWalkingSprites.Add(NPCSprites[2125]);
        HenchmanWalkingSprites.Add(NPCSprites[2128]);
        HenchmanWalkingSprites.Add(NPCSprites[2129]);
        IceElfWalkingSprites.Add(NPCSprites[2257]);
        IceElfWalkingSprites.Add(NPCSprites[2258]);
        IceElfWalkingSprites.Add(NPCSprites[2264]);
        IceElfWalkingSprites.Add(NPCSprites[2263]);
        IceElfWalkingSprites.Add(NPCSprites[2259]);
        IceElfWalkingSprites.Add(NPCSprites[2260]);
        IceElfWalkingSprites.Add(NPCSprites[2261]);
        IceElfWalkingSprites.Add(NPCSprites[2262]);
        LazeeboiWalkingSprites.Add(NPCSprites[1837]);
        LazeeboiWalkingSprites.Add(NPCSprites[1838]);
        LazeeboiWalkingSprites.Add(NPCSprites[1844]);
        LazeeboiWalkingSprites.Add(NPCSprites[1843]);
        LazeeboiWalkingSprites.Add(NPCSprites[1839]);
        LazeeboiWalkingSprites.Add(NPCSprites[1840]);
        LazeeboiWalkingSprites.Add(NPCSprites[1841]);
        LazeeboiWalkingSprites.Add(NPCSprites[1842]);
        MournWalkingSprites.Add(NPCSprites[1877]);
        MournWalkingSprites.Add(NPCSprites[1878]);
        MournWalkingSprites.Add(NPCSprites[1884]);
        MournWalkingSprites.Add(NPCSprites[1883]);
        MournWalkingSprites.Add(NPCSprites[1879]);
        MournWalkingSprites.Add(NPCSprites[1880]);
        MournWalkingSprites.Add(NPCSprites[1881]);
        MournWalkingSprites.Add(NPCSprites[1882]);
        OrangeElfWalkingSprites.Add(NPCSprites[2289]);
        OrangeElfWalkingSprites.Add(NPCSprites[2290]);
        OrangeElfWalkingSprites.Add(NPCSprites[2296]);
        OrangeElfWalkingSprites.Add(NPCSprites[2295]);
        OrangeElfWalkingSprites.Add(NPCSprites[2291]);
        OrangeElfWalkingSprites.Add(NPCSprites[2292]);
        OrangeElfWalkingSprites.Add(NPCSprites[2293]);
        OrangeElfWalkingSprites.Add(NPCSprites[2294]);
        PiersWalkingSprites.Add(NPCSprites[1831]);
        PiersWalkingSprites.Add(NPCSprites[1832]);
        PiersWalkingSprites.Add(NPCSprites[1836]);
        PiersWalkingSprites.Add(NPCSprites[1835]);
        PiersWalkingSprites.Add(NPCSprites[1833]);
        PiersWalkingSprites.Add(NPCSprites[1834]);
        PiersWalkingSprites.Add(NPCSprites[1829]);
        PiersWalkingSprites.Add(NPCSprites[1830]);
        PinkElfWalkingSprites.Add(NPCSprites[2281]);
        PinkElfWalkingSprites.Add(NPCSprites[2282]);
        PinkElfWalkingSprites.Add(NPCSprites[2286]);
        PinkElfWalkingSprites.Add(NPCSprites[2285]);
        PinkElfWalkingSprites.Add(NPCSprites[2283]);
        PinkElfWalkingSprites.Add(NPCSprites[2284]);
        PinkElfWalkingSprites.Add(NPCSprites[2287]);
        PinkElfWalkingSprites.Add(NPCSprites[2288]);
        ProwlerWalkingSprites.Add(NPCSprites[1861]);
        ProwlerWalkingSprites.Add(NPCSprites[1862]);
        ProwlerWalkingSprites.Add(NPCSprites[1868]);
        ProwlerWalkingSprites.Add(NPCSprites[1867]);
        ProwlerWalkingSprites.Add(NPCSprites[1863]);
        ProwlerWalkingSprites.Add(NPCSprites[1864]);
        ProwlerWalkingSprites.Add(NPCSprites[1865]);
        ProwlerWalkingSprites.Add(NPCSprites[1866]);
        SeanWalkingSprites.Add(NPCSprites[1507]);
        SeanWalkingSprites.Add(NPCSprites[1506]);
        SeanWalkingSprites.Add(NPCSprites[1512]);
        SeanWalkingSprites.Add(NPCSprites[1513]);
        SeanWalkingSprites.Add(NPCSprites[1508]);
        SeanWalkingSprites.Add(NPCSprites[1509]);
        SeanWalkingSprites.Add(NPCSprites[1511]);
        SeanWalkingSprites.Add(NPCSprites[1510]);
        SoldierWalkingSprites.Add(NPCSprites[1793]);
        SoldierWalkingSprites.Add(NPCSprites[1780]);
        SoldierWalkingSprites.Add(NPCSprites[1782]);
        SoldierWalkingSprites.Add(NPCSprites[1783]);
        SoldierWalkingSprites.Add(NPCSprites[1794]);
        SoldierWalkingSprites.Add(NPCSprites[1781]);
        SoldierWalkingSprites.Add(NPCSprites[1795]);
        SoldierWalkingSprites.Add(NPCSprites[1787]);
        GuardElfWalkingSprites.Add(NPCSprites[2333]);
        GuardElfWalkingSprites.Add(NPCSprites[2334]);
        GuardElfWalkingSprites.Add(NPCSprites[2338]);
        GuardElfWalkingSprites.Add(NPCSprites[2337]);
        GuardElfWalkingSprites.Add(NPCSprites[2335]);
        GuardElfWalkingSprites.Add(NPCSprites[2336]);
        GuardElfWalkingSprites.Add(NPCSprites[2331]);
        GuardElfWalkingSprites.Add(NPCSprites[2332]);
        WaltonWalkingSprites.Add(NPCSprites[1853]);
        WaltonWalkingSprites.Add(NPCSprites[1854]);
        WaltonWalkingSprites.Add(NPCSprites[1858]);
        WaltonWalkingSprites.Add(NPCSprites[1857]);
        WaltonWalkingSprites.Add(NPCSprites[1855]);
        WaltonWalkingSprites.Add(NPCSprites[1856]);
        WaltonWalkingSprites.Add(NPCSprites[1859]);
        WaltonWalkingSprites.Add(NPCSprites[1860]);
        WhiteElfWalkingSprites.Add(NPCSprites[2305]);
        WhiteElfWalkingSprites.Add(NPCSprites[2306]);
        WhiteElfWalkingSprites.Add(NPCSprites[2309]);
        WhiteElfWalkingSprites.Add(NPCSprites[2310]);
        WhiteElfWalkingSprites.Add(NPCSprites[2307]);
        WhiteElfWalkingSprites.Add(NPCSprites[2308]);
        WhiteElfWalkingSprites.Add(NPCSprites[2311]);
        WhiteElfWalkingSprites.Add(NPCSprites[2312]);
        YellowElfWalkingSprites.Add(NPCSprites[2273]);
        YellowElfWalkingSprites.Add(NPCSprites[2274]);
        YellowElfWalkingSprites.Add(NPCSprites[2280]);
        YellowElfWalkingSprites.Add(NPCSprites[2279]);
        YellowElfWalkingSprites.Add(NPCSprites[2275]);
        YellowElfWalkingSprites.Add(NPCSprites[2276]);
        YellowElfWalkingSprites.Add(NPCSprites[2277]);
        YellowElfWalkingSprites.Add(NPCSprites[2278]);
        POWOutfitWalkingSprites.Add(NPCSprites[677]);
        POWOutfitWalkingSprites.Add(NPCSprites[678]);
        POWOutfitWalkingSprites.Add(NPCSprites[675]);
        POWOutfitWalkingSprites.Add(NPCSprites[676]);
        POWOutfitWalkingSprites.Add(NPCSprites[679]);
        POWOutfitWalkingSprites.Add(NPCSprites[680]);
        POWOutfitWalkingSprites.Add(NPCSprites[673]);
        POWOutfitWalkingSprites.Add(NPCSprites[674]);
        MedicOutfitWalkingSprites.Add(NPCSprites[1291]);
        MedicOutfitWalkingSprites.Add(NPCSprites[1290]);
        MedicOutfitWalkingSprites.Add(NPCSprites[1295]);
        MedicOutfitWalkingSprites.Add(NPCSprites[1296]);
        MedicOutfitWalkingSprites.Add(NPCSprites[1292]);
        MedicOutfitWalkingSprites.Add(NPCSprites[1293]);
        MedicOutfitWalkingSprites.Add(NPCSprites[1294]);
        MedicOutfitWalkingSprites.Add(NPCSprites[1289]);
        SoldierOutfitWalkingSprites.Add(NPCSprites[1522]);
        SoldierOutfitWalkingSprites.Add(NPCSprites[1523]);
        SoldierOutfitWalkingSprites.Add(NPCSprites[1528]);
        SoldierOutfitWalkingSprites.Add(NPCSprites[1529]);
        SoldierOutfitWalkingSprites.Add(NPCSprites[1524]);
        SoldierOutfitWalkingSprites.Add(NPCSprites[1525]);
        SoldierOutfitWalkingSprites.Add(NPCSprites[1526]);
        SoldierOutfitWalkingSprites.Add(NPCSprites[1527]);
        PrisonerOutfitWalkingSprites.Add(NPCSprites[1538]);
        PrisonerOutfitWalkingSprites.Add(NPCSprites[1537]);
        PrisonerOutfitWalkingSprites.Add(NPCSprites[1541]);
        PrisonerOutfitWalkingSprites.Add(NPCSprites[1566]);
        PrisonerOutfitWalkingSprites.Add(NPCSprites[1539]);
        PrisonerOutfitWalkingSprites.Add(NPCSprites[1540]);
        PrisonerOutfitWalkingSprites.Add(NPCSprites[1542]);
        PrisonerOutfitWalkingSprites.Add(NPCSprites[1567]);
        TuxOutfitWalkingSprites.Add(NPCSprites[1804]);
        TuxOutfitWalkingSprites.Add(NPCSprites[1805]);
        TuxOutfitWalkingSprites.Add(NPCSprites[1810]);
        TuxOutfitWalkingSprites.Add(NPCSprites[1811]);
        TuxOutfitWalkingSprites.Add(NPCSprites[1806]);
        TuxOutfitWalkingSprites.Add(NPCSprites[1807]);
        TuxOutfitWalkingSprites.Add(NPCSprites[1808]);
        TuxOutfitWalkingSprites.Add(NPCSprites[1809]);
        HenchmanOutfitWalkingSprites.Add(NPCSprites[1796]);
        HenchmanOutfitWalkingSprites.Add(NPCSprites[1797]);
        HenchmanOutfitWalkingSprites.Add(NPCSprites[1802]);
        HenchmanOutfitWalkingSprites.Add(NPCSprites[1803]);
        HenchmanOutfitWalkingSprites.Add(NPCSprites[1798]);
        HenchmanOutfitWalkingSprites.Add(NPCSprites[1799]);
        HenchmanOutfitWalkingSprites.Add(NPCSprites[1800]);
        HenchmanOutfitWalkingSprites.Add(NPCSprites[1801]);
        ElfOutfitWalkingSprites.Add(NPCSprites[2254]);
        ElfOutfitWalkingSprites.Add(NPCSprites[2224]);
        ElfOutfitWalkingSprites.Add(NPCSprites[2227]);
        ElfOutfitWalkingSprites.Add(NPCSprites[2228]);
        ElfOutfitWalkingSprites.Add(NPCSprites[2255]);
        ElfOutfitWalkingSprites.Add(NPCSprites[2256]);
        ElfOutfitWalkingSprites.Add(NPCSprites[2225]);
        ElfOutfitWalkingSprites.Add(NPCSprites[2226]);
        GuardElfOutfitWalkingSprites.Add(NPCSprites[2339]);
        GuardElfOutfitWalkingSprites.Add(NPCSprites[2340]);
        GuardElfOutfitWalkingSprites.Add(NPCSprites[2343]);
        GuardElfOutfitWalkingSprites.Add(NPCSprites[2344]);
        GuardElfOutfitWalkingSprites.Add(NPCSprites[2341]);
        GuardElfOutfitWalkingSprites.Add(NPCSprites[2342]);
        GuardElfOutfitWalkingSprites.Add(NPCSprites[2329]);
        GuardElfOutfitWalkingSprites.Add(NPCSprites[2330]);

        senderScript.SetNPCLists
        (
            InmateOutiftSprites, GuardOutfitSprites, RabbitSprites, BaldEagleSprites,
            LiferSprites, YoungBuckSprites, OldTimerSprites, BillyGoatSprites, FrosephSprites,
            TangoSprites, MaruSprites, BuddyWalkingSprites, ClintWalkingSprites, ConnellyWalkingSprites,
            AndyWalkingSprites, BlackElfWalkingSprites, BlondeWalkingSprites, BrownElfWalkingSprites,
            CageWalkingSprites, ChenWalkingSprites, CraneWalkingSprites, ElbrahWalkingSprites,
            GenieWalkingSprites, HenchmanWalkingSprites, IceElfWalkingSprites, LazeeboiWalkingSprites,
            MournWalkingSprites, OrangeElfWalkingSprites, PiersWalkingSprites, PinkElfWalkingSprites,
            ProwlerWalkingSprites, SeanWalkingSprites, SoldierWalkingSprites, GuardElfWalkingSprites,
            WaltonWalkingSprites, WhiteElfWalkingSprites, YellowElfWalkingSprites, POWOutfitWalkingSprites,
            MedicOutfitWalkingSprites, SoldierOutfitWalkingSprites, PrisonerOutfitWalkingSprites,
            TuxOutfitWalkingSprites, HenchmanOutfitWalkingSprites, ElfOutfitWalkingSprites,
            GuardElfOutfitWalkingSprites
        );
    }

}