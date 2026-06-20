using System.Collections;
using UnityEngine;

public class RoofFall : MonoBehaviour
{
    private GameObject player;
    private Transform tiles;
    private ItemBehaviours itemBehavioursScript;
    private MouseCollisionOnItems mcs;
    private PlayerFloorCollision floorCollisionScript;
    private Ziplines ziplinesScript;
    Vector3 offset;
    private int groundLayer;
    private int undergroundLayer;
    private int ventLayer;
    private int roofLayer;
    private int playerLayer;
    private int uiLayer;
    private int ventCoverLayer;

    public void Start()
    {
        player = RootObjectCache.GetRoot("Player");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        itemBehavioursScript = GetComponent<ItemBehaviours>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        ziplinesScript = GetComponent<Ziplines>();

        floorCollisionScript = player.GetComponent<PlayerFloorCollision>();
        offset = new Vector3(0, 1.6f, 0);

        groundLayer = LayerMask.NameToLayer("Ground");
        undergroundLayer = LayerMask.NameToLayer("Underground");
        ventLayer = LayerMask.NameToLayer("Vents");
        roofLayer = LayerMask.NameToLayer("Roof");
        playerLayer = LayerMask.NameToLayer("Player");
        uiLayer = LayerMask.NameToLayer("UI");
        ventCoverLayer = LayerMask.NameToLayer("VentCovers");

        StartCoroutine(Loop());
    }
    public IEnumerator Loop()
    {
        while (true)
        {
            if(!Physics2D.GetIgnoreLayerCollision(playerLayer, roofLayer) && !itemBehavioursScript.isRoping && !ziplinesScript.isZipping && false)
            {
                yield return new WaitForFixedUpdate();
                if (floorCollisionScript.touchedRoofFloor == null)
                {
                    player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    player.GetComponent<SpriteRenderer>().sortingOrder = 6;
                    player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
                    tiles.Find("Roof").gameObject.SetActive(false);
                    tiles.Find("RoofObjects").gameObject.SetActive(false);

                    player.transform.position -= offset;

                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Desk"))
                    {
                        obj.GetComponent<DeskPickUp>().enabled = true;
                    }

                    player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
            yield return null;
        }
    }
}
