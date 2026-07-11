using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        pc = GetComponent<PauseController>();
        aStar = RootObjectCache.GetRoot("A*").transform;
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
            if(availableInmates.Count == 0)
            {
                yield return null;
                continue;
            }

            Transform randInmate = availableInmates[UnityEngine.Random.Range(0, availableInmates.Count)];
            Transform mainGuard = null;
            foreach(Transform npc in aStar)
            {
                if(npc.name == "Guard1")
                {
                    mainGuard = npc;
                    break;
                }
            }

            if (normalPeriodCodes.Contains(scheduleScript.periodCode))
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
            else if(scheduleScript.periodCode == "FT" || scheduleScript.periodCode == "W")
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
                        rcPhaseStr = inmateNames[rand1] + " and " + inmateNames[rand2] + "";
                    }
                    else if(rcPhase == 2 && inmateNames.Count == 1)
                    {
                        rcPhaseStr = inmateNames[0];
                        if(UnityEngine.Random.Range(0, 10) == 0)//random easter egg ig
                        {
                            rcPhaseStr = inmateNames[0] + "... (aren't there supposed to be two?)";
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
}
