using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;

public class NPCAnimation : MonoBehaviour
{
    public string xLookDir;
    public string yLookDir;
    public string lookDir;
    private int lookNum;
    private float xDif;
    private float yDif;
    private Vector2 oldPos;
    private Vector2 currentPos;
    public List<Sprite> bodyDirSprites;
    public List<Sprite> outfitDirSprites;
    private int whichCycle = 0;


    public void OnEnable()
    {
        if (CompareTag("Guard"))
        {
            for (int i = 1; i <= 5; i++)
            {
                if (name == "Guard" + i)
                {
                    switch(NPCSave.instance.npcCharacters[i + 8])
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
                }
            }
            outfitDirSprites = DataSender.instance.InmateOutfitSprites;
        }
        else if(CompareTag("Inmate"))
        {
            for(int i = 1; i <= 9; i++)
            {
                if(name == "Inmate" + i)
                {
                    switch (NPCSave.instance.npcCharacters[i - 1])
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
                }
            }
            outfitDirSprites = DataSender.instance.GuardOutfitSprites;
        }
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
            oldPos = transform.position;
            yield return new WaitForEndOfFrame();
            currentPos = transform.position;
            DirGet();
        }
    }
    public void DirGet()
    {
        // Calculate positional differences
        xDif = currentPos.x - oldPos.x;
        yDif = currentPos.y - oldPos.y;

        // Threshold to determine movement
        float threshold = 0.01f;

        // Check if there's enough movement to update direction
        if (Mathf.Abs(xDif) >= threshold || Mathf.Abs(yDif) >= threshold)
        {
            // Always prioritize horizontal movement
            if (Mathf.Abs(xDif) >= threshold)
            {
                lookDir = xDif > 0 ? "right" : "left";
            }
            else if (Mathf.Abs(yDif) >= threshold)
            {
                lookDir = yDif > 0 ? "up" : "down";
            }
        }
        else
        {
            // Default direction when no significant movement
            lookDir = "down";
        }
    }
}
