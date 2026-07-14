using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeepAnimation : MonoBehaviour
{
    private List<Sprite> jeepSprites;
    private ApplyPrisonData applyScript;
    private JeepMovement jeepMovementScript;
    private string lookDir;
    private int lookNum;
    private int whichCycle;
    private string oldLookDir;
    private Particles particlesScript;
    private Vector2 dustPos;
    private Coroutine animCoroutine;
    private Coroutine dustCoroutine;
    private void Start()
    {
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        jeepMovementScript = GetComponent<JeepMovement>();
        particlesScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Particles>();
        jeepSprites = new List<Sprite>()
        {
            applyScript.PrisonObjectSprites[216],
            applyScript.PrisonObjectSprites[217],
            applyScript.PrisonObjectSprites[222],
            applyScript.PrisonObjectSprites[223],
            applyScript.PrisonObjectSprites[218],
            applyScript.PrisonObjectSprites[219],
            applyScript.PrisonObjectSprites[220],
            applyScript.PrisonObjectSprites[221]
        };
    }
    private void OnEnable()
    {
        animCoroutine = StartCoroutine(AnimCycle());
        dustCoroutine = StartCoroutine(DustCycle());
    }
    private void OnDisable()
    {
        StopCoroutine(animCoroutine);
        StopCoroutine(dustCoroutine);
    }
    private void Update()
    {        
        lookDir = jeepMovementScript.currentDir;
        if(lookDir != null)
        {
            switch (lookDir)
            {
                case "right": 
                    lookNum = 0; 
                    GetComponent<SpriteRenderer>().size = new Vector2(4.8f, 3.2f);
                    dustPos = new Vector2(transform.position.x - 2.4f, transform.position.y);
                    break;
                case "up":
                    lookNum = 2;
                    GetComponent<SpriteRenderer>().size = new Vector2(3.2f, 4.8f);
                    dustPos = new Vector2(transform.position.x, transform.position.y - 2.4f);
                    break;
                case "left": 
                    lookNum = 4;
                    GetComponent<SpriteRenderer>().size = new Vector2(4.8f, 3.2f);
                    dustPos = new Vector2(transform.position.x + 2.4f, transform.position.y);
                    break;
                case "down":
                    lookNum = 6;
                    GetComponent<SpriteRenderer>().size = new Vector2(3.2f, 4.8f);
                    dustPos = new Vector2(transform.position.x, transform.position.y + 2.4f);
                    break;
            }

            GetComponent<SpriteRenderer>().sprite = jeepSprites[lookNum + whichCycle];
        }

        oldLookDir = lookDir;
    }
    private IEnumerator AnimCycle()
    {
        while (true)
        {
            whichCycle = 0;
            float time = 0;
            while (time < .266f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            whichCycle = 1;
            time = 0;
            while (time < .266f)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
    private IEnumerator DustCycle()
    {
        while (true)
        {
            float time = 0;
            while(time < .5f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            float rand = UnityEngine.Random.Range(1f, 2f);
            particlesScript.CreateDust(dustPos, rand);
        }
    }
}
