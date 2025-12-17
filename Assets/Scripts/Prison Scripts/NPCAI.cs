using NUnit.Framework;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NPCAI : MonoBehaviour
{
    public string npcType;
    public int npcNum;
    public bool isImportantGuard;
    private Seeker seeker;
    public string period;
    private Schedule scheduleScript;
    private Transform tiles;
    public List<Transform> currentPossibleWaypoints;
    public bool isMoving;
    public Transform currentWaypoint;
    public bool isFreeWalking;
    public bool isFinishing;
    public bool isInCanteen;
    public bool isInGym;
    public bool shouldReset;
    public bool isAtJob;
    private ApplyPrisonData applyPrisonDataScript;
    private List<Transform> positions = new List<Transform>(); //job positions (didnt change the name)
    private int timeBetweenPositions;
    private bool hasNormalJob;
    private string job;
    private List<Transform> deskPositions = new List<Transform>();
    private WeedController weedScript;
    private void Start()
    {   
        //get npctype and num
        if (name.StartsWith("Inmate"))
        {
            npcType = "Inmate";

            string[] parts = name.Split('e');
            npcNum = Convert.ToInt32(parts[1]);
        }
        else if (name.StartsWith("Guard"))
        {
            npcType = "Guard";

            string[] parts = name.Split('d');
            npcNum = Convert.ToInt32(parts[1]);
        }

        if(npcType == "Guard" && npcNum < 4)
        {
            isImportantGuard = true;
        }

        seeker = GetComponent<Seeker>();
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        applyPrisonDataScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        weedScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<WeedController>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        LoadNormalJobPositions();
        LoadDeskPositions();
    }
    private void LoadNormalJobPositions()
    {
        job = GetComponent<NPCCollectionData>().npcData.job;

        //get cycle for the certain job (only for jobs other than gardening, janitor, library, and mailman as they are more involved)
        switch (job)
        {
            case "Tailorshop":
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "TailorBox")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "ClothesBox")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Laundry":
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "DirtyLaundry")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "Washer")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "CleanLaundry")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Woodshop":
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "TimberBox")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "FurnitureBox")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Deliveries":
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name.StartsWith("DeliveryTruck"))
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "RedBox")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "BlueBox")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Kitchen":
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "Freezer")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "Oven")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach (Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name == "FoodTable")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Metalshop":
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "MetalBox")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "LicensePress")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "PlatesBox")
                    {
                        positions.Add(obj);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
        }
    }
    private void LoadDeskPositions()
    {
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "NPCDesk" || obj.name == "PlayerDesk")
            {
                deskPositions.Add(obj);
            }
        }
    }
    private void OnDisable()
    {
        isMoving = false;
        isFinishing = false;
        isInCanteen = false;
        isInGym = false;
        isAtJob = false;
        StopAllCoroutines();
        seeker.CancelCurrentPathRequest(true);
    }
    private void Update()
    {
        //get period
        period = scheduleScript.periodCode;

        if (!isMoving && !isInCanteen && !isInGym && !isAtJob)
        {
            currentWaypoint = null;
            SetCurrentPossibleWaypoints();
            if (!isInCanteen && !isInGym && !isAtJob) //isInCanteen and isInGym can get set to true at SetCurrentPossibleWaypoints()
            {
                SetCurrentWaypoint();
                seeker.StartPath(transform.position, currentWaypoint.position);
                isMoving = true;
            }
        }
        else if (isMoving && !isInCanteen && !isInGym && !isAtJob)
        {
            try
            {
                float distance = Vector2.Distance(transform.position, currentWaypoint.position);
                if (distance < .01f && !isFinishing)
                {
                    isFinishing = true;
                    StartCoroutine(FinishMovement());
                }
            }
            catch //force finish
            {
                isFinishing = true;
                StartCoroutine(FinishMovement());
            }
        }

        if (period != "L" && period != "B" && period != "D")
        {
            isInCanteen = false;
            GetComponent<NPCCollectionData>().npcData.hasFood = false;
        }
        if (period != "E")
        {
            isInGym = false;
        }
        if(period != "W")
        {
            isAtJob = false;
        }
    }
    private void SetCurrentPossibleWaypoints()
    {
        currentPossibleWaypoints = new List<Transform>();

        foreach (Transform waypoint in tiles.Find("GroundObjects"))
        {
            if (waypoint.CompareTag("Waypoint"))
            {
                switch (period)
                {
                    case "LO":
                    case "FT":
                        if (npcType == "Inmate" && waypoint.name == "InmateWaypoint")
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint")
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "W":
                        if(npcType == "Inmate" && !string.IsNullOrEmpty(GetComponent<NPCCollectionData>().npcData.job) && !isAtJob)
                        {
                            isFreeWalking = false;
                            isAtJob = true;
                            StartCoroutine(InmateJobs());
                        }
                        else if(npcType == "Inmate" && string.IsNullOrEmpty(GetComponent<NPCCollectionData>().npcData.job) && waypoint.name == "InmateWaypoint")
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if(npcType == "Guard" && waypoint.name == "GuardWaypoint")
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "R":
                        if (npcType == "Inmate" && waypoint.name == "InmateRollcall")
                        {
                            isFreeWalking = false;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardRollcall" && isImportantGuard)
                        {

                            isFreeWalking = false;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint" && !isImportantGuard)
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "B":
                    case "L":
                    case "D":
                        if (npcType == "Inmate" && !isInCanteen)
                        {
                            isFreeWalking = false;
                            isInCanteen = true;
                            StartCoroutine(InmateCanteen());
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardCanteen" && isImportantGuard)
                        {
                            isFreeWalking = false;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint" && !isImportantGuard)
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "E":
                        if (npcType == "Inmate" && !isInGym)
                        {
                            isFreeWalking = false;
                            isInGym = true;
                            StartCoroutine(InmateExercise());
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardGym" && isImportantGuard)
                        {
                            isFreeWalking = false;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint" && !isImportantGuard)
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                    case "S":
                        if (npcType == "Inmate" && waypoint.name == "InmateShower")
                        {
                            isFreeWalking = false;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardShower" && isImportantGuard)
                        {
                            isFreeWalking = false;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        else if (npcType == "Guard" && waypoint.name == "GuardWaypoint" && !isImportantGuard)
                        {
                            isFreeWalking = true;
                            currentPossibleWaypoints.Add(waypoint);
                        }
                        break;
                }
            }
        }
    }
    private void SetCurrentWaypoint()
    {
        seeker.CancelCurrentPathRequest(true);
        
        if (isFreeWalking)
        {
            int rand = UnityEngine.Random.Range(0, currentPossibleWaypoints.Count);
            currentWaypoint = currentPossibleWaypoints[rand];
        }
        else
        {
            currentWaypoint = currentPossibleWaypoints[npcNum - 1];
        }
    }
    private IEnumerator FinishMovement()
    {
        seeker.CancelCurrentPathRequest(true);
        int rand = UnityEngine.Random.Range(0, 4);
        yield return new WaitForSeconds(rand);
        isMoving = false;
        isFinishing = false;
    }
    private IEnumerator InmateCanteen()
    {
        Debug.Log("Is in canteen");
        
        //get canteen postiions
        List<Vector3> canteenPositions = new List<Vector3>();
        Vector3 currentCanteenWaypointPos = Vector3.zero;
        foreach(Transform waypoint in tiles.Find("GroundObjects"))
        {
            if(waypoint.name == "InmateCanteen")
            {
                canteenPositions.Add(waypoint.position);
                canteenPositions.Add(waypoint.position + new Vector3(-1.6f, 0, 0));
                canteenPositions.Add(waypoint.position + new Vector3(1.6f, 0, 0));
            }
        }

        //randomly choose one to go to
        int rand = UnityEngine.Random.Range(0, canteenPositions.Count);
        currentCanteenWaypointPos = canteenPositions[rand];

        //go to that pos
        seeker.StartPath(transform.position, currentCanteenWaypointPos);

        //wait until it gets there
        while (true)
        {
            float distance = Vector2.Distance(transform.position, currentCanteenWaypointPos);
            if(distance < .01f)
            {
                break;
            }
            yield return null;
        }
        seeker.CancelCurrentPathRequest(true);

        float timer = 0f;
        while(timer < 1f)
        {
            if (!isInCanteen)
            {
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        //grab food
        GetComponent<NPCCollectionData>().npcData.hasFood = true;
        
        List<float> distances = new List<float>();
        foreach(Transform foodTable in tiles.Find("GroundObjects"))
        {
            if(foodTable.name == "FoodTable")
            {
                distances.Add(Vector2.Distance(transform.position, foodTable.position));
            }
        }
        float min = distances.Min();
        foreach(Transform foodTable in tiles.Find("GroundObjects"))
        {
            if(foodTable.name == "FoodTable")
            {
                float aDistance = Vector2.Distance(transform.position, foodTable.position);
                if(aDistance == min)
                {
                    foodTable.GetComponent<FoodTableCounter>().foodCount--;
                    break;
                }
            }
        }

        //go to the corresponding seat
        Vector3 currentCanteenSeatPos = Vector3.zero;
        foreach(Transform seat in tiles.Find("GroundObjects"))
        {
            if(seat.name == "Seat")
            {
                if(seat.GetComponent<SeatNumber>().seatNumber == npcNum)
                {
                    currentCanteenSeatPos = seat.position;
                }
            }
        }
        seeker.StartPath(transform.position, currentCanteenSeatPos);

        //wait until it gets there
        while (true)
        {
            float distance = Vector2.Distance(transform.position, currentCanteenSeatPos);
            if(distance < .01f)
            {
                break;
            }
            yield return null;
        }
        seeker.CancelCurrentPathRequest(true);
        transform.position = currentCanteenSeatPos;

        isMoving = false;
        isFinishing = false;
    }
    private IEnumerator InmateExercise()
    {
        GameObject currentEquipment = null;
        
        //get equipment positions
        Vector3 currentEquipmentPos = Vector3.zero;
        foreach(Transform equipment in tiles.Find("GroundObjects"))
        {
            if (equipment.gameObject.CompareTag("Equipment"))
            {
                if(equipment.GetComponent<EquipmentNumber>().equipmentNumber == npcNum)
                {
                    currentEquipment = equipment.gameObject;
                    currentEquipmentPos = equipment.position;
                }
            }
        }
        seeker.StartPath(transform.position, currentEquipmentPos);

        //wait until it gets there
        while (true)
        {
            float distance = Vector2.Distance(transform.position, currentEquipmentPos);
            if(distance < .01f)
            {
                break;
            }
            yield return null;
        }
        seeker.CancelCurrentPathRequest(true);
        transform.position = currentEquipmentPos;

        //set animation
        Vector3 offset;
        BodyController bc = GetComponent<BodyController>();
        OutfitController oc = GetComponent<OutfitController>();
        GetComponent<NPCAnimation>().enabled = false;
        GetComponent<AILerp>().canMove = false;
        try
        {
            switch (currentEquipment.name)
            {
                case "Benchpress":
                    offset = new Vector3(0, 0, 0);
                    transform.position = offset + currentEquipmentPos;

                    while (true)
                    {
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][0];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][0];
                        }
                        float timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][1];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][1];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][2];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][2];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][1];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][1];
                        }

                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        if (!isInGym)
                        {
                            break;
                        }

                        yield return null;
                    }
                    break;
                case "Treadmill":
                case "RunningMat":
                    offset = new Vector3(0, .4f);
                    transform.position = offset + currentEquipmentPos;

                    while (true)
                    {
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][0];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][0];
                        }
                        float timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][1];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        if (!isInGym)
                        {
                            break;
                        }

                        yield return null;
                    }
                    break;
                case "PushupMat":
                    offset = new Vector3(0, 0, 0);
                    transform.position = offset + currentEquipmentPos;

                    while (true)
                    {
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][7][0];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][7][0];
                        }
                        float timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][7][1];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][7][1];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        if (!isInGym)
                        {
                            break;
                        }

                        yield return null;
                    }
                    break;
                case "SpeedBag":
                    offset = new Vector3(-.4f, .4f);
                    transform.position = offset + currentEquipmentPos;

                    while (true)
                    {
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][0];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][0];
                        }
                        float timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][1];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][3][0];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                        }
                        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[240];
                        timer = 0f;
                        while (timer < .117f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[241];
                        timer = 0f;
                        while (timer < .117f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[239];
                        timer = 0f;
                        while (timer < .117f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[258];
                        timer = 0f;
                        while (timer < .116f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        if (!isInGym)
                        {
                            break;
                        }

                        yield return null;
                    }
                    break;
                case "PunchingMat":
                    offset = new Vector3(-.6f, .6f);
                    transform.position = offset + currentEquipmentPos;

                    while (true)
                    {
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][0];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][0];
                        }
                        float timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][1];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][3][0];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                        }
                        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[235];
                        timer = 0f;
                        while (timer < .15f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[237];
                        timer = 0f;
                        while (timer < .15f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[234];
                        timer = 0f;
                        while (timer < .15f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        if (!isInGym)
                        {
                            break;
                        }

                        yield return null;
                    }
                    break;
                case "JumpropeMat":
                    offset = new Vector3(0, .2f);
                    transform.position = offset + currentEquipmentPos;

                    int i = 0;
                    while (true)
                    {
                        if (i == 8)
                        {
                            i = 0;
                        }

                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][9][i];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][i];
                        }
                        float timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        i++;

                        if (!isInGym)
                        {
                            break;
                        }

                        yield return null;
                    }
                    break;
                case "PullupBar":
                    offset = new Vector3(0, .1f);
                    transform.position = offset + currentEquipmentPos;

                    while (true)
                    {
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][0];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][0];
                        }
                        float timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][1];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][1];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][2];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][2];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }
                        GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][1];
                        if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                        {
                            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][1];
                        }
                        timer = 0f;
                        while (timer < .266f)
                        {
                            if (!isInGym)
                            {
                                yield break;
                            }
                            timer += Time.deltaTime;
                            yield return null;
                        }

                        if (!isInGym)
                        {
                            break;
                        }

                        yield return null;
                    }
                    break;
            }
        }
        finally
        {
            isInGym = false;
            isMoving = true;
            isFinishing = false;
            GetComponent<NPCAnimation>().enabled = true;
            GetComponent<AILerp>().canMove = true;
            foreach(Transform equipment in tiles.Find("GroundObjects")) //reset bags
            {
                if(equipment.name == "PunchingMat")
                {
                    equipment.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[234];
                }
                else if(equipment.name == "SpeedBag")
                {
                    equipment.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[258];
                }
            }
        }
    }
    private IEnumerator InmateJobs()
    {
        Debug.Log("In Job");
        BoxCollider2D outBoxCollider = transform.Find("OutBox").GetComponent<BoxCollider2D>();
        if (hasNormalJob)
        {
            Debug.Log(positions.Count);
            int i = 0;
            while (true)
            {
                if(i == positions.Count)
                {
                    i = 0;
                }

                Transform currentObj = positions[i];
                if(currentObj != null)
                {
                    BoxCollider2D currentObjCollider = currentObj.GetComponent<BoxCollider2D>();

                    seeker.StartPath(transform.position, currentObj.position);
                    while (true)
                    {
                        if (outBoxCollider.IsTouching(currentObjCollider))
                        {
                            seeker.CancelCurrentPathRequest(true);
                            break;
                        }
                        yield return null;
                    }

                    float time = 0f;
                    while(time < timeBetweenPositions)
                    {
                        if (!isAtJob)
                        {
                            yield break;
                        }
                        time += Time.deltaTime;
                        yield return null;
                    }
                }

                if (!isAtJob)
                {
                    break;
                }
                i++;
                yield return null;
            }
        }
        else
        {
            switch (job)
            {
                case "Janitor":
                    while (true)
                    {
                        Transform currentSpill = null;
                        foreach (Transform obj in tiles.Find("GroundObjects"))
                        {
                            if (obj.name == "Spill")
                            {
                                currentSpill = obj;
                                break;
                            }
                        }

                        if(currentSpill != null)
                        {
                            seeker.StartPath(transform.position, currentSpill.position);
                            while (true)
                            {
                                float distance = Vector2.Distance(transform.position, currentSpill.position);
                                if (distance < .8f)
                                {
                                    seeker.CancelCurrentPathRequest(true);
                                    break;
                                }
                                yield return null;
                            }
                            float time = 0f;
                            while (time < 2f)
                            {
                                if (!isAtJob)
                                {
                                    yield break;
                                }
                                time += Time.deltaTime;
                                yield return null;
                            }
                            Destroy(currentSpill.gameObject);
                            weedScript.spillAmount--;
                        }

                        if (!isAtJob)
                        {
                            break;
                        }
                        yield return null;
                    }
                    break;
                case "Gardening":
                    while (true)
                    {
                        Transform currentWeed = null;
                        foreach (Transform obj in tiles.Find("GroundObjects"))
                        {
                            if (obj.name == "Weed")
                            {
                                currentWeed = obj;
                                break;
                            }
                        }

                        if(currentWeed != null)
                        {
                            seeker.StartPath(transform.position, currentWeed.position);
                            while (true)
                            {
                                float distance = Vector2.Distance(transform.position, currentWeed.position);
                                if (distance < .8f)
                                {
                                    seeker.CancelCurrentPathRequest(true);
                                    break;
                                }
                                yield return null;
                            }
                            float time = 0f;
                            while (time < 2f)
                            {
                                if (!isAtJob)
                                {
                                    yield break;
                                }
                                time += Time.deltaTime;
                                yield return null;
                            }
                            Destroy(currentWeed.gameObject);
                            weedScript.weedAmount--;
                        }

                        if (!isAtJob)
                        {
                            break;
                        }
                        yield return null;
                    }
                    break;
                case "Library":
                    Transform bookBoxPos = null;
                    foreach(Transform obj in tiles.Find("GroundObjects"))
                    {
                        if(obj.name == "BookBox")
                        {
                            bookBoxPos = obj;
                            break;
                        }
                    }

                    BoxCollider2D bookBoxCollider = bookBoxPos.GetComponent<BoxCollider2D>();
                    while (true)
                    {
                        Transform currentDesk = null;

                        //get random desk pos
                        int rand = UnityEngine.Random.Range(0, deskPositions.Count);
                        currentDesk = deskPositions[rand];

                        BoxCollider2D currentDeskCollider = currentDesk.GetComponent<BoxCollider2D>();

                        //go to the bookBox first (use outBox for seeing when the npc gets to it)
                        seeker.StartPath(transform.position, bookBoxPos.position);
                        while (true)
                        {
                            if (outBoxCollider.IsTouching(bookBoxCollider))
                            {
                                seeker.CancelCurrentPathRequest(true);
                                break;
                            }
                            yield return null;
                        }

                        float time = 0f;
                        while (time < 1f)
                        {
                            if (!isAtJob)
                            {
                                yield break;
                            }
                            time += Time.deltaTime;
                            yield return null;
                        }

                        //go to the desk
                        seeker.StartPath(transform.position, currentDesk.position);
                        while (true)
                        {
                            if (outBoxCollider.IsTouching(currentDeskCollider))
                            {
                                seeker.CancelCurrentPathRequest(true);
                                break;
                            }
                            yield return null;
                        }

                        time = 0f;
                        while (time < 2f)
                        {
                            if (!isAtJob)
                            {
                                yield break;
                            }
                            time += Time.deltaTime;
                            yield return null;
                        }

                        if (!isAtJob)
                        {
                            break;
                        }
                    }
                    break;
                case "Mailman":
                    Transform mailBoxPos = null;
                    foreach(Transform obj in tiles.Find("GroundObjects"))
                    {
                        if(obj.name == "MailBox")
                        {
                            mailBoxPos = obj;
                            break;
                        }
                    }

                    BoxCollider2D mailBoxCollider = mailBoxPos.GetComponent<BoxCollider2D>();
                    while (true)
                    {
                        Transform currentDesk = null;

                        //get random desk pos
                        int rand = UnityEngine.Random.Range(0, deskPositions.Count);
                        currentDesk = deskPositions[rand];

                        BoxCollider2D currentDeskCollider = currentDesk.GetComponent<BoxCollider2D>();

                        //go to the mailbox
                        seeker.StartPath(transform.position, mailBoxPos.position);
                        while (true)
                        {
                            if (outBoxCollider.IsTouching(mailBoxCollider))
                            {
                                seeker.CancelCurrentPathRequest(true);
                                break;
                            }
                            yield return null;
                        }

                        float time = 0f;
                        while (time < 1f)
                        {
                            if (!isAtJob)
                            {
                                yield break;
                            }
                            time += Time.deltaTime;
                            yield return null;
                        }

                        //go to the desk
                        seeker.StartPath(transform.position, currentDesk.position);
                        while (true)
                        {
                            if (outBoxCollider.IsTouching(currentDeskCollider))
                            {
                                seeker.CancelCurrentPathRequest(true);
                                break;
                            }
                            yield return null;
                        }

                        time = 0f;
                        while (time < 2f)
                        {
                            if (!isAtJob)
                            {
                                yield break;
                            }
                            time += Time.deltaTime;
                            yield return null;
                        }

                        if (!isAtJob)
                        {
                            break;
                        }
                    }
                    break;
            }
        }
    }
}
