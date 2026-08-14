using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RaftEscape : MonoBehaviour
{
    private CapsuleCollider2D playerCol;
    private Inventory invScript;
    private GameObject player;
    private string thisLayer;
    private Escaping escapingScript;
    private void Start()
    {
        playerCol = RootObjectCache.GetRoot("Player").GetComponent<CapsuleCollider2D>();
        invScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Inventory>();
        player = RootObjectCache.GetRoot("Player");
        thisLayer = LayerMask.LayerToName(gameObject.layer);
        escapingScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Escaping>();
    }
    private void FixedUpdate()
    {
        if(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer(thisLayer), player.layer))
        {
            return;
        }

        bool hasRaft = false;
        foreach(InventoryItem item in invScript.inventory)
        {
            if(item.itemData != null)
            {
                hasRaft = item.itemData.id == 188;
                if (hasRaft)
                {
                    break;
                }
            }
        }

        if (!hasRaft)
        {
            return;
        }

        List<Collider2D> hitColliders = new List<Collider2D>();
        ContactFilter2D filter = ContactFilter2D.noFilter;
        playerCol.Overlap(filter, hitColliders);
        foreach(Collider2D col in hitColliders)
        {
            if(col.gameObject.name == "Raft" && LayerMask.LayerToName(col.gameObject.layer) == thisLayer)
            {
                StartCoroutine(escapingScript.Escape());
                break;
            }
        }
    }
}
