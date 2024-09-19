using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float bulletLast;

    private Vector3 initialPos;
    private Vector3 collidePos;
    private float distance;

    private void Awake()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletLast <= 0)
            Destroy(gameObject);

        bulletLast -= Time.deltaTime;
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

        // deal damage here
        collidePos = transform.position;

        distance = Mathf.Ceil(Vector3.Distance(initialPos, collidePos));
        //Debug.LogFormat("Distance: {0}m", distance);

        Destroy(gameObject);
    }
}
