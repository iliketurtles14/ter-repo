using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawn : MonoBehaviour
{
    private Transform tiles;
    private Transform player;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        SpawnMines();
    }
    private void SpawnMines()
    {
        string renderLayer = GetComponent<SpriteRenderer>().sortingLayerName;
        int layer = gameObject.layer;
        string parent = transform.parent.name;

        List<Vector3> mineOffsets = new List<Vector3>
        {
            new Vector3(-1.6f, 1.6f), new Vector3(0, 1.6f), new Vector3(1.6f, 1.6f),
            new Vector3(-1.6f, 0), new Vector3(0, 0), new Vector3(1.6f, 0),
            new Vector3(-1.6f, -1.6f), new Vector3(0, -1.6f), new Vector3(1.6f, -1.6f)
        };

        int rand1 = UnityEngine.Random.Range(0, 9);
        int rand2 = UnityEngine.Random.Range(0, 9);
        int rand3 = UnityEngine.Random.Range(0, 9);

        List<int> randInts = new List<int>
        {
            rand1, rand2, rand3
        };

        for(int i = 0; i < 3; i++)
        {
            GameObject mine = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/Mine"));
            mine.GetComponent<SpriteRenderer>().sortingLayerName = renderLayer;
            mine.layer = layer;
            mine.transform.parent = tiles.Find(parent);
            mine.transform.position = transform.position + mineOffsets[randInts[i]];
            mine.transform.position -= new Vector3(.1f, .1f, 0); //its offset for some reason
        }

        string parentUp = "lmao";
        switch (transform.parent.name)
        {
            case "UndergroundObjects":
                parentUp = "GroundObjects";
                break;
            case "GroundObjects":
                parentUp = "VentObjects";
                break;
            case "VentObjects":
                parentUp = "RoofObjects";
                break;
            case "RoofObjects":
                parentUp = "lmao";
                break;
        }

        if(parentUp == "lmao")
        {
            return;
        }
        GameObject bigMineObj = new GameObject("BigMine");
        bigMineObj.AddComponent<BoxCollider2D>().isTrigger = true;
        bigMineObj.GetComponent<BoxCollider2D>().size = new Vector2(4.8f, 4.8f);
        bigMineObj.AddComponent<MineBigExplode>();
        bigMineObj.transform.parent = tiles.Find(parentUp);
        bigMineObj.transform.position = transform.position;
    }
}
