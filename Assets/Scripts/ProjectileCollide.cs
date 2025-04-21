using System;
using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;

public class ProjectileCollide : MonoBehaviour
{
    private Animator animator;
    private EnemyHealthBarController barController;
    public SpellData spellData;
    private bool healthNotInit;
    private bool _hasHitAnimation = true;
    public bool isPlayerProjectile = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        barController = GetComponentInChildren<EnemyHealthBarController>();
    }
    public void Init(bool hasHisAnimation)
    {
        _hasHitAnimation = hasHisAnimation;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") && isPlayerProjectile) || (other.CompareTag("Player") && !isPlayerProjectile) )
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 knockbackDirection = (-transform.position + other.transform.position).normalized;


            
            if (!spellData.goesThroughEnemies)
            {
                rb.velocity = Vector2.zero;
            }
            other.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * spellData.knockBack);
                var enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(spellData.damage); // Uses Phantom�s override
            }
            

            if (_hasHitAnimation)
            {
                animator.Play("Hit");
            }
            if (!spellData.goesThroughEnemies)
            {
                StartCoroutine(WaitBeforeDestroy());
            }
        }

        if (other.CompareTag("Map Bounds"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero; 
            if (_hasHitAnimation)
            {

                animator.Play("Hit");
            }
            StartCoroutine(WaitBeforeDestroy());
        }
    }
    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
