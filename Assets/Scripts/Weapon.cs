using System.Collections;
using UnityEditor;
using UnityEngine;

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
    [Header("GameObject Variables")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projectileSpawner;

    [Header("Weapon Behaviour")]
    [SerializeField, HideInInspector] 
    private FiringMode firingMode;
    [Min(2), SerializeField, HideInInspector]
    private int bulletsPerBurst;
    [Min(0.001f), SerializeField, HideInInspector]
    private float timePerBullet;

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

    private void Start()
    {
        magazine = totalBullets;
        currentTotalBullets = totalBullets;
    }

    public void Shoot()
    {
        if (magazine <= 0 || isFiring) return;

        if (isPressed && firingMode != FiringMode.Auto)
        {
            isPressed = false;
            return;
        }

        switch(firingMode)
        {
            case FiringMode.Single:
                SpawnProjectile();
                break;

            case FiringMode.Burst:
                StartCoroutine("BurstFire");
                break;

            case FiringMode.Auto:
                if (isPressed)
                {
                    StartCoroutine("AutoFire");
                    isPressed = true;
                }
                else
                {
                    StopCoroutine("AutoFire");
                    isPressed = false;
                }

                break;
        }

        isPressed = true;
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
        spawnedProjectile.GetComponent<Rigidbody>().velocity = Vector3.forward * bulletAirSpeed;
    }

    public void Reload()
    {
        int usedBullets = bulletPerMagazine - magazine;

        magazine = bulletPerMagazine;
        totalBullets -= usedBullets;
    }
}
