using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private Image imgHealthBarFill;
    [SerializeField] private Image imgHealthBarEmpty;

    private bool alreadyWarned;
    private EntityHealth playerHealth;
    private Animation txtHealthAnimation;
    private Animation imgHealthBarFillAnimation;
    private Animation imgHealthBarEmptyAnimation;

    private void Awake()
    {
        txtHealthAnimation = txtHealth.gameObject.GetComponent<Animation>();
        imgHealthBarFillAnimation = imgHealthBarFill.gameObject.GetComponent<Animation>();
        imgHealthBarEmptyAnimation = imgHealthBarEmpty.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            HideHealth();
            return;
        }

        DisplayHealth();

        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityHealth>();
        float currentHealth = playerHealth.GetHealth();

        txtHealth.text = currentHealth.ToString();
        imgHealthBarFill.fillAmount = playerHealth.GetHealthByDecimal();

        if (currentHealth <= playerHealth.GetHealthWarning())
        {
            if (!alreadyWarned)
            {
                alreadyWarned = true;

                txtHealthAnimation.Play();
                imgHealthBarFillAnimation.Play();
                imgHealthBarEmptyAnimation.Play();
            }
        }
        else
        {
            alreadyWarned = false;

            txtHealthAnimation.Stop();
            imgHealthBarFillAnimation.Stop();
            imgHealthBarEmptyAnimation.Stop();

            txtHealth.color = Color.white;
            imgHealthBarEmpty.color = Color.white;
            imgHealthBarFill.color = Color.white;
        }
    }

    private void DisplayHealth()
    {
        txtHealth.enabled = true;
        imgHealthBarEmpty.enabled = true;
        imgHealthBarFill.enabled = true;
    }

    private void HideHealth()
    {
        txtHealth.enabled = false;
        imgHealthBarEmpty.enabled = false;
        imgHealthBarFill.enabled = false;
    }
}
