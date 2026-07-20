using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCIDAnimation : MonoBehaviour
{
    public List<Sprite> bodyDirSprites;
    public List<Sprite> outfitDirSprites;
    public string character;
    public string outfit;
    private List<Sprite> clearList;
    private int whichCycle = 0;
    public GameObject currentNPC;

    private void OnEnable()
    {
        Sprite clear = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        clearList = new List<Sprite>()
        {
            clear, clear, clear, clear, clear, clear, clear, clear
        };

        StartCoroutine(AnimCycle());
    }
    private void Update()
    {
        if(currentNPC != null)
        {
            bodyDirSprites = new List<Sprite>(currentNPC.GetComponent<BodyController>().characterDict[character][2]);
            if(outfit == "empty")
            {
                outfitDirSprites = new List<Sprite>(clearList);
            }
            else
            {
                outfitDirSprites = new List<Sprite>(currentNPC.GetComponent<OutfitController>().outfitDict[outfit][2]);
            }
        }
        else
        {
            return;
        }

        transform.Find("Outfit").position = transform.position;
        GetComponent<Image>().sprite = bodyDirSprites[whichCycle + 6];
        transform.Find("Outfit").GetComponent<Image>().sprite = outfitDirSprites[whichCycle + 6];
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
