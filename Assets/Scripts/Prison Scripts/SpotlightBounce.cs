using UnityEngine;
public class SpotlightBounce : MonoBehaviour
{
    public float speed = 5f;

    public float randomBounceAngle = 20f;

    private Vector2 previousVelocity;
    private Rigidbody2D rb;
    private PauseController pc;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        Vector2 direction = Random.insideUnitCircle.normalized;
        rb.linearVelocity = direction * speed;
    }


    private void FixedUpdate()
    {
        previousVelocity = rb.linearVelocity;

        if (pc.isPaused)
        {
            rb.simulated = false;
        }
        else
        {
            rb.simulated = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        Vector2 reflected = Vector2.Reflect(previousVelocity.normalized, contact.normal);

        float angle = Random.Range(-randomBounceAngle, randomBounceAngle);
        reflected = Quaternion.Euler(0, 0, angle) * reflected;

        rb.linearVelocity = reflected * speed;
    }
}