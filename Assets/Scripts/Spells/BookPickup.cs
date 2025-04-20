using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class BookPickup : MonoBehaviour
    {

        private SpellbookController spellbookController;
        private bool playerInRange;
        public Spellbook.Type type;
        public AttackSpellbook attackSpellbook;
        public PassiveSpellbook passiveSpellbook;
        private void Update()
        {
            if (playerInRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (type == Spellbook.Type.Attack)
                    {
                        spellbookController.AddBook(attackSpellbook);
                    }
                    else if (type == Spellbook.Type.Passive)
                    {
                        spellbookController.AddBook(passiveSpellbook);
                    }
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                playerInRange = true;
                spellbookController = other.GetComponent<SpellbookController>();
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
}

