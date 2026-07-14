using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGetOutfit : MonoBehaviour
{
    private bool isInmate;
    private bool isGuard;
    private bool ready;
    private SpriteRenderer outfitSR;
    private NPCCollectionData npcColData;
    private NPCAI aiScript;
    private Transform tiles;
    private List<Transform> guardBeds = new List<Transform>();
    private bool isGoing;
    private Map currentMap;
    private ItemDataCreator creator;
    private void Start()
    {
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        isInmate = name.Contains("Inmate");
        isGuard = name.Contains("Guard");
        outfitSR = transform.Find("Outfit").GetComponent<SpriteRenderer>();
        npcColData = GetComponent<NPCCollectionData>();
        aiScript = GetComponent<NPCAI>();
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            guardBeds.Add(obj);
        }
        if((isInmate && npcColData.npcData.desk == null) || (isGuard && guardBeds.Count == 0))
        {
            enabled = false;
            StopAllCoroutines();
        }
        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if(npcColData.npcData.isDead || npcColData.npcData.isAggro)
        {
            isGoing = false;
        }

        if(isInmate && !outfitSR.enabled && !npcColData.npcData.isDead && !isGoing)
        {
            isGoing = true;
            StopAllCoroutines();
            StartCoroutine(GoToLoop());
        }
        else if(isGuard && !outfitSR.enabled && !npcColData.npcData.isDead && !isGoing)
        {
            isGoing = true;
            StopAllCoroutines();
            StartCoroutine(GoToLoop());
        }
    }
    private IEnumerator GoToLoop()
    {
        Vector2 goToVector = Vector2.zero;
        Transform goToObj = null;
        List<Vector2> vectors = null;
        if (isInmate)
        {
            vectors = new List<Vector2>
            {
                new Vector2(-1.6f, 0), new Vector2(1.6f, 0), new Vector2(0, 1.6f), new Vector2(0, -1.6f)
            };
            goToObj = npcColData.npcData.desk.transform;
        }
        else if (isGuard)
        {
            vectors = new List<Vector2>
            {
                new Vector2(0, 2.4f), new Vector2(0, -2.4f), new Vector2(1.6f, .8f), new Vector2(1.6f, -.8f), new Vector2(-1.6f, .8f), new Vector2(-1.6f, -.8f)
            };
            int rand = UnityEngine.Random.Range(0, guardBeds.Count);
            goToObj = guardBeds[rand];
        }
        for(int i = 0; i < vectors.Count; i++)
        {
            GameObject checkerObj = new GameObject("CheckerObj");
            checkerObj.AddComponent<BoxCollider2D>().size = new Vector2(.8f, .8f);
            checkerObj.AddComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            checkerObj.GetComponent<BoxCollider2D>().isTrigger = true;
            checkerObj.layer = LayerMask.NameToLayer("Ground");
            checkerObj.transform.position = goToObj.position + new Vector3(vectors[i].x, vectors[i].y, 0);
            yield return new WaitForFixedUpdate();

            Collider2D checkerCollider = checkerObj.GetComponent<BoxCollider2D>();
            List<Collider2D> hitColliders = new List<Collider2D>();
            ContactFilter2D filter = ContactFilter2D.noFilter;
            checkerCollider.Overlap(filter, hitColliders);
            bool hitDigable = false;
            foreach (Collider2D col in hitColliders)
            {
                if (col.CompareTag("Digable"))
                {
                    goToVector = col.transform.position;
                    hitDigable = true;
                    break;
                }
            }
            Destroy(checkerObj);
            if (hitDigable)
            {
                break;
            }
        }
        if (goToVector == Vector2.zero)
        {
            yield break;
        }

        aiScript.SendToPos(goToVector);

        while (true)
        {
            if(Vector2.Distance(transform.position, goToVector) <= .1f)
            {
                break;
            }
            if (!isGoing)
            {
                break;
            }
            yield return null;
        }
        if (!isGoing)
        {
            yield break;
        }

        if (isInmate)
        {
            if (currentMap.powOutfits)
            {
                npcColData.npcData.inventory[7].itemData = creator.CreateItemData(33);
            }
            else
            {
                npcColData.npcData.inventory[7].itemData = creator.CreateItemData(29);
            }
        }
        else if (isGuard)
        {
            npcColData.npcData.inventory[7].itemData = creator.CreateItemData(39);
        }
    }
}
