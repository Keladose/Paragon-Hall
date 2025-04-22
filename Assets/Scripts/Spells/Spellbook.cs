using System;
using UnityEngine;

namespace Spellect
{
    [Serializable]
    public class Spellbook
    {
        public enum Type
        {
            Attack,
            Passive

        }
        public Type type;
        public GameObject spellBookPrefab;
        public CastedSpell castedSpell;
    }

}