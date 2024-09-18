using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float bulletLast;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
        // deal damage here

        Destroy(gameObject);
    }
}
