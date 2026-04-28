using System.Collections;
using UnityEngine;
public static class DamageSettings
{
    public static float GetHeadshotDamageMultiplier() => 2f;
    public static float GetLimbDamageMultiplier() => 0.75f;
}

public class WeaponController : MonoBehaviour
{
    //[SerializeField] private CharacterBlackboard m_StateBlackboard;

    //[Header("Weapon Settings")]
    //[SerializeField] private WeaponClass weaponInfo;
    //[SerializeField] private LayerMask enemyLayer;

    //private bool isFiring;

    //private float nextFireTime;
    //private int currentAmmo;
    //private int currentClips;

    //[Header("Effects Settings")]
    //[SerializeField] private GameObject impact;
    //[SerializeField] private float destroyTime = 10f;
    //[SerializeField] private ParticleSystem muzzleFlash;

    //[Header("Sound Settings")]
    //[SerializeField] private AudioItem[] audioItems;

    //private void Start()
    //{
    //    if (weaponInfo != null)
    //    {
    //        currentClips = weaponInfo.maxClips;
    //        currentAmmo = weaponInfo.clipSize;
    //    }
    //}

    //public void Shoot()
    //{
    //    if (weaponInfo == null || m_StateBlackboard.m_IsPerformingAction) return;

    //    if (weaponInfo.isMelee)
    //    {
    //        if (Time.time >= nextFireTime)
    //        {
    //            TriggerAttackEvents();
    //            nextFireTime = Time.time + (1f / weaponInfo.fireRate);
    //        }
    //    }
    //    else
    //    {
    //        switch (weaponInfo.fireMode)
    //        {
    //            case FireMode.SemiAuto:
    //                ShootWeapon();
    //                break;

    //            case FireMode.FullAuto:
    //                StartFullAuto();
    //                break;

    //            case FireMode.Burst:
    //                StartBurst();
    //                break;
    //        }
    //    }
    //}

    //public void OnTriggerReleased()
    //{
    //    isFiring = false;
    //}

    //private void StartFullAuto()
    //{
    //    if (isFiring) return;

    //    StartCoroutine(FullAutoRoutine());
    //}
    //private void StartBurst()
    //{
    //    if (isFiring) return;

    //    StartCoroutine(BurstFire());
    //}
    //private void ShootWeapon()
    //{
    //    if (Time.time < nextFireTime || m_StateBlackboard.m_IsPerformingAction) return;

    //    if (HasNoAmmo())
    //    {
    //        AudioManager.instance.PlayOneShootFromArray(audioItems, SoundType.emptyShoot);
    //        return;
    //    }

    //    PerformRaycastShoot();

    //    //Feedback
    //    if (m_StateBlackboard != null) m_StateBlackboard.TriggerShoot();
    //    if (muzzleFlash != null) muzzleFlash.Play();
    //    TriggerAttackEvents();


    //    nextFireTime = Time.time + (1f / weaponInfo.fireRate);
    //}
    //private void TriggerAttackEvents()
    //{
    //    if (m_StateBlackboard != null) m_StateBlackboard.TriggerShoot();
    //    if (audioItems != null) AudioManager.instance.PlayOneShootFromArray(audioItems, SoundType.shoot);
    //}

    //public void MeleeAttack()
    //{
    //    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //    RaycastHit hit;

    //    // Use a SphereCast for a more generous hit detection volume
    //    // Tip: You might want to add a LayerMask here so you don't hit yourself!
    //    if (Physics.SphereCast(ray, weaponInfo.hitRadius, out hit, weaponInfo.meleeRange, enemyLayer))
    //    {
    //        // 1. Visual Feedback
    //        if (impact != null)
    //        {
    //            Vector3 spawnPos = hit.point + (hit.normal * 0.02f);
    //            Quaternion spawnRot = Quaternion.LookRotation(hit.normal);
    //            GameObject impactEffect = Instantiate(impact, spawnPos, spawnRot);
    //            Destroy(impactEffect, destroyTime);
    //        }

    //        // 2. Damage Logic
    //        // This looks for a script on the object hit that has a "TakeDamage" method
    //        //if (hit.collider.TryGetComponent(out IDamageable damageable))
    //        //{
    //        //    // You can use the damage value from the ScriptableObject
    //        //    damageable.TakeDamage(weaponInfo.damage);
    //        //}
    //    }
    //}

    //// Shoot script facilitada en los apuntes
    //private void PerformRaycastShoot()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
    //    {

    //        // Impact effect -> Destruyelo en "destroyTime" segundos

    //        // Calcula la position un poco al frente (zfighting)
    //        Vector3 spawnPos = hit.point + (hit.normal * 0.02f);

    //        Quaternion spawnRot = Quaternion.LookRotation(hit.normal);
    //        GameObject hole = Instantiate(impact, spawnPos, spawnRot);

    //        Destroy(hole, destroyTime);
    //    }
    //}
    //private IEnumerator BurstFire()
    //{
    //    isFiring = true;

    //    for (int i = 0; i < weaponInfo.burstCount; i++)
    //    {
    //        if (HasNoAmmo()) break;

    //        ShootWeapon();

    //        yield return new WaitForSeconds(1f / weaponInfo.fireRate);
    //    }

    //    isFiring = false;
    //}

    //private IEnumerator FullAutoRoutine()
    //{
    //    isFiring = true;

    //    float delay = 1f / weaponInfo.fireRate;

    //    while (isFiring)
    //    {
    //        ShootWeapon();

    //        yield return new WaitForSeconds(delay);
    //    }
    //}
    //private bool HasNoAmmo()
    //{
    //    return currentAmmo <= 0;
    //}

    //public void Reload()
    //{
    //    if (currentClips <= 0 || currentAmmo >= weaponInfo.clipSize || weaponInfo == null || m_StateBlackboard.m_IsPerformingAction || weaponInfo.isMelee) return;
        
    //    OnTriggerReleased();
    //    DisableInput();

    //    if (m_StateBlackboard != null) m_StateBlackboard.TriggerReload();
    //}

    //public void AddClip()
    //{
    //    if (weaponInfo == null) return;

    //    if (currentClips < weaponInfo.maxClips)
    //    {
    //        currentClips++;
    //    }
    //}
    //public void StopReloading()
    //{
    //    currentAmmo = weaponInfo.clipSize;
    //    currentClips--;
    //    EnableInput();
    //}
    //public void PlayReloadAudio(int index)
    //{
    //    for (int i = 0; i < audioItems.Length; i++)
    //    {
    //        if (audioItems[i].soundType == SoundType.reload)
    //        {
    //            audioItems[i].source.PlayOneShot(audioItems[i].clips[index]);
    //        }
    //    }
    //}
    //public void HideWeapon() => m_StateBlackboard.TriggerSwap();
    //public void EnableInput() => m_StateBlackboard.m_IsPerformingAction = false;
    //public void DisableInput() => m_StateBlackboard.m_IsPerformingAction = true;
}
