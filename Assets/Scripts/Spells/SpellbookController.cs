using Spellect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{

    public class SpellbookController : MonoBehaviour
    {
        public class BookChangedEventArgs : EventArgs { public Spellbook book; }
        public delegate void OnBookChangedEvent(object source, BookChangedEventArgs e);
        public event OnBookChangedEvent OnBookChanged;
        public List<AttackSpellbook> AttackSpellbooks = new();
        public List<PassiveSpellbook> PassiveSpellbooks = new();

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
        }

        public void ChangeBook(Spellbook spellbook)
        {
            OnBookChanged?.Invoke(this, new BookChangedEventArgs { book = spellbook });
        }



    }

}