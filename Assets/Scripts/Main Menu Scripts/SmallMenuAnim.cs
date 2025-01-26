using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SmallMenuAnim : MonoBehaviour
{
    public List<Sprite> bodyDirSprites;
    public List<Sprite> outfitDirSprites;
    public string bodyDirSpritesPath;
    public string outfitDirSpritesPath;
    private int whichCycle = 0;
    public SmallMenu smallMenuScript;


    public void OnEnable()
    {
        StartCoroutine(AnimCycle());
    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
    public void Update()
    {
        if(smallMenuScript.npcCharacter != null && smallMenuScript.npcType != null)
        {
            bodyDirSpritesPath = "NPC Sprites/" + smallMenuScript.npcCharacter + "/Movement";
            outfitDirSpritesPath = "NPC Sprites/Outfits/" + smallMenuScript.npcType;
            bodyDirSprites = Resources.LoadAll<Sprite>(bodyDirSpritesPath).ToList();
            outfitDirSprites = Resources.LoadAll<Sprite>(outfitDirSpritesPath).ToList();

            transform.Find("Outfit").position = transform.position;

            GetComponent<Image>().sprite = bodyDirSprites[whichCycle + 6];
            transform.Find("Outfit").GetComponent<Image>().sprite = outfitDirSprites[whichCycle + 6];
        }
    }
    public IEnumerator AnimCycle()
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
