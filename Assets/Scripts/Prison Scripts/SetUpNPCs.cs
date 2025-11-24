using System.Collections;
using UnityEngine;
using Pathfinding;
using NavMeshPlus.Components;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

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
        AstarPath.active.Scan();

        surface.BuildNavMesh();

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
                if(waypoint.name == "InmateWaypoint" || waypoint.name == "GuardWaypoint")
                {
                    waypoints.Add(waypoint);
                }
            }
        }
        
        for(int i = 0; i < npcAmount; i++)
        {
            GameObject npc = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/NPC"));
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
            npc.GetComponent<SpriteRenderer>().size = new Vector2(.16f, .16f);
            //set npc pos randomly
            int rand = UnityEngine.Random.Range(0, waypoints.Count);
            npc.transform.position = waypoints[rand].position;
        }
    }
}
