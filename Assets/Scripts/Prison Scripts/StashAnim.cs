using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class StashAnim : MonoBehaviour
{
    private Sprite s0;
    private Sprite s1;
    private Sprite s2;
    private Sprite s3;
    private SpriteRenderer sr;
    private PauseController pc;
    public bool shouldStop;
    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        StartCoroutine(StartWait());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        DataSender ds = DataSender.instance;
        s0 = ds.PrisonObjectImages[224];
        s1 = ds.PrisonObjectImages[225];
        s2 = ds.PrisonObjectImages[226];
        s3 = ds.PrisonObjectImages[227];
        StartCoroutine(Anim());
    }
    private IEnumerator Anim()
    {
        while (true)
        {
            float time = 0f;
            sr.sprite = s0;
            while(time < 1.617f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                if (shouldStop)
                {
                    yield break;
                }
                time += Time.deltaTime;
                yield return null;
            }
            time = 0f;
            sr.sprite = s1;
            while (time < .266f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                if (shouldStop)
                {
                    yield break;
                }
                time += Time.deltaTime;
                yield return null;
            }
            time = 0f;
            sr.sprite = s2;
            while (time < .266f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                if (shouldStop)
                {
                    yield break;
                }
                time += Time.deltaTime;
                yield return null;
            }
            time = 0f;
            sr.sprite = s3;
            while (time < .266f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                if (shouldStop)
                {
                    yield break;
                }
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
