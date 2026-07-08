using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JeepSee : MonoBehaviour
{
    private Vector2 upVector;
    private Vector2 downVector;
    private Vector2 leftVector;
    private Vector2 rightVector;
    private List<Vector2> vectors;
    private float rangeOfSight = 10.4f;
    private Zones zonesScript;
    private Schedule routineScript;
    private Solitary solitaryScript;
    private void Start()
    {
        routineScript = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("Period").GetComponent<Schedule>();
        Transform so = RootObjectCache.GetRoot("ScriptObject").transform;
        zonesScript = so.GetComponent<Zones>();
        solitaryScript = so.GetComponent<Solitary>();
        MakeVectors();
        StartCoroutine(LookLoop());
    }
    private void MakeVectors()
    {
        upVector = new Vector2(0, 1);
        downVector = new Vector2(0, -1);
        leftVector = new Vector2(-1, 0);
        rightVector = new Vector2(1, 0);
        vectors = new List<Vector2>
        {
            upVector, downVector, leftVector, rightVector,
        };
    }
    private IEnumerator LookLoop()
    {
        while (true)
        {
            Vector2 vectorDontUse = Vector2.zero;
            switch (GetComponent<JeepMovement>().currentDir)
            {
                case "up":
                    vectorDontUse = downVector;
                    break;
                case "down":
                    vectorDontUse = upVector;
                    break;
                case "left":
                    vectorDontUse = rightVector;
                    break;
                case "right":
                    vectorDontUse = leftVector;
                    break;
            }

            foreach(Vector2 vector in vectors)
            {
                if(vector == vectorDontUse)
                {
                    continue;
                }

                RaycastHit2D[] wallHits1 = null;
                RaycastHit2D[] wallHits2 = null;

                if(vector == upVector || vector == downVector)
                {
                    wallHits1 = Physics2D.RaycastAll(transform.position + new Vector3(.8f, 0), vector, rangeOfSight);
                    wallHits2 = Physics2D.RaycastAll(transform.position + new Vector3(-.8f, 0), vector, rangeOfSight);
                }
                else if(vector == rightVector || vector == leftVector)
                {
                    wallHits1 = Physics2D.RaycastAll(transform.position + new Vector3(0, .8f), vector, rangeOfSight);
                    wallHits2 = Physics2D.RaycastAll(transform.position + new Vector3(0, -.8f), vector, rangeOfSight);
                }

                List<RaycastHit2D> rchList = new List<RaycastHit2D>();
                foreach(RaycastHit2D rch in wallHits1)
                {
                    rchList.Add(rch);
                }
                foreach(RaycastHit2D rch in wallHits2)
                {
                    rchList.Add(rch);
                }

                RaycastHit2D[] wallHits = rchList.ToArray();
                RaycastHit2D? wallHit = null;
                foreach (RaycastHit2D aHit in wallHits)
                {
                    if (aHit.collider.CompareTag("Wall"))
                    {
                        wallHit = aHit;
                        break;
                    }
                }

                if (wallHit.HasValue)
                {
                    Debug.DrawLine(transform.position, wallHit.Value.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
                }

                RaycastHit2D[] badHits;
                try
                {
                    badHits = Physics2D.RaycastAll(transform.position, wallHit.Value.point, wallHit.Value.distance);

                }
                catch
                {
                    badHits = Physics2D.RaycastAll(transform.position, vector, rangeOfSight);
                }

                RaycastHit2D? badHit = null;
                foreach (RaycastHit2D aHit in badHits)
                {
                    if (aHit.collider.CompareTag("BadObject"))
                    {
                        Debug.Log(aHit.collider.name);
                    }
                }
                foreach (RaycastHit2D aHit in badHits)
                {
                    if (aHit.collider.CompareTag("BadObject"))
                    {
                        if (aHit.collider.name == "inmateOutfit" && !zonesScript.isTouchingYourCell && routineScript.periodCode == "LO")
                        {
                            badHit = aHit;
                            //StartCoroutine(solitaryScript.GoToSolitary());
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }

                if (badHit.HasValue)
                {
                    Debug.DrawLine(transform.position, badHit.Value.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
