using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private void Awake()
    {
        if (GameManager.Instance.playerObject != null)
        {
            Destroy(this.gameObject);
            Destroy(this);
        }
        else
        {
            GameManager.Instance.playerObject = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
            
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized; // Prevent faster diagonal movement
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.velocity = moveInput * moveSpeed;
    }
}
