using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInMenu : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    public float direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (direction == 0)
        {
            rb.velocity = Vector2.left * speed; // Shoot left
        }
        else
        {
            rb.velocity = Vector2.right * speed;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.tag == "Enemy" || other.tag == "Player")
        {
            if (animator != null)
            {
                animator.Play("Hit");
            }
            Destroy(gameObject, 0.5f); // Wait for anim if needed
        }
       
    }
    
}
