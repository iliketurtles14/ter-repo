using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseOverlay : MonoBehaviour
{
    private GameObject MouseOverlayObject;
    private GameObject player;
    private Canvas parentCanvas; // Reference to the parent Canvas
    public Vector2 offset;

    private MouseCollisionOnItems mcs;
    private InventorySelection iss;
    private Combat combatScript;

    public Sprite mouseNormal;
    public Sprite mousePurple;
    public Sprite mouseUp;
    public Sprite mouseDown;
    public Sprite mouseRed;

    private int playerLayer;
    private int ventLayer;
    private int groundLayer;
    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "Main Menu")
        {
            MouseOverlayObject = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").gameObject;
            player = RootObjectCache.GetRoot("Player");
            parentCanvas = transform.parent.GetComponent<Canvas>();
            mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
            iss = RootObjectCache.GetRoot("ScriptObject").GetComponent<InventorySelection>();
            combatScript = player.GetComponent<Combat>();
        }
        else
        {
            MouseOverlayObject = gameObject;
            mcs = GetComponent<MouseCollisionOnItems>();
            parentCanvas = transform.parent.GetComponent<Canvas>();
        }
        // Hide the mouse cursor
        Cursor.visible = false;
        offset = new Vector2(20, -32);

        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        ventLayer = LayerMask.NameToLayer("Vents");
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            // Get the mouse position in screen space
            Vector2 mousePosition = Input.mousePosition;

            // Convert mouse position to Canvas (UI) space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.GetComponent<RectTransform>(),
                mousePosition,
                parentCanvas.worldCamera,
                out Vector2 localPoint
            );

            // Apply the offset
            localPoint += offset;

            // Set the anchoredPosition of the RectTransform
            MouseOverlayObject.GetComponent<RectTransform>().anchoredPosition = localPoint;
        }
        else
        {

            if (iss.aSlotSelected)
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mousePurple;
            }
            else if (combatScript.inAttackMode)
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseRed;
            }
            else if (mcs.isTouchingHoleDown)
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseDown;
            }
            else if (mcs.isTouchingHoleUp)
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseUp;
            }
            else if (mcs.isTouchingOpenVent && !Physics2D.GetIgnoreLayerCollision(playerLayer, groundLayer))
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseUp;
            }
            else if (mcs.isTouchingOpenVent && !Physics2D.GetIgnoreLayerCollision(playerLayer, ventLayer))
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseDown;
            }
            else if (mcs.isTouchingGroundLadder)
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseUp;
            }
            else if (mcs.isTouchingRoofLadder)
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseDown;
            }
            else if (mcs.isTouchingVentLadder && mcs.touchedVentLadder.name.StartsWith("LadderDown (Vent)"))
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseDown;
            }
            else if (mcs.isTouchingVentLadder && mcs.touchedVentLadder.name.StartsWith("LadderUp (Vent)"))
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseUp;
            }
            else
            {
                MouseOverlayObject.GetComponent<Image>().sprite = mouseNormal;
            }

            if (MouseOverlayObject.GetComponent<Image>().sprite == mouseNormal ||
                MouseOverlayObject.GetComponent<Image>().sprite == mousePurple)
            {
                offset = new Vector2(20, -32);
                MouseOverlayObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 65);
            }
            else if (MouseOverlayObject.GetComponent<Image>().sprite == mouseUp ||
                MouseOverlayObject.GetComponent<Image>().sprite == mouseDown)
            {
                offset = new Vector2(5, -25);
                MouseOverlayObject.GetComponent<RectTransform>().sizeDelta = new Vector2(45, 50);
            }

            // Get the mouse position in screen space
            Vector2 mousePosition = Input.mousePosition;

            // Convert mouse position to Canvas (UI) space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.GetComponent<RectTransform>(),
                mousePosition,
                parentCanvas.worldCamera,
                out Vector2 localPoint
            );

            // Apply the offset
            localPoint += offset;

            // Set the anchoredPosition of the RectTransform
            MouseOverlayObject.GetComponent<RectTransform>().anchoredPosition = localPoint;
        }
    }

}
