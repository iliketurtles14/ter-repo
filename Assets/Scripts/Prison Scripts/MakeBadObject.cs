using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class MakeBadObject : MonoBehaviour
{
    private GameObject player;
    private DeskStand deskStandScript;
    private bool playerIsBad;
    private Transform badObjects;
    private Transform tiles;
    private Schedule scheduleScript;
    private Routine routineScript;
    private Transform mc;
    private ItemBehaviours itemBehavioursScript;
    private Zones zonesScript;
    private bool ready;
    private bool hasSniper;
    private List<GameObject> sniperList = new List<GameObject>();
    private bool isShooting;
    private Sprite bulletSprite;
    private ApplyPrisonData applyScript;
    private bool isGivingOutfitHeat;
    public bool hasBadOutfit;
    public bool hasInmateOutfit;
    public bool isOutside;
    private Death deathScript;
    private int playerLayer;
    private int groundLayer;
    private FightEffects fightFX;
    private Map currentMap;
    private bool tookInmateItem;
    private PauseController pc;
    private Lockdown lockdownScript;
    private UnlockDoors unlockDoorsScript;
    
    private List<int> badOutfitIDs = new List<int>()
    {
        39, 44, 49, 54, 103
    };
    private List<int> inmateOutfitIDs = new List<int>()
    {
        29, 30, 31, 32, 33, 34, 35, 36, 40, 41, 42, 43, 45, 46, 47, 48, 50, 51, 52, 53, 54
    };

    //these bools are to make sure the same badObject doesnt get made more than once at a given time
    //these basically just say what types of badObjects are currently active
    private bool onDesk;
    private bool searchingDesk;
    private bool pickedUp;
    private bool inWrongCell;
    private bool playerPunching;
    private bool wrongRoutine;
    private bool notAtWork;
    private bool noOutfit;
    private bool nonInmateOutfit;
    private bool highHeat;
    private bool breakingTile;
    private bool outsideOnInsideMap;
    private bool npcLoot;
    private bool missedRollcall;
    private bool outLate;
    private void Start()
    {
        ready = false;
        Transform scriptObject = RootObjectCache.GetRoot("ScriptObject").transform;

        player = RootObjectCache.GetRoot("Player");
        deskStandScript = scriptObject.GetComponent<DeskStand>();
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        itemBehavioursScript = GetComponent<ItemBehaviours>();
        zonesScript = scriptObject.GetComponent<Zones>();
        applyScript = GetComponent<ApplyPrisonData>();
        deathScript = scriptObject.GetComponent<Death>();
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        fightFX = scriptObject.GetComponent<FightEffects>();
        lockdownScript = scriptObject.GetComponent<Lockdown>();
        unlockDoorsScript = scriptObject.GetComponent<UnlockDoors>();
        pc = GetComponent<PauseController>();

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
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "Sniper")
            {
                hasSniper = true;
                sniperList.Add(obj.gameObject);
            }
        }

        bulletSprite = applyScript.UISprites[151];

        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        // this is only for certain bad actions. the reason some bad actions are missing is because
        // some bad actions can have multiple instances of it occuring (because of how this script is
        // set up, that cant happen within this script).
        // MISSING: inmate punch, toilet clog, tied up npc, sheets on bars,
        //          broken tiles, being in a perimeter/unsafe zone, being on the roof without a
        //          guard/bleached outfit, missing rollcall, being outside with a non-inmate outfit,

        ///bool management
        //bad/inmate outfit
        try
        {
            if (badOutfitIDs.Contains(mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id))
            {
                hasBadOutfit = true;
            }
        }
        catch
        {
            hasBadOutfit = false;
        }
        try
        {
            if (inmateOutfitIDs.Contains(mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id))
            {
                hasInmateOutfit = true;
            }
        }
        catch
        {
            hasInmateOutfit = false;
        }

        //outside
        if (!Physics2D.GetIgnoreLayerCollision(playerLayer, groundLayer) &&
            player.GetComponent<PlayerFloorCollision>().playerFloor.GetComponent<TileCollectionData>().tileData.tileType == "outFloor")
        {
            isOutside = true;
        }
        else
        {
            isOutside = false;
        }

        ///bad object dependent
        //standing on desk
        if (deskStandScript.hasClimbed && !onDesk) //desk stand or standing on anything bad
        {
            onDesk = true;

            BadObjectData data = new BadObjectData
            {
                isMultiplied = true,
                heatGain = 5,
                messageType = "OnDesk",
                attachedObject = player
            };
            CreateBadObject(data, "onDesk");
        }
        else if(!deskStandScript.hasClimbed && onDesk)
        {
            onDesk = false;

            DestroyBadObject("onDesk");
        }

        //searching a desk
        if (!searchingDesk && mc.Find("DeskMenuPanel").GetComponent<DeskInv>().isOpening)
        {
            searchingDesk = true;

            BadObjectData guardData = new BadObjectData //for guard
            {
                isMultiplied = true,
                heatGain = 15,
                shouldAggro = true,
                attachedObject = player
            };

            BadObjectData inmateData = new BadObjectData //for inmate
            {
                shouldAggro = true,
                forInmate = true,
                attachedObject = player,
                messageType = "Chair_Push"
            };

            CreateBadObject(guardData, "guardSearchingDesk");
            CreateBadObject(inmateData, "inmateSearchingDesk");
        }
        else if (searchingDesk && !mc.Find("DeskMenuPanel").GetComponent<DeskInv>().isOpening)
        {
            searchingDesk = false;

            DestroyBadObject("guardSearchingDesk");
            DestroyBadObject("inmateSearchingDesk");
        }

        //picking up a desk/inmate
        if (!pickedUp)
        {
            foreach (Transform desk in tiles.Find("GroundObjects"))
            {
                if (desk.CompareTag("Desk"))
                {
                    if (desk.GetComponent<DeskPickUp>() && desk.GetComponent<DeskPickUp>().isPickedUp)
                    {
                        pickedUp = true;

                        BadObjectData data = new BadObjectData
                        {
                            isMultiplied = true,
                            heatGain = 10,
                            messageType = "DropIt",
                            attachedObject = player
                        };

                        CreateBadObject(data, "pickedUp");
                    }
                }
            }
        }
        else if (pickedUp)
        {
            bool shouldDestroy = false;
            foreach(Transform desk in tiles.Find("GroundObjects"))
            {
                if (desk.CompareTag("Desk"))
                {
                    try
                    {
                        if (desk.GetComponent<DeskPickUp>() && desk.GetComponent<DeskPickUp>().isPickedUp)
                        {
                            shouldDestroy = false;
                            break;
                        }
                        else
                        {
                            shouldDestroy = true;
                        }
                    }
                    catch
                    {
                        shouldDestroy = true;
                    }
                }
            }
            if (shouldDestroy)
            {
                pickedUp = false;
                
                DestroyBadObject("pickedUp");
            }
        }

        //in the wrong cell
        if (!inWrongCell && zonesScript.isTouchingCells)
        {
            //check if mailman or librarian
            string job = player.GetComponent<PlayerCollectionData>().playerData.job;
            string period = scheduleScript.periodCode;
            if (!((job == "Mailman" || job == "Library") && period == "W"))
            {
                inWrongCell = true;

                BadObjectData data = new BadObjectData
                {
                    isMultiplied = true,
                    heatGain = 10,
                    messageType = "Guards_Cell",
                    attachedObject = player
                };

                CreateBadObject(data, "inWrongCell");
            }
        }
        else if (inWrongCell && !zonesScript.isTouchingCells)
        {
            inWrongCell = false;
            DestroyBadObject("inWrongCell");
        }


        //if player is punching
        if (!playerPunching && player.GetComponent<Combat>().isPunching)
        {
            playerPunching = true;

            BadObjectData data = new BadObjectData
            {
                isMultiplied = true,
                heatGain = 15,
                shouldAggro = true,
                messageType = "Guards_Halt",
                attachedObject = player
            };
            CreateBadObject(data, "playerPunching");
        }
        else if (playerPunching && !player.GetComponent<Combat>().isPunching)
        {
            playerPunching = false;
            DestroyBadObject("playerPunching");
        }

        //looting an inmate/guard
        if(tookInmateItem && !npcLoot)
        {
            npcLoot = true;

            BadObjectData data = new BadObjectData
            {
                shouldAggro = true,
                heatGain = 25,
                messageType = "I saw that", //this is not in the speech file lol
                attachedObject = player
            };
            CreateBadObject(data, "npcLoot");
        }
        else if(npcLoot && !tookInmateItem)
        {
            npcLoot = false;
            DestroyBadObject("npcLoot");
        }

        //not at the right routine(not work)
        if (!wrongRoutine && !zonesScript.isTouchingCurrentZone && scheduleScript.periodCode != "W" && zonesScript.timeInPeriod / .75f >= 25f)
        {
            wrongRoutine = true;

            BadObjectData data = new BadObjectData
            {
                heatGain = 10,
                messageType = "Guards_Move",
                attachedObject = player
            };
            CreateBadObject(data, "wrongRoutine");
        }
        else if (wrongRoutine && (zonesScript.isTouchingCurrentZone || scheduleScript.period == "W" || zonesScript.timeInPeriod / .75f < 25f))
        {
            wrongRoutine = false;
            DestroyBadObject("wrongRoutine");
        }

        //not at work
        if (!notAtWork && !zonesScript.isTouchingCurrentZone && scheduleScript.periodCode == "W" && zonesScript.timeInPeriod / .75f <= 25f)
        {
            notAtWork = true;

            BadObjectData data = new BadObjectData
            {
                heatGain = 10,
                messageType = "Guards_Move_Work",
                attachedObject = player
            };
            CreateBadObject(data, "notAtWork");
        }
        else if (notAtWork && (zonesScript.isTouchingCurrentZone || scheduleScript.periodCode != "W" || zonesScript.timeInPeriod / .75f > 25f))
        {
            notAtWork = false;
            DestroyBadObject("notAtWork");
        }

        if(!outLate && (!player.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled || hasInmateOutfit) && unlockDoorsScript.hasLockedCells && !zonesScript.isTouchingYourCell)
        {
            outLate = true;
            BadObjectData data = new BadObjectData
            {
                solitary = true,
                attachedObject = player
            };
            CreateBadObject(data, "outLate");
        }
        else if(!outLate && (!(!player.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled || hasInmateOutfit) || !unlockDoorsScript.hasLockedCells || zonesScript.isTouchingYourCell))
        {
            outLate = false;
            DestroyBadObject("outLate");
        }

        //no outfit
        if (!noOutfit && mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData == null)
        {
            noOutfit = true;

            BadObjectData data = new BadObjectData
            {
                isMultiplied = true,
                heatGain = 5,
                messageType = "Guards_Naked",
                attachedObject = player
            };
            CreateBadObject(data, "noOutfit");
        }
        else if(noOutfit && mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData != null)
        {
            noOutfit = false;
            DestroyBadObject("noOutfit");
        }

        //being in a non-inmate outfit around others
        if (!nonInmateOutfit && !HasInmateTypeOutfit())
        {
            nonInmateOutfit = true;

            BadObjectData guardData = new BadObjectData
            {
                heatGain = 1,
                attachedObject = player
            };
            BadObjectData inmateData = new BadObjectData
            {
                heatGain = 1,
                attachedObject = player,
                forInmate = true
            };
            CreateBadObject(guardData, "guardNonInmateOutfit");
            CreateBadObject(inmateData, "inmateNonInmateOutfit");
        }
        else if(nonInmateOutfit && HasInmateTypeOutfit())
        {
            nonInmateOutfit = false;
            DestroyBadObject("guardNonInmateOutfit");
            DestroyBadObject("inmateNonInmateOutfit");
        }

        //heat is over 89
        if (!highHeat && player.GetComponent<PlayerCollectionData>().playerData.heat > 89)
        {
            highHeat = true;

            BadObjectData data = new BadObjectData
            {
                shouldAggro = true,
                messageType = "Guards_Heat",
                attachedObject = player
            };
            CreateBadObject(data, "highHeat");
        }
        else if(highHeat && player.GetComponent<PlayerCollectionData>().playerData.heat <= 89)
        {
            highHeat = false;
            DestroyBadObject("highHeat");
        }

        //breaking a tile
        if (!breakingTile && (itemBehavioursScript.isChipping || itemBehavioursScript.isCutting ||
            itemBehavioursScript.isDigging || itemBehavioursScript.isScrewing))
        {
            breakingTile = true;

            BadObjectData inmateData = new BadObjectData
            {
                heatGain = 30,
                messageType = "SeeEscape_Call",
                shouldCall = true,
                attachedObject = player,
                forInmate = true
            };
            BadObjectData guardData = new BadObjectData
            {
                heatSet = 99,
                shouldAggro = true,
                messageType = "Guards_Halt",
                attachedObject = player
            };
            CreateBadObject(inmateData, "inmateBreakingTile");
            CreateBadObject(guardData, "guardBreakingTile");
        }
        else if((breakingTile && !(itemBehavioursScript.isChipping || itemBehavioursScript.isCutting ||
            itemBehavioursScript.isDigging || itemBehavioursScript.isScrewing)) || breakingTile && 
            player.layer != 3)
        {
            breakingTile = false;
            DestroyBadObject("inmateBreakingTile");
            DestroyBadObject("guardBreakingTile");
        }

        //outside at inside prison
        if(isOutside && currentMap.grounds == "Inside" && !outsideOnInsideMap && !zonesScript.isTouchingSafe)
        {
            outsideOnInsideMap = true;
            BadObjectData data = new BadObjectData
            {
                heatSet = 99,
                shouldAggro = true,
                messageType = "Guards_Halt",
                attachedObject = player
            };
            CreateBadObject(data, "outsideOnInsideMap");
        }
        else if(!isOutside && (currentMap.grounds != "Inside" || outsideOnInsideMap || zonesScript.isTouchingSafe))
        {
            outsideOnInsideMap = false;
            DestroyBadObject("outsideOnInsideMap");
        }


        //sees you during lockdown from rollcall
        if(lockdownScript.lockdownIsActive && !lockdownScript.isRiotLockdown && !missedRollcall)
        {
            missedRollcall = true;
            BadObjectData data = new BadObjectData
            {
                attachedObject = player
            };
            CreateBadObject(data, "missedRollcall");
            Debug.Log("making missedRollcall");
        }
        else if(missedRollcall && (!lockdownScript.lockdownIsActive || lockdownScript.isRiotLockdown))
        {
            missedRollcall = false;
            DestroyBadObject("missedRollcall");
            Debug.Log("destroying msisedRolclalllalfladjkfasj;dfklasj;f");
        }

        ///not bad object dependent
        //99 heat outside
        if((isOutside || 
            !Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Roof"))) &&
            player.GetComponent<PlayerCollectionData>().playerData.heat > 89 && !player.GetComponent<PlayerCollectionData>().playerData.isDead &&
            !isShooting)
        {
            isShooting = true;
            StartCoroutine(SniperShoot());
        }

        if(!Physics2D.GetIgnoreLayerCollision(playerLayer, LayerMask.NameToLayer("Roof")) && hasInmateOutfit)
        {
            player.GetComponent<PlayerCollectionData>().playerData.heat = 99;
        }

        //chipping/cutting/etc outside
        if(isOutside && !isShooting &&
            (itemBehavioursScript.isChipping || itemBehavioursScript.isCutting ||
            itemBehavioursScript.isDigging || itemBehavioursScript.isScrewing))
        {
            player.GetComponent<PlayerCollectionData>().playerData.heat = 99;
        }

        //unsafe outfit
        if ((isOutside || !Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Roof"))) && hasBadOutfit && !isGivingOutfitHeat && routineScript.periodCode != "LO")
        {
            isGivingOutfitHeat = true;
            if(!Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Roof")))
            {
                StartCoroutine(HeatGain(5));
            }
            StartCoroutine(HeatGain(3));
        }
    }
    private IEnumerator HeatGain(int amount)
    {
        player.GetComponent<PlayerCollectionData>().playerData.heat += amount;
        yield return new WaitForSeconds(1);
        isGivingOutfitHeat = false;
    }
    public IEnumerator TookInmateItem()
    {
        if (tookInmateItem)
        {
            yield break;
        }
        tookInmateItem = true;

        float time = 0f;
        while (time < .35f)
        {
            if (pc.isPaused)
            {
                yield return null;
                continue;
            }
            time += Time.deltaTime;
            yield return null;
        }
        tookInmateItem = false;
    }
    private IEnumerator SniperShoot()
    {
        if (!hasSniper)
        {
            yield break;
        }
        if (player.GetComponent<PlayerCollectionData>().playerData.inGodMode)
        {
            yield break;
        }
        if(scheduleScript.periodCode == "LO")
        {
            yield break;
        }
        
        //get random sniper
        int rand = UnityEngine.Random.Range(0, sniperList.Count);
        GameObject sniper = sniperList[rand];

        //get size position and rotation
        Vector2 pos1 = player.transform.position;
        Vector2 pos2 = sniper.transform.position;

        float length = Vector2.Distance(pos1, pos2);

        float a = pos2.y - pos1.y;
        float b = pos2.x - pos1.x;

        Vector2 midPoint = new Vector2((b / 2f) + pos1.x, (a / 2f) + pos1.y);

        Vector2 direction = pos2 - pos1;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        //instantiate the shoot
        GameObject bullet = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Bullet"));
        bullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
        bullet.name = "Bullet";
        bullet.transform.position = midPoint;
        bullet.transform.rotation = rotation;
        bullet.GetComponent<SpriteRenderer>().size = new Vector2(length, .3f);

        StartCoroutine(fightFX.MakeScreenShake());

        //hurt player
        player.GetComponent<PlayerCollectionData>().playerData.health -= 20;
        if(player.GetComponent<PlayerCollectionData>().playerData.health <= 0)
        {
            deathScript.KillPlayer();
        }

        yield return new WaitForSeconds(.4f);
        Destroy(bullet);
        yield return new WaitForSeconds(1);
        isShooting = false;
    }
    private bool HasInmateTypeOutfit()
    {
        if (mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData == null)
        {
            return true;
        }

        int id = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id;

        if(id == 29 || id == 30 || id == 31 || id == 32 || id == 33 || id == 34 || id == 35 || id == 36 ||
            id == 40 || id == 41 || id == 42 || id == 43 || id == 45 || id == 46 || id == 47 || id == 48 ||
            id == 50 || id == 51 || id == 52 || id == 53)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CreateBadObject(BadObjectData data, string objName)
    {
        GameObject badObject = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/BadObject"));
        BadObjectData badObjectData = badObject.GetComponent<BadObjectData>();

        badObjectData.isMultiplied = data.isMultiplied;
        badObjectData.heatGain = data.heatGain;
        badObjectData.heatSet = data.heatSet;
        badObjectData.shouldAggro = data.shouldAggro;
        badObjectData.solitary = data.solitary;
        badObjectData.item = data.item;
        badObjectData.toilet = data.toilet;
        badObjectData.untie = data.untie;
        badObjectData.sheets = data.sheets;
        badObjectData.messageType = data.messageType;
        badObjectData.shouldCall = data.shouldCall;
        badObjectData.attachedObject = data.attachedObject;
        badObjectData.forInmate = data.forInmate;

        badObject.name = objName;
        badObject.transform.parent = badObjects;
    }
    public void DestroyBadObject(string objName)
    {
        foreach(Transform badObj in badObjects)
        {
            if(badObj.name == objName)
            {
                Destroy(badObj.gameObject);
            }
        }
    }
}
