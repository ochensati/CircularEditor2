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
    class SmallFullMoon : aSyllable
    {

        public SmallFullMoon() : base() { }
        private Point startDecoration;
        private Point endDecoration;

        private int DecorationType = 0;
        public SmallFullMoon(int type)
        {
            DecorationType = type;
        }

        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft || scriptStyle == Circular.aCircleObject.ScriptStyles.Small)
            {
                if (letter.Consonant != null && "_l_k_c_g_q_v_".Contains("_" + letter.Consonant + "_"))
                {
                    switch (letter.Consonant)
                    {
                        case "l":
                            return new SmallFullMoon(0);
                        case "k":
                            return new SmallFullMoon(1);
                        case "c":
                            return new SmallFullMoon(1);
                        case "g":
                            return new SmallFullMoon(2);
                        case "q":
                            return new SmallFullMoon(3);
                        case "v":
                            return new SmallFullMoon(4);
                    }
                    return new SmallFullMoon();
                }

                return null;

            }
            else
            {
                if (letter.Consonant != null && "_j_k_c_l_m_n_p_".Contains("_" + letter.Consonant + "_"))
                    return new SmallFullMoon();
                else
                    return null;
            }
        }

        protected override void DrawArc(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            try
            {

                //make sure to add error handling here and in the paint event.  Error handling bubbles into the .net graphics classes, which do weird things
                border.AddArc(_WordParent.CircleBounds, StartAngle, ArcWidth);


                Pen p = new Pen(Color.Black, 1);
                path.DrawEllipse(p, LetterBounds);
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


        private double StartFAngle1 = rnd.NextDouble() * 360;



        public override void CalculateArc()
        {

            LetterRadius = maxInnerRadius * .8;
            if (LetterRadius > _WordParent.Radius * .4)
                LetterRadius = _WordParent.Radius * .35;

            if (_Big)
                LetterRadius *= 1.5;

            LetterBounds = MathHelps.BoundingRectangle(BehindLine(_WordParent.Radius - LetterRadius * 1.1), LetterRadius);


            FindEdges();


            switch (DecorationType)
            {
                case 1:
                    startDecoration = MathHelps.D2Coords(LetterBounds, MidAngle + 180);
                    endDecoration = LetterCenter;
                    break;
                case 2:
                    startDecoration = MathHelps.D2Coords(LetterBounds, MidAngle + 90);
                    endDecoration = MathHelps.D2Coords(LetterBounds, MidAngle - 90);
                    break;
                case 3:
                case 4:
                    startDecoration = LetterCenter;
                    endDecoration = MathHelps.D2Coords(_WordParent.CircleBounds, MidAngle);
                    break;
            }
        }

        protected override LocationProb LocationProb()
        {
            return new LocationProb(
                vAbove: 0,
                vCenter: 1,
                vLeft: 1,
                dAbove: 1,
                dBottom: 1,
                dCenter: 1,
                dLeft: .25,
                dRight: .25);
        }

        protected override void CalculateDecoration(Decorations.aDecoration decoration)
        {


        }
    }
}
