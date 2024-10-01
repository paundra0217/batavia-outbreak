using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyObject
{
    public Guid enemyID;
    public GameObject enemy;

    public EnemyObject(Guid enemyID, GameObject enemyObject)
    {
        this.enemyID = enemyID;
        this.enemy = enemyObject;
    }
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject[] spawnPoints;

    private static List<EnemyObject> enemies = new List<EnemyObject>();
    private static EnemyManager _instance;
    private static float spawnCooldown;
    private static bool toggleContinuousSpawn;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetPrespawnedEnemies();
        StartAutoEnemySpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCooldown <= 0f)
            SpawnEnemies();

        spawnCooldown -= Time.deltaTime;
    }

    private static void SpawnEnemies()
    {
        if (!toggleContinuousSpawn) return;

        SpawnEnemy();
        spawnCooldown = 1f;
    }

    public static void SpawnEnemy(int spawnPoint = -1)
    {
        GameObject spawnedEnemy;
        Guid spawnedEnemyID;

        if (_instance.spawnPoints.Length > 0)
        {
            int spawnIndex;
            if (spawnPoint <= -1)
                spawnIndex = UnityEngine.Random.Range(0, _instance.spawnPoints.Length);
            else
                spawnIndex = spawnPoint;

            spawnedEnemy = Instantiate(_instance.enemy, _instance.spawnPoints[spawnIndex].transform);
            spawnedEnemyID = spawnedEnemy.GetComponent<Enemy>().GetEnemyID();
        }
        else
        {
            spawnedEnemy = Instantiate(_instance.enemy);
            spawnedEnemyID = spawnedEnemy.GetComponent<Enemy>().GetEnemyID();
        }

        enemies.Add(new EnemyObject(spawnedEnemyID, spawnedEnemy));
    }

    public static void RemoveEnemy(Guid enemyID)
    {
        EnemyObject selectedEnemy = enemies.FirstOrDefault(e => enemyID == e.enemyID);

        Destroy(selectedEnemy.enemy);

        enemies.Remove(selectedEnemy);
    }

    public static void ClearEnemy()
    {
        foreach(EnemyObject enemy in enemies)
        {
            Destroy(enemy.enemy);
        }

        enemies.Clear();
    }

    public static int GetEnemyCount()
    {
        return enemies.Count();
    }

    public static void GetPrespawnedEnemies()
    {
        GameObject[] prespawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in prespawnedEnemies)
        {
            Guid prespawnedEnemyID = enemy.GetComponent<Enemy>().GetEnemyID();

            enemies.Add(new EnemyObject(prespawnedEnemyID, enemy));
        }
    }

    public static void StartAutoEnemySpawn()
    {
        toggleContinuousSpawn = true;
    }

    public static void StopAutoEnemySpawn()
    {
        toggleContinuousSpawn= false;
    }
}
