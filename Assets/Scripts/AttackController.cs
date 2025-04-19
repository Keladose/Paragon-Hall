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
    public SpellData equippedSpell;
    public TextMeshProUGUI text;
    
    private int selectedSpellIndex = 0;
    public GameObject currentBook;
    public Animator bookAnimator;
    
    void Update()
    { 
        if (spells.Count != 0 && Input.GetMouseButtonDown(0))
        {
            equippedSpell = spells[selectedSpellIndex];
            FireProjectile();
        } 
        for (int i = 0; i < spells.Count && i < 9; i++) 
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSpellIndex = i;
                equippedSpell = spells[selectedSpellIndex];
                EquipSpellbook();
            }
        }
    }

    public void FireProjectile()
    {
        Vector2 direction = MouseInput();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject projectile = Instantiate(equippedSpell.projectilePrefab, transform.position,
            Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * equippedSpell.speed;
    }

    public Vector2 MouseInput()
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

    private void EquipSpellbook()
    {
        if (currentBook != null)
        {
            Destroy(currentBook);
        }
        
        currentBook = Instantiate(equippedSpell.spellBookPrefab, transform);
        bookAnimator = currentBook.GetComponent<Animator>();
    }
}
