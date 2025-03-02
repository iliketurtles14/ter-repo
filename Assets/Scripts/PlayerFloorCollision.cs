using UnityEngine;

public class PlayerFloorCollision : MonoBehaviour
{
    public GameObject playerFloor;
    public GameObject lastTouchedRoofFloor;
    private Vector2 position = new Vector2();
    private Collider2D[] hitColliders;
    private void Update()
    {
        if(gameObject.layer == 12)
        {
            position = transform.position;
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.name.StartsWith("Floor 2") ||
                    collider.gameObject.name.StartsWith("VentCover") ||
                    collider.gameObject.name.StartsWith("EmptyVentCover"))
                {
                    playerFloor = collider.gameObject;
                }
            }
        }
        else if(gameObject.layer == 13)
        {
            position = transform.position;
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.name.StartsWith("Roof Floor") ||
                    collider.gameObject.name.StartsWith("Floor 11") ||
                    collider.gameObject.name.StartsWith("Floor 14 (Roof)"))
                {
                    playerFloor = collider.gameObject;
                }
                if(collider.gameObject.name.StartsWith("Roof Floor"))
                {
                    lastTouchedRoofFloor = collider.gameObject;
                }
            }
        }
        else
        {
            position = transform.position;
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.name.StartsWith("Floor") ||
                    collider.gameObject.name.StartsWith("GrassPlaceholder"))
                {
                    playerFloor = collider.gameObject;
                }
            }
        }
    }
}
