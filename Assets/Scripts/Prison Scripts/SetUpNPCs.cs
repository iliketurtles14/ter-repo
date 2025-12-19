using System.Collections;
using UnityEngine;
using Pathfinding;
using NavMeshPlus.Components;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine.AI;

public class SetUpNPCs : MonoBehaviour
{
    private NavMeshSurface surface;
    private Map map;
    private Transform aStar;
    private Transform tiles;
    private void Start()
    {
        surface = RootObjectCache.GetRoot("NavMesh").GetComponent<NavMeshSurface>();
        aStar = RootObjectCache.GetRoot("A*").transform;
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
        map = GetComponent<LoadPrison>().currentMap;

        NPCSetUp();
    }
    private void NPCSetUp()
    {
        //make navmesh surface and a* surface

        foreach(Transform obj in tiles.Find("GroundObjects")) //make objects like seats that normally have collision have no collision for npc's
        {
            if(obj.name == "Seat" || obj.gameObject.CompareTag("Equipment"))
            {
                obj.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }

        GridGraph grid = AstarPath.active.data.graphs.OfType<GridGraph>().FirstOrDefault();
        grid.center = new Vector3((map.sizeX * 1.6f / 2) - .8f, (map.sizeY * 1.6f / 2) - .8f);
        grid.SetDimensions(map.sizeX, map.sizeY, 1.6f);
        AstarPath.active.Scan();

        surface.BuildNavMesh();

        foreach (Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name == "Seat" || obj.gameObject.CompareTag("Equipment"))
            {
                obj.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }

        //make npc property list
        int inmateAmount = map.inmateCount - 1; //without player
        int guardAmount = map.guardCount;
        int npcAmount = inmateAmount + guardAmount;

        //get freetime waypoints (this is temporary stuff)
        List<Transform> waypoints = new List<Transform>();
        foreach(Transform waypoint in tiles.Find("GroundObjects"))
        {
            if (waypoint.CompareTag("Waypoint"))
            {
                if(waypoint.name == "InmateWaypoint")
                {
                    waypoints.Add(waypoint);
                }
            }
        }
        
        for(int i = 0; i < npcAmount; i++)
        {
            GameObject npc = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/NPC"));
            NPCData data = new NPCData();
            npc.GetComponent<NPCCollectionData>().npcData = data;
            npc.transform.parent = aStar;
            if(i < inmateAmount)
            {
                npc.name = "Inmate" + (i + 1);
            }
            else if(i >= inmateAmount)
            {
                npc.name = "Guard" + (i - inmateAmount + 1);
            }

            npc.GetComponent<NPCCollectionData>().npcData.displayName = NPCSave.instance.npcNames[i];
            npc.GetComponent<NPCCollectionData>().npcData.charNum = NPCSave.instance.npcCharacters[i];

            //npc stats
            if (npc.name.StartsWith("Inmate"))
            {
                npc.GetComponent<NPCCollectionData>().npcData.intellect = UnityEngine.Random.Range(10, 101);
                npc.GetComponent<NPCCollectionData>().npcData.strength = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.speed = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.opinion = 75 - (25 * map.npcLevel) + UnityEngine.Random.Range(0, 51);

                string outfitID;

                if(map.mapName == "Santa's SweatShop")
                {
                    outfitID = "040";
                }
                else if(map.mapName == "Duct Tapes are Forever")
                {
                    outfitID = "045";
                }
                else if(map.mapName == "Escape Team")
                {
                    outfitID = "050";
                }
                else if (map.powOutfits)
                {
                    outfitID = "033";
                }
                else
                {
                    outfitID = "029";
                }
                ItemData outfitData = Instantiate(Resources.Load<ItemData>("Item Scriptable Objects/" + outfitID));
                try
                {
                    npc.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData = outfitData;
                }
                catch { }
            }
            else if (npc.name.StartsWith("Guard"))
            {
                npc.GetComponent<NPCCollectionData>().npcData.intellect = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.strength = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.speed = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.opinion = 15 + UnityEngine.Random.Range(0, 66);
                npc.GetComponent<NPCCollectionData>().npcData.isGuard = true;

                string outfitID;

                if (map.mapName == "Santa's SweatShop")
                {
                    outfitID = "044";
                }
                else if (map.mapName == "Duct Tapes are Forever")
                {
                    outfitID = "049";
                }
                else if (map.mapName == "Escape Team")
                {
                    outfitID = "054";
                }
                else
                {
                    outfitID = "039";
                }
                ItemData outfitData = Instantiate(Resources.Load<ItemData>("Item Scriptable Objects/" + outfitID));
                ItemData batonData = Instantiate(Resources.Load<ItemData>("Item Scriptable Objects/056"));
                try
                {
                    npc.GetComponent<NPCCollectionData>().npcData.inventory[7].itemData = outfitData;
                    npc.GetComponent<NPCCollectionData>().npcData.inventory[6].itemData = batonData;
                }
                catch { }
            }
            npc.GetComponent<NPCCollectionData>().npcData.health = Mathf.FloorToInt(npc.GetComponent<NPCCollectionData>().npcData.strength / 2);

            npc.GetComponent<SpriteRenderer>().size = new Vector2(.16f, .16f);

            npc.GetComponent<NavMeshAgent>().updateRotation = false;
            npc.GetComponent<NavMeshAgent>().updateUpAxis = false;
            //set npc pos randomly
            int rand = UnityEngine.Random.Range(0, waypoints.Count);
            npc.transform.position = waypoints[rand].position;
        }
    }
}
