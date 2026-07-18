using System.Collections;
using TMPro;
using UnityEngine;

public class Lockdown : MonoBehaviour//20%, 40%, 80%
{
    private Transform player;
    private Coroutine heatLoopCoroutine;
    private Coroutine textLoopCoroutine;
    public bool lockdownIsActive;
    public bool isRiotLockdown; //one that sends you to solitary if you die (killing guards to make lockdown happen)
    public int lockdownTime;
    private Solitary solitaryScript;
    private Transform mc;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        solitaryScript = GetComponent<Solitary>();
        mc.Find("LockdownTextPanel").Find("Text").GetComponent<TextMeshProUGUI>().text = "";
    }
    public void StartLockdown()
    {
        if (!lockdownIsActive)
        {
            lockdownIsActive = true;
            heatLoopCoroutine = StartCoroutine(HeatLoop());
            textLoopCoroutine = StartCoroutine(LockdownTextLoop());
        }
    }
    public void StopLockdown()
    {
        lockdownIsActive = false;
        isRiotLockdown = false;
        StopCoroutine(heatLoopCoroutine);
        StopCoroutine(textLoopCoroutine);
        mc.Find("LockdownTextPanel").Find("Text").GetComponent<TextMeshProUGUI>().text = "";
    }
    private IEnumerator HeatLoop()
    {
        PlayerCollectionData playerColData = player.GetComponent<PlayerCollectionData>();
        while (lockdownIsActive)
        {
            playerColData.playerData.heat = 99;
            yield return null;
        }
    }
    private IEnumerator LockdownTextLoop()
    {
        lockdownTime = 99;
        TextMeshProUGUI tmp = mc.Find("LockdownTextPanel").Find("Text").GetComponent<TextMeshProUGUI>();
        while (lockdownIsActive)
        {
            if(lockdownTime == 1)
            {
                tmp.text = "BACKUP ARRIVES IN " + lockdownTime.ToString() + " SECOND";
            }
            else
            {
                tmp.text = "BACKUP ARRIVES IN " + lockdownTime.ToString() + " SECONDS";
            }
            yield return new WaitForSeconds(.5f);
            tmp.text = "";
            yield return new WaitForSeconds(.5f);
            lockdownTime--;
            if(lockdownTime == 0)
            {
                lockdownIsActive = false;
                StopLockdown();
                StartCoroutine(solitaryScript.GoToSolitary(""));
            }
        }
    }
}
