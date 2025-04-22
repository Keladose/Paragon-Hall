using Spellect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spellect
{


    public static class ImageCreator
    {
        public static SpellImage.ImageInfo DrawLetterE()
        {
            List<Vector2> allPoints = new();
            List<int[]> allConnections = new();

            int index = 0;
            int numPoints = 10;

            // Vertical line (from bottom to top)
            var verticalLine = GetLinePoints(new Vector2(-0.5f, 0f), new Vector2(-0.5f, 1f), numPoints, index);
            allPoints.AddRange(verticalLine.Points);
            allConnections.AddRange(verticalLine.Connections);
            index += verticalLine.Points.Count;

            // Top horizontal line (from left to right)
            var topLine = GetLinePoints(new Vector2(-0.5f, 1f), new Vector2(0.5f, 1f), numPoints, index);
            allPoints.AddRange(topLine.Points);
            allConnections.AddRange(topLine.Connections);
            index += topLine.Points.Count;

            // Middle horizontal line (from left to right)
            var middleLine = GetLinePoints(new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f), numPoints, index);
            allPoints.AddRange(middleLine.Points);
            allConnections.AddRange(middleLine.Connections);
            index += middleLine.Points.Count;

            // Bottom horizontal line (from left to right)
            var bottomLine = GetLinePoints(new Vector2(-0.5f, 0f), new Vector2(0.5f, 0f), numPoints, index);
            allPoints.AddRange(bottomLine.Points);
            allConnections.AddRange(bottomLine.Connections);
            index += bottomLine.Points.Count;

            for (int i = 0; i < allPoints.Count; i++)
            {
                allPoints[i] *= 6;
            }// 

            return new SpellImage.ImageInfo(allPoints, allConnections);
        }

        public static SpellImage.ImageInfo DrawLetterA()
        {
            List<Vector2> allPoints = new();
            List<int[]> allConnections = new();

            int index = 0;
            int numPointsPerLine = 10;

            // Left leg: from bottom left (-0.5, 0) to top center (0, 1)
            var leftLeg = GetLinePoints(new Vector2(-0.5f, 0f), new Vector2(0f, 1f), numPointsPerLine, index);
            allPoints.AddRange(leftLeg.Points);
            allConnections.AddRange(leftLeg.Connections);
            index += leftLeg.Points.Count;

            // Right leg: from bottom right (0.5, 0) to top center (0, 1)
            var rightLeg = GetLinePoints(new Vector2(0.5f, 0f), new Vector2(0f, 1f), numPointsPerLine, index);
            allPoints.AddRange(rightLeg.Points);
            allConnections.AddRange(rightLeg.Connections);
            index += rightLeg.Points.Count;

            // Crossbar: from left to right across the middle (you can adjust the height as needed)
            var crossbar = GetLinePoints(new Vector2(-0.25f, 0.5f), new Vector2(0.25f, 0.5f), numPointsPerLine, index);
            allPoints.AddRange(crossbar.Points);
            allConnections.AddRange(crossbar.Connections);
            index += crossbar.Points.Count;

            for (int i = 0; i < allPoints.Count; i++)
            {
                allPoints[i] *= 6;
            }// 
            return new SpellImage.ImageInfo(allPoints, allConnections);
        }
        public static SpellImage.ImageInfo DrawLetterS()
        {
            List<Vector2> allPoints = new();
            List<int[]> allConnections = new();

            int index = 0;
            int numPointsPerArc = 30;
            float radius = 3f; // 8x larger scale

            // Top arc: center at (0, 4), from 135° to -45°, clockwise
            var topArc = GetArcPoints(new Vector2(0f, 3f), radius, 0f, 270f, true, numPointsPerArc, index);
            allPoints.AddRange(topArc.Points);
            allConnections.AddRange(topArc.Connections);
            index += topArc.Points.Count;

            // Bottom arc: center at (0, -4), from 135° to -45°, counterclockwise
            var bottomArc = GetArcPoints(new Vector2(0f, -3f), radius, -180f, 90f, true, numPointsPerArc, index);
            allPoints.AddRange(bottomArc.Points);
            allConnections.AddRange(bottomArc.Connections);
            index += bottomArc.Points.Count;

            return new SpellImage.ImageInfo(allPoints, allConnections);
        }
        public static SpellImage.ImageInfo DrawLetterC()
        {
            List<Vector2> allPoints = new();
            List<int[]> allConnections = new();

            int index = 0;
            int numPoints = 60;

            // Bigger and rotated C
            Vector2 center = new Vector2(0f, 0f);     // Centered at origin
            float radius = 4f;                        // 8x larger than original (was 0.5)
            float startAngle = 135f;                  // Top-left
            float endAngle = -135f;                   // Bottom-left
            bool isClockwise = true;                  // So the open part is on the right

            var arc = GetArcPoints(center, radius, startAngle, endAngle, isClockwise, numPoints, index);
            allPoints.AddRange(arc.Points);
            allConnections.AddRange(arc.Connections);

            return new SpellImage.ImageInfo(allPoints, allConnections);
        }



        public static SpellImage.ImageInfo CreateLaser()
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            // head
            int linePoints = 7;
            SpellImage.ImageInfo tempInfo = GetLinePoints(new Vector2(-4, 0), new Vector2(-2, -0), linePoints, 0);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(-2, 0f), new Vector2(-1f, 1f), linePoints, linePoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(-1f, 1f), new Vector2(1, -1f), linePoints, linePoints * 2);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(1, -1f), new Vector2(2f, 0), linePoints, linePoints * 3);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(2, 0f), new Vector2(4f, 0), linePoints, linePoints * 4);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);

            cons.Add(new int[] { linePoints - 1, linePoints });
            cons.Add(new int[] { 2 * linePoints - 1, 2 * linePoints });
            cons.Add(new int[] { 3 * linePoints - 1, 3 * linePoints });
            cons.Add(new int[] { 4 * linePoints - 1, 4 * linePoints });

            for (int i = 0; i < points.Count; i++)
            {
                points[i] *= 2;// new Vector2(-1, 3f);
            }// 
            return new SpellImage.ImageInfo(points, cons);
        }


        public static SpellImage.ImageInfo CreateNado()
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            // head
            int linePoints = 7;
            SpellImage.ImageInfo tempInfo = GetLinePoints(new Vector2(0, -3), new Vector2(0.5f, -2.75f), linePoints, 0);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(0.5f, -2.75f), new Vector2(-1f, -1.5f), linePoints, linePoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(-1f, -1.5f), new Vector2(2, 0f), linePoints, linePoints*2);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(2, 0f), new Vector2(-3f, 2f), linePoints, linePoints*3);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(-3f, 2f), new Vector2(5, 5f), linePoints, linePoints * 3);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);

            cons.Add(new int[] { linePoints - 1, linePoints });
            cons.Add(new int[] { 2 * linePoints - 1, 2 * linePoints });
            cons.Add(new int[] { 3 * linePoints - 1, 3 * linePoints });
            cons.Add(new int[] { 4 * linePoints - 1, 4 * linePoints });

            for (int i = 0; i < points.Count; i++)
            {
                // points[i] += new Vector2(-1, 3f);
            }// 
            return new SpellImage.ImageInfo(points, cons);
        }






        public static SpellImage.ImageInfo CreateIce()
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            // head
            int linePoints = 7;
            SpellImage.ImageInfo tempInfo = GetLinePoints(new Vector2(-2, -0), new Vector2(2, 0f), linePoints, 0);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints(new Vector2(0, 2f), new Vector2(0, -2f), linePoints, linePoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints(new Vector2(-1.5f, -1.5f), new Vector2(1.5f, 1.5f), linePoints, linePoints*2);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints(new Vector2(-1.5f, 1.5f), new Vector2(1.5f, -1.5f), linePoints, linePoints*3);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);

            for (int i = 0; i < points.Count; i++)
            {
                points[i] *= 2; // new Vector2(-1, 3f);
            }// 
            return new SpellImage.ImageInfo(points, cons);
        }

        public static SpellImage.ImageInfo CreateFire()
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            // head
            int linePoints = 5;
            SpellImage.ImageInfo tempInfo =GetLinePoints(new Vector2(-1, -1.5f), new Vector2(-0.5f, 1.5f), linePoints, 0);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(-0.5f, 1.5f), new Vector2(0, 0.75f), linePoints, linePoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(0, 0.75f), new Vector2(0.5f, 1.5f), linePoints, linePoints*2);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(new Vector2(0.5f, 1.5f), new Vector2(1, -1.5f), linePoints, linePoints*3);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            cons.Add(new int[] { linePoints-1, linePoints }); 
            cons.Add(new int[] { 2*linePoints - 1, 2*linePoints });
            cons.Add(new int[] { 3*linePoints - 1, 3*linePoints });


            for (int i = 0; i < points.Count; i++)
            {
                points[i] *= 2;
            }// 
            return new SpellImage.ImageInfo(points, cons);
        }

        public static SpellImage.ImageInfo CreateMeteor()
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            // head
            int ballPoints = 8;
            SpellImage.ImageInfo tempInfo = GetArcPoints(Vector2.zero, 3, -100, 120, true, ballPoints, 0);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            int linePoints = 4;
            tempInfo = GetLinePoints2(points[0], new Vector2(-8.638155725f, -1.523139918f), linePoints, ballPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            tempInfo = GetLinePoints2(points[ballPoints - 1], new Vector2(-8.638155725f, -1.523139919f), linePoints, ballPoints + linePoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            cons.Add(new int[] {0 , ballPoints }); // top
            cons.Add(new int[] { ballPoints - 1, ballPoints + linePoints }); // bottom;


            for (int i = 0; i < points.Count; i++)
            {
                points[i] *= 1.3f;// new Vector2(-1, 3f);
            }// 
            return new SpellImage.ImageInfo(points, cons);
        }

        public static SpellImage.ImageInfo CreateDash()
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            // head
            int headPoints = 32;
            for (int i = 0; i < headPoints; i++)
            {
                points.Add(new Vector2(Mathf.Sin(i * 2 * Mathf.PI / headPoints), -Mathf.Cos(i * 2 * Mathf.PI / headPoints)));
                if (i > 0)
                {
                    cons.Add(new int[] { i - 1, i });
                }
            }
            cons.Add(new int[] { 0, headPoints - 1 });
            // torso
            int torsoPoints = 8;
            SpellImage.ImageInfo tempInfo = GetLinePoints(new Vector2(0, -1.25f), new Vector2(0, -4), torsoPoints, headPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // right arm
            int armPoints = 5;
            tempInfo = GetLinePoints(new Vector2(0.25f, -1.25f), new Vector2(1, -2), armPoints, headPoints + torsoPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // right forearm
            tempInfo = GetLinePoints(new Vector2(1.25f, -1.75f), new Vector2(2, -1), armPoints, headPoints + torsoPoints + armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // left arm
            tempInfo = GetLinePoints(new Vector2(-0.25f, -1.25f), new Vector2(-1, -2), armPoints, headPoints + torsoPoints + 2 * armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // left forearm
            tempInfo = GetLinePoints(new Vector2(-1, -2.25f), new Vector2(-1, -3.16f), armPoints, headPoints + torsoPoints + 3 * armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // right thigh
            tempInfo = GetLinePoints(new Vector2(0.25f, -4.25f), new Vector2(1, -5f), armPoints, headPoints + torsoPoints + 4 * armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // right leg
            tempInfo = GetLinePoints(new Vector2(1f, -5.25f), new Vector2(1, -6f), armPoints, headPoints + torsoPoints + 5 * armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // left thigh
            tempInfo = GetLinePoints(new Vector2(0, -4.25f), new Vector2(0, -5f), armPoints, headPoints + torsoPoints + 6 * armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections);
            // left leg
            tempInfo = GetLinePoints(new Vector2(-0.25f, -5.25f), new Vector2(-1, -6f), armPoints, headPoints + torsoPoints + 7 * armPoints);
            points.AddRange(tempInfo.Points);
            cons.AddRange(tempInfo.Connections); ;

            cons.Add(new int[] { 0, headPoints }); // head to torso
            cons.Add(new int[] { 0, headPoints + torsoPoints }); // head to right arm
            cons.Add(new int[] { headPoints + torsoPoints + armPoints - 1, headPoints + torsoPoints + armPoints }); // right arm to forearm
            cons.Add(new int[] { 0, headPoints + torsoPoints + 2 * armPoints }); // head to left arm
            cons.Add(new int[] { headPoints + torsoPoints + 3 * armPoints - 1, headPoints + torsoPoints + 3 * armPoints }); // left arm to left forearm
            cons.Add(new int[] { headPoints + torsoPoints - 1, headPoints + torsoPoints + 4 * armPoints }); // bototm to right thigh
            cons.Add(new int[] { headPoints + torsoPoints + 5 * armPoints - 1, headPoints + torsoPoints + 5 * armPoints }); // right thigh to right leg
            cons.Add(new int[] { headPoints + torsoPoints - 1, headPoints + torsoPoints + 6 * armPoints }); // bototm to left thigh
            cons.Add(new int[] { headPoints + torsoPoints + 7 * armPoints - 1, headPoints + torsoPoints + 7 * armPoints }); // left thigh to left

            for (int i = 0; i < points.Count; i++)
            {
                points[i] += new Vector2(-1, 3f);
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
        private static SpellImage.ImageInfo GetLinePoints2(Vector2 startPoint, Vector2 endPoint, int numPoints, int index)
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            for (int i = 1; i <= numPoints; i++)
            {
                points.Add(startPoint + i / (numPoints*1f) * (endPoint - startPoint));
                if (i > 1)
                {
                    cons.Add(new int[] { index + i - 2, index + i - 1 });
                }
            }
            return new SpellImage.ImageInfo(points, cons);
        }

        private static SpellImage.ImageInfo GetArcPoints(Vector2 middle, float radius, float startAngle, float endAngle, bool isCounterClockwise, int numPoints, int index)
        {
            List<Vector2> points = new();
            List<int[]> cons = new();
            float multiplier;
            if (isCounterClockwise)
            {
                multiplier = 1f;
            }
            else
            {
                multiplier = -1f;

            }
            for (int i = 0; i < numPoints; i++)
            {
                float angle = (startAngle + multiplier * i / (numPoints - 1f) * (endAngle - startAngle)) * Mathf.PI / 180f;

                points.Add(middle + new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle)));
                if (i > 0)
                {
                    cons.Add(new int[] { index + i - 1, index + i });
                }
            }
            return new SpellImage.ImageInfo(points, cons);
        }

    }
}