using System.Collections;
using UnityEngine;

public class JobBoardAnimation : MonoBehaviour
{
    private ApplyPrisonData applyScript;
    private Sprite s1;
    private Sprite s2;
    private void Start()
    {
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        StartCoroutine(AnimLoop());
    }
    private IEnumerator AnimLoop()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        s1 = applyScript.PrisonObjectSprites[184];
        s2 = applyScript.PrisonObjectSprites[185];
        while (true)
        {
            GetComponent<SpriteRenderer>().sprite = s1;
            yield return new WaitForSeconds(.567f);
            GetComponent<SpriteRenderer>().sprite = s2;
            yield return new WaitForSeconds(.567f);
        }
    }
}
