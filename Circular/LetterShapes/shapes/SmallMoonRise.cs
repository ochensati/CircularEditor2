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
    class SmallMoonRise : aSyllable
    {

          public SmallMoonRise():base() { }
        private Point startDecoration;
        private Point endDecoration;

        private int DecorationType = 0;
        public SmallMoonRise(int type)
        {
            DecorationType = type;
        }


        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft || scriptStyle == Circular.aCircleObject.ScriptStyles.Small)
            {
                if (letter.Consonant != null && "_m_n_j_p_b_".Contains("_" + letter.Consonant + "_"))
                {
                    switch (letter.Consonant)
                    {
                        case "m":
                            return new SmallMoonRise(0);
                        case "n":
                            return new SmallMoonRise(1);
                        case "j":
                            return new SmallMoonRise(2);
                        case "p":
                            return new SmallMoonRise(3);
                        case "b":
                            return new SmallMoonRise(4);
                    }
                    return new SmallMoonRise();
                }
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
                            path.FillEllipse(Brushes.Black, MathHelps.Circle2Rect(endDecoration,4, LetterBounds.Width*.1));
                            break;
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

                SubArc = (float)Math.Abs(_subAngles[1] - _subAngles[0]) - 360;

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
        }
    }
}