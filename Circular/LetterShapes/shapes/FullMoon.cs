using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using Circular.Decorations;
using System.Drawing;
using Circular.Words;

namespace Circular.LetterShapes.Shapes
{
    [Serializable]
    class FullMoon : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                if (letter.Consonant != null && "_l_k_c_g_q_v_".Contains("_" + letter.Consonant + "_"))
                    return new FullMoon();
                else
                    return null;

            }
            else
            {
                if (letter.Consonant != null && "_j_k_c_l_m_n_p_".Contains("_" + letter.Consonant + "_"))
                    return new Arc();
                else
                    return null;
            }
        }

        protected override void DrawArc(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            try
            {
                if (mockup)
                {
                    //used by the pathfinding algorythm to mark where the lines should not pass

                    path.FillEllipse(new SolidBrush(backgroundColor), LetterBounds);
                }
                else
                {

                    //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                    border.AddArc(_WordParent.CircleBounds, StartAngle, ArcWidth);

                    if (_Fancy)
                    {
                        GraphicsPath[] FancyArcs = new GraphicsPath[3];
                        for (int i = 0; i < 3; i++)
                        {

                            float startAngle = (float)(StartFAngle1 + (360d / 3) * i + CircleAngle);
                            float arc = (float)(360f / 3);

                            Point refP = MathHelps.D2Coords(LetterCenter, LetterRadius, startAngle);





                            Point p1 = MathHelps.D2Coords(LetterCenter, 1, startAngle + 180);
                            double r2 = MathHelps.distance(p1, refP);
                            Rectangle r = MathHelps.Circle2Rect(p1, r2);


                            GraphicsPath g = new GraphicsPath();
                            g.StartFigure();
                            g.AddArc(r, startAngle, arc);

                            Point p2;
                            Point p3;
                            p2 = MathHelps.D2Coords(p1, r2, startAngle + arc);

                            p1 = MathHelps.D2Coords(LetterCenter, 7, startAngle + 180);
                            r2 = MathHelps.distance(p1, refP);
                            r = MathHelps.Circle2Rect(p1, r2);


                            p3 = MathHelps.D2Coords(p1, r2, startAngle + arc);
                            g.AddLine(p2, p3);
                            g.AddArc(r, startAngle + arc, (float)(-1 * arc));

                            g.CloseFigure();

                            FancyArcs[i] = g;
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            path.FillPath(Brushes.Black, FancyArcs[i]);
                            //for (int j = 0; j < FancyLines[i].Length; j++)
                            //{
                            //    path.DrawArc(new Pen(Color.Black, 3), FancyLines[i][j], (float)(StartFAngle1 + 360d/3*i), (float)(360d/3));
                            //}
                        }
                    }
                    else
                    {
                        Pen p = new Pen(Color.Black, 2);
                        path.DrawEllipse(p, LetterBounds);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }


        private double StartFAngle1 = rnd.NextDouble() * 360;



        public override void CalculateArc()
        {

            LetterRadius = maxInnerRadius * .8;
            if (LetterRadius > _WordParent.Radius * .4)
                LetterRadius = _WordParent.Radius * .35;

            if (_Big)
                LetterRadius *= 1.5;

            LetterBounds = MathHelps.BoundingRectangle(BehindLine(_WordParent.Radius - LetterRadius * 1.1), LetterRadius);


            FindEdges();

            if (_Fancy)
            {

                //aweward, but just adapting to see if it works


                GraphicsPath[] FancyArcs = new GraphicsPath[3];
                for (int i = 0; i < 3; i++)
                {

                    float startAngle = (float)(StartFAngle1 + (360d / 3) * i);
                    float arc = (float)(360f / 3);

                    Point refP = MathHelps.D2Coords(LetterCenter, LetterRadius, startAngle);





                    Point p1 = MathHelps.D2Coords(LetterCenter, 1, startAngle + 180);
                    double r2 = MathHelps.distance(p1, refP);
                    Rectangle r = MathHelps.Circle2Rect(p1, r2);


                    GraphicsPath g = new GraphicsPath();
                    g.StartFigure();
                    g.AddArc(r, startAngle, arc);

                    Point p2;
                    Point p3;
                    p2 = MathHelps.D2Coords(p1, r2, startAngle + arc);

                    p1 = MathHelps.D2Coords(LetterCenter, 7, startAngle + 180);
                    r2 = MathHelps.distance(p1, refP);
                    r = MathHelps.Circle2Rect(p1, r2);


                    p3 = MathHelps.D2Coords(p1, r2, startAngle + arc);
                    g.AddLine(p2, p3);
                    g.AddArc(r, startAngle + arc, (float)(-1 * arc));

                    g.CloseFigure();

                    FancyArcs[i] = g;
                }
            }
        }

        protected override LocationProb LocationProb()
        {
            return new LocationProb(
                vAbove: 0,
                vCenter: 1,
                vLeft: 1,
                dAbove: 1,
                dBottom: 1,
                dCenter: 1,
                dLeft: .25,
                dRight: .25);
        }

        protected override void CalculateDecoration(Decorations.aDecoration decoration)
        {

            double x0 = LetterBounds.X + LetterRadius;
            double y0 = LetterBounds.Y + LetterRadius;
            double r0 = LetterRadius;

            double arcRadius = r0;
            double arcMidAngle = MidAngle + 180;
            double arcX = x0;
            double arcY = y0;
            double arcWidth = 50;


            if (decoration.GetType() == typeof(Circular.Decorations.Shapes.Rings))
            {

                switch (decoration.Location)
                {
                    case DecorationLocation.Bottom:
                    case DecorationLocation.Top:
                    case DecorationLocation.Right:
                    case DecorationLocation.Left:
                    case DecorationLocation.Center:
                        {
                            arcRadius = r0 * 1.2;
                            arcMidAngle = arcMidAngle + 45;
                            break;
                        }

                }
            }
            else
            {
                switch (decoration.Location)
                {

                    case DecorationLocation.Bottom:
                        {
                            arcRadius = r0 * .7;
                            arcMidAngle = MidAngle;
                            arcWidth = 180;
                            break;
                        }
                    case DecorationLocation.Top:
                        {
                            arcRadius = r0 * 1.2;
                            break;
                        }
                    case DecorationLocation.Left:
                        {
                            arcRadius = r0 * .7;
                            arcMidAngle = arcMidAngle - 45;
                            break;
                        }
                    case DecorationLocation.Right:
                        {
                            arcRadius = r0 * 1.2;
                            arcMidAngle = arcMidAngle + 45;
                            break;
                        }
                    case DecorationLocation.Center:
                        {
                            arcRadius = r0 * .5;

                            break;
                        }

                }
                if (decoration.GetType() == typeof(Circular.Decorations.Shapes.TwoLines))
                    arcRadius = r0;
            }
            decoration.CalculateDecoration(arcRadius, arcMidAngle, arcX, arcY, arcWidth);
        }
    }
}
