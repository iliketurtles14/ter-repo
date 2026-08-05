using System.Collections;
using UnityEngine;

public class StartFade : MonoBehaviour
{
    public Transform blocker;
    private void Start()
    {
        StartCoroutine(FadeWait());
    }
    private IEnumerator FadeWait()
    {
        blocker.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1.6f);
        blocker.GetComponent<Animator>().enabled = false;
    }
}
