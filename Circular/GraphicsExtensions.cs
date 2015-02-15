using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Circular
{
    public static class GraphicsExtensions
    {
        public const double PiTimesTwo = Math.PI * 2;
        public const double DegreesInOneRadian = 180 / Math.PI;

        public enum Direction : int
        {
            AntiClockwise = -1,
            Clockwise = 1
        }

        /// <summary>
        /// Draws a spiral from the startRadius to the endRadius
        /// at the point specified by 'center' in the direction
        /// specified by the 'dir' variable as Direction using the pen specified by 'myPen'.
        /// The startAngle in degrees is measured from the right or the EAST side of a 360 degree compass.
        /// E.G: If direction is Clockwise and startAngle = 45 then the start will be 45 degrees below EAST
        /// otherwise if direction is AntiClockwise it will be 45 degrees above EAST relative to the center of the spiral.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="myPen"></param>
        /// <param name="center"></param>
        /// <param name="startRadius"></param>
        /// <param name="endRadius"></param>
        /// <param name="dir"></param>
        /// <param name="distanceBetweenTurnsInPixels"></param>
        /// <param name="startAngleInDegrees"></param>
        /// <remarks></remarks>

        public static void DrawSpiral(this System.Drawing.Graphics g, Pen myPen, Point center, int startRadius,
                              int endRadius, Direction dir, double distanceBetweenTurnsInPixels,
                              double startAngleInDegrees, double increaseRadius)
        {
            double angle1 = startAngleInDegrees / DegreesInOneRadian;
            double angle2 = (startAngleInDegrees + 360) / DegreesInOneRadian;
            double radius = startRadius;
            double x1, y1;

            List<Point> mypoints = new List<Point>();

            if (startRadius > endRadius)
            {
                for (double index = angle1; radius > endRadius; index += 0.05)
                {
                    radius = radius - (distanceBetweenTurnsInPixels / (PiTimesTwo / 0.05));
                    x1 = radius * Math.Cos(index) + center.X;
                    y1 = (int)dir * radius * Math.Sin(index) + center.Y;
                    mypoints.Add(new Point((int)(x1), (int)(y1)));

                    distanceBetweenTurnsInPixels *= increaseRadius;
                }
            }


            if (startRadius < endRadius)
            {
                for (double index = angle1; radius < endRadius; index += 0.05)
                {
                    radius = radius + (distanceBetweenTurnsInPixels / (PiTimesTwo / 0.05));
                    x1 = radius * Math.Cos(index) + center.X;
                    y1 = (int)dir * radius * Math.Sin(index) + center.Y;
                    mypoints.Add(new Point((int)(x1), (int)(y1)));
                    distanceBetweenTurnsInPixels *= increaseRadius;
                }
            }

            g.DrawCurve(myPen, mypoints.ToArray());
        }

        public static void DrawSpiral(this System.Drawing.Graphics g, Pen myPen, Point center, int startRadius,
                            Direction dir, double distanceBetweenTurnsInPixels,
                            double startAngleInDegrees, double increaseRadius, Point Bounds, double boundRadius)
        {
            double angle1 = startAngleInDegrees / DegreesInOneRadian;
            double angle2 = (startAngleInDegrees + 360) / DegreesInOneRadian;
            double radius = startRadius;
            double x1, y1;

            List<Point> mypoints = new List<Point>();


            double cRadius = 0;
            for (double index = angle1; cRadius < boundRadius; index += 0.05)
            {
                radius = radius + (distanceBetweenTurnsInPixels / (PiTimesTwo / 0.05));
                x1 = radius * Math.Cos(index) + center.X;
                y1 = (int)dir * radius * Math.Sin(index) + center.Y;
                mypoints.Add(new Point((int)(x1), (int)(y1)));
                distanceBetweenTurnsInPixels *= increaseRadius;

                cRadius = MathHelps.distance(Bounds, new Point((int)x1, (int)y1));
            }

            g.DrawCurve(myPen, mypoints.ToArray());
        }

    }

}
