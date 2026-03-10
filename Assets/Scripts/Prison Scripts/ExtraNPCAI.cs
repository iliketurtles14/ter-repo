using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraNPCAI : MonoBehaviour
{
    private Routine routineScript;
    private ApplyPrisonData applyScript;
    private Seeker seeker;
    private Transform jobWP;
    private Transform medicWP;
    private Transform spawnWP;
    private List<Transform> guardWPs = new List<Transform>();
    private Transform tiles;
    private bool alreadyWentToday;
    private int dayWent;
    private void Start()
    {
        //9 - job and medic spawn
        //19 - job leaves
        //22 - medic leaves
        //9 + Random.Range(0, 13) - warden spawn, go to a guard waypoint, go to spawn and leave

        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Time").GetComponent<Routine>();
        seeker = GetComponent<Seeker>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;

        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            switch (obj.name)
            {
                case "NPCSpawnpoint":
                    spawnWP = obj;
                    break;
                case "MedicWaypoint":
                    medicWP = obj;
                    break;
                case "JobWaypoint":
                    jobWP = obj;
                    break;
            }
            if(spawnWP != null && medicWP != null && jobWP != null)
            {
                break;
            }
        }
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "GuardWaypoint")
            {
                guardWPs.Add(obj);
            }
        }

        switch (name)
        {
            case "JobOfficer":
                StartCoroutine(JobOfficerAI());
                break;
            case "Medic":
                StartCoroutine(MedicAI());
                break;
            case "Warden":
                StartCoroutine(WardenAI());
                break;
        }
    }
    private void Update()
    {
        if (dayWent != routineScript.day)
        {
            alreadyWentToday = false;
        }
    }
    private IEnumerator JobOfficerAI()
    {
        while (true)
        {
            while (true)
            {
                if (routineScript.min == 9)
                {
                    break;
                }
                yield return null;
            }

            //spawn
            transform.position = spawnWP.position;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.Find("SpeechCanvas").gameObject.SetActive(true);
            GetComponent<CapsuleCollider2D>().enabled = true;

            //move to job waypoint
            seeker.StartPath(transform.position, jobWP.position);
            while (true)
            {
                float distance = Vector2.Distance(transform.position, jobWP.position);
                if(distance < .01f)
                {
                    seeker.CancelCurrentPathRequest(true);
                    transform.position = jobWP.position;
                    break;
                }
                yield return null;
            }

            //leave at 19
            while (true)
            {
                if(routineScript.min == 19)
                {
                    break;
                }
                yield return null;
            }

            //move to spawn waypoint
            seeker.StartPath(transform.position, spawnWP.position);
            while (true)
            {
                float distance = Vector2.Distance(transform.position, spawnWP.position);
                if(distance < .01f)
                {
                    seeker.CancelCurrentPathRequest(true);
                    transform.position = spawnWP.position;
                    break;
                }
                yield return null;
            }

            //despawn
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            transform.Find("SpeechCanvas").gameObject.SetActive(false);

            yield return null;
        }
    }
    private IEnumerator MedicAI()
    {
        while (true)
        {
            while (true)
            {
                if (routineScript.min == 9)
                {
                    break;
                }
                yield return null;
            }

            //spawn
            transform.position = spawnWP.position;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.Find("SpeechCanvas").gameObject.SetActive(true);
            GetComponent<CapsuleCollider2D>().enabled = true;

            //move to medic waypoint
            seeker.StartPath(transform.position, medicWP.position);
            while (true)
            {
                float distance = Vector2.Distance(transform.position, medicWP.position);
                if (distance < .01f)
                {
                    seeker.CancelCurrentPathRequest(true);
                    transform.position = medicWP.position;
                    break;
                }
                yield return null;
            }

            //leave at 22
            while (true)
            {
                if (routineScript.min == 22)
                {
                    break;
                }
                yield return null;
            }

            //move to spawn waypoint
            seeker.StartPath(transform.position, spawnWP.position);
            while (true)
            {
                float distance = Vector2.Distance(transform.position, spawnWP.position);
                if (distance < .01f)
                {
                    seeker.CancelCurrentPathRequest(true);
                    transform.position = spawnWP.position;
                    break;
                }
                yield return null;
            }

            //despawn
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            transform.Find("SpeechCanvas").gameObject.SetActive(false);

            yield return null;
        }
    }
    private IEnumerator WardenAI()
    {
        while (true)
        {
            if (alreadyWentToday)
            {
                yield return null;
                continue;
            }
            
            int rand = 9 + UnityEngine.Random.Range(0, 13);
            
            while (true)
            {
                if (routineScript.min == rand)
                {
                    alreadyWentToday = true;
                    dayWent = routineScript.day;
                    break;
                }
                yield return null;
            }

            //spawn
            transform.position = spawnWP.position;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.Find("SpeechCanvas").gameObject.SetActive(true);
            GetComponent<CapsuleCollider2D>().enabled = true;

            //move a guard waypoint
            rand = UnityEngine.Random.Range(0, guardWPs.Count);
            Transform wp = guardWPs[rand];
            seeker.StartPath(transform.position, wp.position);
            while (true)
            {
                float distance = Vector2.Distance(transform.position, wp.position);
                if (distance < .01f)
                {
                    seeker.CancelCurrentPathRequest(true);
                    transform.position = wp.position;
                    break;
                }
                yield return null;
            }

            //wait for a random amount of seconds
            float waitTime = UnityEngine.Random.Range(0, 3);
            yield return new WaitForSeconds(waitTime);

            //move to spawn waypoint
            seeker.StartPath(transform.position, spawnWP.position);
            while (true)
            {
                float distance = Vector2.Distance(transform.position, spawnWP.position);
                if (distance < .01f)
                {
                    seeker.CancelCurrentPathRequest(true);
                    transform.position = spawnWP.position;
                    break;
                }
                yield return null;
            }

            //despawn
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            transform.Find("SpeechCanvas").gameObject.SetActive(false);

            yield return null;
        }
    }
}