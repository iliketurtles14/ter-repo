using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public string lookDir;
    private int lookNum;
    public List<Sprite> bodyDirSprites;
    public List<Sprite> outfitDirSprites;
    private int whichCycle;
    public bool shouldRestartCycle;
    public void OnEnable()
    {
        StartCoroutine(DirWait());
        StartCoroutine(AnimCycle());
    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
    public void Update()
    {
        transform.Find("Outfit").position = transform.position;

        if(lookDir != null )
        {
            switch (lookDir)
            {
                case "right": lookNum = 0; break;
                case "up": lookNum = 2; break;
                case "left": lookNum = 4; break;
                case "down": lookNum = 6; break;
            }

            if(lookNum + whichCycle + 1 > bodyDirSprites.Count())
            {
                return;
            }

            try
            {
                GetComponent<SpriteRenderer>().sprite = bodyDirSprites[lookNum + whichCycle];
                transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitDirSprites[lookNum + whichCycle];
            }
            catch
            {

            }
        }
    }
    public IEnumerator AnimCycle()
    {
        while (true)
        {
            whichCycle = 0;
            float time = 0;
            while(time < .266f && !shouldRestartCycle)
            {
                time += Time.deltaTime;
                yield return null;
            }
            if (shouldRestartCycle)
            {
                shouldRestartCycle = false;
                continue;
            }
            whichCycle = 1;
            time = 0;
            while (time < .266f && !shouldRestartCycle)
            {
                time += Time.deltaTime;
                yield return null;
            }
            if (shouldRestartCycle)
            {
                shouldRestartCycle = false;
                continue;
            }
        }
    }
    public IEnumerator DirWait()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            DirGet();

        }
    }
    public void DirGet()
    {
        if (!GetComponent<PlayerCtrl>().enabled)
        {
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            lookDir = "right";
        }
        else if (Input.GetKey(KeyCode.A))
        {
            lookDir = "left";
        }
        else if (Input.GetKey(KeyCode.W))
        {
            lookDir = "up";
        }
        else if (Input.GetKey(KeyCode.S))
        {
            lookDir = "down";
        }
    }
}
