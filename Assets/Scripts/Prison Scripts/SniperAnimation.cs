using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAnimation : MonoBehaviour
{
    private List<Sprite> sniperSprites = new List<Sprite>();
    private ApplyPrisonData applyScript;
    private bool ready;
    private int whichCycle;
    private int direction;
    private void Start()
    {
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        sniperSprites = applyScript.sniperSprites;
        ready = true;

        StartCoroutine(AnimCycle());
        StartCoroutine(DirLoop());
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = sniperSprites[direction + whichCycle];
    }
    private IEnumerator AnimCycle()
    {
        while (true)
        {
            whichCycle = 0;
            yield return new WaitForSeconds(.266f);
            whichCycle = 1;
            yield return new WaitForSeconds(.266f);
        }
    }
    private IEnumerator DirLoop()
    {
        while (true)
        {
            int rand = UnityEngine.Random.Range(0, 4);
            direction = rand * 2;
            yield return new WaitForSeconds(5);
        }
    }
}
