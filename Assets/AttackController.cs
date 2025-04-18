using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public List<SpellData> spells = new List<SpellData>();
    private SpellData equippedSpell;
    public Transform shootInitialPos;
    public GameObject test;
    
    void Update()
    {
        
        if (spells.Count != 0 && Input.GetMouseButtonDown(0))
        {
            equippedSpell = spells[0];
            FireProjectile();
        }
    }

    public void FireProjectile()
    {
        Vector2 direction = MouseInput();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject projectile = Instantiate(equippedSpell.projectilePrefab, shootInitialPos.position,
            Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * equippedSpell.speed;
    }

    public Vector2 MouseInput()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        
        test.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - transform.position).normalized;
        return direction;
    }
}
