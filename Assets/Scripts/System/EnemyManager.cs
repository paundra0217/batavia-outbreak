using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class WaveData
{
    public string WaveName;
    public int enemyCount;
    public GameObject[] spawnPoints;
    public float spawnInterval = 0.1f;
}

[Serializable]
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
    [SerializeField] private GameObject[] currentSpawnPoints;
    [SerializeField] private List<WaveData> waves = new List<WaveData>();
    [SerializeField] private GameObject medicItem;
    [SerializeField] private GameObject ammoItem;

    private static List<EnemyObject> enemies = new List<EnemyObject>();
    private static EnemyManager _instance;
    private static float spawnCooldown;
    private static bool toggleContinuousSpawn;
    private static WaveData currentWave;
    private static int enemiesInQueue;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //GetPrespawnedEnemies();
        //StartAutoEnemySpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCooldown <= 0f)
            SpawnEnemies();

        spawnCooldown -= Time.deltaTime;
    }

    private void SpawnEnemies()
    {
        if (!toggleContinuousSpawn) return;

        SpawnEnemy();
        spawnCooldown = 1f;
    }

    private void SpawnItem(Transform location)
    {
        int itemDropChance = UnityEngine.Random.Range(1, 6);
        //int itemDropChance = 1;
        if (itemDropChance != 5) return;

        GameObject item;
        int itemTypeDrop = UnityEngine.Random.Range(1, 3);
        switch (itemTypeDrop) 
        {
            case 1:
                item = medicItem;
                break;

            case 2:
                item = ammoItem;
                break;

            default:
                Debug.LogWarning("Random system failed");
                return;
        }

        float xDirection = UnityEngine.Random.Range(-1f, 1f);
        float zDirection = UnityEngine.Random.Range(-1f, 1f);
        Vector3 velocity = transform.TransformVector(xDirection, 0, zDirection).normalized * 5f;
        velocity.y = 12f;

        print(velocity);

        GameObject droppedItem = Instantiate(item, location.position, Quaternion.Euler(Vector3.zero));
        droppedItem.GetComponent<Rigidbody>().velocity = velocity;
    }

    public static void SpawnEnemy(int spawnPoint = -1)
    {
        GameObject spawnedEnemy;
        Guid spawnedEnemyID;

        if (_instance.currentSpawnPoints.Length > 0)
        {
            int spawnIndex;
            if (spawnPoint <= -1)
                spawnIndex = UnityEngine.Random.Range(0, _instance.currentSpawnPoints.Length);
            else
                spawnIndex = spawnPoint;

            spawnedEnemy = Instantiate(_instance.enemy, _instance.currentSpawnPoints[spawnIndex].transform);
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

        if (selectedEnemy == null) return;

        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //int enemyWorth = selectedEnemy.enemy.GetComponent<Enemy>().GetEnemyWorth();
        //player.GetComponent<PlayerCurrency>().AddCurrency(enemyWorth);

        _instance.SpawnItem(selectedEnemy.enemy.transform);

        Destroy(selectedEnemy.enemy);
        enemies.Remove(selectedEnemy);

        CheckZombiesCount();
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

    public static void StartWave(string waveName)
    {
        if (currentWave != null)
        {
            Debug.LogWarning("Wave is still running");
            return;
        }

        currentWave = _instance.waves.FirstOrDefault(e => e.WaveName == waveName);
        _instance.currentSpawnPoints = currentWave.spawnPoints;
        enemiesInQueue = currentWave.enemyCount;

        _instance.StartCoroutine("StartWaveProcess");
    }

    public static void EndWave()
    {
        if (currentWave == null)
        {
            Debug.LogWarning("No wave is running");
            return;
        }

        currentWave = null;
    }

    IEnumerator StartWaveProcess()
    {
        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            SpawnEnemy();
            enemiesInQueue--;

            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
    }

    private static void CheckZombiesCount()
    {
        if (currentWave == null) return;

        if (enemiesInQueue <= 0) return;

        if (GetEnemyCount() <= 0)
        {
            EndWave();
        }
    }

    public static void StartAutoEnemySpawn()
    {
        toggleContinuousSpawn = true;
    }

    public static void StopAutoEnemySpawn()
    {
        toggleContinuousSpawn = false;
    }

    #region debugging
    public static void DebugSpawnEnemy()
    {
        SpawnEnemy();
    }

    public static void DebugToggleAutoEnemySpawn()
    {
        toggleContinuousSpawn = !toggleContinuousSpawn;
    }
    #endregion
}