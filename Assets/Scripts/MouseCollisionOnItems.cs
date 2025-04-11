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
    private HashSet<GameObject> extras = new HashSet<GameObject>();
    private HashSet<GameObject> npcs = new HashSet<GameObject>();

    private HashSet<string> disabledTags = new HashSet<string>();

    [SerializeField] private bool _isTouchingItem;
    [SerializeField] private GameObject _touchedItem;
    [SerializeField] private bool _isTouchingInvSlot;
    [SerializeField] private GameObject _touchedInvSlot;
    [SerializeField] private bool _isTouchingDesk;
    [SerializeField] private GameObject _touchedDesk;
    [SerializeField] private bool _isTouchingDeskSlot;
    [SerializeField] private GameObject _touchedDeskSlot;
    [SerializeField] private bool _isTouchingDeskPanel;
    [SerializeField] private GameObject _touchedDeskPanel;
    [SerializeField] private bool _isTouchingWall;
    [SerializeField] private GameObject _touchedWall;
    [SerializeField] private bool _isTouchingBars;
    [SerializeField] private GameObject _touchedBars;
    [SerializeField] private bool _isTouchingFence;
    [SerializeField] private GameObject _touchedFence;
    [SerializeField] private bool _isTouchingElectricFence;
    [SerializeField] private GameObject _touchedElectricFence;
    [SerializeField] private bool _isTouchingButton;
    [SerializeField] private GameObject _touchedButton;
    [SerializeField] private bool _isTouchingIDSlot;
    [SerializeField] private GameObject _touchedIDSlot;
    [SerializeField] private bool _isTouchingIDPanel;
    [SerializeField] private GameObject _touchedIDPanel;
    [SerializeField] private bool _isTouchingFloor;
    [SerializeField] private GameObject _touchedFloor;
    [SerializeField] private bool _isTouchingVentCover;
    [SerializeField] private GameObject _touchedVentCover;
    [SerializeField] private bool _isTouchingOpenVent;
    [SerializeField] private GameObject _touchedOpenVent;
    [SerializeField] private bool _isTouchingSlats;
    [SerializeField] private GameObject _touchedSlats;
    [SerializeField] private bool _isTouchingGroundLadder;
    [SerializeField] private GameObject _touchedGroundLadder;
    [SerializeField] private bool _isTouchingVentLadder;
    [SerializeField] private GameObject _touchedVentLadder;
    [SerializeField] private bool _isTouchingRoofLadder;
    [SerializeField] private GameObject _touchedRoofLadder;
    [SerializeField] private bool _isTouchingRoofLedge;
    [SerializeField] private GameObject _touchedRoofLedge;
    [SerializeField] private bool _isTouchingHoleDown;
    [SerializeField] private GameObject _touchedHoleDown;
    [SerializeField] private bool _isTouchingHoleUp;
    [SerializeField] private GameObject _touchedHoleUp;
    [SerializeField] private bool _isTouchingDirt;
    [SerializeField] private GameObject _touchedDirt;
    [SerializeField] private bool _isTouchingEmptyDirt;
    [SerializeField] private GameObject _touchedEmptyDirt;
    [SerializeField] private bool _isTouchingRock;
    [SerializeField] private GameObject _touchedRock;
    [SerializeField] private bool _isTouchingExtra;
    [SerializeField] private GameObject _touchedExtra;
    [SerializeField] private bool _isTouchingNPC;
    [SerializeField] private GameObject _touchedNPC;

