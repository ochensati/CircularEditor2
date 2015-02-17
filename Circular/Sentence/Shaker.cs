using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Circular.Sentence
{

    public class Shaker
    {
        public class Dot
        {
            public double radius;
            public PointD center = new PointD(0, 0);
            public double angle;
            public PointD acceleration = new PointD(0, 0);
            public bool Fixed = false;
            public string name = "";
            public Rectangle Bounds
            {
                get
                {
                    return new Rectangle((int)(center.X- radius),(int)(center.Y -radius), (int)(radius*2),(int)(radius*2));

                }
            }

            public Dot(double radius, PointD center, double angle, bool fixedPoint)
            {
                this.radius = radius;
                this.center = center;
                this.angle = angle;
                this.Fixed = fixedPoint;
            }

            public Dot(Rectangle circleRect, bool fixedPoint)
            {
                this.radius = circleRect.Width/2;
                this.center = new PointD(circleRect.Width/2 + circleRect.X, circleRect.Y + circleRect.Height/2);
                this.angle = 0;
                this.Fixed = fixedPoint;
            }

            public Dot(double radius, Point center, double angle, bool fixedPoint)
            {
                this.radius = radius;
                this.center = new PointD(center.X, center.Y);
                this.angle = angle;
                this.Fixed = fixedPoint;
            }
        }

        public class Connection
        {
            public double radius;
            public double Strength;
            public int startDot;
            public int endDot;
            public Connection(double radius, int startDot, int endDot)
            {
                this.radius = radius;
                this.startDot = startDot;
                this.endDot = endDot;

            }
        }

        private double distance(PointD p1, PointD p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private PointD UnitVector(double magnitude, PointD p1, PointD p2)
        {
            double x = p1.X - p2.X;
            double y = p1.Y - p2.Y;
            double r = Math.Sqrt(x * x + y * y);

            if (r > .1)
            {
                x = magnitude * x / r;
                y = magnitude * y / r;
            }
            else
            {
                x = 0; y = 0;
            }

            return new PointD(x, y);
        }

        private static Random rnd = new Random();
        public void Jiggle(List<Dot> dots, List<Connection> connections, double kWhole, double dt, double attraction, int jump, int iter, int iterations)
        {

            double[,] radii = new double[dots.Count, dots.Count];
            double[,] strength = new double[dots.Count, dots.Count];
            for (int i = 0; i < dots.Count; i++)
            {
                for (int j = i + 1; j < dots.Count; j++)
                {
                    radii[i, j] = dots[i].radius + dots[j].radius;
                    strength[i, j] = kWhole;
                }
            }

            for (int conI = 0; conI < connections.Count; conI++)
            {
                radii[connections[conI].startDot, connections[conI].endDot] = connections[conI].radius;
                radii[connections[conI].endDot, connections[conI].startDot] = connections[conI].radius;
                strength[connections[conI].endDot, connections[conI].startDot] = connections[conI].Strength;
            }

            //handle the generic space exclusion and mild attractions
            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {
                for (int DotI2 = DotI1 + 1; DotI2 < dots.Count; DotI2++)
                {
                    double dist = distance(dots[DotI1].center, dots[DotI2].center);
                    double idealDistance = radii[DotI1, DotI2];

                    if (dist < 10 * (1 - .7 * iter / iterations) * idealDistance)
                    {
                        if (idealDistance == 0)
                            idealDistance = 1;

                        double f = Math.Exp(-.4 * dist * dist / (idealDistance * idealDistance));

                        f = f - Math.Exp(-.4 * idealDistance * idealDistance / (idealDistance * idealDistance));
                        if (f > 0)
                        {
                            f = f * 15;
                            if (iter > .4 * iterations)
                                f = f * 6;
                        }


                        if (f < 0)
                        {
                            f = -1 * f / 2;

                            if (iter > .4 * iterations)
                                f = f * .1;

                        }

                        PointD force = UnitVector(f, dots[DotI1].center, dots[DotI2].center);

                        dots[DotI1].acceleration.X += force.X;
                        dots[DotI1].acceleration.Y += force.Y;

                        dots[DotI2].acceleration.X -= force.X;
                        dots[DotI2].acceleration.Y -= force.Y;
                    }
                }
            }

            //pull in the items
            double k2 = -1 * Math.Pow(1 + 1000 * iter / iterations, .5) * kWhole;
            for (int conI = 0; conI < connections.Count; conI++)
            {
                Connection c = connections[conI];
                double dist = distance(dots[c.startDot].center, dots[c.endDot].center);

                //target seperation distance
                dist = dist - radii[c.startDot, c.endDot];

                dist = dist * dist * Math.Sign(dist);
                double k3;
                if (Math.Abs(k2 * dist) > 3)
                {
                    k3 = 3 * Math.Sign(k2 * dist);
                }
                else
                    k3 = k2 * dist;

                PointD force = UnitVector(k3, dots[c.startDot].center, dots[c.endDot].center);

                dots[c.startDot].acceleration.X += force.X;
                dots[c.startDot].acceleration.Y += force.Y;

                dots[c.endDot].acceleration.X -= force.X;
                dots[c.endDot].acceleration.Y -= force.Y;
            }

            double jump2 = Math.Pow(1 - iter / iterations, 2) * jump;
            double dt2 = Math.Pow(1 - .3 * iter / iterations, 2) * dt;

            if (iter > iterations * .8)
                jump2 = 0;
            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {

                if (Math.Abs(dots[DotI1].acceleration.X * dt) < 10)
                    dots[DotI1].center.X += (dots[DotI1].acceleration.X * dt2 + jump2 * rnd.NextDouble());
                else
                    dots[DotI1].center.X += (5 * Math.Sign(dots[DotI1].acceleration.X) + jump * rnd.NextDouble());

                if (Math.Abs(dots[DotI1].acceleration.X * dt) < 10)
                    dots[DotI1].center.Y += (dots[DotI1].acceleration.Y * dt2 + jump2 * rnd.NextDouble());
                else
                    dots[DotI1].center.Y += (5 * Math.Sign(dots[DotI1].acceleration.Y) + jump2 * rnd.NextDouble());

                dots[DotI1].acceleration = new PointD(0, 0);
            }


            double cogX = 0;
            double cogY = 0;
            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {
                cogX += dots[DotI1].center.X;
                cogY += dots[DotI1].center.Y;
            }
            cogX /= dots.Count;
            cogY /= dots.Count;

            cogX = 0 - cogX;
            cogY = 0 - cogY;

            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {
                dots[DotI1].center.X += cogX;
                dots[DotI1].center.Y += cogY;
            }
        }

        public void Jiggle(List<Dot> dots, List<Connection> connections, double radius, double kWhole, double dt, double attraction, int jump, int iter, int iterations)
        {
            double[,] radii,  strength;
            CalculateDistances(dots, connections, kWhole,out radii,out strength);

            SetPrivateSpace(dots, radii, strength, iter, iterations);

            SetConnections(dots, connections, radii, kWhole, iter, iterations);

            SetRingAttraction(dots, radius);

            DoMove(dots, radius, dt, jump, iter, iterations);

            RecenterDots(dots);
        }

        private void CalculateDistances(List<Dot> dots, List<Connection> connections, double kWhole, out double[,] radii, out double[,] strength)
        {
            radii = new double[dots.Count, dots.Count];
            strength = new double[dots.Count, dots.Count];
            for (int i = 0; i < dots.Count; i++)
            {
                for (int j = i + 1; j < dots.Count; j++)
                {
                    radii[i, j] = dots[i].radius + dots[j].radius + 10;
                    strength[i, j] = kWhole;
                }
            }

            for (int conI = 0; conI < connections.Count; conI++)
            {
                radii[connections[conI].startDot, connections[conI].endDot] = connections[conI].radius;
                radii[connections[conI].endDot, connections[conI].startDot] = connections[conI].radius;
                strength[connections[conI].endDot, connections[conI].startDot] = connections[conI].Strength;
            }

        }

        private void DoMove(List<Dot> dots, double radius, double dt, int jump, int iter, int iterations)
        {
            double jump2 =  Math.Pow( (1 -iter / iterations), 7) * jump;
            double dt2 = Math.Pow(1 - .3 * iter / iterations, 1) * dt;

         //   double outRadius = (1 + (1 - iter / iterations))* radius;

            if (iter > iterations * .8)
                jump2 = 0;
            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {

                if (dots[DotI1].Fixed == false)
                {
                    if (Math.Abs(dots[DotI1].acceleration.X * dt) < 10)
                        dots[DotI1].center.X += (dots[DotI1].acceleration.X * dt2 + jump2 * rnd.NextDouble());
                    else
                        dots[DotI1].center.X += (dots[DotI1].acceleration.X * dt2/10 + jump2 * rnd.NextDouble());



                    if (Math.Abs(dots[DotI1].acceleration.Y * dt) < 10)
                        dots[DotI1].center.Y += (dots[DotI1].acceleration.Y * dt2 + jump2 * rnd.NextDouble());
                    else
                        dots[DotI1].center.Y += (dots[DotI1].acceleration.Y * dt2 / 10 + jump2 * rnd.NextDouble());



                    if (Math.Abs(dots[DotI1].center.X) > 1000 || Math.Abs(dots[DotI1].center.Y) > 1000)
                        dots[DotI1].center = new PointD(5, 5);

                }
                dots[DotI1].acceleration = new PointD(0, 0);
            }

        }

        private void RecenterDots(List<Dot> dots)
        {
            double cogX = 0;
            double cogY = 0;
            int cc = 0;
            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {
                if (dots[DotI1].Fixed == false)
                {
                    cogX += dots[DotI1].center.X;
                    cogY += dots[DotI1].center.Y;
                    cc++;
                }
            }
            cogX /= cc;
            cogY /= cc;

            cogX = 0 - cogX;
            cogY = 0 - cogY;

            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {
                if (dots[DotI1].Fixed == false)
                {
                    dots[DotI1].center.X += cogX;
                    dots[DotI1].center.Y += cogY;
                }
            }
        }

        public void SetPrivateSpace(List<Dot> dots, double[,] radii, double[,] strength, int iter, int iterations)
        {
            //handle the generic space exclusion and mild attractions
            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {
                for (int DotI2 = DotI1 + 1; DotI2 < dots.Count; DotI2++)
                {
                    double dist = distance(dots[DotI1].center, dots[DotI2].center);
                    double idealDistance = radii[DotI1, DotI2];

                    //if (dist < 10 * (1 - .3 * iter / iterations) * idealDistance)
                    {
                        if (idealDistance == 0)
                            idealDistance = 1;

                        double f = Math.Exp(-.4 * dist * dist / (idealDistance * idealDistance));

                        //f = f - Math.Exp(-.4 );
                        if (f > 0)
                        {
                            f = f * 25;
                            //if (iter > .4 * iterations)
                            //    f = f * 16;
                        }
                        if (dist > idealDistance)
                            f = 0;

                        PointD force = UnitVector(f, dots[DotI1].center, dots[DotI2].center);

                        if (dots[DotI2].Fixed)
                        {
                            dots[DotI1].acceleration.X += 4 * force.X;
                            dots[DotI1].acceleration.Y += 4 * force.Y;
                        }
                        else
                        {
                            dots[DotI1].acceleration.X += force.X;
                            dots[DotI1].acceleration.Y += force.Y;
                        }

                        if (dots[DotI1].Fixed)
                        {
                            dots[DotI2].acceleration.X -= 4 * force.X;
                            dots[DotI2].acceleration.Y -= 4 * force.Y;
                        }
                        else
                        {
                            dots[DotI2].acceleration.X -= force.X;
                            dots[DotI2].acceleration.Y -= force.Y;
                        }
                    }
                }
            }

        }

        public void SetConnections(List<Dot> dots, List<Connection> connections, double[,] radii, double kWhole, int iter, int iterations)
        {
            //pull in the items
            double k2 = -1 * kWhole;//* Math.Pow(1 + 100 * iter / iterations, .5) * kWhole;
            for (int conI = 0; conI < connections.Count; conI++)
            {
                Connection c = connections[conI];
                double dist = distance(dots[c.startDot].center, dots[c.endDot].center);

                //target seperation distance
                dist = dist - radii[c.startDot, c.endDot];

            //    dist = dist *  Math.Sign(dist);
                double k3=k2*dist;
                //if (Math.Abs(k2 * dist) > 3)
                //{
                //    k3 = 3 * Math.Sign(k2 * dist);
                //}
                //else
                //    k3 = k2 * dist;

              
                PointD force = UnitVector(k3, dots[c.startDot].center, dots[c.endDot].center);

                dots[c.startDot].acceleration.X += force.X;
                dots[c.startDot].acceleration.Y += force.Y;

                dots[c.endDot].acceleration.X -= force.X;
                dots[c.endDot].acceleration.Y -= force.Y;
            }



        }

        public void SetRingAttraction(List<Dot> dots, double radius)
        {
            for (int DotI1 = 0; DotI1 < dots.Count; DotI1++)
            {
                double dist = distance(dots[DotI1].center, new PointD(0, 0)) ;

                if (dist + dots[DotI1].radius < radius)
                {
                    double f = -.0005 * dist;

                    PointD force = UnitVector(f, dots[DotI1].center, new PointD(0, 0));

                    dots[DotI1].acceleration.X += force.X;
                    dots[DotI1].acceleration.Y += force.Y;

                }
                else
                {
                    double f =  -.5* dist;

                    PointD force =  UnitVector( f, dots[DotI1].center, new PointD(0, 0));

                    dots[DotI1].acceleration.X += force.X;
                    dots[DotI1].acceleration.Y += force.Y;

                }
            }

        }
    }
}

