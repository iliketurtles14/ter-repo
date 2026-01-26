using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCIDAnimation : MonoBehaviour
{
    public List<Sprite> bodyDirSprites;
    public List<Sprite> outfitDirSprites;
    private int whichCycle = 0;

    private void OnEnable()
    {
        StartCoroutine(AnimCycle());
    }
    private void Update()
    {
        transform.Find("Outfit").position = transform.position;
        try
        {
            GetComponent<Image>().sprite = bodyDirSprites[whichCycle + 6];
            transform.Find("Outfit").GetComponent<Image>().sprite = outfitDirSprites[whichCycle + 6];
        }
        catch { }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
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
}
