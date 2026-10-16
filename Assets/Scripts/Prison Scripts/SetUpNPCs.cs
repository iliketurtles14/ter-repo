using JetBrains.Annotations;
using NavMeshPlus.Components;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SetUpNPCs : MonoBehaviour
{
    private NavMeshSurface surface;
    private Map map;
    private Transform aStar;
    private Transform tiles;
    private ItemDataCreator creator;
    private NPCSleep sleepScript;
    private SetInmateCorrelations setScript;
    private void Start()
    {
        surface = RootObjectCache.GetRoot("NavMesh").GetComponent<NavMeshSurface>();
        aStar = RootObjectCache.GetRoot("A*").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        creator = RootObjectCache.GetRoot("ScriptObject").GetComponent<ItemDataCreator>();
        sleepScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<NPCSleep>();
        setScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<SetInmateCorrelations>();

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        map = GetComponent<LoadPrison>().currentMap;

        StartCoroutine(NPCSetUp());
    }
    private IEnumerator NPCSetUp()
    {
        bool isLoading = false;
        if(DataSender.instance.currentSave != -1 && File.Exists(System.IO.Path.Combine(Application.streamingAssetsPath, "Saves", "Save" + DataSender.instance.currentSave.ToString() + ".ini")))
        {
            isLoading = true;
        }
        
        //make navmesh surface and a* surface

        foreach(Transform obj in tiles.Find("GroundObjects")) //make objects like seats that normally have collision have no collision for npc's
        {                 //REMEMBER TO ALSO ADD THIS TO THE LOWER FOREACH LOOP IF ADDING NEW OBJECTS THAT SHOULDNT HAVE COLLISION WITH INMATES
            if (obj.name == "Seat" || obj.gameObject.CompareTag("Equipment") || obj.gameObject.CompareTag("Door") || obj.name.Contains("Bed"))
            {
                obj.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }

        GridGraph[] grids = AstarPath.active.data.graphs.OfType<GridGraph>().ToArray();
        for(int i = 0; i < grids.Count(); i++)
        {
            grids[i].center = new Vector3((map.sizeX * 1.6f / 2) - .8f, (map.sizeY * 1.6f / 2) - .8f);
            grids[i].SetDimensions(map.sizeX, map.sizeY, 1.6f);
            AstarPath.active.Scan(grids[i]);
        }

        surface.BuildNavMesh();

        foreach (Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name == "Seat" || obj.gameObject.CompareTag("Equipment") || obj.gameObject.CompareTag("Door") || obj.name.Contains("Bed"))
            {
                obj.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }

        //make npc property list
        int inmateAmount = map.inmateCount - 1; //without player
        int guardAmount = map.guardCount;
        int npcAmount = inmateAmount + guardAmount;

        //get freetime guard waypoints (this is NOT temporary stuff) heh
        List<Transform> gWaypoints = new List<Transform>();
        foreach(Transform waypoint in tiles.Find("GroundObjects"))
        {
            if (waypoint.CompareTag("Waypoint"))
            {
                if(waypoint.name == "GuardWaypoint")
                {
                    gWaypoints.Add(waypoint);
                }
            }
        }

        //list of available jobs
        List<string> availableJobs = new List<string>();
        if (map.janitor)
        {
            availableJobs.Add("Janitor");
        }
        if (map.gardening)
        {
            availableJobs.Add("Gardening");
        }
        if (map.library)
        {
            availableJobs.Add("Library");
        }
        if (map.mailman)
        {
            availableJobs.Add("Mailman");
        }
        if (map.deliveries)
        {
            availableJobs.Add("Deliveries");
        }
        if (map.metalshop)
        {
            availableJobs.Add("Metalshop");
        }
        if (map.woodshop)
        {
            availableJobs.Add("Woodshop");
        }
        if (map.tailor)
        {
            availableJobs.Add("Tailor");
        }
        if (map.laundry)
        {
            availableJobs.Add("Laundry");
        }
        if (map.kitchen)
        {
            availableJobs.Add("Kitchen");
        }
        if (availableJobs.Contains(map.startingJob))
        {
            availableJobs.Remove(map.startingJob);
        }

        //set set waypoint stuff
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "InmateSet" || obj.name == "GuardSet")
            {
                foreach(string line in map.groundObjectProperties)
                {
                    if (!line.Contains("="))
                    {
                        continue;
                    }

                    string objName = line.Split("=")[0];
                    if(objName == obj.name)
                    {
                        string rawPos = line.Split("=")[1].Split(";")[0];
                        float posX = Convert.ToSingle(rawPos.Split(",")[0]) * 1.6f - 1.6f;
                        float posY = Convert.ToSingle(rawPos.Split(",")[1]) * 1.6f - 1.6f;
                        Vector2 pos = new Vector2(posX, posY);
                        if(Vector2.Distance(pos, obj.position) <= .01f)
                        {
                            string rawData = line.Split(";")[1];
                            int id = Convert.ToInt32(rawData.Split(",")[0]);
                            int order = Convert.ToInt32(rawData.Split(",")[1]);
                            obj.GetComponent<SetWaypointHandler>().id = id;
                            obj.GetComponent<SetWaypointHandler>().order = order;
                            break;
                        }
                    }
                }
            }
        }

        //determine what inmates and guards should go to set waypoints if any
        List<int> inmateSetIDs = new List<int>();
        List<int> guardSetIDs = new List<int>();
        List<int> inmateIndexesToBeSet = new List<int>();
        List<int> guardIndexesToBeSet = new List<int>();
        foreach (Transform obj in tiles.Find("GroundObjects"))
        {
            if (obj.name == "InmateSet" && !inmateSetIDs.Contains(obj.GetComponent<SetWaypointHandler>().id))
            {
                inmateSetIDs.Add(obj.GetComponent<SetWaypointHandler>().id);
            }
            else if (obj.name == "GuardSet" && !guardSetIDs.Contains(obj.GetComponent<SetWaypointHandler>().id))
            {
                guardSetIDs.Add(obj.GetComponent<SetWaypointHandler>().id);
            }
        }

        foreach(int num in guardSetIDs)
        {
            Debug.Log(num);
        }

        for(int i = 0; i < inmateSetIDs.Count; i++)
        {
            inmateIndexesToBeSet.Add(i);
        }
        for(int i = 3; i < guardSetIDs.Count; i++)
        {
            guardIndexesToBeSet.Add(i);
        }
        if(guardIndexesToBeSet.Count != guardSetIDs.Count)
        {
            int diff = guardSetIDs.Count - guardIndexesToBeSet.Count;
            for(int i = 0; i < diff && i < 3; i++)  
            {
                guardIndexesToBeSet.Add(i);
            }
        }

        foreach(int num in guardIndexesToBeSet)
        {
            Debug.Log(num);
        }

        for (int i = 0; i < npcAmount; i++)
        {
            bool isSet = false;
            GameObject npc = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/NPC"));
            NPCData data = new NPCData();
            npc.GetComponent<NPCCollectionData>().npcData = data;
            npc.transform.parent = aStar;
            if (i < inmateAmount)
            {
                npc.name = "Inmate" + (i + 1);
            }
            else if (i >= inmateAmount)
            {
                npc.name = "Guard" + (i - inmateAmount + 1);
            }
            if (!isLoading)
            {
                npc.GetComponent<NPCCollectionData>().npcData.displayName = NPCSave.instance.npcNames[i].Replace("\n", "").Replace("\r", "");
                npc.GetComponent<NPCCollectionData>().npcData.charNum = NPCSave.instance.npcCharacters[i];
            }

            //npc stats
            if (npc.name.StartsWith("Inmate"))
            {
                npc.GetComponent<NPCCollectionData>().npcData.intellect = UnityEngine.Random.Range(10, 101);
                npc.GetComponent<NPCCollectionData>().npcData.strength = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.speed = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.opinion = 75 - (25 * map.npcLevel) + UnityEngine.Random.Range(0, 51);
            }
            else if (npc.name.StartsWith("Guard"))
            {
                npc.GetComponent<NPCCollectionData>().npcData.intellect = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.strength = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.speed = UnityEngine.Random.Range(0, 26) + (map.npcLevel * 25);
                npc.GetComponent<NPCCollectionData>().npcData.opinion = 15 + UnityEngine.Random.Range(0, 66);
                npc.GetComponent<NPCCollectionData>().npcData.isGuard = true;
            }
            npc.GetComponent<NPCCollectionData>().npcData.health = Mathf.FloorToInt(npc.GetComponent<NPCCollectionData>().npcData.strength / 2);

            //npc jobs
            int rand;
            if(availableJobs.Count > 0)
            {
                rand = UnityEngine.Random.Range(0, availableJobs.Count);
                npc.GetComponent<NPCCollectionData>().npcData.job = availableJobs[rand];
                availableJobs.RemoveAt(rand);
            }
            npc.GetComponent<SpriteRenderer>().size = new Vector2(.16f, .16f);

            npc.GetComponent<NavMeshAgent>().updateRotation = false;
            npc.GetComponent<NavMeshAgent>().updateUpAxis = false;

            //do set waypoint stuff
            if (npc.name.StartsWith("Inmate"))
            {
                if (inmateIndexesToBeSet.Contains(i))
                {
                    npc.GetComponent<NPCAI>().hasSetPath = true;
                    npc.GetComponent<NPCAI>().setPathID = inmateSetIDs[i];
                    Debug.Log(npc.name + " is set.");
                    isSet = true;
                }
            }
            else if (npc.name.StartsWith("Guard"))
            {
                if (guardIndexesToBeSet.Contains(i - inmateAmount))
                {
                    npc.GetComponent<NPCAI>().hasSetPath = true;
                    npc.GetComponent<NPCAI>().setPathID = guardSetIDs[i - inmateAmount];
                    Debug.Log(npc.name + " is set.");
                    isSet = true;
                }
            }

            if (isSet && !isLoading)
            {
                Vector2 posToGo = Vector2.zero;
                foreach(Transform obj in tiles.Find("GroundObjects"))
                {
                    if(npc.name.StartsWith("Inmate") && obj.name == "InmateSet" &&
                        npc.GetComponent<NPCAI>().setPathID == obj.GetComponent<SetWaypointHandler>().id)
                    {
                        posToGo = obj.position;
                        break;
                    }
                    else if(npc.name.StartsWith("Guard") && obj.name == "GuardSet" &&
                        npc.GetComponent<NPCAI>().setPathID == obj.GetComponent<SetWaypointHandler>().id)
                    {
                        posToGo = obj.position;
                        break;
                    }
                }
                npc.transform.position = posToGo;
            }

            //set guard pos randomly
            if (!isLoading && !isSet) //guard pos on loading is set further down with the inmates
            {
                if (npc.name.Contains("Guard") && gWaypoints.Count > 0)
                {
                    rand = UnityEngine.Random.Range(0, gWaypoints.Count);
                    npc.transform.position = gWaypoints[rand].position;
                }
            }

            //other
            if (npc.name.Contains("Guard"))
            {
                npc.transform.Find("IconCanvas").gameObject.SetActive(false);
            }

            Vector3 zFightVector = npc.transform.position;
            zFightVector.z = i;
            npc.transform.position = zFightVector;
        }

        //set inmate correlations
        setScript.Set();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        //send inmates to beds
        if (!isLoading)
        {
            foreach (Transform npc in aStar)
            {
                if (npc.name.StartsWith("Inmate") && !npc.GetComponent<NPCAI>().hasSetPath)
                {
                    if (npc.GetComponent<NPCCollectionData>().npcData.bed != null)
                    {
                        sleepScript.Sleep(npc.gameObject, npc.GetComponent<NPCCollectionData>().npcData.bed);
                    }
                }
            }
        }
        else
        {
            string[] saveFile = File.ReadAllLines(System.IO.Path.Combine(Application.streamingAssetsPath, "Saves", "Save" + DataSender.instance.currentSave.ToString() + ".ini"));
            foreach(Transform npc in aStar)
            {
                string posStr = GetINIVar(npc.name, "Position", saveFile);
                float x = Convert.ToSingle(posStr.Split(",")[0]);
                float y = Convert.ToSingle(posStr.Split(",")[1]);
                Vector2 pos = new Vector2(x, y);
                npc.position = pos;
            }
        }

        //extra npc stuff
        bool spawnExists = false;
        bool jobExists = false;
        bool medicExists = false;
        bool wardenExists = false;
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            switch (obj.name)
            {
                case "NPCSpawnpoint":
                    spawnExists = true;
                    break;
                case "JobWaypoint":
                    jobExists = true;
                    break;
                case "MedicWaypoint":
                    medicExists = true;
                    break;
                case "GuardWaypoint":
                    wardenExists = true;
                    break;
            }
        }

        if (spawnExists)
        {
            if (jobExists)
            {
                GameObject jobOfficer = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/ExtraNPC"));
                jobOfficer.name = "JobOfficer";
                jobOfficer.transform.parent = aStar;
            }
            if (medicExists)
            {
                GameObject medic = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/ExtraNPC"));
                medic.name = "Medic";
                medic.transform.parent = aStar;
            }
            if (wardenExists)
            {
                GameObject warden = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/ExtraNPC"));
                warden.name = "Warden";
                warden.transform.parent = aStar;
            }
        }
        GameObject visitorNPC = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/VisitorNPC"));
        visitorNPC.name = "VisitorNPC";
        visitorNPC.transform.parent = aStar;
    }
    public string GetINIVar(string header, string varName, string[] file)
    {
        string line = null;

        for (int i = 0; i < file.Length; i++)
        {
            if (file[i].Contains(header) && file[i].Contains('[') && file[i].Contains(']'))
            {
                for (int j = i; j < file.Length; j++)
                {
                    if (file[j].Contains("[") && file[j].Contains("]") && j != i)
                    {
                        line = null;
                        break;
                    }
                    if (file[j].Split('=')[0] == varName)
                    {
                        line = file[j];
                        break;
                    }
                }
                break;
            }
        }



        if (line == null)
        {
            return null;
        }

        string[] parts = line.Split('=');
        return parts[1];
    }
}

