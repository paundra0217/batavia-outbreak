using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float damagePerStrike = 10f;
    [SerializeField] private float timePerStrike = 0.5f;

    private Guid enemyID;
    private float strikeCooldown = 0f;
    private GameObject player = null;
    private CapsuleCollider capsuleColl;
    private NavMeshAgent agent;

    private void Awake()
    {
        enemyID = Guid.NewGuid();
    }

    // Start is called before the first frame update
    void Start()
    {
        capsuleColl = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        else
            agent.SetDestination(transform.position);

        DetectPlayer();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if (strikeCooldown <= 0f)
            StrikePlayer();

        strikeCooldown -= Time.deltaTime;
    }

    private void DetectPlayer()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + capsuleColl.center + Vector3.up * -capsuleColl.height * 0.5F;
        Vector3 p2 = p1 + Vector3.up * capsuleColl.height;

        if (Physics.CapsuleCast(p1, p2, capsuleColl.radius, transform.forward, out hit, 1f))
        {
            if (!hit.collider.gameObject.CompareTag("Player")) return;

            player = hit.collider.gameObject;
        }
        else
        {
            player = null;
        }
    }

    private void StrikePlayer()
    {
        EntityHealth entityHealth = player.GetComponent<EntityHealth>();
        entityHealth.TakeDamage(damagePerStrike);

        strikeCooldown = timePerStrike;
    }

    public Guid GetEnemyID()
    {
        return enemyID;
    }
}
