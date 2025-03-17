using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MouseCollisionOnItems : MonoBehaviour //this started as an item script and is now how i do collision for the whole game :skull:
{
    private HashSet<GameObject> items = new HashSet<GameObject>();
    private HashSet<GameObject> invSlots = new HashSet<GameObject>();
    private HashSet<GameObject> desks = new HashSet<GameObject>();
    private HashSet<GameObject> deskSlots = new HashSet<GameObject>();
    private HashSet<GameObject> deskPanels = new HashSet<GameObject>();
    private HashSet<GameObject> walls = new HashSet<GameObject>();
    private HashSet<GameObject> bars = new HashSet<GameObject>();
    private HashSet<GameObject> fences = new HashSet<GameObject>();
    private HashSet<GameObject> electricFences = new HashSet<GameObject>();
    private HashSet<GameObject> buttons = new HashSet<GameObject>();
    private HashSet<GameObject> idSlots = new HashSet<GameObject>();
    private HashSet<GameObject> idPanels = new HashSet<GameObject>();
    private HashSet<GameObject> floors = new HashSet<GameObject>();
    private HashSet<GameObject> ventCovers = new HashSet<GameObject>();
    private HashSet<GameObject> openVents = new HashSet<GameObject>();
    private HashSet<GameObject> slats = new HashSet<GameObject>();
    private HashSet<GameObject> groundLadders = new HashSet<GameObject>();
    private HashSet<GameObject> ventLadders = new HashSet<GameObject>();
    private HashSet<GameObject> roofLadders = new HashSet<GameObject>();
    private HashSet<GameObject> roofLedges = new HashSet<GameObject>();
    private HashSet<GameObject> holeDowns = new HashSet<GameObject>();
    private HashSet<GameObject> holeUps = new HashSet<GameObject>();
    private HashSet<GameObject> dirts = new HashSet<GameObject>();
    private HashSet<GameObject> emptyDirts = new HashSet<GameObject>();
    private HashSet<GameObject> rocks = new HashSet<GameObject>();

    private HashSet<string> disabledTags = new HashSet<string>();

    public bool isTouchingItem => items.Count > 0;
    public GameObject touchedItem => items.Count > 0 ? GetFirst(items) : null;
    public bool isTouchingInvSlot => invSlots.Count > 0;
    public GameObject touchedInvSlot => invSlots.Count > 0 ? GetFirst(invSlots) : null;
    public bool isTouchingDesk => desks.Count > 0;
    public GameObject touchedDesk => desks.Count > 0 ? GetFirst(desks) : null;
    public bool isTouchingDeskSlot => deskSlots.Count > 0;
    public GameObject touchedDeskSlot => deskSlots.Count > 0 ? GetFirst(deskSlots) : null;
    public bool isTouchingDeskPanel => deskPanels.Count > 0;
    public GameObject touchedDeskPanel => deskPanels.Count > 0 ? GetFirst(deskPanels) : null;
    public bool isTouchingWall => walls.Count > 0;
    public GameObject touchedWall => walls.Count > 0 ? GetFirst(walls) : null;
    public bool isTouchingBars => bars.Count > 0;
    public GameObject touchedBars => bars.Count > 0 ? GetFirst(bars) : null;
    public bool isTouchingFence => fences.Count > 0;
    public GameObject touchedFence => fences.Count > 0 ? GetFirst(fences) : null;
    public bool isTouchingElectricFence => electricFences.Count > 0;
    public GameObject touchedElectricFence => electricFences.Count > 0 ? GetFirst(electricFences) : null;
    public bool isTouchingButton => buttons.Count > 0;
    public GameObject touchedButton => buttons.Count > 0 ? GetFirst(buttons) : null;
    public bool isTouchingIDSlot => idSlots.Count > 0;
    public GameObject touchedIDSlot => idSlots.Count > 0 ? GetFirst(idSlots) : null;
    public bool isTouchingIDPanel => idPanels.Count > 0;
    public GameObject touchedIDPanel => idPanels.Count > 0 ? GetFirst(idPanels) : null;
    public bool isTouchingFloor => floors.Count > 0;
    public GameObject touchedFloor => floors.Count > 0 ? GetFirst(floors) : null;
    public bool isTouchingVentCover => ventCovers.Count > 0;
    public GameObject touchedVentCover => ventCovers.Count > 0 ? GetFirst(ventCovers) : null;
    public bool isTouchingOpenVent => openVents.Count > 0;
    public GameObject touchedOpenVent => openVents.Count > 0 ? GetFirst(openVents) : null;
    public bool isTouchingSlats => slats.Count > 0;
    public GameObject touchedSlats => slats.Count > 0 ? GetFirst(slats) : null;
    public bool isTouchingGroundLadder => groundLadders.Count > 0;
    public GameObject touchedGroundLadder => groundLadders.Count > 0 ? GetFirst(groundLadders) : null;
    public bool isTouchingVentLadder => ventLadders.Count > 0;
    public GameObject touchedVentLadder => ventLadders.Count > 0 ? GetFirst(ventLadders) : null;
    public bool isTouchingRoofLadder => roofLadders.Count > 0;
    public GameObject touchedRoofLadder => roofLadders.Count > 0 ? GetFirst(roofLadders) : null;
    public bool isTouchingRoofLedge => roofLedges.Count > 0;
    public GameObject touchedRoofLedge => roofLedges.Count > 0 ? GetFirst(roofLedges) : null;
    public bool isTouchingHoleDown => holeDowns.Count > 0;
    public GameObject touchedHoleDown => holeDowns.Count > 0 ? GetFirst(holeDowns) : null;
    public bool isTouchingHoleUp => holeUps.Count > 0;
    public GameObject touchedHoleUp => holeUps.Count > 0 ? GetFirst(holeUps) : null;
    public bool isTouchingDirt => dirts.Count > 0;
    public GameObject touchedDirt => dirts.Count > 0 ? GetFirst(dirts) : null;
    public bool isTouchingEmptyDirt => emptyDirts.Count > 0;
    public GameObject touchedEmptyDirt => emptyDirts.Count > 0 ? GetFirst(emptyDirts) : null;
    public bool isTouchingRock => rocks.Count > 0;
    public GameObject touchedRock => rocks.Count > 0 ? GetFirst(rocks) : null;
    void Update()
    {
        
        // Get mouse position in world space
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check for collisions at the mouse position
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);

        bool touchingInvSlot = false;
        bool touchingVentCover = false;
        bool touchingOpenVent = false;
        bool touchingItem = false;
        bool touchingGroundLadder = false;
        bool touchingVentLadder = false;
        bool touchingRoofLadder = false;
        bool touchingRock = false;

        // First pass: Check for specific tags
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("InvSlot"))
            {
                touchingInvSlot = true;
            }
            if (collider.CompareTag("Item"))
            {
                touchingItem = true;
            }
            if (collider.CompareTag("VentCover"))
            {
                touchingVentCover = true;
            }
            if (collider.CompareTag("OpenVent"))
            {
                touchingOpenVent = true;
            }
            if (collider.CompareTag("Ladder(Ground)"))
            {
                touchingGroundLadder = true;
            }
            if (collider.CompareTag("Ladder(Vent)"))
            {
                touchingVentLadder = true;
            }
            if (collider.CompareTag("Ladder(Roof)"))
            {
                touchingRoofLadder = true;
            }
            if (collider.CompareTag("Rock"))
            {
                touchingRock = true;
            }
        }

        // Clear previous collisions
        ClearCollisions();

        // Second pass: Add current collisions
        foreach (var collider in hitColliders)
        {
            GameObject touchedObject = collider.gameObject;

            if (disabledTags.Contains(touchedObject.tag))
            {
                continue;
            }

            if (touchingInvSlot && touchedObject.CompareTag("InvSlot"))
            {
                AddCollision(touchedObject);
            }
            else if (touchingItem && touchedObject.CompareTag("Item"))
            {
                AddCollision(touchedObject);
            }
            else if (touchingVentCover && touchedObject.CompareTag("VentCover"))
            {
                AddCollision(touchedObject);
            }
            else if (touchingOpenVent && touchedObject.CompareTag("OpenVent"))
            {
                AddCollision(touchedObject);
            }
            else if (touchingGroundLadder && touchedObject.CompareTag("Ladder(Ground)"))
            {
                AddCollision(touchedObject);
            }
            else if (touchingVentLadder && touchedObject.CompareTag("Ladder(Vent)"))
            {
                AddCollision(touchedObject);
            }
            else if (touchingRoofLadder && touchedObject.CompareTag("Ladder(Roof)"))
            {
                AddCollision(touchedObject);
            }
            else if(touchingRock && touchedObject.CompareTag("Rock"))
            {
                AddCollision(touchedObject);
            }
            else if (!touchingInvSlot && !touchingVentCover && !touchingOpenVent && !touchingItem && !touchingGroundLadder
                && !touchingVentLadder && !touchingRoofLadder && !touchingRock)
            {
                AddCollision(touchedObject);
            }
        }

        Debug.Log(isTouchingSlats);
    }

    public void DisableTag(string tag)
    {
        disabledTags.Add(tag);
    }

    public void EnableTag(string tag)
    {
        disabledTags.Remove(tag);
    }

    private void AddCollision(GameObject obj)
    {
        switch (obj.tag)
        {
            case "Item":
                items.Add(obj);
                break;
            case "InvSlot":
                invSlots.Add(obj);
                break;
            case "Desk":
                desks.Add(obj);
                break;
            case "DeskSlot":
                deskSlots.Add(obj);
                break;
            case "DeskPanel":
                deskPanels.Add(obj);
                break;
            case "Wall":
                walls.Add(obj);
                break;
            case "Bars":
                bars.Add(obj);
                break;
            case "Fence":
                fences.Add(obj);
                break;
            case "ElectricFence":
                electricFences.Add(obj);
                break;
            case "Button":
                buttons.Add(obj);
                break;
            case "IDSlot":
                idSlots.Add(obj);
                break;
            case "IDPanel":
                idPanels.Add(obj);
                break;
            case "Digable":
                floors.Add(obj);
                break;
            case "VentCover":
                ventCovers.Add(obj);
                break;
            case "OpenVent":
                openVents.Add(obj);
                break;
            case "Slats":
                slats.Add(obj);
                break;
            case "Ladder(Ground)":
                groundLadders.Add(obj);
                break;
            case "Ladder(Vent)":
                ventLadders.Add(obj);
                break;
            case "Ladder(Roof)":
                roofLadders.Add(obj);
                break;
            case "RoofLedge":
                roofLedges.Add(obj);
                break;
            case "HoleDown":
                holeDowns.Add(obj);
                break;
            case "HoleUp":
                holeUps.Add(obj);
                break;
            case "Dirt":
                dirts.Add(obj);
                break;
            case "EmptyDirt":
                emptyDirts.Add(obj);
                break;
            case "Rock":
                rocks.Add(obj);
                break;
        }
    }

    private void ClearCollisions()
    {
        items.Clear();
        invSlots.Clear();
        desks.Clear();
        deskSlots.Clear();
        deskPanels.Clear();
        walls.Clear();
        bars.Clear();
        fences.Clear();
        electricFences.Clear();
        buttons.Clear();
        idSlots.Clear();
        idPanels.Clear();
        floors.Clear();
        ventCovers.Clear();
        openVents.Clear();
        slats.Clear();
        groundLadders.Clear();
        ventLadders.Clear();
        roofLadders.Clear();
        roofLedges.Clear();
        holeDowns.Clear();
        holeUps.Clear();
        dirts.Clear();
        emptyDirts.Clear();
        rocks.Clear();
    }

    private GameObject GetFirst(HashSet<GameObject> set)
    {
        foreach (var obj in set)
        {
            return obj;
        }
        return null;
    }
}