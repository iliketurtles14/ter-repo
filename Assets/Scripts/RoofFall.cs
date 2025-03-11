using System.Collections;
using UnityEngine;

public class RoofFall : MonoBehaviour
{
    public GameObject player;
    public GameObject perksTiles;
    public ItemBehaviours itemBehavioursScript;
    public MouseCollisionOnItems mcs;
    private PlayerFloorCollision floorCollisionScript;
    Vector3 offset;

    public void Start()
    {
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
                if (floorCollisionScript.touchedRoofFloor == null)
                {
                    player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

                    player.layer = 3;
                    player.GetComponent<SpriteRenderer>().sortingOrder = 6;
                    player.transform.Find("Outfit").GetComponent<SpriteRenderer>().sortingOrder = 7;
                    perksTiles.transform.Find("Roof").gameObject.SetActive(false);
                    perksTiles.transform.Find("RoofObjects").gameObject.SetActive(false);
                    foreach (Transform child in perksTiles.transform.Find("GroundObjects"))
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

                    player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
            yield return null;
        }
    }
}
