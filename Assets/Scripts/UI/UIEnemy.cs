using UnityEngine;
using TMPro;

public class UIEnemy : MonoBehaviour
{
    [SerializeField] private TMP_Text labelTotalEnemies;

    // Update is called once per frame
    void Update()
    {
        try
        {
            labelTotalEnemies.text = EnemyManager.GetEnemyCount().ToString();
        }
        catch
        {
            Debug.LogWarning("Enemy manager is null");
            gameObject.SetActive(false);
        }
    }
}
