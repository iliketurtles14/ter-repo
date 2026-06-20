using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTAFComputerAnimation : MonoBehaviour
{
    private Sprite left1;
    private Sprite left2;
    private Sprite down1;
    private Sprite down2;
    private Sprite right1;
    private Sprite right2;
    private List<Sprite> sprites;
    private ApplyPrisonData applyScript;
    private void OnEnable()
    {
        applyScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<ApplyPrisonData>();
        left1 = applyScript.PrisonObjectSprites[312];
        left2 = applyScript.PrisonObjectSprites[316];
        down1 = applyScript.PrisonObjectSprites[313];
        down2 = applyScript.PrisonObjectSprites[314];
        right1 = applyScript.PrisonObjectSprites[311];
        right2 = applyScript.PrisonObjectSprites[315];

        switch (name)
        {
            case "DTAFComputerLeft":
                sprites = new List<Sprite>()
                {
                    left1, left2
                };
                break;
            case "DTAFComputerDown":
                sprites = new List<Sprite>()
                {
                    down1, down2
                };
                break;
            case "DTAFComputerRight":
                sprites = new List<Sprite>()
                {
                    right1, right2
                };
                break;
        }

        StartCoroutine(AnimCycle());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator AnimCycle()
    {
        while (true)
        {
            //.55
            GetComponent<SpriteRenderer>().sprite = sprites[0];
            yield return new WaitForSeconds(.55f);
            GetComponent<SpriteRenderer>().sprite = sprites[1];
            yield return new WaitForSeconds(.55f);
        }
    }
}
