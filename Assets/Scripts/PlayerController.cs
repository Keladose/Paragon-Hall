using Spellect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 _movement;
        public Rigidbody2D rb;
        public float moveSpeed = 5f;
        public Animator animator;
        private Animator bookAnimator;
        private SpriteRenderer spriteRenderer;

        private AttackController attackController;
        public HealthBarController healthBarController;
        public HealthController healthController;
        public PlayerSoundController playerSoundController;
        bool moving = false;

        private bool isInvulnerable = false;
        private float invulnerabilityDuration = 1f;
        public float damageRadius = 0.5f;

        void Awake()
        {
            Debug.Log("Awoke player");
        }
        void Start()
        {
            if (GameManager.Instance.playerObject != null)
            {
                Destroy(this.gameObject);
                Destroy(this);
            }
            else
            {
                GameManager.Instance.playerObject = this.gameObject;
                DontDestroyOnLoad(this.gameObject);
            }
            spriteRenderer = GetComponent<SpriteRenderer>();
            attackController = GetComponent<AttackController>();


            healthController.Init(100);
            healthBarController.Init(healthController.GetMaxHealth());
            healthController.OnDamageTaken += healthBarController.UpdateHealth;
            healthController.OnHealed += healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged += healthBarController.UpdateMaxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement = _movement.normalized;
            SetAnimation(_movement);
            if (_movement.magnitude > 0 && !moving)
            {
                playerSoundController.StartFootsteps();
                moving = true;
            }
            else if (_movement.magnitude < 0.1 && moving)
            {
                playerSoundController.StopFootsteps();
                moving = false;
            }
        }

        private void LateUpdate()
        {


        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(_movement.x * moveSpeed, _movement.y * moveSpeed);

        }

        private void SetAnimation(Vector2 velocity)
        {
            if (velocity.magnitude > 0)
            {
                if (velocity.x > 0)
                {
                    animator.Play("PlayerRun");
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    if (attackController.currentBook)
                    {
                        attackController.bookAnimator.Play("Book");
                    }
                }
                else if (velocity.x < 0)
                {
                    animator.Play("PlayerRun");
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    if (attackController.currentBook)
                    {
                        attackController.bookAnimator.Play("Book");
                    }
                }
            }
            else
            {
                animator.Play("PlayerIdle");
                if (attackController.currentBook)
                {
                    attackController.bookAnimator.Play("BookIdle");
                }
            }
        }


        private void OnDestroy()
        {
            healthController.OnDamageTaken -= healthBarController.UpdateHealth;
            healthController.OnHealed -= healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged -= healthBarController.UpdateMaxHealth;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && !isInvulnerable)
            {
                Debug.Log($"Collided with enemy: {other.name}");
                TakeDamage(10);
            }
        }

        private void TakeDamage(int amount)
        {
            if (healthController != null)
            {
                healthController.TakeDamage(amount);
                Debug.Log("Player took " + amount + " damage! Health: ");

                StartCoroutine(FlashPlayerRed());

                StartCoroutine(Invulnerability());

            }

        }

        private IEnumerator Invulnerability()
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(invulnerabilityDuration);
            isInvulnerable = false;
        }
        private IEnumerator FlashPlayerRed()
        {
            Color originalColor = spriteRenderer.color;
            float flashInterval = 0.1f; // How fast it flashes
            float elapsedTime = 0f;

            while (elapsedTime < invulnerabilityDuration)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(flashInterval);
                spriteRenderer.color = originalColor;
                yield return new WaitForSeconds(flashInterval);

                elapsedTime += flashInterval * 2;
            }

            // Just to be safe, ensure it's the original color at the end
            spriteRenderer.color = originalColor;
        }
    }
}
