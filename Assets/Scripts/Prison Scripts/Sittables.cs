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
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        vitalScript = player.GetComponent<VitalController>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        HPAScript = player.GetComponent<HPAChecker>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
    }
    public void Update()
    {
        HPAScript.isSeated = onSittable;
        isBusy = HPAScript.isBusy;
        
        if (!isBusy && mcs.isTouchingSittable && Input.GetMouseButtonDown(0) && !onSittable)
        {
            float distance = Vector2.Distance(player.transform.position, mcs.touchedSittable.transform.position);
            if(distance <= 2.4f)
            {
                sittable = mcs.touchedSittable;
                StartCoroutine(ClimbSittable());
            }
        }

        if(onSittable && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            clearTile = false;
            Vector3 bedOffset;
            if (sittable.name.StartsWith("PlayerBed") || sittable.name.StartsWith("SunChair") ||
                sittable.name.StartsWith("MedicBed"))
            {
                if(NPCSave.instance.playerCharacter != 1) //if not baldeagle (cuz baldeagle has a smaller sleeping sprite)
                {
                    bedOffset = new Vector3(0, .4f);
                    onBed = true;
                }
                else
                {
                    bedOffset = new Vector3(0, .45f);
                    onBed = true;
                }
            }
            else
            {
                bedOffset = Vector3.zero;
                onBed = false;
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

            Vector3 wantedTilePos = leaveVector + bedOffset + player.transform.position;
            foreach (Transform tile in tiles.Find("Ground"))
            {
                if (Vector2.Distance(tile.position, wantedTilePos) <= .01f)
                {
                    if (tile.gameObject.CompareTag("Digable"))
                    {
                        clearTile = true;
                        goToTile = tile.gameObject;
                    }
                    else if (!tile.gameObject.CompareTag("Digable"))
                    {
                        clearTile = false;
                        break;
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
        ResetRates();
        
        player.GetComponent<PlayerAnimation>().enabled = true;

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        while(Vector2.Distance(player.transform.position, goToTile.transform.position) > .1f)
        {
            player.transform.position += 5f * Time.deltaTime * (goToTile.transform.position - player.transform.position).normalized;
            yield return null;
        }
        player.transform.position = goToTile.transform.position;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<PlayerCtrl>().enabled = true;

        sittable.GetComponent<BoxCollider2D>().enabled = true;
        sittable = null;
        onSittable = false;
    }
    public IEnumerator ClimbSittable()
    {
        sittable.GetComponent<BoxCollider2D>().enabled = false;

        Vector3 climbOffset;
        if (sittable.name.StartsWith("PlayerBed") || sittable.name.StartsWith("SunChair") ||
            sittable.name.StartsWith("MedicBed"))
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
        else
        {
            climbOffset = Vector3.zero;
        }

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerCtrl>().enabled = false;
        while(Vector2.Distance(player.transform.position, sittable.transform.position + climbOffset) > .1f)
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
        else if (sittable.name.StartsWith("Seat"))
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
        else if (sittable.name.StartsWith("SunChair"))
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
    }
    public void ResetRates()
    {
        vitalScript.energyRate = 5;
        vitalScript.healthRate = 3;
        vitalScript.energyRateAmount = 1;
        vitalScript.healthRateAmount = 1;
    }
}
