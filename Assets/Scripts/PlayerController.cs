using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 _movement;
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement = _movement.normalized;
        SetAnimation(_movement);
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
}
