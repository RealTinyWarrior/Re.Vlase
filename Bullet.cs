using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector2 direciton;
    public float speed;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = speed * direciton;
    }
}
