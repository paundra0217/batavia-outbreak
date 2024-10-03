using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDebugging : MonoBehaviour
{
    private EntityHealth entityHealth;

    // Start is called before the first frame update
    void Start()
    {
        entityHealth = GameObject.FindWithTag("Player").GetComponent<EntityHealth>();
    }

    public void DebugTakeDamage()
    {
        entityHealth.TakeDamage(10f);
    }

    public void DebugHealEntity()
    {
        entityHealth.HealEntity(10f);
    }

    public void DebugSpawnEnemy()
    {
        EnemyManager.DebugSpawnEnemy();
    }

    public void DebugToggleAutoEnemySpawn()
    {
        EnemyManager.DebugToggleAutoEnemySpawn();
    }

    public void DebugSpawnPlayer()
    {

    }
}
