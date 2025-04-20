using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    public class SpellDrawing 
    {
        private List<Vector2> _points = new();
        private float score;
        public int FirstUnevaluatedPoint = 0;

        public bool EnoughDistanceFromLastPoint(Vector2 pos, float dist)
        {
            if (_points.Count == 0)
            {
                return true;
            }
            if (Vector2.Distance(pos,_points[^1]) >= dist)
            {
                return true;
            }
            return false;
        }
        public List<Vector2> GetPoints()
        {
            List<Vector2> points = new();
            foreach (Vector2 point in _points)
            {
                points.Add(point);
            }
            return points;
        }
        public void AddPoint(Vector2 pos)
        {
            _points.Add(pos);
        }
        public Vector2 GetPoint(int index)
        {
            return _points[index];
        }

        public int GetNumPoints()
        {
            return _points.Count;
        }

        public void Clear()
        {
            _points.Clear();

        }
    }

}
