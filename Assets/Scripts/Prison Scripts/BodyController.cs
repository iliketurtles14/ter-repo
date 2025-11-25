using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.IO;

public class BodyController : MonoBehaviour
{
    private ApplyPrisonData prisonDataScript;
    private ItemBehaviours itemBehavioursScript;
    private Transform player;
    public bool deskIsPickedUp;
    public int currentActionNum;
    public string character;
    public List<List<Sprite>> RabbitLists = new List<List<Sprite>>();
    public List<List<Sprite>> BaldEagleLists = new List<List<Sprite>>();
    public List<List<Sprite>> BillyGoatLists = new List<List<Sprite>>();
    public List<List<Sprite>> FrosephLists = new List<List<Sprite>>();
    public List<List<Sprite>> LiferLists = new List<List<Sprite>>();
    public List<List<Sprite>> MaruLists = new List<List<Sprite>>();
    public List<List<Sprite>> OldTimerLists = new List<List<Sprite>>();
    public List<List<Sprite>> TangoLists = new List<List<Sprite>>();
    public List<List<Sprite>> YoungBuckLists = new List<List<Sprite>>();
    public List<List<Sprite>> BuddyLists = new List<List<Sprite>>();
    public List<List<Sprite>> ClintLists = new List<List<Sprite>>();
    public List<List<Sprite>> ConnellyLists = new List<List<Sprite>>();
    public List<List<Sprite>> AndyLists = new List<List<Sprite>>();
    public List<List<Sprite>> BlackElfLists = new List<List<Sprite>>();
    public List<List<Sprite>> BlondeLists = new List<List<Sprite>>();
    public List<List<Sprite>> BrownElfLists = new List<List<Sprite>>();
    public List<List<Sprite>> CageLists = new List<List<Sprite>>();
    public List<List<Sprite>> ChenLists = new List<List<Sprite>>();
    public List<List<Sprite>> CraneLists = new List<List<Sprite>>();
    public List<List<Sprite>> ElbrahLists = new List<List<Sprite>>();
    public List<List<Sprite>> GenieLists = new List<List<Sprite>>();
    public List<List<Sprite>> HenchmanLists = new List<List<Sprite>>();
    public List<List<Sprite>> IceElfLists = new List<List<Sprite>>();
    public List<List<Sprite>> LazeeboiLists = new List<List<Sprite>>();
    public List<List<Sprite>> MournLists = new List<List<Sprite>>();
    public List<List<Sprite>> OrangeElfLists = new List<List<Sprite>>();
    public List<List<Sprite>> PiersLists = new List<List<Sprite>>();
    public List<List<Sprite>> PinkElfLists = new List<List<Sprite>>();
    public List<List<Sprite>> ProwlerLists = new List<List<Sprite>>();
    public List<List<Sprite>> SeanLists = new List<List<Sprite>>();
    public List<List<Sprite>> SoldierLists = new List<List<Sprite>>();
    public List<List<Sprite>> GuardElfLists = new List<List<Sprite>>();
    public List<List<Sprite>> WaltonLists = new List<List<Sprite>>();
    public List<List<Sprite>> WhiteElfLists = new List<List<Sprite>>();
    public List<List<Sprite>> YellowElfLists = new List<List<Sprite>>();
    public Dictionary<string, List<List<Sprite>>> characterDict = new Dictionary<string, List<List<Sprite>>>();

