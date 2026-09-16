using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReindeerAI : MonoBehaviour
{
    private PauseController pc;
    public float speed;
    public bool shouldMove;
    private ReindeerAnimation animScript;
    private void Start()
    {
        animScript = GetComponent<ReindeerAnimation>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        StartCoroutine(AILoop());
    }
    private IEnumerator AILoop()
    {
        while (true)
        {
            float time = 0f;
            float randTime = UnityEngine.Random.Range(1f, 3f);
            while (time < randTime)
            {
                if (pc.isPaused || !shouldMove)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }

            List<string> dirs = new List<string>
            {
                "up", "right", "down", "left"
            };
            int rand = UnityEngine.Random.Range(0, dirs.Count);
            string randDir = dirs[rand];

            int randSteps = UnityEngine.Random.Range(3, 6);
            float distanceToTravel = randSteps * .3f;
            Vector3 travelPosFromDeer = Vector2.zero;
            switch (randDir)
            {
                case "up":
                    travelPosFromDeer = new Vector2(0, distanceToTravel);
                    break;
                case "right":
                    travelPosFromDeer = new Vector2(distanceToTravel, 0);
                    break;
                case "down":
                    travelPosFromDeer = new Vector2(0, -distanceToTravel);
                    break;
                case "left":
                    travelPosFromDeer = new Vector2(-distanceToTravel, 0);
                    break;
            }

            //check for collision
            GameObject checkerObj = new GameObject("Checker");
            checkerObj.AddComponent<BoxCollider2D>().isTrigger = true;
            checkerObj.GetComponent<BoxCollider2D>().size = new Vector2(1.6f, 1.6f);
            checkerObj.transform.position = transform.position + travelPosFromDeer;

            yield return new WaitForFixedUpdate();

            List<Collider2D> hitCols = new List<Collider2D>();
            ContactFilter2D filter = ContactFilter2D.noFilter;
            checkerObj.GetComponent<BoxCollider2D>().Overlap(filter, hitCols);
            bool hitSomething = false;
            foreach (Collider2D col in hitCols)
            {
                if (col.gameObject.layer == gameObject.layer && !col.isTrigger)
                {
                    hitSomething = true;
                    break;
                }
            }
            Destroy(checkerObj);
            if (hitSomething)
            {
                yield return null;
                continue;
            }

            //move
            animScript.lookDir = randDir;
            Vector3 goToPos = transform.position + travelPosFromDeer;
            while (true)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                if (!shouldMove)
                {
                    break;
                }
                
                float dist = Vector2.Distance(transform.position, goToPos);
                if (dist <= .01f)
                {
                    break;
                }

                float distToMove = Time.deltaTime * speed;
                bool missedWaypoint = false;
                switch (randDir)
                {
                    case "up":
                        missedWaypoint = transform.position.y > goToPos.y;
                        transform.position += new Vector3(0, distToMove);
                        break;
                    case "down":
                        missedWaypoint = transform.position.y < goToPos.y;
                        transform.position += new Vector3(0, -distToMove);
                        break;
                    case "left":
                        missedWaypoint = transform.position.x < goToPos.x;
                        transform.position += new Vector3(-distToMove, 0);
                        break;
                    case "right":
                        missedWaypoint = transform.position.x > goToPos.x;
                        transform.position += new Vector3(distToMove, 0);
                        break;
                }
                if (missedWaypoint)
                {
                    break;
                }
                yield return null;
            }
            if (!shouldMove)
            {
                yield return null;
                continue;
            }
            transform.position = goToPos;
            yield return null;
        }
    }
}
