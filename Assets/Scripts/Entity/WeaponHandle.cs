using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandle : MonoBehaviour
{
    [SerializeField] private float primaryMeleeDamage = 50f;
    [SerializeField] private float secondaryMeleeDamage = 100f;
    [SerializeField] private float primaryMeleeCooldown = 0.5f;
    [SerializeField] private float secondaryMeleeCooldown = 1.25f;
    public Sprite meleeIcon;

    [SerializeField] private GameObject[] weapons = new GameObject[3];
    [SerializeField] private Detector detector;
    private GameObject detectedEnemy;
    private int activeWeaponIndex = 2;
    private float currentMeleeCooldown;

    private void Awake()
    {
        GameObject meleeObject = transform.Find("Knife").gameObject;
        weapons[2] = meleeObject;
    }

    private void Update()
    {
        detectedEnemy = detector.GetDetectedEntity();

        if (currentMeleeCooldown >= 0)
            currentMeleeCooldown -= Time.deltaTime;
    }

    public void SwitchWeapon(int direction)
    {
        if (CountWeapon() <= 0) return;

        if (activeWeaponIndex != 2)
            if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading())
                weapons[activeWeaponIndex].GetComponent<Weapon>().CancelReload();

        if (direction == 1)
        {
            switch(activeWeaponIndex)
            {
                case 0:
                    //if (weapons[2] == null) activeWeaponIndex = 1;
                    //else activeWeaponIndex = 2;

                    activeWeaponIndex = 2;
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
                    //if (weapons[2] == null) activeWeaponIndex = 0;
                    //else activeWeaponIndex = 2;

                    activeWeaponIndex = 2;
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
        if (activeWeaponIndex == 2)
        {
            if (!context.performed) return;
            StrikeMelee(context.ReadValue<float>());
        }
        else
        {
            if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading()) return;

            weapons[activeWeaponIndex].GetComponent<Weapon>().Shoot(context);
        }
    }

    public void WeaponReload()
    {
        if (activeWeaponIndex == 2) return;

        if(weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading()) return;

        weapons[activeWeaponIndex].GetComponent<Weapon>().Reload();
    }

    public void DropWeapon()
    {
        if (CountWeapon() <= 0 || activeWeaponIndex == 2) return;

        int selectedWeapon = activeWeaponIndex;

        if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading())
            weapons[activeWeaponIndex].GetComponent<Weapon>().CancelReload();

        if (CountWeapon() - 1 <= 0)
            activeWeaponIndex = 2;
        else
            SwitchWeapon(-1);

        Destroy(weapons[selectedWeapon].gameObject);
        weapons[selectedWeapon] = null;

        RefreshWeaponInventory();
    }

    public void PickUpWeapon(GameObject weapon)
    {
        //if (activeWeaponIndex != -1)
        //{
        //    if (weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading()) return;
        //}

        if (CountWeapon() >= 2) return;

        int availableWeapons = CountWeapon();
        int placedWeapon = -1;

        //if (weapon.GetComponent<Weapon>().GetWeaponType() == WeaponType.Primary)
        //{
        //    if (weapons[0] != null && weapons[1] != null) return;

        //    GameObject spanwedWeapon = Instantiate(weapon, transform);

        //    if (weapons[0] != null)
        //    {
        //        weapons[1] = spanwedWeapon;
        //        placedWeapon = 1;
        //    }
        //    else
        //    {
        //        weapons[0] = spanwedWeapon;
        //        placedWeapon = 0;
        //    }
        //}
        //else if (weapon.GetComponent<Weapon>().GetWeaponType() == WeaponType.Secondary)
        //{
        //    if (weapons[2] != null) return;

        //    GameObject spanwedWeapon = Instantiate(weapon, transform);

        //    weapons[2] = spanwedWeapon;
        //    placedWeapon = 2;
        //}

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

        if (availableWeapons <= 0 && placedWeapon != -1)
        {
            activeWeaponIndex = placedWeapon;
        }

        RefreshWeaponInventory();
    }

    public int GetWeaponMagazine()
    {
        if (CountWeapon() <= 0 || activeWeaponIndex == 2) return -1;

        return weapons[activeWeaponIndex].GetComponent<Weapon>().GetMagazineAmmo();
    }

    public int GetWeaponTotalAmmo()
    {
        if (CountWeapon() <= 0 || activeWeaponIndex == 2) return -1;

        return weapons[activeWeaponIndex].GetComponent<Weapon>().GetTotalAmmo();
    }

    private int CountWeapon()
    {
        int amount = 0;
        foreach (var weapon in weapons)
        {
            if (weapon != null)
                if (weapon.GetComponent<Weapon>() != null)
                    amount++;
        }

        return amount;
    }

    //private int CountPrimaryWeapon()
    //{
    //    int amount = 0;
    //    foreach (var weapon in weapons)
    //    {
    //        if (weapon != null)
    //            if (weapon.GetComponent<Weapon>().GetWeaponType() == WeaponType.Primary) 
    //                amount++;
    //    }

    //    return amount;
    //}

    private void RefreshWeaponInventory()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null) continue;

            if (i == activeWeaponIndex)
            {
                weapons[i].SetActive(true);

                if (activeWeaponIndex != 2)
                    if (weapons[i].GetComponent<Weapon>().GetMagazineAmmo() <= 0)
                        weapons[i].GetComponent<Weapon>().Reload();
            }
            else
                weapons[i].SetActive(false);
        }
    }

    public GameObject GetWeaponByIndex(int index)
    {
        return weapons[index];
    }

    public int GetActiveWeaponIndex()
    {
        return activeWeaponIndex;
    }

    public bool GetWeaponReloadStatus()
    {
        if (CountWeapon() <= 0 || activeWeaponIndex == 2) return false;

        return weapons[activeWeaponIndex].GetComponent<Weapon>().IsReloading();
    }

    public void StrikeMelee(float strikeType)
    {
        if (detectedEnemy == null || currentMeleeCooldown > 0) return;

        EntityHealth enemyHealth = detectedEnemy.GetComponent<EntityHealth>();
        if (enemyHealth == null) return;

        if (strikeType == 1)
        {
            enemyHealth.TakeDamage(primaryMeleeDamage);
            currentMeleeCooldown = primaryMeleeCooldown;
        }
        else if (strikeType == 2)
        {
            enemyHealth.TakeDamage(secondaryMeleeDamage);
            currentMeleeCooldown = secondaryMeleeCooldown;
        }
    }
}