    public void Start()
    {
        Transform so = RootObjectCache.GetRoot("ScriptObject").transform;
        prisonDataScript = so.GetComponent<ApplyPrisonData>();
        itemBehavioursScript = so.GetComponent<ItemBehaviours>();
        player = RootObjectCache.GetRoot("Player").transform;

        RabbitLists.Add(prisonDataScript.RabbitSleepDeadSprites);
        RabbitLists.Add(prisonDataScript.RabbitDiggingSprites);
        RabbitLists.Add(DataSender.instance.RabbitSprites);
        RabbitLists.Add(prisonDataScript.RabbitPunchingSprites);
        RabbitLists.Add(prisonDataScript.RabbitCuttingSprites);
        RabbitLists.Add(prisonDataScript.RabbitRakingSprites);
        RabbitLists.Add(prisonDataScript.RabbitBroomingSprites);
        RabbitLists.Add(prisonDataScript.RabbitPushUpSprites);
        RabbitLists.Add(prisonDataScript.RabbitBenchingSprites);
        RabbitLists.Add(prisonDataScript.RabbitJumpRopeSprites);
        RabbitLists.Add(prisonDataScript.RabbitPullUpSprites);
        RabbitLists.Add(prisonDataScript.RabbitChippingSprites);
        RabbitLists.Add(prisonDataScript.RabbitBoundSprites);
        RabbitLists.Add(prisonDataScript.RabbitTraySprites);
        RabbitLists.Add(prisonDataScript.RabbitZippingSprites);
        RabbitLists.Add(prisonDataScript.RabbitHoldingSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleSleepDeadSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleDiggingSprites);
        BaldEagleLists.Add(DataSender.instance.BaldEagleSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEaglePunchingSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleCuttingSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleRakingSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleBroomingSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEaglePushUpSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleBenchingSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleJumpRopeSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEaglePullUpSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleChippingSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleBoundSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleTraySprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleZippingSprites);
        BaldEagleLists.Add(prisonDataScript.BaldEagleHoldingSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatSleepDeadSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatDiggingSprites);
        BillyGoatLists.Add(DataSender.instance.BillyGoatSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatPunchingSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatCuttingSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatRakingSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatBroomingSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatPushUpSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatBenchingSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatJumpRopeSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatPullUpSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatChippingSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatBoundSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatTraySprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatZippingSprites);
        BillyGoatLists.Add(prisonDataScript.BillyGoatHoldingSprites);
        FrosephLists.Add(prisonDataScript.FrosephSleepDeadSprites);
        FrosephLists.Add(prisonDataScript.FrosephDiggingSprites);
        FrosephLists.Add(DataSender.instance.FrosephSprites);
        FrosephLists.Add(prisonDataScript.FrosephPunchingSprites);
        FrosephLists.Add(prisonDataScript.FrosephCuttingSprites);
        FrosephLists.Add(prisonDataScript.FrosephRakingSprites);
        FrosephLists.Add(prisonDataScript.FrosephBroomingSprites);
        FrosephLists.Add(prisonDataScript.FrosephPushUpSprites);
        FrosephLists.Add(prisonDataScript.FrosephBenchingSprites);
        FrosephLists.Add(prisonDataScript.FrosephJumpRopeSprites);
        FrosephLists.Add(prisonDataScript.FrosephPullUpSprites);
        FrosephLists.Add(prisonDataScript.FrosephChippingSprites);
        FrosephLists.Add(prisonDataScript.FrosephBoundSprites);
        FrosephLists.Add(prisonDataScript.FrosephTraySprites);
        FrosephLists.Add(prisonDataScript.FrosephZippingSprites);
        FrosephLists.Add(prisonDataScript.FrosephHoldingSprites);
        LiferLists.Add(prisonDataScript.LiferSleepDeadSprites);
        LiferLists.Add(prisonDataScript.LiferDiggingSprites);
        LiferLists.Add(DataSender.instance.LiferSprites);
        LiferLists.Add(prisonDataScript.LiferPunchingSprites);
        LiferLists.Add(prisonDataScript.LiferCuttingSprites);
        LiferLists.Add(prisonDataScript.LiferRakingSprites);
        LiferLists.Add(prisonDataScript.LiferBroomingSprites);
        LiferLists.Add(prisonDataScript.LiferPushUpSprites);
        LiferLists.Add(prisonDataScript.LiferBenchingSprites);
        LiferLists.Add(prisonDataScript.LiferJumpRopeSprites);
        LiferLists.Add(prisonDataScript.LiferPullUpSprites);
        LiferLists.Add(prisonDataScript.LiferChippingSprites);
        LiferLists.Add(prisonDataScript.LiferBoundSprites);
        LiferLists.Add(prisonDataScript.LiferTraySprites);
        LiferLists.Add(prisonDataScript.LiferZippingSprites);
        LiferLists.Add(prisonDataScript.LiferHoldingSprites);
        MaruLists.Add(prisonDataScript.MaruSleepDeadSprites);
        MaruLists.Add(prisonDataScript.MaruDiggingSprites);
        MaruLists.Add(DataSender.instance.MaruSprites);
        MaruLists.Add(prisonDataScript.MaruPunchingSprites);
        MaruLists.Add(prisonDataScript.MaruCuttingSprites);
        MaruLists.Add(prisonDataScript.MaruRakingSprites);
        MaruLists.Add(prisonDataScript.MaruBroomingSprites);
        MaruLists.Add(prisonDataScript.MaruPushUpSprites);
        MaruLists.Add(prisonDataScript.MaruBenchingSprites);
        MaruLists.Add(prisonDataScript.MaruJumpRopeSprites);
        MaruLists.Add(prisonDataScript.MaruPullUpSprites);
        MaruLists.Add(prisonDataScript.MaruChippingSprites);
        MaruLists.Add(prisonDataScript.MaruBoundSprites);
        MaruLists.Add(prisonDataScript.MaruTraySprites);
        MaruLists.Add(prisonDataScript.MaruZippingSprites);
        MaruLists.Add(prisonDataScript.MaruHoldingSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerSleepDeadSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerDiggingSprites);
        OldTimerLists.Add(DataSender.instance.OldTimerSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerPunchingSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerCuttingSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerRakingSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerBroomingSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerPushUpSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerBenchingSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerJumpRopeSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerPullUpSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerChippingSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerBoundSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerTraySprites);
        OldTimerLists.Add(prisonDataScript.OldTimerZippingSprites);
        OldTimerLists.Add(prisonDataScript.OldTimerHoldingSprites);
        TangoLists.Add(prisonDataScript.TangoSleepDeadSprites);
        TangoLists.Add(prisonDataScript.TangoDiggingSprites);
        TangoLists.Add(DataSender.instance.TangoSprites);
        TangoLists.Add(prisonDataScript.TangoPunchingSprites);
        TangoLists.Add(prisonDataScript.TangoCuttingSprites);
        TangoLists.Add(prisonDataScript.TangoRakingSprites);
        TangoLists.Add(prisonDataScript.TangoBroomingSprites);
        TangoLists.Add(prisonDataScript.TangoPushUpSprites);
        TangoLists.Add(prisonDataScript.TangoBenchingSprites);
        TangoLists.Add(prisonDataScript.TangoJumpRopeSprites);
        TangoLists.Add(prisonDataScript.TangoPullUpSprites);
        TangoLists.Add(prisonDataScript.TangoChippingSprites);
        TangoLists.Add(prisonDataScript.TangoBoundSprites);
        TangoLists.Add(prisonDataScript.TangoTraySprites);
        TangoLists.Add(prisonDataScript.TangoZippingSprites);
        TangoLists.Add(prisonDataScript.TangoHoldingSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckSleepDeadSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckDiggingSprites);
        YoungBuckLists.Add(DataSender.instance.YoungBuckSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckPunchingSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckCuttingSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckRakingSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckBroomingSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckPushUpSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckBenchingSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckJumpRopeSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckPullUpSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckChippingSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckBoundSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckTraySprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckZippingSprites);
        YoungBuckLists.Add(prisonDataScript.YoungBuckHoldingSprites);
        BuddyLists.Add(prisonDataScript.BuddySleepDeadSprites);
        BuddyLists.Add(prisonDataScript.BuddyDiggingSprites);
        BuddyLists.Add(DataSender.instance.BuddyWalkingSprites);
        BuddyLists.Add(prisonDataScript.BuddyPunchingSprites);
        BuddyLists.Add(prisonDataScript.BuddyCuttingSprites);
        BuddyLists.Add(prisonDataScript.BuddyRakingSprites);
        BuddyLists.Add(prisonDataScript.BuddyBroomingSprites);
        BuddyLists.Add(prisonDataScript.BuddyPushUpSprites);
        BuddyLists.Add(prisonDataScript.BuddyBenchingSprites);
        BuddyLists.Add(prisonDataScript.BuddyJumpRopeSprites);
        BuddyLists.Add(prisonDataScript.BuddyChippingSprites);
        BuddyLists.Add(prisonDataScript.BuddyBoundSprites);
        BuddyLists.Add(prisonDataScript.BuddyTraySprites);
        BuddyLists.Add(prisonDataScript.BuddyZippingSprites);
        BuddyLists.Add(prisonDataScript.BuddyHoldingSprites);
        ClintLists.Add(prisonDataScript.ClintSleepDeadSprites);
        ClintLists.Add(prisonDataScript.ClintDiggingSprites);
        ClintLists.Add(DataSender.instance.ClintWalkingSprites);
        ClintLists.Add(prisonDataScript.ClintPunchingSprites);
        ClintLists.Add(prisonDataScript.ClintCuttingSprites);
        ClintLists.Add(prisonDataScript.ClintRakingSprites);
        ClintLists.Add(prisonDataScript.ClintBroomingSprites);
        ClintLists.Add(prisonDataScript.ClintPushUpSprites);
        ClintLists.Add(prisonDataScript.ClintBenchingSprites);
        ClintLists.Add(prisonDataScript.ClintJumpRopeSprites);
        ClintLists.Add(prisonDataScript.ClintPullUpSprites);
        ClintLists.Add(prisonDataScript.ClintChippingSprites);
        ClintLists.Add(prisonDataScript.ClintBoundSprites);
        ClintLists.Add(prisonDataScript.ClintTraySprites);
        ClintLists.Add(prisonDataScript.ClintZippingSprites);
        ClintLists.Add(prisonDataScript.ClintHoldingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellySleepDeadSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyDiggingSprites);
        ConnellyLists.Add(DataSender.instance.ConnellyWalkingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyPunchingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyCuttingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyRakingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyBroomingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyPushUpSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyBenchingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyJumpRopeSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyPullUpSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyChippingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyBoundSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyTraySprites);
        ConnellyLists.Add(prisonDataScript.ConnellyZippingSprites);
        ConnellyLists.Add(prisonDataScript.ConnellyHoldingSprites);
        AndyLists.Add(prisonDataScript.AndySleepDeadSprites);
        AndyLists.Add(DataSender.instance.AndyWalkingSprites);
        AndyLists.Add(prisonDataScript.AndyPunchingSprites);
        AndyLists.Add(prisonDataScript.AndyBenchingSprites);
        AndyLists.Add(prisonDataScript.AndyBoundSprites);
        BlackElfLists.Add(prisonDataScript.BlackElfSleepDeadSprites);
        BlackElfLists.Add(DataSender.instance.BlackElfWalkingSprites);
        BlackElfLists.Add(prisonDataScript.BlackElfPunchingSprites);
        BlackElfLists.Add(prisonDataScript.BlackElfBenchingSprites);
        BlackElfLists.Add(prisonDataScript.BlackElfBoundSprites);
        BlackElfLists.Add(prisonDataScript.BlackElfTraySprites);
        BlondeLists.Add(prisonDataScript.BlondeSleepDeadSprites);
        BlondeLists.Add(DataSender.instance.BlondeWalkingSprites);
        BlondeLists.Add(prisonDataScript.BlondePunchingSprites);
        BlondeLists.Add(prisonDataScript.BlondeBoundSprites);
        BlondeLists.Add(prisonDataScript.BlondeTraySprites);
        BrownElfLists.Add(prisonDataScript.BrownElfSleepDeadSprites);
        BrownElfLists.Add(DataSender.instance.BrownElfWalkingSprites);
        BrownElfLists.Add(prisonDataScript.BrownElfPunchingSprites);
        BrownElfLists.Add(prisonDataScript.BrownElfBenchingSprites);
        BrownElfLists.Add(prisonDataScript.BrownElfBoundSprites);
        BrownElfLists.Add(prisonDataScript.BrownElfTraySprites);
        CageLists.Add(prisonDataScript.CageSleepDeadSprites);
        CageLists.Add(DataSender.instance.CageWalkingSprites);
        CageLists.Add(prisonDataScript.CagePunchingSprites);
        CageLists.Add(prisonDataScript.CageBenchingSprites);
        CageLists.Add(prisonDataScript.CageBoundSprites);
        CageLists.Add(prisonDataScript.CageTraySprites);
        ChenLists.Add(prisonDataScript.ChenSleepDeadSprites);
        ChenLists.Add(DataSender.instance.ChenWalkingSprites);
        ChenLists.Add(prisonDataScript.ChenPunchingSprites);
        ChenLists.Add(prisonDataScript.ChenBenchingSprites);
        ChenLists.Add(prisonDataScript.ChenTraySprites);
        CraneLists.Add(prisonDataScript.CraneSleepDeadSprites);
        CraneLists.Add(DataSender.instance.CraneWalkingSprites);
        CraneLists.Add(prisonDataScript.CranePunchingSprites);
        CraneLists.Add(prisonDataScript.CraneBoundSprites);
        CraneLists.Add(prisonDataScript.CraneTraySprites);
        ElbrahLists.Add(prisonDataScript.ElbrahSleepDeadSprites);
        ElbrahLists.Add(DataSender.instance.ElbrahWalkingSprites);
        ElbrahLists.Add(prisonDataScript.ElbrahPunchingSprites);
        ElbrahLists.Add(prisonDataScript.ElbrahBoundSprites);
        ElbrahLists.Add(prisonDataScript.ElbrahTraySprites);
        GenieLists.Add(prisonDataScript.GenieSleepDeadSprites);
        GenieLists.Add(DataSender.instance.GenieWalkingSprites);
        GenieLists.Add(prisonDataScript.GeniePunchingSprites);
        GenieLists.Add(prisonDataScript.GenieBenchingSprites);
        GenieLists.Add(prisonDataScript.GenieBoundSprites);
        GenieLists.Add(prisonDataScript.GenieTraySprites);
        HenchmanLists.Add(prisonDataScript.HenchmanSleepDeadSprites);
        HenchmanLists.Add(DataSender.instance.HenchmanWalkingSprites);
        HenchmanLists.Add(prisonDataScript.HenchmanPunchingSprites);
        IceElfLists.Add(prisonDataScript.IceElfSleepDeadSprites);
        IceElfLists.Add(DataSender.instance.IceElfWalkingSprites);
        IceElfLists.Add(prisonDataScript.IceElfPunchingSprites);
        IceElfLists.Add(prisonDataScript.IceElfBenchingSprites);
        IceElfLists.Add(prisonDataScript.IceElfBoundSprites);
        IceElfLists.Add(prisonDataScript.IceElfTraySprites);
        LazeeboiLists.Add(prisonDataScript.LazeeboiSleepDeadSprites);
        LazeeboiLists.Add(DataSender.instance.LazeeboiWalkingSprites);
        LazeeboiLists.Add(prisonDataScript.LazeeboiPunchingSprites);
        LazeeboiLists.Add(prisonDataScript.LazeeboiBenchingSprites);
        LazeeboiLists.Add(prisonDataScript.LazeeboiBoundSprites);
        LazeeboiLists.Add(prisonDataScript.LazeeboiTraySprites);
        MournLists.Add(prisonDataScript.MournSleepDeadSprites);
        MournLists.Add(DataSender.instance.MournWalkingSprites);
        MournLists.Add(prisonDataScript.MournPunchingSprites);
        MournLists.Add(prisonDataScript.MournBoundSprites);
        MournLists.Add(prisonDataScript.MournTraySprites);
        OrangeElfLists.Add(prisonDataScript.OrangeElfSleepDeadSprites);
        OrangeElfLists.Add(DataSender.instance.OrangeElfWalkingSprites);
        OrangeElfLists.Add(prisonDataScript.OrangeElfPunchingSprites);
        OrangeElfLists.Add(prisonDataScript.OrangeElfBenchingSprites);
        OrangeElfLists.Add(prisonDataScript.OrangeElfBoundSprites);
        OrangeElfLists.Add(prisonDataScript.OrangeElfTraySprites);
        PiersLists.Add(prisonDataScript.PiersSleepDeadSprites);
        PiersLists.Add(DataSender.instance.PiersWalkingSprites);
        PiersLists.Add(prisonDataScript.PiersPunchingSprites);
        PiersLists.Add(prisonDataScript.PiersBenchingSprites);
        PiersLists.Add(prisonDataScript.PiersBoundSprites);
        PiersLists.Add(prisonDataScript.PiersTraySprites);
        PinkElfLists.Add(prisonDataScript.PinkElfSleepDeadSprites);
        PinkElfLists.Add(DataSender.instance.PinkElfWalkingSprites);
        PinkElfLists.Add(prisonDataScript.PinkElfPunchingSprites);
        PinkElfLists.Add(prisonDataScript.PinkElfBenchingSprites);
        PinkElfLists.Add(prisonDataScript.PinkElfBoundSprites);
        PinkElfLists.Add(prisonDataScript.PinkElfTraySprites);
        ProwlerLists.Add(prisonDataScript.ProwlerSleepDeadSprites);
        ProwlerLists.Add(DataSender.instance.ProwlerWalkingSprites);
        ProwlerLists.Add(prisonDataScript.ProwlerPunchingSprites);
        ProwlerLists.Add(prisonDataScript.ProwlerBoundSprites);
        ProwlerLists.Add(prisonDataScript.ProwlerTraySprites);
        SeanLists.Add(prisonDataScript.SeanSleepDeadSprites);
        SeanLists.Add(DataSender.instance.SeanWalkingSprites);
        SeanLists.Add(prisonDataScript.SeanPunchingSprites);
        SeanLists.Add(prisonDataScript.SeanBoundSprites);
        SeanLists.Add(prisonDataScript.SeanTraySprites);
        SoldierLists.Add(prisonDataScript.SoldierSleepDeadSprites);
        SoldierLists.Add(DataSender.instance.SoldierWalkingSprites);
        SoldierLists.Add(prisonDataScript.SoldierPunchingSprites);
        SoldierLists.Add(prisonDataScript.SoldierBoundSprites);
        GuardElfLists.Add(prisonDataScript.GuardElfSleepDeadSprites);
        GuardElfLists.Add(DataSender.instance.GuardElfWalkingSprites);
        GuardElfLists.Add(prisonDataScript.GuardElfPunchingSprites);
        GuardElfLists.Add(prisonDataScript.GuardElfBoundSprites);
        WaltonLists.Add(prisonDataScript.WaltonSleepDeadSprites);
        WaltonLists.Add(DataSender.instance.WaltonWalkingSprites);
        WaltonLists.Add(prisonDataScript.WaltonPunchingSprites);
        WaltonLists.Add(prisonDataScript.WaltonBenchingSprites);
        WaltonLists.Add(prisonDataScript.WaltonBoundSprites);
        WaltonLists.Add(prisonDataScript.WaltonTraySprites);
        WhiteElfLists.Add(prisonDataScript.WhiteElfSleepDeadSprites);
        WhiteElfLists.Add(DataSender.instance.WhiteElfWalkingSprites);
        WhiteElfLists.Add(prisonDataScript.WhiteElfPunchingSprites);
        WhiteElfLists.Add(prisonDataScript.WhiteElfBenchingSprites);
        WhiteElfLists.Add(prisonDataScript.WhiteElfBoundSprites);
        WhiteElfLists.Add(prisonDataScript.WhiteElfTraySprites);
        YellowElfLists.Add(prisonDataScript.YellowElfSleepDeadSprites);
        YellowElfLists.Add(DataSender.instance.YellowElfWalkingSprites);
        YellowElfLists.Add(prisonDataScript.YellowElfPunchingSprites);
        YellowElfLists.Add(prisonDataScript.YellowElfBenchingSprites);
        YellowElfLists.Add(prisonDataScript.YellowElfBoundSprites);
        YellowElfLists.Add(prisonDataScript.YellowElfTraySprites);


        characterDict = new Dictionary<string, List<List<Sprite>>>
        {
            { "Rabbit", RabbitLists },
            { "BaldEagle", BaldEagleLists },
            { "BillyGoat", BillyGoatLists },
            { "Froseph", FrosephLists },
            { "Lifer", LiferLists },
            { "Maru", MaruLists },
            { "OldTimer", OldTimerLists },
            { "Tango", TangoLists },
            { "YoungBuck", YoungBuckLists },
            { "Buddy", BuddyLists },
            { "Clint", ClintLists },
            { "Connelly", ConnellyLists },
            { "Andy", AndyLists },
            { "BlackElf", BlackElfLists },
            { "Blonde", BlondeLists },
            { "BrownElf", BrownElfLists },
            { "Cage", CageLists },
            { "Chen", ChenLists },
            { "Crane", CraneLists },
            { "Elbrah", ElbrahLists },
            { "Genie", GenieLists },
            { "Henchman", HenchmanLists },
            { "IceElf", IceElfLists },
            { "Lazeeboi", LazeeboiLists },
            { "Mourn", MournLists },
            { "OrangeElf", OrangeElfLists },
            { "Piers", PiersLists },
            { "PinkElf", PinkElfLists },
            { "Prowler", ProwlerLists },
            { "Sean", SeanLists },
            { "Soldier", SoldierLists },
            { "GuardElf", GuardElfLists },
            { "Walton", WaltonLists },
            { "WhiteElf", WhiteElfLists },
            { "YellowElf", YellowElfLists }
        };
    }
    public void Update()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        if (itemBehavioursScript.isChipping)//remember to chagne this stuff for the outfitcontroller too
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
        else if ((player.GetComponent<PlayerCollectionData>().playerData.hasFood && name == "Player") ||
            ((name.StartsWith("Guard") || name.StartsWith("Inmate")) && GetComponent<NPCCollectionData>().npcData.hasFood))
        {
            currentActionNum = 13;
        }
        else
        {
            currentActionNum = 2;
        }

