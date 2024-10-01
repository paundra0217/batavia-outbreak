using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = health;
    }

    public float GetHealthByDecimal()
    {
        return currentHealth / health;
    }

    public void HealEntity(float hp)
    {
        currentHealth += hp;

        if (currentHealth > health) currentHealth = health;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0f;

            if (gameObject.CompareTag("Enemy"))
            {
                // if entity is enemy
                EnemyManager.RemoveEnemy(gameObject.GetComponent<Enemy>().GetEnemyID());
            }
            else
            {
                // if entity is player
                EnemyManager.StopAutoEnemySpawn();
                Destroy(gameObject);
            }
        }
    }
}
