using System.Collections;
using UnityEngine;

public class LoadBlockerController : MonoBehaviour
{
    private Transform loadCanvas;
    private Routine routineScript;
    private void Start()
    {
        loadCanvas = RootObjectCache.GetRoot("LoadBlockerCanvas").transform;
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
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
        yield return new WaitForSeconds(1);
        routineScript.isFrozen = false;
        Destroy(loadCanvas.gameObject);
    }
}
