using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private CharacterBlackboard m_StateBlackboard;
    [SerializeField] private EnemyInput m_EnemyInput;

    [Header("Equipamiento")]
    [SerializeField] private Weapon weapon;

    private void Update()
    {
        if (weapon == null) return;

        if (m_EnemyInput.IsShooting)
        {
            weapon.TryShoot();
        }
        else
        {
            weapon.StopShooting();
        }

        if (weapon is IReloadable reloadable)
        {
            // Si el cargador está vacío, recarga automáticamente
            if (weapon.GetCurrentAmmo().x == 0)
            {
                reloadable.Reload(false);
            }
        }
    }

    public Weapon GetActiveWeapon() => weapon;
}
