using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCIDAnimation : MonoBehaviour
{
    public List<Sprite> bodyDirSprites;
    public List<Sprite> outfitDirSprites;
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
            bodyDirSprites = new List<Sprite>(currentNPC.GetComponent<NPCAnimation>().bodyDirSprites);
            outfitDirSprites = new List<Sprite>(currentNPC.GetComponent <NPCAnimation>().outfitDirSprites);
        }


        transform.Find("Outfit").position = transform.position;
        try
        {
            GetComponent<Image>().sprite = bodyDirSprites[whichCycle + 6];
            if (currentNPC.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData.sprite == null)
            {
                outfitDirSprites = new List<Sprite>(clearList);
            }
            transform.Find("Outfit").GetComponent<Image>().sprite = outfitDirSprites[whichCycle + 6];
        }
        catch { }

        if (transform.Find("Outfit").GetComponent<Image>().sprite == null)
        {
            transform.Find("Outfit").GetComponent<Image>().sprite = Resources.Load<Sprite>("Main Menu Resources/UI Stuff/clear");
        }
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
