using UnityEngine;

public class WeaponRemove : MonoBehaviour
{
    [SerializeField] private bool removeAllWeapons = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            GameObject weaponHandle = player.transform.Find("WeaponHandle").gameObject;

            if (removeAllWeapons)
                weaponHandle.GetComponent<WeaponHandle>().DropAllWeapons();
            else
                weaponHandle.GetComponent<WeaponHandle>().DropWeapon();

            //if (weaponHandle.transform.childCount <= 0) return;

            //Destroy(weaponHandle.transform.GetChild(0).gameObject);
        }
    }
}
