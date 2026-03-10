using System.Collections;
using UnityEngine;

public class PatchNotesSet : MonoBehaviour
{
    public Transform mmc;
    private void Start()
    {
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        mmc.Find("PatchNotesPanel").gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        mmc.Find("PatchNotesPanel").gameObject.SetActive(false);
    }
}
