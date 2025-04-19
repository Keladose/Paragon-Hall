using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell Data")]
public class SpellData : ScriptableObject
{
        public string spellName;
        public GameObject projectilePrefab;
        public float damage;
        public float range;
        public float speed;
}
