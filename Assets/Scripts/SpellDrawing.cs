using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class SpellDrawing 
    {
        private List<Vector2> points = new();
        private float score;
        public int FirstUnevaluatedPoint = 0;
        [SerializeField] private const float MIN_DIST = 0.3f;

        public bool EnoughDistanceFromLastPoint(Vector2 pos)
        {
            if (points.Count == 0)
            {
                return true;
            }
            if (Vector2.Distance(pos,points[^1]) >= MIN_DIST)
            {
                return true;
            }
            return false;
        }

        public void AddPoint(Vector2 pos)
        {
            points.Add(pos);
        }
        public Vector2 GetPoint(int index)
        {
            return points[index];
        }

        public int GetNumPoints()
        {
            return points.Count;
        }

        public void Clear()
        {
            points.Clear();

        }
    }

}
