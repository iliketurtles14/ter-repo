using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypertubeController : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Transform player;
    private bool inHypertube;
    private HPAChecker hpaScript;
    private Transform mc;
    private List<string> objLayers = new List<string>
    {
        "UndergroundObjects", "GroundObjects", "VentObjects", "RoofObjects"
    };
    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player").transform;
        hpaScript = GetComponent<HPAChecker>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
    }
    private void Update()
    {
        hpaScript.isInHypertube = inHypertube;
        if(mcs.isTouchingHypertube && Input.GetMouseButtonDown(0) && !inHypertube)
        {
            float distance = Vector2.Distance(player.position, mcs.touchedHypertube.transform.position);
            if(distance <= 2.4f)
            {
                StartCoroutine(ClimbTube(mcs.touchedHypertube));
            }
        }
    }
    private IEnumerator ClimbTube(GameObject tubeOpening)
    {
        inHypertube = true;

        player.GetComponent<PlayerCtrl>().canMove = false;
        while (Vector2.Distance(player.position, tubeOpening.transform.position) > .1f)
        {
            player.position += 5f * Time.deltaTime * (tubeOpening.transform.position + player.position).normalized;
            yield return null;
        }
        player.position = tubeOpening.transform.position;
        player.GetComponent<PlayerAnimation>().enabled = false;
        BodyController bc = player.GetComponent<BodyController>();
        OutfitController oc = player.GetComponent<OutfitController>();
        player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
        if (player.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
        {
            player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
            int outfitItemID = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id;
            if (outfitItemID == 29 || outfitItemID == 30 || outfitItemID == 31 || outfitItemID == 32) //check if its an inmate outfit (this is because the inmate sleeping outfit sprite is not 16x16 like every other sprite for some reason)
            {
                if (NPCSave.instance.playerCharacter != 1)
                {
                    player.transform.Find("Outfit").localPosition = new Vector3(0, -.025f, 0);
                }
                else
                {
                    player.transform.Find("Outfit").localPosition = new Vector3(0, -.02f, 0);
                }
            }
        }
        StartCoroutine(TubeMove(tubeOpening));
    }
    private IEnumerator TubeMove(GameObject tubeOpening)
    {
        //get list of tubes for the player to move in
        List<GameObject> tubes = new List<GameObject>();
        List<GameObject> openTubes = new List<GameObject>();

        string currentDir = "";
        if (tubeOpening.name.Contains("Up"))
        {
            currentDir = "up";
        }
        else if (tubeOpening.name.Contains("Down"))
        {
            currentDir = "down";
        }
        else if (tubeOpening.name.Contains("Left"))
        {
            currentDir = "left";
        }
        else if (tubeOpening.name.Contains("Right"))
        {
            currentDir = "right";
        }

        Dictionary<string, Vector2> dirDict = new Dictionary<string, Vector2>
        {
            { "up", new Vector2(0, 1.6f) }, { "down", new Vector2(0, -1.6f) },
            { "left", new Vector2(-1.6f, 0) }, { "right", new Vector2(1.6f, 0) }
        };

        tubes.Add(tubeOpening);
        GameObject lastTube = tubeOpening;
        foreach(Transform obj in tubeOpening.transform.parent)
        {
            Vector3 dirVector = dirDict[currentDir];

            if(Vector2.Distance(obj.position, lastTube.transform.position + dirVector) <= .1f)
            {
                lastTube = obj.gameObject;
                if (lastTube.name.Contains("Up"))
                {
                    currentDir = "up";
                }
                else if (lastTube.name.Contains("Down"))
                {
                    currentDir = "down";
                }
                else if (lastTube.name.Contains("Left"))
                {
                    currentDir = "left";
                }
                else if (lastTube.name.Contains("Right"))
                {
                    currentDir = "right";
                }
                tubes.Add(lastTube);
                if (lastTube.name.Contains("Open"))
                {
                    openTubes.Add(lastTube);
                }
            }
        }

        //move player throught that
        yield return null;
    }
}
