using System.Collections;
using System.Collections.Generic;
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

            Destroy(gameObject);
        }
    }
}
