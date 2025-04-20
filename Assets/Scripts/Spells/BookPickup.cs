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
        public Spellbook spellbook;
        private void Update()
        {
            if (playerInRange)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    spellbookController.AddBook(spellbook);
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

