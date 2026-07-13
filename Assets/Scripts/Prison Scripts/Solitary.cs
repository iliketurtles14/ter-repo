using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Solitary : MonoBehaviour
{
    private Transform solitaryCanvas;
    private Transform tiles;
    private Sittables sittablesScript;
    private Transform player;
    private VitalController vitalScript;
    private Transform ic;
    private Sprite clear;
    private Inventory inventoryScript;
    private PlayerIDInv playerIdInvScript;
    private Transform mc;
    private SetInitialOutfits setInitialOutfitsScript;
    private CreateNote noteScript;
    private Map currentMap;
    private Routine routineScript;
    private Transform aStar;
    private NPCSleep npcSleepScript;
    public bool inSolitary;
    private Schedule scheduleScript;
    public List<GameObject> damagedTiles = new List<GameObject>();
    private ToiletMenu toiletMenuScript;
    private bool startingSolitary;
    private LadderClimb ladderClimbScript;
    private void Start()
    {
        solitaryCanvas = RootObjectCache.GetRoot("SolitaryBlockerCanvas").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        sittablesScript = GetComponent<Sittables>();
        player = RootObjectCache.GetRoot("Player").transform;
        vitalScript = player.GetComponent<VitalController>();
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        inventoryScript = GetComponent<Inventory>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        playerIdInvScript = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
        setInitialOutfitsScript = GetComponent<SetInitialOutfits>();
        noteScript = GetComponent<CreateNote>();
        routineScript = ic.Find("Time").GetComponent<Routine>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        npcSleepScript = GetComponent<NPCSleep>();
        scheduleScript = ic.Find("Period").GetComponent<Schedule>();
        toiletMenuScript = mc.Find("ToiletMenuPanel").GetComponent<ToiletMenu>();
        ladderClimbScript = GetComponent<LadderClimb>();

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
    }
    private void Update()
    {
        if(inSolitary && scheduleScript.periodCode != "LO")
        {
            inSolitary = false;
        }

        //if (Input.GetKeyDown(KeyCode.F3))
        //{
        //    StartCoroutine(GoToSolitary("youre in solitary now heh"));
        //}
    }
    public IEnumerator GoToSolitary(string noteMsg)
    {
        if (player.GetComponent<PlayerCollectionData>().playerData.inGodMode)
        {
            yield break;
        }
        
        //spawn in solitary bed - 
        //go 3 days into the future !!! -
        //7:40 -
        //inmates go to beds -
        //make the solitary zone the current zone -
        //take all items away -
        //repair any broken stuff -
        //get contraband out of desk -
        //do black stuff first -
        //make note -
        //remove job -
        //finish black stuff -

        //start black fade anim and pause it
        if (startingSolitary)
        {
            yield break;
        }
        startingSolitary = true;
        solitaryCanvas.gameObject.SetActive(true);
        noteScript.CreateWardenNote("solitary", noteMsg, currentMap.warden);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1);
        solitaryCanvas.GetComponent<Animator>().speed = 0; //pause anim

        while (noteScript.inNote) //wait until note is cleared
        {
            yield return null;
        }

        inSolitary = true;

        //set time
        routineScript.sec = 40;
        routineScript.min = routineScript.startingMin;
        routineScript.day += 3;

        //reset extra npcs
        foreach(Transform npc in aStar)
        {
            if(npc.name == "JobOfficer" || npc.name == "Medic" || npc.name == "Warden")
            {
                npc.GetComponent<ExtraNPCAI>().StopAllCoroutines();
                npc.GetComponent<Seeker>().CancelCurrentPathRequest(true);
                npc.position = npc.GetComponent<ExtraNPCAI>().spawnWP.position;
                npc.Find("SpeechCanvas").gameObject.SetActive(false);
                npc.GetComponent<CapsuleCollider2D>().enabled = false;
                npc.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //send inmates to bed and send guards to random FT waypoints
        List<Transform> wps = new List<Transform>();
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name == "GuardWaypoint")
            {
                wps.Add(obj);
            }
        }
        foreach(Transform npc in aStar)
        {
            if(npc.name.Contains("Inmate") || npc.name.Contains("Guard"))
            {
                npc.GetComponent<NPCAI>().enabled = false;
                npc.GetComponent<AILerp>().enabled = false;
            }
            
            if (npc.name.Contains("Inmate") && npc.GetComponent<NPCCollectionData>().npcData.bed != null)
            {
                npcSleepScript.Sleep(npc.gameObject, npc.GetComponent<NPCCollectionData>().npcData.bed);
            }
            else if (npc.name.Contains("Inmate")) //default if no bed
            {
                npc.transform.position = new Vector2(0, 0);
            }
            else if (npc.name.Contains("Guard"))
            {
                if(wps.Count > 0)
                {
                    int rand = UnityEngine.Random.Range(0, wps.Count);
                    npc.transform.position = wps[rand].position;
                }
            }
        }
        yield return new WaitForEndOfFrame();
        foreach(Transform npc in aStar)
        {
            if (npc.name.Contains("Inmate") || npc.name.Contains("Guard"))
            {
                npc.GetComponent<NPCAI>().enabled = true;
                npc.GetComponent<AILerp>().enabled = true;
            }
        }

        //remove job
        player.GetComponent<PlayerCollectionData>().playerData.job = "";

        //remove items from inv
        foreach (Transform slot in ic.Find("GUIPanel"))
        {
            slot.GetComponent<Image>().sprite = clear;
        }
        foreach (InventoryItem item in inventoryScript.inventory)
        {
            item.itemData = null;
        }

        playerIdInvScript.idInv[0].itemData = setInitialOutfitsScript.outfitData;
        playerIdInvScript.idInv[1].itemData = null;
        mc.Find("PlayerMenuPanel").Find("Weapon").GetComponent<Image>().sprite = clear;

        //send to solitary bed
        GameObject solitaryBed = null;
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "SolitaryBed")
            {
                solitaryBed = obj.gameObject;
                break;
            }
        }
        if(solitaryBed != null)
        {
            sittablesScript.sittable = solitaryBed;
            solitaryBed.GetComponent<BoxCollider2D>().enabled = false;
            player.GetComponent<PlayerCtrl>().enabled = false;
            yield return new WaitForFixedUpdate();
            if(NPCSave.instance.playerCharacter != 1)
            {
                player.position = solitaryBed.transform.position + new Vector3(0, .4f);
            }
            else
            {
                player.position = solitaryBed.transform.position + new Vector3(0, .35f);
            }
            sittablesScript.onSittable = true;
            sittablesScript.onBed = true;

            BodyController bc = player.GetComponent<BodyController>();
            OutfitController oc = player.GetComponent<OutfitController>();

            vitalScript.energyRate = 1;
            vitalScript.energyRateAmount = 2;
            player.GetComponent<PlayerAnimation>().enabled = false;
            player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
            if(oc.outfit == "Inmate")
            {
                if(NPCSave.instance.playerCharacter != 1)
                {
                    player.transform.Find("Outfit").localPosition = new Vector3(0, -.025f, 0);
                }
                else
                {
                    player.transform.Find("Outfit").localPosition = new Vector3(0, -.02f, 0);
                }
            }

            ladderClimbScript.SendToGround();
        }
        //do stuff to stats
        PlayerData data = player.GetComponent<PlayerCollectionData>().playerData;
        data.energy = 0;
        data.heat = 0;
        data.strength -= 10;
        data.speed -= 10;
        data.intellect -= 10;
        if(data.strength < 10)
        {
            data.strength = 10;
        }
        if(data.speed < 10)
        {
            data.speed = 10;
        }
        if(data.intellect < 10)
        {
            data.intellect = 10;
        }
        data.health = Mathf.FloorToInt((data.strength / 2) * .75f);

        //remove contra items from player desk
        List<DeskData> datas = new List<DeskData>(); //multiple datas cuz possibility of multiple player desks
        List<string> layers = new List<string>
        {
            "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
        };
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform obj in tiles.Find(layers[i]))
            {
                if (obj.name.Contains("PlayerDesk"))
                {
                    datas.Add(obj.GetComponent<DeskData>());
                }
            }
        }
        for(int i = 0; i < datas.Count; i++)
        {
            foreach(DeskItem item in datas[i].deskInv)
            {
                if (item.itemData != null && item.itemData.isContraband)
                {
                    item.itemData = null;
                }
            }
        }

        //repair broken stuff
        foreach(GameObject tile in damagedTiles)
        {
            if(tile == null)
            {
                continue;
            }
            if(tile.GetComponent<TileCollectionData>() != null)
            {
                TileCollectionData tileColData = tile.GetComponent<TileCollectionData>();
                switch (tileColData.tileData.tileType)
                {
                    case "chip":
                    case "elec":
                    case "cut":
                    case "bar":
                        tile.GetComponent<BoxCollider2D>().enabled = true;
                        tileColData.tileData.currentDurability = 100;
                        break;
                }
                continue;
            }
            else if (tile.name == "SlatsHorizontal")
            {
                GameObject slat = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/SlatsHorizontal"));
                slat.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[139];
                slat.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
                slat.layer = tile.layer;
                slat.transform.position = tile.transform.position;
                slat.GetComponent<TileCollectionData>().tileData = new TileData
                {
                    tileType = null,
                    currentDurability = 100,
                    holeStability = -1
                };
                try
                {
                    Destroy(tile);
                }
                catch { }
            }
            else if(tile.name == "SlatsVertical")
            {
                GameObject slat = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/SlatsVertical"));
                slat.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[40];
                slat.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
                slat.layer = tile.layer;
                slat.transform.position = tile.transform.position;
                slat.GetComponent<TileCollectionData>().tileData = new TileData
                {
                    tileType = null,
                    currentDurability = 100,
                    holeStability = -1
                };
                try
                {
                    Destroy(tile);
                }
                catch { }
            }
        }

        List<string> tileLayers = new List<string>
        {
            "Underground", "Ground", "Vents", "Roof"
        };
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform tile in tiles.Find(tileLayers[i]))
            {
                if(tile.name == "BrokenTile")
                {
                    Destroy(tile.gameObject);
                }
            }
            foreach(Transform obj in tiles.Find(layers[i]))
            {
                if(obj.name == "Poster" || obj.name == "Stepladder" || obj.name == "FakeWallBlock" ||
                    obj.name.Contains("Hole") || obj.name == "DirtCrumbs" || obj.name == "Dirt(Clone)" ||
                    obj.name == "DirtEmpty(Clone)" || obj.name == "Rock(Clone)" || obj.name == "Brace(Clone)" ||
                    obj.name == "FakeVent" || obj.CompareTag("Item") || obj.name == "Sheet")
                {
                    Destroy(obj.gameObject);
                }
                else if(obj.name == "Vent")
                {
                    obj.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                }
                else if (obj.name.Contains("EmptyVentCover"))
                {
                    GameObject vent = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Vent"));
                    vent.GetComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[137];
                    vent.transform.position = obj.position;
                    vent.GetComponent<TileCollectionData>().tileData = new TileData();
                    vent.GetComponent<TileCollectionData>().tileData.tileType = null;
                    vent.GetComponent<TileCollectionData>().tileData.currentDurability = 100;
                    vent.GetComponent<TileCollectionData>().tileData.holeStability = -1;
                    Destroy(obj.gameObject);
                }
                else if (obj.name.Contains("Toilet"))
                {
                    if (obj.GetComponent<ToiletInv>().isClogged)
                    {
                        toiletMenuScript.UnclogToilet(obj.gameObject);
                    }
                    obj.GetComponent<ToiletInv>().flushTimer = 0;
                    for(int j = 0; j < 3; j++)
                    {
                        obj.GetComponent<ToiletInv>().toiletInv[j] = null;
                    }
                }

                if(obj.GetComponent<DeskRNG>() != null && !obj.name.Contains("PlayerDesk"))
                {
                    obj.GetComponent<DeskRNG>().RandomizeDesk();
                }
            }
        }

        //resume solitary anim
        solitaryCanvas.GetComponent<Animator>().speed = 1;
        yield return new WaitForSeconds(1);
        solitaryCanvas.gameObject.SetActive(false);
        startingSolitary = false;
    }
}
