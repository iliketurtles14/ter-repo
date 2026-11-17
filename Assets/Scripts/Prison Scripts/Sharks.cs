using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.PlayerLoop;
using static UnityEditor.PlayerSettings;

public class Sharks : MonoBehaviour
{
    private Map currentMap;
    private Transform tiles;
    private List<string> sortingOrders = new List<string>
    {
        "UndergroundVisible", "Ground", "Vents", "Roof"
    };
    private List<string> layerNames = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private List<string> layerNames2 = new List<string>
    {
        "Underground", "Ground", "Vents", "Roof"
    };
    private void Start()
    {
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
        currentMap = GetComponent<LoadPrison>().currentMap;
        SharkSpawn();
    }
    private void SharkSpawn()
    {
        List<string[]> objectProperties = new List<string[]>
        {
            currentMap.undergroundObjectProperties,
            currentMap.groundObjectProperties,
            currentMap.ventObjectProperties,
            currentMap.roofObjectProperties
        };
        List<Transform> undergroundSharkWPs = new List<Transform>();
        List<Transform> groundSharkWPs = new List<Transform>();
        List<Transform> ventSharkWPs = new List<Transform>();
        List<Transform> roofSharkWPs = new List<Transform>();
        List<List<Transform>> sharkWPs = new List<List<Transform>>
        {
            undergroundSharkWPs, groundSharkWPs, ventSharkWPs, roofSharkWPs
        };
        for (int i = 0; i < 4; i++)
        {
            string[] data = objectProperties[i];
            foreach (string str in data)
            {
                if (!str.StartsWith("DTAFShark"))
                {
                    continue;
                }

                string rawData = str.Split("=")[1];
                string rawPos = rawData.Split(";")[0];
                string rawWPData = rawData.Split(";")[1];
                string rawX = rawPos.Split(",")[0];
                float xPos = (Convert.ToSingle(rawX) * 1.6f) - 1.6f;
                string rawY = rawPos.Split(",")[1];
                float yPos = (Convert.ToSingle(rawY) * 1.6f) - 1.6f;
                Vector2 pos = new Vector2(xPos, yPos);
                int id = Convert.ToInt32(rawWPData.Split(",")[0]);
                int order = Convert.ToInt32(rawWPData.Split(",")[1]);
                foreach(Transform obj in tiles.Find(layerNames[i]))
                {
                    if(obj.name == "DTAFSharkWP" && Vector2.Distance(obj.position, pos) <= .1f)
                    {
                        obj.GetComponent<SharkWPData>().id = id;
                        obj.GetComponent<SharkWPData>().order = order;
                        sharkWPs[i].Add(obj);
                    }
                }
            }
            if (sharkWPs[i].Count == 0)
            {
                continue;
            }
            
            List<int> uniqueIDs = new List<int>();
            foreach(Transform obj in sharkWPs[i])
            {
                if (!uniqueIDs.Contains(obj.GetComponent<SharkWPData>().id))
                {
                    uniqueIDs.Add(obj.GetComponent<SharkWPData>().id);
                }
            }
            for(int j = 0; j < uniqueIDs.Count; j++)
            {
                List<Transform> wps = new List<Transform>();
                foreach(Transform obj in sharkWPs[i])
                {
                    if (obj.GetComponent<SharkWPData>().id == uniqueIDs[j])
                    {
                        wps.Add(obj);
                    }
                }
                wps.Sort((a, b) => a.GetComponent<SharkWPData>().order.CompareTo(b.GetComponent<SharkWPData>().order));
                GameObject shark = new GameObject("DTAFShark");
                shark.AddComponent<SpriteRenderer>().sprite = DataSender.instance.PrisonObjectImages[317];
                shark.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
                shark.GetComponent<SpriteRenderer>().size = new Vector2(1.6f, 1.6f);
                shark.GetComponent<SpriteRenderer>().sortingLayerName = sortingOrders[i];
                shark.GetComponent<SpriteRenderer>().sortingOrder = 2;
                shark.AddComponent<SharkAI>().id = uniqueIDs[j];
                shark.GetComponent<SharkAI>().wps = wps;
                shark.AddComponent<BoxCollider2D>().isTrigger = true;
                shark.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 1.6f);
                shark.layer = LayerMask.NameToLayer(layerNames2[i]);
                shark.tag = "DTAFShark";
                shark.transform.parent = tiles.Find(layerNames[i]);
                shark.transform.position = Vector2.zero;
            }
        }
    }
}