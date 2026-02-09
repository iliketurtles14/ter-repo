using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverlay : MonoBehaviour
{
    private GameObject MouseOverlayObject;
    private RectTransform MouseOverlayRectTransform;
    public Canvas parentCanvas; // Reference to the parent Canvas
    private Vector2 offset;
    void Start()
    {
        // Hide the mouse cursor
        Cursor.visible = false;
        offset = new Vector2(32, -32);
    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector2 mousePosition = Input.mousePosition;

        MouseOverlayObject = parentCanvas.transform.Find("MouseOverlay").gameObject;
        MouseOverlayRectTransform = MouseOverlayObject.GetComponent<RectTransform>();


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
        MouseOverlayRectTransform.anchoredPosition = localPoint;
    }
}
