using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBodyController : MonoBehaviour
{
    public BodyController bodyControllerScript;
    public ItemBehaviours itemBehavioursScript;
    public int currentActionNum;
    public string character;
    private bool doneWaiting;
    private int order;
    public Dictionary<string, List<List<Sprite>>> characterDict = new Dictionary<string, List<List<Sprite>>>();

    public void Start()
    {
        characterDict = bodyControllerScript.characterDict;

        StartCoroutine(WaitForOrder());
    }
    public IEnumerator WaitForOrder()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        order = GetComponent<NPCCollectionData>().npcData.order;
        doneWaiting = true;
    }
    public void Update()
    {
        if (!doneWaiting)
        {
            return;
        }

        currentActionNum = 2;
        //add more action num things for npcs later (since there is only walking)

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        switch (NPCSave.instance.npcCharacters[order])
        {
            case 1: character = "Rabbit"; break;
            case 2: character = "BaldEagle"; break;
            case 3: character = "Lifer"; break;
            case 4: character = "YoungBuck"; break;
            case 5: character = "OldTimer"; break;
            case 6: character = "BillyGoat"; break;
            case 7: character = "Froseph"; break;
            case 8: character = "Tango"; break;
            case 9: character = "Maru"; break;
            case 10: character = "Buddy"; break;
            case 11: character = "IceElf"; break;
            case 12: character = "BlackElf"; break;
            case 13: character = "YellowElf"; break;
            case 14: character = "PinkElf"; break;
            case 15: character = "OrangeElf"; break;
            case 16: character = "BrownElf"; break;
            case 17: character = "WhiteElf"; break;
            case 18: character = "Genie"; break;
            case 19: character = "GuardElf"; break;
            case 20: character = "Connelly"; break;
            case 21: character = "Elbrah"; break;
            case 22: character = "Chen"; break;
            case 23: character = "Piers"; break;
            case 24: character = "Mourn"; break;
            case 25: character = "Lazeeboi"; break;
            case 26: character = "Blonde"; break;
            case 27: character = "Walton"; break;
            case 28: character = "Prowler"; break;
            case 29: character = "Crane"; break;
            case 30: character = "Henchman"; break;
            case 31: character = "Clint"; break;
            case 32: character = "Cage"; break;
            case 33: character = "Sean"; break;
            case 34: character = "Andy"; break;
            case 35: character = "Soldier"; break;
        }

        try
        {
            GetComponent<NPCAnimation>().bodyDirSprites = characterDict[character][currentActionNum];
        }
        catch { }
        sr.size = new Vector2((sr.sprite.rect.width / sr.sprite.pixelsPerUnit) * 10, (sr.sprite.rect.height / sr.sprite.pixelsPerUnit) * 10);
    }
}
