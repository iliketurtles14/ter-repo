using UnityEngine;

public class PlayerFloorCollision : MonoBehaviour
{
    public GameObject playerFloor;
    public GameObject lastTouchedRoofFloor;
    public GameObject touchedRoofFloor;
    private Vector2 position = new Vector2();
    private Collider2D[] hitColliders;
    private void FixedUpdate()
    {
        if(gameObject.layer == 12)
        {
            touchedRoofFloor = null;
            position = transform.position;
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.CompareTag("Digable") ||
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
                if (collider.gameObject.CompareTag("Digable"))
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
                if (collider.gameObject.CompareTag("Digable"))
                {
                    playerFloor = collider.gameObject;
                }
            }
        }
    }
}
