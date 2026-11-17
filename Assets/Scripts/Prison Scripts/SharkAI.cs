using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAI : MonoBehaviour
{
    public float speed = 3.5f;
    public int id;
    public List<Transform> wps = new List<Transform>(); //sorted to their order
    private PauseController pc;
    private SpriteRenderer sr;
    private DataSender ds;
    private void Start()
    {
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait() //js to make sure it has all its stuff
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if(wps.Count == 0)
        {
            yield break;
        }
        transform.position = wps[0].position;
        ds = DataSender.instance;
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(AILoop());
    }
    private IEnumerator AILoop()
    {
        int nextWPIndex = 0;
        int lastWPIndex = 0;
        while (true)
        {
            if(wps.Count == lastWPIndex + 1)
            {
                nextWPIndex = 0;
            }
            else
            {
                nextWPIndex++;
            }

            Vector3 posToGo = wps[nextWPIndex].position;

            while (Vector2.Distance(transform.position, posToGo) >= .1f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                Vector3 dir = (posToGo - transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                angle = (angle + 360) % 360;
                Sprite spr = GetSharkDir(angle);
                sr.sprite = spr;
                transform.position += speed * Time.deltaTime * dir;
                yield return null;
            }
            transform.position = posToGo;
            lastWPIndex = nextWPIndex;
            yield return null;
        }
    }
    private Sprite GetSharkDir(float angle) //if ur wondering why this looks like this, just draw the unit circle with the angles below
    {
        if(angle <= 90 || angle >= 315)
        {
            //right 317
            return ds.PrisonObjectImages[317];
        }
        else if(angle > 90 && angle <= 225)
        {
            //left 319
            return ds.PrisonObjectImages[319];
        }
        else
        {
            //down 318
            return ds.PrisonObjectImages[318];
        }
    }
}
