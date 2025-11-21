using UnityEngine;

public class PlayerFloorCollision : MonoBehaviour
{
    public GameObject playerFloor;
    public GameObject lastTouchedRoofFloor;
    public GameObject touchedRoofFloor;
    private Vector2 position = new Vector2();
    private Collider2D[] hitColliders;
    private void FixedUpdate()// 9 - vent, 13 - roof, 2 - ground
    {
        if(gameObject.layer == 12)
        {
            touchedRoofFloor = null;
            position = transform.position;
            hitColliders = Physics2D.OverlapPointAll(position);
            foreach (Collider2D collider in hitColliders)
            {
                if ((collider.gameObject.CompareTag("Digable") ||
                    collider.gameObject.name.StartsWith("VentCover") ||
                    collider.gameObject.name.StartsWith("EmptyVentCover")) &&
                    collider.gameObject.layer == 9)
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
                if (collider.gameObject.CompareTag("Digable") && collider.gameObject.layer == 13)
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
                if((collider.gameObject.name.StartsWith("DirtEmpty") ||
                    collider.gameObject.name.StartsWith("100%HoleUp") ||
                    collider.gameObject.name.StartsWith("Brace")) &&
                    collider.gameObject.layer == 10)
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
                if (collider.gameObject.CompareTag("Digable") && collider.gameObject.layer == 2)
                {
                    playerFloor = collider.gameObject;
                }
            }
        }
    }
}
