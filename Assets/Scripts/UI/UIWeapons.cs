using UnityEngine;
using UnityEngine.UI;

public class UIWeapons : MonoBehaviour
{
    private WeaponHandle weaponHandle;

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            GetComponent<CanvasGroup>().alpha = 0f;
            return;
        }

        GetComponent<CanvasGroup>().alpha = 1f;
        weaponHandle = GameObject.Find("Player/WeaponHandle").GetComponent<WeaponHandle>();

        RefreshWeaponIconStatus();
    }

    private void RefreshWeaponIconStatus()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject weaponIcon = transform.GetChild(i).gameObject;
            Image weaponIconImage = weaponIcon.transform.Find("ImgWeaponIcon").GetComponent<Image>();

            if (weaponHandle.GetWeaponByIndex(i) == null)
            {
                weaponIconImage.enabled = false;
                weaponIcon.GetComponent<CanvasGroup>().alpha = 0.25f;

                continue;
            }

            weaponIconImage.enabled = true;
            Weapon weapon = weaponHandle.GetWeaponByIndex(i).GetComponent<Weapon>();

            weaponIconImage.sprite = i == 2 ? weaponHandle.meleeIcon : weapon.weaponIcon;
            weaponIcon.GetComponent<CanvasGroup>().alpha = weaponHandle.GetActiveWeaponIndex() == i ? 1f : 0.25f;
        }
    }
}
