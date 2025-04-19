using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackController : MonoBehaviour
{
    public List<SpellData> spells = new List<SpellData>();
    private SpellData equippedSpell;
    public TextMeshProUGUI text;
    
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
        Vector2 direction = DirectionToMouse();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject projectile = Instantiate(equippedSpell.projectilePrefab, transform.position,
            Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * equippedSpell.speed;
    }

    public Vector2 DirectionToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector2 direction = (mouseWorldPos - transform.position).normalized;
        return direction;
    }

    public void DisplayText(String spellName)
    {
        text.text = "New spell added: " + spellName;
    }
}
