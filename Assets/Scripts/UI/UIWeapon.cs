using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIWeapon : MonoBehaviour
{
    [SerializeField] private TMP_Text labelMagazine;
    [SerializeField] private TMP_Text labelTotalBullets;
    [SerializeField] private TMP_Text labelReloading;

    private Weapon weapon;

    private void Awake()
    {
        labelReloading.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            ClearAmmoIndicator();
            return;
        }

        if (GameObject.Find("Player/WeaponHandle").transform.childCount <= 0)
        {
            ClearAmmoIndicator();
            return;
        }

        weapon = GameObject.Find("Player/WeaponHandle").transform.GetChild(0).GetComponent<Weapon>();

        if (weapon == null)
        {
            ClearAmmoIndicator();
            return;
        }

        labelReloading.enabled = weapon.IsReloading();

        labelMagazine.text = weapon.GetMagazineAmmo().ToString();
        labelTotalBullets.text = string.Format("/ {0}", weapon.GetTotalAmmo().ToString());
    }

    private void ClearAmmoIndicator()
    {
        labelMagazine.text = "-";
        labelTotalBullets.text = "/ -";
        labelReloading.enabled = false;
    }
}