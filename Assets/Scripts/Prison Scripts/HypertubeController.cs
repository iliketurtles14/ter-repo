using System.Collections;
using UnityEngine;

public class HypertubeController : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private Transform player;
    private bool inHypertube;
    private HPAChecker hpaScript;
    private Transform mc;
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

    }
}
