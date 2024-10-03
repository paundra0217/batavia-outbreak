using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponHandle;

    public void ShootAction(InputAction.CallbackContext context)
    {
        if (weaponHandle == null) return;

        //if (weaponHandle.transform.childCount <= 0) return;
        //weaponHandle.transform.GetChild(0).GetComponent<Weapon>().Shoot(context);

        weaponHandle.GetComponent<WeaponHandle>().WeaponShoot(context);
    }

    public void ReloadAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (weaponHandle == null) return;
        if (weaponHandle.transform.childCount <= 0) return;

        //weaponHandle.transform.GetChild(0).GetComponent<Weapon>().Reload();

        weaponHandle.GetComponent<WeaponHandle>().WeaponReload();
    }

    public void SwitchWeaponAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (context.ReadValue<float>() > 0)
            weaponHandle.GetComponent<WeaponHandle>().SwitchWeapon(1);
        else if (context.ReadValue<float>() < 0)
            weaponHandle.GetComponent<WeaponHandle>().SwitchWeapon(-1);
    }

    public void GoToWeaponAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        weaponHandle.GetComponent<WeaponHandle>().GoToWeapon((int)context.ReadValue<float>());
    }

    public void DropWeaponAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
    }
}
