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
    public class Connector : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.isConnector == true)
                return new Connector();
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
                    float arc1 = (float)Math.Abs(_mainAngles[0] - StartAngle);
                    float arc2 = (float)Math.Abs(EndAngle - _mainAngles[1]);
                    border.AddArc(_WordParent.CircleBounds, StartAngle, arc1);
                    border.AddLine(edges[0], edges[1]);
                    border.AddArc(MathHelps.Circle2Rect(new Point((int)0, (int)0), radius2), StartAngle + arc1, (float)(_mainAngles[1] - (StartAngle + arc1)));
                    border.AddLine(edges[2], edges[3]);
                    border.AddArc(_WordParent.CircleBounds, (float)(_mainAngles[1]), arc2);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }

        public override double PreferredAngle()
        {
            return 2;
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


        Point[] edges = new Point[4];
        double radius2;
        private void FinalizeCalc()
        {
            FindEdges();

            this.SubArc = -1 * Math.Abs(-360 + Math.Abs(this.SubArc));

            float arc1 = (float)Math.Abs(_mainAngles[0] - StartAngle);
            float arc2 = (float)Math.Abs(EndAngle - _mainAngles[1]);
            radius2 =  MathHelps.distance(new Point((int)0, (int)0), LetterCenter) - LetterRadius*.8;
            edges[0] = MathHelps.D2Coords(_WordParent.CircleBounds, StartAngle + arc1);
            edges[1] = MathHelps.D2Coords(new Point((int)0, (int)0),radius2, StartAngle + arc1);
           // edges[1] = MathHelps.D2Coords(LetterCenter, radius2, StartAngle + arc1);
          //  edges[2] = MathHelps.D2Coords(LetterCenter, radius2, _mainAngles[1]);
            edges[2] = MathHelps.D2Coords(new Point((int)0, (int)0), radius2, _mainAngles[1]);
            edges[3] = MathHelps.D2Coords(_WordParent.CircleBounds, _mainAngles[1]);

            radius2 = MathHelps.distance(new Point((int)0, (int)0), edges[1]);
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
