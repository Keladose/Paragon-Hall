using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static Spellect.SpellbookController;

namespace Spellect
{
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
                //equippedSpell = spells[selectedSpellIndex];
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


        public void ChangeBook(object o, BookChangedEventArgs e)
        {
            if (e.book.type == Spellbook.Type.Attack)
            {
                AttackSpellbook book = (AttackSpellbook)e.book;
                equippedSpell = book.basicAttack;
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

            if (currentBook != null)
            {
                Transform normal = currentBook.transform.Find("Normal");
                Transform aura = currentBook.transform.Find("Aura");

                if (normal != null) normal.gameObject.SetActive(false);
                if (aura != null) aura.gameObject.SetActive(true);

                StartCoroutine(ResetBookEffect(0.1f, normal, aura));
            }

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

        private void EquipSpellbook()
        {
            if (currentBook != null)
            {
                Destroy(currentBook);
            }

            currentBook = Instantiate(equippedSpell.spellBookPrefab, transform);
            bookAnimator = currentBook.GetComponent<Animator>();
        }



        IEnumerator ResetBookEffect(float delay, Transform normal, Transform aura)
        {
            yield return new WaitForSeconds(delay);

            if (normal != null) normal.gameObject.SetActive(true);
            if (aura != null) aura.gameObject.SetActive(false);

        }
    }
}