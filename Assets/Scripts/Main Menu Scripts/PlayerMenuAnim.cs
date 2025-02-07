using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuAnim : MonoBehaviour
{
    public List<Sprite> bodyDirSprites;
    public List<Sprite> outfitDirSprites;
    public string bodyDirSpritesPath;
    public string outfitDirSpritesPath;
    private int whichCycle = 0;
    public PlayerMenu playerMenuScript;
    public ApplyMainMenuData dataScript;

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
        if(playerMenuScript.playerCharacter != null)
        {
            switch (playerMenuScript.playerCharacter)
            {
                case "BaldEagle": bodyDirSprites = dataScript.BaldEagleSprites; break;
                case "BillyGoat": bodyDirSprites = dataScript.BillyGoatSprites; break;
                case "Froseph": bodyDirSprites = dataScript.FrosephSprites; break;
                case "Lifer": bodyDirSprites = dataScript.LiferSprites; break;
                case "Maru": bodyDirSprites = dataScript.MaruSprites; break;
                case "OldTimer": bodyDirSprites = dataScript.OldTimerSprites; break;
                case "Rabbit": bodyDirSprites = dataScript.RabbitSprites; break;
                case "Tango": bodyDirSprites = dataScript.TangoSprites; break;
                case "YoungBuck": bodyDirSprites = dataScript.YoungBuckSprites; break;

            }
            outfitDirSprites = dataScript.InmateOutiftSprites;
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
