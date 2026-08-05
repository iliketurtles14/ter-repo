using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.AddressableAssets;

public class Escaping : MonoBehaviour
{
    private Map currentMap;
    private float southBound;
    private float westBound;
    private float northBound;
    private float eastBound;
    private bool ready = false;
    private Transform player;
    public bool hasEscaped;
    private PauseController pc;
    private bool isQuitting;
    private Zones zonesScript;
    private PlayerCollectionData playerColData;
    private Routine routineScript;
    private bool hasResetHeat;
    private Transform aStar;
    private Transform ic;

    //esc calc stuff
    public int pStats;
    public int avgHeat;
    public int pRep;
    public int good;
    public int bad;
    public int daysTaken;
    public int efficiency;
    public int total;

    public int highestHeat;
    public int totalHeat;
    public int effNum;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        pc = GetComponent<PauseController>();
        zonesScript = GetComponent<Zones>();
        playerColData = player.GetComponent<PlayerCollectionData>();
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        ic = RootObjectCache.GetRoot("InventoryCanvas").transform;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForSeconds(3);

        currentMap = GetComponent<LoadPrison>().currentMap;
        SetBounds();
        ready = true;
    }
    private void SetBounds()
    {
        southBound = westBound = -.8f;
        northBound = (currentMap.sizeY * 1.6f) - .8f;
        eastBound = (currentMap.sizeX * 1.6f) - .8f;
    }
    private void Update()
    {
        if (!ready || hasEscaped)
        {
            if (hasEscaped && Input.GetMouseButtonDown(0) && !isQuitting)
            {
                Leave();
            }
            else
            {
                return;
            }
        }

        if(player.position.x <= westBound || player.position.x >= eastBound ||
            player.position.y <= southBound || player.position.y >= northBound)
        {
            hasEscaped = true;
            StartCoroutine(Escape());
        }

        if (zonesScript.isTouchingEscape)
        {
            hasEscaped = true;
            StartCoroutine(Escape());
        }

        if(playerColData.playerData.heat > highestHeat)
        {
            highestHeat = playerColData.playerData.heat;
        }
        if(routineScript.min == routineScript.startingMin + 1 && routineScript.sec == 0 && !hasResetHeat)
        {
            hasResetHeat = true;
            totalHeat += highestHeat;
            highestHeat = 0;
        }
        if(routineScript.sec != 0)
        {
            hasResetHeat = false;
        }
    }
    public IEnumerator Escape()
    {
        if (hasEscaped)
        {
            yield break;
        }
        
        hasEscaped = true;

        Calculate();

        pc.Pause(true);
        
        //do black bar anim
        Transform ec = RootObjectCache.GetRoot("EscapeCanvas").transform;
        ec.Find("EscapeMenuPanel").Find("IGT").GetComponent<TextMeshProUGUI>().text = ic.Find("IGT").GetComponent<TextMeshProUGUI>().text;
        ec.Find("EscapeMenuPanel").Find("PausedIGT").GetComponent<TextMeshProUGUI>().text = ic.Find("PausedIGT").GetComponent<TextMeshProUGUI>().text;
        ec.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(.49f);

        ec.Find("EscapeMenuPanel").Find("PrisonNameText").GetComponent<TextMeshProUGUI>().text = currentMap.mapName.ToUpper();

        //kill black bars and show screen
        ec.Find("BigBlockerPanel").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").gameObject.SetActive(true);
        ec.Find("BlockerPanel1").gameObject.SetActive(false);
        ec.Find("BlockerPanel2").gameObject.SetActive(false);
        PSoundController.PlaySound("rumble");
        PMusicController.PlayMusic("escaped");
        //do score anims
        yield return new WaitForSeconds(.85f);
        ec.Find("EscapeMenuPanel").Find("PlayerStatsText").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").Find("PlayerStatsValue").gameObject.SetActive(true);
        yield return new WaitForSeconds(.55f);
        ec.Find("EscapeMenuPanel").Find("AverageHeatText").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").Find("AverageHeatValue").gameObject.SetActive(true);
        yield return new WaitForSeconds(.55f);
        ec.Find("EscapeMenuPanel").Find("PlayerReputationText").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").Find("PlayerReputationValue").gameObject.SetActive(true);
        yield return new WaitForSeconds(.55f);
        ec.Find("EscapeMenuPanel").Find("GoodBehaviorText").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").Find("GoodBehaviorValue").gameObject.SetActive(true);
        yield return new WaitForSeconds(.55f);
        ec.Find("EscapeMenuPanel").Find("BadBehaviorText").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").Find("BadBehaviorValue").gameObject.SetActive(true);
        yield return new WaitForSeconds(.55f);
        ec.Find("EscapeMenuPanel").Find("DaysTakenBonusText").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").Find("DaysTakenBonusValue").gameObject.SetActive(true);
        yield return new WaitForSeconds(.55f);
        ec.Find("EscapeMenuPanel").Find("EfficiencyText").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").Find("EfficiencyValue").gameObject.SetActive(true);
        yield return new WaitForSeconds(.55f);
        ec.Find("EscapeMenuPanel").Find("OverallScoreText").gameObject.SetActive(true);
        yield return new WaitForSeconds(.55f);
        ec.Find("EscapeMenuPanel").Find("OverallScoreValue").gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        while (true)
        {
            ec.Find("EscapeMenuPanel").Find("ClickToContinueText").gameObject.SetActive(true);
            yield return new WaitForSeconds(.55f);
            ec.Find("EscapeMenuPanel").Find("ClickToContinueText").gameObject.SetActive(false);
            yield return new WaitForSeconds(.55f);
        }
    }
    private void Calculate()
    {
        //player stats
        PlayerCollectionData pColData = player.GetComponent<PlayerCollectionData>();
        int str = pColData.playerData.strength;
        int spd = pColData.playerData.speed;
        int intel = pColData.playerData.intellect;
        pStats = str + spd + intel;

        //avg heat
        totalHeat += highestHeat;
        totalHeat = Mathf.FloorToInt(Mathf.Min(100, totalHeat) / routineScript.day);
        totalHeat = Mathf.Max(0, totalHeat);

        //player rep
        foreach(Transform npc in aStar)
        {
            if(npc.name.StartsWith("Inmate") || npc.name.StartsWith("Guard"))
            {
                pRep += npc.GetComponent<NPCCollectionData>().npcData.opinion;
            }
        }

        //bad
        bad *= -1;

        //days taken bonus
        daysTaken = (16 - routineScript.day) * 1000;

        //efficiency
        efficiency = Mathf.Max(0, (30000 - effNum));

        //total
        total = pStats + efficiency + pRep + good + bad + daysTaken;
        if(total < 0)
        {
            total = 0;
        }

        //set text stuff
        Transform ec = RootObjectCache.GetRoot("EscapeCanvas").transform;
        ec.Find("EscapeMenuPanel").Find("PlayerStatsValue").GetComponent<TextMeshProUGUI>().text = pStats.ToString();
        ec.Find("EscapeMenuPanel").Find("AverageHeatValue").GetComponent<TextMeshProUGUI>().text = avgHeat.ToString() + "%";
        ec.Find("EscapeMenuPanel").Find("PlayerReputationValue").GetComponent<TextMeshProUGUI>().text = pRep.ToString();
        ec.Find("EscapeMenuPanel").Find("GoodBehaviorValue").GetComponent<TextMeshProUGUI>().text = good.ToString();
        ec.Find("EscapeMenuPanel").Find("BadBehaviorValue").GetComponent<TextMeshProUGUI>().text = bad.ToString();
        ec.Find("EscapeMenuPanel").Find("DaysTakenBonusValue").GetComponent<TextMeshProUGUI>().text = daysTaken.ToString();
        ec.Find("EscapeMenuPanel").Find("EfficiencyValue").GetComponent<TextMeshProUGUI>().text = efficiency.ToString();
        ec.Find("EscapeMenuPanel").Find("OverallScoreValue").GetComponent<TextMeshProUGUI>().text = total.ToString();
    }
    private void Leave()
    {
        isQuitting = true;
        Addressables.LoadSceneAsync("Main Menu");
        GetGivenData.instance.GetComponent<DumperStartStop>().isGoingToMainMenu = true;
    }
}
