using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 _movement;
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");
    }

    private void LateUpdate()
    {

             
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_movement.x * moveSpeed, _movement.y * moveSpeed);
    }
}
