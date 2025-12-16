using System.Collections;
using System.Net;
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
    private bool emptyBed;
    private void Start()
    {
        Transform scriptObject = RootObjectCache.GetRoot("ScriptObject").transform;

        player = RootObjectCache.GetRoot("Player");
        deskStandScript = scriptObject.GetComponent<DeskStand>();
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        itemBehavioursScript = GetComponent<ItemBehaviours>();
    }
    private void Update()
    {
        // this is only for certain bad actions. the reason some bad actions are missing is because
        // some bad actions can have multiple instances of it occuring (because of how this script is
        // set up, that cant happen within this script).
        // MISSING: inmate punch, toilet clog, tied up npc, items on the floor, sheets on bars,
        //          broken tiles, being in a perimeter/unsafe zone, being on the roof without a
        //          guard/bleached outfit, missing rollcall, being outside with a non-inmate outfit,
        //          missing routines,


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
                    if (desk.GetComponent<DeskPickUp>().isPickedUp)
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
                    if (desk.GetComponent<DeskPickUp>().isPickedUp)
                    {
                        shouldDestroy = false;
                        break;
                    }
                    else
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
        if (!inWrongCell && player.GetComponent<ZoneCollision>().isTouchingCellsZone)
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
        else if (inWrongCell && !player.GetComponent<ZoneCollision>().isTouchingCellsZone)
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

        //looting an inmate
        //MAKE INMATE INVENTORIES

        //not at the right routine (not work)
        if (!wrongRoutine && !AtRightRoutine(false))
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
        else if(wrongRoutine && AtRightRoutine(false))
        {
            wrongRoutine = false;
            DestroyBadObject("wrongRoutine");
        }

        //not at work
        if (!notAtWork && !AtRightRoutine(true))
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
        else if(notAtWork && AtRightRoutine(true))
        {
            notAtWork = false;
            DestroyBadObject("notAtWork");
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
            itemBehavioursScript.isDigging || itemBehavioursScript.isScrewing) && player.layer == 3)
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

        //empty bed at midnight
        //ADD BED DUMMIES

        //sees you during lockdown
        //ADD LOCKDOWN
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
    private bool AtRightRoutine(bool checkForWork)
    {
        if(routineScript.sec < 25)
        {
            return true;
        }
        
        if (!checkForWork)
        {
            string period = scheduleScript.periodCode;

            switch (period)
            {
                case "LO":
                    return player.GetComponent<ZoneCollision>().isTouchingPlayerCell;
                case "R":
                    return player.GetComponent<ZoneCollision>().isTouchingRollcallZone;
                case "B":
                case "L":
                case "D":
                    return player.GetComponent<ZoneCollision>().isTouchingCanteenZone;
                case "E":
                    return player.GetComponent<ZoneCollision>().isTouchingGymZone;
                case "S":
                    return player.GetComponent<ZoneCollision>().isTouchingShowersZone;
                default:
                    return true;
            }
        }
        else
        {
            if(scheduleScript.periodCode != "W")
            {
                return true;
            }
            
            string job = player.GetComponent<PlayerCollectionData>().playerData.job;

            switch (job)
            {
                case "Tailorshop":
                    return player.GetComponent<ZoneCollision>().isTouchingTailorshopZone;
                case "Laundry":
                    return player.GetComponent<ZoneCollision>().isTouchingLaundryZone;
                case "Woodshop":
                    return player.GetComponent<ZoneCollision>().isTouchingWoodshopZone;
                case "Library":
                    return player.GetComponent<ZoneCollision>().hasArrivedToCurrentPeriod;
                case "Deliveries":
                    return player.GetComponent<ZoneCollision>().hasArrivedToCurrentPeriod;
                case "Janitor":
                    return player.GetComponent<ZoneCollision>().hasArrivedToCurrentPeriod;
                case "Gardening":
                    return player.GetComponent<ZoneCollision>().hasArrivedToCurrentPeriod;
                case "Mailman":
                    return player.GetComponent<ZoneCollision>().hasArrivedToCurrentPeriod;
                case "Kitchen":
                    return player.GetComponent<ZoneCollision>().isTouchingKitchenZone;
                case "Metalshop":
                    return player.GetComponent<ZoneCollision>().isTouchingMetalshopZone;
            }

            return true;
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
