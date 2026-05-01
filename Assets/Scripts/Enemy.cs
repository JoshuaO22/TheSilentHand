using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// TODO: Reorganize properties to fit with new NavMeshAgent
public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public bool followPlayer = true;
    public float moveSpeed = 2f;
    public float stoppingDistance = 1f;
    public float rotationSpeed = 8f;
    public float attackDamage = 2f;
    public float attackRange = 1.25f;
    public float attackCooldown = 1f;

    private Transform playerTarget;
    private float nextAttackTime;
    private NavMeshAgent navMeshAgent;

    public bool destroyOnDeath = true;
    public bool IsDead => currentHealth <= 0;

    public event UnityAction<float, float> OnHealthChanged;
    public event UnityAction OnDeath;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        if (followPlayer)
        {
            GameObject player = GameManager.Instance.PlayerController != null ? GameManager.Instance.PlayerController.gameObject : null;
            if (player != null)
            {
                playerTarget = player.transform;
            }

            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }

    protected virtual void Update()
    {
        if (!followPlayer || IsDead || playerTarget == null)
        {
            return;
        }

        Vector3 targetPosition = new(playerTarget.position.x, transform.position.y, playerTarget.position.z);
        Vector3 directionToTarget = targetPosition - transform.position;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // // Rotate towards the player
        // if (directionToTarget.sqrMagnitude > 0.0001f)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // }

        // Move towards the player stopping at distance
        if (distanceToTarget > stoppingDistance)
        {
            // transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            navMeshAgent.SetDestination(targetPosition);
        }

        // Attack if in range and cooldown has passed
        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
        {
            PlayerStats.Instance.TakeDamage(attackDamage);
            nextAttackTime = Time.time + attackCooldown;
        }
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