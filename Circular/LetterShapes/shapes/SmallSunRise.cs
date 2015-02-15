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
    public class SmallSunrise : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Consonant != null && "_t_d_w_st_s_".Contains("_" + letter.Consonant + "_"))
            {
                switch (letter.Consonant)
                {
                    case "t":
                        return new SmallSunrise(0);
                    case "d":
                        return new SmallSunrise(1);
                    case "w":
                        return new SmallSunrise(2);
                    case "st":
                        return new SmallSunrise(3);
                    case "s":
                        return new SmallSunrise(4);
                }
                return new SmallSunrise();
            }
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
                float arc1 = (float)Math.Abs(_mainAngles[0] - StartAngle);
                float arc2 = (float)Math.Abs(EndAngle - _mainAngles[1]);
                border.AddArc(_WordParent.CircleBounds, StartAngle, arc1);
                border.AddArc(LetterBounds, (float)_subAngles[0], (float)(SubArc));
                border.AddArc(_WordParent.CircleBounds, (float)(_mainAngles[1]), arc2);

                switch (DecorationType)
                {
                    case 1:
                    case 2:
                        path.DrawLine(Pens.Black, startDecoration, endDecoration);
                        break;
                    case 3:
                        path.FillEllipse(Brushes.Black, MathHelps.Circle2Rect(startDecoration, 4, LetterBounds.Width * .1));
                        break;
                    case 4:
                        path.FillEllipse(Brushes.Black, MathHelps.Circle2Rect(startDecoration, 4, LetterBounds.Width * .1));
                        path.FillEllipse(Brushes.Black, MathHelps.Circle2Rect(endDecoration, 4, LetterBounds.Width * .1));
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }


        double firstArc;

        public SmallSunrise():base() { }
        private Point startDecoration;
        private Point endDecoration;

        private int DecorationType = 0;
        public SmallSunrise(int type)
        {
            DecorationType = type;
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

                FinalizeCalc();
        }

        private void FinalizeCalc()
        {
            FindEdges();

            this.SubArc = -1 * Math.Abs(-360 + Math.Abs(this.SubArc));
            firstArc = this.SubArc;

            startDecoration = MathHelps.D2Coords(LetterBounds, MidAngle + 180);
            switch (DecorationType)
            {
                case 1:
                    endDecoration = LetterCenter;
                    break;
                case 2:
                    endDecoration =new Point((int)0, (int)0);
                    break;
                case 3:
                case 4:
                    endDecoration = MathHelps.D2Coords(_WordParent.CircleBounds, MidAngle);
                    break;
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
                dCenter: 1,
                dLeft: 1,
                dRight: 1);
        }

        protected override void CalculateDecoration(Decorations.aDecoration decoration)
        {



        }
    }
}
