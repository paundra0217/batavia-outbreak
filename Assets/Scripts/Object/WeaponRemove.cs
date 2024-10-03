using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRemove : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Removing weapon");

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            GameObject weaponHandle = player.transform.Find("WeaponHandle").gameObject;

            if (weaponHandle.transform.childCount <= 0) return;

            Destroy(weaponHandle.transform.GetChild(0).gameObject);
        }
    }
}
