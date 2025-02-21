using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DeskStand : MonoBehaviour
{
    public GameObject player;
    private bool hasClimbed;
    private bool isClimbing;
    private GameObject[] desks;
    private GameObject currentDesk;
    private void Start()
    {
        desks = GameObject.FindGameObjectsWithTag("Desk");
    }
    private void Update()
    {
        if (!hasClimbed && !isClimbing)
        {
            foreach (GameObject desk in desks)
            {
                if (player.GetComponent<PolygonCollider2D>().IsTouching(desk.transform.Find("ClimbingArea").GetComponent<BoxCollider2D>()) && Input.GetKeyDown(KeyCode.F))
                {
                    currentDesk = desk;
                    isClimbing = true;
                    StartCoroutine(ClimbDesk(desk));
                    break;
                }
            }
        }
        else if (hasClimbed && !isClimbing)
        {
            if (!player.GetComponent<PolygonCollider2D>().IsTouching(currentDesk.GetComponent<BoxCollider2D>()))
            {
                hasClimbed = false;
                StepOffDesk();
            }
        }
    }
    private IEnumerator ClimbDesk(GameObject desk)
    {
        desk.GetComponent<BoxCollider2D>().isTrigger = true;
        player.GetComponent<PlayerCtrl>().enabled = false;

        Vector3 deskVector = desk.transform.position;
        float speed = 5f;
        Vector3 direction = (deskVector - player.transform.position).normalized;
        while(Vector3.Distance(player.transform.position, deskVector) > 0.01f)//move player
        {
            player.transform.position += speed * Time.deltaTime * direction;
            yield return null;
        }
        player.transform.position = deskVector;
        player.layer = 15;
        player.GetComponent<PlayerCtrl>().enabled = true;
        isClimbing = false;
        hasClimbed = true;
    }
    private void StepOffDesk()
    {
        currentDesk.GetComponent<BoxCollider2D>().isTrigger = false;
        player.layer = 3;
    }
}
