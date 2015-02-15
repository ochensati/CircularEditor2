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
    class VowelOnly : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.isVowel == true)
            {
               // if ("aeiou".Contains(letter.Vowel))
                    return new VowelOnly();
                //else
                //    return null;
            }
            else
                return null;
        }

        protected override void DrawArc(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                border.AddArc(_WordParent.CircleBounds, StartAngle, ArcWidth);

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
                //if (LetterRadius > _WordParent.WordRadius * .4)
                //    LetterRadius = _WordParent.WordRadius * .35;

                if (_Big)
                    LetterRadius *= 1.5;

                LetterBounds = MathHelps.BoundingRectangle(CenterLine, LetterRadius);


                FindEdges();

                this.SubArc = -1 * Math.Abs(-360 + Math.Abs(this.SubArc));

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
                vCenter:0,
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