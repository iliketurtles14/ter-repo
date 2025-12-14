using UnityEngine;

public class MakeBadObject : MonoBehaviour
{
    private GameObject player;
    private DeskStand deskStandScript;
    private bool playerIsBad;
    private Transform badObjects;
    private Transform tiles;

    //these bools are to make sure the same badObject doesnt get made more than once at a given time
    //these basically just say what types of badObjects are currently active
    private bool onDesk;
    private bool searchingDesk;
    private bool pickedUp;
    private bool inWrongCell;
    private bool playerPunching;
    private bool wrongRoutine;
    private bool notAtWork;
    private void Start()
    {
        Transform scriptObject = RootObjectCache.GetRoot("ScriptObject").transform;

        player = RootObjectCache.GetRoot("Player");
        deskStandScript = scriptObject.GetComponent<DeskStand>();
        badObjects = RootObjectCache.GetRoot("BadObjects").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
    }
    private void Update()
    {
        // this is only for certain bad actions. the reason some bad actions are missing is because
        // some bad actions can have multiple instances of it occuring (because of how this script is
        // set up, that cant happen within this script).
        // MISSING: inmate punch, toilet clog, tied up npc, items on the floor, sheets on bars,
        //          broken tiles


        //standing on desk
        if(deskStandScript.hasClimbed && !onDesk) //desk stand or standing on anything bad
        {
            onDesk = true;

            BadObjectData data = new BadObjectData
            {
                isMultiplied = true,
                heatGain = 5,
                messageType = "OnDesk",
                attachedObject = player
            };
            CreateBadObject(data);
        }

        //searching a desk
        if (!searchingDesk)
        {
            foreach (Transform desk in tiles.Find("GroundObjects"))
            {
                if (desk.GetComponent<DeskInv>().isOpening)
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

                    CreateBadObject(guardData);
                    CreateBadObject(inmateData);
                }
            }
        }

        //picking up a desk/inmate
        if (!pickedUp)
        {
            foreach(Transform desk in tiles.Find("GroundObjects"))
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

                    CreateBadObject(data);
                }
            }
        }

        //in the wrong cell
        if(!inWrongCell && player.GetComponent<ZoneCollision>().isTouchingCellsZone)
        {
            inWrongCell = true;

            BadObjectData data = new BadObjectData
            {
                isMultiplied = true,
                heatGain = 10,
                messageType = "Guards_Cell",
                attachedObject = player
            };

            CreateBadObject(data);
        }

        //if player is punching
        if(!playerPunching && player.GetComponent<Combat>().isPunching)
        {
            playerPunching = true;

            BadObjectData data = new BadObjectData
            {
                isMultiplied = true,
                heatGain = 15,
                shouldAggro = true,
                messageType = "Guards_Halt",
            };
            CreateBadObject(data);
        }

        //looting an inmate
        //MAKE INMATE INVENTORIES

        //not at the right routine (not work)
        if(!wrongRoutine && !AtRightRoutine(false))
        {
            wrongRoutine = true;

            BadObjectData data = new BadObjectData
            {
                heatGain = 10,
                messageType = "Guards_Move",
                attachedObject = player
            };
            CreateBadObject(data);
        }

        //not at work
        if(!notAtWork && !AtRightRoutine(true))
        {
            notAtWork = true;

            BadObjectData data = new BadObjectData
            {
                heatGain = 10,
                messageType = "Guards_Move_Work",
                attachedObject = player
            };
            CreateBadObject(data);
        }
    }
    private bool AtRightRoutine(bool checkForWork)
    {
        return true; //remove this
        
        if (!checkForWork)
        {

        }
    }
    public void CreateBadObject(BadObjectData data)
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

        badObject.name = "BadObject";
        badObject.transform.parent = badObjects;
        badObject.layer = 10;
        badObject.tag = "BadObject";
    }
}
