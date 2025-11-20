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
    public bool onSittable = false;
    private Vector3 leaveVector;
    private bool clearTile;
    private GameObject goToTile;
    private GameObject sittable;
    private Vector3 bedOffset;
    private bool onBed;
    private Vector3 climbOffset;
    private bool isBusy;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        vitalScript = player.GetComponent<VitalController>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        HPAScript = player.GetComponent<HPAChecker>();
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

            if (sittable.name.StartsWith("PlayerBed") || sittable.name.StartsWith("SunChair") ||
                sittable.name.StartsWith("MedicBed"))
            {
                bedOffset = new Vector3(0, .8f);
                onBed = true;
            }
            else
            {
                bedOffset = Vector3.zero;
                onBed = false;
            }

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

            foreach (Transform tile in tiles.Find("Ground"))
            {
                if (tile.position == leaveVector + bedOffset + player.transform.position - climbOffset&& tile.gameObject.CompareTag("Digable"))
                {
                    clearTile = true;
                    goToTile = tile.gameObject;
                }
                else if (tile.position == leaveVector + bedOffset + player.transform.position - climbOffset && !tile.gameObject.CompareTag("Digable"))
                {
                    clearTile = false;
                    break;
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

        if (sittable.name.StartsWith("PlayerBed") || sittable.name.StartsWith("SunChair") ||
            sittable.name.StartsWith("MedicBed"))
        {
            climbOffset = new Vector3(0, .4f);
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
