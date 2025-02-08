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
    public List<Sprite> GroundList = new List<Sprite>();
    public List<Sprite> PerksList = new List<Sprite>();
    public List<Sprite> StalagList = new List<Sprite>();
    public List<Sprite> ShanktonList = new List<Sprite>();
    public List<Sprite> JungleList = new List<Sprite>();
    public List<Sprite> SanPanchoList = new List<Sprite>();
    public List<Sprite> HMPList = new List<Sprite>();
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
    public void SetNPCLists(List<Sprite> inmate, List<Sprite> guard, List<Sprite> rabbit,
        List<Sprite> baldEagle, List<Sprite> lifer, List<Sprite> youngBuck, List<Sprite> oldTimer,
        List<Sprite> billyGoat, List<Sprite> froseph, List<Sprite> tango, List<Sprite> maru)
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
    }
    public void SetKnownLists(List<Sprite> ground, List<Sprite> perks, List<Sprite> stalag,
        List<Sprite> shankton, List<Sprite> jungle, List<Sprite> sanPancho, List<Sprite> hmp)
    {
        GroundList = ground;
        PerksList = perks;
        StalagList = stalag;
        ShanktonList = shankton;
        JungleList = jungle;
        SanPanchoList = sanPancho;
        HMPList = hmp;
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
