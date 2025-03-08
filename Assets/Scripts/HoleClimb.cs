using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HoleClimb : MonoBehaviour
{
    public MouseCollisionOnItems mcs;
    public Canvas ic;
    public InventorySelection selectionScript;
    public VentClimb ventClimbScript; //to get mosue srpites
    public ItemBehaviours itemBehavioursScript;
    public GameObject player;
    public GameObject perksTiles;
    public Sprite dirtSprite;
    public Sprite emptyDirtSprite;
    public GameObject undergroundLight;
    public GameObject globalLight;
    private GameObject currentHole;
    private float distance;
    private bool hasDisabledColliders;
    public void Start()
    {
        undergroundLight.SetActive(false);
    }
    public void Update()
    {
        if(player.layer == 11)
        {
            undergroundLight.transform.position = player.transform.position;
        }

        if (mcs.isTouchingHoleDown && player.layer == 3)
        {
            ic.transform.Find("MouseOverlay").GetComponent<RectTransform>().sizeDelta = new Vector2(45, 50);
            ic.transform.Find("MouseOverlay").GetComponent<Image>().sprite = ventClimbScript.mouseUpSprite;
            distance = Vector2.Distance(player.transform.position, mcs.touchedHoleDown.transform.position);
            if (Input.GetMouseButtonDown(0) && distance <= 2.4f)
            {
                currentHole = mcs.touchedHoleDown;
                StartCoroutine(ClimbHole());
            }
        }
    }
    public IEnumerator ClimbHole()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForEndOfFrame();

        player.transform.position = currentHole.transform.position;
        player.layer = 11;
        player.GetComponent<SpriteRenderer>().sortingOrder = -3;
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = -2;
        perksTiles.transform.Find("GroundPlane").gameObject.SetActive(false);
        perksTiles.transform.Find("Ground").gameObject.SetActive(false);
        perksTiles.transform.Find("GroundObjects").gameObject.SetActive(false);
        undergroundLight.SetActive(true);
        globalLight.SetActive(false);

        foreach(Transform tile in perksTiles.transform.Find("Underground"))
        {
            if (tile.name.StartsWith("Dirt"))
            {
                tile.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
