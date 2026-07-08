using UnityEngine;

public class MouseGrabPlayer : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool dragging;
    private Vector2 lastMousePos;
    private Vector2 throwVelocity;
    private MouseCollisionOnItems mcs;
    private Transform player;

    public float throwMultiplier = 1.5f;

    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        rb = player.GetComponent<Rigidbody2D>();
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mcs.isTouchingPlayer)
        {
            dragging = true;
            rb.bodyType = RigidbodyType2D.Kinematic;

            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (dragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            throwVelocity = (mousePos - lastMousePos) / Time.deltaTime;
            lastMousePos = mousePos;

            player.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = throwVelocity * throwMultiplier;
        }
    }
}
