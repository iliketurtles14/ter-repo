using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBlockerController : MonoBehaviour
{
    private Transform loadCanvas;
    private Routine routineScript;
    private Transform player;
    private Sittables sittablesScript;
    private TutorialController tutControllerScript;
    private Dictionary<string, string> periodMusicDict = new Dictionary<string, string>
    {
        { "B", "chow" }, { "L", "chow" }, { "D", "chow" }, { "R", "rollcall" }, { "LO", "lightsout" },
        { "LD", "lockdown" }, { "S", "shower" }, { "E", "workout" }, { "FT", "freetime" }, { "W", "work" }
    };
    private void Start()
    {
        loadCanvas = RootObjectCache.GetRoot("LoadBlockerCanvas").transform;
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        player = RootObjectCache.GetRoot("Player").transform;
        sittablesScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Sittables>();
        player.GetComponent<PlayerCtrl>().canMove = false;
        sittablesScript.canLeaveSittable = false;
        tutControllerScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<TutorialController>();
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
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        StartCoroutine(PlayBlockerAnim());
    }
    private IEnumerator PlayBlockerAnim()
    {
        loadCanvas.GetComponent<Animator>().enabled = true;
        routineScript.isFrozen = true;
        yield return new WaitForSeconds(1.5f);
        routineScript.isFrozen = false;
        tutControllerScript.canStart = true;
        Destroy(loadCanvas.gameObject);
        player.GetComponent<PlayerCtrl>().canMove = true;
        sittablesScript.canLeaveSittable = true;
        PMusicController.PlayMusic(periodMusicDict[routineScript.periodCode]);
    }
}
