using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;

namespace Spellect
{
    public abstract class BaseEnemy : MonoBehaviour
    {
        public Transform[] patrolPoints;
        protected int targetPoint;

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

        protected Animator animator;
        protected bool isDead = false;

        protected SpriteRenderer spriteRenderer;

        protected virtual void Start()
        {
            originalSpeed = moveSpeed;
            enemyCollider = GetComponent<Collider>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
            UpdateSpriteDirection(player.position);
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        protected virtual void Patrol()
        {

            Vector3 targetPos = patrolPoints[targetPoint].position;
            UpdateSpriteDirection(targetPos);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        protected abstract void AttackPlayer();
        public virtual void TakeDamage(float amount)
        {
            if (isDead || healthController == null) return;

            healthController.TakeDamage(amount);

            if (healthController.GetHealth() <= 0)
            {
                Die();
            }
        }
        protected virtual void OnDestroy()
        {
            if (healthController != null)
            {
                healthController.OnDamageTaken -= healthBarController.UpdateHealth;
                healthController.OnHealed -= healthBarController.UpdateHealth;
                healthController.OnMaxHealthChanged -= healthBarController.UpdateMaxHealth;
            }
        }
        protected virtual void Die()
        {
            isDead = true;

            if (animator != null)
            {
                animator.SetTrigger("Die"); // Or animator.SetBool("isDead", true);
            }

            // Disable collider & movement if needed
            if (enemyCollider != null) enemyCollider.enabled = false;
            moveSpeed = 0f;

            Destroy(gameObject, 0.25f); // Adjust time based on animation length
        }

        private IEnumerator DeathCleanup()
        {
            yield return new WaitForSeconds(0.25f); // Adjust to your animation length
            Destroy(gameObject);
        }

        protected virtual void UpdateSpriteDirection(Vector3 Target)
        {
            if (spriteRenderer == null) return;

            Vector3 direction = Target - transform.position;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }

    }
}
