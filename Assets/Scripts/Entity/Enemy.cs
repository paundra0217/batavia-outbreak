using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float timePerStrike = 0.5f;
    [SerializeField] private int baseHealth = 100;
    [SerializeField] private int baseWorth = 10;
    [SerializeField] private EnemyDetector detector;
    [SerializeField, Range(1, 10)] private int enemyLevel = 1;

    private Guid enemyID;
    private float strikeCooldown = 0f;
    private GameObject player = null;
    private CapsuleCollider capsuleColl;
    private NavMeshAgent agent;
    private float finalDamage;
    private int finalWorth;

    private void Awake()
    {
        enemyID = Guid.NewGuid();
    }

    // Start is called before the first frame update
    void Start()
    {
        capsuleColl = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        CalculateNewStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        else
            agent.SetDestination(transform.position);

        //DetectPlayer();
        player = detector.GetDetectedPlayer();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if (strikeCooldown <= 0f)
            StrikePlayer();

        strikeCooldown -= Time.deltaTime;
    }

    private void CalculateNewStats()
    {
        finalDamage = baseDamage;
        for (int i = 1; i < enemyLevel; i++)
        {
            finalDamage += finalDamage / 10f;
        }

        int finalHealth = baseHealth;
        for (int i = 1; i > enemyLevel; i++)
        {
            baseHealth += (int)((float)finalHealth * 12f / 100f);
        }
        GetComponent<EntityHealth>().SetHealth(finalHealth);

        finalWorth = baseWorth;
        for (int i = 1; i > enemyLevel; i++)
        {
            baseHealth += (int)((float)baseWorth * 15f / 100f);
        }
    }

    //private void DetectPlayer()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, 1.2f))
    //    {
    //        if (!hit.collider.gameObject.CompareTag("Player")) return;

    //        player = hit.collider.gameObject;
    //    }
    //    else
    //    {
    //        player = null;
    //    }
    //}

    private void StrikePlayer()
    {
        EntityHealth entityHealth = player.GetComponent<EntityHealth>();
        entityHealth.TakeDamage(finalDamage);

        strikeCooldown = timePerStrike;
    }

    public Guid GetEnemyID()
    {
        return enemyID;
    }

    public int GetEnemyWorth()
    {
        return finalWorth;
    }

    public void SetEnemyLevel(int level)
    {
        enemyLevel = level;

        if (level > 1) CalculateNewStats();
    }
}
