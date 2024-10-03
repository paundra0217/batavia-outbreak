using UnityEngine;
using TMPro;

public class UICurrency : MonoBehaviour
{
    [SerializeField] private TMP_Text labelTotalCurrency;

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        labelTotalCurrency.text = "¤ " + player.GetComponent<PlayerCurrency>().GetCurrency().ToString();
    }
}
