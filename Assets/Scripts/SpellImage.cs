using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Spellect
{
    public class SpellImage
    {
        private List<SpellImagePoint> points = new();

    }

    public class SpellImagePoint : MonoBehaviour
    {
        private List<SpellImageConnection> connections;
        private CastedSpell _spell;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            SpellImagePoint otherPoint = (SpellImagePoint)obj;
            if (otherPoint.transform.position.Equals(transform.position) &&
               otherPoint._spell.Equals(_spell))
            {
                return true;
            }
            return false;
        }
        public SpellImagePoint(CastedSpell spell)
        {
            _spell = spell;
        }
    }

    public class SpellImageConnection
    {
        private SpellImagePoint _point0;
        private SpellImagePoint _point1;

        public SpellImageConnection(SpellImagePoint point0, SpellImagePoint point1)
        {
            _point0 = point0;
            _point1 = point1;
        }

        public SpellImagePoint GetOtherPoint(SpellImagePoint pointIn)
        {
            if (pointIn.Equals(_point0))
            {
                return _point1;
            }
            else if (pointIn.Equals(_point1))
            {
                return _point0;
            }
            else
            {
                Debug.Log("point not in connection");
                return null;
            }
        }
    }
}