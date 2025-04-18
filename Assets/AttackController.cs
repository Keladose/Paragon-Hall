using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public List<SpellData> spells = new List<SpellData>();
    private SpellData equippedSpell;
    public Transform shootInitialPos;
    public GameObject test;

    private void Start()
    {
        Instantiate(test);
    }

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
        GameObject projectile = Instantiate(equippedSpell.projectilePrefab, shootInitialPos.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = MouseInput() * equippedSpell.speed;

    }

    public Vector2 MouseInput()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;
        mouseWorldPos.z = 0f;
        
        test.transform.position = Camera.main.ScreenToWorldPoint(mouseWorldPos);
        Vector2 direction = (mouseWorldPos - transform.position).normalized;
        return direction;
    }
}
