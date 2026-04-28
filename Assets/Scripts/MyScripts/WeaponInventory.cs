using UnityEngine;

[RequireComponent(typeof(IEntityInput))]
public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private CharacterBlackboard m_StateBlackboard;
    [SerializeField] private Weapon[] weapons;

    private int selectedWeaponIndex = 0;
    private IEntityInput m_EntityInput;

    private void Awake()
    {
        m_EntityInput = GetComponent<IEntityInput>();
    }

    private void OnEnable()
    {
        if (m_StateBlackboard != null) m_StateBlackboard.OnSwapTick += SwapWeapon;
    }

    private void OnDisable()
    {
        if (m_StateBlackboard != null) m_StateBlackboard.OnSwapTick -= SwapWeapon;
    }

    private void Start() => SwapWeapon();

    private void Update()
    {
        HandleWeaponSwitching();
        HandleCombatInput();
    }

    private void HandleWeaponSwitching()
    {
        int previousSelectedWeapon = selectedWeaponIndex;

        // Input: rueda
        float scroll = m_EntityInput.MouseScrollDelta;

        if (scroll > 0f) selectedWeaponIndex = (selectedWeaponIndex + 1) % weapons.Length;
        else if (scroll < 0f) selectedWeaponIndex = (selectedWeaponIndex - 1 + weapons.Length) % weapons.Length;

        // Input: números. Si se pulsa un número superior a la cantidad de armas no hace nada
        int requestedSwap = m_EntityInput.WeaponSwapIndex;
        if (requestedSwap >= 0 && requestedSwap < weapons.Length)
        {
            selectedWeaponIndex = requestedSwap;
        }

        if (previousSelectedWeapon != selectedWeaponIndex) HideWeapon();
    }

    private void HandleCombatInput()
    {
        Weapon active = GetActiveWeapon();
        if (active == null) return;

        // Shooting
        if (m_EntityInput.IsShooting) active.TryShoot();
        else active.StopShooting();

        // Reloading - Solo si el arma se puede recargar
        if (m_EntityInput.IsReloading && active is IReloadable reloadableWeapon)
        {
            reloadableWeapon.Reload();
        }
    }

    private void HideWeapon()
    {
        m_StateBlackboard.TriggerHide();
        foreach (var weapon in weapons)
        {
            if (weapon.gameObject.activeSelf)
            {
                weapon.StopShooting();
                weapon.DisableInput();
            }
        }
    }

    private void SwapWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == selectedWeaponIndex);
        }
    }

    public Weapon GetActiveWeapon()
    {
        if (selectedWeaponIndex >= 0 && selectedWeaponIndex < weapons.Length)
        {
            return weapons[selectedWeaponIndex];
        }
        return null;
    }
}