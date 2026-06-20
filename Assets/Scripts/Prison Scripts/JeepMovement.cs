using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeepMovement : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> jeepWPs;

    private Vector2 nextWPPos;
    public string currentDir;
    private Transform currentWP; //currentWP is the one that the jeeps movement is currently based off of
    private bool noMove;
    private bool isHitByStinger;
    private Death deathScript;
    private void Start()
    {
        deathScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Death>();
    }
    private void OnEnable()
    {
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame(); //just waiting to make sure this jeep gets its waypoints from Jeeps.cs

        //get initial dir (the jeep is spawned at the first waypoint in jeepWPs)
        currentDir = jeepWPs[0].name.Replace("Jeep", "").ToLower();
        nextWPPos = jeepWPs[0].GetComponent<JeepWaypointConnection>().connectedWP.position;
        currentWP = jeepWPs[0];
        StartCoroutine(JeepMove());
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (isHitByStinger)
        {
            return;
        }

        if (otherCollider.GetComponent<ItemCollectionData>() != null && otherCollider.GetComponent<ItemCollectionData>().itemData != null)
        {
            if (otherCollider.GetComponent<ItemCollectionData>().itemData.id == 138)
            {
                GetComponent<JeepAnimation>().enabled = false;
                noMove = true;
                Destroy(otherCollider.gameObject);
                isHitByStinger = true;
                StartCoroutine(StingerWait());
            }
        }
    }
    private IEnumerator StingerWait()
    {
        yield return new WaitForSeconds(6.5f);
        GetComponent<JeepAnimation>().enabled = true;
        noMove = false;
        isHitByStinger = false;
    }
    private IEnumerator JeepMove()//jeeps move 1 te1 pixel (.1 unity units) per .01 seconds
    {
        while (true)
        {
            if (noMove)
            {
                yield return null;
                continue;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, nextWPPos, 10f * Time.deltaTime);

            if (Vector2.Distance(transform.position, nextWPPos) <= 0.001f)
            {
                transform.position = nextWPPos;
                currentWP = currentWP.GetComponent<JeepWaypointConnection>().connectedWP;
                nextWPPos = currentWP.GetComponent<JeepWaypointConnection>().connectedWP.position;

                currentDir = currentWP.name.Replace("Jeep", "").ToLower();

                switch (currentDir)
                {
                    case "up":
                    case "down":
                        GetComponent<BoxCollider2D>().size = new Vector2(3.2f, 4.8f);
                        break;
                    case "left":
                    case "right":
                        GetComponent<BoxCollider2D>().size = new Vector2(4.8f, 3.2f);
                        break;
                }
                BoxCollider2D killBox = transform.Find("KillBox").GetComponent<BoxCollider2D>();
                switch (currentDir)
                {
                    case "up":
                        killBox.size = new Vector2(3.2f, .1f);
                        killBox.offset = new Vector2(0, 2.45f);
                        break;
                    case "down":
                        killBox.size = new Vector2(3.2f, .1f);
                        killBox.offset = new Vector2(0, -2.45f);
                        break;
                    case "right":
                        killBox.size = new Vector2(.1f, 3.2f);
                        killBox.offset = new Vector2(2.45f, 0);
                        break;
                    case "left":
                        killBox.size = new Vector2(.1f, 3.2f);
                        killBox.offset = new Vector2(-2.45f, 0);
                        break;
                }
            }

            yield return null;
        }
    }
}
