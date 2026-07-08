using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HoleClimb : MonoBehaviour
{
    private MouseCollisionOnItems mcs;
    private GameObject player;
    private Transform tiles;
    private GameObject undergroundLight;
    private GameObject globalLight;
    private GameObject currentHole;
    private HPAChecker HPAScript;
    private InventorySelection selectionScript;
    private int groundLayer;
    private int undergroundLayer;
    private int ventLayer;
    private int roofLayer;
    private int playerLayer;
    private int uiLayer;
    public bool isUnderground;
    public void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        player = RootObjectCache.GetRoot("Player");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        undergroundLight = RootObjectCache.GetRoot("UndergroundLight");
        globalLight = RootObjectCache.GetRoot("GlobalLight");
        HPAScript = player.GetComponent<HPAChecker>();
        selectionScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<InventorySelection>();

        groundLayer = LayerMask.NameToLayer("Ground");
        undergroundLayer = LayerMask.NameToLayer("Underground");
        ventLayer = LayerMask.NameToLayer("Vents");
        roofLayer = LayerMask.NameToLayer("Roof");
        playerLayer = LayerMask.NameToLayer("Player");
        uiLayer = LayerMask.NameToLayer("UI");

        undergroundLight.SetActive(false);

        mcs.DisableTag("Dirt");
        mcs.DisableTag("EmptyDirt");

        foreach (Transform child in tiles.Find("UndergroundObjects"))
        {
            if (child.gameObject.CompareTag("Item"))
            {
                child.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    public void Update()
    {
        if(isUnderground)
        {
            undergroundLight.transform.position = player.transform.position;
        }

        if (!HPAScript.isBusy && mcs.isTouchingHoleDown && !selectionScript.aSlotSelected)
        {
            float distance = Vector2.Distance(player.transform.position, mcs.touchedHoleDown.transform.position);
            if (Input.GetMouseButtonDown(0) && distance <= 2.4f)
            {
                currentHole = mcs.touchedHoleDown;
                StartCoroutine(ClimbDown());
            }
        }
        else if(!HPAScript.isBusy && mcs.isTouchingHoleUp && !selectionScript.aSlotSelected)
        {
            float distance = Vector2.Distance(player.transform.position, mcs.touchedHoleUp.transform.position);
            if (Input.GetMouseButtonDown(0) && distance <= 2.4f)
            {
                currentHole = mcs.touchedHoleUp;
                StartCoroutine(ClimbUp());
            }
        }
    }
    public IEnumerator ClimbUp()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForEndOfFrame();

        player.transform.position = currentHole.transform.position;
        DisableAllLayerCollisions();
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, false);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
        player.GetComponent<SpriteRenderer>().sortingOrder = 6;
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
        tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingOrder = -1;
        tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingOrder = -1;
        tiles.Find("UndergroundObjects").gameObject.SetActive(false);
        undergroundLight.SetActive(false);
        globalLight.SetActive(true);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        isUnderground = false;
    }
    public IEnumerator ClimbDown()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForEndOfFrame();

        player.transform.position = currentHole.transform.position;
        DisableAllLayerCollisions();
        Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, false);
        Physics2D.IgnoreLayerCollision(playerLayer, undergroundLayer, false);
        player.GetComponent<SpriteRenderer>().sortingOrder = 12;
        player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 13;
        tiles.Find("UndergroundTiles").GetComponent<SpriteRenderer>().sortingOrder = 10;
        tiles.Find("UndergroundPlane").GetComponent<SpriteRenderer>().sortingOrder = 9;
        tiles.Find("UndergroundObjects").gameObject.SetActive(true);
        undergroundLight.SetActive(true);
        globalLight.SetActive(false);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        isUnderground = true;
    }
    private void DisableAllLayerCollisions()
    {
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, roofLayer, true);
    }
}
