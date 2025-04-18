using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    private AttackController attackController;
    private bool playerInRange;
    public SpellData spellData;
    void Start()
    {
        attackController = FindObjectOfType<AttackController>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                attackController.spells.Add(spellData);
                attackController.DisplayText(spellData.spellName);
                Destroy(gameObject);
            }
        }
    }   

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("entered");
        if (other.tag == "Player")
        {
            playerInRange = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
        
    }
}
