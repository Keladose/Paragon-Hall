using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.PlayerSettings;

namespace Spellect
{
    [Serializable]
    public class SpellImage
    {
        public List<SpellImagePoint> Points = new();
        public List<SpellImageConnection> Connections = new();
        public struct ImageInfo
        {
            public List<Vector2> Points;
            public List<int[]> Connections;

            public ImageInfo(List<Vector2> points, List<int[]> connections)
            {
                Points = points;
                Connections = connections;
            }
        }

        [SerializeField] private GameObject _spellPointPrefab;
        [SerializeField] private GameObject _spellConnectionPrefab;
        [SerializeField] private Material _startMaterial;
        [SerializeField] private Material _doneMaterial;
        public CastedSpell Spell;

        public SpellcastingController _spellcastingController;
        [SerializeField] private const float DRAWING_Z = 0.5f;

        [SerializeField] private const float DONE_DIST = 0.6f;


        public SpellImage(ImageInfo imageInfo, GameObject spellPointPrefab, GameObject spellConnectionPrefab, Material startMaterial, Material doneMaterial, SpellcastingController spellcastingController, CastedSpell spell)
        {
            foreach( Vector2 point in imageInfo.Points)
            {
                Points.Add(new SpellImagePoint(point));
            }
            foreach (int[] conPoints in imageInfo.Connections)
            {
                Connections.Add(new SpellImageConnection(Points[conPoints[0]], Points[conPoints[1]]));
                Points[conPoints[0]].connections.Add(Connections[^1]);
                Points[conPoints[1]].connections.Add(Connections[^1]);
            }


            _spellPointPrefab = spellPointPrefab;
            _spellConnectionPrefab = spellConnectionPrefab;
            _spellcastingController = spellcastingController;
            _startMaterial = startMaterial;
            _doneMaterial = doneMaterial;
            Spell = spell;
        }

        public void Init(SpellcastingController spellcastingController)
        {
            
            _spellcastingController = spellcastingController;
        }
        public float GetScore(Vector2 pos)
        {
            int nearestPoint = GetClosestPoint(pos);
            int nearestConnection = GetClosestConnection(pos);
            float closestDist = Vector2.Distance(pos, Points[nearestPoint].Position);
            if (nearestConnection != -1)
            {
                Debug.Log("Connection score: " + Connections[nearestConnection].GetDistanceToPoint(pos) + ", point score is: " + closestDist);
                closestDist = Mathf.Min(closestDist, Connections[nearestConnection].GetDistanceToPoint(pos));

            }
            else
            {
                Debug.Log("Not between connections, score is" + closestDist);
            }

            return closestDist;
        }
        public int GetClosestPoint(Vector2 pos)
        {
            float minDist = 10000;
            int closestPoint = -1;
            for (int i = 0; i < Points.Count; i++)
            {
                float dist = Vector2.Distance(pos, Points[i].Position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestPoint = i;
                }

                if (dist < DONE_DIST && !Points[i].Done)
                {
                    Points[i].SetDone(_doneMaterial);
                }
            }
            return closestPoint;
        }

        public int GetClosestConnection(Vector2 pos)
        {
            int closestConnection = -1;
            float minDist = 10000;

            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].IsBetweenPoints(pos))
                {
                    float dist = Connections[i].GetDistanceToPoint(pos);
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
            foreach (SpellImagePoint pos in Points)
            {
                pos.gameObject = _spellcastingController.DrawPoint(new Vector3(pos.Position.x, pos.Position.y, DRAWING_Z), _spellPointPrefab);
            }
            foreach (SpellImageConnection con in Connections)
            {
                con.gameObject = _spellcastingController.DrawConnection(con.GetPoint0(), con.GetPoint1(), _spellConnectionPrefab);
            }
        }

        public void Hide()
        {
            foreach (SpellImagePoint pos in Points)
            {
                pos.Reset(_startMaterial);
            }
            foreach (SpellImageConnection con in Connections)
            {
                con.Reset(_startMaterial);
            }
        }

        public void Show()
        {
            foreach (SpellImagePoint pos in Points)
            {
                pos.gameObject.SetActive(true);
            }
            foreach (SpellImageConnection con in Connections)
            {
                con.gameObject.SetActive(true);
            }
        }

