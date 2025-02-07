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
    public ApplyMainMenuData dataScript;
    public string bodyDirSpritesPath;
    private string outfitDirSpritesPath;
    private int whichCycle = 0;


    public void Start()
    {
        OnEnable();
    }
    public void OnEnable()
    {
        if (bodyDirSprites != null)
        {
            if (tag == "Inmate")
            {
                outfitDirSprites = dataScript.InmateOutiftSprites;
            }
            else if (tag == "Guard")
            {
                outfitDirSprites = dataScript.GuardOutfitSprites;
            }

            StartCoroutine(AnimCycle());
        }
        else { OnEnable(); }
    }
    public void Randomize()
    {
        if (tag == "Inmate")
        {
            outfitDirSprites = dataScript.InmateOutiftSprites;
        }
        else if (tag == "Guard")
        {
            outfitDirSprites = dataScript.GuardOutfitSprites;
        }
    }

    public void Update()
    {
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