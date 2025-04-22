using Spellect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spellect.AttackController;
using static Spellect.HealthController;

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
        public SpellbookController spellbookController;
        public SpellcastingController spellCastingController;
        public EffectsController effectsController;
        public DrawableSpellController drawableSpellController;
        bool moving = false;

        private bool isInvulnerable = false;
        private float invulnerabilityDuration = 1f;
        public float damageRadius = 0.5f;
        private bool initialised = false;
        public bool canMove = true;
        private Color originalColor;
        

        void Awake()
        {
            Debug.Log("Awoke player");
        }
        void Start()
        {
            
            if (GameManager.Instance != null)
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
                    Init();
                }
            }
            else
            {
                Init();
            }
            
        }

        private void Init()
        {
            initialised = true;
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            attackController = GetComponent<AttackController>();
            originalColor = spriteRenderer.color;

            healthController.Init(100);
            healthBarController.Init(healthController.GetMaxHealth());
            healthController.OnDamageTaken += healthBarController.UpdateHealth;
            healthController.OnDamageTaken += ShowDamage;
            healthController.OnHealed += healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged += healthBarController.UpdateMaxHealth;
            if (GameManager.Instance != null)
            {            
                healthController.OnDeath += GameManager.Instance.OnDeath;
            }

            healthController.OnDeath += OnDeath;
            if (spellbookController != null)
            {
                spellbookController.OnBookChanged += attackController.ChangeBook;
                attackController.OnAttackSpell += spellbookController.AnimateSpell;
                attackController.OnAttackSpell += OnAttackSpell;
                spellCastingController.OnSpellCast += attackController.OnSpellCast;
                if (spellCastingController != null)
                {
                    spellbookController.OnBookChanged += spellCastingController.ChangeSpell;
                }
            }
            if (spellCastingController != null && drawableSpellController != null)
            {
                spellCastingController.OnSpellCast += drawableSpellController.StartDrawing;
            }
            if (drawableSpellController != null)
            {
                drawableSpellController.OnDrawingFinish += attackController.OnDrawingFinished;
            }
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
                //playerSoundController.StartFootsteps();
                moving = true;
            }
            else if (_movement.magnitude < 0.1 && moving)
            {
                //playerSoundController.StopFootsteps();
                moving = false;
            }
        }

        private void OnDeath(object o, EventArgs e)
        {
            healthController.Heal(healthController.GetMaxHealth());
        }
        private void LateUpdate()
        {


        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                rb.velocity = new Vector2(_movement.x * moveSpeed, _movement.y * moveSpeed);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

        }

        private void SetAnimation(Vector2 velocity)
        {
            if (velocity.magnitude > 0)
            {
                if (velocity.x > 0)
                {
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else if (velocity.x < 0 )
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    
                }
                animator.Play("PlayerRun");
                if (spellbookController.currentBook)
                {
                    spellbookController.bookAnimator.Play("Book");
                }
            }
            else
            {
                animator.Play("PlayerIdle");
                if (spellbookController.currentBook)
                {
                    spellbookController.bookAnimator.Play("BookIdle");
                }
            }
        }


        private void OnDestroy()
        {
            if (initialised)
            {
                healthController.OnDamageTaken -= healthBarController.UpdateHealth;
                healthController.OnHealed -= healthBarController.UpdateHealth;
                healthController.OnMaxHealthChanged -= healthBarController.UpdateMaxHealth;
                spellbookController.OnBookChanged -= attackController.ChangeBook;
                spellbookController.OnBookChanged -= spellCastingController.ChangeSpell;
                attackController.OnAttackSpell -= spellbookController.AnimateSpell;
                spellCastingController.OnSpellCast -= drawableSpellController.StartDrawing;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") && !isInvulnerable)
            {
                Debug.Log($"Collided with enemy: {other.name}");
                healthController.TakeDamage(10);
            }
        }

        private void ShowDamage(object o, HealthChangedEventArgs e)
        {
            if (healthController != null)
            {
                StartCoroutine(FlashPlayerRed());
                StartCoroutine(Invulnerability());
            }

        }

        private void OnAttackSpell( object o, AttackSpellEventArgs e )
        {
            if (e.type == CastedSpell.Type.Laser)
            {
                StartCoroutine(DelayedInvulnForTime(e.value0, e.value1));
            }
        }



        private IEnumerator Invulnerability()
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(invulnerabilityDuration);
            isInvulnerable = false;
        }



        private IEnumerator DelayedInvulnForTime(float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(InvulnForTime(duration));
        }



        private IEnumerator InvulnForTime(float duration)
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(duration);
            isInvulnerable = false;
        }

        private IEnumerator FlashPlayerRed()
        {
            
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
