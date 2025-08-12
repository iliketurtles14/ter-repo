using System.Collections;
using UnityEngine;

public class OnMapEditorLoad : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitForLoad());
    }
    private IEnumerator WaitForLoad()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        GetComponent<LoadMap>().StartLoad(true);
    }
}
