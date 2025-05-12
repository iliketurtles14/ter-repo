using System.Collections;
using System.Transactions;
using UnityEngine;

public class Exercising : MonoBehaviour
{
    public MouseCollisionOnItems mcs;
    public ItemBehaviours itemBehavioursScript;
    public Transform perksTiles;
    private GameObject barLine;
    public GameObject actionBarPanel;
    public GameObject currentEquipment;
    public bool onEquipment = false;
    private bool onQ = true;
    private bool onE = false;
    private int amountOfBars = 0;
    private Vector3 leaveVector;
    private bool clearTile;
    private GameObject goToTile;
    private bool isLeaving;
    private bool hasAdded;
    private int subDistanceNum;
    private Vector3 offset;
    private bool running;
    public void Start()
    {
        barLine = Resources.Load<GameObject>("BarLine");
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

            foreach (Transform tile in perksTiles.Find("Ground"))
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

        if (currentEquipment.name.StartsWith("BenchPress"))
        {
            StartCoroutine(BenchPress());
        }
        else if (currentEquipment.name.StartsWith("Treadmill") || currentEquipment.name.StartsWith("RunningPad"))
        {
            StartCoroutine(Treadmill());
        }
        else if (currentEquipment.name.StartsWith("PushupPad"))
        {
            StartCoroutine(PushupPad());
        }
    }
    public IEnumerator LeaveEquipment()
    {
        running = false;
        isLeaving = true;
        itemBehavioursScript.DestroyActionBar();
        GetComponent<PlayerAnimation>().enabled = true;

        if (currentEquipment.name.StartsWith("BenchPress"))
        {
            StopCoroutine(BenchPress());
        }
        else if (currentEquipment.name.StartsWith("Treadmill") || currentEquipment.name.StartsWith("RunningPad"))
        {
            StopCoroutine(Treadmill());
            StopCoroutine(TreadmillWalk());
        }
        else if (currentEquipment.name.StartsWith("PushupPad"))
        {
            StopCoroutine(PushupPad());
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
        BodyController bodyController = GetComponent<BodyController>();
        OutfitController outfitController = GetComponent<OutfitController>();

        amountOfBars = 0;

        while (onEquipment && !isLeaving)
        {
            if(hasAdded == false)
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
            
            if(amountOfBars == 49)
            {
                GetComponent<PlayerCollectionData>().playerData.strength++;
                hasAdded = true;
            }

            foreach(Transform barLine in actionBarPanel.transform)
            {
                Destroy(barLine.gameObject);
            }
            for(int i = 0; i < amountOfBars; i++)
            {
                Instantiate(barLine, actionBarPanel.transform);
            }

            if(amountOfBars >= 0 && amountOfBars < 16)
            {
                GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][8][0];
                if(transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][8][0];
                }
            }
            else if(amountOfBars >= 16 && amountOfBars < 32)
            {
                GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][8][1];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][8][1];
                }
            }
            else if(amountOfBars >= 32)
            {
                GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][8][2];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][8][2];
                }
            }

            yield return null;
        }
    }
    //public IEnumerator SpeedBag()
    //{
    //    StopCoroutine(ClimbEquipment());
    //    StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
    //    GetComponent<PlayerAnimation>().enabled = false;
    //    itemBehavioursScript.CreateActionText("asdf");
    //    BodyController bodyController = GetComponent<BodyController>();
    //    OutfitController outfitController = GetComponent<OutfitController>();

    //    amountOfBars = 0;
    //    yield return null;
    //}
    public IEnumerator PushupPad()
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");
        BodyController bodyController = GetComponent<BodyController>();
        OutfitController outfitController = GetComponent<OutfitController>();

        amountOfBars = 0;
        while(onEquipment && !isLeaving)
        {
            if(hasAdded == false)
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

            if (amountOfBars == 49)
            {
                GetComponent<PlayerCollectionData>().playerData.strength++;
                hasAdded = true;
            }

            foreach (Transform barLine in actionBarPanel.transform)
            {
                Destroy(barLine.gameObject);
            }
            for (int i = 0; i < amountOfBars; i++)
            {
                Instantiate(barLine, actionBarPanel.transform);
            }

            if(amountOfBars < 25)
            {
                GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][7][0];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][7][0];
                }
            }
            else if(amountOfBars >= 25)
            {
                GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][7][1];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][7][1];
                }
            }
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
        subDistanceNum = 0;

        running = true;
        StartCoroutine(TreadmillWalk());
        while(onEquipment && !isLeaving)
        {
            GetComponent<PlayerAnimation>().enabled = false;
            if (Input.GetKeyDown(KeyCode.Q) && onQ)
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
            else if (Input.GetKeyDown(KeyCode.E) && onE)
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
                GetComponent<PlayerCollectionData>().playerData.speed++;
            }

            foreach (Transform barLine in actionBarPanel.transform)
            {
                Destroy(barLine.gameObject);
            }
            for (int i = 0; i < amountOfBars; i++)
            {
                Instantiate(barLine, actionBarPanel.transform);
            }


            yield return null;
        }
    }
    public IEnumerator TreadmillWalk()
    {
        BodyController bodyController = GetComponent<BodyController>();
        OutfitController outfitController = GetComponent<OutfitController>();

        while(true)
        {
            GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][2][0];
            if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][2][0];
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
            GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][2][1];
            if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][2][1];
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
            if(onEquipment && (currentEquipment.name.StartsWith("BenchPress") || currentEquipment.name.StartsWith("PushupPad")))
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
            else if (onEquipment && (currentEquipment.name.StartsWith("Treadmill") || currentEquipment.name.StartsWith("RunningPad")))
            {
                Debug.Log("Here");
                if(amountOfBars > 0)
                {
                    amountOfBars--;
                    yield return new WaitForSeconds(.1f);
                }
            }
            yield return null;
        }
    }
}
