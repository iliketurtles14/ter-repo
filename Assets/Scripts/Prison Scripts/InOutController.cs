using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class InOutController : MonoBehaviour
{
    private Routine routineScript;
    private Map currentMap;
    private bool ready = false;
    private Transform mc;
    private TextMeshProUGUI tmp;
    private string doorsCloseStr;
    private Lockdown lockdownScript;
    private Transform player;
    private bool isOutside;
    private Zones zonesScript;
    private void Start()
    {
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        tmp = mc.Find("LockdownTextPanel").Find("Text").GetComponent<TextMeshProUGUI>();
        lockdownScript = GetComponent<Lockdown>();
        player = RootObjectCache.GetRoot("Player").transform;
        zonesScript = GetComponent<Zones>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = GetComponent<LoadPrison>().currentMap;
        if(routineScript.doorsCloseMin.ToString().Count() == 1)
        {
            doorsCloseStr = "0" + routineScript.doorsCloseMin.ToString() + ":00";
        }
        else
        {
            doorsCloseStr = routineScript.doorsCloseMin.ToString() + ":00";
        }
        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if(currentMap.grounds == "Outside" || currentMap.grounds == "Inside")
        {
            enabled = false;
            return;
        }

        if (!Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground")) &&
            player.GetComponent<PlayerFloorCollision>().playerFloor.GetComponent<TileCollectionData>().tileData.tileType == "outFloor")
        {
            isOutside = true;
        }
        else
        {
            isOutside = false;
        }

        if (isOutside && routineScript.min == routineScript.doorsCloseMin - 1 && routineScript.sec >= 30 && !lockdownScript.lockdownIsActive && !zonesScript.isTouchingSafe)
        {
            tmp.text = "WARNING! INMATES MUST BE INSIDE BY " + doorsCloseStr + "!";
        }
        else if (!lockdownScript.lockdownIsActive)
        {
            tmp.text = "";
        }
    }
}
