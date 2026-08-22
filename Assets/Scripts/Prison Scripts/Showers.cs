using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showers : MonoBehaviour
{
    private bool isOn;
    private BoxCollider2D bc;
    private PauseController pc;
    private List<Transform> npcsInShower = new List<Transform>();
    private bool playerIsInShower;
    private Transform player;
    private Transform aStar;
    private SpriteRenderer sr;
    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        player = RootObjectCache.GetRoot("Player").transform;
        aStar = RootObjectCache.GetRoot("A*").transform;
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(ShowerAnim());
    }
    private void FixedUpdate()
    {
        List<Collider2D> hitCols = new List<Collider2D>();
        ContactFilter2D filter = ContactFilter2D.noFilter;
        bc.Overlap(filter, hitCols);
        npcsInShower.Clear();
        bool shouldTurnOn = false;
        foreach(Collider2D col in hitCols)
        {
            if (col.gameObject.name.Contains("Inmate") && col.GetComponent<NPCCollectionData>() != null && col.gameObject.layer == gameObject.layer)
            {
                if (!npcsInShower.Contains(col.transform))
                {
                    shouldTurnOn = true;
                    npcsInShower.Add(col.transform);
                }
            }
            else if(col.gameObject.name == "Player" && !Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), col.gameObject.layer))
            {
                playerIsInShower = true;
                shouldTurnOn = true;
            }
        }

        isOn = shouldTurnOn;

        if(npcsInShower.Count > 0)
        {
            foreach(Transform npc in npcsInShower)
            {
                npc.Find("Outfit").GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        foreach(Transform npc in aStar)
        {
            if (!npcsInShower.Contains(npc) && npc.name.Contains("Inmate"))
            {
                if(npc.GetComponent<OutfitController>().currentOutfitID != -1)
                {
                    npc.Find("Outfit").GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }

        if (playerIsInShower)
        {
            player.Find("Outfit").GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            if(player.GetComponent<OutfitController>().currentOutfitID != -1)
            {
                player.Find("Outfit").GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
    private IEnumerator ShowerAnim()
    {
        List<Sprite> sprList = new List<Sprite>();
        DataSender ds = DataSender.instance;
        sprList.Add(ds.PrisonObjectImages[57]);
        sprList.Add(ds.PrisonObjectImages[58]);
        sprList.Add(ds.PrisonObjectImages[59]);
        while (true)
        {
            if (!isOn)
            {
                yield return null;
                sr.enabled = false;
                continue;
            }
            sr.enabled = true;

            for(int i = 0; i < 3; i++)
            {
                sr.sprite = sprList[i];
                float time = 0f;
                while(time <= .067f)
                {
                    if (pc.isPaused)
                    {
                        yield return null;
                        continue;
                    }
                    if (!isOn)
                    {
                        break;
                    }
                    yield return null;
                    time += Time.deltaTime;
                }
                if (!isOn)
                {
                    yield return null;
                    continue;
                }
            }
            yield return null;
        }
    }
}
