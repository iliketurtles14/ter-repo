using System.Collections;
using UnityEngine;

public class LoadBlockerController : MonoBehaviour
{
    private Transform loadCanvas;
    private void Start()
    {
        loadCanvas = RootObjectCache.GetRoot("LoadBlockerCanvas").transform;
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
        Destroy(loadCanvas.gameObject);
    }
}
