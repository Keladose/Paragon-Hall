using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{
    [Serializable]
    public class SpellImage
    {
        public List<SpellImagePoint> points = new();
        public List<SpellImageConnection> connections = new();


        [SerializeField] private GameObject _spellPointPrefab;
        [SerializeField] private GameObject _spellConnectionPrefab;

        private List<GameObject> _pointObjects = new();
        private List<GameObject> _connectionObjects = new();
        public SpellcastingController _spellcastingController;
        [SerializeField] private const float DRAWING_Z = 0.5f;


        public void Init(SpellcastingController spellcastingController)
        {
            _spellcastingController = spellcastingController;
        }
        public float GetScore(Vector2 pos)
        {
            int nearestPoint = GetClosestPoint(pos);
            int nearestConnection = GetClosestConnection(pos);
            float closestDist = Vector2.Distance(pos, points[nearestPoint].Position);
            if (nearestConnection != -1)
            {
                closestDist = Mathf.Min(closestDist, connections[nearestConnection].GetDistanceToPoint(pos));
            }
            return closestDist;
        }
        public int GetClosestPoint(Vector2 pos)
        {
            float minDist = 10000;
            int closestPoint = -1;
            for (int i = 0; i < points.Count; i++)
            {
                float dist = Vector2.Distance(pos, points[i].Position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestPoint = i;
                }
            }
            return closestPoint;
        }

        public int GetClosestConnection(Vector2 pos)
        {
            int closestConnection = -1;
            float minDist = 10000;

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].IsBetweenPoints(pos))
                {
                    float dist = connections[i].GetDistanceToPoint(pos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestConnection = i;
                    }

                }
            }
            return closestConnection;
        }
        public void Draw()
        {
            foreach (SpellImagePoint pos in points)
            {
                _pointObjects.Add(_spellcastingController.DrawPoint(new Vector3(pos.Position.x, pos.Position.y, DRAWING_Z), _spellPointPrefab));

            }
            foreach (SpellImageConnection con in connections)
            {
                _connectionObjects.Add(_spellcastingController.DrawConnection(con.GetPoint0(), con.GetPoint1(), _spellConnectionPrefab));

            }
        }


    }
    [Serializable]
    public class SpellImagePoint 
    {
        private List<SpellImageConnection> connections;
        private CastedSpell _spell;
        public Vector2 Position;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            SpellImagePoint otherPoint = (SpellImagePoint)obj;
            if (otherPoint.Position.Equals(Position) &&
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
    [Serializable]
    public class SpellImageConnection
    {
        public SpellImagePoint _point0;
        public SpellImagePoint _point1;

        public SpellImageConnection(SpellImagePoint point0, SpellImagePoint point1)
        {
            _point0 = point0;
            _point1 = point1;
        }

        public Vector2 GetPoint0()
        {
            return _point0.Position;
        }

        public Vector2 GetPoint1()
        {
            return _point1.Position;
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

        public bool IsBetweenPoints(Vector2 pos)
        {
            if (Vector2.Dot(-_point0.Position+pos, -_point0.Position+pos) < 0)
            {
                return true;
            }
            return false;
        }

        public float GetDistanceToPoint(Vector2 point)
        {
            Vector2 lineDir = _point1.Position - _point0.Position;
            float numerator = Mathf.Abs((lineDir.y) * (_point0.Position.x - point.x) - (lineDir.x) * (_point0.Position.y - point.y));
            float denominator = lineDir.magnitude;
            return numerator / denominator;
        }

    }



}