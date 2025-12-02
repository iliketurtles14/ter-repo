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
    }
    private void OnDisable()
    {
        isMoving = false;
        isFinishing = false;
        isInCanteen = false;
        isInGym = false;
        seeker.CancelCurrentPathRequest(true);
    }
    private void Update()
    {
        //get period
        period = scheduleScript.periodCode;

        if (!isMoving && !isInCanteen && !isInGym)
        {
            currentWaypoint = null;
            SetCurrentPossibleWaypoints();
            if (!isInCanteen && !isInGym) //isInCanteen and isInGym can get set to true at SetCurrentPossibleWaypoints()
            {
                SetCurrentWaypoint();
                seeker.StartPath(transform.position, currentWaypoint.position);
                isMoving = true;
            }
        }
        else if (isMoving && !isInCanteen && !isInGym)
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
                    case "FP":
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

        yield return new WaitForSeconds(1);

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
        string job = GetComponent<NPCCollectionData>().npcData.job;

        //get cycle for the certain job (only for jobs other than gardening, janitor, library, and mailman as they are more involved)
        List<Vector3> positions = new List<Vector3>();
        int timeBetweenPositions = 0;
        bool hasNormalJob = false;
        switch (job)
        {
            case "Tailorshop":
                bool gotTailorBox = false;
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "TailorBox" && !gotTailorBox)
                    {
                        positions.Add(obj.position);
                        gotTailorBox = true;
                    }
                    else if(obj.name == "ClothesBox" && gotTailorBox)
                    {
                        positions.Add(obj.position);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Laundry":
                bool gotDirtyLaundry = false;
                bool gotWasher = false;
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "DirtyLaundry" && !gotDirtyLaundry && !gotWasher)
                    {
                        positions.Add(obj.position);
                        gotDirtyLaundry = true;
                    }
                    else if(obj.name == "Washer" && gotDirtyLaundry && !gotWasher)
                    {
                        positions.Add(obj.position);
                        gotWasher = true;
                    }
                    else if(obj.name == "CleanLaundry" && gotDirtyLaundry && gotWasher)
                    {
                        positions.Add(obj.position);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Woodshop":
                bool gotTimberBox = false;
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "TimberBox" && !gotTimberBox)
                    {
                        positions.Add(obj.position);
                        gotTimberBox = true;
                    }
                    else if(obj.name == "FurnitureBox" && gotTimberBox)
                    {
                        positions.Add(obj.position);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Deliveries":
                bool gotDeliveryTruck1 = false;
                bool gotRedBox = false;
                bool gotDeliveryTruck2 = false;
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if (obj.name.StartsWith("DeliveryTruck") && !gotDeliveryTruck1 && !gotRedBox && !gotDeliveryTruck2)
                    {
                        positions.Add(obj.position);
                        gotDeliveryTruck1 = true;
                    }
                    else if(obj.name == "RedBox" && gotDeliveryTruck1 && !gotRedBox && !gotDeliveryTruck2)
                    {
                        positions.Add(obj.position);
                        gotRedBox = true;
                    }
                    else if(gotDeliveryTruck1 && gotRedBox && !gotDeliveryTruck2)
                    {
                        positions.Add(positions[0]); //just get the first truck pos
                        gotDeliveryTruck2 = true;
                    }
                    else if(obj.name == "BlueBox" && gotDeliveryTruck1 && gotRedBox && gotDeliveryTruck2)
                    {
                        positions.Add(obj.position);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Kitchen":
                bool gotFreezer = false;
                bool gotOven = false;
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "Freezer" && !gotFreezer && !gotOven)
                    {
                        positions.Add(obj.position);
                        gotFreezer = true;
                    }
                    else if(obj.name == "Oven" && gotFreezer && !gotOven)
                    {
                        positions.Add(obj.position);
                        gotOven = true;
                    }
                    else if(obj.name == "FoodTable" && gotFreezer && gotOven)
                    {
                        positions.Add(obj.position);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
            case "Metalshop":
                bool gotMetalBox = false;
                bool gotLicensePress = false;
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(obj.name == "MetalBox" && !gotMetalBox && !gotLicensePress)
                    {
                        positions.Add(obj.position);
                        gotMetalBox = true;
                    }
                    else if(obj.name == "LicensePress" && gotMetalBox && !gotLicensePress)
                    {
                        positions.Add(obj.position);
                        gotLicensePress = true;
                    }
                    else if(obj.name == "PlatesBox" && gotMetalBox && gotLicensePress)
                    {
                        positions.Add(obj.position);
                        break;
                    }
                }
                timeBetweenPositions = 1;
                hasNormalJob = true;
                break;
        }

        if (hasNormalJob)
        {
            while (true)
            {
                Vector3 currentPos = Vector3.zero;

            }
        }
        
        switch (job)
        {
            case "Janitor":
                while (true)
                {
                    Transform currentSpill = null;
                    foreach(Transform obj in tiles.Find("GroundObjects"))
                    {
                        if(obj.name == "Spill")
                        {
                            currentSpill = obj;
                            break;
                        }
                    }

                    seeker.StartPath(transform.position, currentSpill.position);
                    while (true)
                    {
                        float distance = Vector2.Distance(transform.position, currentSpill.position);
                        if(distance < .8f)
                        {
                            seeker.CancelCurrentPathRequest(true);
                            break;
                        }
                        yield return null;
                    }
                    yield return new WaitForSeconds(2);
                    Destroy(currentSpill.gameObject);

                    if (!isAtJob)
                    {
                        break;
                    }
                }
                break;
            case "Gardening":
                while (true)
                {
                    Transform currentWeed = null;
                    foreach(Transform obj in tiles.Find("GroundObjects"))
                    {
                        if(obj.name == "Weed")
                        {
                            currentWeed = obj;
                            break;
                        }
                    }

                    seeker.StartPath(transform.position, currentWeed.position);
                    while (true)
                    {
                        float distance = Vector2.Distance(transform.position, currentWeed.position);
                        if(distance < .8f)
                        {
                            seeker.CancelCurrentPathRequest(true);
                            break;
                        }
                        yield return null;
                    }
                    yield return new WaitForSeconds(2);
                    Destroy(currentWeed.gameObject);

                    if (!isAtJob)
                    {
                        break;
                    }
                }
                break;
            case "Tailorshop":

        }
    }
}
