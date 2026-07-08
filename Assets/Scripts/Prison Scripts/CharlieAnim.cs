using System.Collections;
using UnityEngine;

public class CharlieAnim : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animation());
    }
    private IEnumerator Animation()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while (true)
        {
            sr.flipX = true;
            yield return new WaitForSeconds(.266f);
            sr.flipX = false;
            yield return new WaitForSeconds(.266f);
        }
    }
}
