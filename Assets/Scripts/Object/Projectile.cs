using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float bulletLast;
    private Vector3 initialPos;
    private Vector3 collidePos;
    private float distance;
    //private Light projectileLight;
    //private float lightLast = 0.002f;

    private void Awake()
    {
        initialPos = transform.position;
        //projectileLight = GetComponent<Light>();

        //projectileLight.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        //if (lightLast <= 0)
        //    projectileLight.enabled = false;

        if (bulletLast <= 0)
            Destroy(gameObject);

        bulletLast -= Time.deltaTime;
        //lightLast -= Time.deltaTime;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetBulletLast(float time)
    {
        bulletLast = time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("Player")) return;

        collidePos = transform.position;
        distance = Mathf.Ceil(Vector3.Distance(initialPos, collidePos));
        //Debug.LogFormat("Distance: {0}m", distance);

        // deal damage here
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EntityHealth entityHealth = collision.gameObject.GetComponent<EntityHealth>();

            entityHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
