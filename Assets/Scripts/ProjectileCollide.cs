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
    void Start()
    {
        animator = GetComponent<Animator>();
        barController = GetComponentInChildren<EnemyHealthBarController>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;

            var enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(spellData.damage); // Uses Phantom’s override
            }

            animator.Play("Hit");
            StartCoroutine(WaitBeforeDestroy());
        }

        if (other.CompareTag("Map Bounds"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            animator.Play("Hit");
            StartCoroutine(WaitBeforeDestroy());
        }
    }
    IEnumerator WaitBeforeDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
