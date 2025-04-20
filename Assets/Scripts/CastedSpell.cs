using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    [Serializable]
    public class CastedSpell
    {
        [Serializable]
        public enum Type
        {
            WaterBolt,
            FireBolt,
            Undefined
        }

        public Type type;
        public float strength;
        public override bool Equals(object obj)
        {


            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            CastedSpell otherSpell = (CastedSpell)obj;

            if (otherSpell.type == type)
            {
                return true;
            }
            return false;
        }



    }
}