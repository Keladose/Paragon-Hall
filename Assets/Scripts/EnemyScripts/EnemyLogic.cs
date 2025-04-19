using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int targetPoint;

    public float moveSpeed = 2f;
    private float originalSpeed;

    private float waitTime = 2f;
    private bool isWaiting = false;

    public Transform player;
    public float chaseRange = 2f;
    public float attackRange = 2f;

    private bool isChasing = false;
    private bool isSlowed = false;

    private float attackCooldown = 2f;
    private float lastAttackTime;

    public float maxHealthInitial = 100;

    public HealthBarController healthBarController;
    public HealthController healthController;
    // Start is called before the first frame update
    void Start()
    {
        targetPoint = 0;
        originalSpeed = moveSpeed;
        healthController.Init(maxHealthInitial);
        healthBarController.Init(healthController.GetMaxHealth());
        healthController.OnDamageTaken += healthBarController.UpdateHealth;
        healthController.OnHealed += healthBarController.UpdateHealth;
        healthController.OnMaxHealthChanged += healthBarController.UpdateMaxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            chasePlayer();
        }
        else if (!isWaiting)
        {
            patrol();
        }

        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            // PLAYER TAKES DAMAGE
            lastAttackTime = Time.time;
            onDamagePlayer();
        }
        
    }

    void increaseTargetInt()
    {
        targetPoint++;
        if(targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        increaseTargetInt();
    }

    void chasePlayer()
    {
        if (isChasing)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
    
    void patrol()
    {
        if (patrolPoints.Length != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, moveSpeed * Time.deltaTime);
            if (transform.position == patrolPoints[targetPoint].position)
            {
                StartCoroutine(WaitAtPoint());
            }
        }

    }

    public void onDamagePlayer()
    {
        if (!isSlowed)
        {
            StartCoroutine(slowAfterHit());
        }
    }
    IEnumerator slowAfterHit()
    {
        isSlowed = true;
        moveSpeed = originalSpeed * 0.25f;
        yield return new WaitForSeconds(2f);
        moveSpeed = originalSpeed;
        isSlowed = false;
    }


    private void OnDestroy()
    {
        healthController.OnDamageTaken -= healthBarController.UpdateHealth;
        healthController.OnHealed -= healthBarController.UpdateHealth;
        healthController.OnMaxHealthChanged -= healthBarController.UpdateMaxHealth;
    }
}
