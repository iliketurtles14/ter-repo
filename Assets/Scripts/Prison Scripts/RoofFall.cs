using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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
    private LadderClimb ladderClimbScript;
    private Particles particlesScript;
    private BoxCollider2D bc;
    private readonly List<Collider2D> currentContacts = new List<Collider2D>();
    private ContactFilter2D filter;

    public void Start()
    {
        player = RootObjectCache.GetRoot("Player");
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        itemBehavioursScript = GetComponent<ItemBehaviours>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        ziplinesScript = GetComponent<Ziplines>();
        ladderClimbScript = GetComponent<LadderClimb>();
        particlesScript = GetComponent<Particles>();
        bc = player.transform.Find("RoofFallChecker").GetComponent<BoxCollider2D>();
        filter = new ContactFilter2D
        {
            useLayerMask = false,
            useTriggers = true,
        };

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
    private IEnumerator Loop()
    {
        while (true)
        {
            if (!Physics2D.GetIgnoreLayerCollision(playerLayer, roofLayer) && !itemBehavioursScript.isRoping && !ziplinesScript.isZipping)
            {
                yield return new WaitForFixedUpdate();
                currentContacts.Clear();
                int count = bc.GetContacts(filter, currentContacts);
                bool shouldFall = true;
                for (int i = 0; i < count; i++)
                {
                    Collider2D col = currentContacts[i];
                    if (col.gameObject.GetComponent<Collider2D>() != null && !col.gameObject.GetComponent<Collider2D>().isTrigger)
                    {
                        shouldFall = false;
                        break;
                    }
                }
                yield return new WaitForFixedUpdate();
                if(Physics2D.GetIgnoreLayerCollision(playerLayer, roofLayer))
                {
                    continue;
                }
                if (shouldFall && floorCollisionScript.touchedRoofFloor == null)
                {
                    ladderClimbScript.SendToGround();
                    player.transform.position -= offset;
                    particlesScript.CreateDust(player.transform.position, 1, player.GetComponent<SpriteRenderer>().sortingLayerName);
                }
                else if (!shouldFall && floorCollisionScript.touchedRoofFloor == null)
                {
                    player.transform.position = floorCollisionScript.lastTouchedRoofFloor.transform.position;
                }
            }
            yield return null;
        }
    }
}
