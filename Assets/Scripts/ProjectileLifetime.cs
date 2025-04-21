using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLifetime : MonoBehaviour
{
    public float LifeTime = 1f;
    private float _startTime = 0f;

    private void Start()
    {
        _startTime = Time.time;
    }

    void Update()
    {
        if (Time.time > _startTime + LifeTime)
        {
            Destroy(gameObject);
        }
    }


}
