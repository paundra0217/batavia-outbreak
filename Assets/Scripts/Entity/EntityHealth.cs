using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [Range(0f, 1f), SerializeField] private float healthPercentageWarning = 0.25f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = health;
    }

    public float GetHealthByDecimal()
    {
        return currentHealth / health;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetHealthWarning()
    {
        return healthPercentageWarning * health;
    }

    public void SetHealth(float health)
    {
        this.health = health;
        currentHealth = this.health;
    }

    public void ReAdjustHealth(float newMaxHealth)
    {
        float previousMaxHealth = health;
        health = newMaxHealth;
        currentHealth = (currentHealth / previousMaxHealth) * health;
    }

    public void HealEntity(float hp)
    {
        currentHealth += hp;

        if (currentHealth > health) currentHealth = health;

        if (gameObject.CompareTag("Player"))
            UIHealth.CheckHealth(1);
    }

    public void TakeDamage(float damage, string weaponName = null)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0f;

            if (gameObject.CompareTag("Enemy"))
            {
                // if entity is enemy
                EnemyManager.RemoveEnemy(gameObject.GetComponent<Enemy>().GetEnemyID(), weaponName);
            }
            else
            {
                // if entity is player
                EnemyManager.StopAutoEnemySpawn();
                Destroy(gameObject);
            }
        }

        if (gameObject.CompareTag("Player"))
            UIHealth.CheckHealth(0);
    }
}
