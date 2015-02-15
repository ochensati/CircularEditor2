using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Circular
{
    public interface iMouseable
    {
        iMouseable HitTest(Point p);
        Point DrawCenter { get; set; }
        void MoveDelta(Point originalPoint, int deltaX, int deltaY);

        Point GetStartAngle(Point mousePoint, out double startAngle);
        void SetAngleDelta(double setAngle);
    }

    public interface iArcJoin
    {
        void UseWordForArc(aCircleObject join);
    }
}
