using UnityEngine;

public class EnemyWaveTrigger : MonoBehaviour
{
    [SerializeField] private string setName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EnemyManager.StartWave(setName);
            Destroy(gameObject);
        }
    }
}
