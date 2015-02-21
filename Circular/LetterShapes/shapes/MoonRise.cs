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
    class MoonRise : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                if (letter.Consonant != null && "_m_n_j_p_b_".Contains("_" + letter.Consonant + "_"))
                    return new MoonRise();
                else
                    return null;

            }
            else
            {
                if (letter.Consonant != null && "_b_d_f_g_h_ch_".Contains("_" + letter.Consonant + "_"))
                    return new Arc();
                else
                    return null;
            }
        }


        private Rectangle[] FancyLines;


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

                    border.AddArc(_WordParent.CircleBounds, StartAngle, (float)(_mainAngles[0] - StartAngle));
                    border.AddArc(LetterBounds, (float)_subAngles[0], (float)(SubArc));
                    border.AddArc(_WordParent.CircleBounds, (float)(_mainAngles[1]), (float)(EndAngle - _mainAngles[1]));

                    if (_Fancy)
                    {

                        GraphicsPath g = new GraphicsPath();
                        g.StartFigure();
                        g.AddArc(LetterBounds, (float)_subAngles[0], (float)(SubArc));
                        g.AddArc(FancyLines[FancyLines.Length - 1], (float)_subAngles[1], (float)(-1 * SubArc));
                        g.CloseFigure();

                        path.FillPath(Brushes.Black, g);

                        //foreach (var r in FancyLines)
                        //    path.DrawArc( new Pen(Color.Black,2), r, (float)subAngles[0], (float)(SubArc));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }

        public override void CalculateArc()
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                LetterRadius = maxInnerRadius * .8;
                if (LetterRadius > _WordParent.Radius * .4)
                    LetterRadius = _WordParent.Radius * .35;

                if (_Big)
                    LetterRadius *= 2;

                LetterBounds = MathHelps.BoundingRectangle(BehindLine(_WordParent.Radius - LetterRadius * .8), LetterRadius);
                FindEdges();

                if (_subAngles == null)
                {
                    LetterRadius *= 2;
                    LetterBounds = MathHelps.BoundingRectangle(BehindLine(_WordParent.Radius - LetterRadius * .8), LetterRadius);
                    FindEdges();
                }

                SubArc = (float)Math.Abs(_subAngles[1] - _subAngles[0]) - 360;

                if (_Fancy)
                {
                    Point p = MathHelps.D2Coords(LetterCenter, LetterRadius, MidAngle);

                    double l = Math.Sqrt(Math.Pow((0 - LetterCenter.X), 2) + Math.Pow((0- LetterCenter.Y), 2));
                    double mX = (0 - LetterCenter.X) / l;
                    double mY = (0 - LetterCenter.Y) / l;

                    FancyLines = new Rectangle[7];
                    for (int i = 0; i < FancyLines.Length; i++)
                    {
                        Point p1 = new Point((int)(LetterCenter.X + mX * (i + 1 * 2)), (int)(LetterCenter.Y + mY * (i + 1 * 2)));
                        double r2 = MathHelps.distance(p1, p);
                        FancyLines[i] = MathHelps.Circle2Rect(p1, r2);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }

        protected override LocationProb LocationProb()
        {
            return new LocationProb(
                vAbove: 0,
                vCenter: 1,
                vLeft: 1,
                dAbove: 1,
                dBottom: 0,
                dCenter: 1,
                dLeft: 1,
                dRight: 1);
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
            double arcWidth = SubArc;


            AddAnchor("Corner", new DecorationAnchor(
                new Point[] { 
                    MathHelps.D2Coords(LetterCenter, arcRadius, SubEndAngle), 
                    MathHelps.D2Coords(LetterCenter, arcRadius*1, SubEndAngle+2),
                    MathHelps.D2Coords(LetterCenter, arcRadius*1, SubEndAngle+6),
                    MathHelps.D2Coords(LetterCenter, arcRadius*1, SubEndAngle+10), 
                    MathHelps.D2Coords(LetterCenter, arcRadius*1.1, SubEndAngle+25), 
                    MathHelps.D2Coords(LetterCenter, arcRadius*1.5, SubEndAngle+75), 
                }, .02, 1, this));

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
                            arcRadius = r0 * .7;
                            arcMidAngle = SubStartAngle + arcWidth * .25f;
                            arcWidth = 50;
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
                            arcWidth = 70;
                            break;
                        }
                    case DecorationLocation.Top:
                        {
                            arcRadius = r0 * 1.26;
                            arcMidAngle = SubStartAngle + arcWidth / 2;
                            arcWidth = 50;
                            break;
                        }
                    case DecorationLocation.Left:
                        {
                            arcRadius = r0 * 1.1;
                            arcMidAngle = SubStartAngle + arcWidth * .75f;
                            arcWidth = 50;
                            break;
                        }
                    case DecorationLocation.Right:
                        {
                            arcRadius = r0 * .6;
                            arcMidAngle = SubStartAngle + arcWidth * .25f;
                            arcWidth = 50;
                            break;
                        }
                    case DecorationLocation.Center:
                        {
                            arcRadius = r0 * .6;
                            arcMidAngle = SubStartAngle + arcWidth / 2;
                            arcWidth = 0;
                            break;
                        }
                }


                if (decoration.GetType() == typeof(Circular.Decorations.Shapes.TwoLines))
                {

                    if (_Fancy)
                    {
                        if (FancyLines != null)
                        {
                            Rectangle r = FancyLines[FancyLines.Length - 1];

                            arcX = r.X + r.Width / 2;
                            arcY = r.Y + r.Height / 2;
                            arcRadius = r.Width / 2;
                        }
                    }
                    else
                        arcRadius = r0;

                }

                if ((decoration.GetType() == typeof(Circular.Decorations.Shapes.ThreeDots) || decoration.GetType() == typeof(Circular.Decorations.Shapes.TwoDots))
                    && decoration.Location == DecorationLocation.Top && _Fancy)
                {
                    arcRadius = r0 * 1.26 * 1.1;
                }

                if ((decoration.GetType() == typeof(Circular.Decorations.Shapes.ThreeDots) || decoration.GetType() == typeof(Circular.Decorations.Shapes.TwoDots))
                   && decoration.Location == DecorationLocation.Left && _Fancy)
                {
                    arcRadius = r0 * 1.26;
                }
            }
            decoration.CalculateDecoration(arcRadius, arcMidAngle, arcX, arcY, arcWidth);
        }
    }
}