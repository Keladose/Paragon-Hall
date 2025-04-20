using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell Data")]
    public class SpellData : ScriptableObject
    {
        public string spellName;
        public GameObject projectilePrefab;
        public GameObject specialPrefab;
        public float damage;
        public float range;
        public float speed;
        public float cooldown;
    }

}