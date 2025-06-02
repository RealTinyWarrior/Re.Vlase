using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector] public bool allowMovement = true;
    [HideInInspector] public bool reverseControls = false;

    public GameObject vlazeSprite;
    public GameObject bulletSprite;
    public float speed = 400f;
    public float acceleration = 5f;
    public float deceleration = 5f;

    Vector2 currentVelocity;
    Vector2 direction;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!allowMovement) return;

        float horizontal = Input.GetAxisRaw("Horizontal") * (reverseControls ? -1 : 1);
        float vertical = Input.GetAxisRaw("Vertical") * (reverseControls ? -1 : 1);

        direction = new Vector2(horizontal, vertical).normalized;
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = speed * direction;

        if (direction.magnitude > 0.1f)
        {
            // Gradually accelerating to "target" Velocity
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Gradually decelerating to Zero Velocity
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        rb.linearVelocity = currentVelocity;
    }
}
