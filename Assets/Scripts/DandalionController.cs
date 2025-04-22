using System;
using System.Collections;
using System.Collections.Generic;
using Spellect;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DandalionController : BaseEnemy
{

    public float stopDistance;
    public List<SpellData> spellData;

    private bool inRange;
    // Start is called before the first frame update
    void Start()
    {
        if (healthController != null)
        {
            Debug.Log("Initialised health controller");
            healthController.Init(1000);
            healthBarController.Init(healthController.GetMaxHealth());
            healthController.OnDamageTaken += healthBarController.UpdateHealth;
            healthController.OnHealed += healthBarController.UpdateHealth;
            healthController.OnMaxHealthChanged += healthBarController.UpdateMaxHealth;
            healthController.OnDeath += Die;
        }
    }
    void Awake()
    {
        if (GameManager.Instance != null)
        {
            if (!GameManager.Instance.SpawnBoss)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
    }


    protected override void Update()
    {
        if (player == null) return; 
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
            if (Random.Range(0, 500) == 2)
            {
                ActivateSpecialAbility();
            }
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
            for (int i = 0; i < spellData.Count; i++)
            {
                if (Random.Range(0, 400) == 1)
                {
                    GameObject projectile = Instantiate(spellData[i].projectilePrefab, transform.position,
                        Quaternion.identity);
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    Vector2 direction = (player.position - transform.position).normalized;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
                    rb.velocity = direction * spellData[0].speed;
                }
            }
        }
    }

    protected void ActivateSpecialAbility()
    {
        int projectileCount = 12;
        
        if (spellData.Count > 0)
        {
            int randomIndex = Random.Range(0, spellData.Count);

            for (int i = 0; i < projectileCount; i++)
            {
                float angle = i * (360f / projectileCount);
                float rad = angle * Mathf.Deg2Rad; 
                Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
                Vector2 spawnPos = (Vector2)transform.position + direction;
                
                GameObject projectile = Instantiate(spellData[randomIndex].projectilePrefab, spawnPos, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

                projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
                rb.velocity = direction * spellData[randomIndex].speed;
            }
        }
    }

    protected override void Die(object o, EventArgs e)
    {
        base.Die(o, e);
        SceneManager.LoadScene("GameOverScreen");
    }
}