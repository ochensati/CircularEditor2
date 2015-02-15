using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circular.Sentence
{
    public class PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return X + ", " + Y;
        }
    }
}
