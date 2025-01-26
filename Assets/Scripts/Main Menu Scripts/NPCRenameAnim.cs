using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;

public class NPCRenameAnim : MonoBehaviour
{
    public List<Sprite> bodyDirSprites;
    public List<Sprite> outfitDirSprites;
    public string bodyDirSpritesPath;
    private string outfitDirSpritesPath;
    private int whichCycle = 0;


    public void Start()
    {
        OnEnable();
    }
    public void OnEnable()
    {
        if (bodyDirSpritesPath != null)
        {
            outfitDirSpritesPath = "NPC Sprites/Outfits/" + tag + "/Movement";

            bodyDirSprites = Resources.LoadAll<Sprite>(bodyDirSpritesPath).ToList();
            outfitDirSprites = Resources.LoadAll<Sprite>(outfitDirSpritesPath).ToList();

            StartCoroutine(AnimCycle());
        }
        else { OnEnable(); }
    }
    public void Randomize()
    {
        outfitDirSpritesPath = "NPC Sprites/Outfits/" + tag + "/Movement";

        bodyDirSprites = Resources.LoadAll<Sprite>(bodyDirSpritesPath).ToList();
        outfitDirSprites = Resources.LoadAll<Sprite>(outfitDirSpritesPath).ToList();
    }

    public void Update()
    {
        bodyDirSprites = Resources.LoadAll<Sprite>(bodyDirSpritesPath).ToList();

        transform.Find("Outfit").position = transform.position;


        GetComponent<Image>().sprite = bodyDirSprites[6 + whichCycle];
        transform.Find("Outfit").GetComponent<Image>().sprite = outfitDirSprites[6 + whichCycle];
        
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