using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class SeeBadActions : MonoBehaviour
{
    private List<Vector2> upVectors = new List<Vector2>();
    private List<Vector2> rightVectors = new List<Vector2>();
    private List<Vector2> downVectors = new List<Vector2>();
    private List<Vector2> leftVectors = new List<Vector2>();
    private List<Vector2> currentVectors;
    public float rangeOfSight = 10f;
    private Transform player;
    private Map map;
    private bool canSee = true;
    public List<string> availableBadObjects = new List<string>();
    private MissionAsk missionAskScript;
    private Schedule scheduleScript;
    private SpecialMessages specialMessagesScript;
    private StatEffects statEffectsScript;
    private Solitary solitaryScript;
    private List<Transform> guardsNotSpecial = new List<Transform>();//for calls
    private PauseController pc;
    private Lockdown lockdownScript;
    private UnlockDoors unlockDoorsScript;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        map = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
        missionAskScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("MissionPanel").GetComponent<MissionAsk>();
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        specialMessagesScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("SpecialMessagePanel").GetComponent<SpecialMessages>();
        statEffectsScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<StatEffects>();
        solitaryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Solitary>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        lockdownScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Lockdown>();
        unlockDoorsScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<UnlockDoors>();
        MakeVectorLists();
        MakeBadObjectList();

        StartCoroutine(LookWait());
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        foreach(Transform npc in transform.parent)
        {
            if(npc.name.Contains("Guard") && npc.name != "Guard1" && npc.name != "Guard2" && npc.name != "Guard3")
            {
                guardsNotSpecial.Add(npc);
            }
        }
    }
    private void MakeVectorLists()
    {
        //upVectors
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 162f), Mathf.Sin(Mathf.Deg2Rad * 162f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 144f), Mathf.Sin(Mathf.Deg2Rad * 144f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 126f), Mathf.Sin(Mathf.Deg2Rad * 126f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 108f), Mathf.Sin(Mathf.Deg2Rad * 108f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90f), Mathf.Sin(Mathf.Deg2Rad * 90f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 72f), Mathf.Sin(Mathf.Deg2Rad * 72f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 54f), Mathf.Sin(Mathf.Deg2Rad * 54f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 36f), Mathf.Sin(Mathf.Deg2Rad * 36f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 18f), Mathf.Sin(Mathf.Deg2Rad * 18f)));
        //rightVectors
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 72f), Mathf.Sin(Mathf.Deg2Rad * 72f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 54f), Mathf.Sin(Mathf.Deg2Rad * 54f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 36f), Mathf.Sin(Mathf.Deg2Rad * 36f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 18f), Mathf.Sin(Mathf.Deg2Rad * 18f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 0f), Mathf.Sin(Mathf.Deg2Rad * 0f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 342f), Mathf.Sin(Mathf.Deg2Rad * 342f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 324f), Mathf.Sin(Mathf.Deg2Rad * 324f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 306f), Mathf.Sin(Mathf.Deg2Rad * 306f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 288f), Mathf.Sin(Mathf.Deg2Rad * 288f)));
        //leftVectors
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 108f), Mathf.Sin(Mathf.Deg2Rad * 108f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 126f), Mathf.Sin(Mathf.Deg2Rad * 126f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 144f), Mathf.Sin(Mathf.Deg2Rad * 144f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 162f), Mathf.Sin(Mathf.Deg2Rad * 162f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 180f), Mathf.Sin(Mathf.Deg2Rad * 180f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 198f), Mathf.Sin(Mathf.Deg2Rad * 198f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 216f), Mathf.Sin(Mathf.Deg2Rad * 216f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 234f), Mathf.Sin(Mathf.Deg2Rad * 234f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 252f), Mathf.Sin(Mathf.Deg2Rad * 252f)));
        //downVectors
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 198f), Mathf.Sin(Mathf.Deg2Rad * 198f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 216f), Mathf.Sin(Mathf.Deg2Rad * 216f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 234f), Mathf.Sin(Mathf.Deg2Rad * 234f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 252f), Mathf.Sin(Mathf.Deg2Rad * 252f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 270f), Mathf.Sin(Mathf.Deg2Rad * 270f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 288f), Mathf.Sin(Mathf.Deg2Rad * 288f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 306f), Mathf.Sin(Mathf.Deg2Rad * 306f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 324f), Mathf.Sin(Mathf.Deg2Rad * 324f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 342f), Mathf.Sin(Mathf.Deg2Rad * 342f)));
    }
    private void MakeBadObjectList()
    {
        availableBadObjects.Add("onDesk");
        availableBadObjects.Add("guardSearchingDesk");
        availableBadObjects.Add("inmateSearchingDesk");
        availableBadObjects.Add("pickedUp");
        availableBadObjects.Add("inWrongCell");
        availableBadObjects.Add("playerPunching");
        availableBadObjects.Add("wrongRoutine");
        availableBadObjects.Add("notAtWork");
        availableBadObjects.Add("noOutfit");
        availableBadObjects.Add("guardNonInmateOutfit");
        availableBadObjects.Add("inmateNonInmateOutfit");
        availableBadObjects.Add("highHeat");
        availableBadObjects.Add("inmateBreakingTile");
        availableBadObjects.Add("guardBreakingTile");
        availableBadObjects.Add("item");
        availableBadObjects.Add("outsideOnInsideMap");
        availableBadObjects.Add("npcLoot");
        availableBadObjects.Add("noDummy");
        availableBadObjects.Add("missedRollcall");
        availableBadObjects.Add("outLate");
    }
    private IEnumerator LookWait()
    {
        float rand = UnityEngine.Random.Range(0f, .1f);
        yield return new WaitForSeconds(rand);
        StartCoroutine(Look());
    }
    private IEnumerator Look()
    {
        while (true)
        {
            if (pc.isPaused)
            {
                yield return null;
                continue;
            }
            
            switch (GetComponent<NPCAnimation>().lookDir)
            {
                case "up":
                    currentVectors = upVectors;
                    break;
                case "right":
                    currentVectors = rightVectors;
                    break;
                case "left":
                    currentVectors = leftVectors;
                    break;
                case "down":
                    currentVectors = downVectors;
                    break;
                case "any":
                    currentVectors = upVectors;
                    break;
                default:
                    currentVectors = upVectors;
                    break;
            }

            foreach (Vector2 vector in currentVectors)
            {
                RaycastHit2D[] wallHits = Physics2D.RaycastAll(transform.position, vector, rangeOfSight, LayerMask.GetMask("Ground"));

                RaycastHit2D? wallHit = null;
                foreach (RaycastHit2D aHit in wallHits)
                {
                    if (aHit.collider.CompareTag("Wall") || aHit.collider.CompareTag("Obstacle"))
                    {
                        wallHit = aHit;
                        break;
                    }
                }

                if (wallHit.HasValue)
                {
                    Debug.DrawLine(transform.position, wallHit.Value.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
                }

                RaycastHit2D[] badHits;
                try
                {
                    badHits = Physics2D.RaycastAll(transform.position, wallHit.Value.point, wallHit.Value.distance, LayerMask.GetMask("Ground"));

                }
                catch
                {
                    badHits = Physics2D.RaycastAll(transform.position, vector, rangeOfSight, LayerMask.GetMask("Ground"));
                }

                RaycastHit2D? badHit = null;
                foreach (RaycastHit2D aHit in badHits)
                {
                    if (aHit.collider.CompareTag("BadObject"))
                    {
                        Debug.Log(aHit.collider.name);
                    }
                }
                foreach (RaycastHit2D aHit in badHits)
                {
                    if (aHit.collider.CompareTag("BadObject"))
                    {
                        if (aHit.collider.name == "missedRollcall")//take priority
                        {
                            badHit = aHit;
                            SeeBadAction(aHit.transform.gameObject);
                        }

                        if (availableBadObjects.Contains(aHit.collider.name) && !GetComponent<NPCCombat>().isAggro && aHit.collider.name != "playerPunching")
                        {
                            badHit = aHit;
                            SeeBadAction(aHit.transform.gameObject);
                        }
                        else if (availableBadObjects.Contains(aHit.collider.name) && aHit.collider.name == "playerPunching")
                        {
                            badHit = aHit;
                            SeeBadAction(aHit.transform.gameObject);
                        }
                    }
                }

                if (badHit.HasValue)
                {
                    Debug.DrawLine(transform.position, badHit.Value.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    public void SeeBadAction(GameObject badObject)
    {        
        if(badObject.name != "highHeat") //add things here that shouldnt have a cooldown
        {
            availableBadObjects.Remove(badObject.name);
        }
        
        BadObjectData data = badObject.GetComponent<BadObjectData>();
        bool isGuard = GetComponent<NPCCollectionData>().npcData.isGuard;

        if(data.attachedObject == player.gameObject && player.GetComponent<PlayerCollectionData>().playerData.isDead)
        {
            return;
        }

        if(data.forInmate && isGuard)
        {
            return;
        }
        if(!data.forInmate && !isGuard)
        {
            return;
        }

        if (badObject.name == "missedRollcall")
        {
            lockdownScript.StopLockdown();
            return;
        }

        //add heat
        int heatToAdd = 0;
        if (data.isMultiplied)
        {
            heatToAdd = data.heatGain * map.npcLevel;
        }
        else if(!data.isMultiplied)
        {
            heatToAdd = data.heatGain;
        }
        if(heatToAdd > 0)
        {
            player.GetComponent<PlayerCollectionData>().playerData.heat += heatToAdd;
            if(badObject.name != "guardNonInmateOutfit" && badObject.name != "inmateNonInmateOutfit")
            {
                StartCoroutine(statEffectsScript.MakeEffect(transform, "heat"));
            }
        }

        //set heat
        if (data.heatSet != -1)
        {
            player.GetComponent<PlayerCollectionData>().playerData.heat = data.heatSet;
            StartCoroutine(statEffectsScript.MakeEffect(transform, "heat"));
        }

        //should aggro
        if (data.shouldAggro) //make this also work for searching inmate desks when the same inmate sees it
        {
            GetComponent<NPCCombat>().isAggro = true;
            GetComponent<NPCCombat>().target = data.attachedObject;

            DistractionFavor();
        }

        //solitary
        if (data.solitary)
        {
            StartCoroutine(solitaryScript.GoToSolitary(""));
        }

        //item
        if (data.item)
        {
            StartCoroutine(GoToItem(badObject));
        }

        //toilet
        if (data.toilet)
        {
            Debug.Log("toilet");
        }

        //untie
        if (data.untie)
        {
            Debug.Log("untie");
        }

        //sheets
        if (data.sheets)
        {
            Debug.Log("sheets");
        }

        //message type
        if(data.messageType != null &&
            (isGuard && !data.forInmate ||
            !isGuard && data.forInmate))
        {
            NPCSpeech speechScript = GetComponent<NPCSpeech>();
            if(data.messageType != "I saw that")
            {
                StartCoroutine(speechScript.MakeTextBox(speechScript.GetMessage(data.messageType), transform, true));
            }
            else
            {
                string msg = "I saw that, " + player.GetComponent<NPCCollectionData>().npcData.displayName.Replace("\n", "").Replace("\r", "") + "!";
                StartCoroutine(speechScript.MakeTextBox(msg, transform, true));
            }
        }

        //should call
        if (data.shouldCall)
        {
            if(guardsNotSpecial.Count > 0)
            {
                List<Transform> availableGuards = new List<Transform>();
                foreach(Transform guard in guardsNotSpecial)
                {
                    if(!guard.GetComponent<NPCCollectionData>().npcData.isDead &&
                        !guard.GetComponent<NPCCombat>().isAggro &&
                        !guard.GetComponent<NPCCollectionData>().npcData.isSleeping)
                    {
                        availableGuards.Add(guard);
                    }
                }

                if(availableGuards.Count > 0)
                {
                    int rand = UnityEngine.Random.Range(0, availableGuards.Count);
                    availableGuards[rand].GetComponent<NPCAI>().SendToPos(transform.position);
                }
            }
            player.GetComponent<PlayerCollectionData>().playerData.heat += 30;
        }

        //do timers
        if (badObject.name != "guardNonInmateOutfit" && badObject.name != "inmateNonInmateOutfit")
        {
            StartCoroutine(SeeBadCooldown(5, badObject.name));
        }
        else if (badObject.name == "guardNonInmateOutfit" || badObject.name == "inmateNonInmateOutfit")
        {
            StartCoroutine(SeeBadCooldown(2, badObject.name));
        }
    }
    private void DistractionFavor()
    {
        List<Mission> distractionMissions = new List<Mission>();
        foreach(Mission mission in missionAskScript.savedMissions)
        {
            if(mission.type == "distract")
            {
                distractionMissions.Add(mission);
            }
        }

        Debug.Log(distractionMissions.Count);

        if(distractionMissions.Count == 0)
        {
            return;
        }

        string currentPeriod = scheduleScript.period;
        foreach(Mission mission in distractionMissions)
        {
            Debug.Log(currentPeriod + ", " + mission.period);
            if(mission.period == currentPeriod)
            {
                StartCoroutine(specialMessagesScript.MakeMessage("You completed a Favor!\n+$" + mission.pay, "favor"));
                player.GetComponent<PlayerCollectionData>().playerData.money += mission.pay;
                missionAskScript.savedMissions.Remove(mission);
            }
        }
    }
    private IEnumerator GoToItem(GameObject badObject)
    {
        GetComponent<NPCAI>().SendToPos(badObject.transform.position);
        while (true)
        {
            if(badObject == null)
            {
                yield break;
            }
            if(Vector2.Distance(transform.position, badObject.transform.position) < .4f)
            {
                break;
            }
            if (!GetComponent<NPCAI>().enabled)
            {
                yield break;
            }
            yield return null;
        }
        ItemData itemData = badObject.GetComponent<BadObjectData>().attachedObject.GetComponent<ItemCollectionData>().itemData;
        if (itemData.causeSolitary)
        {
            StartCoroutine(solitaryScript.GoToSolitary(""));
            yield break;
        }
        Destroy(badObject.GetComponent<BadObjectData>().attachedObject);
        Destroy(badObject);
    }

    private IEnumerator SeeBadCooldown(float time, string name)
    {
        yield return new WaitForSeconds(time);
        availableBadObjects.Add(name);
    }
}
