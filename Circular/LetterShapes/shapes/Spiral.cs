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
    class Spiral : aSyllable
    {
        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (scriptStyle == Circular.aCircleObject.ScriptStyles.Ashcroft)
            {
                if ("_ment_" .Contains("_" + letter.ToString() + "_"))
                    return new Spiral(0);
                if ("_.ize_".Contains("_" + letter.ToString() + "_"))
                    return new Spiral(1);
                if ("_tion_".Contains("_" + letter.ToString() + "_"))
                    return new Spiral(2);
                if ("_er_".Contains("_" + letter.ToString() + "_"))
                    return new Spiral(3);

                 //)ize_tion_er_")

               
                    return null;

            }
            else
            {
                return null;
            }
        }

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
                    border.AddArc(_WordParent.CircleBounds, StartAngle, ArcWidth);

                    Pen p = new Pen(Color.Black, 1);
                    path.DrawSpiral(p, LetterCenter, 0, GraphicsExtensions.Direction.Clockwise,
                        1, MidAngle+90, 1.005,new Point((int)0, (int)0),_WordParent.Radius);
                    //path.DrawEllipse(p, LetterBounds);

                    switch (DecorationType)
                    {
                        case 0:
                            path.FillEllipse(new SolidBrush(Color.White), MathHelps.Circle2Rect(LetterCenter, LetterRadius/6));
                            path.DrawEllipse(new Pen(Color.Black,2), MathHelps.Circle2Rect(LetterCenter, LetterRadius/6));
                            break;
                        case 2:
                            path.FillEllipse(new SolidBrush(Color.Black), MathHelps.Circle2Rect(LetterCenter, LetterRadius / 6));
                            path.DrawEllipse(new Pen(Color.Black, 2), MathHelps.Circle2Rect(LetterCenter, LetterRadius / 6));
                            break;
                        case 3:
                            path.DrawEllipse(new Pen(Color.Black, 2), MathHelps.Circle2Rect(LetterCenter, LetterRadius / 2));
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);

            }
        }


        private double StartFAngle1 = rnd.NextDouble() * 360;

        public override void DrawLetter(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            if (!mockup)
                DrawArc(ref path, ref border, backgroundColor, foregroundColor, mockup);

           

        }

        protected int DecorationType = 0;
        public Spiral(int DecorationType):base()
        {
            this.DecorationType = DecorationType;
        }

        public Spiral()
            : base()
        {
           
        }

        public override void CalculateArc()
        {

            LetterRadius = maxInnerRadius * .8;
            if (LetterRadius > _WordParent.Radius * .4)
                LetterRadius = _WordParent.Radius * .35;

            if (_Big)
                LetterRadius *= 1.5;

            LetterBounds = MathHelps.BoundingRectangle(BehindLine(_WordParent.Radius - LetterRadius * 1.1), LetterRadius);


            FindEdges();


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
