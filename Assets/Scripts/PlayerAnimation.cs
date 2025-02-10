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
    private string bodyDirSpritesPath;
    private string outfitDirSpritesPath;
    private int whichCycle;
    private string character;

    public void Start()
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

        StartCoroutine(DirWait());
        StartCoroutine(AnimCycle());
    }
    public void Update()
    {
        transform.Find("Outfit").position = transform.position;

        if(lookDir != null)
        {
            switch (lookDir)
            {
                case "right": lookNum = 0; break;
                case "up": lookNum = 2; break;
                case "left": lookNum = 4; break;
                case "down": lookNum = 6; break;
            }

            GetComponent<SpriteRenderer>().sprite = bodyDirSprites[lookNum + whichCycle];
            transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitDirSprites[lookNum + whichCycle];

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
