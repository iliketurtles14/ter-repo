using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Sittables : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private VitalController vitalScript;
    private GameObject player;
    private Transform tiles;
    private HPAChecker HPAScript;
    private Transform mc;
    public bool onSittable = false;
    public bool clearTile;
    private GameObject goToTile;
    public GameObject sittable;
    public bool onBed;
    private bool isBusy;
    public bool inLocker;
    private InventorySelection selectionScript;
    private int previousBodyLayer;
    private int previousOutfitLayer;
    private PlayerShowerOutfit pso;
    public bool canLeaveSittable;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        vitalScript = player.GetComponent<VitalController>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        HPAScript = player.GetComponent<HPAChecker>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        selectionScript = GetComponent<InventorySelection>();
        pso = player.GetComponent<PlayerShowerOutfit>();
    }
    public void Update()
    {
        HPAScript.isSeated = onSittable;
        isBusy = HPAScript.isBusy;
        
        if(!pso.isShowering && !onSittable)
        {
            ResetRates();
        }
        
        if (!isBusy && mcs.isTouchingSittable && Input.GetMouseButtonDown(0) && !onSittable && !selectionScript.aSlotSelected)
        {
            float distance = Vector2.Distance(player.transform.position, mcs.touchedSittable.transform.position);
            if (mcs.touchedSittable.name == "Locker")
            {
                distance = Vector2.Distance(player.transform.position, mcs.touchedSittable.transform.position + new Vector3(0, -.8f));
            }
            if(distance <= 2.4f)
            {
                sittable = mcs.touchedSittable;
                StartCoroutine(ClimbSittable());
            }
        }

        if(canLeaveSittable && onSittable && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            clearTile = false;
            Vector3 bedOffset;
            Vector3 otherPossibleTileBedOffset;
            if (sittable.name.StartsWith("PlayerBed") || sittable.name.StartsWith("SunChair") ||
                sittable.name.StartsWith("MedicBed") || sittable.name == "SolitaryBed") //when making a horizontal bed, change otherPossibleTileBedOffset to -1.6, 0
            {
                bedOffset = new Vector3(0, .8f);
                otherPossibleTileBedOffset = new Vector3(0, -1.6f);
                onBed = true;
            }
            else
            {
                bedOffset = Vector3.zero;
                otherPossibleTileBedOffset = Vector3.zero;
                onBed = false;
            }

            Vector3 lockerOffset;
            if (sittable.name.StartsWith("Locker"))
            {
                lockerOffset = new Vector3(0, -.8f);
                otherPossibleTileBedOffset = new Vector3(0, -1.6f);
                inLocker = true;
            }
            else
            {
                lockerOffset = Vector3.zero;
                otherPossibleTileBedOffset = Vector3.zero;
                inLocker = false;
            }

            Vector3 leaveVector = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                leaveVector = new Vector3(0, 1.6f);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                leaveVector = new Vector3(-1.6f, 0);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                leaveVector = new Vector3(0, -1.6f);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                leaveVector = new Vector3(1.6f, 0);
            }

            if (onBed && Input.GetKeyDown(KeyCode.S))
            {
                return;
            }
            if(inLocker && Input.GetKeyDown(KeyCode.W))
            {
                return;
            }
            Vector3 wantedTilePos = leaveVector + bedOffset + lockerOffset + sittable.transform.position;
            foreach (Transform tile in tiles.Find("Ground"))
            {
                if (tile.CompareTag("Digable"))
                {
                    if (Vector2.Distance(tile.position, wantedTilePos) <= .1f)
                    {
                        clearTile = true;
                        goToTile = tile.gameObject;
                        break;
                    }
                }
            }
            if (!clearTile && onBed)
            {
                foreach(Transform tile in tiles.Find("Ground"))
                {
                    if (tile.CompareTag("Digable"))
                    {
                        if(Vector2.Distance(tile.position, wantedTilePos + otherPossibleTileBedOffset) <= .1f)
                        {
                            clearTile = true;
                            goToTile = tile.gameObject;
                            break;
                        }
                    }
                }
            }

            if (clearTile)
            {
                StartCoroutine(LeaveSittable());
            }
        }
    }
    public IEnumerator LeaveSittable()
    {
        onSittable = false;
        ResetRates();
        
        player.GetComponent<PlayerAnimation>().enabled = true;

        if (sittable.name.StartsWith("Locker"))
        {
            player.GetComponent<SpriteRenderer>().sortingOrder = previousBodyLayer;
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = previousOutfitLayer;
        }

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        while(Vector2.Distance(player.transform.position, goToTile.transform.position) > .1f)
        {
            player.transform.position += 5f * Time.deltaTime * (goToTile.transform.position - player.transform.position).normalized;
            yield return null;
        }
        player.transform.position = goToTile.transform.position;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<PlayerCtrl>().enabled = true;//dont remove ts line
        player.GetComponent<PlayerCtrl>().canMove = true;

        sittable.GetComponent<BoxCollider2D>().enabled = true;
        sittable = null;
    }
    public IEnumerator ClimbSittable()
    {
        sittable.GetComponent<BoxCollider2D>().enabled = false;

        Vector3 climbOffset;
        if (sittable.name.StartsWith("PlayerBed") || sittable.name.StartsWith("SunChair") ||
            sittable.name.StartsWith("MedicBed") || sittable.name == "SolitaryBed")
        {
            if (NPCSave.instance.playerCharacter != 1) //if not baldeagle (cuz baldeagle has a smaller sleeping sprite)
            {
                climbOffset = new Vector3(0, .4f);
                onBed = true;
            }
            else
            {
                climbOffset = new Vector3(0, .35f);
                onBed = true;
            }
        }
        else if (sittable.name.StartsWith("Locker"))
        {
            climbOffset = new Vector3(0, -.8f);
            inLocker = true;
        }
        else
        {
            climbOffset = Vector3.zero;
        }

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerCtrl>().canMove = false;
        if(sittable.GetComponent<WaypointData>() != null)
        {
            player.GetComponent<PlayerAnimation>().lookDir = sittable.GetComponent<WaypointData>().dir;
        }
        while (Vector2.Distance(player.transform.position, sittable.transform.position + climbOffset) > .1f)
        {
            player.transform.position += 5f * Time.deltaTime * (sittable.transform.position + climbOffset - player.transform.position).normalized;
            yield return null;
        }
        player.transform.position = sittable.transform.position + climbOffset;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        onSittable = true;

        BodyController bc = player.GetComponent<BodyController>();
        OutfitController oc = player.GetComponent<OutfitController>();

        if (sittable.name.StartsWith("PlayerBed"))
        {
            vitalScript.energyRate = 1;
            vitalScript.energyRateAmount = 2;
            player.GetComponent<PlayerAnimation>().enabled = false;
            player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
            if (player.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
                int outfitItemID = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id;
                if (outfitItemID == 29 || outfitItemID == 30 || outfitItemID == 31 || outfitItemID == 32) //check if its an inmate outfit (this is because the inmate sleeping outfit sprite is not 16x16 like every other sprite for some reason)
                {
                    if (NPCSave.instance.playerCharacter != 1)
                    {
                        player.transform.Find("Outfit").localPosition = new Vector3(0, -.025f, 0);
                    }
                    else
                    {
                        player.transform.Find("Outfit").localPosition = new Vector3(0, -.02f, 0);
                    }
                }
            }
        }
        else if (sittable.name.StartsWith("Seat") || sittable.name.StartsWith("VisitorPlayer"))
        {
            vitalScript.energyRate = 1;
            vitalScript.energyRateAmount = 2;
        }
        else if (sittable.name.StartsWith("MedicBed"))
        {
            vitalScript.energyRate = 1;
            vitalScript.energyRateAmount = 2;
            vitalScript.healthRate = 1;
            vitalScript.healthRateAmount = 1;
            player.GetComponent<PlayerAnimation>().enabled = false;
            player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
            if (player.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
                int outfitItemID = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id;
                if (outfitItemID == 29 || outfitItemID == 30 || outfitItemID == 31 || outfitItemID == 32) //check if its an inmate outfit (this is because the inmate sleeping outfit sprite is not 16x16 like every other sprite for some reason)
                {
                    if (NPCSave.instance.playerCharacter != 1)
                    {
                        player.transform.Find("Outfit").localPosition = new Vector3(0, -.025f, 0);
                    }
                    else
                    {
                        player.transform.Find("Outfit").localPosition = new Vector3(0, -.02f, 0);
                    }                
                }
            }
        }
        else if (sittable.name.StartsWith("SunChair") || sittable.name.StartsWith("SolitaryBed"))
        {
            vitalScript.energyRate = 1;
            vitalScript.energyRateAmount = 2;
            player.GetComponent<PlayerAnimation>().enabled = false;
            player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
            if (player.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
                if (oc.outfit == "Inmate") //check if its an inmate outfit (this is because the inmate sleeping outfit sprite is not 16x16 like every other sprite for some reason)
                {
                    if (NPCSave.instance.playerCharacter != 1)
                    {
                        player.transform.Find("Outfit").localPosition = new Vector3(0, -.025f, 0);
                    }
                    else
                    {
                        player.transform.Find("Outfit").localPosition = new Vector3(0, -.02f, 0);
                    }
                }
            }
        }
        else if (sittable.name.StartsWith("Locker"))
        {
            previousBodyLayer = player.GetComponent<SpriteRenderer>().sortingOrder;
            previousOutfitLayer = player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder;
            player.GetComponent<SpriteRenderer>().sortingOrder = 0;
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }
    public void ResetRates()
    {
        vitalScript.energyRate = 5;
        vitalScript.healthRate = 3;
        vitalScript.energyRateAmount = 1;
        vitalScript.healthRateAmount = 1;
    }
}
