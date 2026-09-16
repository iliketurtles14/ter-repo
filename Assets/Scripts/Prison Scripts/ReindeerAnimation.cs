using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReindeerAnimation : MonoBehaviour
{
    public string lookDir = "right";
    private int lookNum;
    public List<Sprite> dirSprites;
    public List<Sprite> rudolphSprites;
    private int whichCycle;
    private SpriteRenderer sr;
    private ReindeerHandler reindeerHandlerScript;
    private PauseController pc;
    private ApplyPrisonData apd;
    private BoxCollider2D bc;
    private void OnEnable()
    {
        apd = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        dirSprites = apd.reindeerSprites;
        rudolphSprites = apd.rudolphSprites;
        sr = GetComponent<SpriteRenderer>();
        reindeerHandlerScript = GetComponent<ReindeerHandler>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        bc = GetComponent<BoxCollider2D>();
        StartCoroutine(AnimCycle());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        if(lookDir != null)
        {
            switch (lookDir)
            {
                case "right": lookNum = 0; break;
                case "up": lookNum = 2; break;
                case "left": lookNum = 4; break;
                case "down": lookNum = 6; break;
            }
        }
        if(reindeerHandlerScript.displayName == "Rudolph")
        {
            sr.sprite = rudolphSprites[lookNum + whichCycle];
        }
        else
        {
            sr.sprite = dirSprites[lookNum + whichCycle];
        }
        sr.size = new Vector2(sr.sprite.texture.width / 10f, sr.sprite.texture.height / 10f);
        bc.size = sr.size;
    }
    private IEnumerator AnimCycle()
    {
        while (true)
        {
            whichCycle = 0;
            float time = 0;
            while(time < .45f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }
            whichCycle = 1;
            time = 0;
            while (time < .45f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
