using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takeover : MonoBehaviour
{
    private CreateNote noteScript;
    private Transform player;
    private Map currentMap;
    private UnlockDoors unlockScript;
    public bool takeoverIsActive;
    private void Start()
    {
        noteScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<CreateNote>();
        player = RootObjectCache.GetRoot("Player").transform;
        unlockScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<UnlockDoors>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
    }
    public void StartTakeover()
    {
        takeoverIsActive = true;
        PSoundController.PlaySound("buy");
        string msg = "Okay " + player.GetComponent<PlayerCollectionData>().playerData.displayName + ", you've made your point!\n\nThe main prison gate is now unlocked as requested, just please... don't hurt anyone else!";
        string warden = "Warden " + currentMap.warden;
        noteScript.CreateWardenNote("getJob", msg, warden);
        unlockScript.aUnlockDoors(new List<string> { "white", "yellow", "purple" });
    }
    public void StopTakeover()
    {
        takeoverIsActive = false;
        unlockScript.LockDoors(new List<string> { "white", "yellow", "purple" });
    }
}
