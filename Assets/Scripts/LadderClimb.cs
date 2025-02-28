using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LadderClimb : MonoBehaviour
{
    public MouseCollisionOnItems mcs;
    public Canvas ic;
    public InventorySelection selectionScript;
    public VentClimb ventClimbScript; //to get mouse sprites
    public bool onRoof;
    public bool inVent;
    public GameObject player;
    public GameObject perksTiles;
    private GameObject currentLadder;
    private Vector3 offsetVector = new Vector3(); //only for ground-to-roof operations
    private bool hasDisabledTags;
    private float distance;
    public void Start()
    {
        offsetVector.y = 1.6f;
    }
    public void Update()
    {
        if (mcs.isTouchingGroundLadder || mcs.isTouchingVentLadder || mcs.isTouchingRoofLadder)
        {
            ic.transform.Find("MouseOverlay").GetComponent<RectTransform>().sizeDelta = new Vector2(45, 50);
            ic.transform.Find("MouseOverlay").GetComponent<Image>().sprite = ventClimbScript.mouseUpSprite;
            if (mcs.isTouchingGroundLadder)
            {
                distance = Vector2.Distance(player.transform.position, mcs.touchedGroundLadder.transform.position);
            }
            else if (mcs.isTouchingVentLadder)
            {
                distance = Vector2.Distance(player.transform.position, mcs.touchedVentLadder.transform.position);
            }
            else if (mcs.isTouchingRoofLadder)
            {
                distance = Vector2.Distance(player.transform.position, mcs.touchedRoofLadder.transform.position);
            }
            if(Input.GetMouseButtonDown(0) && distance <= 2.4f)
            {
                if (mcs.isTouchingGroundLadder)
                {
                    currentLadder = mcs.touchedGroundLadder;
                }
                else if (mcs.isTouchingVentLadder)
                {
                    currentLadder = mcs.touchedVentLadder;
                }
                else if (mcs.isTouchingRoofLadder)
                {
                    currentLadder = mcs.touchedRoofLadder;
                }
                StartCoroutine(ClimbLadderUp());
            }
        }
    }
    public IEnumerator ClimbLadderUp()
    {
        yield return new WaitForEndOfFrame();

        player.transform.position = currentLadder.GetComponent<LadderConnect>().connectedLadder.transform.position;

        if(currentLadder.GetComponent<LadderConnect>().connectedLadder.layer == 12)
        {
            perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().enabled = true;
            Color color = perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().color;
            color.a = 235f / 256f;
            perksTiles.transform.Find("Backdrop").GetComponent<SpriteRenderer>().color = color;

            SpriteRenderer[] ventSpriteRenderers = perksTiles.transform.Find("Vents").GetComponentsInChildren<SpriteRenderer>();
            SpriteRenderer[] ventObjectSpriteRenderers = perksTiles.transform.Find("VentObjects").GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in ventSpriteRenderers)
            {
                Color aColor = sr.color;
                aColor.a = 1;
                sr.color = aColor;
            }
            foreach (SpriteRenderer sr in ventObjectSpriteRenderers)
            {
                Color aColor = sr.color;
                aColor.a = 1;
                sr.color = aColor;
            }
        }

        switch (currentLadder.GetComponent<LadderConnect>().connectedLadder.layer)
        {
            case 12:
                player.layer = 12;
                player.GetComponent<SpriteRenderer>().sortingOrder = 11;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 12;
                perksTiles.transform.Find("Roof").gameObject.SetActive(false);
                perksTiles.transform.Find("RoofObjects").gameObject.SetActive(false);
                mcs.DisableTag("Bars");
                mcs.DisableTag("Fence");
                mcs.DisableTag("ElectricFence");
                mcs.DisableTag("Digable");
                mcs.DisableTag("Wall");
                mcs.DisableTag("Ladder(Ground)");
                mcs.DisableTag("Desk");//currently the only menu
                break;
            case 13:
                player.layer = 13;
                player.GetComponent<SpriteRenderer>().sortingOrder = 15;
                player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 16;
                break;
        }
    }
}
