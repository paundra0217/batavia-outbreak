using UnityEngine;
using TMPro;

public class UIEnemy : MonoBehaviour
{
    [SerializeField] private TMP_Text labelTotalEnemies;

    // Update is called once per frame
    void Update()
    {
        labelTotalEnemies.text = EnemyManager.GetEnemyCount().ToString();
    }
}
