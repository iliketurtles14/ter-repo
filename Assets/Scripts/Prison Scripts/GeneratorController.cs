using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Transform player;
    public bool genIsOff;
    private Coroutine genWaitCoroutine;
    private Transform tiles;
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

    }
    private void Update()
    {
        if(mcs.isTouchingGenerator && Input.GetMouseButtonDown(0))
        {
            float distance = Vector2.Distance(player.position, mcs.touchedGenerator.transform.position);
            if(distance <= 2.4f)
            {
                FlipGenerator();
            }
        }
    }
    public void FlipGenerator()
    {
        if (genIsOff)
        {
            genIsOff = false;
        }
        else
        {
            if(genWaitCoroutine != null)
            {
                StopCoroutine(genWaitCoroutine);
            }
            genWaitCoroutine = StartCoroutine(GenWait());
            genIsOff = true;
        }
    }
    private IEnumerator GenWait()
    {
        yield return new WaitForSeconds(2000f / 45f);
        genIsOff = false;
    }
}
