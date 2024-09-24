using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponHandle;

    public void ShootAction(InputAction.CallbackContext context)
    {
        if (weaponHandle == null) return;
        if (weaponHandle.transform.childCount <= 0) return;

        weaponHandle.transform.GetChild(0).GetComponent<Weapon>().Shoot(context);
    }

    public void ReloadAction(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (weaponHandle == null) return;
        if (weaponHandle.transform.childCount <= 0) return;

        weaponHandle.transform.GetChild(0).GetComponent<Weapon>().Reload();
    }
}
