using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private Image imgHealthBarFill;
    [SerializeField] private Image imgHealthBarEmpty;

    private static bool alreadyWarned;
    private static EntityHealth playerHealth;
    private static Animation txtHealthAnimation;
    private static Animation imgHealthBarFillAnimation;
    private static Animation imgHealthBarEmptyAnimation;

    private static UIHealth _instance;

    private void Awake()
    {
        _instance = this;

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

        txtHealth.text = playerHealth.GetHealth().ToString();
        imgHealthBarFill.fillAmount = playerHealth.GetHealthByDecimal();
    }

    //public static void CheckWarnings()
    //{
    //    Debug.LogFormat("{0} {1}", playerHealth.GetHealth(), playerHealth.GetHealthWarning());

    //    if (playerHealth.GetHealth() <= playerHealth.GetHealthWarning())
    //    {
    //        if (alreadyWarned) return;

    //        alreadyWarned = true;

    //        txtHealthAnimation.Play();
    //        imgHealthBarFillAnimation.Play();
    //        imgHealthBarEmptyAnimation.Play();
    //    }
    //    else
    //    {
    //        if (!alreadyWarned) return;

    //        alreadyWarned = false;

    //        txtHealthAnimation.Stop();
    //        imgHealthBarFillAnimation.Stop();
    //        imgHealthBarEmptyAnimation.Stop();

    //        _instance.txtHealth.color = Color.white;
    //        _instance.imgHealthBarEmpty.color = Color.white;
    //        _instance.imgHealthBarFill.color = Color.white;
    //    }
    //}

    public static void CheckHealth(int mode)
    {
        _instance.StartCoroutine(_instance.PlayHealthUIAnimation(mode));
    }

    IEnumerator PlayHealthUIAnimation(int mode)
    {
        txtHealthAnimation.Stop();
        imgHealthBarFillAnimation.Stop();
        imgHealthBarEmptyAnimation.Stop();

        if (mode == 1)
        {
            txtHealthAnimation.Play("UIHealthHeal");
            imgHealthBarFillAnimation.Play("UIHealtBarFillHeal");
            imgHealthBarEmptyAnimation.Play("UIHealtBarEmptyHeal");

            yield return new WaitForSeconds(txtHealthAnimation.clip.length);
        }

        if (playerHealth.GetHealth() <= playerHealth.GetHealthWarning())
        {
            txtHealthAnimation.Play();
            imgHealthBarFillAnimation.Play();
            imgHealthBarEmptyAnimation.Play();
        }
        else
        {
            _instance.txtHealth.color = Color.white;
            _instance.imgHealthBarEmpty.color = Color.white;
            _instance.imgHealthBarFill.color = Color.white;
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
