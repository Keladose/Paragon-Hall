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

    protected override void AttackPlayer()
    {

    }

}
