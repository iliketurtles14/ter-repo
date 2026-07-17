using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeeps : MonoBehaviour
{
    private List<string> layers = new List<string>()
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private Transform tiles;
    private List<Transform> jeepGround = new List<Transform>();
    private List<Transform> jeepUnderground = new List<Transform>();
    private List<Transform> jeepVents = new List<Transform>();
    private List<Transform> jeepRoof = new List<Transform>();
    private List<List<Transform>> wpLists = new List<List<Transform>>();
    private List<List<List<Transform>>> wpPaths = new List<List<List<Transform>>>();
    private List<Transform> finishedWPs = new List<Transform>();

    private void Start()
    {
        wpPaths.Add(new List<List<Transform>>());
        wpPaths.Add(new List<List<Transform>>());
        wpPaths.Add(new List<List<Transform>>());
        wpPaths.Add(new List<List<Transform>>());

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
        CollectWaypoints();
        StartCoroutine(ConnectWaypoints());
    }
    private void CollectWaypoints()
    {
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform wp in tiles.Find(layers[i]))
            {
                if (wp.name == "JeepLeft" || wp.name == "JeepDown" || wp.name == "JeepUp" || wp.name == "JeepRight")
                {
                    switch (i)
                    {
                        case 0:
                            jeepUnderground.Add(wp);
                            break;
                        case 1:
                            jeepGround.Add(wp);
                            break;
                        case 2:
                            jeepVents.Add(wp);
                            break;
                        case 3:
                            jeepRoof.Add(wp);
                            break;
                    }
                }
            }
        }
        wpLists.Add(jeepUnderground);
        wpLists.Add(jeepGround);
        wpLists.Add(jeepVents);
        wpLists.Add(jeepRoof);
    }
    private IEnumerator ConnectWaypoints() //this is prolly so unoptimized
    {
        for(int i = 0; i < 4; i++)
        {
            foreach(Transform wp in wpLists[i])
            {
                if(wp.GetComponent<JeepWaypointConnection>().connectedWP != null)
                {
                    continue;
                }
                Vector2 dirVector = new Vector2();
                switch (wp.name)
                {
                    case "JeepUp":
                        dirVector = new Vector2(0, 1.6f);
                        break;
                    case "JeepDown":
                        dirVector = new Vector2(0, -1.6f);
                        break;
                    case "JeepLeft":
                        dirVector = new Vector2(-1.6f, 0);
                        break;
                    case "JeepRight":
                        dirVector = new Vector2(1.6f, 0);
                        break;
                }
                Vector2 currentPos = wp.position;
                bool hasConnected = false;
                while (true)
                {
                    foreach(Transform wpa in wpLists[i])
                    {
                        float distance = Vector2.Distance(currentPos, wpa.position);
                        if (distance < .01f && wpa != wp)
                        {
                            wp.GetComponent<JeepWaypointConnection>().connectedWP = wpa;
                            hasConnected = true;
                            break;
                        }
                    }
                    if (hasConnected)
                    {
                        break;
                    }
                    currentPos += dirVector;
                    yield return null;
                }
            }
        }
        StartCoroutine(SeparateWaypointPaths());
    }
    private IEnumerator SeparateWaypointPaths()
    {
        List<Transform> cycledWPs = new List<Transform>();
        for(int i = 0; i < 4; i++)
        {
            foreach (Transform wp in wpLists[i])
            {
                if (finishedWPs.Contains(wp))
                {
                    continue;
                }

                Transform currentWP = wp;
                cycledWPs.Add(wp);
                while (true)
                {
                    Transform nextWP = currentWP.GetComponent<JeepWaypointConnection>().connectedWP;
                    if (nextWP == null || finishedWPs.Contains(nextWP))
                    {
                        // Stop if it leads to a dead end or an already processed path
                        foreach (Transform a in cycledWPs) finishedWPs.Add(a);
                        cycledWPs.Clear();
                        break;
                    }

                    currentWP = nextWP;

                    if (cycledWPs.Contains(currentWP))
                    {
                        // Found a loop! Only extract the cyclic part
                        int loopStartIndex = cycledWPs.IndexOf(currentWP);
                        List<Transform> actualLoop = cycledWPs.GetRange(loopStartIndex, cycledWPs.Count - loopStartIndex);

                        Debug.Log("Adding a jeep path");
                        wpPaths[i].Add(new List<Transform>(actualLoop));

                        foreach (Transform a in cycledWPs)
                        {
                            finishedWPs.Add(a);
                        }
                        cycledWPs.Clear();
                        break;
                    }
                    cycledWPs.Add(currentWP);
                    yield return null;
                }
            }
        }
        SpawnJeeps();
    }
    private void SpawnJeeps()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < wpPaths[i].Count; j++)
            {
                Debug.Log("Spawning Jeep at layer " + i.ToString());
                GameObject jeep = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Jeep"));
                jeep.GetComponent<JeepMovement>().jeepWPs = wpPaths[i][j]; //holy shit lmao
                jeep.transform.position = wpPaths[i][j][0].position;
                jeep.transform.parent = tiles.Find("GroundObjects");
                jeep.name = "Jeep";
                switch (i)
                {
                    case 0:
                        jeep.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        jeep.GetComponent<SpriteRenderer>().sortingLayerName = "Underground";
                        jeep.layer = LayerMask.NameToLayer("Underground");
                        jeep.GetComponent<Rigidbody2D>().includeLayers = LayerMask.NameToLayer("Underground");
                        jeep.transform.parent = tiles.Find("UndergroundObjects");
                        break;
                        //its already defaulted to ground layer, so no need to check for that
                    case 2:
                        jeep.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        jeep.GetComponent<SpriteRenderer>().sortingLayerName = "Vents";
                        jeep.layer = 8;
                        jeep.GetComponent<Rigidbody2D>().includeLayers = LayerMask.NameToLayer("Vents");
                        jeep.transform.parent = tiles.Find("VentObjects");
                        break;
                    case 3:
                        jeep.GetComponent<SpriteRenderer>().sortingOrder = 2;
                        jeep.GetComponent<SpriteRenderer>().sortingLayerName = "Roof";
                        jeep.layer = 9;
                        jeep.GetComponent<Rigidbody2D>().includeLayers = LayerMask.NameToLayer("Roof");
                        jeep.transform.parent = tiles.Find("RoofObjects");
                        break;
                }
            }
        }
    }
}
