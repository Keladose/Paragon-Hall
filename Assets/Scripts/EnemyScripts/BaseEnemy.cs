using System.Collections;
using System.Collections.Generic;
using Spellect;
using System;
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
        public Rigidbody2D rb;
        protected Animator animator;
        protected bool isDead = false;

        protected SpriteRenderer spriteRenderer;
        private float wanderTime = 2f;
        private Vector2 wanderDirection;

        public AudioClip deathClunk;
        protected AudioSource audioSource;

        protected virtual void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            originalSpeed = moveSpeed;
            enemyCollider = GetComponent<Collider>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();

            if (healthController != null)
            {
                Debug.Log("Initialised health controller");
                healthController.Init(50);
                healthBarController.Init(healthController.GetMaxHealth());
                healthController.OnDamageTaken += healthBarController.UpdateHealth;
                healthController.OnHealed += healthBarController.UpdateHealth;
                healthController.OnMaxHealthChanged += healthBarController.UpdateMaxHealth;
                healthController.OnDeath += Die;

            }
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
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
            AddForceTowardsTarget(player.position, moveSpeed);
        }

        protected virtual void Patrol()
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                // Wander randomly if no patrol points are assigned
                if (wanderTime <= 0f)
                {
                    wanderTime = UnityEngine.Random.Range(1f, 3f);
                    wanderDirection = UnityEngine.Random.insideUnitCircle.normalized;
                }

                wanderTime -= Time.deltaTime;

                Vector3 wanderTarget = transform.position + (Vector3)wanderDirection * 2f;
                UpdateSpriteDirection(wanderTarget);


                AddForceTowardsTarget(wanderTarget, moveSpeed);

                return;
            }

            Vector3 targetPos = patrolPoints[targetPoint].position;
            UpdateSpriteDirection(targetPos);
            AddForceTowardsTarget(targetPos, moveSpeed);
        }

        protected abstract void AttackPlayer();


        private protected void AddForceTowardsTarget(Vector2 target, float force)
        {
            Vector2 forceDirection = (-(Vector2)transform.position + target).normalized;
            rb.AddForce(forceDirection * force);
        }

        protected virtual void Die(object o, EventArgs e)
        {
            isDead = true;

            // Play death sound
            if (deathClunk != null && audioSource != null)
            {
                audioSource.PlayOneShot(deathClunk);
            }

            if (animator != null)
            {
                animator.SetTrigger("Die");
            }

            if (enemyCollider != null) enemyCollider.enabled = false;
            moveSpeed = 0f;

            Destroy(gameObject, 0.25f);
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

        private IEnumerator DeathCleanup()
        {
            yield return new WaitForSeconds(0.25f);
            Destroy(gameObject);
        }

        protected virtual void UpdateSpriteDirection(Vector3 target)
        {
            if (spriteRenderer == null) return;

            Vector3 direction = target - transform.position;
            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
    }
}