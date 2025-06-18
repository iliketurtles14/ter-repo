using UnityEngine;

public class PlayerFloorCollision : MonoBehaviour
{
    public GameObject playerFloor;
    public GameObject lastTouchedRoofFloor;
    public GameObject touchedRoofFloor;
    private Vector2 position = new Vector2();
    private Collider2D[] hitColliders;
    private void Update()
    {
        if(gameObject.layer == 12)
        {
            touchedRoofFloor = null;
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
                if (collider.gameObject.name.StartsWith("Roof Floor"))
                {
                    lastTouchedRoofFloor = collider.gameObject;
                }
                if (collider.gameObject.name.StartsWith("Roof Floor") ||
                    collider.gameObject.name.StartsWith("Floor 11") ||
                    collider.gameObject.name.StartsWith("Floor 14 (Roof)") ||
                    collider.gameObject.name.StartsWith("Roofing") ||
                    collider.gameObject.name.StartsWith("Box (Roof)") ||
                    collider.gameObject.name.StartsWith("RoofFallConstraint"))
                {
                    playerFloor = collider.gameObject;
                    touchedRoofFloor = collider.gameObject;
                    break;
                }
                else
                {
                    touchedRoofFloor = null;
                }
            }
        }
        else if(gameObject.layer == 11)
        {
            position = transform.position;
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach(Collider2D collider in hitColliders)
            {
                if(collider.gameObject.name.StartsWith("DirtEmpty") ||
                    collider.gameObject.name.StartsWith("100%HoleUp") ||
                    collider.gameObject.name.StartsWith("Brace"))
                {
                    playerFloor = collider.gameObject;
                    break;
                }
            }
        }
        else
        {
            touchedRoofFloor = null;
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
