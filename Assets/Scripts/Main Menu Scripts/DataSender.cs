using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DataSender : MonoBehaviour
{
    public List<Sprite> ItemImages = new List<Sprite>();
    public List<Sprite> NPCImages = new List<Sprite>();
    public List<Sprite> PrisonObjectImages = new List<Sprite>();
    public List<Sprite> UIImages = new List<Sprite>();
    public List<Sprite> InmateOutfitSprites = new List<Sprite>();
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
    public List<Sprite> POWOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> MedicOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> SoldierOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> PrisonerOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> TuxOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> HenchmanOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> ElfOutfitWalkingSprites = new List<Sprite>();
    public List<Sprite> GuardElfOutfitWalkingSprites = new List<Sprite>();
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
    public List<Sprite> TileList = new List<Sprite>();
    public Sprite GroundSprite;
    public Sprite UndergroundSprite;
    public List<AudioClip> SoundList = new List<AudioClip>();
    public List<AudioClip> MusicList = new List<AudioClip>();
    public static DataSender instance { get; private set; }
    
    public void SetFullLists(List<Sprite> item, List<Sprite> npc, List<Sprite> prisonObject, List<Sprite> ui)
    {
        ItemImages = item;
        NPCImages = npc;
        PrisonObjectImages = prisonObject;
        UIImages = ui;
    }
    public void SetNPCLists(
        List<Sprite> inmate, List<Sprite> guard, List<Sprite> rabbit, List<Sprite> baldEagle,
        List<Sprite> lifer, List<Sprite> youngBuck, List<Sprite> oldTimer, List<Sprite> billyGoat,
        List<Sprite> froseph, List<Sprite> tango, List<Sprite> maru, List<Sprite> buddy,
        List<Sprite> clint, List<Sprite> connelly, List<Sprite> andy, List<Sprite> blackElf,
        List<Sprite> blonde, List<Sprite> brownElf, List<Sprite> cage, List<Sprite> chen,
        List<Sprite> crane, List<Sprite> elbrah, List<Sprite> genie, List<Sprite> henchman,
        List<Sprite> iceElf, List<Sprite> lazeeboi, List<Sprite> mourn, List<Sprite> orangeElf,
        List<Sprite> piers, List<Sprite> pinkElf, List<Sprite> prowler, List<Sprite> sean,
        List<Sprite> soldier, List<Sprite> guardElf, List<Sprite> walton, List<Sprite> whiteElf,
        List<Sprite> yellowElf, List<Sprite> powOutfit, List<Sprite> medicOutfit,
        List<Sprite> soldierOutfit, List<Sprite> prisonerOutfit, List<Sprite> tuxOutfit,
        List<Sprite> henchmanOutfit, List<Sprite> elfOutfit, List<Sprite> guardElfOutfit
    )
    {
        InmateOutfitSprites = inmate;
        GuardOutfitSprites = guard;
        RabbitSprites = rabbit;
        BaldEagleSprites = baldEagle;
        LiferSprites = lifer;
        YoungBuckSprites = youngBuck;
        OldTimerSprites = oldTimer;
        BillyGoatSprites = billyGoat;
        FrosephSprites = froseph;
        TangoSprites = tango;
        MaruSprites = maru;
        BuddyWalkingSprites = buddy;
        ClintWalkingSprites = clint;
        ConnellyWalkingSprites = connelly;
        AndyWalkingSprites = andy;
        BlackElfWalkingSprites = blackElf;
        BlondeWalkingSprites = blonde;
        BrownElfWalkingSprites = brownElf;
        CageWalkingSprites = cage;
        ChenWalkingSprites = chen;
        CraneWalkingSprites = crane;
        ElbrahWalkingSprites = elbrah;
        GenieWalkingSprites = genie;
        HenchmanWalkingSprites = henchman;
        IceElfWalkingSprites = iceElf;
        LazeeboiWalkingSprites = lazeeboi;
        MournWalkingSprites = mourn;
        OrangeElfWalkingSprites = orangeElf;
        PiersWalkingSprites = piers;
        PinkElfWalkingSprites = pinkElf;
        ProwlerWalkingSprites = prowler;
        SeanWalkingSprites = sean;
        SoldierWalkingSprites = soldier;
        GuardElfWalkingSprites = guardElf;
        WaltonWalkingSprites = walton;
        WhiteElfWalkingSprites = whiteElf;
        YellowElfWalkingSprites = yellowElf;
        POWOutfitWalkingSprites = powOutfit;
        MedicOutfitWalkingSprites = medicOutfit;
        SoldierOutfitWalkingSprites = soldierOutfit;
        PrisonerOutfitWalkingSprites = prisonerOutfit;
        TuxOutfitWalkingSprites = tuxOutfit;
        HenchmanOutfitWalkingSprites = henchmanOutfit;
        ElfOutfitWalkingSprites = elfOutfit;
        GuardElfOutfitWalkingSprites = guardElfOutfit;
    }
    public void SetKnownLists(List<Sprite> tiles, Sprite ground, Sprite underground)
    {
        TileList = tiles;
        GroundSprite = ground;
        UndergroundSprite = underground;
    }
    public void SetSoundList(List<AudioClip> sound)
    {
        SoundList = sound;
    }
    public void SetMusicList(List<AudioClip> music)
    {
        MusicList = music;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
