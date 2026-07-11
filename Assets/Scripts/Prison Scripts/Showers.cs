using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showers : MonoBehaviour
{
    private Schedule scheduleScript;
    private bool doAnim;
    private SpriteRenderer thisSR;
    private List<Sprite> sprites = new List<Sprite>();
    private PauseController pc;
    private void Start()
    {
        scheduleScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        thisSR = GetComponent<SpriteRenderer>();
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
        sprites.Add(ds.PrisonObjectImages[57]);
        sprites.Add(ds.PrisonObjectImages[58]);
        sprites.Add(ds.PrisonObjectImages[59]);
        StartCoroutine(Anim());
    }
    private void Update()
    {
        if (doAnim)
        {
            thisSR.enabled = true;
        }
        else
        {
            thisSR.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player" || (other.gameObject.CompareTag("NPC") && other.gameObject.name.StartsWith("Inmate")))
        {
            if(other.gameObject.name.Contains("Inmate") && scheduleScript.periodCode == "S")
            {
                doAnim = true;
            }
            else if(other.gameObject.name == "Player")
            {
                doAnim = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" || (other.gameObject.CompareTag("NPC") && other.gameObject.name.StartsWith("Inmate")))
        {
            if (other.gameObject.name.Contains("Inmate") && scheduleScript.periodCode == "S")
            {
                doAnim = true;
            }
            else if (other.gameObject.name == "Player")
            {
                doAnim = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" || (other.gameObject.CompareTag("NPC") && other.gameObject.name.StartsWith("Inmate")))
        {
            doAnim = false;
        }
    }
    private IEnumerator Anim()
    {
        while (true)
        {
            thisSR.sprite = sprites[0];
            float time = 0f;
            while(time < .1f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }
            thisSR.sprite = sprites[1];
            time = 0f;
            while (time < .1f)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }
            thisSR.sprite = sprites[2];
            time = 0f;
            while (time < .1f)
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
