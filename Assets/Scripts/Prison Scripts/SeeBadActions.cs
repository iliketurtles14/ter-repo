using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SeeBadActions : MonoBehaviour
{
    private List<Vector2> upVectors = new List<Vector2>();
    private List<Vector2> rightVectors = new List<Vector2>();
    private List<Vector2> downVectors = new List<Vector2>();
    private List<Vector2> leftVectors = new List<Vector2>();
    private List<Vector2> currentVectors;
    public float rangeOfSight = 6.4f;
    private void Start()
    {
        MakeVetorLists();
    }
    private void MakeVetorLists()
    {
        //upVectors
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 162f), Mathf.Sin(Mathf.Deg2Rad * 162f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 144f), Mathf.Sin(Mathf.Deg2Rad * 144f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 126f), Mathf.Sin(Mathf.Deg2Rad * 126f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 108f), Mathf.Sin(Mathf.Deg2Rad * 108f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90f), Mathf.Sin(Mathf.Deg2Rad * 90f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 72f), Mathf.Sin(Mathf.Deg2Rad * 72f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 54f), Mathf.Sin(Mathf.Deg2Rad * 54f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 36f), Mathf.Sin(Mathf.Deg2Rad * 36f)));
        upVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 18f), Mathf.Sin(Mathf.Deg2Rad * 18f)));
        //rightVectors
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 72f), Mathf.Sin(Mathf.Deg2Rad * 72f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 54f), Mathf.Sin(Mathf.Deg2Rad * 54f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 36f), Mathf.Sin(Mathf.Deg2Rad * 36f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 18f), Mathf.Sin(Mathf.Deg2Rad * 18f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 0f), Mathf.Sin(Mathf.Deg2Rad * 0f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 342f), Mathf.Sin(Mathf.Deg2Rad * 342f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 324f), Mathf.Sin(Mathf.Deg2Rad * 324f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 306f), Mathf.Sin(Mathf.Deg2Rad * 306f)));
        rightVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 288f), Mathf.Sin(Mathf.Deg2Rad * 288f)));
        //leftVectors
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 108f), Mathf.Sin(Mathf.Deg2Rad * 108f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 126f), Mathf.Sin(Mathf.Deg2Rad * 126f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 144f), Mathf.Sin(Mathf.Deg2Rad * 144f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 162f), Mathf.Sin(Mathf.Deg2Rad * 162f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 180f), Mathf.Sin(Mathf.Deg2Rad * 180f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 198f), Mathf.Sin(Mathf.Deg2Rad * 198f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 216f), Mathf.Sin(Mathf.Deg2Rad * 216f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 234f), Mathf.Sin(Mathf.Deg2Rad * 234f)));
        leftVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 252f), Mathf.Sin(Mathf.Deg2Rad * 252f)));
        //downVectors
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 198f), Mathf.Sin(Mathf.Deg2Rad * 198f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 216f), Mathf.Sin(Mathf.Deg2Rad * 216f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 234f), Mathf.Sin(Mathf.Deg2Rad * 234f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 252f), Mathf.Sin(Mathf.Deg2Rad * 252f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 270f), Mathf.Sin(Mathf.Deg2Rad * 270f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 288f), Mathf.Sin(Mathf.Deg2Rad * 288f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 306f), Mathf.Sin(Mathf.Deg2Rad * 306f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 324f), Mathf.Sin(Mathf.Deg2Rad * 324f)));
        downVectors.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 342f), Mathf.Sin(Mathf.Deg2Rad * 342f)));
    }
    private void Update()
    {
        switch (GetComponent<NPCAnimation>().lookDir)
        {
            case "up":
                currentVectors = upVectors;
                break;
            case "right":
                currentVectors = rightVectors;
                break;
            case "left":
                currentVectors = leftVectors;
                break;
            case "down":
                currentVectors = downVectors;
                break;
        }

        foreach(Vector2 vector in currentVectors)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, vector, rangeOfSight);

            RaycastHit2D? hit = null;
            foreach(RaycastHit2D aHit in hits)
            {
                if (aHit.collider.CompareTag("Wall"))
                {
                    hit = aHit;
                    break;
                }
            }

            if (hit.HasValue)
            {
                Debug.DrawLine(transform.position, hit.Value.point, Color.red);
            }
            else
            {
                Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
            }

        }
    }
}
