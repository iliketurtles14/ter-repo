using System.Collections;
using UnityEngine;
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
    private bool hasEscaped;
    private PauseController pc;
    private bool isQuitting;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        pc = GetComponent<PauseController>();
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
    }
    public IEnumerator Escape()
    {
        pc.Pause(true);
        
        //do black bar anim
        Transform ec = RootObjectCache.GetRoot("EscapeCanvas").transform;
        ec.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(.49f);

        //kill black bars and show screen
        ec.Find("BigBlockerPanel").gameObject.SetActive(true);
        ec.Find("EscapeMenuPanel").gameObject.SetActive(true);
        ec.Find("BlockerPanel1").gameObject.SetActive(false);
        ec.Find("BlockerPanel2").gameObject.SetActive(false);

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
    private void Leave()
    {
        isQuitting = true;
        Addressables.LoadSceneAsync("Main Menu");
        GetGivenData.instance.GetComponent<DumperStartStop>().isGoingToMainMenu = true;
    }
}
