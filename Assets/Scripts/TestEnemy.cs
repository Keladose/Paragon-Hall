using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class TestEnemy : MonoBehaviour
    {
        public float moveSpeed = 5f;
        private Rigidbody2D rb;
        private Vector2 moveInput;
        public HealthBarController healthBarController;
        public HealthController healthController;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            healthController.Init(100);
            healthBarController.Init(healthController.GetMaxHealth());
            healthController.OnDamageTaken += healthBarController.UpdateHealth;
            healthController.OnHealed += healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged += healthBarController.UpdateMaxHealth;
        }
        // Update is called once per frame
        void Update()
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput = moveInput.normalized; // Prevent faster diagonal movement

            if (Input.GetKeyDown(KeyCode.K))
            {
                healthController.TakeDamage(10);
            }


            if (Input.GetKeyDown(KeyCode.H))
            {
                healthController.Heal(30);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                healthController.AddMaxHealth(10);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                healthController.AddMaxHealth(-10);
            }
        }

        private void OnDestroy()
        {
            healthController.OnDamageTaken -= healthBarController.UpdateHealth;
            healthController.OnHealed -= healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged -= healthBarController.UpdateMaxHealth;
        }

        void FixedUpdate()
        {
            // Apply movement
            rb.velocity = moveInput * moveSpeed;
        }
    }

}