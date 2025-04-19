using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public float moveSpeed = 2f;
    protected float originalSpeed;
    public float chaseRange = 2f;
    public float attackRange = 2f;

    protected bool isChasing = false;
    protected float attackCooldown = 2f;
    protected float lastAttackTime;
    protected Collider enemyCollider;

    public HealthBarController healthBarController;
    public HealthController healthController;

    protected virtual void Start()
    {
        originalSpeed = moveSpeed;
        enemyCollider = GetComponent<Collider>();

        if (healthController != null)
        {
            healthController.Init(50);
            healthBarController.Init(healthController.GetMaxHealth());
            healthController.OnDamageTaken += healthBarController.UpdateHealth;
            healthController.OnHealed += healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged += healthBarController.UpdateMaxHealth;
        }
    }

    protected virtual void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <= chaseRange;

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        TryAttack(distanceToPlayer);
    }

    protected virtual void TryAttack(float distanceToPlayer)
    {
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            AttackPlayer();
        }
    }

    protected virtual void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    protected abstract void Patrol();
    protected abstract void AttackPlayer();

    protected virtual void OnDestroy()
    {
        if (healthController != null)
        {
            healthController.OnDamageTaken -= healthBarController.UpdateHealth;
            healthController.OnHealed -= healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged -= healthBarController.UpdateMaxHealth;
        }
    }
}
