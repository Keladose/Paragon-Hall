using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpeedup : MonoBehaviour
{
    public float SpeedupTime = 0.5f;
    public float FinalSpeed = 25f;
    private Rigidbody2D _rb;
    private bool _initialised;
    private float _startTime = 0f;
    private Vector2 _direction;
    // Start is called before the first frame update
    public void Init(float finalSpeed, float speedupTime, Vector2 direction)
    {
        FinalSpeed = finalSpeed;
        SpeedupTime = speedupTime;
        _initialised = true;
        _startTime = Time.time;
        _rb = GetComponent<Rigidbody2D>();
        _direction = direction;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_initialised)
        {
            return;
        }
        if (Time.time < _startTime + SpeedupTime)
        {
            _rb.velocity = _direction.normalized * Mathf.Pow((Time.time - _startTime+0.01f) / SpeedupTime,4f)*FinalSpeed;
        }
        else
        {
            _rb.velocity = _direction * FinalSpeed;
        }

    }
}
