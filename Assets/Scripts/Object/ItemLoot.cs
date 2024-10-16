using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Medic,
    Ammo
}

public class ItemLoot : MonoBehaviour
{
    [SerializeField] private ItemType type;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectItem(collision.gameObject);
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.point.y <= transform.position.y)
            {
                GetComponent<SphereCollider>().isTrigger = true;
                Destroy(GetComponent<Rigidbody>());
            }
        }

        //if (collision.gameObject.layer == 3)
        //{
        //    GetComponent<SphereCollider>().isTrigger = true;
        //    Destroy(GetComponent<Rigidbody>());
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        CollectItem(other.gameObject);
    }

    private void CollectItem(GameObject player)
    {
        switch (type)
        {
            case ItemType.Medic:
                EntityHealth health = player.GetComponent<EntityHealth>();
                health.HealEntity(30f);

                break;

            case ItemType.Ammo:
                WeaponHandle handle = player.transform.Find("WeaponHandle").GetComponent<WeaponHandle>();
                GameObject weaponObject = handle.GetWeaponByIndex(handle.GetActiveWeaponIndex());

                Weapon weapon = weaponObject.GetComponent<Weapon>();
                if (weapon == null)
                {
                    weaponObject = handle.GetWeaponByIndex(0);
                    if (weaponObject == null) return;

                    weapon = weaponObject.GetComponent<Weapon>();
                }

                weapon.AddTotalAmmo();

                break;
        }

        Destroy(gameObject);
    }
}
