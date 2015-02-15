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
    public class Arc : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {

            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                if (letter.Consonant != null && "_r_z_h_ng_f_".Contains("_" + letter.Consonant + "_"))
                    return new Arc();
                else
                    return null;
            }
            else
            {
                if (letter.Consonant != null && "_t_sh_r_s_v_w_".Contains("_" + letter.Consonant + "_"))
                    return new Arc();
                else
                    return null;
            }
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
                    float arc1 = (float)Math.Abs(_mainAngles[0] - StartAngle);
                    float arc2 = (float)Math.Abs(EndAngle - _mainAngles[1]);
                    border.AddArc(_WordParent.CircleBounds, StartAngle, arc1);
                    border.AddArc(LetterBounds, (float)_subAngles[0], (float)(SubArc));
                    border.AddArc(_WordParent.CircleBounds, (float)(_mainAngles[1]), arc2);

                    if (_Fancy)
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

        public override double PreferredAngle()
        {
            return 1;
        }

        public override void UseWordForArc(aCircleObject otherWord)
        {
            LetterRadius = otherWord.Radius + 20;

            var p = new Point(otherWord.DrawCenter.X - this._WordParent.DrawCenter.X, otherWord.DrawCenter.Y - this._WordParent.DrawCenter.Y);
            double r2 = Math.Sqrt(p.X * p.X + p.Y * p.Y);

            if (r2 < (LetterRadius + _WordParent.Radius))
            {
                double angle = MathHelps.Atan2(p.Y, p.X) - this._WordParent.CircleAngle;
                LetterBounds = MathHelps.Circle2Rect(MathHelps.D2Coords(new Point((int)0, (int)0), r2, angle), LetterRadius);

                FinalizeCalc();


                foreach (var d in Decorations)
                {
                    if (d != null)
                        CalculateDecoration(d);
                }
            }
            else
            {
                CalculateArc();
            }
        }

        double[] fancyArcLengths;
        double[] fancyArcLengths1;
        double[] fancyArcLengths2;

        private void FinalizeCalc()
        {
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

                FinalizeCalc();
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
                            arcMidAngle = SubStartAngle + arcWidth / 2;
                            arcWidth = 70;
                            break;
                        }
                    case DecorationLocation.Top:
                        {
                            arcRadius = r0 * 1.1;
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
                            arcRadius = r0 * 1.1;
                            arcMidAngle = SubStartAngle + arcWidth * .25f;
                            arcWidth = 50;
                            break;
                        }
                    case DecorationLocation.Center:
                        {
                            arcRadius = r0 * .7;
                            arcMidAngle = SubStartAngle + arcWidth / 2 + 90;
                            arcWidth = 50;
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
