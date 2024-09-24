using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float damagePerStrike = 10f;
    [SerializeField] private float timePerStrike = 2f;

    private float strikeCooldown = 0f;
    private GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if (strikeCooldown <= 0f)
            StrikePlayer();

        strikeCooldown -= Time.deltaTime;
    }

    private void StrikePlayer()
    {
        EntityHealth entityHealth = player.GetComponent<EntityHealth>();
        entityHealth.TakeDamage(damagePerStrike);

        strikeCooldown = timePerStrike;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Enemy Detects");

        if (collision.gameObject.CompareTag("Player"))
            player = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            player = null;
    }
}
