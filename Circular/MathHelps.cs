using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circular.Sentence;

namespace Circular
{
    public static class MathHelps
    {
        public static double DistanceFromLine(Point Line1, Point Line2, Point p)
        {

            return DistanceFromLine(Line1.X, Line1.Y, Line2.X, Line2.Y, p.X, p.Y);

        }
        public static double DistanceFromLine(double x1, double y1, double x2, double y2, double x0, double y0)
        {

            return ((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1) / (Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2)));

        }


        public static Point[] FindIntersections(Rectangle circle1, Rectangle circle2)
        {
            double r0 = circle2.Width / 2;

            double a = circle2.X + circle2.Width / 2d;
            double b = circle2.Y + circle2.Height / 2d;

            double c = circle1.X + circle1.Width / 2d;
            double d = circle1.Y + circle1.Height / 2d;
            double r1 = circle1.Width / 2d;

            double D = Math.Sqrt(Math.Pow(a - c, 2) + Math.Pow(b - d, 2));

            if (Math.Abs(D - (r1 + r0)) < 4)
                return null;


            double delta = 1.0 / 4.0 * Math.Sqrt(
                (D + r0 + r1) * (D + r0 - r1) * (D - r0 + r1) * (-1 * D + r0 + r1));

            double x1 = (a + c) / 2 + (c - a) * (r0 * r0 - r1 * r1) / 2 / D / D + 2 * (b - d) * delta / D / D;
            double x2 = (a + c) / 2 + (c - a) * (r0 * r0 - r1 * r1) / 2 / D / D - 2 * (b - d) * delta / D / D;

            double y1 = (b + d) / 2 + (d - b) * (r0 * r0 - r1 * r1) / 2 / D / D - 2 * (a - c) * delta / D / D;
            double y2 = (b + d) / 2 + (d - b) * (r0 * r0 - r1 * r1) / 2 / D / D + 2 * (a - c) * delta / D / D;

            if (D == (r1 + r0))
                return new Point[] { new Point((int)x1, (int)y1) };


            return new Point[] { new Point((int)x1, (int)y1), new Point((int)x2, (int)y2) };
        }

        public static double distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static double distance(PointD p1, PointD p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }


        public static double distance(Rectangle r, Point p2)
        {
            Point p1 = new Point((int)(r.X + r.Width / 2f), (int)(r.Y + r.Height / 2d));
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }


        public static Rectangle BoundingRectangle(Point center, double radius)
        {
            return Circle2Rect(center, radius);// new Rectangle((int)(center.X - radius), (int)(center.Y - radius), (int)(2 * radius), (int)(2 * radius));
        }


        public static double Atan2(double y, double x)
        {
            if ((y + x) == 0)
                return 0;

            double angle = Math.Atan(Math.Abs(y / x)) / Math.PI * 180;

            if (x >= 0 && y >= 0)
            {
                return angle;
            }

            if (x >= 0 && y <= 0)
            {
                return 360 - angle;
            }

            if (x <= 0 && y >= 0)
            {
                return 180 - angle;
            }

            return 180 + angle;


        }

        public static double Atan2(Point p1, Point p2)
        {
            double x = p1.X - p2.X;
            double y = p1.Y - p2.Y;
            double angle = Math.Atan(Math.Abs(y / x)) / Math.PI * 180;

            if (x >= 0 && y >= 0)
            {
                return angle;
            }

            if (x >= 0 && y <= 0)
            {
                return 360 - angle;
            }

            if (x <= 0 && y >= 0)
            {
                return 180 - angle;
            }

            return 180 + angle;


        }

        public static Point D2Coords(Rectangle center, double degrees)
        {
            return new Point((int)(center.X + center.Width / 2 + center.Width / 2 * Math.Cos(degrees / 180 * Math.PI)),
                (int)(center.Y + center.Height / 2 + center.Height / 2 * Math.Sin(degrees / 180 * Math.PI)));
        }

        public static Point D2Coords(Point center, double radius, double degrees)
        {
            return new Point((int)(center.X + radius * Math.Cos(degrees / 180 * Math.PI)), (int)(center.Y + radius * Math.Sin(degrees / 180 * Math.PI)));
        }

        public static Point D2Coords(double centerX, double centerY, double radius, double degrees)
        {
            return new Point((int)(centerX + radius * Math.Cos(degrees / 180 * Math.PI)), (int)(centerY + radius * Math.Sin(degrees / 180 * Math.PI)));
        }

        public static Rectangle Circle2Rect(Point X, double radius)
        {
            return new Rectangle((int)(X.X - radius), (int)(X.Y - radius), (int)(2 * radius), (int)(2 * radius));
        }

        public static Rectangle Circle2Rect(PointF X, double radius)
        {
            return new Rectangle((int)(X.X - radius), (int)(X.Y - radius), (int)(2 * radius), (int)(2 * radius));
        }

        public static Rectangle Circle2Rect(Point X, double minRadius, double radius)
        {
            if (minRadius > radius)
                return new Rectangle((int)(X.X - minRadius), (int)(X.Y - minRadius), (int)(2 * minRadius), (int)(2 * minRadius));
            else
                return new Rectangle((int)(X.X - radius), (int)(X.Y - radius), (int)(2 * radius), (int)(2 * radius));
        }

        public static Rectangle Circle2Rect(PointF X, double minRadius, double radius)
        {
            if (minRadius > radius)
                return new Rectangle((int)(X.X - minRadius), (int)(X.Y - minRadius), (int)(2 * minRadius), (int)(2 * minRadius));
            else
                return new Rectangle((int)(X.X - radius), (int)(X.Y - radius), (int)(2 * radius), (int)(2 * radius));
        }

        public static Rectangle Circle2Rect(double x, double y, double radius)
        {
            return new Rectangle((int)(x - radius), (int)(y - radius), (int)(2 * radius), (int)(2 * radius));
        }

        public static Rectangle Circle2Rect(Rectangle rect, double radius)
        {
            return new Rectangle((int)(rect.X + rect.Width / 2d - radius), (int)(rect.Y + rect.Height / 2d - radius), (int)(2 * radius), (int)(2 * radius));
        }

        public static double[] GetAngles(Point[] points, Point pivotCenter)
        {
            double[] angles = new double[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                angles[i] = Atan2(points[i].Y - pivotCenter.Y, points[i].X - pivotCenter.X);
            }

            return angles;
        }


        public static double[] GetAngles(Point[] points, Point pivotCenter, Rectangle circleBounds)
        {
            double x0 = circleBounds.X + circleBounds.Width / 2;
            double y0 = circleBounds.Y + circleBounds.Height / 2;

            double[] angles = new double[points.Length + 1];
            for (int i = 0; i < points.Length; i++)
            {
                angles[i] = Atan2(points[i].Y - y0, points[i].X - x0);
            }

            angles[points.Length] = Atan2(pivotCenter.Y - y0, pivotCenter.X - x0);
            return angles;
        }

        /// <summary>
        /// Travel from p1 (0) to p2 (1) by amount specified on u (0..1)
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="startPoint"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public static Point TravelLine(Point endPoint, Point startPoint, double u)
        {
            return new Point((int)(endPoint.X * u + (1 - u) * startPoint.X), (int)(endPoint.Y * u + (1 - u) * startPoint.Y));
        }

        public static Point TravelLineLiteral(Point endPoint, Point startPoint, double u)
        {
            return new Point((int)((u) / (endPoint.X - startPoint.X + .0001) + startPoint.X), (int)((u) / (endPoint.Y - startPoint.Y + .0001) + startPoint.Y));
        }
    }
}
