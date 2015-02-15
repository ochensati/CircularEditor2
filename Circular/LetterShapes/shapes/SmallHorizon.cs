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
    public class SmallHorizon : aSyllable
    {

          public SmallHorizon():base() { }
        private Point startDecoration;
        private Point endDecoration;

        private int DecorationType = 0;
        public SmallHorizon(int type)
        {
            DecorationType = type;
        }


        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Consonant != null && "_t_d_w_st_s_".Contains("_" + letter.Consonant + "_"))
            {
                switch (letter.Consonant)
                {
                    case "t":
                        return new SmallHorizon(0);
                    case "d":
                        return new SmallHorizon(1);
                    case "w":
                        return new SmallHorizon(2);
                    case "st":
                        return new SmallHorizon(3);
                    case "s":
                        return new SmallHorizon(4);
                }
                return new SmallHorizon();
            }
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

             

                    border.AddArc(_WordParent.CircleBounds, StartAngle, ArcWidth);

                  
                        path.DrawArc(Pens.Black, LetterBounds, (float)_subAngles[0], (float)(SubArc));
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
           
        }
    }
}
