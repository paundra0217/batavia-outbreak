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

public enum WeaponType
{
    Primary,
    Secondary,
}

#if UNITY_EDITOR
[CustomEditor(typeof(Weapon), true), CanEditMultipleObjects]
public class WeaponEditor : Editor
{
    SerializedProperty firingMode;
    SerializedProperty firingCooldown;
    SerializedProperty bulletsPerBurst;
    SerializedProperty roundsPerMinute;

    private void OnEnable()
    {
        firingMode = serializedObject.FindProperty("firingMode");
        firingCooldown = serializedObject.FindProperty("firingCooldown");
        bulletsPerBurst = serializedObject.FindProperty("bulletsPerBurst");
        roundsPerMinute = serializedObject.FindProperty("roundsPerMinute");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(firingMode);

        if (firingMode.enumValueIndex == 0)
            EditorGUILayout.PropertyField(firingCooldown);

        if (firingMode.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(bulletsPerBurst);
            EditorGUILayout.PropertyField(roundsPerMinute);
        }

        if (firingMode.enumValueIndex == 2)
            EditorGUILayout.PropertyField(roundsPerMinute);

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
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private AudioClip reloadSound;

    [Header("Weapon Behaviour")]
    [SerializeField, HideInInspector]
    private FiringMode firingMode;
    [Min(0f), SerializeField, HideInInspector]
    private float firingCooldown;
    [Min(2), SerializeField, HideInInspector]
    private int bulletsPerBurst;
    [SerializeField, HideInInspector]
    private int roundsPerMinute;
    [SerializeField] private float reloadTime = 3f;

    [Header("Magazine and Bullet Configuration")]
    [SerializeField] private int bulletPerMagazine;
    [SerializeField] private int totalBullets;
    [SerializeField] private float bulletAirSpeed = 10f;
    [SerializeField] private float damage = 100f;
    [SerializeField] private float timeBulletLast = 10f;

    private int magazine;
    private int currentTotalBullets;
    private float timePerBullet;
    private bool isFiring;
    private bool isPressed;
    private bool isReloading;
    private float fireCooldownTime;

    private void Start()
    {
        magazine = bulletPerMagazine;
        currentTotalBullets = totalBullets;

        CalculateFireRate();
    }

    private void Update()
    {
        if (fireCooldownTime >= 0)
            fireCooldownTime -= Time.deltaTime;
    }

    private void CalculateFireRate()
    {
        if (firingMode == FiringMode.Auto || firingMode == FiringMode.Burst)
            timePerBullet = 60f / roundsPerMinute;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (firingMode != FiringMode.Auto && !context.performed) return;

        if (isFiring || isReloading || magazine <= 0) return;

        switch (firingMode)
        {
            case FiringMode.Single:
                if (fireCooldownTime <= 0)
                {
                    SpawnProjectile();
                    fireCooldownTime = firingCooldown;
                }
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
        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawner.transform.position, projectileSpawner.transform.rotation);
        spawnedProjectile.GetComponent<Projectile>().SetDamage(damage);
        spawnedProjectile.GetComponent<Projectile>().SetBulletLast(timeBulletLast);
        spawnedProjectile.GetComponent<Rigidbody>().velocity = transform.forward * bulletAirSpeed;

        magazine--;

        if (magazine <= 0)
        {
            StopAllCoroutines();
            Reload();
            return;
        }
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

    public void UpdateFireRate()
    {
        CalculateFireRate();
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }
}
