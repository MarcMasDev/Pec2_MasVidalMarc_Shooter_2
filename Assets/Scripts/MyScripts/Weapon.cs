using NaughtyAttributes;
using UnityEngine;

public interface IReloadable
{
    void Reload();
    void StopReloading();
}

/// <summary>
/// Esta clase agrupa ambas clases: armas melee y a rango.
/// Esta es su clase padre que se encarga de manejar toda la tecnología que comparten
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected CharacterBlackboard m_StateBlackboard;
    [SerializeField] protected WeaponClass weaponInfo;
    [SerializeField] protected AudioItem[] audioItems;
    [SerializeField] protected GameObject impact;

    [SerializeField] protected GameObject bloodEffect; 
    [SerializeField] private string[] bloodTags;

    [SerializeField] protected float vfxDestroyTime = 10f;
    [SerializeField] protected LayerMask impactLayer;
    [HideIf("isPlayer")][SerializeField] protected Transform origin;

    protected float nextFireTime;
    protected bool isFiring;
    private bool isPlayer => CompareTag("Player");
    public virtual void TryShoot()
    {
        if (weaponInfo == null || m_StateBlackboard.m_IsPerformingAction) return;

        if (Time.time >= nextFireTime)
        {
            ExecuteAttack();
        }
    }

    protected abstract void ExecuteAttack();

    public virtual void StopShooting() => isFiring = false;

    protected void TriggerFeedback()
    {
        m_StateBlackboard.TriggerAttack();
        if (audioItems != null) AudioManager.instance.PlayOneShootFromArray(audioItems, SoundType.shoot);
    }

    public void EnableInput() => m_StateBlackboard.m_IsPerformingAction = false;
    public void DisableInput() => m_StateBlackboard.m_IsPerformingAction = true;
    public void HideWeapon() => m_StateBlackboard.TriggerSwap();

    protected void ApplyDamage(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(weaponInfo.damage);
        }
    }

    protected void ImpactVFX(RaycastHit hit)
    {
        GameObject prefabToSpawn = impact;

        if (IsBloodAgent(hit.collider)) prefabToSpawn = bloodEffect;

        if (prefabToSpawn != null)
        {
            // Evita el z-figthing
            Vector3 spawnPos = hit.point + (hit.normal * 0.01f);
            Quaternion spawnRot = Quaternion.LookRotation(hit.normal);

            //Crea el efecto
            GameObject vfxInstance = Instantiate(prefabToSpawn, spawnPos, spawnRot);

            //Lo ponemos como hijo, asi se mueve con el padre.
            vfxInstance.transform.SetParent(hit.transform);

            Destroy(vfxInstance, vfxDestroyTime);
        }
    }
    public virtual Vector2Int GetCurrentAmmo()
    {
        return new Vector2Int(-1, -1);
    }
    public string GetWeaponName()
    {
        return weaponInfo.name;
    }
    private bool IsBloodAgent(Collider hit)
    {
        for (int i = 0; i < bloodTags.Length; i++)
        {
            if (hit.CompareTag(bloodTags[i])) return true;
        }
        return false;
    }
    protected Ray GetRayOrigin()
    {
        if (isPlayer) return Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        else return new Ray(origin.position, origin.forward); ;
    }
}
