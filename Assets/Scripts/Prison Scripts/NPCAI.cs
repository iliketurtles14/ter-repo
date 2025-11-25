using NUnit.Framework;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool isFinishing;
    private bool isInCanteen;
    private bool isInGym;
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
            float distance = Vector2.Distance(transform.position, currentWaypoint.position);
            if(distance < .01f && !isFinishing)
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
        if(period != "E")
        {
            isInGym = false;
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
                    case "W":
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
                        if (npcType == "Inmate")
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
                        if (npcType == "Inmate")
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
        switch (currentEquipment.name)
        {
            case "Benchpress":
                offset = new Vector3(0, 0, 0);
                transform.position += offset;

                while (true)
                {
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][0];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][0];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][1];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][1];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][2];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][2];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][8][1];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][8][1];
                    }

                    yield return new WaitForSeconds(.266f);

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
                transform.position += offset;

                while (true)
                {
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][0];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][0];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][1];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                    }
                    yield return new WaitForSeconds(.266f);

                    if (!isInGym)
                    {
                        break;
                    }

                    yield return null;
                }
                break;
            case "PushupMat":
                offset = new Vector3(0, 0, 0);
                transform.position += offset;

                while (true)
                {
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][7][0];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][7][0];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][7][1];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][7][1];
                    }
                    yield return new WaitForSeconds(.266f);

                    if (!isInGym)
                    {
                        break;
                    }

                    yield return null;
                }
                break;
            case "SpeedBag":
                offset = new Vector3(-.4f, .4f);
                transform.position += offset;

                while (true)
                {
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][0];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][0];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][1];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                    }
                    yield return new WaitForSeconds(.266f);

                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][3][0];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                    }
                    currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[240];
                    yield return new WaitForSeconds(.117f);
                    currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[241];
                    yield return new WaitForSeconds(.117f);
                    currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[239];
                    yield return new WaitForSeconds(.117f);
                    currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[258];
                    yield return new WaitForSeconds(.116f);

                    if (!isInGym)
                    {
                        break;
                    }

                    yield return null;
                }
                break;
            case "PunchingMat":
                offset = new Vector3(-.6f, .6f);
                transform.position += offset;

                while (true)
                {
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][0];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][0];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][2][1];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                    }
                    yield return new WaitForSeconds(.266f);

                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][3][0];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][1];
                    }
                    currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[235];
                    yield return new WaitForSeconds(.15f);
                    currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[237];
                    yield return new WaitForSeconds(.15f);
                    currentEquipment.transform.Find("Bag").GetComponent<SpriteRenderer>().sprite = applyPrisonDataScript.PrisonObjectSprites[234];
                    yield return new WaitForSeconds(.167f);

                    if (!isInGym)
                    {
                        break;
                    }

                    yield return null;
                }
                break;
            case "JumpropeMat":
                offset = new Vector3(0, .2f);
                transform.position += offset;

                int i = 0;
                while (true)
                {
                    if(i == 8)
                    {
                        i = 0;
                    }

                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][9][i];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][2][i];
                    }
                    yield return new WaitForSeconds(.266f);
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
                transform.position += offset;

                while (true)
                {
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][0];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][0];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][1];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][1];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][2];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][2];
                    }
                    yield return new WaitForSeconds(.266f);
                    GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][10][1];
                    if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
                    {
                        transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][10][1];
                    }
                    yield return new WaitForSeconds(.266f);

                    if (!isInGym)
                    {
                        break;
                    }

                    yield return null;
                }
                break;
        }
        GetComponent<NPCAnimation>().enabled = true;

        isMoving = false;
        isFinishing = false;
    }
}
