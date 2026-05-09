using UnityEngine;

public class PlayerFloorCollision : MonoBehaviour
{
    public GameObject playerFloor;
    public GameObject lastTouchedRoofFloor;
    public GameObject touchedRoofFloor;
    private Vector2 position = new Vector2();
    private Collider2D[] hitColliders;
    private int groundLayer;
    private int undergroundLayer;
    private int ventLayer;
    private int roofLayer;
    private int playerLayer;
    private void Start()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        undergroundLayer = LayerMask.NameToLayer("Underground");
        ventLayer = LayerMask.NameToLayer("Vents");
        roofLayer = LayerMask.NameToLayer("Roof");
        playerLayer = LayerMask.NameToLayer("Player");
    }
    private void FixedUpdate()// 12 - vent, 13 - roof, 10 - ground
    {
        if(!Physics2D.GetIgnoreLayerCollision(playerLayer, ventLayer))
        {
            touchedRoofFloor = null;
            position = transform.position - new Vector3(0, .4f, 0);
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach (Collider2D collider in hitColliders)
            {
                if ((collider.gameObject.CompareTag("Digable") ||
                    collider.gameObject.name.StartsWith("VentCover") ||
                    collider.gameObject.name.StartsWith("EmptyVentCover")) &&
                    collider.gameObject.layer == ventLayer)
                {
                    playerFloor = collider.gameObject;
                }
            }
        }
        else if(!Physics2D.GetIgnoreLayerCollision(playerLayer, roofLayer))
        {
            position = transform.position - new Vector3(0, .4f, 0);
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.CompareTag("Digable") && collider.gameObject.layer == roofLayer)
                {
                    playerFloor = collider.gameObject;
                    touchedRoofFloor = collider.gameObject;
                    lastTouchedRoofFloor = collider.gameObject;
                    break;
                }
                else
                {
                    touchedRoofFloor = null;
                }
            }
        }
        else if(!Physics2D.GetIgnoreLayerCollision(playerLayer, undergroundLayer))
        {
            position = transform.position - new Vector3(0, .4f, 0);
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach(Collider2D collider in hitColliders)
            {
                if((collider.gameObject.name.StartsWith("DirtEmpty") ||
                    collider.gameObject.name.StartsWith("100%HoleUp") ||
                    collider.gameObject.name.StartsWith("Brace")) &&
                    collider.gameObject.layer == undergroundLayer)
                {
                    playerFloor = collider.gameObject;
                    break;
                }
            }
        }
        else if(!Physics2D.GetIgnoreLayerCollision(playerLayer, groundLayer))
        {
            touchedRoofFloor = null;
            position = transform.position - new Vector3(0, .4f, 0);
            hitColliders = Physics2D.OverlapPointAll(position);
            Debug.Log(hitColliders.Length);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.CompareTag("Digable") && collider.gameObject.layer == groundLayer)
                {
                    playerFloor = collider.gameObject;
                }
            }
        }
    }
}
