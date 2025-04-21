using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;

public class WaterSpirit : BaseEnemy
{
    [SerializeField] private GameObject smallSpiritPrefab;
    [SerializeField] private int numberToSpawn = 2;
    [SerializeField] private float spawnOffset = 0.5f;

    private float dashSpeed = 8f;
    private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 2f;

    private bool isDashing = false;
    private float lastDashTime;

    protected override void Update()
    {
        base.Update(); // Handle chase/patrol logic from BaseEnemy

        if (!isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            Vector2 dashDir;

            if (isChasing && player != null)
            {
                // Dash toward player
                dashDir = (player.position - transform.position).normalized;
            }
            else
            {
                // Dash randomly
                dashDir = Random.insideUnitCircle.normalized;
            }

            StartCoroutine(Dash(dashDir));
        }
    }

    private IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        lastDashTime = Time.time;

        float timer = 0f;
        while (timer < dashDuration)
        {
            transform.Translate(direction * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    protected override void Die()
    {
        if (!isDead)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                Vector2 offset = Random.insideUnitCircle * spawnOffset;
                var spirit = Instantiate(smallSpiritPrefab, (Vector2)transform.position + offset, Quaternion.identity);

                BaseEnemy enemyScript = spirit.GetComponent<BaseEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.player = this.player;
                    enemyScript.patrolPoints = null;
                }
            }
        }

        base.Die();
    }

    protected override void AttackPlayer()
    {
        // Optional custom attack behavior
    }

}

