using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NPCSpeechController : MonoBehaviour
{
    //rollcall: 150 (guards)
    //canteen: rand(175) + 80 and inmates have food and are sitting
    //shower: rand(150) + 50
    //gym: rand(150) + 50
    //lockdown: rand(150) + 200
    //freetime: check each 50/45 seconds if rand(5) + 1 = 1 and that the npc is on screen
    private Schedule scheduleScript;
    private int randTime;
    private int offsetTime;
    private PauseController pc;
    private int rcPhase;
    Dictionary<string, List<int>> periodTimeDict = new Dictionary<string, List<int>>
    {
        { "L", new List<int>{ 175, 80 } },
        { "D", new List<int>{ 175, 80 } },
        { "B", new List<int>{ 175, 80 } },
        { "S", new List<int>{ 150, 50 } },
        { "E", new List<int>{ 150, 50 } },
        { "LD", new List<int>{ 150, 200 } }
    };
    private List<string> normalPeriodCodes = new List<string>
    {
        "L", "D", "B", "S", "E", "LD"
    };
    private Dictionary<string, string> periodSpeechDict = new Dictionary<string, string>
    {
        { "S", "Shower" }, { "E", "Gym" }, { "B", "Canteen" }, { "L", "Canteen" }, { "D", "Canteen" },
        { "FT", "Banter" }, { "LD", "Lockdown" }, { "W", "Banter" }
    };
    private List<string> inmateNames = new List<string>();
    private Transform aStar;
    private Map currentMap;
    private List<string> rollcallSpeechTypes = new List<string>();
    private Transform player;
    private Transform tiles;
    private Solitary solitaryScript;

    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        pc = GetComponent<PauseController>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        player = RootObjectCache.GetRoot("Player").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        solitaryScript = GetComponent<Solitary>();
        StartCoroutine(StartWait());
    }
    private void Update()
    {
        if(scheduleScript.periodCode != "R")
        {
            rcPhase = 0;
        }
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = GetComponent<LoadPrison>().currentMap;
        switch (currentMap.npcLevel)//name and name
        {
            case 1:
                rollcallSpeechTypes.Add("Rollcall_Commence_MinSec");
                rollcallSpeechTypes.Add("Shakedowns_MinSec");
                rollcallSpeechTypes.Add("");
                rollcallSpeechTypes.Add("Rollcall_Banter_MinSec");
                break;
            case 2:
            case 3:
                rollcallSpeechTypes.Add("Rollcall_Commence");
                rollcallSpeechTypes.Add("Shakedowns");
                rollcallSpeechTypes.Add("");
                rollcallSpeechTypes.Add("Rollcall_Banter");
                break;
        }
        foreach(Transform npc in aStar)
        {
            if (npc.name.Contains("Inmate"))
            {
                inmateNames.Add(npc.GetComponent<NPCCollectionData>().npcData.displayName.Replace("\n", "").Replace("\r", ""));
            }
        }
        inmateNames.Add(RootObjectCache.GetRoot("Player").GetComponent<PlayerCollectionData>().playerData.displayName);
        StartCoroutine(SpeechWait());
    }
    private IEnumerator SpeechWait()
    {
        while (true)
        {
            string currentPeriod = scheduleScript.periodCode;
            List<Transform> availableInmates = new List<Transform>();
            foreach (Transform npc in aStar)
            {
                if (npc.name.Contains("Inmate"))
                {
                    NPCCollectionData npcColData = npc.GetComponent<NPCCollectionData>();
                    if(!npcColData.npcData.isDead && !npcColData.npcData.isSleeping && !npc.GetComponent<NPCSpeech>().isTalking && !npc.GetComponent<NPCCombat>().isAggro)
                    {
                        availableInmates.Add(npc);
                    }
                }
            }
            Transform randInmate = null;
            if(availableInmates.Count == 0 && inmateNames.Count > 1)
            {
                yield return null;
                continue;
            }
            if(availableInmates.Count > 0)
            {
                randInmate = availableInmates[UnityEngine.Random.Range(0, availableInmates.Count)];
            }
            Transform mainGuard = null;
            foreach(Transform npc in aStar)
            {
                if(npc.name == "Guard1")
                {
                    mainGuard = npc;
                    break;
                }
            }

            if (normalPeriodCodes.Contains(scheduleScript.periodCode) && randInmate != null)
            {
                if(scheduleScript.periodCode == "B" || scheduleScript.periodCode == "D" || scheduleScript.periodCode == "L")
                {
                    NPCAI ai = randInmate.GetComponent<NPCAI>();
                    while (true)
                    {
                        if (ai.atCanteenSeat || currentPeriod != scheduleScript.periodCode)
                        {
                            break;
                        }
                        yield return null;
                        continue;
                    }
                }
                else if(scheduleScript.periodCode == "E")
                {
                    NPCAI ai = randInmate.GetComponent<NPCAI>();
                    while (true)
                    {
                        if (ai.atExerciseEquipment || currentPeriod != scheduleScript.periodCode)
                        {
                            break;
                        }
                        yield return null;
                        continue;
                    }
                }
                else if(scheduleScript.periodCode == "S")
                {
                    NPCAI ai = randInmate.GetComponent<NPCAI>();
                    while (true)
                    {
                        if (ai.atShowerPoint || currentPeriod != scheduleScript.periodCode)
                        {
                            break;
                        }
                        yield return null;
                        continue;
                    }
                }
                if(currentPeriod != scheduleScript.periodCode)
                {
                    yield return null;
                    continue;
                }

                float time = 0f;
                int rand = UnityEngine.Random.Range(0, periodTimeDict[scheduleScript.periodCode][0]);
                int offset = periodTimeDict[scheduleScript.periodCode][1];
                while (time <= (1f / 45f) * (rand + offset))
                {
                    if (currentPeriod != scheduleScript.periodCode)
                    {
                        break;
                    }
                    if (pc.isPaused)
                    {
                        yield return null;
                        continue;
                    }
                    time += Time.deltaTime;
                    yield return null;
                }
                if(currentPeriod != scheduleScript.periodCode)
                {
                    yield return null;
                    continue;
                }
                NPCSpeech speech = randInmate.GetComponent<NPCSpeech>();
                StartCoroutine(speech.MakeTextBox(speech.GetMessage(periodSpeechDict[scheduleScript.periodCode]), randInmate, false));
            }
            else if((scheduleScript.periodCode == "FT" || scheduleScript.periodCode == "W") && randInmate != null)
            {
                bool shouldTalk = false;
                while (true)
                {
                    if(currentPeriod != scheduleScript.periodCode)
                    {
                        break;
                    }
                    if (pc.isPaused)
                    {
                        yield return null;
                        continue;
                    }
                    yield return new WaitForSeconds(50f / 45f);
                    int rand = UnityEngine.Random.Range(0, 5) + 1;
                    if(rand == 1)
                    {
                        shouldTalk = true;
                        break;
                    }
                }
                if (currentPeriod != scheduleScript.periodCode || !shouldTalk)
                {
                    yield return null;
                    continue;
                }
                NPCSpeech speech = randInmate.GetComponent<NPCSpeech>();
                StartCoroutine(speech.MakeTextBox(speech.GetMessage("Banter"), randInmate, false));
            }
            else if(scheduleScript.periodCode == "R" && mainGuard != null)
            {
                NPCCollectionData npcColData = mainGuard.GetComponent<NPCCollectionData>();
                if (!npcColData.npcData.isDead && !npcColData.npcData.isSleeping && !mainGuard.GetComponent<NPCSpeech>().isTalking && !mainGuard.GetComponent<NPCCombat>().isAggro)
                {
                    NPCAI ai = mainGuard.GetComponent<NPCAI>();
                    while (true)
                    {
                        if (ai.atGuardRollcall || currentPeriod != scheduleScript.periodCode)
                        {
                            break;
                        }
                        yield return null;
                        continue;
                    }
                    if(currentPeriod != scheduleScript.periodCode)
                    {
                        yield return null;
                        continue;
                    }

                    float time = 0f;
                    while(time <= (1f / 45f) * 150f)
                    {
                        if(currentPeriod != scheduleScript.periodCode)
                        {
                            break;
                        }
                        if (pc.isPaused)
                        {
                            yield return null;
                            continue;
                        }
                        time += Time.deltaTime;
                        yield return null;
                    }
                    if(currentPeriod != scheduleScript.periodCode)
                    {
                        yield return null;
                        continue;
                    }
                    NPCSpeech speech = mainGuard.GetComponent<NPCSpeech>();
                    string rcPhaseStr = rollcallSpeechTypes[rcPhase];
                    if (rcPhase == 2 && inmateNames.Count >= 2)
                    {
                        int rand1 = UnityEngine.Random.Range(0, inmateNames.Count);//do shakedown stuff idk
                        int rand2 = UnityEngine.Random.Range(0, inmateNames.Count);
                        if (rand1 == rand2)
                        {
                            rand2 = inmateNames.Count - 1;//player
                        }
                        rcPhaseStr = inmateNames[rand1] + " and " + inmateNames[rand2];
                        StartCoroutine(Shakedown(rand1, rand2));
                    }
                    else if(rcPhase == 2 && inmateNames.Count == 1)
                    {
                        rcPhaseStr = inmateNames[0];
                        if(UnityEngine.Random.Range(0, 10) == 0)//random easter egg ig
                        {
                            rcPhaseStr = inmateNames[0] + "... (aren't there supposed to be two?)";
                            StartCoroutine(Shakedown(0, -1));
                        }
                    }

                    if(rcPhase != 2)
                    {
                        StartCoroutine(speech.MakeTextBox(speech.GetMessage(rcPhaseStr), mainGuard, false));
                    }
                    else
                    {
                        StartCoroutine(speech.MakeTextBox(rcPhaseStr, mainGuard, false));
                    }
                    if (rcPhase < 3)
                    {
                        rcPhase++;
                    }
                }
            }
            yield return null;
        }
    }
    private IEnumerator Shakedown(int rand1, int rand2)
    {
        List<Transform> inmates = new List<Transform>();
        foreach(Transform npc in aStar)
        {
            if (npc.name.Contains("Inmate"))
            {
                inmates.Add(npc);
            }
        }
        Transform playerDesk = null;
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name.Contains("PlayerDesk"))
            {
                playerDesk = obj;
                break;
            }
        }
        inmates.Add(player);

        List<Transform> goToDesks = new List<Transform>();
        if (inmates[rand1].GetComponent<NPCCollectionData>() != null && inmates[rand1].GetComponent<NPCCollectionData>().npcData.desk != null)
        {
            goToDesks.Add(inmates[rand1].GetComponent<NPCCollectionData>().npcData.desk.transform);
        }
        else if (inmates[rand1].name == "Player" && playerDesk != null)
        {
            goToDesks.Add(playerDesk);
        }

        if(rand2 != -1)
        {
            if (inmates[rand2].GetComponent<NPCCollectionData>() != null && inmates[rand2].GetComponent<NPCCollectionData>().npcData.desk != null)
            {
                goToDesks.Add(inmates[rand2].GetComponent<NPCCollectionData>().npcData.desk.transform);
            }
            else if (inmates[rand2].name == "Player" && playerDesk != null)
            {
                goToDesks.Add(playerDesk);
            }
        }

        Debug.Log(goToDesks.Count);
        foreach(Transform desk in goToDesks)
        {
            Debug.Log(desk.name);
        }

        while (true)
        {
            if(scheduleScript.periodCode != "R")
            {
                break;
            }
            yield return null;
        }

        List<Transform> availableGuards = new List<Transform>();
        foreach(Transform npc in aStar)
        {
            if(npc.name.Contains("Guard") && !npc.GetComponent<NPCCollectionData>().npcData.isDead && !npc.GetComponent<NPCCollectionData>().npcData.isAggro)
            {
                availableGuards.Add(npc);
            }
        }

        if(availableGuards.Count == 0)
        {
            yield break;
        }

        int rand = UnityEngine.Random.Range(0, availableGuards.Count);
        Transform guard = availableGuards[rand];
        NPCCollectionData npcColData = guard.GetComponent<NPCCollectionData>();
        for (int i = 0; i < goToDesks.Count; i++)
        {
            //cehck for surroudning free tiles
            List<Vector3> vectors = new List<Vector3>
            {
                new Vector3(1.6f, 0), new Vector3(-1.6f, 0), new Vector3(0, 1.6f), new Vector3(0, -1.6f)
            };
            Vector2 goToVector = Vector2.zero;
            for (int j = 0; j < 4; j++)
            {
                GameObject checkerObj = new GameObject("CheckerObj");
                checkerObj.AddComponent<BoxCollider2D>().size = new Vector2(.8f, .8f);
                checkerObj.AddComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                checkerObj.GetComponent<BoxCollider2D>().isTrigger = true;
                checkerObj.layer = LayerMask.NameToLayer("Ground");
                checkerObj.transform.position = goToDesks[i].position + vectors[j];
                yield return new WaitForFixedUpdate();

                Collider2D checkerCollider = checkerObj.GetComponent<BoxCollider2D>();
                List<Collider2D> hitColliders = new List<Collider2D>();
                ContactFilter2D filter = ContactFilter2D.noFilter;
                checkerCollider.Overlap(filter, hitColliders);
                bool hitDigable = false;
                foreach (Collider2D col in hitColliders)
                {
                    Debug.Log(j.ToString() + ": " + col.gameObject.name);
                    if (col.CompareTag("Digable") && col.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        goToVector = col.transform.position;
                        hitDigable = true;
                        break;
                    }
                }
                Destroy(checkerObj);
                if (hitDigable)
                {
                    break;
                }
            }
            if (goToVector == Vector2.zero)
            {
                continue;
            }

            Debug.Log("sending guard" + guard.name + " to " + goToVector);
            guard.GetComponent<NPCAI>().SendToPos(goToVector);

            while (true)
            {
                if(Vector2.Distance(guard.position, goToVector) <= .1f)
                {
                    break;
                }
                if(npcColData.npcData.isDead || npcColData.npcData.isAggro)
                {
                    yield break;
                }
                yield return null;
            }
            Debug.Log("Checking desk: " + goToDesks[i].name);
            foreach (DeskItem item in goToDesks[i].GetComponent<DeskData>().deskInv)
            {
                if(item.itemData != null && item.itemData.isContraband)
                {
                    item.itemData = null;
                    if (goToDesks[i].name.Contains("PlayerDesk"))
                    {
                        StartCoroutine(solitaryScript.GoToSolitary("CaughtShakedown"));
                        yield break;
                    }
                }
            }
        }
    }
}
