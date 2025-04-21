using System.Collections;
using System.Collections.Generic;
using Spellect;
using UnityEngine;

public class FlyingSkull : BaseEnemy
{
    public float orbitSpeed = 2f;
    public float spiralInSpeed = 1f;
    private float currentAngle = 0f;
    private float currentRadius = 5f;
    private float minRadius = 0.1f;

    protected override void Start()
    {
        base.Start();
        currentRadius = Vector3.Distance(transform.position, player.position);
    }

    protected override void ChasePlayer()
    {
        currentAngle += orbitSpeed * Time.deltaTime;
        currentRadius = Mathf.Max(minRadius, currentRadius - spiralInSpeed * Time.deltaTime);


        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * currentRadius;
        Vector3 targetPosition = player.position + offset;

        UpdateSpriteDirection(targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

    }

    protected override void AttackPlayer()
    {

    }

}
