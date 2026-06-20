using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrocution : MonoBehaviour
{
    private Transform player;
    private Transform mc;
    private ApplyPrisonData applyScript;
    private Sprite r1;
    private Sprite r2;
    private Sprite l1;
    private Sprite l2;
    private Sprite u1;
    private Sprite u2;
    private Sprite d1;
    private Sprite d2;
    private HPAChecker hpaScript;
    private Death deathScript;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        applyScript = GetComponent<ApplyPrisonData>();
        hpaScript = player.GetComponent<HPAChecker>();
        deathScript = GetComponent<Death>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        r1 = applyScript.NPCSprites[383];
        r2 = applyScript.NPCSprites[386];
        l1 = applyScript.NPCSprites[384];
        l2 = applyScript.NPCSprites[385];
        u1 = Resources.Load<Sprite>("PrisonResources/Sprites/elec3");
        u2 = Resources.Load<Sprite>("PrisonResources/Sprites/elec2");
        d1 = Resources.Load<Sprite>("PrisonResources/Sprites/elec1");
        d2 = Resources.Load<Sprite>("PrisonResources/Sprites/elec4");
    }
    public void Electrocute()
    {
        StopAllCoroutines();
        StartCoroutine(ElectrocuteCoroutine());
    }
    private IEnumerator ElectrocuteCoroutine()
    {
        hpaScript.isBeingElectrocuted = true;
        bool hasOutfit = player.Find("Outfit").GetComponent<SpriteRenderer>().enabled;
        player.GetComponent<PlayerCtrl>().canMove = false;
        player.GetComponent<PlayerAnimation>().enabled = false;
        if (hasOutfit)
        {
            player.Find("Outfit").gameObject.SetActive(false);
        }
        player.GetComponent<PlayerCollectionData>().playerData.health -= 10;
        bool shouldDie = player.GetComponent<PlayerCollectionData>().playerData.health <= 0;

        switch (player.GetComponent<PlayerAnimation>().lookDir) //its like 4:30 in the morning and i dont wanna make this code look nice
        {
            case "up":
                player.GetComponent<SpriteRenderer>().sprite = u1;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = u2;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = u1;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = u2;
                yield return new WaitForSeconds(.117f);
                break;
            case "down":
                player.GetComponent<SpriteRenderer>().sprite = d1;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = d2;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = d1;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = d2;
                yield return new WaitForSeconds(.117f);
                break;
            case "right":
                player.GetComponent<SpriteRenderer>().sprite = r1;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = r2;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = r1;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = r2;
                yield return new WaitForSeconds(.117f);
                break;
            case "left":
                player.GetComponent<SpriteRenderer>().sprite = l1;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = l2;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = l1;
                yield return new WaitForSeconds(.117f);
                player.GetComponent<SpriteRenderer>().sprite = l2;
                yield return new WaitForSeconds(.117f);
                break;
        }
        if (hasOutfit)
        {
            player.Find("Outfit").gameObject.SetActive(true);
        }
        if (shouldDie)
        {
            deathScript.KillPlayer();
        }
        else
        {
            player.GetComponent<PlayerCtrl>().canMove = true;
            player.GetComponent<PlayerAnimation>().enabled = true;
        }
        hpaScript.isBeingElectrocuted = false;
    }
}
