using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private EntityHealth entityHealth;
    private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        entityHealth = gameObject.GetComponentInParent<EntityHealth>();
        healthBar = transform.Find("BarFill").GetComponent<Image>();
    }

    private void LateUpdate()
    {
        healthBar.fillAmount = entityHealth.GetHealthByDecimal();

        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
