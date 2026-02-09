using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollision : MonoBehaviour
{
    public bool isTouchingCellsZone;
    public GameObject touchedCellsZone;
    public bool isTouchingSolitaryZone;
    public GameObject touchedSolitaryZone;
    public bool isTouchingPlayerCell;
    public GameObject touchedPlayerCell;
    public bool isTouchingRollcallZone;
    public GameObject touchedRollcallZone;
    public bool isTouchingMailroomZone;
    public GameObject touchedMailroomZone;
    public bool isTouchingMetalshopZone;
    public GameObject touchedMetalshopZone;
    public bool isTouchingJanitorZone;
    public GameObject touchedJanitorZone;
    public bool isTouchingLaundryZone;
    public GameObject touchedLaundryZone;
    public bool isTouchingShowersZone;
    public GameObject touchedShowersZone;
    public bool isTouchingKitchenZone;
    public GameObject touchedKitchenZone;
    public bool isTouchingGymZone;
    public GameObject touchedGymZone;
    public bool isTouchingCanteenZone;
    public GameObject touchedCanteenZone;

    private void OnTriggerStay2D(Collider2D collider)
    {
        // Get the collider's bounds and the center of this GameObject
        Bounds colliderBounds = collider.bounds;
        Vector2 objectPosition = transform.position;

        // Check if the center of the collider is inside the bounds of the trigger
        if (colliderBounds.Contains(objectPosition))
        {
            if (collider.gameObject.tag == "CellsZone")
            {
                isTouchingCellsZone = true;
                touchedCellsZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "SolitaryZone")
            {
                isTouchingSolitaryZone = true;
                touchedSolitaryZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "PlayerCell")
            {
                isTouchingPlayerCell = true;
                touchedPlayerCell = collider.gameObject;
            }
            else if (collider.gameObject.tag == "RollcallZone")
            {
                isTouchingRollcallZone = true;
                touchedRollcallZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "MailroomZone")
            {
                isTouchingMailroomZone = true;
                touchedMailroomZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "MetalshopZone")
            {
                isTouchingMetalshopZone = true;
                touchedMetalshopZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "JanitorZone")
            {
                isTouchingJanitorZone = true;
                touchedJanitorZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "LaundryZone")
            {
                isTouchingLaundryZone = true;
                touchedLaundryZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "ShowersZone")
            {
                isTouchingShowersZone = true;
                touchedShowersZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "KitchenZone")
            {
                isTouchingKitchenZone = true;
                touchedKitchenZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "GymZone")
            {
                isTouchingGymZone = true;
                touchedGymZone = collider.gameObject;
            }
            else if (collider.gameObject.tag == "CanteenZone")
            {
                isTouchingCanteenZone = true;
                touchedCanteenZone = collider.gameObject;
            }
        }
        else
        {
            // If not inside, set the flags to false
            if (collider.gameObject.tag == "CellsZone")
            {
                isTouchingCellsZone = false;
                touchedCellsZone = null;
            }
            else if (collider.gameObject.tag == "SolitaryZone")
            {
                isTouchingSolitaryZone = false;
                touchedSolitaryZone = null;
            }
            else if (collider.gameObject.tag == "PlayerCell")
            {
                isTouchingPlayerCell = false;
                touchedPlayerCell = null;
            }
            else if (collider.gameObject.tag == "RollcallZone")
            {
                isTouchingRollcallZone = false;
                touchedRollcallZone = null;
            }
            else if (collider.gameObject.tag == "MailroomZone")
            {
                isTouchingMailroomZone = false;
                touchedMailroomZone = null;
            }
            else if (collider.gameObject.tag == "MetalshopZone")
            {
                isTouchingMetalshopZone = false;
                touchedMetalshopZone = null;
            }
            else if (collider.gameObject.tag == "JanitorZone")
            {
                isTouchingJanitorZone = false;
                touchedJanitorZone = null;
            }
            else if (collider.gameObject.tag == "LaundryZone")
            {
                isTouchingLaundryZone = false;
                touchedLaundryZone = null;
            }
            else if (collider.gameObject.tag == "ShowersZone")
            {
                isTouchingShowersZone = false;
                touchedShowersZone = null;
            }
            else if (collider.gameObject.tag == "KitchenZone")
            {
                isTouchingKitchenZone = false;
                touchedKitchenZone = null;
            }
            else if (collider.gameObject.tag == "GymZone")
            {
                isTouchingGymZone = false;
                touchedGymZone = null;
            }
            else if (collider.gameObject.tag == "CanteenZone")
            {
                isTouchingCanteenZone = false;
                touchedCanteenZone = null;
            }
        }
    }
}
