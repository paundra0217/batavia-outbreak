using TMPro;
using UnityEngine;
using System.Collections;

public class UIMagazine : MonoBehaviour
{
    [SerializeField] private TMP_Text labelMagazine;
    [SerializeField] private TMP_Text labelTotalAmmo;
    [SerializeField] private TMP_Text labelReloading;

    //private Weapon weapon;
    private WeaponHandle weaponHandle;
    private bool magazineAlreadyWarned;
    private bool totalAmmoAlreadyWarned;
    private Animation labelMagazineAnimation;
    private Animation labelTotalAmmoAnimation;

    private static UIMagazine _instance;

    private void Awake()
    {
        _instance = this;

        labelReloading.enabled = false;
        labelMagazineAnimation = labelMagazine.gameObject.GetComponent<Animation>();
        labelTotalAmmoAnimation = labelTotalAmmo.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            ClearAmmoIndicator();
            return;
        }

        weaponHandle = GameObject.Find("Player/WeaponHandle").GetComponent<WeaponHandle>();

        if (weaponHandle.GetActiveWeaponIndex() == 2)
        {
            ClearAmmoIndicator();
            return;
        }

        //if (GameObject.Find("Player/WeaponHandle").transform.childCount <= 0)
        //{
        //    ClearAmmoIndicator();
        //    return;
        //}

        //weapon = GameObject.Find("Player/WeaponHandle").transform.GetChild(0).GetComponent<Weapon>();

        //if (weapon == null)
        //{
        //    ClearAmmoIndicator();
        //    return;
        //}

        DisplayAmmoIndicator();

        labelReloading.enabled = weaponHandle.GetWeaponReloadStatus();

        int magazine = weaponHandle.GetWeaponMagazine();

        labelMagazine.text = magazine.ToString();
        labelTotalAmmo.text = string.Format(" / {0}", weaponHandle.GetWeaponTotalAmmo().ToString());

        if (magazine <= weaponHandle.GetWeaponMagazineWarning())
        {
            if (!magazineAlreadyWarned)
            {
                magazineAlreadyWarned = true;
                labelMagazineAnimation.Play();
            }
        }
        else
        {
            magazineAlreadyWarned = false;
            labelMagazineAnimation.Stop();
            labelMagazine.color = Color.white;
        }

        //if (totalAmmo <= weaponHandle.GetWeaponTotalAmmoWarning())
        //{
        //    if (!totalAmmoAlreadyWarned)
        //    {
        //        totalAmmoAlreadyWarned = true;
        //        labelTotalAmmoAnimation.Play();
        //    }
        //}
        //else
        //{
        //    totalAmmoAlreadyWarned = false;
        //    labelTotalAmmoAnimation.Stop();
        //    labelTotalAmmo.color = Color.white;
        //}
    }

    public static void CheckTotalAmmo(int mode)
    {
        _instance.StartCoroutine(_instance.PlayTotalAmmoUIAnimation(mode));
    }

    IEnumerator PlayTotalAmmoUIAnimation(int mode)
    {
        if (weaponHandle.GetActiveWeaponIndex() == 2) yield return null;

        labelTotalAmmoAnimation.Stop();

        if (mode == 1)
        {
            labelTotalAmmoAnimation.Play("UITotalAmmoAdd");

            yield return new WaitForSeconds(labelTotalAmmoAnimation.clip.length);
        }

        Debug.LogFormat("{0} {1}", weaponHandle.GetWeaponTotalAmmo(), weaponHandle.GetWeaponTotalAmmoWarning());

        if (weaponHandle.GetWeaponTotalAmmo() <= weaponHandle.GetWeaponTotalAmmoWarning())
        {
            labelTotalAmmoAnimation.Play();
        }
        else
        {
            _instance.labelTotalAmmo.color = Color.white;
        }
    }

    private void ClearAmmoIndicator()
    {
        //labelMagazine.text = "-";
        //labelTotalBullets.text = "/ -";

        labelMagazine.enabled = false;
        labelTotalAmmo.enabled = false;
        labelReloading.enabled = false;
    }

    private void DisplayAmmoIndicator()
    {
        labelMagazine.enabled = true;
        labelTotalAmmo.enabled = true;
    }
}
