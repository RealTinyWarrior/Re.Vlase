using UnityEngine;

public class Shooter : MonoBehaviour
{
    [HideInInspector] public bool reverseAmmo = false;
    [HideInInspector] public bool reverseAim = false;
    [HideInInspector] public bool isMachineGun = false;
    public GameObject bullet;
    public GameObject agentBullet;
    public AudioSource laserAudio;
    public float attackDelay;

    Movement movement;
    float delay;

    void Start()
    {
        delay = attackDelay;
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        if (!movement.allowMovement) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float degree = GetDegree(mousePosition);
        transform.rotation = Quaternion.Euler(0, 0, degree);

        // Mouse Left Click
        if (Input.GetKeyDown(KeyCode.Mouse0) && delay >= attackDelay) CreateBullet(mousePosition, degree);

        // Mouse Left Click Hold
        if (isMachineGun)
        {
            if (Input.GetKey(KeyCode.Mouse0) && delay >= attackDelay) CreateBullet(mousePosition, degree);
        }

        if (delay < attackDelay) delay += Time.deltaTime;
    }

    void CreateBullet(Vector2 mousePosition, float degree)
    {
        laserAudio.Play();
        Bullet bulletObject;

        if (!reverseAmmo)
        {
            bulletObject = Instantiate(bullet, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, degree)).GetComponent<Bullet>();
        }
        else
        {
            bulletObject = Instantiate(agentBullet, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, degree)).GetComponent<Bullet>();
        }

        bulletObject.direciton = ((mousePosition - (Vector2)transform.position) * (reverseAim ? -1 : 1)).normalized;
        delay = 0;
    }

    float GetDegree(Vector2 position)
    {
        Vector2 distance = (Vector2)transform.position - position;

        // Adding 90 degrees because agent points upwards on 0 degree
        float degree = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg + 90;
        return degree < 0 ? 360 + degree : degree;
    }
}
