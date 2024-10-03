using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandle : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons = new GameObject[3];

    [SerializeField] private int activeWeaponIndex = -1;

    public void SwitchWeapon(int direction)
    {
        if (CountWeapon() <= 1) return;

        if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading()) return;

        if (direction == 1)
        {
            switch(activeWeaponIndex)
            {
                case 0:
                    if (weapons[2] == null) activeWeaponIndex = 1;
                    else activeWeaponIndex = 2;
                    break;

                case 1:
                    if (weapons[0] == null) activeWeaponIndex = 2;
                    else activeWeaponIndex = 0;
                    break;

                case 2:
                    if (weapons[1] == null) activeWeaponIndex = 0;
                    else activeWeaponIndex = 1;
                    break;
            }           
        }
        else if (direction == -1)
        {
            switch (activeWeaponIndex)
            {
                case 0:
                    if (weapons[1] == null) activeWeaponIndex = 2;
                    else activeWeaponIndex = 1;
                    break;

                case 1:
                    if (weapons[2] == null) activeWeaponIndex = 0;
                    else activeWeaponIndex = 2;
                    break;

                case 2:
                    if (weapons[0] == null) activeWeaponIndex = 1;
                    else activeWeaponIndex = 0;
                    break;
            }
        }

        RefreshWeaponInventory();
    }

    public void GoToWeapon(int index)
    {
        if (weapons[index - 1] == null) return;

        activeWeaponIndex = index - 1;

        RefreshWeaponInventory();
    }

    public void WeaponShoot(InputAction.CallbackContext context)
    {
        if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading()) return;

        weapons[activeWeaponIndex].GetComponent<Weapon>().Shoot(context);
    }

    public void WeaponReload()
    {
        if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading()) return;

        weapons[activeWeaponIndex].GetComponent<Weapon>().Reload();
    }

    public void DropWeapon()
    {
        int selectedWeapon = activeWeaponIndex;

        if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading()) return;

        if (CountWeapon() - 1 <= 0)
            activeWeaponIndex = -1;
        else
            SwitchWeapon(-1);

        Destroy(weapons[selectedWeapon].gameObject);
        weapons[selectedWeapon] = null;

        RefreshWeaponInventory();
    }

    public void PickUpWeapon(GameObject weapon)
    {
        if (activeWeaponIndex != -1)
        {
            if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading()) return;
        }

        if (CountWeapon() >= 3) return;

        int availableWeapons = CountWeapon();
        int placedWeapon = -1;

        if (weapon.GetComponent<Weapon>().GetWeaponType() == WeaponType.Primary)
        {
            if (weapons[0] != null && weapons[1] != null) return;

            GameObject spanwedWeapon = Instantiate(weapon, transform);

            if (weapons[0] != null)
            {
                weapons[1] = spanwedWeapon;
                placedWeapon = 1;
            }
            else
            {
                weapons[0] = spanwedWeapon;
                placedWeapon = 0;
            }
        }
        else if (weapon.GetComponent<Weapon>().GetWeaponType() == WeaponType.Secondary)
        {
            if (weapons[2] != null) return;

            GameObject spanwedWeapon = Instantiate(weapon, transform);

            weapons[2] = spanwedWeapon;
            placedWeapon = 2;
        }

        if (availableWeapons <= 0 && placedWeapon != -1)
        {
            activeWeaponIndex = placedWeapon;
        }

        RefreshWeaponInventory();
    }

    public int GetWeaponMagazine()
    {
        if (CountWeapon() <= 0) return -1;

        return weapons[activeWeaponIndex].GetComponent<Weapon>().GetMagazineAmmo();
    }

    public int GetWeaponTotalAmmo()
    {
        if (CountWeapon() <= 0) return -1;

        return weapons[activeWeaponIndex].GetComponent<Weapon>().GetTotalAmmo();
    }

    private int CountWeapon()
    {
        int amount = 0;
        foreach (var weapon in weapons)
        {
            if (weapon != null)
                amount++;
        }

        return amount;
    }

    private int CountPrimaryWeapon()
    {
        int amount = 0;
        foreach (var weapon in weapons)
        {
            if (weapon != null)
                if (weapon.GetComponent<Weapon>().GetWeaponType() == WeaponType.Primary) 
                    amount++;
        }

        return amount;
    }

    private void RefreshWeaponInventory()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null) continue;

            if (i == activeWeaponIndex)
                weapons[i].SetActive(true);
            else
                weapons[i].SetActive(false);
        }
    }

    public int GetWeaponIndex()
    {
        return activeWeaponIndex;
    }

    public bool GetWeaponReloadStatus()
    {
        if (CountWeapon() <= 0) return false;

        return weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading();
    }
}
