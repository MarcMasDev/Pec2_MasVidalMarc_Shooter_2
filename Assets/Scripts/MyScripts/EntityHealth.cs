using UnityEngine;
public interface IDamageable
{
    void TakeDamage(float amount);
}
public class EntityHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private CharacterBlackboard m_StateBlackboard;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxShield = 100f;
    [SerializeField] [Range(0,1)] private float shieldProtection = 0.75f;
    private float currentHealth;
    private float currentShield;

    private void Start()
    {
        Heal(maxHealth, maxShield); //Updates UI and sets init values
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0) return;

        float damageToShield = amount * shieldProtection;
        float damageToHealth = amount - damageToShield;

        if (currentShield > 0)
        {
            if (currentShield >= damageToShield) currentShield -= damageToShield;
            else //overflow
            {
                float overflow = damageToShield - currentShield;
                currentShield = 0;
                damageToHealth += overflow;
            }
        }
        else damageToHealth = amount;

        currentHealth -= damageToHealth;

        if (m_StateBlackboard != null) m_StateBlackboard.TriggerHurt(new Vector2(currentHealth,maxHealth), new Vector2(currentShield, maxShield));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healthAmount, float shieldAmount)
    {
        Mathf.Clamp(currentHealth, currentHealth += healthAmount, maxHealth);
        Mathf.Clamp(currentShield, currentShield += shieldAmount, maxShield);

        if (m_StateBlackboard != null) m_StateBlackboard.TriggerHeal(new Vector2(currentHealth, maxHealth), new Vector2(currentShield, maxShield));
    }
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        // Logic for death (disable scripts, play anim, etc.)
    }
}
