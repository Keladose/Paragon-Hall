using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : BaseEnemy
{
    private bool isWaiting;
    private float waitTime = 1.5f;

    private bool isSpeedCycling = false;
    private bool isSprinting;
    public float sprintMultiplier = 2f;
    public float slowMultiplier = 0.25f;
    private float sprintDuration = 0.75f;
    public float slowDuration = 2f;

    public float aggroRadiusMultiplier;
    private bool isAggro = false;

    private Coroutine speedCycleCoroutine;

    // Animator + position tracking
    private Vector3 lastPosition;

    protected override void Start()
    {
        base.Start();
        targetPoint = 0;
        
        // Initialize Animator and lastPosition
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    protected override void Update()
    {
        if (isChasing)
        {
            if (!isAggro)
            {
                isAggro = true;
                chaseRange = chaseRange * aggroRadiusMultiplier;
            }
            
            if (speedCycleCoroutine == null)
            {
                speedCycleCoroutine = StartCoroutine(SpeedCycle());
            }
        }
        else
        {
            if (speedCycleCoroutine != null)
            {
                StopCoroutine(speedCycleCoroutine);
                speedCycleCoroutine = null;
            }
            moveSpeed = originalSpeed;
        }

        // Check if ghoul is walking
        bool isWalking = (transform.position != lastPosition);
        animator.SetBool("isWalking", isWalking);
        lastPosition = transform.position;

        base.Update();
    }

    IEnumerator SpeedCycle()
    {
        isSpeedCycling = true;

        // Sprint phase
        moveSpeed = originalSpeed * sprintMultiplier;
        yield return new WaitForSeconds(sprintDuration);

        // Slow phase
        moveSpeed = originalSpeed * slowMultiplier;
        yield return new WaitForSeconds(slowDuration);

        // Reset to normal
        moveSpeed = originalSpeed;
        isSpeedCycling = false;
    }

    protected override void Patrol()
    {
        if (patrolPoints != null)
        {
            if (patrolPoints.Length == 0 || isWaiting) return;
            UpdateSpriteDirection(patrolPoints[targetPoint].position);

            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, patrolPoints[targetPoint].position) < 0.1f)
            {
                StartCoroutine(WaitAtPoint());
            }
        } else
        {
            base.Patrol();
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        targetPoint = (targetPoint + 1) % patrolPoints.Length;
        isWaiting = false;
    }

    protected override void AttackPlayer()
    {
        // Add damage logic here
    }

    protected override void UpdateSpriteDirection(Vector3 Target)
    {
        if (spriteRenderer == null) return;

        Vector3 direction = Target - transform.position;
        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x > 0;
        }
    }

    protected override void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die"); // Or animator.SetBool("isDead", true);
        }

        // Disable collider & movement if needed
        if (enemyCollider != null) enemyCollider.enabled = false;
        moveSpeed = 0f;

        Destroy(gameObject, 0.35f); // Adjust time based on animation length
    }
}