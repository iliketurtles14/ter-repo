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
    private PauseController pc;
    private List<string> objLayers = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        pc = GetComponent<PauseController>();

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
            for(int i = 0; i < 4; i++)
            {
                foreach(Transform obj in tiles.Find(objLayers[i]))
                {
                    if(obj.name == "Camera")
                    {
                        obj.GetComponent<CameraController>().TurnOnCam();
                    }
                }
            }
        }
        else
        {
            if(genWaitCoroutine != null)
            {
                StopCoroutine(genWaitCoroutine);
            }
            genWaitCoroutine = StartCoroutine(GenWait());
            genIsOff = true;
            for (int i = 0; i < 4; i++)
            {
                foreach (Transform obj in tiles.Find(objLayers[i]))
                {
                    if (obj.name == "Camera")
                    {
                        StartCoroutine(obj.GetComponent<CameraController>().TurnOffCam(1, true));
                    }
                }
            }
        }
    }
    private IEnumerator GenWait()
    {
        float time = 0f;
        while(time <= 2000f / 45f)
        {
            if (pc.isPaused)
            {
                yield return null;
                continue;
            }
            time += Time.deltaTime;
            yield return null;
        }
        genIsOff = false;
        for (int i = 0; i < 4; i++)
        {
            foreach (Transform obj in tiles.Find(objLayers[i]))
            {
                if (obj.name == "Camera")
                {
                    obj.GetComponent<CameraController>().TurnOnCam();
                }
            }
        }
    }
}
