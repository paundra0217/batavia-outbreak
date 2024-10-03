using UnityEngine;

public enum EntityType
{
    Player,
    Enemy
}

public class Detector : MonoBehaviour
{
    [SerializeField] private EntityType entityType;
    private GameObject entity;
    private string detectingTag;

    private void Awake()
    {
        if (entityType == EntityType.Player)
            detectingTag = "Player";
        else if (entityType == EntityType.Enemy)
            detectingTag = "Enemy";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(detectingTag))
        {
            entity = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(detectingTag))
        {
            entity = null;
        }
    }

    public GameObject GetDetectedEntity()
    {
        return entity;
    }
}
