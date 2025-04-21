using System;
using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;

public class EliteSkeletonMage : BaseEnemy
{
    private Vector3 lastPosition;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    protected override void Update()
    {
        bool isWalking = (transform.position - lastPosition).sqrMagnitude > 0.0001f;
        animator.SetBool("isWalking", isWalking);
        lastPosition = transform.position;

        base.Update();
    }

    protected override void Die(object o, EventArgs e)
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (enemyCollider != null) enemyCollider.enabled = false;
        moveSpeed = 0f;

        Destroy(gameObject, 0.35f);
    }

    protected override void AttackPlayer()
    {
        
    }
    private void SetAnimation(Vector2 velocity)
    {
        if (velocity.magnitude > 0)
        {
            if (velocity.x > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            animator.Play("EliteMageWalk");
            
        }
        else
        {
            animator.Play("EliteMageIdle");
        }
    }
}