        if(name == "Player")
        {
            character = CharacterEnumClass.GetCharacterString(NPCSave.instance.playerCharacter);
        }
        else
        {
            character = CharacterEnumClass.GetCharacterString(GetComponent<NPCCollectionData>().npcData.charNum);
        }

        try
        {
            if(name == "Player")
            {
                GetComponent<PlayerAnimation>().bodyDirSprites = characterDict[character][currentActionNum];
            }
            else
            {
                GetComponent<NPCAnimation>().bodyDirSprites = characterDict[character][currentActionNum];
            }
        }
        catch { }
        sr.size = new Vector2((sr.sprite.rect.width / sr.sprite.pixelsPerUnit) * 10, (sr.sprite.rect.height / sr.sprite.pixelsPerUnit) * 10);

        
    }
    public static void SaveSpriteAsPNG(Sprite sprite, string filePath)
    {
        // Convert Sprite to Texture2D
        Texture2D texture = new Texture2D(
            (int)sprite.rect.width,
            (int)sprite.rect.height,
            TextureFormat.RGBA32,
            false
        );

        // Copy the pixels from the sprite's texture
        Color[] pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height
        );
        texture.SetPixels(pixels);
        texture.Apply();

        // Encode texture to PNG
        byte[] pngData = texture.EncodeToPNG();

        // Save to file
        File.WriteAllBytes(filePath, pngData);

        // Clean up
        Object.Destroy(texture);
    }
}
