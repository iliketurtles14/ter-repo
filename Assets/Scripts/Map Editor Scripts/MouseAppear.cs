using System.Collections;
using UnityEngine;

public class MouseAppear : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();

        Cursor.visible = true;
    }
}
