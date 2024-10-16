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
    public Sprite weaponIcon;

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
    public float slowDownWalkSpeed = 100f;
    [SerializeField] private float horizontalSpread = 0f;
    [SerializeField] private float verticalSpread = 0f;
    [SerializeField] private float spreadIncrement = 0.1f;
    [SerializeField] private float walkingSpeedStage = 0.1f;

    [Header("Magazine and Bullet Configuration")]
    public int bulletPerMagazine;
    public int totalBullets;
    [SerializeField] private float bulletAirSpeed = 10f;
    [SerializeField] private float damage = 100f;
    [SerializeField] private float timeBulletLast = 10f;
    [SerializeField] private float fireFlashTime = 0.02f;

    private int magazine;
    private int currentTotalBullets;
    private float timePerBullet;
    private bool isFiring;
    private bool isPressed;
    private bool isReloading;
    private float fireCooldownTime;
    private float flashTime = 0f;
    private float currentSpreadStage = 0f;
    private float normalSpreadStage = 0f;
    private float maxSpreadStage = 1f;
    private float currentReloadingTime = 0f;

    private void Awake()
    {
        magazine = bulletPerMagazine;
        currentTotalBullets = totalBullets;

        projectileSpawner.GetComponent<Light>().enabled = false;
    }

    private void Start()
    {
        CalculateFireRate();
    }

    private void Update()
    {
        if (fireCooldownTime > 0)
            fireCooldownTime -= Time.deltaTime;

        if (flashTime > 0)
        {
            projectileSpawner.GetComponent<Light>().enabled = true;
            flashTime -= Time.deltaTime;
        }
        else
        {
            projectileSpawner.GetComponent<Light>().enabled = false;
        }

        if (transform.GetComponentInParent<WeaponHandle>().IsPlayerMoving())
        {
            normalSpreadStage = walkingSpeedStage;
            maxSpreadStage = 1f + walkingSpeedStage;
        }
        else
        {
            normalSpreadStage = 0f;
            maxSpreadStage = 1f;
        }

        if (currentSpreadStage != normalSpreadStage)
            currentSpreadStage -= Mathf.Sign(currentSpreadStage - normalSpreadStage) * (Time.deltaTime * 0.75f);

        if (currentReloadingTime > 0)
            currentReloadingTime -= Time.deltaTime;

    }

    private void OnDisable()
    {
        currentSpreadStage = 0f;
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
        flashTime = fireFlashTime;

        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawner.transform.position, projectileSpawner.transform.rotation);
        spawnedProjectile.GetComponent<Projectile>().SetUpProjectile(gameObject, damage, timeBulletLast);

        float currentVerticalSpread = Random.Range(-(verticalSpread * currentSpreadStage), verticalSpread * currentSpreadStage);
        float currentHorizontalSpread = Random.Range(-(horizontalSpread * currentSpreadStage), horizontalSpread * currentSpreadStage);

        //Debug.LogFormat("{0} {1}", currentVerticalSpread, currentHorizontalSpread);

        Vector3 velocityDirection = transform.TransformVector(currentHorizontalSpread, currentVerticalSpread, 1f);

        spawnedProjectile.GetComponent<Rigidbody>().velocity = velocityDirection * bulletAirSpeed;

        magazine--;

        currentSpreadStage += spreadIncrement;
        if (currentSpreadStage > maxSpreadStage)
            currentSpreadStage = maxSpreadStage;

        //print(currentSpreadStage);

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

        UIMagazine.CheckTotalAmmo(0);

        print("Reload complete");

        isReloading = false;
    }

    public void Reload()
    {
        if (magazine >= bulletPerMagazine || currentTotalBullets <= 0 || isReloading) return;

        print("Reloading");
        currentReloadingTime = reloadTime;
        StartCoroutine("ReloadAnimation");
    }

    public void CancelReload()
    {
        if (!isReloading) return;

        print("Reload cancelled");

        StopCoroutine("ReloadAniamtion");
        isReloading = false;
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

    public float GetReloadProgress()
    {
        return (reloadTime - currentReloadingTime) / reloadTime;
    }

    public void AddTotalAmmo()
    {
        currentTotalBullets += bulletPerMagazine;

        UIMagazine.CheckTotalAmmo(1);

        if (currentTotalBullets > 999) currentTotalBullets = 999;
    }
}
