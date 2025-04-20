using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileController : MonoBehaviour
{

    private List<Vector2> _points;
    private int _nextPoint = 0;
    private Rigidbody2D _rb;
    public float ReachedDistance = 0.8f;
    private bool _finishedPath = false;
    public float SpeedupTime = 0.3f;
    private float _startTime = 0f;
    public float FinalForce = 1f;
    private bool _initialised = false;
    // Start is called before the first frame update

    public void Init(List<Vector2> points, Vector2 velocity)
    {
        _points = points;
        _rb = GetComponent<Rigidbody2D>();
        _startTime = Time.time;
        _rb.velocity = velocity;
        FinalForce = FinalForce * Random.Range(0.8f, 1.2f);
        _initialised = true;
    }

    private void FixedUpdate()
    {
        if (_finishedPath || !_initialised)
        {
            return;
        }
        float homingForce;
        if (Time.fixedTime < _startTime + SpeedupTime)
        {
            homingForce = FinalForce * (Time.fixedTime - _startTime) / SpeedupTime;
        }
        else
        {
            homingForce = FinalForce;
        }
        Vector2 direction = (-(Vector2)transform.position + _points[_nextPoint]).normalized;
        _rb.AddForce(direction * homingForce);
        CheckReachedNextPoint();
    }

    private void CheckReachedNextPoint()
    {
        if (Vector2.Distance(transform.position, _points[_nextPoint]) < ReachedDistance)
        {
            Debug.Log("Reached point " + _nextPoint);
            _nextPoint++;
        }
        if (_nextPoint == _points.Count)
        {
            _finishedPath = true;
            _rb.drag = 0f;

        }
    }

}
