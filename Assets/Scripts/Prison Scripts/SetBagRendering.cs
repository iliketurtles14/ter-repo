using System.Collections;
using UnityEngine;

public class SetBagRendering : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(EnableWait());
    }
    private IEnumerator EnableWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GetComponent<SpriteRenderer>().sortingLayerID = transform.parent.GetComponent<SpriteRenderer>().sortingLayerID;
    }
}
