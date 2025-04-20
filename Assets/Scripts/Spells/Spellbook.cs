using Spellect;
using System;
using System.Collections;
using System.Collections.Generic;
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