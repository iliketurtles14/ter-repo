using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class WeedAnimation : MonoBehaviour
{
    private ApplyPrisonData applyScript;
    private void Start()
    {
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
    }
    private void OnEnable()
    {
        StartCoroutine(Animate());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator Animate()
    {
        while (true)
        {
            GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[197];
            yield return new WaitForSeconds(.55f);
            GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[198];
            yield return new WaitForSeconds(.55f);
            GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[199];
            yield return new WaitForSeconds(.55f);
            GetComponent<SpriteRenderer>().sprite = applyScript.PrisonObjectSprites[198];
            yield return new WaitForSeconds(.55f);
        }
    }
}
