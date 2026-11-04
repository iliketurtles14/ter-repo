using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterShredderAnim : MonoBehaviour
{
    //.1
    //342
    //340
    //341
    private List<Sprite> sprites = new List<Sprite>();
    private PauseController pc;
    private void Start()
    {
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        DataSender ds = DataSender.instance;
        sprites.Add(ds.PrisonObjectImages[342]);
        sprites.Add(ds.PrisonObjectImages[340]);
        sprites.Add(ds.PrisonObjectImages[341]);
        sprites.Add(ds.PrisonObjectImages[340]);
        StartCoroutine(Anim());
    }
    private IEnumerator Anim()
    {
        float time = 0f;
        int index = 0;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while (true)
        {
            if(index > 3 || index < 0)
            {
                index = 0;
            }
            sr.sprite = sprites[index];
            time = 0f;
            while(time <= .1f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }
            index++;
            yield return null;
        }
    }
}