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
        private SpriteRenderer spriteRenderer;
        public HealthBarController healthBarController;
        public HealthController healthController;
        public PlayerSoundController playerSoundController;
        bool moving = false;
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
                    spriteRenderer.flipX = true;
                }
                else if (velocity.x < 0)
                {
                    animator.Play("PlayerRun");
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                animator.Play("PlayerIdle");
            }
        }


        private void OnDestroy()
        {
            healthController.OnDamageTaken -= healthBarController.UpdateHealth;
            healthController.OnHealed -= healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged -= healthBarController.UpdateMaxHealth;
        }
    }
}
