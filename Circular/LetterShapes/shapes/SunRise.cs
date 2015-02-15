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
    public class Sunrise : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Consonant != null && "_t_d_w_st_s_".Contains("_" + letter.Consonant + "_"))
                return new Sunrise();
            else
                return null;
        }

        public override double PreferredAngle()
        {
            return .6;
        }

        protected override void DrawArc(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
          
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

                    GraphicsPath Sun = new GraphicsPath();
                    Sun.StartFigure();
                    Sun.AddArc(_WordParent.CircleBounds, (float)_mainAngles[1], -1 * (float)Math.Abs(_mainAngles[1] - _mainAngles[0]));
                    Sun.AddArc(LetterBounds, (float)_subAngles[0], (float)(SubArc));
                    Sun.CloseFigure();

                    path.FillPath(new SolidBrush(foregroundColor), Sun);

                    if (_Fancy)
                    {
                        Rectangle VowelBounds2 = new Rectangle( LetterBounds.X,LetterBounds.Y,LetterBounds.Width,LetterBounds.Height);
                        VowelBounds2.Inflate((int)(LetterBounds.Width / -4d), (int)(LetterBounds.Width / -4d));

                        path.DrawArc(new Pen(backgroundColor, 2), VowelBounds2, (float)_subAngles[0], (float)(SubArc));

                        VowelBounds2.Inflate(-3, -3);

                        path.DrawArc(new Pen(backgroundColor, 2), VowelBounds2, (float)_subAngles[0], (float)(SubArc));
                       
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }


        double firstArc;


        public override void UseWordForArc(aCircleObject otherWord)
        {

            LetterRadius = otherWord.Radius + 20;
            var p = new Point(otherWord.DrawCenter.X - this._WordParent.DrawCenter.X, otherWord.DrawCenter.Y - this._WordParent.DrawCenter.Y);
            double r2 = Math.Sqrt(p.X * p.X + p.Y * p.Y);
            if (r2 < (LetterRadius + _WordParent.Radius))
            {

                double angle = MathHelps.Atan2(p.Y, p.X) - this._WordParent.CircleAngle;
                LetterBounds = MathHelps.Circle2Rect(MathHelps.D2Coords(new Point((int)0, (int)0), r2, angle), LetterRadius);

                FindEdges();
                firstArc = this.SubArc;
                this.SubArc = -1 * Math.Abs(-360 + Math.Abs(this.SubArc));

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

        public override void CalculateArc()
        {
          
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                LetterRadius = maxInnerRadius * .8;
                //if (LetterRadius > _WordParent.WordRadius * .4)
                //    LetterRadius = _WordParent.WordRadius * .35;

                if (_Big)
                    LetterRadius *= 1.5;

                LetterBounds = MathHelps.BoundingRectangle(CenterLine, LetterRadius);


                FindEdges();
                firstArc = this.SubArc;
                this.SubArc = -1 * Math.Abs(-360 + Math.Abs(this.SubArc));

        }

        protected override LocationProb LocationProb()
        {
            return new LocationProb(
                vAbove: 1,
                vCenter: 0,
                vLeft: 1,
                dAbove: 1,
                dBottom: 1,
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


            if ( decoration.GetType() == typeof(Circular.Decorations.Shapes.Rings))
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
                            arcRadius = r0 * .8;
                            arcMidAngle = SubStartAngle + arcWidth * .7f;

                            arcWidth = 50;
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
                            arcMidAngle = SubStartAngle + arcWidth * .65f;
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
                            arcRadius = r0 * 1;
                            arcMidAngle = SubStartAngle + arcWidth / 2;
                            arcWidth = 0;
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
