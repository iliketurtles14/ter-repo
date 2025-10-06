using JetBrains.Annotations;
using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class Exercising : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private ItemBehaviours itemBehavioursScript;
    private Transform tiles;
    private GameObject ic;
    private Transform so;
    private ApplyPrisonData applyPrisonDataScript;
    private GameObject barLine;
    private GameObject actionBarPanel;
    public GameObject currentEquipment;
    public bool onEquipment = false;
    private bool onQ = true;
    private bool onE = false;
    private int amountOfBars = 0;
    private Vector3 leaveVector;
    public bool clearTile;
    private GameObject goToTile;
    private bool isLeaving;
    public bool hasAdded;
    private int subDistanceNum;
    private Vector3 offset;
    private bool running;
    private bool punching;
    private bool isGoingDown;
    private int subGain;
    public void Start()
    {
        so = RootObjectCache.GetRoot("ScriptObject").transform;
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        itemBehavioursScript = so.GetComponent<ItemBehaviours>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        ic = RootObjectCache.GetRoot("InventoryCanvas");
        applyPrisonDataScript = so.GetComponent<ApplyPrisonData>();
        actionBarPanel = ic.transform.Find("ActionBarPanel").gameObject;

        barLine = Resources.Load<GameObject>("BarLine");
        ic.transform.Find("ActionBarHitBox").GetComponent<Image>().enabled = false;
        StartCoroutine(BarLoop());
    }
    public void Update()
    {
        if (mcs.isTouchingEquipment && Input.GetMouseButtonDown(0) && !onEquipment)
        {
            float distance = Vector2.Distance(transform.position, mcs.touchedEquipment.transform.position);
            if(distance <= 2.4f)
            {
                currentEquipment = mcs.touchedEquipment;
                onEquipment = true;
                StartCoroutine(ClimbEquipment());
            }
        }

        if(onEquipment && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            GetComponent<PlayerAnimation>().enabled = true;
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

            clearTile = false;

            foreach (Transform tile in tiles.Find("Ground"))
            {
                if (tile.position == leaveVector + (transform.position - offset) && tile.gameObject.CompareTag("Digable"))
                {
                    clearTile = true;
                    goToTile = tile.gameObject;
                }
                else if(tile.position == leaveVector + (transform.position - offset) && !tile.gameObject.CompareTag("Digable"))
                {
                    clearTile = false;
                    break;
                }
            }

            if (clearTile)
            {
                StartCoroutine(LeaveEquipment());
            }
        }

        try
        {
            if (currentEquipment.name.StartsWith("SpeedBag"))
            {
                SpriteRenderer sr = currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>();
                sr.size = new Vector2((sr.sprite.rect.width / sr.sprite.pixelsPerUnit) * 10, (sr.sprite.rect.height / sr.sprite.pixelsPerUnit) * 10);
            }
        }
        catch { }
        
        try
        {
            if (currentEquipment.name.StartsWith("PunchBag"))
            {
                SpriteRenderer sr = currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>();
                sr.size = new Vector2((sr.sprite.rect.width / sr.sprite.pixelsPerUnit) * 10, (sr.sprite.rect.height / sr.sprite.pixelsPerUnit) * 10);
            }
        }
        catch { }
    }
    public IEnumerator ClimbEquipment()
    {
        currentEquipment.GetComponent<BoxCollider2D>().enabled = false;

        if (currentEquipment.name.StartsWith("BenchPress"))
        {
            offset = new Vector3(0, 0, 0);
        }
        else if (currentEquipment.name.StartsWith("Treadmill"))
        {
            offset = new Vector3(0, .4f);
        }
        else if (currentEquipment.name.StartsWith("RunningPad"))
        {
            offset = new Vector3(0, .4f);
        }
        else if (currentEquipment.name.StartsWith("PushupPad"))
        {
            offset = new Vector3(0, 0, 0);
        }
        else if (currentEquipment.name.StartsWith("SpeedBag"))
        {
            offset = new Vector3(-.4f, .4f);
        }
        else if (currentEquipment.name.StartsWith("PunchBag"))
        {
            offset = new Vector3(-.6f, .6f);
        }
        else if (currentEquipment.name.StartsWith("JumpRopePad"))
        {
            offset = new Vector3(0, .2f);
        }
        else if (currentEquipment.name.StartsWith("PullUpBar"))
        {
            offset = new Vector3(0, .1f);
        }
        else
        {
            offset = new Vector3(0, 0, 0);
        }
        
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<PlayerCtrl>().enabled = false;
        while (Vector2.Distance(transform.position, currentEquipment.transform.position + offset) > .1f)
        {
            transform.position += 5f * Time.deltaTime * ((currentEquipment.transform.position + offset)- transform.position).normalized;
            yield return null;
        }
        transform.position = currentEquipment.transform.position + offset;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        if (currentEquipment.name.StartsWith("Benchpress"))
        {
            StartCoroutine(BenchPress());
        }
        else if (currentEquipment.name.StartsWith("Treadmill") || currentEquipment.name.StartsWith("RunningMat"))
        {
            StartCoroutine(Treadmill());
        }
        else if (currentEquipment.name.StartsWith("PushupMat"))
        {
            StartCoroutine(PushupPad());
        }
        else if (currentEquipment.name.StartsWith("SpeedBag"))
        {
            StartCoroutine(SpeedBag());
        }
        else if (currentEquipment.name.StartsWith("PunchingMat"))
        {
            StartCoroutine(PunchBag());
        }
        else if (currentEquipment.name.StartsWith("JumpropeMat"))
        {
            StartCoroutine(JumpRopePad());
        }
        else if (currentEquipment.name.StartsWith("PullupBar"))
        {
            StartCoroutine(PullUpBar());
        }
    }
    public IEnumerator LeaveEquipment()
    {
        Debug.Log("Leaving Equipment.");
        running = false;
        isLeaving = true;
        itemBehavioursScript.DestroyActionBar();
        GetComponent<PlayerAnimation>().enabled = true;
        ic.transform.Find("ActionBarHitBox").GetComponent<Image>().enabled = false;

        if (currentEquipment.name.StartsWith("Benchpress"))
        {
            StopCoroutine(BenchPress());
        }
        else if (currentEquipment.name.StartsWith("Treadmill") || currentEquipment.name.StartsWith("RunningMat"))
        {
            StopCoroutine(Treadmill());
            StopCoroutine(TreadmillWalk());
        }
        else if (currentEquipment.name.StartsWith("PushupMat"))
        {
            StopCoroutine(PushupPad());
        }
        else if (currentEquipment.name.StartsWith("SpeedBag"))
        {
            StopCoroutine(SpeedBag());
            StopCoroutine(SpeedBagWalk());
        }
        else if (currentEquipment.name.StartsWith("PunchingMat"))
        {
            StopCoroutine(PunchBag());
            StopCoroutine(SpeedBagWalk());
        }
        else if (currentEquipment.name.StartsWith("JumpropeMat"))
        {
            StopCoroutine(JumpRopePad());
            StopCoroutine(JumpRopeWalk());
        }
        else if (currentEquipment.name.StartsWith("PullupBar"))
        {
            StopCoroutine(PullUpBar());
        }

        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        while(Vector2.Distance(transform.position, goToTile.transform.position) > .1f)
        {
            transform.position += 5f * Time.deltaTime * (goToTile.transform.position - transform.position).normalized;
            yield return null;
        }
        transform.position = goToTile.transform.position;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<PlayerCtrl>().enabled = true;

        currentEquipment.GetComponent<BoxCollider2D>().enabled = true;

        currentEquipment = null;
        onEquipment = false;
        isLeaving = false;
    }
    public IEnumerator BenchPress()
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();

        amountOfBars = 0;
        subGain = 0;

        while (onEquipment && !isLeaving)
        {
            if(!hasAdded && GetComponent<PlayerCollectionData>().playerData.energy < 100)
            {
                if (Input.GetKeyDown(KeyCode.Q) && onQ)
                {
                    onQ = false;
                    onE = true;

                    amountOfBars += 10;
                }
                else if (Input.GetKeyDown(KeyCode.E) && onE)
                {
                    onQ = true;
                    onE = false;

                    amountOfBars += 10;
                }
            }

            if (amountOfBars > 49)
            {
                amountOfBars = 49;
            }
            
            if(amountOfBars == 49 && !hasAdded)
            {
                subGain++;
                GetComponent<PlayerCollectionData>().playerData.energy += 5;
                if(subGain == 2)
                {
                    GetComponent<PlayerCollectionData>().playerData.strength++;
                    subGain = 0;
                }
                hasAdded = true;
            }

            if (amountOfBars >= 0 && amountOfBars < 16)
            {
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][0];
                if(transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][0];
                }
            }
            else if(amountOfBars >= 16 && amountOfBars < 32)
            {
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][1];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][1];
                }
            }
            else if(amountOfBars >= 32)
            {
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][2];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][2];
                }
            }

            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().sizeDelta = new Vector2(5 * amountOfBars, 25);
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().anchoredPosition = new Vector2(55f + (2.5f * amountOfBars), 47.5f);

            yield return null;
        }
    }
    public IEnumerator JumpRopePad()
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        StartCoroutine(JumpRopeWalk());
        ic.transform.Find("ActionBarHitBox").GetComponent<Image>().enabled = true;
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");

        amountOfBars = 0;
        subGain = 0;
        while(onEquipment && !isLeaving)
        {
            GetComponent<PlayerAnimation>().enabled = false;

            if (amountOfBars < 35)
            {
                ic.transform.Find("ActionBarHitBox").GetComponent<Image>().sprite = applyPrisonDataScript.UISprites[349];
            }
            else if (amountOfBars >= 35 && GetComponent<PlayerCollectionData>().playerData.energy < 100)
            {
                ic.transform.Find("ActionBarHitBox").GetComponent<Image>().sprite = applyPrisonDataScript.UISprites[347];
                if (Input.GetKeyDown(KeyCode.Q) && !hasAdded)
                {
                    subGain++;
                    GetComponent<PlayerCollectionData>().playerData.energy += 2;
                    if (subGain == 4)
                    {
                        GetComponent<PlayerCollectionData>().playerData.speed++;
                        subGain = 0;
                    }
                    hasAdded = true;
                }
            }
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().sizeDelta = new Vector2(5 * amountOfBars, 25);
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().anchoredPosition = new Vector2(55f + (2.5f * amountOfBars), 47.5f);

            yield return null;
        }
    }
    public IEnumerator PullUpBar()
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();

        amountOfBars = 0;
        subGain = 0;
        while (onEquipment && !isLeaving)
        {
            if (hasAdded == false && GetComponent<PlayerCollectionData>().playerData.energy < 100)
            {
                if (Input.GetKeyDown(KeyCode.Q) && onQ)
                {
                    onQ = false;
                    onE = true;

                    amountOfBars += 10;
                }
                else if (Input.GetKeyDown(KeyCode.E) && onE)
                {
                    onQ = true;
                    onE = false;

                    amountOfBars += 10;
                }
            }

            if (amountOfBars > 49)
            {
                amountOfBars = 49;
            }

            if (amountOfBars == 49 && !hasAdded)
            {
                subGain++;
                GetComponent<PlayerCollectionData>().playerData.energy += 5;
                if (subGain == 2)
                {
                    GetComponent<PlayerCollectionData>().playerData.strength++;
                    subGain = 0;
                }
                hasAdded = true;
            }

            if (amountOfBars >= 0 && amountOfBars < 16)
            {
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][0];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][0];
                }
            }
            else if (amountOfBars >= 16 && amountOfBars < 32)
            {
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][1];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][1];
                }
            }
            else if (amountOfBars >= 32)
            {
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][2];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][2];
                }
            }

            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().sizeDelta = new Vector2(5 * amountOfBars, 25);
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().anchoredPosition = new Vector2(55f + (2.5f * amountOfBars), 47.5f);

            yield return null;
        }
    }
    public IEnumerator PunchBag()
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        running = true;
        StartCoroutine(SpeedBagWalk());
        ic.transform.Find("ActionBarHitBox").GetComponent<Image>().enabled = true;
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();

        amountOfBars = 0;
        subGain = 0;
        while(onEquipment && !isLeaving)
        {
            GetComponent<PlayerAnimation>().enabled = false;

            if (amountOfBars < 35)
            {
                ic.transform.Find("ActionBarHitBox").GetComponent<Image>().sprite = applyPrisonDataScript.UISprites[349];
                punching = false;
            }
            else if (amountOfBars >= 35 && GetComponent<PlayerCollectionData>().playerData.energy < 100)
            {
                ic.transform.Find("ActionBarHitBox").GetComponent<Image>().sprite = applyPrisonDataScript.UISprites[347];
                if (Input.GetKeyUp(KeyCode.Q) && !hasAdded)
                {
                    punching = true;
                    subGain++;
                    GetComponent<PlayerCollectionData>().playerData.energy += 3;
                    if (subGain == 3)
                    {
                        GetComponent<PlayerCollectionData>().playerData.strength++;
                        subGain = 0;
                    }
                    hasAdded = true;
                    StartCoroutine(PunchBagPunch());
                }
            }

            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().sizeDelta = new Vector2(5 * amountOfBars, 25);
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().anchoredPosition = new Vector2(55f + (2.5f * amountOfBars), 47.5f);

            yield return null;
        }
    }
    public IEnumerator JumpRopeWalk()
    {
        int cycle = 0;
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();
        while (true)
        {
            if(cycle == 8)
            {
                cycle = 0;
            }
            GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][9][cycle];
            if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][9][cycle];
            }
            
            //timer for .266f seconds
            float timer = 0f;
            while(timer < .266f)
            {
                if (isLeaving) { yield break; }
                timer += Time.deltaTime;
                yield return null;
            }
            
            cycle++;
        }
    }
    public IEnumerator SpeedBag()
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        running = true;
        StartCoroutine(SpeedBagWalk());
        ic.transform.Find("ActionBarHitBox").GetComponent<Image>().enabled = true;
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();

        amountOfBars = 0;
        subGain = 0;
        while (onEquipment && !isLeaving)
        {
            GetComponent<PlayerAnimation>().enabled = false;
            
            if (amountOfBars < 35)
            {
                ic.transform.Find("ActionBarHitBox").GetComponent<Image>().sprite = applyPrisonDataScript.UISprites[349];
                punching = false;
            }
            else if (amountOfBars >= 35 && GetComponent<PlayerCollectionData>().playerData.energy < 100)
            {
                ic.transform.Find("ActionBarHitBox").GetComponent<Image>().sprite = applyPrisonDataScript.UISprites[347];
                if (Input.GetKeyDown(KeyCode.Q) && !hasAdded)
                {
                    punching = true;
                    subGain++;
                    GetComponent<PlayerCollectionData>().playerData.energy += 2;
                    if (subGain == 4)
                    {
                        GetComponent<PlayerCollectionData>().playerData.speed++;
                        subGain = 0;
                    }
                    hasAdded = true;
                    StartCoroutine(SpeedBagPunch());
                }
            }

            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().sizeDelta = new Vector2(5 * amountOfBars, 25);
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().anchoredPosition = new Vector2(55f + (2.5f * amountOfBars), 47.5f);

            yield return null;
        }
    }
    public IEnumerator PunchBagPunch()
    {
        Debug.Log("bag is punched");
        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[235];
        yield return new WaitForSeconds(.15f);
        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[237];
        yield return new WaitForSeconds(.15f);
        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[234];
    }
    public IEnumerator SpeedBagPunch()
    {
        Debug.Log("bag is punched");
        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[240];
        yield return new WaitForSeconds(.117f);
        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[241];
        yield return new WaitForSeconds(.117f);
        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[239];
        yield return new WaitForSeconds(.117f);
        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[258];
    }
    public IEnumerator SpeedBagWalk()//stuipd supid stupdi stupid
    {
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();

        while (true)
        {
            if (punching)
            {
                Debug.Log("doing punch anim");
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][3][0];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][3][0];
                }
                punching = false;

                // Custom timer for .467 seconds
                float timer = 0f;
                while (timer < 0.467f)
                {
                    if (!running) { Debug.Log("stopping early during punch anim"); yield break; } // Exit coroutine early
                    if (punching) { Debug.Log("punching again during punch anim"); break; } // Break timer to return to main loop
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                Debug.Log("not punching");
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][0];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][0];
                }

                if (!running) { Debug.Log("not running1"); break; }
                if (punching) { Debug.Log("punching1"); continue; }

                // Custom timer for .266 seconds
                float timer = 0f;
                while (timer < 0.266f)
                {
                    if (!running) { Debug.Log("stopping early during wait"); yield break; } // Exit coroutine early
                    if (punching) { Debug.Log("punching again during wait"); break; } // Break timer to return to main loop
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (!running) { Debug.Log("not running2"); break; }
                if (punching) { Debug.Log("punching2"); continue; }

                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][1];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                }

                // Custom timer for another .266 seconds
                timer = 0f;
                while (timer < 0.266f)
                {
                    if (!running) { Debug.Log("stopping early during second wait"); yield break; } // Exit coroutine early
                    if (punching) { Debug.Log("punching again during second wait"); break; } // Break timer to return to main loop
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            yield return null;
        }
    } // also for punchbag
    public IEnumerator PushupPad()
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();

        amountOfBars = 0;
        subGain = 0;
        while(onEquipment && !isLeaving)
        {
            if(hasAdded == false && GetComponent<PlayerCollectionData>().playerData.energy < 100)
            {
                if (Input.GetKeyDown(KeyCode.Q) && onQ)
                {
                    onQ = false;
                    onE = true;

                    amountOfBars += 10;
                }
                else if (Input.GetKeyDown(KeyCode.E) && onE)
                {
                    onQ = true;
                    onE = false;

                    amountOfBars += 10;
                }
            }

            if (amountOfBars > 49)
            {
                amountOfBars = 49;
            }

            if (amountOfBars == 49 && !hasAdded)
            {
                subGain++;
                GetComponent<PlayerCollectionData>().playerData.energy += 5;
                if (subGain == 2)
                {
                    GetComponent<PlayerCollectionData>().playerData.strength++;
                    subGain = 0;
                }
                hasAdded = true;
            }

            if(amountOfBars < 25)
            {
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][7][0];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][7][0];
                }
            }
            else if(amountOfBars >= 25)
            {
                GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][7][1];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][7][1];
                }
            }

            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().sizeDelta = new Vector2(5 * amountOfBars, 25);
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().anchoredPosition = new Vector2(55f + (2.5f * amountOfBars), 47.5f);
            yield return null;
        }
    }
    public IEnumerator Treadmill() //also for running mat
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");

        amountOfBars = 0;
        subGain = 0;
        subDistanceNum = 0;

        running = true;
        StartCoroutine(TreadmillWalk());
        while(onEquipment && !isLeaving)
        {
            GetComponent<PlayerAnimation>().enabled = false;
            if (Input.GetKeyDown(KeyCode.Q) && onQ && GetComponent<PlayerCollectionData>().playerData.energy < 100)
            {
                onQ = false;
                onE = true;
                subDistanceNum++;
                if(amountOfBars >= 48)
                {
                    amountOfBars = 49;
                }
                else if(amountOfBars < 48)
                {
                    amountOfBars += 3;
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) && onE && GetComponent<PlayerCollectionData>().playerData.energy < 100)
            {
                onQ = true;
                onE = false;
                subDistanceNum++;
                if (amountOfBars >= 48)
                {
                    amountOfBars = 49;
                }
                else if (amountOfBars < 48)
                {
                    amountOfBars += 3;
                }
            }

            if (subDistanceNum == 20)
            {
                subDistanceNum = 0;
                subGain++;
                GetComponent<PlayerCollectionData>().playerData.energy += 5;
                if (subGain == 2)
                {
                    GetComponent<PlayerCollectionData>().playerData.speed++;
                    subGain = 0;
                }
            }
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().sizeDelta = new Vector2(5 * amountOfBars, 25);
            actionBarPanel.transform.Find("BarLine").GetComponent<RectTransform>().anchoredPosition = new Vector2(55f + (2.5f * amountOfBars), 47.5f);

            yield return null;
        }
    }
    public IEnumerator TreadmillWalk()
    {
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();

        while(true)
        {
            GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][0];
            if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][0];
            }
            if (!running)
            {
                break;
            }
            yield return new WaitForSeconds(.266f);
            if (!running)
            {
                break;
            }
            GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][1];
            if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
            }
            if (!running)
            {
                break;
            }
            yield return new WaitForSeconds(.266f);
            if (!running)
            {
                break;
            }
        }
    }
    public IEnumerator BarLoop()
    {
        while (true)
        {
            if (onEquipment && (currentEquipment.name.StartsWith("Benchpress") || currentEquipment.name.StartsWith("PushupMat") || currentEquipment.name.StartsWith("PullupBar")))
            {
                if (amountOfBars > 0)
                {
                    amountOfBars--;
                    yield return new WaitForSeconds(.015f);
                }

                if(amountOfBars == 0)
                {
                    hasAdded = false;
                }
            }
            else if (onEquipment && (currentEquipment.name.StartsWith("Treadmill") || currentEquipment.name.StartsWith("RunningMat")))
            {
                if(amountOfBars > 0)
                {
                    amountOfBars--;
                    yield return new WaitForSeconds(.1f);
                }
            }
            else if(onEquipment && (currentEquipment.name.StartsWith("SpeedBag") || currentEquipment.name.StartsWith("JumpropeMat")))
            {
                amountOfBars++;
                if(amountOfBars == 50)
                {
                    amountOfBars = 0;
                    hasAdded = false;
                }
                yield return new WaitForSeconds(.01f);
            }
            else if(onEquipment && currentEquipment.name.StartsWith("PunchingMat"))
            {
                if (Input.GetKey(KeyCode.Q) && !isGoingDown)
                {
                    amountOfBars++;
                    if(amountOfBars > 49)
                    {
                        amountOfBars = 49;
                    }
                    yield return new WaitForSeconds(.007f);
                }
                else
                {
                    if(amountOfBars > 0)
                    {
                        isGoingDown = true;
                        amountOfBars--;
                        yield return new WaitForSeconds(.002f);
                    }
                    else
                    {
                        isGoingDown = false;
                    }
                }

                if(amountOfBars == 0)
                {
                    hasAdded = false;
                }
            }
            yield return null;
        }
    }
}
