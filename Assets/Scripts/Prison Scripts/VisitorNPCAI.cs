using NUnit.Framework;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorNPCAI : MonoBehaviour
{
    private Sittables sittablesScript;
    private Transform tiles;
    private Vector2 spawnPos = new Vector2(0, 0);
    private bool visitorIsGoing;
    private List<GameObject> visitorNPCChairs = new List<GameObject>();
    private Transform player;
    private Seeker seeker;
    private NPCSpeech npcSpeechScript;
    private bool shouldLeave; //this is for when the player just gets up
    public string dirToLook = "any";
    private void Start()
    {
        sittablesScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Sittables>();
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        player = RootObjectCache.GetRoot("Player").transform;
        seeker = GetComponent<Seeker>();
        npcSpeechScript = GetComponent<NPCSpeech>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "VisitorNPC")
            {
                visitorNPCChairs.Add(obj.gameObject);
            }
        }

        if(visitorNPCChairs.Count == 0)
        {
            Destroy(gameObject);
        }

        GetSpawnPos();

        GetComponent<VisitorNPCAnimation>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("SpeechCanvas").gameObject.SetActive(false);
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
    private void GetSpawnPos()
    {
        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "NPCSpawnpoint")
            {
                spawnPos = obj.position;
                break;
            }
        }
    }
    private void Update()
    {
        if(sittablesScript.onSittable && sittablesScript.sittable.name == "VisitorPlayer" && !visitorIsGoing)
        {
            visitorIsGoing = true;
            StartCoroutine(VisitorAI());
        }
        if(visitorIsGoing && !sittablesScript.onSittable)
        {
            shouldLeave = true;
        }
    }
    private IEnumerator VisitorAI()
    {
        //spawn
        GetComponent<VisitorNPCAnimation>().enabled = true;
        yield return new WaitForEndOfFrame();
        transform.position = spawnPos;
        GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("SpeechCanvas").gameObject.SetActive(true);
        GetComponent<CapsuleCollider2D>().enabled = true;

        //move to chair
        GameObject chair = GetClosestChair();
        seeker.StartPath(transform.position, chair.transform.position);
        while (true)
        {
            float distance = Vector2.Distance(transform.position, chair.transform.position);
            if(distance < .01f)
            {
                seeker.CancelCurrentPathRequest(true);
                transform.position = chair.transform.position;
                break;
            }
            yield return null;
        }

        dirToLook = chair.GetComponent<WaypointData>().dir;

        //talk
        float time = 0f;
        while(time < 1f && !shouldLeave)
        {
            time += Time.deltaTime;
            yield return null;
        }
        if (!shouldLeave)
        {
            StartCoroutine(npcSpeechScript.MakeTextBox(npcSpeechScript.GetMessage("Vis_Greet"), transform, false));
        }
        time = 0f;
        while (time < 5f && !shouldLeave)
        {
            time += Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < 3; i++)
        {
            if (!shouldLeave)
            {
                StartCoroutine(npcSpeechScript.MakeTextBox(npcSpeechScript.GetMessage("Vis_Banter"), transform, false));
            }
            time = 0f;
            while (time < 5f && !shouldLeave)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
        if (!shouldLeave)
        {
            StartCoroutine(npcSpeechScript.MakeTextBox(npcSpeechScript.GetMessage("Vis_Leave"), transform, false));
        }
        time = 0f;
        while (time < 3f && !shouldLeave)
        {
            time += Time.deltaTime;
            yield return null;
        }
        dirToLook = "any";
        //go back to spawn
        seeker.StartPath(transform.position, spawnPos);
        while (true)
        {
            float distance = Vector2.Distance(transform.position, spawnPos);
            if(distance < .01f)
            {
                seeker.CancelCurrentPathRequest(true);
                transform.position = spawnPos;
                break;
            }
            yield return null;
        }

        //disappear
        GetComponent<VisitorNPCAnimation>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("SpeechCanvas").gameObject.SetActive(false);
        GetComponent<CapsuleCollider2D>().enabled = false;

        visitorIsGoing = false;
        shouldLeave = false;
    }
    private GameObject GetClosestChair()
    {
        List<float> distances = new List<float>();
        foreach(GameObject chair in visitorNPCChairs)
        {
            distances.Add(Vector2.Distance(chair.transform.position, player.position));
        }
        float lowestDistance = 0f;
        foreach(float distance in distances)
        {
            if(lowestDistance == 0f)
            {
                lowestDistance = distance;
            }
            else if(distance < lowestDistance)
            {
                lowestDistance = distance;
            }
        }
        GameObject closestChair = null;
        foreach(GameObject chair in visitorNPCChairs)
        {
            float distance = Vector2.Distance(player.position, chair.transform.position);
            if(Mathf.Abs(distance - lowestDistance) < .01f)
            {
                closestChair = chair;
                break;
            }
        }
        return closestChair;
    }
}