public bool isTouchingItem => _isTouchingItem;
    public GameObject touchedItem => _touchedItem;
    public bool isTouchingInvSlot => _isTouchingInvSlot;
    public GameObject touchedInvSlot => _touchedInvSlot;
    public bool isTouchingDesk => _isTouchingDesk;
    public GameObject touchedDesk => _touchedDesk;
    public bool isTouchingDeskSlot => _isTouchingDeskSlot;
    public GameObject touchedDeskSlot => _touchedDeskSlot;
    public bool isTouchingDeskPanel => _isTouchingDeskPanel;
    public GameObject touchedDeskPanel => _touchedDeskPanel;
    public bool isTouchingWall => _isTouchingWall;
    public GameObject touchedWall => _touchedWall;
    public bool isTouchingBars => _isTouchingBars;
    public GameObject touchedBars => _touchedBars;
    public bool isTouchingFence => _isTouchingFence;
    public GameObject touchedFence => _touchedFence;
    public bool isTouchingElectricFence => _isTouchingElectricFence;
    public GameObject touchedElectricFence => _touchedElectricFence;
    public bool isTouchingButton => _isTouchingButton;
    public GameObject touchedButton => _touchedButton;
    public bool isTouchingIDSlot => _isTouchingIDSlot;
    public GameObject touchedIDSlot => _touchedIDSlot;
    public bool isTouchingIDPanel => _isTouchingIDPanel;
    public GameObject touchedIDPanel => _touchedIDPanel;
    public bool isTouchingFloor => _isTouchingFloor;
    public GameObject touchedFloor => _touchedFloor;
    public bool isTouchingVentCover => _isTouchingVentCover;
    public GameObject touchedVentCover => _touchedVentCover;
    public bool isTouchingOpenVent => _isTouchingOpenVent;
    public GameObject touchedOpenVent => _touchedOpenVent;
    public bool isTouchingSlats => _isTouchingSlats;
    public GameObject touchedSlats => _touchedSlats;
    public bool isTouchingGroundLadder => _isTouchingGroundLadder;
    public GameObject touchedGroundLadder => _touchedGroundLadder;
    public bool isTouchingVentLadder => _isTouchingVentLadder;
    public GameObject touchedVentLadder => _touchedVentLadder;
    public bool isTouchingRoofLadder => _isTouchingRoofLadder;
    public GameObject touchedRoofLadder => _touchedRoofLadder;
    public bool isTouchingRoofLedge => _isTouchingRoofLedge;
    public GameObject touchedRoofLedge => _touchedRoofLedge;
    public bool isTouchingHoleDown => _isTouchingHoleDown;
    public GameObject touchedHoleDown => _touchedHoleDown;
    public bool isTouchingHoleUp => _isTouchingHoleUp;
    public GameObject touchedHoleUp => _touchedHoleUp;
    public bool isTouchingDirt => _isTouchingDirt;
    public GameObject touchedDirt => _touchedDirt;
    public bool isTouchingEmptyDirt => _isTouchingEmptyDirt;
    public GameObject touchedEmptyDirt => _touchedEmptyDirt;
    public bool isTouchingRock => _isTouchingRock;
    public GameObject touchedRock => _touchedRock;
    public bool isTouchingExtra => _isTouchingExtra;
    public GameObject touchedExtra => _touchedExtra;
    public bool isTouchingNPC => _isTouchingNPC;
    public GameObject touchedNPC => _touchedNPC;
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
        bool touchingDesk = false;
        bool touchingNPC = false;

        // First pass: Check for specific tags
        foreach (var collider in hitColliders)
        {
            if(collider.CompareTag("Inmate") || collider.CompareTag("Guard"))
            {
                touchingNPC = true;
            }
            if (collider.CompareTag("Desk"))
            {
                touchingDesk = true;
            }
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

            if(touchingNPC && (touchedObject.CompareTag("Inmate") || touchedObject.CompareTag("Guard")))
            {
                AddCollision(touchedObject);
            }
            else if(touchingDesk && touchedObject.CompareTag("Desk"))
            {
                AddCollision(touchedObject);
            }
            else if (touchingInvSlot && touchedObject.CompareTag("InvSlot"))
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
            else if (!touchingNPC && !touchingDesk && !touchingInvSlot && !touchingVentCover && !touchingOpenVent && !touchingItem && !touchingGroundLadder
                && !touchingVentLadder && !touchingRoofLadder && !touchingRock)
            {
                AddCollision(touchedObject);
            }
        }

        // Update serialized fields for inspector
        _isTouchingItem = items.Count > 0;
        _touchedItem = items.Count > 0 ? GetFirst(items) : null;
        _isTouchingInvSlot = invSlots.Count > 0;
        _touchedInvSlot = invSlots.Count > 0 ? GetFirst(invSlots) : null;
        _isTouchingDesk = desks.Count > 0;
        _touchedDesk = desks.Count > 0 ? GetFirst(desks) : null;
        _isTouchingDeskSlot = deskSlots.Count > 0;
        _touchedDeskSlot = deskSlots.Count > 0 ? GetFirst(deskSlots) : null;
        _isTouchingDeskPanel = deskPanels.Count > 0;
        _touchedDeskPanel = deskPanels.Count > 0 ? GetFirst(deskPanels) : null;
        _isTouchingWall = walls.Count > 0;
        _touchedWall = walls.Count > 0 ? GetFirst(walls) : null;
        _isTouchingBars = bars.Count > 0;
        _touchedBars = bars.Count > 0 ? GetFirst(bars) : null;
        _isTouchingFence = fences.Count > 0;
        _touchedFence = fences.Count > 0 ? GetFirst(fences) : null;
        _isTouchingElectricFence = electricFences.Count > 0;
        _touchedElectricFence = electricFences.Count > 0 ? GetFirst(electricFences) : null;
        _isTouchingButton = buttons.Count > 0;
        _touchedButton = buttons.Count > 0 ? GetFirst(buttons) : null;
        _isTouchingIDSlot = idSlots.Count > 0;
        _touchedIDSlot = idSlots.Count > 0 ? GetFirst(idSlots) : null;
        _isTouchingIDPanel = idPanels.Count > 0;
        _touchedIDPanel = idPanels.Count > 0 ? GetFirst(idPanels) : null;
        _isTouchingFloor = floors.Count > 0;
        _touchedFloor = floors.Count > 0 ? GetFirst(floors) : null;
        _isTouchingVentCover = ventCovers.Count > 0;
        _touchedVentCover = ventCovers.Count > 0 ? GetFirst(ventCovers) : null;
        _isTouchingOpenVent = openVents.Count > 0;
        _touchedOpenVent = openVents.Count > 0 ? GetFirst(openVents) : null;
        _isTouchingSlats = slats.Count > 0;
        _touchedSlats = slats.Count > 0 ? GetFirst(slats) : null;
        _isTouchingGroundLadder = groundLadders.Count > 0;
        _touchedGroundLadder = groundLadders.Count > 0 ? GetFirst(groundLadders) : null;
        _isTouchingVentLadder = ventLadders.Count > 0;
        _touchedVentLadder = ventLadders.Count > 0 ? GetFirst(ventLadders) : null;
        _isTouchingRoofLadder = roofLadders.Count > 0;
        _touchedRoofLadder = roofLadders.Count > 0 ? GetFirst(roofLadders) : null;
        _isTouchingRoofLedge = roofLedges.Count > 0;
        _touchedRoofLedge = roofLedges.Count > 0 ? GetFirst(roofLedges) : null;
        _isTouchingHoleDown = holeDowns.Count > 0;
        _touchedHoleDown = holeDowns.Count > 0 ? GetFirst(holeDowns) : null;
        _isTouchingHoleUp = holeUps.Count > 0;
        _touchedHoleUp = holeUps.Count > 0 ? GetFirst(holeUps) : null;
        _isTouchingDirt = dirts.Count > 0;
        _touchedDirt = dirts.Count > 0 ? GetFirst(dirts) : null;
        _isTouchingEmptyDirt = emptyDirts.Count > 0;
        _touchedEmptyDirt = emptyDirts.Count > 0 ? GetFirst(emptyDirts) : null;
        _isTouchingRock = rocks.Count > 0;
        _touchedRock = rocks.Count > 0 ? GetFirst(rocks) : null;
        _isTouchingExtra = extras.Count > 0;
        _touchedExtra = extras.Count > 0 ? GetFirst(extras) : null;
        _isTouchingNPC = npcs.Count > 0;
        _touchedNPC = npcs.Count > 0 ? GetFirst(npcs) : null;
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
            case "Extra":
                extras.Add(obj);
                break;
            case "Inmate":
                npcs.Add(obj);
                break;
            case "Guard":
                npcs.Add(obj);
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
        extras.Clear();
        npcs.Clear();
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