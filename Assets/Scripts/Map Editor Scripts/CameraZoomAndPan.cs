using UnityEngine;

public class CameraZoomAndPan : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    private Camera cam;
    private Vector3 dragOrigin;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            Vector3 mouseWorldBeforeZoom = cam.ScreenToWorldPoint(Input.mousePosition);

            cam.orthographicSize -= scrollInput * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

            Vector3 mouseWorldAfterZoom = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 offset = mouseWorldBeforeZoom - mouseWorldAfterZoom;
            cam.transform.position += offset;
        }
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(2)) // Middle mouse button pressed
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2)) // Middle mouse held
        {
            Vector3 currentPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPosition;
            cam.transform.position += difference;
        }
    }
}
