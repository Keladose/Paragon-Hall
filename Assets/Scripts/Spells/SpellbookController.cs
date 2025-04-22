using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Spellect.AttackController;

namespace Spellect
{

    public class SpellbookController : MonoBehaviour
    {
        public class BookChangedEventArgs : EventArgs { public Spellbook book; }
        public delegate void OnBookChangedEvent(object source, BookChangedEventArgs e);
        public event OnBookChangedEvent OnBookChanged;
        public List<AttackSpellbook> AttackSpellbooks = new();
        public List<PassiveSpellbook> PassiveSpellbooks = new();
        public Spellbook currentSpellbook;
        public GameObject currentBook;
        public Animator bookAnimator;
        public TextMeshProUGUI text;
        
        private WheelSelectController wheelSelectController;

        public void Start()
        {
            wheelSelectController = FindObjectOfType<WheelSelectController>();
        }

        public void AddBook(Spellbook spellBook)
        {
            if (spellBook.type == Spellbook.Type.Attack)
            {
                AttackSpellbooks.Add((AttackSpellbook)spellBook);
            }
            else if (spellBook.type == Spellbook.Type.Passive)
            {
                PassiveSpellbooks.Add((PassiveSpellbook)spellBook);
            }
            ChangeBook(spellBook);
            if (wheelSelectController != null)
            {
                wheelSelectController.UpdateWheelSlots();

            }

        }

        public void ChangeBook(Spellbook spellbook)
        {
            currentSpellbook = spellbook;
            OnBookChanged?.Invoke(this, new BookChangedEventArgs { book = spellbook });
            EquipSpellbook();
        }


        private void EquipSpellbook()
        {
            if (currentBook != null)
            {
                Destroy(currentBook);
            }

            currentBook = Instantiate(currentSpellbook.spellBookPrefab, transform);
            bookAnimator = currentBook.GetComponent<Animator>();
        }

        public void AnimateSpell(object o, AttackSpellEventArgs e)
        {
            if (currentBook != null)
            {
                Transform normal = currentBook.transform.Find("Normal");
                Transform aura = currentBook.transform.Find("Aura");

                if (normal != null) normal.gameObject.SetActive(false);
                if (aura != null) aura.gameObject.SetActive(true);

                StartCoroutine(ResetBookEffect(0.1f, normal, aura));
            }
        }
        private void Update()
        {
            for (int i = 0; i < AttackSpellbooks.Count && i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    ChangeBook(AttackSpellbooks[i]);
                }
            }
        }


        IEnumerator ResetBookEffect(float delay, Transform normal, Transform aura)
        {
            yield return new WaitForSeconds(delay);

            if (normal != null) normal.gameObject.SetActive(true);
            if (aura != null) aura.gameObject.SetActive(false);

        }

        public void DisplayText(string spellName)
        {
            text.text = "New spell added: " + spellName;
        }

        
    }

}