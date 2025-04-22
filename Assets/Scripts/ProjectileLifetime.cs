using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLifetime : MonoBehaviour
{
    public float LifeTime = 1f;
    private float _startTime = 0f;
    private bool isDestroying = false;

    private void Start()
    {
        _startTime = Time.time;
    }

    void Update()
    {
        if (Time.time > _startTime + LifeTime && !isDestroying)
        {
            if (GetComponent<AudioSource>() != null)
            {
                if (GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<Collider2D>().enabled = false;
                    isDestroying = true;
                }
                else
                {
                    Destroy(gameObject);

                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (isDestroying && !GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
    }




}
