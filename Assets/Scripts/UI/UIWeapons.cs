using UnityEngine;
using UnityEngine.UI;

public class UIWeapons : MonoBehaviour
{
    [SerializeField] private float iconWitdh = 272f;
    [SerializeField] private float iconHeight = 48f;
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
            GameObject weaponIconReloadBar = weaponIcon.transform.Find("BarReloading").gameObject;

            if (weaponHandle.GetWeaponByIndex(i) == null)
            {
                weaponIconImage.enabled = false;
                weaponIcon.GetComponent<CanvasGroup>().alpha = 0.25f;

                if (i == 2) continue;

                weaponIconReloadBar.GetComponent<CanvasGroup>().alpha = 0f;

                continue;
            }

            weaponIconImage.enabled = true;
            Weapon weapon = weaponHandle.GetWeaponByIndex(i).GetComponent<Weapon>();

            Sprite newWeaponIcon = i == 2 ? newWeaponIcon = weaponHandle.meleeIcon : newWeaponIcon = weapon.weaponIcon;
            Vector3 newWeaponIconSize = newWeaponIcon.bounds.size * 100f;   

            newWeaponIconSize.x = newWeaponIconSize.x * iconHeight / newWeaponIconSize.y;
            newWeaponIconSize.y = iconHeight;

            if (newWeaponIconSize.x > iconWitdh)
            {
                newWeaponIconSize.y = newWeaponIconSize.y * iconWitdh / newWeaponIconSize.x;
                newWeaponIconSize.x = iconWitdh;
            }

            weaponIconImage.sprite = newWeaponIcon;
            weaponIconImage.rectTransform.sizeDelta = newWeaponIconSize;
            weaponIcon.GetComponent<CanvasGroup>().alpha = weaponHandle.GetActiveWeaponIndex() == i ? 1f : 0.25f;

            if (weaponHandle.GetActiveWeaponIndex() != i)
            {
                if (i == 2) continue;

                weaponIconReloadBar.GetComponent<CanvasGroup>().alpha = 0f;
                continue;
            }

            if (weaponHandle.GetWeaponReloadStatus())
            {
                weaponIconReloadBar.GetComponent<CanvasGroup>().alpha = 0.1f;
                weaponIconReloadBar.GetComponent<Image>().fillAmount = weaponHandle.GetWeaponReloadProgress();
            }
            else
            {
                weaponIconReloadBar.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }
    }
}
