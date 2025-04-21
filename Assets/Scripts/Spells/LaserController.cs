using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserController : MonoBehaviour
{
    private enum State
    {
        Charging,
        Firing,
        Inactive            
    }
    private State _state = State.Inactive;
    private float _timeStartedState = 0f;
    public  float ChargingTime;
    public float FiringTime;
    public float FinalScale = 1f;
    public SpellData spellData;
    public Animator Animator;
    public Animator StartAnimator;
    public GameObject HitPrefab;
    public Collider2D col;


    public void Charge()
    {
        _state = State.Charging;
        _timeStartedState = Time.time;
        Animator.Play("Charging");
        StartAnimator.Play("Charging");
    }
    

    private void Fire()
    {
        _timeStartedState = Time.time;
        _state = State.Firing;
        Animator.Play("Firing");
        StartAnimator.Play("Firing");
        col.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (_state == State.Charging)
        {
            if (Time.time > _timeStartedState + ChargingTime)
            {
                //transform.localScale = new Vector3((Time.time - _timeStartedState) / _chargingTime * FinalScale, transform.localScale.y,
                //transform.localScale.z);
                Fire();
            }
        }
        else if (_state == State.Firing)
        {
            if (Time.time > _timeStartedState + FiringTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_state == State.Firing)
        {
            if (other.CompareTag("Enemy"))
            {
                Vector2 closestPointOnLaser = col.ClosestPoint((Vector2)other.transform.position);
                Vector2 knockbackDirection = (-closestPointOnLaser + (Vector2)other.transform.position).normalized;



                other.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * spellData.knockBack);
                var enemy = other.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(spellData.damage); // Uses Phantom�s override
                }

                Instantiate(HitPrefab, closestPointOnLaser, Quaternion.identity);
            }

            if (other.CompareTag("Map Bounds"))
            {
                Vector2 closestPointOnLaser = col.ClosestPoint((Vector2)other.transform.position);
                Instantiate(HitPrefab, closestPointOnLaser, Quaternion.identity);
            }
        }
        
    }
}
