using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{


    [CreateAssetMenu(fileName = "New Drawable Spell", menuName = "Spells/Drawable Spell Data")]
    public class DrawableSpellData : ScriptableObject
    {
        public CastedSpell.Type type;
        public GameObject objectPrefab;
        public float distanceBetween;
        public float timeBetween;
    }

}