        public bool IsCompleted()
        {
            foreach (SpellImagePoint point in Points)
            {
                if (!point.Done)
                {
                    return false;
                }
            }
            return true;
        }

    }
    [Serializable]
    public class SpellImagePoint 
    {
        public List<SpellImageConnection> connections = new();
        public Vector2 Position;
        public GameObject gameObject;
        public bool Done = false;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            SpellImagePoint otherPoint = (SpellImagePoint)obj;
            if (otherPoint.Position.Equals(Position))
            {
                return true;
            }
            return false;
        }
        public SpellImagePoint(Vector2 position)
        {
            Position = position;
            Done = false;
        }


        public void SetDone(Material doneMaterial)
        {
            gameObject.GetComponent<SpriteRenderer>().material = doneMaterial;
            Done = true;
            foreach (SpellImageConnection con in connections)
            {
                if (con.GetOtherPoint(this).Done)
                {
                    con.SetDone(doneMaterial);
                }
            }
        }
        public void Reset(Material material)
        {
            gameObject.GetComponent<SpriteRenderer>().material = material;
            gameObject.SetActive(false);
            Done = false;
        }
    }
    [Serializable]
    public class SpellImageConnection
    {
        public SpellImagePoint _point0;
        public SpellImagePoint _point1;
        public GameObject gameObject;

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

        public void SetDone(Material doneMaterial)
        {
            gameObject.GetComponent<SpriteRenderer>().material = doneMaterial;
        }
        public void Reset(Material material)
        {
            gameObject.GetComponent<SpriteRenderer>().material = material;
            gameObject.SetActive(false);
        }
    }

    public static class ImageCreator
    {

        public static SpellImage.ImageInfo CreateDash()
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            // head
            int headPoints = 16;
            for (int i = 0; i < headPoints; i++)
            {
                points.Add(new Vector2(Mathf.Sin(i * 2 * Mathf.PI / headPoints), -Mathf.Cos(i * 2 * Mathf.PI / headPoints)));
                if (i > 0)
                {
                    cons.Add(new int[] { i - 1, i });
                }
            }
            cons.Add(new int[] {0, headPoints-1});
            // torso
            int torsoPoints = 3;
            SpellImage.ImageInfo tempInfo = GetLinePoints(new Vector2(0, -1.25f), new Vector2(0, -4), torsoPoints, headPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // right arm
            int armPoints = 3;
            tempInfo = GetLinePoints(new Vector2(0.25f, -1.25f), new Vector2(1,-2), armPoints, headPoints + torsoPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // right forearm
            tempInfo = GetLinePoints(new Vector2(1.25f, -1.75f), new Vector2(2,-1 ), armPoints, headPoints + torsoPoints + armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // left arm
            tempInfo = GetLinePoints(new Vector2(-0.25f, -1.25f), new Vector2(-1, -2), armPoints, headPoints + torsoPoints + 2*armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // left forearm
            tempInfo = GetLinePoints(new Vector2(-1, -2.25f), new Vector2(-1, -3.16f), armPoints, headPoints + torsoPoints + 3*armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // right thigh
            tempInfo = GetLinePoints(new Vector2(0.25f,-4.25f), new Vector2(1,-5f), armPoints, headPoints + torsoPoints + 4*armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // right leg
            tempInfo = GetLinePoints(new Vector2(1f, -5.25f), new Vector2(1, -6f), armPoints, headPoints + torsoPoints + 5*armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // left thigh
            tempInfo = GetLinePoints(new Vector2(0, -4.25f), new Vector2(0, -5f), armPoints, headPoints + torsoPoints + 6*armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // left leg
            tempInfo = GetLinePoints(new Vector2(-0.25f, -5.25f), new Vector2(-1, -6f), armPoints, headPoints + torsoPoints + 7 * armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections); ;

            cons.Add(new int[] { 0, headPoints }); // head to torso
            cons.Add(new int[] { 0, headPoints+torsoPoints }); // head to right arm
            cons.Add(new int[] { headPoints + torsoPoints + armPoints-1, headPoints + torsoPoints +armPoints}); // right arm to forearm
            cons.Add(new int[] { 0, headPoints + torsoPoints + 2*armPoints }); // head to left arm
            cons.Add(new int[] { headPoints+torsoPoints + 3 * armPoints-1, headPoints + torsoPoints + 3 * armPoints }); // left arm to left forearm
            cons.Add(new int[] { headPoints + torsoPoints-1, headPoints + torsoPoints + 4 * armPoints }); // bototm to right thigh
            cons.Add(new int[] { headPoints + torsoPoints + 5 * armPoints-1, headPoints + torsoPoints + 5 * armPoints }); // right thigh to right leg
            cons.Add(new int[] { headPoints + torsoPoints - 1, headPoints + torsoPoints + 6 * armPoints }); // bototm to left thigh
            cons.Add(new int[] { headPoints + torsoPoints + 7 * armPoints-1, headPoints + torsoPoints + 7 * armPoints }); // left thigh to left

            for (int i = 0; i < points.Count; i++)
            {
                points[i]+= new Vector2(-1, 3f);
            }// 
            return new SpellImage.ImageInfo(points, cons);
        }

        private static SpellImage.ImageInfo GetLinePoints(Vector2 startPoint, Vector2 endPoint, int numPoints, int index)
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            for (int i = 0; i < numPoints; i++)
            {
                points.Add(startPoint + i / (numPoints - 1f) * (endPoint - startPoint));
                if (i > 0)
                {
                    cons.Add(new int[] { index + i - 1, index + i });
                }
            }
            return new SpellImage.ImageInfo(points, cons);
        }
    }



}