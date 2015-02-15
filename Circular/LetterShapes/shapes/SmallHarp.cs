using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using Circular.Decorations;
using System.Drawing;
using Circular.Words;
using Circular.Decorations.Shapes;

namespace Circular.LetterShapes.Shapes
{
    [Serializable]
    class SmallHarp : aSyllable
    {

          public SmallHarp():base() { }
        private Point startDecoration;
        private Point endDecoration;

        private int DecorationType = 0;
        public SmallHarp(int type)
        {
            DecorationType = type;
        }

        public override aSyllable HandlesEngLetter(engLetter letter, Circular.aCircleObject.ScriptStyles scriptStyle)
        {
            if (letter.Consonant != null && "_th_sh_x_y_ch_".Contains("_" + letter.Consonant + "_"))
            {
                switch (letter.Consonant)
                {
                    case "th":
                        return new SmallHarp(0);
                    case "sh":
                        return new SmallHarp(1);
                    case "x":
                        return new SmallHarp(2);
                    case "y":
                        return new SmallHarp(3);
                    case "ch":
                        return new SmallHarp(4);
                }
                return new SmallHarp();
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
                for (int i = 0; i < StartLines.Length; i++)
                {
                    path.DrawLine(Pens.Black, StartLines[i], EndLines[i]);
                }

                switch (DecorationType)
                {
                    case 1:
                    case 2:
                        path.DrawLine(new Pen(Color.Black,2), startDecoration, endDecoration);
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

        private Point[] StartLines;
        private Point[] EndLines;

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

                StartLines = new Point[4];
                EndLines = new Point[4];
                double a1 = 361 - _mainAngles[0];
                double gapArc = (float)(Math.Abs((_mainAngles[1] + a1) % 360 - (_mainAngles[0] + a1) % 360));

                gapArc /= (StartLines.Length - 1);
                double[] d = new double[EndLines.Length];
                Point e = MathHelps.D2Coords(new Point((int)0, (int)0), _WordParent.Radius, MidAngle);

                for (int i = 0; i < StartLines.Length; i++)
                {
                    a1 = _mainAngles[0] + i * gapArc;
                    StartLines[i] = MathHelps.D2Coords(new Point((int)0, (int)0), _WordParent.Radius, a1);

                    //  d[i] = MathHelps.DistanceFromLine(WordParent.WordCenter, e, StartLines[i]);
                }

                double rTop = _WordParent.Radius - 2 * LetterRadius;

                //  Point p0 = MathHelps.D2Coords(WordParent.WordCenter, rTop, midAngle);

                // double a2 = (midAngle + 90) / 180 * Math.PI;

                for (int i = 0; i < StartLines.Length; i++)
                {
                    a1 = _mainAngles[0] + i * gapArc;
                    EndLines[i] = MathHelps.D2Coords(new Point((int)0, (int)0), rTop, a1);// new Point((int)(p0.X - d[i] * Math.Cos(a2)), (int)(p0.Y - d[i] * Math.Sin(a2)));
                }

                startDecoration = MathHelps.D2Coords(LetterBounds, MidAngle + 180);
                switch (DecorationType)
                {
                    case 1:
                        startDecoration = MathHelps.D2Coords(new Point((int)0, (int)0),_WordParent.Radius*1.2, MidAngle );
                        endDecoration =new Point((int)0, (int)0);
                        break;
                    case 2:
                        startDecoration = MathHelps.D2Coords(LetterBounds, MidAngle + 90);
                        endDecoration = MathHelps.D2Coords(LetterBounds, MidAngle - 90);
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

        public override void DrawLetter(ref Graphics path, ref GraphicsPath border, Color backgroundColor, Color foregroundColor, bool mockup)
        {
            if (!mockup)
                DrawArc(ref path, ref border, backgroundColor,foregroundColor,mockup);

            foreach (var v in Vowels)
                v.DrawVowel(ref path, backgroundColor, foregroundColor, mockup);

            foreach (var l in Lines)
                l.Draw(ref path);

            if (Dots != null)
            {
                foreach (var d in Dots)
                {
                    d.DrawDot(ref path, backgroundColor, foregroundColor, mockup);
                }
            }


        }

        public override void CalcPositions()
        {
            LocationProb prob = this.LocationProb();

            foreach (var v in Vowels)
                if (v != null)
                    v.Location = prob.GetVowelLocation();

            foreach (var d in Decorations)
                if (d != null)
                    d.Location = prob.GetDecLocation();
        }

        protected override LocationProb LocationProb()
        {
            return new LocationProb(
                vAbove: 0,
                vCenter: 0,
                vLeft: 1,
                dAbove: 1,
                dBottom:1,
                dCenter: 1,
                dLeft: 1,
                dRight: 1);
        }

        protected List<DecorationDot> Dots = new List<DecorationDot>();
        protected List<VectorGraphics.Line> Lines = new List<VectorGraphics.Line>();

        protected override void CalculateDecoration(Decorations.aDecoration decoration)
        {
            
        }
    }
}
