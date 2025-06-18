using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIDAnimation : MonoBehaviour
{
    private List<Sprite> bodyDirSprites;
    private List<Sprite> outfitDirSprites;
    private int whichCycle = 0;

    private void OnEnable()
    {
        switch (NPCSave.instance.playerCharacter)
        {
            case 1: bodyDirSprites = DataSender.instance.RabbitSprites; break;
            case 2: bodyDirSprites = DataSender.instance.BaldEagleSprites; break;
            case 3: bodyDirSprites = DataSender.instance.LiferSprites; break;
            case 4: bodyDirSprites = DataSender.instance.YoungBuckSprites; break;
            case 5: bodyDirSprites = DataSender.instance.OldTimerSprites; break;
            case 6: bodyDirSprites = DataSender.instance.BillyGoatSprites; break;
            case 7: bodyDirSprites = DataSender.instance.FrosephSprites; break;
            case 8: bodyDirSprites = DataSender.instance.TangoSprites; break;
            case 9: bodyDirSprites = DataSender.instance.MaruSprites; break;
        }
        outfitDirSprites = DataSender.instance.InmateOutfitSprites;
        StartCoroutine(AnimCycle());
    }
    private void Update()
    {
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
