using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum FiringMode
{
    Single,
    Burst,
    Auto
}

#if UNITY_EDITOR
[CustomEditor(typeof(Weapon), true), CanEditMultipleObjects]
public class WeaponEditor : Editor
{
    SerializedProperty firingMode;
    SerializedProperty bulletsPerBurst;
    SerializedProperty timePerBullet;

    private void OnEnable()
    {
        firingMode = serializedObject.FindProperty("firingMode");
        bulletsPerBurst = serializedObject.FindProperty("bulletsPerBurst");
        timePerBullet = serializedObject.FindProperty("timePerBullet");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(firingMode);

        if (firingMode.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(bulletsPerBurst);
            EditorGUILayout.PropertyField(timePerBullet);
        }

        if (firingMode.enumValueIndex == 2)
            EditorGUILayout.PropertyField(timePerBullet);

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
#endif

public class Weapon : MonoBehaviour
{
    [Header("Weapon Configuration")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projectileSpawner;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private AudioClip reloadSound;

    [Header("Weapon Behaviour")]
    [SerializeField, HideInInspector]
    private FiringMode firingMode;
    [Min(2), SerializeField, HideInInspector]
    private int bulletsPerBurst;
    [Min(0.001f), SerializeField, HideInInspector]
    private float timePerBullet;
    [SerializeField] private float reloadTime = 3f;

    [Header("Magazine and Bullet Configuration")]
    [SerializeField] private int bulletPerMagazine;
    [SerializeField] private int totalBullets;
    [SerializeField] private float bulletAirSpeed = 10f;
    [SerializeField] private float damage = 100f;
    [SerializeField] private float timeBulletLast = 10f;

    private int magazine;
    private int currentTotalBullets;
    private bool isFiring;
    private bool isPressed;
    private bool isReloading;

    private void Start()
    {
        magazine = bulletPerMagazine;
        currentTotalBullets = totalBullets;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (firingMode != FiringMode.Auto && !context.performed) return;

        if (isFiring || isReloading || magazine <= 0) return;

        switch (firingMode)
        {
            case FiringMode.Single:
                SpawnProjectile();
                break;

            case FiringMode.Burst:
                StartCoroutine("BurstFire");
                break;

            case FiringMode.Auto:
                if (context.performed)
                    StartCoroutine("AutoFire");
                else if (context.canceled)
                    StopCoroutine("AutoFire");
                break;
        }
    }

    IEnumerator BurstFire()
    {
        isFiring = true;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            SpawnProjectile();
            
            yield return new WaitForSeconds(timePerBullet);
        }

        isFiring = false;
    }

    IEnumerator AutoFire()
    {
        while (true)
        {
            SpawnProjectile();

            yield return new WaitForSeconds(timePerBullet);
        }
    }

    private void SpawnProjectile()
    {
        magazine--;
        print(magazine);

        if (magazine <= 0)
        {
            StopAllCoroutines();
            Reload();
            return;
        }

        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawner.transform.position, projectileSpawner.transform.rotation);
        spawnedProjectile.GetComponent<Projectile>().SetDamage(damage);
        spawnedProjectile.GetComponent<Projectile>().SetBulletLast(timeBulletLast);
        spawnedProjectile.GetComponent<Rigidbody>().velocity = transform.forward * bulletAirSpeed;
    }

    IEnumerator ReloadAnimation()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int usedBullets = bulletPerMagazine - magazine;

        if (currentTotalBullets >= bulletPerMagazine)
        {
            magazine = bulletPerMagazine;
            currentTotalBullets -= usedBullets;
        }
        else
        {
            magazine += currentTotalBullets;
            currentTotalBullets = 0;
        }

        print("Reload complete");

        isReloading = false;
    }

    public void Reload()
    {
        if (magazine >= bulletPerMagazine || currentTotalBullets <= 0 || isReloading) return;

        print("Reloading");
        StartCoroutine("ReloadAnimation");
    }

    public int GetMagazineAmmo()
    {
        return magazine;
    }

    public int GetTotalAmmo()
    {
        return currentTotalBullets;
    }

    public bool IsReloading()
    {
        return isReloading;
    }
}
