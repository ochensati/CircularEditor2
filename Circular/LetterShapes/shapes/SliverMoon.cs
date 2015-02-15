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
    class SliverMoon : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Consonant != null &&  "rzh@f".Contains(letter.Consonant))
                return new SliverMoon();
            else
                return null;
        }

        protected override void DrawArc(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            try
            {
                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things

                if (mockup)
                {
                    path.FillEllipse(new SolidBrush(backgroundColor), LetterBounds);
                }
                else
                {

                   
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


                LetterBounds = MathHelps.BoundingRectangle(BehindLine(_WordParent.Radius - LetterRadius * 1), LetterRadius);

                FindEdges();

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