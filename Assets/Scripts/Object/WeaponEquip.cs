using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    [SerializeField] private GameObject weapon;

    private void OnTriggerEnter(Collider other)
    {
        print("Equipping weapon");

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            GameObject weaponHandle = player.transform.Find("WeaponHandle").gameObject;

            if (weaponHandle.transform.childCount > 0) return;

            Instantiate(weapon, weaponHandle.transform);
        }
    }
}
