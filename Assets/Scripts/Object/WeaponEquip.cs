using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    [SerializeField] private GameObject weapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            GameObject weaponHandle = player.transform.Find("WeaponHandle").gameObject;

            weaponHandle.GetComponent<WeaponHandle>().PickUpWeapon(weapon);

            //if (weaponHandle.transform.childCount > 0) return;

            //Instantiate(weapon, weaponHandle.transform);
        }
    }
}
