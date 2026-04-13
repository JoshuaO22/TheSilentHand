using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public bool destroyOnDeath = true;
    public bool IsDead => currentHealth <= 0;

    public event UnityAction<float, float> OnHealthChanged;
    public event UnityAction OnDeath;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damageAmount)
    {
        if (IsDead) return;

        currentHealth = Mathf.Max(0, currentHealth - damageAmount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (IsDead)
        {
            Die();
        }
    }

    public virtual void Heal(float healAmount)
    {
        if (IsDead) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke();

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}