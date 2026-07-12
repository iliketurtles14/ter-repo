using System.Collections;
using UnityEngine;

public class LoadBlockerController : MonoBehaviour
{
    private Transform loadCanvas;
    private Routine routineScript;
    private Transform player;
    private Sittables sittablesScript;
    private void Start()
    {
        loadCanvas = RootObjectCache.GetRoot("LoadBlockerCanvas").transform;
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        player = RootObjectCache.GetRoot("Player").transform;
        sittablesScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Sittables>();
        player.GetComponent<PlayerCtrl>().canMove = false;
        sittablesScript.canLeaveSittable = false;
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
        yield return new WaitForSeconds(1);
        routineScript.isFrozen = false;
        Destroy(loadCanvas.gameObject);
        player.GetComponent<PlayerCtrl>().canMove = true;
        sittablesScript.canLeaveSittable = true;
    }
}
