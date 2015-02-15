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
    public class Horizon : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Consonant != null && "_t_d_w_st_s_".Contains("_" + letter.Consonant + "_"))
                return new Horizon();
            else
                return null;
        }

        protected override void DrawArc(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            double sAngle = _subAngles[0];
            Rectangle temp = LetterBounds;
            try
            {

                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things

                if (mockup)
                {
                    //used by the pathfinding algorythm to mark where the lines should not pass

                    path.FillPie(new SolidBrush(backgroundColor), LetterBounds, (float)_subAngles[0], (float)(SubArc));
                }
                else
                {

                    border.AddArc(_WordParent.CircleBounds, StartAngle, ArcWidth);

                    if (!_Fancy)
                    {
                        path.DrawArc(Pens.Black, LetterBounds, (float)_subAngles[0], (float)(SubArc));
                    }
                    else
                    {


                        const int line = 3;
                        sAngle = _subAngles[1];

                        if (sAngle < 0)
                            sAngle = 360 + sAngle;
                        for (int i = 0; i < fancyArcLengths.Length; i++)
                        {
                            path.DrawArc(new Pen(foregroundColor, line), temp, (float)(sAngle), (float)Math.Abs(fancyArcLengths[i] - 8));
                            sAngle += fancyArcLengths[i];
                        }

                        temp.Inflate(-line - 1, -line - 1);
                        sAngle = _subAngles[1];

                        if (sAngle < 0)
                            sAngle = 360 + sAngle;
                        for (int i = 0; i < fancyArcLengths1.Length; i++)
                        {
                            try
                            {
                                path.DrawArc(new Pen(foregroundColor, line), temp, (float)(sAngle), (float)Math.Abs(fancyArcLengths1[i] - 8));
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Print(ex.Message);
                            }
                            sAngle += fancyArcLengths1[i];
                        }

                        temp.Inflate(-line - 1, -line - 1);

                        sAngle = _subAngles[1];

                        if (sAngle < 0)
                            sAngle = 360 + sAngle;
                        for (int i = 0; i < fancyArcLengths2.Length; i++)
                        {
                            path.DrawArc(new Pen(foregroundColor, line), temp, (float)(sAngle), (float)Math.Abs(fancyArcLengths2[i] - 8));
                            sAngle += fancyArcLengths2[i];
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }



        double[] fancyArcLengths;
        double[] fancyArcLengths1;
        double[] fancyArcLengths2;

        public override void CalculateArc()
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                LetterRadius = maxInnerRadius * .8;
                //if (LetterRadius > _WordParent.WordRadius * .4)
                //    LetterRadius = _WordParent.WordRadius * .35;

                if (_Big)
                    LetterRadius *= 1.5;

                LetterBounds = MathHelps.BoundingRectangle(CenterLine, LetterRadius);


                FindEdges();

                this.SubArc = -1 * Math.Abs(-360 + Math.Abs(this.SubArc));

                if (_Fancy)
                {
                    fancyArcLengths = new double[5];
                    double arc = Math.Abs(this.SubArc);
                    double remaining = 0;
                    for (int i = 0; i < fancyArcLengths.Length; i++)
                    {
                        fancyArcLengths[i] = rnd.NextDouble() * .5;
                        remaining += fancyArcLengths[i];
                    }

                    for (int i = 0; i < fancyArcLengths.Length; i++)
                    {
                        fancyArcLengths[i] = arc * (fancyArcLengths[i] / remaining);
                    }

                    fancyArcLengths1 = new double[5];
                    remaining = 0;
                    for (int i = 0; i < fancyArcLengths1.Length; i++)
                    {
                        fancyArcLengths1[i] = rnd.NextDouble() * .5;
                        remaining += fancyArcLengths1[i];
                    }

                    for (int i = 0; i < fancyArcLengths1.Length; i++)
                    {
                        fancyArcLengths1[i] = arc * (fancyArcLengths1[i] / remaining);
                    }

                    fancyArcLengths2 = new double[5];
                    remaining = 0;
                    for (int i = 0; i < fancyArcLengths2.Length; i++)
                    {
                        fancyArcLengths2[i] = rnd.NextDouble() * .5;
                        remaining += fancyArcLengths2[i];
                    }

                    for (int i = 0; i < fancyArcLengths2.Length; i++)
                    {
                        fancyArcLengths2[i] = arc * (fancyArcLengths2[i] / remaining);
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
                vAbove: 1,
                vCenter: 0,
                vLeft: 1,
                dAbove: 1,
                dBottom: 1,
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
                        arcMidAngle = SubStartAngle + arcWidth / 2;
                        break;
                    }
                case DecorationLocation.Top:
                    {
                        arcRadius = r0 * 1.1;
                        arcMidAngle = SubStartAngle + arcWidth / 2;
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
                        arcRadius = r0 * 1;
                        arcMidAngle = SubStartAngle + arcWidth / 2;
                        break;
                    }

            }
            if (decoration.GetType() == typeof(Circular.Decorations.Shapes.TwoLines))
                arcRadius = r0;
            decoration.CalculateDecoration(arcRadius, arcMidAngle, arcX, arcY, arcWidth);
        }
    }
}
