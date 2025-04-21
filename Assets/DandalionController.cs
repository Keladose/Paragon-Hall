using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;

public class DandalionController : BaseEnemy
{

    public float stopDistance;
    public List<SpellData> spellData;

    private bool inRange;
    // Start is called before the first frame update
    void Start()
    {

    }


    protected override void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <= chaseRange;

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        if (inRange)
        {
            ShootProjectile();
        }
    }

    protected override void AttackPlayer()
    {

    }
    protected override void ChasePlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        
        if (distance > stopDistance)
        {
            UpdateSpriteDirection(player.position);
            AddForceTowardsTarget(player.position, moveSpeed);
            inRange = false;
        }
        else
        {
           
            rb.velocity = Vector2.zero;
            inRange = true;
        }
    }

    protected void ShootProjectile()
    {
        if (spellData.Count > 0)
        {
            if (Random.Range(0, 400) == 1)
            {
                GameObject projectile = Instantiate(spellData[0].projectilePrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                Vector2 direction = (player.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
                rb.velocity = direction * spellData[0].speed;

            }
        }
    }
}