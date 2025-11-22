using System.Collections;
using UnityEngine;

public class SetInitialLocation : MonoBehaviour
{
    private Transform player;
    private Transform tiles;
    private Sittables sittablesScript;
    private VitalController vitalScript;
    private Transform mc;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        sittablesScript = GetComponent<Sittables>();
        vitalScript = player.GetComponent<VitalController>();
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;

        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame(); //there are six waitforframes here because we also wait for the outfit to be set in SetInitialOutfits.cs

        StartCoroutine(SetLocation());
    }
    private IEnumerator SetLocation()
    {
        //get bed location
        Vector3 bedLocation = Vector3.zero;

        foreach(Transform obj in tiles.Find("GroundObjects"))
        {
            if(obj.name == "PlayerBedVertical")
            {
                bedLocation = obj.position;
                sittablesScript.sittable = obj.gameObject;
            }
        }
        sittablesScript.sittable.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<PlayerCtrl>().enabled = false;
        yield return new WaitForFixedUpdate();
        player.position = bedLocation + new Vector3(0, .4f); //offset for beds. this is in Sittables.cs
        sittablesScript.onSittable = true;

        //stuff pulled from Sittables.cs
        BodyController bc = player.GetComponent<BodyController>();
        OutfitController oc = player.GetComponent<OutfitController>();

        vitalScript.energyRate = 1;
        vitalScript.energyRateAmount = 2;
        player.GetComponent<PlayerAnimation>().enabled = false;
        player.GetComponent<SpriteRenderer>().sprite = bc.characterDict[bc.character][0][1];
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = oc.outfitDict[oc.outfit][0][1];
        int outfitItemID = mc.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>().idInv[0].itemData.id;
        if (outfitItemID == 29 || outfitItemID == 30 || outfitItemID == 31 || outfitItemID == 32) //check if its an inmate outfit (this is because the inmate sleeping outfit sprite is not 16x16 like every other sprite for some reason)
        {
            player.transform.Find("Outfit").position = new Vector3(0, -.025f, 0);
        }
    }
}
