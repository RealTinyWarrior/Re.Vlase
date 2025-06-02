using UnityEngine;

public class Vlaze : MonoBehaviour
{
    public GameObject agentSprite;

    public Color color;
    public float speed = 5;
    public int minDamage = 4;
    public int maxDamage = 9;
    public bool isAlien = false;

    EntitySpawner entityManager;
    AudioSource blastAudio;
    GameManager gameManager;
    HealthManager agentHealth;
    EffectManager effect;
    Vector2 distance;
    GameObject agent;
    Rigidbody2D rb;


    public void ApplyColor()
    {
        if (!entityManager.reverseAlly)
        {
            if (isAlien)
            {
                // Lime Green
                color = new Color(0.537f, 0.953f, 0.212f);
            }

            // Red which is assigned in the hirerarchy
            else GetComponent<SpriteRenderer>().color = color;
        }

        else
        {
            // Means fake alien aka reversed alien
            if (isAlien)
            {
                // Red
                GetComponent<SpriteRenderer>().color = color;
            }

            else
            {
                // Lime
                color = new Color(0.537f, 0.953f, 0.212f);
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    void Start()
    {
        agent = GameObject.FindGameObjectWithTag("Agent");
        agentHealth = agent.GetComponent<HealthManager>();
        rb = GetComponent<Rigidbody2D>();

        GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        gameManager = gameManagerObject.GetComponent<GameManager>();
        effect = gameManagerObject.GetComponent<EffectManager>();
        entityManager = gameManagerObject.GetComponent<EntitySpawner>();
        blastAudio = GameObject.FindGameObjectWithTag("BlastAudio").GetComponent<AudioSource>();

        ApplyColor();
    }

    void Update()
    {
        distance = agent.transform.position - transform.position;

        float degree = GetDegree(distance);
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Agent"))
        {
            if (!isAlien) agentHealth.Damage(Random.Range(minDamage, maxDamage));
            else agentHealth.Heal(Random.Range(minDamage, maxDamage));

            Kill();
        }

        else if (collision.CompareTag("Bullet"))
        {
            if (!isAlien)
            {
                gameManager.IncrementScore(10);
                gameManager.IncrementIgnition();
            }

            else gameManager.IncrementScore(-25);

            Destroy(collision.gameObject);
            blastAudio.Play();
            Kill();
        }
    }

    void Kill()
    {
        Vector2 position = transform.position + (agent.transform.position - transform.position).normalized * 0.5f;
        effect.Create(0, 0.45f, position, color, 1);
        Destroy(gameObject);

        blastAudio.Play();
    }

    void FixedUpdate()
    {
        Vector2 direction = distance.normalized;
        rb.linearVelocity = speed * direction;
    }

    float GetDegree(Vector2 distance)
    {
        // Adding 90 degrees because flame points upwards on 0 degree and we want it's bottom to face Agent
        float degree = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg + 90 * (isAlien ? -1 : 1);
        return degree < 0 ? 360 + degree : degree;
    }
}