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
    class Saturn : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                if (letter.Consonant != null && "_t_d_w_st_s_".Contains("_" + letter.Consonant + "_"))
                    return new Saturn();
                else
                    return null;

            }
            else
            {
                if (letter.Consonant != null && "_th_y_z_q_ng_x_".Contains("_" + letter.Consonant + "_"))
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
                    //newBounds = edge.BoundingRectangle(edge.CenterLine, maxRadius);

                    if (!_Fancy)
                    {
                        Pen p = new Pen(foregroundColor, 2);
                        path.DrawEllipse(p, LetterBounds);
                    }
                    else
                    {
                        double sAngle = startsFancyArc;
                        for (int i = 0; i < fancyArcLengths.Length; i++)
                        {
                            path.DrawArc(new Pen(foregroundColor, 3), LetterBounds, (float)(sAngle), (float)(fancyArcLengths[i] - 5));
                            sAngle += fancyArcLengths[i];
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }

        double startsFancyArc = rnd.NextDouble() * 360;
        double[] fancyArcLengths;

        public override void CalculateArc()
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                LetterRadius = maxInnerRadius * .8;
                if (LetterRadius > _WordParent.Radius * .7)
                    LetterRadius = _WordParent.Radius * .7;

                if (_Big)
                    LetterRadius *= 1.5;

                LetterBounds = MathHelps.BoundingRectangle(CenterLine, LetterRadius);

                FindEdges();

                fancyArcLengths = new double[5];
                double remaining = 0;
                for (int i = 0; i < fancyArcLengths.Length; i++)
                {
                    fancyArcLengths[i] = rnd.NextDouble();
                    remaining += fancyArcLengths[i];
                }

                for (int i = 0; i < fancyArcLengths.Length; i++)
                {
                    fancyArcLengths[i] = 360 * (fancyArcLengths[i] / remaining);
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
                vAbove: 1,
                vCenter: 0,
                vLeft: 1,
                dAbove: 1,
                dBottom: 0,
                dCenter: 0,
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

            switch (decoration.Location)
            {

                case DecorationLocation.Bottom:
                    {
                        arcRadius = r0 * .8;
                        break;
                    }
                case DecorationLocation.Top:
                    {
                        arcRadius = r0 * 1.1;
                        break;
                    }
                case DecorationLocation.Left:
                    {
                        arcRadius = r0 * 1.1;
                        arcMidAngle = SubStartAngle + arcWidth * .75f;
                        break;
                    }
                case DecorationLocation.Right:
                    {
                        arcRadius = r0 * 1.1;
                        arcMidAngle = SubStartAngle + arcWidth * .25f;
                        break;
                    }
                case DecorationLocation.Center:
                    {
                        arcX = 0;
                        arcY = 0;

                        arcRadius = _WordParent.Radius;
                        arcMidAngle = MidAngle;
                        arcWidth = this.ArcWidth;
                        break;
                    }

            }
            if (decoration.GetType() == typeof(Circular.Decorations.Shapes.TwoLines))
                arcRadius = r0;
            decoration.CalculateDecoration(arcRadius, arcMidAngle, arcX, arcY, arcWidth);
        }
    }
}
