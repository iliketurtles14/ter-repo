using System.Collections;
using UnityEngine;

public class RoofFall : MonoBehaviour
{
    private GameObject player;
    private Transform tiles;
    private ItemBehaviours itemBehavioursScript;
    private MouseCollisionOnItems mcs;
    private PlayerFloorCollision floorCollisionScript;
    Vector3 offset;

    public void Start()
    {
        player = RootObjectCache.GetRoot("Player");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        itemBehavioursScript = GetComponent<ItemBehaviours>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        
        floorCollisionScript = player.GetComponent<PlayerFloorCollision>();
        offset = new Vector3(0, 1.6f, 0);

        StartCoroutine(Loop());
    }
    public IEnumerator Loop()
    {
        while (true)
        {
            if(player.layer == 13 && !itemBehavioursScript.isRoping)
            {
                yield return new WaitForFixedUpdate();
                if (floorCollisionScript.touchedRoofFloor == null)
                {
                    player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    player.layer = 3;
                    player.GetComponent<SpriteRenderer>().sortingOrder = 6;
                    player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
                    tiles.Find("Roof").gameObject.SetActive(false);
                    tiles.Find("RoofObjects").gameObject.SetActive(false);
                    foreach (Transform child in tiles.Find("GroundObjects"))
                    {
                        if (child.CompareTag("Item"))
                        {
                            child.GetComponent<BoxCollider2D>().enabled = true;
                        }
                    }
                    mcs.EnableTag("Bars");
                    mcs.EnableTag("Fence");
                    mcs.EnableTag("ElectricFence");
                    mcs.EnableTag("Digable");
                    mcs.EnableTag("Wall");
                    mcs.EnableTag("Ladder(Ground)");
                    mcs.EnableTag("Desk");//currently the only menu

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
